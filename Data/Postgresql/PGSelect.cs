using AqbaServer.Helper;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data.Common;

namespace AqbaServer.Data.Postgresql
{
    public class PGSelect
    {
        public static readonly long limit = 1000;
        public static readonly long limitForEquipment = 500;

        public static async Task<ICollection<Issue>?> SelectIssues(DateTime dateFrom, DateTime dateTo, long lastIssueId)
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Issue> issues = [];
                Issue issue;

                string sqlCommand = string.Format(
                    "SELECT issues.sequential_id, assigned.sequential_id AS assignee_id, author.sequential_id AS author_id, issues.title, issues.created_at, issues.completed_at, issues.deadline_at, issues.employees_updated_at, issues.deleted_at, issues.delay_to, issue_statuses.code AS statusCode, issue_work_types.code AS typeCode, issue_priorities.code AS priorityCode, companies.sequential_id AS companyId, company_maintenance_entities.sequential_id AS maintenanceEntityId " +
                    "FROM issues " +
                    "LEFT OUTER JOIN issue_statuses ON issues.status_id = issue_statuses.id " +
                    "LEFT OUTER JOIN issue_work_types ON issues.work_type_id = issue_work_types.id " +
                    "LEFT OUTER JOIN issue_priorities ON issues.priority_id = issue_priorities.id " +
                    "LEFT OUTER JOIN companies ON issues.company_id = companies.id " +
                    "LEFT OUTER JOIN company_maintenance_entities ON issues.maintenance_entity_id = company_maintenance_entities.id " +
                    "LEFT OUTER JOIN users AS assigned ON issues.assignee_id = assigned.id " +
                    "LEFT OUTER JOIN users AS author ON issues.author_id = author.id " +
                    "WHERE ((issues.employees_updated_at BETWEEN '{0}' AND '{1}') OR (issues.deleted_at BETWEEN '{0}' AND '{1}')) " +
                    "AND issues.sequential_id > '{2}' ORDER BY issues.sequential_id  LIMIT '{3}';", 
                    dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), dateTo.ToString("yyyy-MM-dd HH:mm:ss"), lastIssueId, limit);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        issue = new();
                        issue.Status = new();
                        issue.Priority = new();
                        issue.Type = new();
                        issue.Company = new();
                        issue.Service_object = new();

                        issue.Id = Convert.ToInt32(reader["sequential_id"]);                        

