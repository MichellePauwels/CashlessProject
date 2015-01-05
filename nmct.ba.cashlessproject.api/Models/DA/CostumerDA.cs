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
    public class CostumerDA
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

        public static List<Costumer> GetCostumers(IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, CustomerName, Address, Balance, RegisterNumber FROM Customers";
                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

                List<Costumer> costumers = new List<Costumer>();
                while (reader.Read())
                {
                    Costumer cost = Create(reader);
                    costumers.Add(cost);
                }

                reader.Close();

                return costumers;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Costumer GetCostumerById(int id, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, CustomerName, Address, Balance, RegisterNumber FROM Customers WHERE Id=@Id";
                DbParameter par2 = Database.AddParam(CONNECTIONSTRING, "@Id", id);

                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par2);

                Costumer costumer = new Costumer();
                while (reader.Read())
                {
                    costumer = Create(reader);
                }

                reader.Close();

                return costumer;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
                throw;
            }
        }

        public static int InsertCostumer(Costumer cost, IEnumerable<Claim> claims)
        {
            try
            {
                if (cost.CostumerName != null && cost.Rijksregisternummer != null)
                {
                    string sql = "INSERT INTO Customers (CustomerName, Address, Balance, RegisterNumber) VALUES (@CustomerName, @Address, @Balance, @RegisterNumber)";

                    DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@CustomerName", cost.CostumerName);
                    DbParameter addressParam = Database.AddParam(CONNECTIONSTRING, "@Address", cost.Address);
                    DbParameter balanceParan = Database.AddParam(CONNECTIONSTRING, "@Balance", 0.0);
                    DbParameter registernumberParam = Database.AddParam(CONNECTIONSTRING, "@RegisterNumber", cost.Rijksregisternummer);

                    int lastInsertedID = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, addressParam, balanceParan, registernumberParam);
                    return lastInsertedID;
                }
                else
                {
                    return 0;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int UpdateCostumer(Costumer cost, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "UPDATE Customers SET CustomerName=@CustomerName, Address=@Address, Balance=@Balance WHERE RegisterNumber=@RegisterNumber";

                DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@CustomerName", cost.CostumerName);
                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@RegisterNumber", cost.Rijksregisternummer);
                DbParameter addressParam = Database.AddParam(CONNECTIONSTRING, "@Address", cost.Address);
                DbParameter balanceParam = Database.AddParam(CONNECTIONSTRING, "@Balance", cost.Balance);

                int lastInsertedID = Database.UpdateData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, idParam, addressParam, balanceParam);
                return lastInsertedID;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static Costumer Create(IDataRecord record)
        {
            return new Costumer()
            {
                Id = (int)record["Id"],
                CostumerName = record["CustomerName"].ToString(),
                Address = record["Address"].ToString(),
                Balance = Double.Parse(record["Balance"].ToString()),
                Rijksregisternummer = record["RegisterNumber"].ToString()
            };
        }
    }
}