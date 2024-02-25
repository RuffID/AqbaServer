using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskEntities;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;

namespace AqbaServer.Data
{
    public class DBInsert
    {
        public static async Task<bool> InsertCategory(Category category)
        {
            if (category == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();

                string sqlCommand = $"INSERT company_category (id, color) VALUES (@id, @color)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = category.Id;
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

        public static async Task<bool> InsertCompany(int categoryId, Company company)
        {
            if (company == null || categoryId < 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "INSERT company (id, name, additional_name, active, categoryId)  VALUES (@id, @name, @additional_name, @active, @categoryId)";
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = company.Id;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = company.Name;
                cmd.Parameters.Add("@additional_name", MySqlDbType.String).Value = company.AdditionalName;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = company.Active;
                cmd.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = categoryId;
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

        public static async Task<bool> InsertMaintenanceEntity(MaintenanceEntity maintenanceEntity)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "INSERT maintenance_entity (id, name, address, companyId)  VALUES (@id, @name, @address, @companyId)";
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = maintenanceEntity.Id;
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

        public static async Task<bool> InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT manufacturer (code, name, description, visible) " +
                    "VALUES (@code, @name, @description, @visible)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;
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

        public static async Task<bool> InsertKind(Kind kind)
        {
            if (kind == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT kind (code, name, description, visible) VALUES (@code, @name, @description, @visible)";
                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

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

        public static async Task<bool> InsertKindParameter(KindParameter kindParameter)
        {
            if (kindParameter == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommandParam = $"INSERT kinds_parameters (name, code, fieldType) VALUES (@name, @code, @fieldType)";
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommandParam;

                cmd.Parameters.Add("@name", MySqlDbType.String).Value = kindParameter.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = kindParameter.Code;
                cmd.Parameters.Add("@fieldType", MySqlDbType.String).Value = kindParameter.FieldType;

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

        public static async Task<bool> InsertModel(Model model)
        {
            if (model == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT model (code, name, description, visible, kindId, manufacturerId) " +
                    "VALUES (@code, @name, @description, @visible, @kindId, @manufacturerId)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@code", MySqlDbType.String).Value = model?.Code;
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

        public static async Task<bool> InsertEquipment(Equipment equipment)
        {
            if (equipment == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT equipment (id, serial_number, inventory_number, kindId, manufacturerId, modelId, companyId, maintenanceEntitiesId) " +
                    "VALUES (@id, @serial_number, @inventory_number, @kindId, @manufacturerId, @modelId, @companyId, @maintenanceEntitiesId)";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = equipment.Id;
                cmd.Parameters.Add("@serial_number", MySqlDbType.String).Value = equipment?.Serial_number;
                cmd.Parameters.Add("@inventory_number", MySqlDbType.String).Value = equipment?.Inventory_number;
                cmd.Parameters.Add("@kindId", MySqlDbType.Int32).Value = equipment?.Equipment_kind?.Id;
                cmd.Parameters.Add("@manufacturerId", MySqlDbType.Int32).Value = equipment?.Equipment_manufacturer?.Id;
                cmd.Parameters.Add("@modelId", MySqlDbType.Int32).Value = equipment?.Equipment_model?.Id;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = equipment?.Company?.Id;
                cmd.Parameters.Add("@maintenanceEntitiesId", MySqlDbType.Int32).Value = equipment?.Maintenance_entity?.Id;

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

        public static async Task<bool> InsertEquipmentParameter(EquipmentParameter equipmentParameter)
        {
            if (equipmentParameter == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT parameter (equipmentId, kindParameterId, value) VALUES (@equipmentId, @kindParameterId, @value)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@equipmentId", MySqlDbType.Int32).Value = equipmentParameter?.Equipment?.Id;
                cmd.Parameters.Add("@kindParameterId", MySqlDbType.Int32).Value = equipmentParameter?.KindParam?.Id;
                cmd.Parameters.Add("@value", MySqlDbType.String).Value = equipmentParameter?.Value;

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

        public static async Task<bool> InsertCoord(int maintenanceEntityId, decimal[] coord)
        {
            if (coord[0] <= 0 && coord[1] <= 0 && maintenanceEntityId < 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT coordinates (maintenanceEntitiesId, N, E) VALUES (@maintenanceEntitiesId, @N, @E)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters?.Clear();

                cmd.Parameters.Add("@maintenanceEntitiesId", MySqlDbType.Int32).Value = maintenanceEntityId;
                cmd.Parameters.Add("@N", MySqlDbType.Decimal).Value = coord[0];
                cmd.Parameters.Add("@E", MySqlDbType.Decimal).Value = coord[1];

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

        public static async Task<bool> InsertKindParam(int kindId, int kindParamId)
        {
            if (kindId <= 0 || kindParamId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT kind_param (kindId, kindParamId) VALUES (@kindId, @kindParamId)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@kindId", MySqlDbType.Int32).Value = kindId;
                cmd.Parameters.Add("@kindParamId", MySqlDbType.Int32).Value = kindParamId;
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

        public static async Task<bool> InsertEmployeeGroup(int employeeId, int groupId)
        {
            if (employeeId <= 0 || groupId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT employee_groups (employeeId, groupId) VALUES (@employeeId, @groupId)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;
                cmd.Parameters.Add("@groupId", MySqlDbType.Int32).Value = groupId;
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

        public static async Task<bool> InsertEmployeeRole(int employeeId, int roleId)
        {
            if (employeeId <= 0 || roleId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT employee_roles (employeeId, roleId) VALUES (@employeeId, @roleId)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;
                cmd.Parameters.Add("@roleId", MySqlDbType.Int32).Value = roleId;
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

        public static async Task<bool> InsertEmployee(Employee employee)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT employee (id, first_name, last_name, patronymic, position, active, email, login, phone) " +
                    "VALUES (@id, @first_name, @last_name, @patronymic, @position, @active, @email, @login, @phone)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = employee.Id;
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

        public static async Task<bool> InsertGroup(Group group)
        {
            if (group == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT `group` (id, name, active, description) " +
                    "VALUES (@id, @name, @active, @description)";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = group.Id;
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

        public static async Task<bool> InsertRole(Role role)
        {
            if (role == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT role (name) " +
                    "VALUES (@name)";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;
                // id нет потому что оно автоинкрементируемое
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

        public static async Task<bool> InsertEmployeePerformance(Employee employee, DateTime day)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT employee_performance (date, employeeId, solvedTasks, spentedTime) " +
                    "VALUES (@date, @employeeId, @solvedTasks, @spentedTime)";

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

        public static async Task<bool> InsertUser(User user)
        {
            if (user == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT user (email, passwordHash, roleId, active) " +
                    "VALUES (@email, @passwordHash, @roleId, @active)";

                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = sqlCommand;
                // id нет потому что оно автоинкрементируемое
                cmd.Parameters.Add("@email", MySqlDbType.String).Value = user?.Email;
                cmd.Parameters.Add("@passwordHash", MySqlDbType.String).Value = user?.Password;
                cmd.Parameters.Add("@roleId", MySqlDbType.Int32).Value = Convert.ToInt32(user?.Role);
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = user?.Active;

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