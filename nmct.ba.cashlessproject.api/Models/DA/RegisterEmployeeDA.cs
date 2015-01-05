using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models.DA
{
    public class RegisterEmployeeDA
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

        public static List<Employee> GetEmployeePerRegister(int registerId, IEnumerable<Claim> claims)
        {
            string sql = "SELECT Employee.EmployeeName FROM Register_Employee INNER JOIN Employee ON Employee.Id = Register_Employee.EmployeeID INNER JOIN Registers ON Registers.Id = Register_Employee.RegisterID WHERE Register_Employee.RegisterID = @RegisterId";
            DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@RegisterId", registerId);

            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            List<Employee> employees = new List<Employee>();
            while (reader.Read())
            {
                Employee emp = new Employee()
                {
                    EmployeeName = reader["EmployeeName"].ToString()
                };

                employees.Add(emp);
            }

            reader.Close();

            return employees;
        }

        public static int LinkEmployeeToRegister(RegisterEmployee regemp, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "INSERT INTO Register_Employee (RegisterID, EmployeeID, FromTime, UntilTime) VALUES (@RegisterID, @EmployeeID, @FromTime, @UntilTime)";
                DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@RegisterID", regemp.RegisterId);
                DbParameter par2 = Database.AddParam(CONNECTIONSTRING, "@EmployeeID", regemp.EmployeeId);
                DbParameter par3 = Database.AddParam(CONNECTIONSTRING, "@FromTime", regemp.From);
                DbParameter par4 = Database.AddParam(CONNECTIONSTRING, "@UntilTime", regemp.Until);

                int lastInsertedID = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
                return lastInsertedID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}