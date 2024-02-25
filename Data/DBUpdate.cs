using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskEntities;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;

namespace AqbaServer.Data
{
    public class DBUpdate
    {
        public static async Task<bool> UpdateCategory(int categoryId, Category category)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE company_category " +
                    "SET id = @id, color = @color " +
                    "WHERE id = @categoryId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = category.Id;
                cmd.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = categoryId;
                cmd.Parameters.Add("@color", MySqlDbType.String).Value = category.Color;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateCompany(int companyId, Company company)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE company " +
                    "SET id = @id, name = @name, additional_name = @additional_name, active = @active, categoryId = @categoryId " +
                    "WHERE id = @companyId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = company.Id;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = companyId;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = company.Name;
                cmd.Parameters.Add("@additional_name", MySqlDbType.String).Value = company.AdditionalName;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = company.Active;
                cmd.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = company?.Category?.Id;
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity maintenanceEntity)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE maintenance_entity " +
                    "SET id = @id, name = @name, address = @address, companyId = @companyId " +
                    "WHERE id = @maintenanceEntityId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = maintenanceEntity.Id;
                cmd.Parameters.Add("@maintenanceEntityId", MySqlDbType.Int32).Value = maintenanceEntityId;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = maintenanceEntity?.Name;
                cmd.Parameters.Add("@address", MySqlDbType.String).Value = maintenanceEntity?.Address;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = maintenanceEntity?.Company_Id;
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateManufacturer(string manufacturerCode, Manufacturer manufacturer)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE manufacturer " +
                    "SET code = @code, name = @name, description = @description, visible = @visible " +
                    "WHERE code = @manufacturerCode";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@manufacturerCode", MySqlDbType.String).Value = manufacturerCode;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = manufacturer.Code;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = manufacturer.Name;
                cmd.Parameters.Add("@description", MySqlDbType.String).Value = manufacturer.Description;
                cmd.Parameters.Add("@visible", MySqlDbType.Bit).Value = manufacturer.Visible;

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateKind(string kindCode, Kind kind)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE kind " +
                    "SET code = @code, name = @name, description = @description, visible = @visible " +
                    "WHERE code = @kindCode";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@kindCode", MySqlDbType.String).Value = kindCode;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = kind.Code;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = kind.Name;
                cmd.Parameters.Add("@description", MySqlDbType.String).Value = kind.Description;
                cmd.Parameters.Add("@visible", MySqlDbType.Bit).Value = kind.Visible;
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateKindParameter(string kindParameterCode, KindParameter parameter)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE kinds_parameters " +
                    "SET code = @code, name = @name, fieldType = @fieldType " +
                    "WHERE code = @kindParameterCode";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@kindParameterCode", MySqlDbType.String).Value = kindParameterCode;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = parameter.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = parameter.Code;
                cmd.Parameters.Add("@fieldType", MySqlDbType.String).Value = parameter.FieldType;

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateCoordinate(int maintenanceEntityId, decimal[] coord)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"UPDATE coordinates SET N = {coord[0]}, E = {coord[1]} WHERE maintenanceEntitiesId = {maintenanceEntityId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.ExecuteReader();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateModel(string modelCode, Model model)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE model " +
                    "SET code = @code, name = @name, description = @description, visible = @visible, kindId = @kindId, manufacturerId = @manufacturerId " +
                    "WHERE code = @modelCode";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@code", MySqlDbType.String).Value = model?.Code;
                cmd.Parameters.Add("@modelCode", MySqlDbType.String).Value = modelCode;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = model?.Name;
                cmd.Parameters.Add("@description", MySqlDbType.String).Value = model?.Description;
                cmd.Parameters.Add("@visible", MySqlDbType.Bit).Value = model?.Visible;
                cmd.Parameters.Add("@kindId", MySqlDbType.Int32).Value = model?.EquipmentKind?.Id;
                cmd.Parameters.Add("@manufacturerId", MySqlDbType.Int32).Value = model?.EquipmentManufacturer?.Id;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter parameter)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE parameter " +
                    "SET equipmentId = @equipmentId, kindParameterId = @kindParameterId, value = @value " +
                    "WHERE id  = @equipmentParameterId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@equipmentId", MySqlDbType.Int32).Value = parameter.Equipment.Id;
                cmd.Parameters.Add("@equipmentParameterId", MySqlDbType.Int32).Value = equipmentParameterId;
                cmd.Parameters.Add("@kindParameterId", MySqlDbType.Int32).Value = parameter.KindParam.Id;
                cmd.Parameters.Add("@value", MySqlDbType.String).Value = parameter?.Value;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEquipment(int equipmentId, Equipment equipment)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE equipment " +
                    "SET id = @id, serial_number = @serial_number, inventory_number = @inventory_number, kindId = @kindId, manufacturerId = @manufacturerId, modelId = @modelId, companyId = @companyId, maintenanceEntitiesId = @maintenanceEntitiesId " +
                    "WHERE id = @eqipmentId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = equipment.Id;
                cmd.Parameters.Add("@eqipmentId", MySqlDbType.Int32).Value = equipmentId;
                cmd.Parameters.Add("@serial_number", MySqlDbType.String).Value = equipment?.Serial_number;
                cmd.Parameters.Add("@inventory_number", MySqlDbType.String).Value = equipment?.Inventory_number;
                cmd.Parameters.Add("@kindId", MySqlDbType.Int32).Value = equipment?.Equipment_kind?.Id;
                cmd.Parameters.Add("@manufacturerId", MySqlDbType.Int32).Value = equipment?.Equipment_manufacturer?.Id;
                cmd.Parameters.Add("@modelId", MySqlDbType.Int32).Value = equipment?.Equipment_model?.Id;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = equipment?.Company?.Id;
                cmd.Parameters.Add("@maintenanceEntitiesId", MySqlDbType.Int32).Value = equipment?.Maintenance_entity?.Id;

                /*if (equipment != null && equipment.Parameters != null && equipment.Parameters.Count > 0)
                {
                    foreach (var param in equipment.Parameters)
                    {
                        param.Equipment = equipment;
                        param.KindParam = equipment.Equipment_kind.Parameters;
                        await UpdateEquipmentParameter(equipmentId, param);
                    }
                } */               

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEmployee(int employeeId, Employee employee)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE employee " +
                    "SET id = @id, first_name = @first_name, last_name = @last_name, patronymic = @patronymic, position = @position, active = @active, email = @email, login = @login, phone = @phone " +
                    "WHERE id = @employeeId";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = employee.Id;
                cmd.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;
                cmd.Parameters.Add("@first_name", MySqlDbType.String).Value = employee?.First_name;
                cmd.Parameters.Add("@last_name", MySqlDbType.String).Value = employee?.Last_name;
                cmd.Parameters.Add("@patronymic", MySqlDbType.String).Value = employee?.Patronymic;
                cmd.Parameters.Add("@position", MySqlDbType.String).Value = employee?.Position;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = employee?.Active;
                cmd.Parameters.Add("@email", MySqlDbType.String).Value = employee?.Email;
                cmd.Parameters.Add("@login", MySqlDbType.String).Value = employee?.Login;
                cmd.Parameters.Add("@phone", MySqlDbType.String).Value = employee?.Phone;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateGroup(int groupId, Group group)
        {
            if (group == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE `group` " +
                    "SET id = @id, name = @name, active = @active, description = @description " +
                    "WHERE id = @groupId";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = group.Id;
                cmd.Parameters.Add("@groupId", MySqlDbType.Int32).Value = groupId;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = group?.Name;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = group?.Active;
                cmd.Parameters.Add("@description", MySqlDbType.String).Value = group?.Description;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateRole(string roleName, Role role)
        {
            if (role == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE role " +
                    "SET name = @name " +
                    "WHERE name = @roleName";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@roleName", MySqlDbType.String).Value = roleName;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = role?.Name;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEmployeePerformance(Employee employee, DateTime day)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE employee_performance " +
                    "SET solvedTasks = @solvedTasks, spentedTime = @spentedTime " +
                    "WHERE date = @date AND employeeId = @employeeId";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@date", MySqlDbType.Date).Value = day;
                cmd.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employee?.Id;

                if (employee?.SolvedTasks > 0)
                    cmd.Parameters.Add("@solvedTasks", MySqlDbType.Int32).Value = employee?.SolvedTasks;
                else
                    cmd.Parameters.Add("@solvedTasks", MySqlDbType.Int32).Value = null;

                if (employee?.SpentedTimeDouble > 0)
                    cmd.Parameters.Add("@spentedTime", MySqlDbType.Double).Value = employee?.SpentedTimeDouble;
                else
                    cmd.Parameters.Add("@spentedTime", MySqlDbType.Double).Value = null;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateRefreshToken(int userId, string refreshToken, DateTime expirationRefreshToken)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE user " +
                    "SET refreshToken = @refreshToken, expirationRefreshToken = @expirationRefreshToken " +
                    "WHERE id = @userId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
                cmd.Parameters.Add("@expirationRefreshToken", MySqlDbType.DateTime).Value = expirationRefreshToken;
                cmd.Parameters.Add("@refreshToken", MySqlDbType.String).Value = refreshToken;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateUser(User userUpdate)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE user " +
                    "SET email = @email, roleId = @roleId, passwordHash = @passwordHash, refreshToken = @refreshToken, expirationRefreshToken = @expirationRefreshToken, active = @active " +
                    "WHERE id = @userId";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userUpdate.Id;
                cmd.Parameters.Add("@email", MySqlDbType.String).Value = userUpdate.Email;
                cmd.Parameters.Add("@roleId", MySqlDbType.Int32).Value = Convert.ToInt32( userUpdate.Role );

                if (string.IsNullOrEmpty(userUpdate.Password))
                    cmd.Parameters.Add("@passwordHash", MySqlDbType.String).Value = null;
                else
                    cmd.Parameters.Add("@passwordHash", MySqlDbType.String).Value = userUpdate.Password;

                if (userUpdate.TokenExpires < DateTime.UtcNow)
                    cmd.Parameters.Add("@expirationRefreshToken", MySqlDbType.DateTime).Value = null;
                else
                    cmd.Parameters.Add("@expirationRefreshToken", MySqlDbType.DateTime).Value = userUpdate.TokenExpires;

                if (string.IsNullOrEmpty(userUpdate.RefreshToken))
                    cmd.Parameters.Add("@refreshToken", MySqlDbType.String).Value = null;
                else
                    cmd.Parameters.Add("@refreshToken", MySqlDbType.String).Value = userUpdate.RefreshToken;

                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = Convert.ToBoolean( userUpdate.Active );

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }
}
