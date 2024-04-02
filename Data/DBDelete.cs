using AqbaServer.Helper;
using AqbaServer.Models.OkdeskPerformance;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace AqbaServer.Data
{
    public class DBDelete
    {
        public static async Task<bool> DeleteCategory(int categoryId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM company_category WHERE id = {categoryId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM company WHERE id = {companyId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteManufacturer(string manufacturerCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM manufacturer WHERE code = '{manufacturerCode}'";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteKind(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind WHERE id = {kindId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteKindParameter(int kindParameterId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kinds_parameters WHERE id = {kindParameterId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };
                using DbDataReader reader = cmd.ExecuteReader();
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

        public static async Task<bool> DeleteKindParamByKind(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind_param WHERE kindId = {kindId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteKindParamByKindParam(int kindParamId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind_param WHERE kindParamId = {kindParamId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteCoordinate(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM coordinates WHERE maintenanceEntitiesId = {maintenanceEntityId}";

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

        public static async Task<bool> DeleteModel(int modelId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM model WHERE id = {modelId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM maintenance_entity WHERE id = {maintenanceEntityId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteEquipmentParameter(int equipmentaParameterId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM parameter WHERE id = {equipmentaParameterId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteEquipmentParameterByEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM parameter WHERE equipmentId = {equipmentId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        public static async Task<bool> DeleteEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM equipment WHERE id = {equipmentId}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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

        internal static async Task<bool> DeleteUser(int id)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM user WHERE id = {id}";

                MySqlCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = sqlCommand
                };

                using DbDataReader reader = cmd.ExecuteReader();

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