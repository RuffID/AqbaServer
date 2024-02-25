using AqbaServer.Authorization;
using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Interfaces.Authorization;
using AqbaServer.Models.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace AqbaServer.Controllers.Authorization
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class userController : Controller
    {
        private readonly IMapper _mapper;        
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _roleRepository;

        public userController(IMapper mapper, IUserRepository userRepository, IUserRoleRepository roleRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet("list"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsers();

            if (users == null || users.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpGet, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUser([FromQuery] string userEmail)
        {
            var user = await _userRepository.GetUser(userEmail);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        
        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Token))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest authenticateRequest)
        {
            if (authenticateRequest == null)
                return BadRequest(ModelState);

            User? user = await _userRepository.GetUser(authenticateRequest.Email);

            if (user == null || user.Active == false || user.Password == null || !PasswordHasher.Verify(authenticateRequest.Password, user.Password))
            {
                ModelState.AddModelError("", "Неверный логин или пароль. Не исключено что пользователь заблокирован..");
                return StatusCode(401, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Token token = new()
            {
                AccessToken = _userRepository.GenerateToken(user),
                RefreshToken = _userRepository.GenerateRefreshToken()
            };

            if (!await _userRepository.UpdateRefreshToken(user.Id, token.RefreshToken, DateTime.UtcNow.AddDays(Config.RefreshTokenLifeTimeFromDays)))
            {
                ModelState.AddModelError("", "Не удалось сохранить refreshToken в БД");
                return StatusCode(500, ModelState);
            }

            return Ok(token);
        }

        [HttpPut("refresh"), AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Token))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Refresh([FromQuery] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                ModelState.AddModelError("", "Не передан токен обновления");
                return BadRequest(ModelState);
            }

            User? user = await _userRepository.GetUserByRefreshToken(refreshToken);

            if (user == null || user.Active == false)
            {
                ModelState.AddModelError("", "Пользователь с данным токеном не найден либо неактивен");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.TokenExpires < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Токен обновления истёк");
                return BadRequest(ModelState);
            }

            Token token = new()
            {
                AccessToken = _userRepository.GenerateToken(user),
                RefreshToken = _userRepository.GenerateRefreshToken()
            };

            DateTime refreshTokenLifeTime = DateTime.UtcNow.AddDays(Config.RefreshTokenLifeTimeFromDays);

            if (!await _userRepository.UpdateRefreshToken(user.Id, token.RefreshToken, refreshTokenLifeTime))
            {
                ModelState.AddModelError("", "Не удалось сохранить refreshToken в БД");
                return StatusCode(500, ModelState);
            }

            return Ok(token);
        }

        [HttpGet("restorePassword"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RestorePassword([FromQuery] string email)
        {
            //TODO сначала отправлять код подтверждения, а только потом сбрасывать новый пароль
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Не передан email");
                return BadRequest(ModelState);
            }

            User? user = await _userRepository.GetUser(email);

            if (user == null || string.IsNullOrEmpty(user.Email) || user.Active == false)
            {
                ModelState.AddModelError("", "Пользователь с данным email не найден либо неактивен");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string newPassword = RandomString.GetString(length: 12);

            MailMessage? mail = SMTP.CreateMail(name: "Аквабайт", Config.SMTPEmail, user.Email, "Новый пароль от сервиса", $"Пароль: {newPassword}");

            if (!SMTP.SendMail("smtp.gmail.com", 587, Config.SMTPEmail, Config.SMTPPAssword, mail))
            {
                ModelState.AddModelError("", "Ошибка при отправке пароля на почту");
                return StatusCode(500, ModelState);
            }

            int? roleId = null;
            if (!string.IsNullOrEmpty( user.Role ))
                roleId = await _roleRepository.GetUserRole(user.Role);
            if (roleId == null)
            {
                ModelState.AddModelError("", "Ошибка при получении роли");
                return StatusCode(500, ModelState);
            }

            user.Role = roleId.ToString();
            user.Password = PasswordHasher.Hash( newPassword );         

            if (!await _userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", $"Не удалось обновить пользователя {user.Email} в БД");
                return StatusCode(500, ModelState);
            }

            return Ok("Новый пароль успешно отправлен на почту");
        }


        [HttpPost("registration"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registration([FromBody] UserDto userCreate)
        {
            if (userCreate == null || string.IsNullOrEmpty(userCreate.Email) || string.IsNullOrEmpty(userCreate.Role) || string.IsNullOrEmpty(userCreate.Password))
                return BadRequest(ModelState);

            User? user = await _userRepository.GetUser(userCreate.Email);

            if (user != null)
            {
                ModelState.AddModelError("", $"Пользователь {userCreate.Email} уже существует.");
                return StatusCode(422, ModelState);
            }

            int? roleId = await _roleRepository.GetUserRole(userCreate.Role);
            if (roleId == null)
            {
                ModelState.AddModelError("", "Роль не найдена.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            userMap.Password = PasswordHasher.Hash(userCreate.Password);
            userMap.Role = roleId.ToString();

            if (!await _userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving user");
                return StatusCode(500, ModelState);
            }

            return Ok($"Пользователь {userCreate.Email} успешно создан.");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null || string.IsNullOrEmpty(updatedUser.Email) || string.IsNullOrEmpty(updatedUser.Role) || string.IsNullOrEmpty(updatedUser.Password))
            {
                ModelState.AddModelError("", "Не передан пользователь для обновления");
                return BadRequest(ModelState);
            }               

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User? user = await _userRepository.GetUser(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return NotFound(ModelState);
            }

            int? roleId = await _roleRepository.GetUserRole(updatedUser.Role);
            if (roleId == null)
            {
                ModelState.AddModelError("", "Роль не найдена.");
                return StatusCode(422, ModelState);
            }

            updatedUser.Password = PasswordHasher.Hash(updatedUser.Password);
            updatedUser.Role = roleId.ToString();
            
            var userMap = _mapper.Map<User>(updatedUser);
            userMap.Id = user.Id;

            if (!await _userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser([FromQuery] string userEmail)
        {
            User? user = await _userRepository.GetUser(userEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _userRepository.DeleteUser(user.Id))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
