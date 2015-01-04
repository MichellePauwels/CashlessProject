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
    public class ProductDA
    {
        // hier komt de rollback en commit in

        private const string CONNECTIONSTRING = "ConnectionString";

        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;
            string id = claims.FirstOrDefault(c => c.Type == "id").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"MichelleToshiba", dbname, dblogin, dbpass);
        }

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, Name, Price FROM Products";
                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

                List<Product> products = new List<Product>();
                while (reader.Read())
                {
                    Product prod = Create(reader);
                    products.Add(prod);
                }

                reader.Close();

                return products;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Product GetProductById(int Id, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, Name, Price FROM Products WHERE Id = @Id";

                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", Id);
                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, idParam);

                Product product = new Product();
                while (reader.Read())
                {
                    Product prod = Create(reader);
                }

                reader.Close();

                return product;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int InsertProduct(Product prod, IEnumerable<Claim> claims)
        {
            try
            {
                if (prod.Name != null && prod.Price != null)
                {
                    string sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";

                    DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@Name", prod.Name);
                    DbParameter priceParam = Database.AddParam(CONNECTIONSTRING, "@Price", Convert.ToDouble(prod.Price));

                    int lastInsertedID = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, priceParam);
                    return lastInsertedID;
                }
                else
                {
                    return 0;
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int UpdateProduct(Product prod, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "UPDATE Products SET Name=@Name, Price=@Price WHERE Id=@Id";

                DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@Name", prod.Name);
                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", prod.Id);
                DbParameter priceParam = Database.AddParam(CONNECTIONSTRING, "@Price", prod.Price);

                int lastInsertedID = Database.UpdateData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, idParam, priceParam);
                return lastInsertedID;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "DELETE FROM Products WHERE Id=@Id";

                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", id);

                int i = Database.UpdateData(Database.GetConnection(CreateConnectionString(claims)), sql, idParam);

                return i;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static Product Create(IDataRecord record)
        {
            return new Product()
            {
                Id = (int)record["Id"],
                Name = record["Name"].ToString(),
                Price = Double.Parse(record["Price"].ToString())
            };
        }
    }
}