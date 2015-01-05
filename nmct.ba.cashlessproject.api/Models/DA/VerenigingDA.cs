using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Hosting;

namespace nmct.ba.cashlessproject.api.Models.DA
{
    public class VerenigingDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;
            string id = claims.FirstOrDefault(c => c.Type == "id").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"MichelleToshiba", dbname, dblogin, Cryptography.Decrypt(dbpass));
        }

        public static Vereniging CheckCredentials(string username, string password)
        {
            string sql = "SELECT Id, Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone FROM Organisation WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@Login", username);
            DbParameter par2 = Database.AddParam(CONNECTIONSTRING, "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2);
                reader.Read();

                Vereniging vereniging = Create(reader);

                return vereniging;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static int UpdatePassword(IEnumerable<Claim> claims, Vereniging ver)
        {
            try
            {
                ver.Id = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "id").Value);

                string sql = "UPDATE Organisation SET Password=@Password WHERE Id=@Id";

                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", ver.Id);
                DbParameter pwdParam = Database.AddParam(CONNECTIONSTRING, "@Password", Cryptography.Encrypt(ver.Password));

                int lastInsertedID = Database.UpdateData(CONNECTIONSTRING, sql, idParam, pwdParam);
                return lastInsertedID;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int InsertVereniging(Vereniging ver)
        {
            string sql = "INSERT INTO Organisation VALUES(@Login,@Password,@DbName,@DbLogin,@DbPassword,@OrganisationName,@Address,@Email,@Phone)";

            DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@Login", ver.Login);
            DbParameter par2 = Database.AddParam(CONNECTIONSTRING, "@Password", Cryptography.Encrypt(ver.Password));
            DbParameter par3 = Database.AddParam(CONNECTIONSTRING, "@DbName", ver.DbName);
            DbParameter par4 = Database.AddParam(CONNECTIONSTRING, "@DbLogin", ver.DbLogin);
            DbParameter par5 = Database.AddParam(CONNECTIONSTRING, "@DbPassword", Cryptography.Encrypt(ver.DbPassword));
            DbParameter par6 = Database.AddParam(CONNECTIONSTRING, "@OrganisationName", ver.OrganisationName);
            DbParameter par7 = Database.AddParam(CONNECTIONSTRING, "@Address", ver.Address);
            DbParameter par8 = Database.AddParam(CONNECTIONSTRING, "@Email", ver.Email);
            DbParameter par9 = Database.AddParam(CONNECTIONSTRING, "@Phone", ver.Phone);

            int id = Database.InsertData(Database.GetConnection(CONNECTIONSTRING), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);

            CreateDatabase(ver);

            return id;
        }

        public static Vereniging GetOrganisationById(int verenigingId)
        {
            string sql = "SELECT Id, Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone FROM Organisation WHERE Id=@Id";
            DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@Id", verenigingId);

            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, par1);
                reader.Read();

                Vereniging vereniging = Create(reader);

                return vereniging;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static int UpdateVereniging(Vereniging ver)
        {
            try
            {
                string sql = "UPDATE Organisation SET OrganisationName=@OrganisationName, Address=@Address, Email=@Email, Phone=@Phone WHERE Id=@Id";

                DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@OrganisationName", ver.OrganisationName);
                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", ver.Id);
                DbParameter addressParam = Database.AddParam(CONNECTIONSTRING, "@Address", ver.Address);
                DbParameter emailParam = Database.AddParam(CONNECTIONSTRING, "@Email", ver.Email);
                DbParameter phoneParam = Database.AddParam(CONNECTIONSTRING, "@Phone", ver.Phone);

                int lastInsertedID = Database.UpdateData(Database.GetConnection(CONNECTIONSTRING), sql, nameParam, idParam, addressParam, emailParam, phoneParam);
                return lastInsertedID;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void CreateDatabase(Vereniging vereniging)
        {
            // create the actual database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); //only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", vereniging.DbName).Replace("@@DbLogin",vereniging.DbLogin).Replace("@@DbPassword", vereniging.DbPassword);
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection(CONNECTIONSTRING), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction(CONNECTIONSTRING);

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", vereniging.DbName).Replace("@@DbLogin", vereniging.DbLogin).Replace("@@DbPassword", vereniging.DbPassword);

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }

         public static List<Vereniging> GetOrganisations()
        {
            string sql = "SELECT ID, Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Address, Email, Phone FROM Organisation";
            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql);

            List<Vereniging> verenigingen = new List<Vereniging>();
            while (reader.Read())
            {
                Vereniging ver = Create(reader);
                verenigingen.Add(ver);
            }

            reader.Close();

            return verenigingen;
        }

        private static Vereniging Create(IDataRecord record)
        {
            return new Vereniging()
            {
                Id = Int32.Parse(record["ID"].ToString()),
                Login = record["Login"].ToString(),
                Password = record["Password"].ToString(),
                DbName = record["DbName"].ToString(),
                DbLogin = record["DbLogin"].ToString(),
                DbPassword = record["DbPassword"].ToString(),
                OrganisationName = record["OrganisationName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }
    }
}