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
    public class EmployeeDA
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

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "SELECT Id, EmployeeName, Address, Email, Phone FROM Employee";
                DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

                List<Employee> employee = new List<Employee>();
                while (reader.Read())
                {
                    Employee emp = Create(reader);
                    employee.Add(emp);
                }

                reader.Close();

                return employee;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int InsertEmployee(Employee emp, IEnumerable<Claim> claims)
        {
            try
            {
                if (emp.EmployeeName != null && emp.Address != null && emp.Email != null && emp.Phone != null)
                {
                    string sql = "INSERT INTO Employee (EmployeeName, Address, Email, Phone) VALUES (@EmployeeName, @Address, @Email, @Phone)";

                    DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@EmployeeName", emp.EmployeeName);
                    DbParameter addressParam = Database.AddParam(CONNECTIONSTRING, "@Address", emp.Address);
                    DbParameter emailParam = Database.AddParam(CONNECTIONSTRING, "@Email", emp.Email);
                    DbParameter phoneParam = Database.AddParam(CONNECTIONSTRING, "@Phone", emp.Phone);

                    int lastInsertedID = Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, addressParam, emailParam, phoneParam);
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

        public static int UpdateEmployee(Employee emp, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "UPDATE Employee SET EmployeeName=@EmployeeName, Address=@Address, Email=@Email, Phone=@Phone WHERE Id=@Id";

                DbParameter nameParam = Database.AddParam(CONNECTIONSTRING, "@EmployeeName", emp.EmployeeName);
                DbParameter idParam = Database.AddParam(CONNECTIONSTRING, "@Id", emp.Id);
                DbParameter addressParam = Database.AddParam(CONNECTIONSTRING, "@Address", emp.Address);
                DbParameter emailParam = Database.AddParam(CONNECTIONSTRING, "@Email", emp.Email);
                DbParameter phoneParam = Database.AddParam(CONNECTIONSTRING, "@Phone", emp.Phone);

                int lastInsertedID = Database.UpdateData(Database.GetConnection(CreateConnectionString(claims)), sql, nameParam, idParam, addressParam, emailParam, phoneParam);
                return lastInsertedID;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int DeleteEmployee(int id, IEnumerable<Claim> claims)
        {
            try
            {
                string sql = "DELETE FROM Employee WHERE Id=@Id";

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

        private static Employee Create(IDataRecord record)
        {
            return new Employee()
            {
                Id = (int)record["Id"],
                EmployeeName = record["EmployeeName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }
    }
}