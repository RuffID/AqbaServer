using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace AqbaServer.Data.MySql
{
    public class DBSelect
    {
        static readonly int limit = 100;

        public static async Task<ICollection<Company>?> SelectCompanies(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Company> companies = [];
                Company company;

                string sqlCommand = $"SELECT * FROM company WHERE id >= {companyId} LIMIT {limit}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;                

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        company = new();

                        company.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            company.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("additional_name")))
                            company.AdditionalName = reader["additional_name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            company.Active = Convert.ToBoolean(reader["active"]);

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

        public static async Task<ICollection<Company>?> SelectCompaniesByCategory(string categoryCode, int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Company> Companies = [];
                Company company;
                string sqlCommand = string.Format(
                    "SELECT company.id, company.name, company.additional_name, company.active, company.categoryId, company_category.color " +
                    "FROM company " +
                    "JOIN company_category ON company.categoryId = company_category.id " +
                    "WHERE company_category.code = '{0}' AND company.id >= {1} LIMIT {2}", categoryCode, companyId, limit);

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
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            company.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("categoryId")))
                            company.Category.Id = Convert.ToInt32(reader["categoryId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("color")))
                            company.Category.Color = reader["color"].ToString();

                        Companies.Add(company);
                    }
                }
                return Companies;
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

        public static async Task<Company?> SelectCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Company company;
                string sqlCommand = string.Format(
                    "SELECT company.id, company.name, company.additional_name, company.active, company.categoryId, company_category.color " +
                    "FROM company " +
                    "JOIN company_category ON company.categoryId = company_category.id " +
                    "WHERE company.id = {0}", companyId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    company = new();
                    company.Category = new();

                    company.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        company.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("additional_name")))
                        company.AdditionalName = reader["additional_name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("active")))
                        company.Active = Convert.ToBoolean(reader["active"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("categoryId")))
                        company.Category.Id = Convert.ToInt32(reader["categoryId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("color")))
                        company.Category.Color = reader["color"].ToString();

                    return company;
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

        public static async Task<int?> SelectLastCompany()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM company ORDER BY id DESC LIMIT 1";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        return Convert.ToInt32(reader["id"].ToString());
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

        public static async Task<ICollection<MaintenanceEntity>?> SelectMaintenanceEntities(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<MaintenanceEntity> maintenanceEntities = [];
                MaintenanceEntity maintenanceEntity;
                string sqlCommand = $"SELECT * FROM maintenance_entity WHERE id >= {maintenanceEntityId} LIMIT {limit}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        maintenanceEntity = new();
                        maintenanceEntity.Id = Convert.ToInt32(reader["id"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            maintenanceEntity.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("address")))
                            maintenanceEntity.Address = reader["address"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            maintenanceEntity.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            maintenanceEntity.Company_Id = Convert.ToInt32(reader["companyId"]);

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

        public static async Task<ICollection<MaintenanceEntity>?> SelectMaintenanceEntitiesByCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<MaintenanceEntity> maintenanceEntities = [];
                MaintenanceEntity maintenanceEntity;
                string sqlCommand = $"SELECT * FROM `maintenance_entity` WHERE companyId = '{companyId}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        maintenanceEntity = new();
                        maintenanceEntity.Id = Convert.ToInt32(reader["id"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            maintenanceEntity.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("address")))
                            maintenanceEntity.Address = reader["address"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            maintenanceEntity.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            maintenanceEntity.Company_Id = Convert.ToInt32(reader["companyId"]);

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

        public static async Task<MaintenanceEntity?> SelectMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                MaintenanceEntity maintenanceEntity;
                string sqlCommand = $"SELECT * FROM maintenance_entity WHERE id = {maintenanceEntityId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    maintenanceEntity = new();
                    maintenanceEntity.Id = Convert.ToInt32(reader["id"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        maintenanceEntity.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("address")))
                        maintenanceEntity.Address = reader["address"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("active")))
                        maintenanceEntity.Active = Convert.ToBoolean(reader["active"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                        maintenanceEntity.Company_Id = Convert.ToInt32(reader["companyId"]);

                    return maintenanceEntity;
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

        public static async Task<int?> SelectLastMaintenanceEntity()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM maintenance_entity ORDER BY id DESC LIMIT 1";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        return Convert.ToInt32(reader["id"].ToString());
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

        public static async Task<ICollection<Equipment>?> SelectEquipments()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;
                string sqlCommand = $"SELECT * FROM equipment";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        equipment = new();

                        equipment.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                            equipment.Serial_number = reader["serial_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                            equipment.Inventory_number = reader["inventory_number"].ToString();

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

        public static async Task<ICollection<Equipment>?> SelectEquipments(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;
                string sqlCommand = $"SELECT * FROM equipment WHERE id >= {equipmentId} LIMIT {limit}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        equipment = new();

                        equipment.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                            equipment.Serial_number = reader["serial_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                            equipment.Inventory_number = reader["inventory_number"].ToString();

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

        public static async Task<Equipment?> SelectEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Equipment equipment;
                string sqlCommand = string.Format
                    ("SELECT equipment.id, equipment.serial_number, equipment.inventory_number, " +
                    "kind.id AS kindId, kind.code AS kindCode, kind.name AS kindName, kind.description AS kindDescription, kind.visible AS kindVisible, " +
                    "manufacturer.id AS manufacturerId, manufacturer.code AS manufacturerCode, manufacturer.name AS manufacturerName, manufacturer.description AS manufacturerDescription, manufacturer.visible AS manufacturerVisible, " +
                    "model.id AS modelId, model.code AS modelCode, model.name AS modelName, model.description AS modelDescription, model.visible AS modelVisible " +
                    "FROM equipment " +
                    "LEFT OUTER JOIN kind ON equipment.kindId = kind.id " +
                    "LEFT OUTER JOIN manufacturer ON equipment.manufacturerId = manufacturer.id " +
                    "LEFT OUTER JOIN model ON equipment.modelId = model.Id " +
                    "WHERE equipment.id = {0}", equipmentId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    equipment = new();
                    equipment.Equipment_kind = new Kind();
                    equipment.Equipment_kind.Parameters = new List<KindParameter>();
                    equipment.Equipment_manufacturer = new Manufacturer();
                    equipment.Equipment_model = new Model();
                    equipment.Parameters = new List<EquipmentParameter>();

                    equipment.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                        equipment.Serial_number = reader["serial_number"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                        equipment.Inventory_number = reader["inventory_number"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("kindId")))
                        equipment.Equipment_kind.Id = Convert.ToInt32(reader["kindId"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("kindCode")))
                        equipment.Equipment_kind.Code = reader["kindCode"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("kindName")))
                        equipment.Equipment_kind.Name = reader["kindName"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("kindDescription")))
                        equipment.Equipment_kind.Description = reader["kindDescription"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("kindVisible")))
                        equipment.Equipment_kind.Visible = Convert.ToBoolean(reader["kindVisible"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("manufacturerId")))
                        equipment.Equipment_manufacturer.Id = Convert.ToInt32(reader["manufacturerId"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("manufacturerCode")))
                        equipment.Equipment_manufacturer.Code = reader["manufacturerCode"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("manufacturerName")))
                        equipment.Equipment_manufacturer.Name = reader["manufacturerName"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("manufacturerDescription")))
                        equipment.Equipment_manufacturer.Description = reader["manufacturerDescription"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("manufacturerVisible")))
                        equipment.Equipment_manufacturer.Visible = Convert.ToBoolean(reader["manufacturerVisible"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("modelId")))
                        equipment.Equipment_model.Id = Convert.ToInt32(reader["modelId"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("modelCode")))
                        equipment.Equipment_model.Code = reader["modelCode"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("modelName")))
                        equipment.Equipment_model.Name = reader["modelName"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("modelVisible")))
                        equipment.Equipment_model.Visible = Convert.ToBoolean(reader["modelVisible"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("modelDescription")))
                        equipment.Equipment_model.Description = reader["modelDescription"].ToString();

                    var parameters = await SelectEquipmentParameters(equipment.Id);
                    if (parameters != null)
                        equipment.Parameters = (List<EquipmentParameter>)parameters;
                    equipment.Equipment_kind.Parameters = await SelectKindParameters(equipment.Equipment_kind.Id);


                    return equipment;
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

        public static async Task<int?> SelectLastEquipment()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM equipment ORDER BY id DESC LIMIT 1";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        return Convert.ToInt32( reader["id"].ToString() );
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

        public static async Task<ICollection<Equipment>?> SelectEquipmentsByMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;

                string sqlCommand = string.Format
                    ("SELECT equipment.id, equipment.serial_number, equipment.inventory_number, " +
                    "kind.id AS kindId, kind.code AS kindCode, kind.name AS kindName, kind.description AS kindDescription, kind.visible AS kindVisible, " +
                    "manufacturer.id AS manufacturerId, manufacturer.code AS manufacturerCode, manufacturer.name AS manufacturerName, manufacturer.description AS manufacturerDescription, manufacturer.visible AS manufacturerVisible, " +
                    "model.id AS modelId, model.code AS modelCode, model.name AS modelName, model.description AS modelDescription, model.visible AS modelVisible " +
                    "FROM equipment " +
                    "LEFT OUTER JOIN kind ON equipment.kindId = kind.id " +
                    "LEFT OUTER JOIN manufacturer ON equipment.manufacturerId = manufacturer.id " +
                    "LEFT OUTER JOIN model ON equipment.modelId = model.Id " +
                    "WHERE maintenanceEntitiesId = {0}", maintenanceEntityId);


                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        equipment = new();
                        equipment.Equipment_kind = new Kind();
                        equipment.Equipment_kind.Parameters = new List<KindParameter>();
                        equipment.Equipment_manufacturer = new Manufacturer();
                        equipment.Equipment_model = new Model();
                        equipment.Parameters = new List<EquipmentParameter>();

                        equipment.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                            equipment.Serial_number = reader["serial_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                            equipment.Inventory_number = reader["inventory_number"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindId")))
                            equipment.Equipment_kind.Id = Convert.ToInt32(reader["kindId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("kindCode")))
                            equipment.Equipment_kind.Code = reader["kindCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindName")))
                            equipment.Equipment_kind.Name = reader["kindName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindDescription")))
                            equipment.Equipment_kind.Description = reader["kindDescription"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindVisible")))
                            equipment.Equipment_kind.Visible = Convert.ToBoolean(reader["kindVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerId")))
                            equipment.Equipment_manufacturer.Id = Convert.ToInt32(reader["manufacturerId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerCode")))
                            equipment.Equipment_manufacturer.Code = reader["manufacturerCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerName")))
                            equipment.Equipment_manufacturer.Name = reader["manufacturerName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerDescription")))
                            equipment.Equipment_manufacturer.Description = reader["manufacturerDescription"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerVisible")))
                            equipment.Equipment_manufacturer.Visible = Convert.ToBoolean(reader["manufacturerVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelId")))
                            equipment.Equipment_model.Id = Convert.ToInt32(reader["modelId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelCode")))
                            equipment.Equipment_model.Code = reader["modelCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("modelName")))
                            equipment.Equipment_model.Name = reader["modelName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("modelVisible")))
                            equipment.Equipment_model.Visible = Convert.ToBoolean(reader["modelVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelDescription")))
                            equipment.Equipment_model.Description = reader["modelDescription"].ToString();
                                                
                        equipments.Add(equipment);
                    }
                }

                foreach (var equip in equipments)
                {
                    equip.Parameters = (List<EquipmentParameter>?)await SelectEquipmentParameters(equip.Id);
                    equip.Equipment_kind ??= new();
                    equip.Equipment_kind.Parameters = await SelectKindParameters(equip.Equipment_kind.Id);
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

        public static async Task<ICollection<Equipment>?> SelectEquipmentsByCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;

                string sqlCommand = string.Format
                    ("SELECT equipment.id, equipment.serial_number, equipment.inventory_number, " +
                    "kind.id AS kindId, kind.code AS kindCode, kind.name AS kindName, kind.description AS kindDescription, kind.visible AS kindVisible, " +
                    "manufacturer.id AS manufacturerId, manufacturer.code AS manufacturerCode, manufacturer.name AS manufacturerName, manufacturer.description AS manufacturerDescription, manufacturer.visible AS manufacturerVisible, " +
                    "model.id AS modelId, model.code AS modelCode, model.name AS modelName, model.description AS modelDescription, model.visible AS modelVisible " +
                    "FROM equipment " +
                    "LEFT OUTER JOIN kind ON equipment.kindId = kind.id " +
                    "LEFT OUTER JOIN manufacturer ON equipment.manufacturerId = manufacturer.id " +
                    "LEFT OUTER JOIN model ON equipment.modelId = model.Id " +
                    "WHERE companyId = {0}", companyId);


                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        equipment = new();
                        equipment.Equipment_kind = new Kind();
                        equipment.Equipment_kind.Parameters = new List<KindParameter>();
                        equipment.Equipment_manufacturer = new Manufacturer();
                        equipment.Equipment_model = new Model();
                        equipment.Parameters = new List<EquipmentParameter>();

                        equipment.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("serial_number")))
                            equipment.Serial_number = reader["serial_number"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("inventory_number")))
                            equipment.Inventory_number = reader["inventory_number"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindId")))
                            equipment.Equipment_kind.Id = Convert.ToInt32(reader["kindId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("kindCode")))
                            equipment.Equipment_kind.Code = reader["kindCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindName")))
                            equipment.Equipment_kind.Name = reader["kindName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindDescription")))
                            equipment.Equipment_kind.Description = reader["kindDescription"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("kindVisible")))
                            equipment.Equipment_kind.Visible = Convert.ToBoolean(reader["kindVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerId")))
                            equipment.Equipment_manufacturer.Id = Convert.ToInt32(reader["manufacturerId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerCode")))
                            equipment.Equipment_manufacturer.Code = reader["manufacturerCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerName")))
                            equipment.Equipment_manufacturer.Name = reader["manufacturerName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerDescription")))
                            equipment.Equipment_manufacturer.Description = reader["manufacturerDescription"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("manufacturerVisible")))
                            equipment.Equipment_manufacturer.Visible = Convert.ToBoolean(reader["manufacturerVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelId")))
                            equipment.Equipment_model.Id = Convert.ToInt32(reader["modelId"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelCode")))
                            equipment.Equipment_model.Code = reader["modelCode"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("modelName")))
                            equipment.Equipment_model.Name = reader["modelName"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("modelVisible")))
                            equipment.Equipment_model.Visible = Convert.ToBoolean(reader["modelVisible"]);

                        if (!reader.IsDBNull(reader.GetOrdinal("modelDescription")))
                            equipment.Equipment_model.Description = reader["modelDescription"].ToString();

                        equipments.Add(equipment);
                    }
                }

                foreach (var equip in equipments)
                {
                    equip.Parameters = (List<EquipmentParameter>?)await SelectEquipmentParameters(equip.Id);
                    equip.Equipment_kind ??= new();
                    equip.Equipment_kind.Parameters = await SelectKindParameters(equip.Equipment_kind.Id);
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

        public static async Task<ICollection<Kind>?> SelectKinds()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Kind> kinds = [];
                Kind kind;
                string sqlCommand = "SELECT * FROM kind";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        kind = new()
                        {
                            Id = Convert.ToInt32(reader["id"].ToString())
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            kind.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            kind.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            kind.Description = reader["description"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                            kind.Visible = Convert.ToBoolean(reader["visible"]);

                        kind.Parameters = await SelectKindParameters(kind.Id);

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

        public static async Task<Kind?> SelectKind(string? kindCode)
        {
            if (string.IsNullOrEmpty(kindCode)) return null;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Kind kind;
                string sqlCommand = $"SELECT * FROM kind WHERE code = '{kindCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    kind = new();

                    kind.Id = Convert.ToInt32(reader["id"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        kind.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        kind.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("description")))
                        kind.Description = reader["description"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                        kind.Visible = Convert.ToBoolean(reader["visible"]);

                    return kind;
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

        public static async Task<ICollection<KindParameter>?> SelectKindParameters()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<KindParameter> kindParameters = [];
                KindParameter parameter;
                string sqlCommand = "SELECT * FROM kinds_parameters";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {

                        parameter = new();
                        parameter.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            parameter.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("fieldType")))
                            parameter.FieldType = reader["fieldType"].ToString();

                        kindParameters.Add(parameter);
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

        public static async Task<ICollection<KindParameter>?> SelectKindParameters(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<KindParameter> kindParameters = [];
                KindParameter parameter;
                string sqlCommand = string.Format(
                    "SELECT kinds_parameters.id, kinds_parameters.code, kinds_parameters.name, kinds_parameters.fieldType " +
                    "FROM kind " +
                    "JOIN kind_param ON kind.id = kind_param.kindId " +
                    "JOIN kinds_parameters ON kind_param.kindParamId = kinds_parameters.id " +
                    "WHERE kind.id = {0}", kindId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        parameter = new();

                        parameter.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            parameter.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            parameter.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("fieldType")))
                            parameter.FieldType = reader["fieldType"].ToString();

                        kindParameters.Add(parameter);
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

        public static async Task<KindParameter?> SelectKindParameter(string kindParameterCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                KindParameter parameter;
                string sqlCommand = $"SELECT * FROM kinds_parameters WHERE code = '{kindParameterCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    parameter = new();
                    parameter.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        parameter.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        parameter.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("fieldType")))
                        parameter.FieldType = reader["fieldType"].ToString();

                    return parameter;
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

        public static async Task<ICollection<Manufacturer>?> SelectManufacturers()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Manufacturer> manufacturers = new List<Manufacturer>();
                Manufacturer manufacturer;
                string sqlCommand = "SELECT * FROM manufacturer";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = cmd.ExecuteReader();
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
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            manufacturer.Description = reader["description"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                            manufacturer.Visible = Convert.ToBoolean(reader["visible"]);

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

        public static async Task<Manufacturer?> SelectManufacturer(string? manufacturerCode)
        {
            if (string.IsNullOrEmpty(manufacturerCode)) return null;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Manufacturer manufacturer;
                string sqlCommand = $"SELECT * FROM manufacturer WHERE code = '{manufacturerCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    manufacturer = new();

                    manufacturer.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        manufacturer.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        manufacturer.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("description")))
                        manufacturer.Description = reader["description"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                        manufacturer.Visible = Convert.ToBoolean(reader["visible"]);

                    return manufacturer;
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

        public static async Task<ICollection<Model>?> SelectModels()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Model> models = [];
                Model model;
                string sqlCommand = "SELECT * FROM model";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        model = new();
                        model.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            model.Code = reader["code"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            model.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            model.Description = reader["description"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                            model.Visible = Convert.ToBoolean(reader["visible"]);

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

        public static async Task<Model?> SelectModel(string? modelCode)
        {
            if (string.IsNullOrEmpty(modelCode)) return null;

            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Model model;
                string sqlCommand = $"SELECT * FROM model WHERE code = '{modelCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    model = new();
                    model.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        model.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        model.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("description")))
                        model.Description = reader["description"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("visible")))
                        model.Visible = Convert.ToBoolean(reader["visible"]);

                    return model;
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

        public static async Task<ICollection<Category>?> SelectCategories()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Category> categories = [];
                Category category;
                string sqlCommand = "SELECT * FROM company_category";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        category = new();
                        category.Id = Convert.ToInt32(reader["id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("color")))
                            category.Color = reader["color"].ToString();
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

        public static async Task<Category?> SelectCategory(string categoryCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Category category;
                string sqlCommand = $"SELECT * FROM company_category WHERE code = '{categoryCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    category = new();
                    category.Id = Convert.ToInt32(reader["id"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("color")))
                        category.Color = reader["color"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        category.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        category.Code = reader["code"].ToString();
                    
                    return category;
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

        public static async Task<Category?> SelectCategory(int categoryId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Category category;
                string sqlCommand = $"SELECT * FROM company_category WHERE id = '{categoryId}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    category = new();
                    category.Id = Convert.ToInt32(reader["id"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("color")))
                        category.Color = reader["color"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        category.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        category.Code = reader["code"].ToString();

                    return category;
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

        public static async Task<ICollection<EquipmentParameter>?> SelectEquipmentParameters()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<EquipmentParameter> equipmentParameters = new List<EquipmentParameter>();
                EquipmentParameter parameter;
                string sqlCommand = $"SELECT * FROM parameter";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        parameter = new();
                        parameter.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("value")))
                            parameter.Value = reader["value"].ToString();

                        equipmentParameters.Add(parameter);
                    }

                    return equipmentParameters;
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

        public static async Task<EquipmentParameter?> SelectEquipmentParameter(int equipmentId, int kindParamId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                EquipmentParameter parameter;
                string sqlCommand = $"SELECT * FROM parameter WHERE equipmentId = {equipmentId} AND kindParameterId = {kindParamId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    parameter = new();
                    parameter.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("value")))
                        parameter.Value = reader["value"].ToString();

                    return parameter;
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

        public static async Task<ICollection<EquipmentParameter>?> SelectEquipmentParameters(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<EquipmentParameter> equipmentParameters = [];
                EquipmentParameter parameter;

                string sqlCommand = string.Format(
                    "SELECT parameter.id, parameter.value, " +
                    "kinds_parameters.name, kinds_parameters.code, kinds_parameters.fieldType " +
                    "FROM parameter " +
                    "JOIN kinds_parameters ON parameter.kindParameterId = kinds_parameters.id " +
                    "WHERE equipmentId = {0}", equipmentId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        parameter = new();
                        parameter.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("value")))
                            parameter.Value = reader["value"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            parameter.Name = reader["name"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("code")))
                            parameter.Code = reader["code"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("fieldType")))
                            parameter.FieldType = reader["fieldType"].ToString();

                        equipmentParameters.Add(parameter);
                    }

                    return equipmentParameters;
                }
                return null;
            }
            catch (Exception e)
            {
                await cmd.DisposeAsync();
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> SelectKindParam(int kindId, int kindParameterId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM kind_param WHERE kindId = {kindId} AND kindParamId = {kindParameterId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                    return true;

                return false;
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

        public static async Task<Employee?> SelectEmployee(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Employee employee;
                string sqlCommand = $"SELECT * FROM employee WHERE id = {employeeId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    employee = new();

                    employee.Id = Convert.ToInt32(reader["id"]);
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
                    if (!reader.IsDBNull(reader.GetOrdinal("phone")))
                        employee.Phone = reader["phone"].ToString();

                    return employee;
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

        public static async Task<Employee?> SelectEmployee(string employeeEmail)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Employee employee;
                string sqlCommand = $"SELECT * FROM employee WHERE email = '{employeeEmail}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    employee = new();

                    employee.Id = Convert.ToInt32(reader["id"]);
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
                    if (!reader.IsDBNull(reader.GetOrdinal("phone")))
                        employee.Phone = reader["phone"].ToString();

                    return employee;
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

        public static async Task<List<Employee>?> SelectEmployees(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                List<Employee> employees = new();
                Employee employee;

                string sqlCommand = $"SELECT * FROM employee WHERE id >= {employeeId} LIMIT {limit}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        employee = new();
                        
                        employee.Id = Convert.ToInt32(reader["id"]);
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
                        if (!reader.IsDBNull(reader.GetOrdinal("phone")))
                            employee.Phone = reader["phone"].ToString();


                        employee.Groups = (await SelectGroups(employee.Id))?.ToArray();
                        employee.Roles = (await SelectRoles(employee.Id))?.ToArray();

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

        public static async Task<ICollection<int>?> SelectGroupsByEmployee(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<int> groups = [];

                // ¬озвращает массив строк с id сотрудников
                string sqlCommand = $"SELECT groupId FROM employee_groups WHERE employeeId = {employeeId};";
                /*string sqlCommand = string.Format(
                    "SELECT employee_groups.employeeId AS employeeId " +
                    "FROM employee_groups " +
                    "JOIN `group` ON employee_groups.groupId = `group`.id " +
                    "WHERE `group`.id = {0}", groupId);*/

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("groupId")))
                            groups.Add(Convert.ToInt32(reader["groupId"]));                  
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

        public static async Task<ICollection<Employee>?> SelectEmployees()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Employee> employees = [];
                Employee employee;

                string sqlCommand = $"SELECT * FROM employee";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        employee = new();

                        employee.Id = Convert.ToInt32(reader["id"]);
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
                        if (!reader.IsDBNull(reader.GetOrdinal("phone")))
                            employee.Phone = reader["phone"].ToString();

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

        public static async Task<bool> SelectEmployeeGroup(int id)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM `employee_groups` WHERE id = '{id}';";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                    return true;

                return false;
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

        public static async Task<bool> SelectEmployeeRole(int employeeId, int roleId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM employee_roles WHERE employeeId = {employeeId} AND roleId = {roleId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                    return true;

                return false;
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

        public static async Task<Role?> SelectRole(string? roleName)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Role? role;
                string sqlCommand = $"SELECT * FROM role WHERE name = '{roleName}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    role = new();

                    role.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        role.Name = reader["name"].ToString();

                    return role;
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

        public static async Task<ICollection<Role>?> SelectRoles(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Role> roles = [];
                Role? role;
                string sqlCommand = string.Format(
                    "SELECT employee_roles.roleId, role.name " +
                    "FROM employee " +
                    "JOIN employee_roles ON employee.id = employee_roles.employeeId " +
                    "JOIN role ON employee_roles.roleId = role.id " +
                    "WHERE employee.id = {0}", employeeId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        role = new();

                        role.Id = Convert.ToInt32(reader["roleId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            role.Name = reader["name"].ToString();

                        roles.Add(role);
                    }

                    return roles;
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

        public static async Task<Group?> SelectGroup(int groupId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Group? group;
                string sqlCommand = $"SELECT * FROM `group` WHERE id = {groupId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    group = new();

                    group.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        group.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("active")))
                        group.Active = Convert.ToBoolean(reader["active"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("description")))
                        group.Description = reader["description"].ToString();

                    return group;
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

        public static async Task<ICollection<Group>?> SelectGroups(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Group> groups = [];
                Group? group;
                string sqlCommand = string.Format(
                    "SELECT employee_groups.groupId, `group`.name, `group`.active, `group`.description " +
                    "FROM employee " +
                    "JOIN employee_groups ON employee.id = employee_groups.employeeId " +
                    "JOIN `group` ON employee_groups.groupId = `group`.id " +
                    "WHERE employee.id = {0}", employeeId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        group = new();

                        group.Id = Convert.ToInt32(reader["groupId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            group.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            group.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            group.Description = reader["description"].ToString();

                        groups.Add(group);
                    }
                    return groups;
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

        public static async Task<ICollection<Group>?> SelectGroups()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Group> groups = [];
                Group? group;
                string sqlCommand = "SELECT * FROM `group`";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        group = new();

                        group.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            group.Name = reader["name"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            group.Active = Convert.ToBoolean(reader["active"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            group.Description = reader["description"].ToString();

                        //group.EmployeesId = await SelectEmployeesByGroup(group.Id);

                        groups.Add(group);
                    }
                    return groups;
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

        public static async Task<User?> SelectUser(string userEmail)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.active, user.expirationRefreshToken " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.email = '{0}'", userEmail);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    user = new();
                    user.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                        user.Email = reader["email"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("passwordHash")))
                        user.Password = reader["passwordHash"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("roleName")))
                        user.Role = reader["roleName"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("active")))
                        user.Active = Convert.ToBoolean(reader["active"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("expirationRefreshToken")))
                        user.TokenExpires = Convert.ToDateTime(reader["expirationRefreshToken"]);

                    return user;
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

        public static async Task<ICollection<User>?> SelectUsers()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<User> users = [];
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.active " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id");

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user = new();
                        user.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("email")))
                            user.Email = reader["email"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("passwordHash")))
                            user.Password = reader["passwordHash"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("roleName")))
                            user.Role = reader["roleName"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("active")))
                            user.Active = Convert.ToBoolean(reader["active"]);

                        users.Add(user);
                    }
                    return users;
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

        public static async Task<int?> SelectUserRole(string roleName)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = string.Format(
                    "SELECT * " +
                    "FROM user_role " +
                    "WHERE user_role.Name = '{0}'", roleName);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        return Convert.ToInt32(reader["id"]);
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

        public static async Task<string?> SelectUserRoles(string apiKey)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = string.Format(
                    "SELECT user_role.name AS roleName " +
                    "FROM aqba.user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.apiKey = '{0}'", apiKey);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    if (!await reader.IsDBNullAsync(reader.GetOrdinal("roleName")))
                        return reader["roleName"].ToString();
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

        public static async Task<User?> SelectUserByRefreshToken(string refreshToken)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.expirationRefreshToken, user.active " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.refreshToken = '{0}'", refreshToken);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    user = new();
                    user.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                        user.Email = reader["email"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("passwordHash")))
                        user.Password = reader["passwordHash"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("roleName")))
                        user.Role = reader["roleName"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("active")))
                        user.Active = Convert.ToBoolean(reader["active"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("expirationRefreshToken")))
                        user.TokenExpires = Convert.ToDateTime(reader["expirationRefreshToken"]);

                    return user;
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

        public static async Task<ICollection<Issue>?> SelectIssues(DateTime employeesUpdatedFrom, DateTime employeesUpdatedTo)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Issue> issues = [];
                Issue issue;

                string sqlCommand = string.Format(
                    "SELECT * FROM issue WHERE (employees_updated_at BETWEEN '{0}' AND '{1}');", 
                    employeesUpdatedFrom.ToString("yyyy-MM-dd HH:mm:ss"), employeesUpdatedTo.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        issue = new()
                        {
                            Status = new(),
                            Priority = new(),
                            Type = new(),
                            Company = new(),
                            Service_object = new(),

                            Id = Convert.ToInt32(reader["id"])
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("assignee_id")))
                            issue.Assignee_id = Convert.ToInt32(reader["assignee_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("author_id")))
                            issue.Author_id = Convert.ToInt32(reader["author_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("title")))
                            issue.Title = reader["title"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("employees_updated_at")))
                            issue.Employees_updated_at = Convert.ToDateTime(reader["employees_updated_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                            issue.Created_at = Convert.ToDateTime(reader["created_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("completed_at")))
                            issue.Completed_at = Convert.ToDateTime(reader["completed_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("deadline_at")))
                            issue.Deadline_at = Convert.ToDateTime(reader["deadline_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("delay_to")))
                            issue.Delay_to = Convert.ToDateTime(reader["delay_to"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("deleted_at")))
                            issue.Deleted_at = Convert.ToDateTime(reader["deleted_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("statusId")))
                            issue.Status.Id = Convert.ToInt32(reader["statusId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("typeId")))
                            issue.Type.Id = Convert.ToInt32(reader["typeId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityId")))
                            issue.Priority.Id = Convert.ToInt32(reader["priorityId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            issue.Company.Id = Convert.ToInt32(reader["companyId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("service_objectId")))
                            issue.Service_object.Id = Convert.ToInt32(reader["service_objectId"]);

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

        public static async Task<ICollection<Issue>?> SelectOpenAndCompletedIssues(DateTime dateFrom, DateTime datedTo, int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Issue> issues = [];
                Issue issue;

                string sqlCommand = string.Format(
                    "SELECT * " +
                    "FROM issue " +
                    "LEFT OUTER JOIN issue_status ON issue.statusId = issue_status.id  " +
                    "WHERE (issue_status.code != 'completed' AND issue_status.code != 'closed' OR issue.completed_at BETWEEN '{0}' AND '{1}')  " +
                    "AND issue.assignee_id = '{2}' AND issue.deleted_at IS NULL;",
                    dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), datedTo.ToString("yyyy-MM-dd HH:mm:ss"), employeeId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        issue = new()
                        {
                            Status = new(),
                            Priority = new(),
                            Type = new(),
                            Company = new(),
                            Service_object = new(),

                            Id = Convert.ToInt32(reader["id"])
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("assignee_id")))
                            issue.Assignee_id = Convert.ToInt32(reader["assignee_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("author_id")))
                            issue.Author_id = Convert.ToInt32(reader["author_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("title")))
                            issue.Title = reader["title"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("employees_updated_at")))
                            issue.Employees_updated_at = Convert.ToDateTime(reader["employees_updated_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                            issue.Created_at = Convert.ToDateTime(reader["created_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("completed_at")))
                            issue.Completed_at = Convert.ToDateTime(reader["completed_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("deadline_at")))
                            issue.Deadline_at = Convert.ToDateTime(reader["deadline_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("delay_to")))
                            issue.Delay_to = Convert.ToDateTime(reader["delay_to"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("deleted_at")))
                            issue.Deleted_at = Convert.ToDateTime(reader["deleted_at"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("statusId")))
                            issue.Status.Id = Convert.ToInt32(reader["statusId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("typeId")))
                            issue.Type.Id = Convert.ToInt32(reader["typeId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityId")))
                            issue.Priority.Id = Convert.ToInt32(reader["priorityId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            issue.Company.Id = Convert.ToInt32(reader["companyId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("service_objectId")))
                            issue.Service_object.Id = Convert.ToInt32(reader["service_objectId"]);

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

        public static async Task<int?> SelectCountCompletedOrClosedIssues(DateTime dateFrom, DateTime dateTo, int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();

                string sqlCommand = string.Format(
                    "SELECT COUNT(*) AS numberSolvedIssues " +
                    "FROM issue " +
                    "LEFT OUTER JOIN issue_status ON issue.statusId = issue_status.id " +
                    "WHERE (issue_status.code = 'completed' OR issue_status.code = 'closed') " +
                    "AND (issue.completed_at BETWEEN '{0}' AND '{1}') " +
                    "AND issue.assignee_id = '{2}' " +
                    "AND issue.deleted_at IS NULL;",
                    dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), dateTo.ToString("yyyy-MM-dd HH:mm:ss"), employeeId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("numberSolvedIssues")))
                            return Convert.ToInt32(reader["numberSolvedIssues"]);
                    }
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

        public static async Task<Issue?> SelectIssue(int issueId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Issue? issue = null;                
                string sqlCommand = $"SELECT * FROM issue WHERE id = '{issueId}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    issue = new();
                    issue.Status = new();
                    issue.Priority = new();
                    issue.Type = new();
                    issue.Company = new();
                    issue.Service_object = new();

                    issue.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("assignee_id")))
                        issue.Assignee_id = Convert.ToInt32(reader["assignee_id"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("author_id")))
                        issue.Author_id = Convert.ToInt32(reader["author_id"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("title")))
                        issue.Title = reader["title"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("employees_updated_at")))
                        issue.Employees_updated_at = Convert.ToDateTime(reader["employees_updated_at"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                        issue.Created_at = Convert.ToDateTime(reader["created_at"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("completed_at")))
                        issue.Completed_at = Convert.ToDateTime(reader["completed_at"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("deadline_at")))
                        issue.Deadline_at = Convert.ToDateTime(reader["deadline_at"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("delay_to")))
                        issue.Delay_to = Convert.ToDateTime(reader["delay_to"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("deleted_at")))
                        issue.Deleted_at = Convert.ToDateTime(reader["deleted_at"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("statusId")))
                        issue.Status.Id = Convert.ToInt32(reader["statusId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("typeId")))
                        issue.Type.Id = Convert.ToInt32(reader["typeId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityId")))
                        issue.Priority.Id = Convert.ToInt32(reader["priorityId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                        issue.Company.Id = Convert.ToInt32(reader["companyId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("service_objectId")))
                        issue.Service_object.Id = Convert.ToInt32(reader["service_objectId"]);
                }
                return issue;

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

        public static async Task<IssueType?> SelectType(IssueType? type)
        {
            if (type == null) return null;
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                IssueType tp;
                string sqlCommand = $"SELECT * FROM issue_type WHERE code = '{type.Code}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    tp = new();
                    tp.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        tp.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        tp.Code = reader["code"].ToString();                    
                    if (!reader.IsDBNull(reader.GetOrdinal("default")))
                        tp.Default = Convert.ToBoolean(reader["default"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("inner")))
                        tp.Inner = Convert.ToBoolean(reader["inner"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("available_for_client")))
                        tp.Available_for_client = Convert.ToBoolean(reader["available_for_client"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("type")))
                        tp.Type = reader["type"].ToString();

                    return tp;
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

        public static async Task<ICollection<IssueType>?> SelectTypes()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<IssueType> types = [];
                IssueType? type;
                string sqlCommand = "SELECT * FROM issue_type";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
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
                        if (!reader.IsDBNull(reader.GetOrdinal("default")))
                            type.Default = Convert.ToBoolean(reader["default"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("inner")))
                            type.Inner = Convert.ToBoolean(reader["inner"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("available_for_client")))
                            type.Available_for_client = Convert.ToBoolean(reader["available_for_client"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("type")))
                            type.Type = reader["type"].ToString();

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

        public static async Task<Status?> SelectIssueStatus(Status? status)
        {
            if (status == null) return null;
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Status st;
                string sqlCommand = $"SELECT * FROM issue_status WHERE code = '{status.Code}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    st = new();
                    st.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        st.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        st.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("color")))
                        st.Color = reader["color"].ToString();

                    return st;
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

        public static async Task<ICollection<Status>?> SelectIssueStatuses()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Status> statuses = [];
                Status? status;
                string sqlCommand = "SELECT * FROM issue_status";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
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
                        if (!reader.IsDBNull(reader.GetOrdinal("color")))
                            status.Color = reader["color"].ToString();

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

        public static async Task<Priority?> SelectIssuePriority(Priority? priority)
        {
            if (priority == null) return null;
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                Priority pt;
                string sqlCommand = $"SELECT * FROM issue_priority WHERE code = '{priority.Code}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    pt = new();
                    pt.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        pt.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        pt.Code = reader["code"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("position")))
                        pt.Position = Convert.ToInt32(reader["position"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("color")))
                        pt.Color = reader["color"].ToString();

                    return pt;
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
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<Priority> priorities = [];
                Priority? priority;
                string sqlCommand = "SELECT * FROM issue_priority";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
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
                        if (!reader.IsDBNull(reader.GetOrdinal("position")))
                            priority.Position = Convert.ToInt32(reader["position"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("color")))
                            priority.Color = reader["color"].ToString();

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

        public static async Task<TimeEntry?> SelectTimeEntry(int timeEntryId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                TimeEntry? timeEntry = null;
                string sqlCommand = $"SELECT * FROM time_entry WHERE id = {timeEntryId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    timeEntry = new();
                    timeEntry.Employee = new();

                    timeEntry.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                        timeEntry.Employee.Id = Convert.ToInt32(reader["employeeId"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("spentTime")))
                        timeEntry.Spent_Time = Convert.ToDouble(reader["spentTime"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("issueId")))
                        timeEntry.Issue_id = Convert.ToInt32(reader["issueId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("logged_at")))
                        timeEntry.Logged_At = Convert.ToDateTime(reader["logged_at"]);

                }
                return timeEntry;
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

        public static async Task<ICollection<TimeEntry>?> SelectTimeEntries(DateTime? dateFrom, DateTime? dateTo)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<TimeEntry> timeEntries = [];
                TimeEntry? timeEntry = null;
                string sqlCommand = string.Format(
                    "SELECT * " +
                    "FROM time_entry " +
                    "WHERE logged_at BETWEEN '{0}' AND '{1}'",
                    dateFrom?.ToString("yyyy-MM-dd HH:mm:ss"), dateTo?.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while(await reader.ReadAsync())
                    {
                        timeEntry = new();
                        timeEntry.Employee = new();

                        timeEntry.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                            timeEntry.Employee.Id = Convert.ToInt32(reader["employeeId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("spentTime")))
                            timeEntry.Spent_Time = Convert.ToDouble(reader["spentTime"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("issueId")))
                            timeEntry.Issue_id = Convert.ToInt32(reader["issueId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("logged_at")))
                            timeEntry.Logged_At = Convert.ToDateTime(reader["logged_at"]);

                        timeEntries.Add(timeEntry);
                    }
                    return timeEntries;
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

        public static async Task<double?> SelectTimeEntry(DateTime dateFrom, DateTime dateTo, int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = string.Format(
                    "SELECT SUM(spentTime) AS spentTime " +
                    "FROM time_entry " +
                    "WHERE employeeId = {2} AND (logged_at BETWEEN '{0}' AND '{1}')",
                    dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), dateTo.ToString("yyyy-MM-dd HH:mm:ss"), employeeId);

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    if (!reader.IsDBNull(reader.GetOrdinal("spentTime")))
                        return Convert.ToDouble(reader["spentTime"]);
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

        public static async Task<ICollection<TimeEntry>?> SelectTimeEntriesByIssueId(int issueId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                ICollection<TimeEntry> timeEntries = [];
                TimeEntry? timeEntry = null;
                string sqlCommand = $"SELECT * FROM time_entry WHERE issueId = {issueId}";

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
                        if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                            timeEntry.Employee.Id = Convert.ToInt32(reader["employeeId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("spentTime")))
                            timeEntry.Spent_Time = Convert.ToDouble(reader["spentTime"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("issueId")))
                            timeEntry.Issue_id = Convert.ToInt32(reader["issueId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("logged_at")))
                            timeEntry.Logged_At = Convert.ToDateTime(reader["logged_at"]);

                        timeEntries.Add(timeEntry);
                    }
                    return timeEntries;
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
    }
}