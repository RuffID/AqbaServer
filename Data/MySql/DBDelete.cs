using AqbaServer.Helper;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace AqbaServer.Data.MySql
{
    public class DBDelete
    {
        public static async Task<bool> DeleteCategory(string categoryCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM company_category WHERE code = '{categoryCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteCompany(int companyId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM company WHERE id = {companyId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteManufacturer(string manufacturerCode)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM manufacturer WHERE code = '{manufacturerCode}'";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteKind(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind WHERE id = {kindId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteKindParameter(int kindParameterId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kinds_parameters WHERE id = {kindParameterId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteKindParamByKind(int kindId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind_param WHERE kindId = {kindId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteKindParamByKindParam(int kindParamId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM kind_param WHERE kindParamId = {kindParamId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteModel(int modelId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM model WHERE id = {modelId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM maintenance_entity WHERE id = {maintenanceEntityId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteEquipmentParameter(int equipmentaParameterId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM parameter WHERE id = {equipmentaParameterId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteEquipmentParameterByEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM parameter WHERE equipmentId = {equipmentId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteEquipment(int equipmentId)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM equipment WHERE id = {equipmentId}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteUser(int id)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM user WHERE id = {id}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }

        public static async Task<bool> DeleteTimeEntry(int id)
        {
            MySqlConnection connection = DBConfig.GetDBConnection();
            MySqlCommand cmd = connection.CreateCommand();
            try
            {
                await connection.OpenAsync();
                string sqlCommand = $"DELETE FROM time_entry WHERE id = {id}";

                cmd.Connection = connection;
                cmd.CommandText = sqlCommand;

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
                await cmd.DisposeAsync();
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }
}