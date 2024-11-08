﻿using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;

namespace AqbaServer.Data.MySql
{
    public class DBUpdate
    {
        public static async Task<bool> UpdateCategory(string categoryCode, Category category)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE company_category " +
                    "SET id = @id, code = @code, name = @name, color = @color " +
                    "WHERE code = @categoryCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = category.Id;
                cmd.Parameters.Add("@categoryCode", MySqlDbType.String).Value = categoryCode;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = category?.Code;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = category?.Name;
                cmd.Parameters.Add("@color", MySqlDbType.String).Value = category?.Color;

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

        /*public static async Task<bool> UpdateCategoryWithoutColor(string categoryCode, Category category)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE company_category " +
                    "SET id = @id, code = @code, name = @name " +
                    "WHERE code = @categoryCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = category.Id;
                cmd.Parameters.Add("@categoryCode", MySqlDbType.String).Value = categoryCode;
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
        }*/

        public static async Task<bool> UpdateCompany(int companyId, Company company)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE company " +
                    "SET id = @id, name = @name, additional_name = @additional_name, active = @active, categoryId = @categoryId " +
                    "WHERE id = @companyId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                company.Category ??= new();

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
                WriteLog.Error($"[Method: {nameof(UpdateCompany)}] Company id: {company.Id} \n{e}");
                return false;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity maintenanceEntity)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE maintenance_entity " +
                    "SET id = @id, name = @name, address = @address, active = @active, companyId = @companyId " +
                    "WHERE id = @maintenanceEntityId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = maintenanceEntity.Id;
                cmd.Parameters.Add("@maintenanceEntityId", MySqlDbType.Int32).Value = maintenanceEntityId;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = maintenanceEntity?.Name;
                cmd.Parameters.Add("@address", MySqlDbType.String).Value = maintenanceEntity?.Address;
                cmd.Parameters.Add("@active", MySqlDbType.Bit).Value = maintenanceEntity?.Active;
                cmd.Parameters.Add("@companyId", MySqlDbType.Int32).Value = maintenanceEntity?.Company_Id;
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception e)
            {
                WriteLog.Error($"[Method: {nameof(UpdateMaintenanceEntity)}] Maintenance id: {maintenanceEntityId} \n{e}");
                return false;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateManufacturer(string manufacturerCode, Manufacturer manufacturer)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE manufacturer " +
                    "SET code = @code, name = @name, description = @description, visible = @visible " +
                    "WHERE code = @manufacturerCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateKind(string kindCode, Kind kind)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE kind " +
                    "SET code = @code, name = @name, description = @description, visible = @visible " +
                    "WHERE code = @kindCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateKindParameter(string kindParameterCode, KindParameter parameter)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE kinds_parameters " +
                    "SET code = @code, name = @name, fieldType = @fieldType " +
                    "WHERE code = @kindParameterCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                //cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = parameter.Id;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateModel(string modelCode, Model model)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE model " +
                    "SET code = @code, name = @name, description = @description, visible = @visible, kindId = @kindId, manufacturerId = @manufacturerId " +
                    "WHERE code = @modelCode";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter parameter)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE parameter " +
                    "SET equipmentId = @equipmentId, kindParameterId = @kindParameterId, value = @value " +
                    "WHERE id  = @equipmentParameterId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@equipmentId", MySqlDbType.Int32).Value = parameter?.Equipment?.Id;
                cmd.Parameters.Add("@equipmentParameterId", MySqlDbType.Int32).Value = equipmentParameterId;
                cmd.Parameters.Add("@kindParameterId", MySqlDbType.Int32).Value = parameter?.KindParam?.Id;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEquipment(int equipmentId, Equipment equipment)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE equipment " +
                    "SET id = @id, serial_number = @serial_number, inventory_number = @inventory_number, kindId = @kindId, manufacturerId = @manufacturerId, modelId = @modelId, companyId = @companyId, maintenanceEntitiesId = @maintenanceEntitiesId " +
                    "WHERE id = @eqipmentId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = equipment.Id;
                cmd.Parameters.Add("@eqipmentId", MySqlDbType.Int32).Value = equipmentId;
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
                WriteLog.Error($"[Method: {nameof(UpdateEquipment)}] Equipment id: {equipmentId} \n{e}");
                return false;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateEmployee(int employeeId, Employee employee)
        {
            if (employee == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE employee " +
                    "SET id = @id, first_name = @first_name, last_name = @last_name, patronymic = @patronymic, position = @position, active = @active, email = @email, login = @login, phone = @phone " +
                    "WHERE id = @employeeId";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateGroup(int groupId, Group group)
        {
            if (group == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE `group` " +
                    "SET id = @id, name = @name, active = @active, description = @description " +
                    "WHERE id = @groupId";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateRole(string roleName, Role role)
        {
            if (role == null) return false;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE role " +
                    "SET name = @name " +
                    "WHERE name = @roleName";

                cmd.Connection = connection;
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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateRefreshToken(int userId, string refreshToken, DateTime expirationRefreshToken)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE user " +
                    "SET refreshToken = @refreshToken, expirationRefreshToken = @expirationRefreshToken " +
                    "WHERE id = @userId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateUser(User userUpdate)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE user " +
                    "SET email = @email, roleId = @roleId, passwordHash = @passwordHash, refreshToken = @refreshToken, expirationRefreshToken = @expirationRefreshToken, active = @active " +
                    "WHERE id = @userId";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> UpdateIssue(Issue issue)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE issue " +
                    "SET id = @id, assignee_id = @assignee_id, author_id = @author_id, title = @title, employees_updated_at = @employees_updated_at, created_at = @created_at, completed_at = @completed_at, deadline_at = @deadline_at, delay_to = @delay_to, deleted_at = @deleted_at, statusId = @statusId, typeId = @typeId, priorityId = @priorityId, companyId = @companyId, service_objectId = @service_objectId " +
                    "WHERE issue.id = @id";

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
                string msg = $"Issue update info: Id: {issue.Id}, AssigneeId: {issue.Assignee_id}, statusId: {issue.Status?.Id}, typeId: {issue.Type?.Id}, priorityId: {issue.Priority?.Id}, companyId: {issue.Company?.Id}, service_objectId: {issue.Service_object?.Id}";
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

        public static async Task<bool> UpdateIssueType(IssueType type)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE issue_type " +
                    "SET `name` = @name, `code` = @code, `default` = @default, `inner` = @inner, `available_for_client` = @available_for_client, `type` = @type " +
                    "WHERE code = @code";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                // id нет потому что оно автоинкрементируемое
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = type?.Name;
                cmd.Parameters.Add("@code", MySqlDbType.String).Value = type?.Code;
                cmd.Parameters.Add("@default", MySqlDbType.Bit).Value = type?.Default;
                cmd.Parameters.Add("@inner", MySqlDbType.Bit).Value = type?.Inner;
                cmd.Parameters.Add("@available_for_client", MySqlDbType.Bit).Value = type?.Available_for_client;
                cmd.Parameters.Add("type", MySqlDbType.String).Value = type?.Type;

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

        public static async Task<bool> UpdateIssueStatus(Status status)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE issue_status " +
                    "SET name = @name, code = @code, color = @color " +
                    "WHERE code = @code";

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

        public static async Task<bool> UpdateIssuePriority(Priority priority)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE issue_priority " +
                    "SET name = @name, code = @code, position = @position, color = @color " +
                    "WHERE code = @code";

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

        public static async Task<bool> UpdateTimeEntry(TimeEntry timeEntry)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand =
                    "UPDATE time_entry " +
                    "SET id = @id, employeeId = @employeeId, spentTime = @spentTime, issueId = @issueId, logged_at = @logged_at " +
                    "WHERE id = @id";

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
