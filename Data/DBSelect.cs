using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace AqbaServer.Data
{
    public class DBSelect
    {
        static readonly int limit = 100;

        public static async Task<ICollection<Company>?> SelectCompanies(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Company> companies = [];
                Company company;

                string sqlCommand = $"SELECT * FROM company WHERE id >= {companyId} LIMIT {limit}";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Company>?> SelectCompaniesByCategory(int categoryId, int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Company> Companies = [];
                Company company;
                string sqlCommand = string.Format(
                    "SELECT company.id, company.name, company.additional_name, company.active, company.categoryId, company_category.color " +
                    "FROM company " +
                    "JOIN company_category ON company.categoryId = company_category.id " +
                    "WHERE categoryId = {0} AND company.id >= {1} LIMIT {2}", categoryId, companyId, limit);

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Company?> SelectCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Company company;
                string sqlCommand = string.Format(
                    "SELECT company.id, company.name, company.additional_name, company.active, company.categoryId, company_category.color " +
                    "FROM company " +
                    "JOIN company_category ON company.categoryId = company_category.id " +
                    "WHERE company.id = {0}", companyId);

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<int?> SelectLastCompany()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM company ORDER BY id DESC LIMIT 1";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<MaintenanceEntity>> SelectMaintenanceEntities(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<MaintenanceEntity> maintenanceEntities = new List<MaintenanceEntity>();
                MaintenanceEntity maintenanceEntity;
                string sqlCommand = $"SELECT * FROM maintenance_entity WHERE id >= {maintenanceEntityId} LIMIT {limit}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<MaintenanceEntity> SelectMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                MaintenanceEntity maintenanceEntity;
                string sqlCommand = $"SELECT * FROM maintenance_entity WHERE id = {maintenanceEntityId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<int?> SelectLastMaintenanceEntity()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM maintenance_entity ORDER BY id DESC LIMIT 1";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Equipment>?> SelectEquipments()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;
                string sqlCommand = $"SELECT * FROM equipment";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Equipment>> SelectEquipments(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Equipment> equipments = new List<Equipment>();
                Equipment equipment;
                string sqlCommand = $"SELECT * FROM equipment WHERE id >= {equipmentId} LIMIT {limit}";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Equipment?> SelectEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
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

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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

                    equipment.Parameters = await SelectEquipmentParameters(equipment.Id);
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<int?> SelectLastEquipment()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = "SELECT id FROM equipment ORDER BY id DESC LIMIT 1";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Equipment>?> SelectEquipmentsByMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
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


                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                    equip.Parameters = await SelectEquipmentParameters(equip.Id);
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Equipment>?> SelectEquipmentsByCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
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


                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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

                        /*equipment.Model.EquipmentKind = equipment.Kind;
                        equipment.Model.EquipmentManufacturer = equipment.Manufacturer;*/

                        equipments.Add(equipment);
                    }
                }

                foreach (var equip in equipments)
                {
                    equip.Parameters = await SelectEquipmentParameters(equip.Id);
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Kind>> SelectKinds()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Kind> kinds = new List<Kind>();
                Kind kind;
                string sqlCommand = "SELECT * FROM kind";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Kind> SelectKind(string kindCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Kind kind;
                string sqlCommand = $"SELECT * FROM kind WHERE code = '{kindCode}'";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<KindParameter>> SelectKindParameters()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<KindParameter> kindParameters = new List<KindParameter>();
                KindParameter parameter;
                string sqlCommand = "SELECT * FROM kinds_parameters";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<KindParameter>> SelectKindParameters(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<KindParameter> kindParameters = new List<KindParameter>();
                KindParameter parameter;
                string sqlCommand = string.Format(
                    "SELECT kinds_parameters.id, kinds_parameters.code, kinds_parameters.name, kinds_parameters.fieldType " +
                    "FROM kind " +
                    "JOIN kind_param ON kind.id = kind_param.kindId " +
                    "JOIN kinds_parameters ON kind_param.kindParamId = kinds_parameters.id " +
                    "WHERE kind.id = {0}", kindId);

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<KindParameter> SelectKindParameter(string kindParameterCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                KindParameter parameter;
                string sqlCommand = $"SELECT * FROM kinds_parameters WHERE code = '{kindParameterCode}'";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    parameter = new();
                    parameter.Id = Convert.ToInt32(reader["id"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        parameter.Name = reader["name"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("code")))
                        parameter.Name = reader["code"].ToString();
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Manufacturer>> SelectManufacturers()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Manufacturer> manufacturers = new List<Manufacturer>();
                Manufacturer manufacturer;
                string sqlCommand = "SELECT * FROM manufacturer";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Manufacturer> SelectManufacturer(string manufacturerCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Manufacturer manufacturer;
                string sqlCommand = $"SELECT * FROM manufacturer WHERE code = '{manufacturerCode}'";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Model>> SelectModels()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Model> models = new List<Model>();
                Model model;
                string sqlCommand = "SELECT * FROM model";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Model> SelectModel(string modelCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Model model;
                string sqlCommand = $"SELECT * FROM model WHERE code = '{modelCode}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<Category>> SelectCategories()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<Category> categories = new List<Category>();
                Category category;
                string sqlCommand = "SELECT * FROM company_category";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        category = new()
                        {
                            Id = Convert.ToInt32(reader["id"].ToString()),
                            Color = reader["color"].ToString()
                        };
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Category> SelectCategory(int categoryId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Category category;
                string sqlCommand = $"SELECT * FROM company_category WHERE id = {categoryId}";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    category = new()
                    {
                        Id = Convert.ToInt32(reader["id"].ToString()),
                        Color = reader["color"].ToString()
                    };
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<ICollection<EquipmentParameter>> SelectEquipmentParameters()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                ICollection<EquipmentParameter> equipmentParameters = new List<EquipmentParameter>();
                EquipmentParameter parameter;
                string sqlCommand = $"SELECT * FROM parameter";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<EquipmentParameter> SelectEquipmentParameter(int equipmentId, int kindParamId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                EquipmentParameter parameter;
                string sqlCommand = $"SELECT * FROM parameter WHERE equipmentId = {equipmentId} AND kindParameterId = {kindParamId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<EquipmentParameter>> SelectEquipmentParameters(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<EquipmentParameter> equipmentParameters = new();
                EquipmentParameter parameter;
                /*string sqlCommand = $"SELECT * FROM parameter WHERE equipmentId = {equipmentId}";*/

                string sqlCommand = string.Format
                    ("SELECT parameter.id, parameter.value, " +
                    "kinds_parameters.name, kinds_parameters.code, kinds_parameters.fieldType " +
                    "FROM parameter " +
                    "JOIN kinds_parameters ON parameter.kindParameterId = kinds_parameters.id " +
                    "WHERE equipmentId = {0}", equipmentId);

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> SelectKindParam(int kindId, int kindParamId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM kind_param WHERE kindId = {kindId} AND kindParamId = {kindParamId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Employee?> SelectEmployee(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Employee employee;
                string sqlCommand = $"SELECT * FROM employee WHERE id = {employeeId}";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Employee?> SelectEmployee(string employeeEmail)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Employee employee;
                string sqlCommand = $"SELECT * FROM employee WHERE email = '{employeeEmail}'";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Employee>?> SelectEmployees(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Employee> employees = new();
                Employee employee;

                string sqlCommand = $"SELECT * FROM employee WHERE id >= {employeeId} LIMIT {limit}";               

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<int[]?> SelectEmployeesByGroup(int groupId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<int> employees = [];
                // Возвращает массив строк с id сотрудников
                string sqlCommand = string.Format(
                    "SELECT employee_groups.employeeId AS employeeId " +
                    "FROM employee_groups " +
                    "JOIN `group` ON employee_groups.groupId = `group`.id " +
                    "WHERE `group`.id = {0}", groupId);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                            employees.Add(Convert.ToInt32(reader["employeeId"]));                  
                    }
                }
                return employees.ToArray();
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Employee>?> SelectEmployees()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Employee> employees = new();
                Employee employee;

                string sqlCommand = $"SELECT * FROM employee";

                // Создать объект Command.
                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }        

        public static async Task<bool> SelectEmployeeGroup(int employeeId, int groupId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM employee_groups WHERE employeeId = {employeeId} AND groupId = {groupId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> SelectEmployeeRole(int employeeId, int roleId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"SELECT * FROM employee_roles WHERE employeeId = {employeeId} AND roleId = {roleId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Role?> SelectRole(string? roleName)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Role? role;
                string sqlCommand = $"SELECT * FROM role WHERE name = '{roleName}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Role>?> SelectRoles(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Role> roles = [];
                Role? role;
                string sqlCommand = string.Format(
                    "SELECT employee_roles.roleId, role.name " +
                    "FROM employee " +
                    "JOIN employee_roles ON employee.id = employee_roles.employeeId " +
                    "JOIN role ON employee_roles.roleId = role.id " +
                    "WHERE employee.id = {0}", employeeId);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Group?> SelectGroup(int groupId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Group? group;
                string sqlCommand = $"SELECT * FROM `group` WHERE id = {groupId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Group>?> SelectGroups(int employeeId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Group> groups = [];
                Group? group;
                string sqlCommand = string.Format(
                    "SELECT employee_groups.groupId, `group`.name, `group`.active, `group`.description " +
                    "FROM employee " +
                    "JOIN employee_groups ON employee.id = employee_groups.employeeId " +
                    "JOIN `group` ON employee_groups.groupId = `group`.id " +
                    "WHERE employee.id = {0}", employeeId);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Group>?> SelectGroups()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Group> groups = [];
                Group? group;
                string sqlCommand = "SELECT * FROM `group`";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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

                        group.EmployeesId = await SelectEmployeesByGroup(group.Id);

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<EmployeeDto?> SelectEmployeePerformance(int employeeId, DateTime date)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                EmployeeDto employee = new();
                string sqlCommand = 
                    $"SELECT employeeId, solvedTasks, spentedTime FROM `employee_performance` WHERE employeeId = {employeeId} AND `date` = '{date:yyyy-MM-dd}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    employee.Id = Convert.ToInt32(reader["employeeId"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("solvedTasks")))
                        employee.SolvedTasks = Convert.ToInt32(reader["solvedTasks"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("spentedTime")))
                        employee.SpentedTime = Convert.ToDouble(reader["spentedTime"]);

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<EmployeeDto>?> SelectEmployeesPerformance(DateTime dateFrom, DateTime dateTo)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<EmployeeDto> employees = [];
                EmployeeDto employee;
                string sqlCommand = string.Format(
                    "SELECT employeeId, SUM(solvedTasks) AS solvedTasks, SUM(spentedTime) AS spentedTime " +
                    "FROM `employee_performance` " +
                    "WHERE `date` between '{0}' and '{1}' GROUP BY employeeId ", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        employee = new();
                        employee.Id = Convert.ToInt32(reader["employeeId"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("solvedTasks")))
                            employee.SolvedTasks = Convert.ToInt32(reader["solvedTasks"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("spentedTime")))
                            employee.SpentedTime = Convert.ToDouble(reader["spentedTime"]);

                        employees.Add(employee);
                    }
                    return employees;
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<User?> SelectUser(string userEmail)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.active, user.expirationRefreshToken " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.email = '{0}'", userEmail);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<User>?> SelectUsers()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<User> users = [];
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.active " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id");

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<int?> SelectUserRole(string roleName)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = string.Format(
                    "SELECT * " +
                    "FROM user_role " +
                    "WHERE user_role.Name = '{0}'", roleName);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<string?> SelectUserRoles(string apiKey)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = string.Format(
                    "SELECT user_role.name AS roleName " +
                    "FROM aqba.user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.apiKey = '{0}'", apiKey);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<User?> SelectUserByRefreshToken(string refreshToken)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                User user;
                string sqlCommand = string.Format(
                    "SELECT user.id, user.email, user.passwordHash, user_role.Name AS roleName, user.expirationRefreshToken, user.active " +
                    "FROM user " +
                    "JOIN user_role ON user.roleId = user_role.id " +
                    "WHERE user.refreshToken = '{0}'", refreshToken);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<int>?> SelectIssues(int statusIdNot)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<int>? issues = [];
                string sqlCommand = string.Format( "SELECT id FROM issue WHERE statusId != {0}", statusIdNot);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        issues.Add( Convert.ToInt32(reader["id"]) );
                    }
                    return issues;
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Issue>?> SelectIssues(bool unknownIssues = false)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Issue> issues = [];
                Issue issue;
                string sqlCommand = 
                    "SELECT issue.id, issue.assignee_id, issue.author_id, issue.title, issue.internal_status, issue.created_at, issue.completed_at, " +
                    "issue.deadline_at, issue.delay_to, issue.deleted_at, issue.companyId, issue.service_objectId, " +
                    "issue_priority.id AS priorityId, issue_priority.name AS priorityName, " +
                    "issue_priority.code AS priorityCode, issue_priority.position AS priorityPosition, issue_priority.color AS priorityColor, " +
                    "issue_type.id AS typeId, issue_type.name AS typeName, issue_type.code AS typeCode, issue_type.default AS typeDefault, " +
                    "issue_type.inner AS typeInner, issue_type.type AS typeType, issue_type.available_for_client AS typeAvailable, " +
                    "issue_status.id AS statusId, issue_status.code AS statusCode, issue_status.name AS statusName, issue_status.color AS statusColor " +
                    "FROM issue " +
                    "JOIN issue_priority ON issue.priorityId = issue_priority.id " +
                    "JOIN issue_type ON issue.typeId = issue_type.id " +
                    "JOIN issue_status ON issue.statusId = issue_status.id " +
                    "WHERE issue_status.Code != 'closed'";

                if (unknownIssues) sqlCommand += " AND (issue.internal_status = 'unknown' OR issue.internal_status IS NULL)";
                else sqlCommand += " AND issue.internal_status IS NULL";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = await cmd.ExecuteReaderAsync();

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

                        issue.Id = Convert.ToInt32(reader["id"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("assignee_id")))
                            issue.Assignee_id = Convert.ToInt32(reader["assignee_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("author_id")))
                            issue.Author_id = Convert.ToInt32(reader["author_id"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("title")))
                            issue.Title = reader["title"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("internal_status")))
                            issue.Title = reader["internal_status"].ToString();
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
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityId")))
                            issue.Priority.Id = Convert.ToInt32(reader["priorityId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityName")))
                            issue.Priority.Name = reader["priorityName"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityCode")))
                            issue.Priority.Code = reader["priorityCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityPosition")))
                            issue.Priority.Position = Convert.ToInt32(reader["priorityPosition"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("priorityColor")))
                            issue.Priority.Color = reader["priorityColor"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("typeId")))
                            issue.Type.Id = Convert.ToInt32(reader["typeId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("typeName")))
                            issue.Type.Name = reader["typeName"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("typeCode")))
                            issue.Type.Code = reader["typeCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("typeDefault")))
                            issue.Type.Default = Convert.ToBoolean(reader["typeDefault"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("typeInner")))
                            issue.Type.Inner = Convert.ToBoolean(reader["typeInner"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("typeType")))
                            issue.Type.Type = reader["typeType"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("typeAvailable")))
                            issue.Type.Available_for_client = Convert.ToBoolean(reader["typeAvailable"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("statusId")))
                            issue.Status.Id = Convert.ToInt32(reader["statusId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("statusCode")))
                            issue.Status.Code = reader["statusCode"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("statusName")))
                            issue.Status.Name = reader["statusName"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("statusColor")))
                            issue.Status.Color = reader["statusColor"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                            issue.Company.Id = Convert.ToInt32(reader["companyId"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("service_objectId")))
                            issue.Service_object.Id = Convert.ToInt32(reader["service_objectId"].ToString());

                        issues.Add(issue);
                    }
                        return issues;
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Issue?> SelectIssue(int issueId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Issue issue;
                string sqlCommand = string.Format(
                    "SELECT issue.id, issue.assignee_id, issue.author_id, issue.title, issue.created_at, issue.completed_at, issue.deadline_at, issue.delay_to, " +
                    "issue.deleted_at, issue.companyId, issue.service_objectId, " +
                    "issue_priority.id AS priorityId, issue_priority.name AS priorityName, " +
                    "issue_priority.code AS priorityCode, issue_priority.position AS priorityPosition, issue_priority.color AS priorityColor, " +
                    "issue_type.id AS typeId, issue_type.name AS typeName, issue_type.code AS typeCode, issue_type.default AS typeDefault, " +
                    "issue_type.inner AS typeInner, issue_type.type AS typeType, issue_type.available_for_client AS typeAvailable, " +
                    "issue_status.id AS statusId, issue_status.code AS statusCode, issue_status.name AS statusName, issue_status.color AS statusColor " +
                    "FROM issue " +
                    "JOIN issue_priority ON issue.priorityId = issue_priority.id " +
                    "JOIN issue_type ON issue.typeId = issue_type.id " +
                    "JOIN issue_status ON issue.statusId = issue_status.id " +
                    "WHERE issue.id = '{0}'", issueId);

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityId")))
                        issue.Priority.Id = Convert.ToInt32(reader["priorityId"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityName")))
                        issue.Priority.Name = reader["priorityName"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityCode")))
                        issue.Priority.Code = reader["priorityCode"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityPosition")))
                        issue.Priority.Position = Convert.ToInt32(reader["priorityPosition"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("priorityColor")))
                        issue.Priority.Color = reader["priorityColor"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("typeId")))
                        issue.Type.Id = Convert.ToInt32(reader["typeId"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("typeName")))
                        issue.Type.Name = reader["typeName"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("typeCode")))
                        issue.Type.Code = reader["typeCode"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("typeDefault")))
                        issue.Type.Default = Convert.ToBoolean(reader["typeDefault"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("typeInner")))
                        issue.Type.Inner = Convert.ToBoolean(reader["typeInner"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("typeType")))
                        issue.Type.Type = reader["typeType"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("typeAvailable")))
                        issue.Type.Available_for_client = Convert.ToBoolean(reader["typeAvailable"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("statusId")))
                        issue.Status.Id = Convert.ToInt32(reader["statusId"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("statusCode")))
                        issue.Status.Code = reader["statusCode"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("statusName")))
                        issue.Status.Name = reader["statusName"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("statusColor")))
                        issue.Status.Color = reader["statusColor"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("companyId")))
                        issue.Company.Id = Convert.ToInt32(reader["companyId"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("service_objectId")))
                        issue.Service_object.Id = Convert.ToInt32(reader["service_objectId"].ToString());

                    return issue;
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<TaskType?> SelectType(TaskType type)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                TaskType tp;
                string sqlCommand = $"SELECT * FROM issue_type WHERE code = '{type.Code}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<TaskType>?> SelectTypes()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<TaskType> types = [];
                TaskType? type;
                string sqlCommand = "SELECT * FROM issue_type";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Status?> SelectIssueStatus(Status status)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Status st;
                string sqlCommand = $"SELECT * FROM issue_status WHERE code = '{status.Code}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Status>?> SelectIssueStatuses()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Status> statuses = [];
                Status? status;
                string sqlCommand = "SELECT * FROM issue_status";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<Priority?> SelectIssuePriority(Priority priority)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                Priority pt;
                string sqlCommand = $"SELECT * FROM issue_priority WHERE code = '{priority.Code}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                        pt.Position = Convert.ToInt32(reader["position"].ToString());
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<List<Priority>?> SelectIssuePriorities()
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                List<Priority> priorities = [];
                Priority? priority;
                string sqlCommand = "SELECT * FROM issue_priority";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

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
                            priority.Position = Convert.ToInt32(reader["position"].ToString());
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
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }
}