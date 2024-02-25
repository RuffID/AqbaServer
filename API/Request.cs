using AqbaServer.Models.OkdeskEntities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using AqbaServer.Helper;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.API
{
    public static class Request
    {
        static readonly SocketsHttpHandler socketsHandler = new()
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(30),
        };
        static readonly HttpClient httpClient = new(socketsHandler);

        static readonly HttpClientHandler handler;  // здесь хранятся куки
        static readonly CookieContainer cookie;

        static Request()    // Конструктор класса для создания контейнера под куки при инициализации экземпляра класса
        {
            cookie = new CookieContainer();
            handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None,
                CookieContainer = cookie,
            };
        }

        public static async Task<Company[]?> GetСompanies(int lastCompanyId, long companyCategoryId)
        {
            string link = $"{Immutable.OkdeskApiLink}/companies/list?api_token={Config.OkdeskApiToken}&page[from_id]={lastCompanyId}&page[direction]=forward&category_ids[]={companyCategoryId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {

                return JsonConvert.DeserializeObject<Company[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<MaintenanceEntity[]?> GetMaintenanceEntities(int lastMaintenanceEntitiesId)
        {
            string link = $"{Immutable.OkdeskApiLink}/maintenance_entities/list?api_token={Config.OkdeskApiToken}&page[from_id]={lastMaintenanceEntitiesId}&page[direction]=forward";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {

                return JsonConvert.DeserializeObject<MaintenanceEntity[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Equipment[]?> GetEquipments(int lastEquipmentId, int pageSize = 100)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/list/?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastEquipmentId}&page[size]={pageSize}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                if (response == null || response == string.Empty)
                    return null;
                return JsonConvert.DeserializeObject<Equipment[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Manufacturer[]?> GetManufacturers(long lastManufacturerId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/manufacturers/?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastManufacturerId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                if (response == null || response == string.Empty)
                    return null;
                return JsonConvert.DeserializeObject<Manufacturer[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Kind[]?> GetKinds(long lastKindId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/kinds/?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastKindId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                if (response == null || response == string.Empty)
                    return null;
                return JsonConvert.DeserializeObject<Kind[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Model[]?> GetModels(long lastModelId)
        {
            string link = $"{Immutable.OkdeskApiLink}/equipments/models/?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastModelId}";
            var response = await SendApiRequest(link);
            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                if (response == null || response == string.Empty)
                    return null;
                return JsonConvert.DeserializeObject<Model[]>(response);
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }

        }

        public static async Task<Employee[]?> GetEmployees(long lastEmployeeId)  // Получение списка сотрудников
        {
            string link = $"{Immutable.OkdeskApiLink}/employees/list/?api_token={Config.OkdeskApiToken}&page[direction]=forward&page[from_id]={lastEmployeeId}";
            Employee[]? employees = Array.Empty<Employee>();
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                employees = JsonConvert.DeserializeObject<Employee[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (employees != null && employees.Length > 0)
                return employees;

            return null;

        }

        /*public static async Task<bool> GetReportSolvedTasks(List<Employee> employees, DateTime dateFrom, DateTime dateTo)    // Получение списка открытых заявок
        {
            StringBuilder linkSolvedTasks = new();
            int[]? numberOfSolvedTask;

            foreach (var employee in employees)
            {
                linkSolvedTasks.Clear();
                linkSolvedTasks.Append($"{Immutable.OkdeskApiLink}/issues/count?api_token={Config.OkdeskApiToken}&assignee_ids[]={employee.Id}&completed_since={dateFrom:dd-MM-yyyy} 00:00&completed_until={dateTo:dd-MM-yyyy} 23:59");
                var responseSolved = await GetResponse(linkSolvedTasks.ToString());
                if (string.IsNullOrEmpty(responseSolved) || responseSolved == "[]")
                {
                    try
                    {
                        numberOfSolvedTask = JsonConvert.DeserializeObject<int[]>(responseSolved);
                    }
                    catch (Exception e) { WriteLog.Error(e.ToString()); return false; }

                    if (numberOfSolvedTask?.Length == 0 || numberOfSolvedTask?.Length == null)
                        employee.SolvedTasks = 0;
                    else
                    {
                        employee.SolvedTasks = numberOfSolvedTask.Length;
                        employee.IsSelected = true;
                    }

                    numberOfSolvedTask = null;
                }
                else
                    employee.SolvedTasks = 0;
            }
            return true;
        }

        public static async Task GetReportOpenTasks(ICollection<Employee> employees, ICollection<Status> statuses, ICollection<TaskType> types)    // Получение списка открытых заявок
        {
            StringBuilder linkOpenTasks = new();
            int[]? numberOfTasks = null;

            foreach (var employee in employees)
            {
                linkOpenTasks.Clear();
                linkOpenTasks.Append($"{Immutable.OkdeskApiLink}/issues/count?api_token={Config.OkdeskApiToken}");

                foreach (var status in statuses)
                    if (status.IsChecked == true)
                        linkOpenTasks.Append($"&status[]={status.Code}");

                foreach (var type in types)
                    if (type.IsChecked == true)
                        linkOpenTasks.Append($"&type[]={type.Code}");

                linkOpenTasks.Append($"&assignee_ids[]={employee.Id}");

                var responseOpen = await GetResponse(linkOpenTasks.ToString());
                if (!string.IsNullOrEmpty(responseOpen) || responseOpen != "[]")
                {
                    try
                    {
                        numberOfTasks = JsonConvert.DeserializeObject<int[]>(responseOpen);
                    }
                    catch (Exception e) { WriteLog.Error(e.ToString()); }

                    if (numberOfTasks?.Length == 0 || numberOfTasks?.Length == null)
                        employee.OpenTasks = 0;
                    else
                    {
                        employee.OpenTasks = numberOfTasks.Length;
                        employee.IsSelected = true;
                    }

                    numberOfTasks = null;
                }
                else
                    employee.OpenTasks = 0;
            }
        }*/

        public static async Task<Group[]?> GetGroups()
        {
            string link = Immutable.OkdeskApiLink + "/employees/groups?api_token=" + Config.OkdeskApiToken;
            Group[]? tempGroups = Array.Empty<Group>();
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                tempGroups = JsonConvert.DeserializeObject<Group[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (tempGroups != null && tempGroups.Length > 0)
                return tempGroups;

            return null;

        }

        public static async Task<Role[]?> GetRoles()
        {
            string link = Immutable.OkdeskApiLink + "/employees/roles/?api_token=" + Config.OkdeskApiToken;
            Role[]? roles = Array.Empty<Role>();
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;
            try
            {
                roles = JsonConvert.DeserializeObject<Role[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (roles != null && roles.Length > 0)
                return roles;

            return null;

        }

        public static async Task<TaskType[]?> GetTypes()
        {
            string link = Immutable.OkdeskApiLink + "/dictionaries/issues/types?api_token=" + Config.OkdeskApiToken;
            TaskType[]? types = Array.Empty<TaskType>();
            List<TaskType> list;
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                types = JsonConvert.DeserializeObject<TaskType[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (types != null && types.Length > 0)
            {
                list = types.ToList();
                // Цикл вытаскивает все типы из "папок" в общий список
                if (types?.Length > 0)
                {
                    foreach (var type in types)
                    {
                        if (type?.Children?.Length > 0)
                        {
                            foreach (var child in type.Children)
                            {
                                list.Add(new(child));
                            }
                            list.Remove(type);
                        }
                    }
                }
                return list.ToArray();
            }
            return null;

        }

        public static async Task<Status[]?> GetStatuses()
        {
            string link = Immutable.OkdeskApiLink + "/issues/statuses/?api_token=" + Config.OkdeskApiToken;
            Status[]? statuses = Array.Empty<Status>();
            var response = await SendApiRequest(link);

            if (string.IsNullOrEmpty(response) || response == "[]")
                return null;

            try
            {
                statuses = JsonConvert.DeserializeObject<Status[]>(response);
            }
            catch (Exception e) { WriteLog.Error(e.ToString()); }

            if (statuses != null && statuses.Length > 0)
                return statuses;

            return null;

        }

        public static async Task GetTime(List<Employee> employees, DateTime dateFrom, DateTime dateTo)
        {
            foreach (var employee in employees)
            {
                employee.SpentedTimeDouble = 0;
            }

            string linkFirstPage = $"{Config.OkdeskDomainLink}/reports/employee_time_entries?reports[logged_at_from]={dateFrom:dd.MM.yyyy}&reports[logged_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=time_desc";

            string linkAnotherPage;
            var reportPage = await SendGetRequest(linkFirstPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.JSON_GET_ACCEPT_HEADER);
            ReportDataHandler.GetEmployeesResults(reportPage, employees);


            for (int i = 2; ;)
            {

                if (i <= ReportDataHandler.GetPagesCountTimePerReportPage())    // Парсит страницу со списанным временем
                {
                    linkAnotherPage = $"{Config.OkdeskDomainLink}/reports/employee_time_entries?page={i}&reports[logged_at_from]={dateFrom:dd.MM.yyyy}&reports[logged_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=time_desc";

                    var response = await SendGetRequest(linkAnotherPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.JSON_GET_ACCEPT_HEADER);
                    ReportDataHandler.GetEmployeesResults(response, employees);
                }

                // Если оба инетатора больше чем количество страниц, то завершает цикл
                if (i > ReportDataHandler.GetPagesCountTimePerReportPage())
                    break;
                
                i++;
            }
        }

        public static async Task GetTasks(List<Employee> Employees, DateTime dateFrom, DateTime dateTo)
        {
            foreach (var employee in Employees)
            {
                employee.SolvedTasks = 0;
            }
            string linkFirstPage = $"{Config.OkdeskDomainLink}/reports/assignee?reports[completed_at_from]={dateFrom:dd.MM.yyyy}&reports[completed_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=completed-count_desc";

            string linkAnotherPage;
            var reportPage = await SendGetRequest(linkFirstPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.TEXT_JS_GET_ACCEPT_HEADER);
            ReportDataHandler.GetEmployeesResults(reportPage, Employees);


            for (int i = 2; ;)
            {

                if (i <= ReportDataHandler.GetPagesCountTaskPerReportPage())    // Парсит страницу с решёнными заявками
                {
                    linkAnotherPage = $"{Config.OkdeskDomainLink}/reports/assignee?page={i}&reports[completed_at_from]={dateFrom:dd.MM.yyyy}&reports[completed_at_to]={dateTo:dd.MM.yyyy}&reports[sort_by]=completed-count_desc";

                    var response = await SendGetRequest(linkAnotherPage, $"{Config.OkdeskDomainLink}/reports/assignee", Immutable.TEXT_JS_GET_ACCEPT_HEADER);
                    ReportDataHandler.GetEmployeesResults(response, Employees);
                }

                // Если оба инетатора больше чем количество страниц, то завершает цикл
                if (i > ReportDataHandler.GetPagesCountTaskPerReportPage())
                    break;

                i++;
            }
        }

        public static async Task<bool> Login(string loginReferer, string content)
        {

            using var client = new HttpClient(handler, false);
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.58");
            client.DefaultRequestHeaders.Referrer = new Uri(loginReferer);
            client.DefaultRequestHeaders.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Microsoft Edge\";v=\"114");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            try
            {
                using var resp = await client.PostAsync(loginReferer, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
                IEnumerable<string> str = resp.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

                if (resp.StatusCode == HttpStatusCode.Found && str.Any(str => str.Contains("remember_user_token")))
                {
                    WriteLog.Info($"Successfull login in service: {loginReferer}");
                    return true;
                }
                else
                {
                    WriteLog.Warn($"Failed to log in to the service using the link: {loginReferer}");
                    return false;
                }
            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return false;
            }
        }

        public static async Task<string?> SendGetRequest(string getReferer, string siteReferer, string acceptHeader)
        {
            using var client = new HttpClient(handler, false);
            client.DefaultRequestHeaders.Add("Accept", $"{acceptHeader}, */*; q=0.01");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Referrer = new Uri(siteReferer);
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");

            try
            {
                using var resp = await client.GetAsync(getReferer);
                await Task.Delay(200);
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsStringAsync();
                else
                {
                    WriteLog.Warn($"Error when retrieving a get page using a link request: {siteReferer}");
                    return null;
                }
            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return null;
            }
        }

        public static async Task<string?> SendApiRequest(string? link)
        {
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(link);
                await Task.Delay(400);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return null;
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return null;
            }
        }
    }
}