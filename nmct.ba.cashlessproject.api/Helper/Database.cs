using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Helper
{
    public class Database
    {
        #region "connections"
        public static ConnectionStringSettings CreateConnectionString(string provider, string server, string database, string username, string password)
        {
            ConnectionStringSettings settings = new ConnectionStringSettings();
            settings.ProviderName = provider;
            settings.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password; //cashless.app vervangen door db
            return settings;
        }

        public static DbConnection GetConnection(string ConnectionString)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionString];
            return GetConnection(settings);
        }

        public static DbConnection GetOtherConnection(string ConnectionString)
        {
            DbConnection setting = new SqlConnection(ConnectionString);
            return setting;
        }

        public static DbConnection GetConnection(ConnectionStringSettings Settings)
        {
            DbConnection con = DbProviderFactories.GetFactory(Settings.ProviderName).CreateConnection();
            con.ConnectionString = Settings.ConnectionString;
            con.Open();

            return con;
        }

        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
                con.Close();
        }
        #endregion

        #region "build commands"
        private static DbCommand BuildCommand(DbConnection conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = conName.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sqlQuery;

            if (sqlParams != null)
                command.Parameters.AddRange(sqlParams);

            return command;
        }

        private static DbCommand BuildCommand(DbTransaction transaction, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = transaction.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sqlQuery;
            command.Transaction = transaction; //linken

            if (sqlParams != null)
                command.Parameters.AddRange(sqlParams);

            return command;
        }

        private static DbCommand BuildCommand(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = GetConnection(ConnectionString).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }
        #endregion

        # region "data aanschrijven (get/modify)"
        public static DbDataReader GetData(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(ConnectionString, sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null)
                    reader.Close();
                if (command != null)
                    ReleaseConnection(command.Connection);
                throw;
            }
        }

        public static DbDataReader GetData(DbConnection conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(conName, sqlQuery, sqlParams);

                DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }
        }

        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int ModifyData(DbConnection conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(conName, sqlQuery, sqlParams);
                int aantalRijen = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return aantalRijen;
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                return 0;
            }
        }

        public static int ModifyData(DbTransaction transaction, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(transaction, sqlQuery, sqlParams);
                command.Transaction = transaction;

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }
        }

        //insert
        public static int InsertData(DbConnection conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(conName, sqlQuery, sqlParams);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT @@IDENTITY";
                int identity = Convert.ToInt32(command.ExecuteScalar());

                ReleaseConnection(command.Connection);

                return identity;
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                return 0;
            }
        }

        public static int InsertData(DbTransaction transaction, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(transaction, sqlQuery, sqlParams);
                command.Transaction = transaction;
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT @@IDENTITY";
                //int identity = Convert.ToInt32(command.ExecuteScalar());

                return 1;
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }
        }

        public static int InsertData(string ConnectionString, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(ConnectionString, sql, parameters);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT @@IDENTITY";

                int identity = Convert.ToInt32(command.ExecuteScalar());
                command.Connection.Close();

                return identity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                    ReleaseConnection(command.Connection);
                return 0;
            }
        }

        public static int UpdateData(DbConnection conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(conName, sqlQuery, sqlParams);

                int i = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return i;
                
            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }
        }

        public static int UpdateData(string conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(conName, sqlQuery, sqlParams);

                int i = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return i;

            }
            catch (Exception)
            {
                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }
        }

        public static DbParameter AddParam(string conName, string name, object value)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conName];

            DbParameter dbParam = DbProviderFactories.GetFactory(settings.ProviderName).CreateParameter();
            dbParam.ParameterName = name;
            dbParam.Value = value;

            return dbParam;
        }

        public static DbParameter AddParam(SqlConnection conName, string name, object value)
        {
            DbConnection settings = conName;

            DbParameter dbParam = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateParameter();
            dbParam.ParameterName = name;
            dbParam.Value = value;

            return dbParam;
        }

        public static DbTransaction BeginTransaction(string ConnectionString)
        {
            DbConnection con = null;
            try
            {
                con = GetConnection(ConnectionString);          
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);
                throw;
            }
        }

        public static DbTransaction BeginTransaction(SqlConnection ConnectionString)
        {
            DbConnection con = null;
            try
            {
                con = ConnectionString;
                con.Open();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);
                throw;
            }
        }

        public static DbTransaction BeginTransaction(ConnectionStringSettings Setting)
        {
            DbConnection con = null;
            try
            {
                con = GetConnection(Setting);
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);

                throw;
            }
        }

        /*public static DbTransaction BuildTransaction(string conName, string sqlQuery, params DbParameter[] sqlParams)
        {
            DbConnection con = null;

            try
            {
                con = GetConnection(conName);
                return con.BeginTransaction(); //returned dus een DbTransaction
            }
            catch (Exception)
            {
                ReleaseConnection(con);

                throw;
            }
        }*/
        #endregion
    }
}