                        if (!reader.IsDBNull(reader.GetOrdinal("assignee_id")))
                            issue.Assignee_id = Convert.ToInt32(reader["assignee_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("author_id")))
                            issue.Author_id = Convert.ToInt32(reader["author_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("title")))
                            issue.Title = reader["title"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                            issue.Created_at = Convert.ToDateTime(reader["created_at"]).ToLocalTime();
                        if (!reader.IsDBNull(reader.GetOrdinal("completed_at")))
                            issue.Completed_at = Convert.ToDateTime(reader["completed_at"]).ToLocalTime();
                        if (!reader.IsDBNull(reader.GetOrdinal("deadline_at")))
                            issue.Deadline_at = Convert.ToDateTime(reader["deadline_at"]).ToLocalTime();
                        if (!reader.IsDBNull(reader.GetOrdinal("delay_to")))
                            issue.Delay_to = Convert.ToDateTime(reader["delay_to"]).ToLocalTime();
                        if (!reader.IsDBNull(reader.GetOrdinal("employees_updated_at")))
                            issue.Employees_updated_at = Convert.ToDateTime(reader["employees_updated_at"]).ToLocalTime();
                        if (!reader.IsDBNull(reader.GetOrdinal("deleted_at")))
                            issue.Deleted_at = Convert.ToDateTime(reader["deleted_at"]).ToLocalTime();

                        if (!reader.IsDBNull(reader.GetOrdinal("statusCode")))
                            issue.Status.Code = reader["statusCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("typeCode")))
                            issue.Type.Code = reader["typeCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityCode")))
                            issue.Priority.Code = reader["priorityCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            issue.Company.Id = Convert.ToInt32(reader["companyId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("maintenanceEntityId")))
                            issue.Service_object.Id = Convert.ToInt32(reader["maintenanceEntityId"].ToString());

                        issues.Add(issue);
                    }
                }                                
                return issues;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
        public static async Task<ICollection<TimeEntry>?> SelectTimeEntries(DateTime? dateFrom, DateTime? dateTo, long lastTimeEntryId)
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<TimeEntry> timeEntries = [];
                TimeEntry timeEntry;
                string sqlCommand = string.Format(
                    "SELECT time_entries.id, users.sequential_id AS employee_id, time_entries.spent_time, issues.sequential_id AS issue_id, time_entries.logged_at " +
                    "FROM time_entries " +
                    "LEFT OUTER JOIN users ON time_entries.employee_id = users.id " +
                    "LEFT OUTER JOIN issues ON time_entries.issue_id = issues.id " +
                    "WHERE (time_entries.logged_at BETWEEN '{0}' AND '{1}') " +
                    "AND time_entries.id > '{2}' ORDER BY time_entries.id LIMIT '{3}';",
                    dateFrom?.ToString("yyyy-MM-dd HH:mm:ss"), dateTo?.ToString("yyyy-MM-dd HH:mm:ss"), lastTimeEntryId, limit);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        timeEntry = new();
                        timeEntry.Employee = new();

                        timeEntry.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("employee_id")))
                            timeEntry.Employee.Id = Convert.ToInt32(reader["employee_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("issue_id")))
                            timeEntry.Issue_id = Convert.ToInt32(reader["issue_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("spent_time")))
                            timeEntry.Spent_Time = Convert.ToDouble(reader["spent_time"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("logged_at")))
                            timeEntry.Logged_At = Convert.ToDateTime(reader["logged_at"]).ToLocalTime();                                       

                        timeEntries.Add(timeEntry);
                    }
                }
                return timeEntries;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Status>?> SelectIssueStatuses()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Status> statuses = [];
                Status status;

                string sqlCommand = "SELECT * FROM issue_statuses";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        status = new();

                        status.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            status.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            status.Code = reader["code"].ToString();

                        statuses.Add(status);
                    }
                    return statuses;
                }
                return null;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Priority>?> SelectIssuePriorities()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Priority> priorities = [];
                Priority priority;

                string sqlCommand = "SELECT * FROM issue_priorities";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        priority = new();

                        priority.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            priority.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            priority.Code = reader["code"].ToString();

                        priorities.Add(priority);
                    }
                    return priorities;
                }
                return null;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<IssueType>?> SelectIssueTypes()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<IssueType> types = [];
                IssueType type;

                string sqlCommand = "SELECT * FROM issue_work_types";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        type = new();

                        type.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            type.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            type.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inner")))
                            type.Inner = Convert.ToBoolean(reader["inner"]);

                        types.Add(type);
                    }
                    return types;
                }
                return null;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Kind>?> SelectKinds()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Kind> kinds = [];
                Kind kind;
                string sqlCommand = "SELECT * FROM equipment_kinds";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        kind = new();

                        kind.Id = Convert.ToInt32(reader["id"].ToString());                        

                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            kind.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            kind.Name = reader["name"].ToString();                        

                        kinds.Add(kind);
                    }
                }
                return kinds;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Manufacturer>?> SelectManufacturers()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Manufacturer> manufacturers = [];
                Manufacturer manufacturer;
                string sqlCommand = "SELECT * FROM equipment_manufacturers";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        manufacturer = new();

                        manufacturer.Id = Convert.ToInt32(reader["id"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            manufacturer.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            manufacturer.Name = reader["name"].ToString();

                        manufacturers.Add(manufacturer);
                    }
                }
                return manufacturers;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Model>?> SelectModels()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Model> models = [];
                Model model;
                string sqlCommand =
                    "SELECT equipment_models.id, equipment_models.code, equipment_models.name, equipment_kinds.code AS kindCode, equipment_manufacturers.code AS manufacturerCode " +
                    "FROM equipment_models " +
                    "LEFT OUTER JOIN equipment_kinds ON equipment_models.equipment_kind_id = equipment_kinds.id " +
                    "LEFT OUTER JOIN equipment_manufacturers ON equipment_models.equipment_manufacturer_id = equipment_manufacturers.id";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        model = new();
                        model.EquipmentKind = new();
                        model.EquipmentManufacturer = new();

                        model.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            model.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            model.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("kindCode")))
                            model.EquipmentKind.Code = reader["kindCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerCode")))
                            model.EquipmentManufacturer.Code = reader["manufacturerCode"].ToString();                                               

                        models.Add(model);
                    }
                }
                return models;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Employee>?> SelectEmployees()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Employee> employees = [];
                Employee employee;
                string sqlCommand = "SELECT * FROM users WHERE type = 'Employee'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        employee = new();

                        employee.Id = Convert.ToInt32(reader["sequential_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("first_name")))
                            employee.First_name = reader["first_name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("last_name")))
                            employee.Last_name = reader["last_name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("patronymic")))
                            employee.Patronymic = reader["patronymic"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("position")))
                            employee.Position = reader["position"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            employee.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("email")))
                            employee.Email = reader["email"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("login")))
                            employee.Login = reader["login"].ToString();

                        employees.Add(employee);
                    }
                }
                return employees;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Group>?> SelectEmployeeGroups()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Group> groups = [];
                Group? group;
                string sqlCommand = "SELECT * FROM groups";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        group = new();

                        group.Id = Convert.ToInt32(reader["sequential_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            group.Name = reader["name"].ToString();

                        groups.Add(group);
                    }
                }
                return groups;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<(int id, int employeeId, int groupId)>?> SelectEmployeeGroupsConnections()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<(int id, int employeeId, int groupId)> employeeGroups = [];
                string sqlCommand =
                    "SELECT employee_groups.id, users.sequential_id AS userId, groups.sequential_id AS groupId " +
                    "FROM employee_groups " +
                    "LEFT OUTER JOIN users ON employee_groups.employee_id = users.id " +
                    "LEFT OUTER JOIN groups ON employee_groups.group_id = groups.id " +
                    "WHERE type = 'Employee' " +
                    "ORDER BY users.sequential_id;";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int employeeId = 0;
                        int groupId = 0;
                        int id = 0;

                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                            id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("userId")))
                            employeeId = Convert.ToInt32(reader["userId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("groupId")))
                            groupId = Convert.ToInt32(reader["groupId"]);

                        if (id == 0 || employeeId == 0 || groupId == 0) continue;

                        employeeGroups.Add(new(id, employeeId, groupId));
                    }
                }
                return employeeGroups;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Company>?> SelectCompanies()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Company> companies = [];
                Company company;
                string sqlCommand =
                    "SELECT companies.sequential_id AS id, companies.name, companies.additional_name, company_categories.code AS categoryCode, company_categories.name AS categoryName " +
                    "FROM companies " +
                    "LEFT OUTER JOIN company_categories ON companies.category_id = company_categories.id;";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        company = new();
                        company.Category = new();

                        company.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            company.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("additional_name")))
                            company.AdditionalName = reader["additional_name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("categoryCode")))
                            company.Category.Code = reader["categoryCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("categoryName")))
                            company.Category.Name = reader["categoryName"].ToString();

                        companies.Add(company);
                    }
                }
                return companies;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Category>?> SelectCompanyCategories()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Category> categories = [];
                Category category;
                string sqlCommand = "SELECT * FROM company_categories";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        category = new();

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            category.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            category.Code = reader["code"].ToString();
                        
                        categories.Add(category);
                    }
                }
                return categories;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<MaintenanceEntity>?> SelectMaintenanceEntities()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<MaintenanceEntity> maintenanceEntities = [];
                MaintenanceEntity maintenanceEntity;
                string sqlCommand =
                    "SELECT company_maintenance_entities.sequential_id, company_maintenance_entities.name, companies.sequential_id AS companyId, company_maintenance_entities.active " +
                    "FROM company_maintenance_entities " +
                    "JOIN companies ON company_maintenance_entities.company_id = companies.id";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        maintenanceEntity = new();
                        maintenanceEntity.Id = Convert.ToInt32(reader["sequential_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            maintenanceEntity.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            maintenanceEntity.Company_Id = Convert.ToInt32(reader["companyId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            maintenanceEntity.Active = Convert.ToBoolean(reader["active"]);

                        maintenanceEntities?.Add(maintenanceEntity);
                    }
                }
                return maintenanceEntities;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Equipment>?> SelectEquipments(long lastEquipmentId)
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment>? equipments = [];
                Equipment equipment;
                string sqlCommand = string.Format(
                    "SELECT equipments.sequential_id, equipments.inventory_number, equipments.serial_number, equipments.parameters, company_maintenance_entities.sequential_id AS maintenanceEntitiesId, companies.sequential_id AS companyId, equipment_kinds.code AS kindCode, equipment_manufacturers.code AS manufacturerCode, equipment_models.code AS modelCode " +
                    "FROM equipments " +
                    "LEFT OUTER JOIN companies ON equipments.company_id = companies.id " +
                    "LEFT OUTER JOIN company_maintenance_entities ON equipments.maintenance_entity_id = company_maintenance_entities.id " +
                    "LEFT OUTER JOIN equipment_kinds ON equipments.equipment_kind_id = equipment_kinds.id " +
                    "LEFT OUTER JOIN equipment_manufacturers ON equipments.equipment_manufacturer_id = equipment_manufacturers.id " +
                    "LEFT OUTER JOIN equipment_models ON equipments.equipment_model_id = equipment_models.id " +
                    "WHERE equipments.sequential_id > '{0}' ORDER BY equipments.sequential_id LIMIT '{1}';", lastEquipmentId, limitForEquipment);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        equipment = new();
                        equipment.Company = new();
                        equipment.Maintenance_entity = new();
                        equipment.Equipment_kind = new();
                        equipment.Equipment_manufacturer = new();
                        equipment.Equipment_model = new();
                        equipment.Parameters = [];

                        equipment.Id = Convert.ToInt32(reader["sequential_id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                            equipment.Serial_number = reader["serial_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                            equipment.Inventory_number = reader["inventory_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            equipment.Company.Id = Convert.ToInt32(reader["companyId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("maintenanceEntitiesId")))
                            equipment.Maintenance_entity.Id = Convert.ToInt32(reader["maintenanceEntitiesId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("kindCode")))
                            equipment.Equipment_kind.Code = reader["kindCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerCode")))
                            equipment.Equipment_manufacturer.Code = reader["manufacturerCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("modelCode")))
                            equipment.Equipment_model.Code = reader["modelCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("parameters")))
                        {
                            string? tempParameters = reader["parameters"].ToString();
                            if (!string.IsNullOrEmpty(tempParameters))
                            {
                                JObject o = JObject.Parse(tempParameters);
                                foreach(var prop in o.Properties())
                                    equipment.Parameters.Add(new() { Code = prop.Name, Value = prop.Value.ToString() });
                            }
                        }                        

                        equipments.Add(equipment);
                    }
                }
                return equipments;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<KindParameter>?> SelectEquipmentParameters()
        {
            NpgsqlConnection connection = PGConfig.GetPsqlConnection();
            NpgsqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<KindParameter>? kindParameters = [];
                KindParameter param;
                string sqlCommand =  "SELECT sequential_id AS id, code, name FROM equipment_parameters;";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        param = new();

                        param.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            param.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            param.Name = reader["name"].ToString();
                        
                        kindParameters.Add(param);
                    }
                }
                return kindParameters;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
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
