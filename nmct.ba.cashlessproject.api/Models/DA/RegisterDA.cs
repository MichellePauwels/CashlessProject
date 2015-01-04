using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models.DA
{
    public class RegisterDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;
            string id = claims.FirstOrDefault(c => c.Type == "id").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"MichelleToshiba", dbname, dblogin, dbpass);
        }

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, RegisterName, Device, PurchaseDate, ExpiresDate FROM Registers";
                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

                List<Register> registers = new List<Register>();
                while (reader.Read())
                {
                    Register reg = Create(reader);
                    registers.Add(reg);
                }

                reader.Close();

                return registers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static List<Register> GetRegisters()
        {
            try
            {
                string sql = "SELECT Id, RegisterName, Device, PurchaseDate, ExpiresDate FROM Registers";
                DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql);

                List<Register> registers = new List<Register>();
                while (reader.Read())
                {
                    Register reg = Create(reader);
                    registers.Add(reg);
                }

                reader.Close();

                return registers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static List<Register> GetRegisterPerOrganisation(int orgId)
        {
            try
            {
                string sql = "SELECT a.Id, a.RegisterName, a.Device, a.PurchaseDate, a.ExpiresDate FROM Registers as a INNER JOIN Register_Organisation as b ON a.Id = b.RegisterId WHERE b.OrganisationId = @OrganisationId";
                DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@OrganisationId", orgId);

                DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql, par1);

                List<Register> registers = new List<Register>();
                while (reader.Read())
                {
                    Register reg = Create(reader);
                    registers.Add(reg);
                }

                reader.Close();

                return registers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int InsertRegister(Register reg)
        {
            try
            {
                string sql = "INSERT INTO Registers (RegisterName, Device, PurchaseDate, ExpiresDate) VALUES (@RegisterName, @Device, @PurchaseDate, @ExpiresDate)";

                DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@RegisterName", reg.RegisterName);
                DbParameter deviceParam = Database.AddParam(CONNECTIONSTRING, "@Device", reg.Device);
                DbParameter purchaseDate = Database.AddParam(CONNECTIONSTRING, "@PurchaseDate", DateTime.Now);
                DbParameter expiresDate = Database.AddParam(CONNECTIONSTRING, "@ExpiresDate", DateTime.Now.AddYears(5));

                int lastInsertedID = Database.InsertData(CONNECTIONSTRING, sql, nameParam, deviceParam, purchaseDate, expiresDate);
                return lastInsertedID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Register GetRegisterById(int id)
        {
            try
            {
                string sql = "SELECT Id, RegisterName, Device, PurchaseDate, ExpiresDate FROM Registers WHERE Id=@Id";

                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", id);

                DbDataReader reader = Database.GetData(Database.GetConnection(CONNECTIONSTRING), sql, idParam);
                reader.Read();

                Register register = Create(reader);

                return register;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static Register Create(IDataRecord record)
        {
            return new Register()
            {
                Id = (int)record["Id"],
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString(),
                PurchaseDate = Convert.ToDateTime(record["PurchaseDate"]),
                ExpiresDate = Convert.ToDateTime(record["ExpiresDate"])
            };
        }
    }
}