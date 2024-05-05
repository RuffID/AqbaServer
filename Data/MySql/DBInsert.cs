using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AqbaServer.Data.MySql
{
    public class DBInsert
    {
        public static async Task<bool> InsertCategory(Category category)
        {
            if (category == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();

                string sqlCommand = $"INSERT company_category (id, color, name, code) VALUES (@id, @color, @name, @code)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = category.Id;
                cmd.Parameters.Add("@color", MySqlDbType.String).Value = category.Color;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = category?.Code;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = category?.Name;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertCompany(int categoryId, Company company)
        {
            if (company == null || categoryId < 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "INSERT company (id, name, additional_name, active, categoryId)  VALUES (@id, @name, @additional_name, @active, @categoryId)";
                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertMaintenanceEntity(MaintenanceEntity maintenanceEntity)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "INSERT maintenance_entity (id, name, address, active, companyId)  VALUES (@id, @name, @address, @active, @companyId)";
                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = maintenanceEntity.Id;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = maintenanceEntity?.Name;
                cmd.Parameters.Add("@address", MySqlDbType.String).Value = maintenanceEntity?.Address;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = maintenanceEntity?.Active;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT manufacturer (code, name, description, visible) " +
                    "VALUES (@code, @name, @description, @visible)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertKind(Kind kind)
        {
            if (kind == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT kind (code, name, description, visible) VALUES (@code, @name, @description, @visible)";
                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertKindParameter(KindParameter kindParameter)
        {
            if (kindParameter == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT kinds_parameters (name, code, fieldType) VALUES (@name, @code, @fieldType)";
                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                //cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = kindParameter.Id;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertModel(Model model)
        {
            if (model == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT model (code, name, description, visible, kindId, manufacturerId) " +
                    "VALUES (@code, @name, @description, @visible, @kindId, @manufacturerId)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertEquipment(Equipment equipment)
        {
            if (equipment == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT equipment (id, serial_number, inventory_number, kindId, manufacturerId, modelId, companyId, maintenanceEntitiesId) " +
                    "VALUES (@id, @serial_number, @inventory_number, @kindId, @manufacturerId, @modelId, @companyId, @maintenanceEntitiesId)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = equipment.Id;
                cmd.Parameters.Add("@serial_number", MySqlDbType.String).Value = equipment?.Serial_number;
                cmd.Parameters.Add("@inventory_number", MySqlDbType.String).Value = equipment?.Inventory_number;
                cmd.Parameters.Add("@kindId", MySqlDbType.Int32).Value = equipment?.Equipment_kind?.Id;
                cmd.Parameters.Add("@manufacturerId", MySqlDbType.Int32).Value = equipment?.Equipment_manufacturer?.Id;
                cmd.Parameters.Add("@modelId", MySqlDbType.Int32).Value = equipment?.Equipment_model?.Id;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = equipment?.Company?.Id == 0 ? null : equipment?.Company?.Id;
                cmd.Parameters.Add("@maintenanceEntitiesId", MySqlDbType.Int32).Value = equipment?.Maintenance_entity?.Id == 0 ? null : equipment?.Maintenance_entity?.Id;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertEquipmentParameter(EquipmentParameter equipmentParameter)
        {
            if (equipmentParameter == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT parameter (equipmentId, kindParameterId, value) VALUES (@equipmentId, @kindParameterId, @value)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
                
        public static async Task<bool> InsertKindParam(int kindId, int kindParamId)
        {
            if (kindId <= 0 || kindParamId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT kind_param (kindId, kindParamId) VALUES (@kindId, @kindParamId)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertEmployeeGroup(int id, int employeeId, int groupId)
        {
            if (id <= 0 || employeeId <= 0 || groupId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT employee_groups (id, employeeId, groupId) VALUES (@id, @employeeId, @groupId)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertEmployeeRole(int employeeId, int roleId)
        {
            if (employeeId <= 0 || roleId <= 0) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"INSERT employee_roles (employeeId, roleId) VALUES (@employeeId, @roleId)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertEmployee(Employee employee)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT employee (id, first_name, last_name, patronymic, position, active, email, login, phone) " +
                    "VALUES (@id, @first_name, @last_name, @patronymic, @position, @active, @email, @login, @phone)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertGroup(Group group)
        {
            if (group == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT `group` (id, name, active, description) " +
                    "VALUES (@id, @name, @active, @description)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertRole(Role role)
        {
            if (role == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT role (name) " +
                    "VALUES (@name)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertUser(User user)
        {
            if (user == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT user (email, passwordHash, roleId, active) " +
                    "VALUES (@email, @passwordHash, @roleId, @active)";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertIssue(Issue issue)
        {
            if (issue == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT issue (id, assignee_id, author_id, title, employees_updated_at, created_at, completed_at, deadline_at, delay_to, deleted_at, statusId, typeId, priorityId, companyId, service_objectId) " +
                    "VALUES (@id, @assignee_id, @author_id, @title, @employees_updated_at, @created_at, @completed_at, @deadline_at, @delay_to, @deleted_at, @statusId, @typeId, @priorityId, @companyId, @service_objectId)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = issue.Id;
                cmd.Parameters.Add("@assignee_id", MySqlDbType.Int32).Value = issue?.Assignee_id ?? null;
                cmd.Parameters.Add("@author_id", MySqlDbType.Int32).Value = issue?.Author_id ?? null;
                cmd.Parameters.Add("@title", MySqlDbType.String).Value = issue?.Title ?? null;
                cmd.Parameters.Add("@employees_updated_at", MySqlDbType.DateTime).Value = issue?.Employees_updated_at != null ? issue.Employees_updated_at : null;
                cmd.Parameters.Add("@created_at", MySqlDbType.DateTime).Value = issue?.Created_at != null ? issue.Created_at : null;
                cmd.Parameters.Add("@completed_at", MySqlDbType.DateTime).Value = issue?.Completed_at != null ? issue.Completed_at : null;
                cmd.Parameters.Add("@deadline_at", MySqlDbType.DateTime).Value = issue?.Deadline_at != null ? issue.Deadline_at : null;
                cmd.Parameters.Add("@delay_to", MySqlDbType.DateTime).Value = issue?.Delay_to != null ? issue.Delay_to : null;
                cmd.Parameters.Add("@deleted_at", MySqlDbType.DateTime).Value = issue?.Deleted_at != null ? issue.Deleted_at : null;
                cmd.Parameters.Add("@statusId", MySqlDbType.Int32).Value = issue?.Status?.Id ?? null;
                cmd.Parameters.Add("@typeId", MySqlDbType.Int32).Value = issue?.Type?.Id ?? null;
                cmd.Parameters.Add("@priorityId", MySqlDbType.Int32).Value = issue?.Priority?.Id ?? null;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = issue?.Company?.Id != null && issue.Company.Id != 0 ? issue.Company.Id : null;
                cmd.Parameters.Add("@service_objectId", MySqlDbType.Int32).Value = issue?.Service_object != null && issue.Service_object.Id != 0 ? issue.Service_object.Id : null;

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                string msg = $"Issue create info: Id: {issue.Id}, AssigneeId: {issue.Assignee_id}, statusId: {issue.Status?.Id}, typeId: {issue.Type?.Id}, priorityId: {issue.Priority?.Id}, companyId: {issue.Company?.Id}, service_objectId: {issue.Service_object?.Id}";
                WriteLog.Error(msg + "\n" + e.ToString());
                return false;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertIssueType(IssueType type)
        {
            if (type == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT issue_type (`name`, `code`, `default`, `inner`, `available_for_client`, `type`) " +
                    "VALUES (@name, @code, @default, @inner, @available_for_client, @type)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                // id нет потому что оно автоинкрементируемое
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = type?.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = type?.Code;
                cmd.Parameters.Add("@default", MySqlDbType.Bit).Value = type?.Default;
                cmd.Parameters.Add("@inner", MySqlDbType.Bit).Value = type?.Inner;
                cmd.Parameters.Add("@available_for_client", MySqlDbType.Bit).Value = type?.Available_for_client;
                cmd.Parameters.Add("@type", MySqlDbType.String).Value = type?.Type;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertIssueStatus(Status? status)
        {
            if (status == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT issue_status (name, code, color) " +
                    "VALUES (@name, @code, @color)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                // id нет потому что оно автоинкрементируемое
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = status?.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = status?.Code;
                cmd.Parameters.Add("@color", MySqlDbType.String).Value = status?.Color;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertIssuePriority(Priority? priority)
        {
            if (priority == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT issue_priority (name, code, position, color) " +
                    "VALUES (@name, @code, @position, @color)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                // id нет потому что оно автоинкрементируемое
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = priority?.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = priority?.Code;
                cmd.Parameters.Add("@position", MySqlDbType.Int32).Value = priority?.Position == 0 ? null : priority?.Position;
                cmd.Parameters.Add("@color", MySqlDbType.String).Value = priority?.Color;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> InsertTimeEntry(TimeEntry? timeEntry)
        {
            if (timeEntry == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "INSERT time_entry (id, employeeId, spentTime, issueId, logged_at) " +
                    "VALUES (@id, @employeeId, @spentTime, @issueId, @logged_at)";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = timeEntry.Id;
                cmd.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = timeEntry.Employee?.Id;
                cmd.Parameters.Add("@spentTime", MySqlDbType.Double).Value = timeEntry.Spent_Time;
                cmd.Parameters.Add("@issueId", MySqlDbType.Int32).Value = timeEntry.Issue_id;
                cmd.Parameters.Add("@logged_at", MySqlDbType.DateTime).Value = timeEntry.Logged_At;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }
}