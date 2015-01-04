using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models.DA
{
    public class RegisterOrganisationDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        public static int LinkRegisterToOrganisation(Register register, Vereniging vereniging)
        {
            int rowsaffected = 0;
            DbTransaction trans = null;
            DbTransaction trans2 = null;

            try
            {
                trans = Database.BeginTransaction(CONNECTIONSTRING);

                string sql = "INSERT INTO Register_Organisation(RegisterId, OrganisationId) VALUES(@RegisterId, @OrganisationId)";
                DbParameter par1 = Database.AddParam(CONNECTIONSTRING, "@RegisterId", register.Id);
                DbParameter par2 = Database.AddParam(CONNECTIONSTRING, "@OrganisationId", vereniging.Id);

                rowsaffected += Database.InsertData(trans, sql, par1, par2);

                SqlConnection CONNECTIONSTRING2 = new SqlConnection("Data Source=MichelleToshiba;Initial Catalog=" + vereniging.DbName + ";Integrated Security=True");
                trans2 = Database.BeginTransaction(CONNECTIONSTRING2);

                string sql2 = "INSERT INTO Registers(RegisterName, Device, PurchaseDate, ExpiresDate) VALUES (@RegisterName, @Device, @PurchaseDate, @ExpiresDate)";
                DbParameter par3 = Database.AddParam(CONNECTIONSTRING2, "@RegisterName", register.RegisterName);
                DbParameter par4 = Database.AddParam(CONNECTIONSTRING2, "@Device", register.Device);
                DbParameter par5 = Database.AddParam(CONNECTIONSTRING2, "@PurchaseDate", register.PurchaseDate);
                DbParameter par6 = Database.AddParam(CONNECTIONSTRING2, "@ExpiresDate", register.ExpiresDate);
                rowsaffected += Database.InsertData(trans2, sql2, par3, par4, par5, par6);

                trans.Commit();
                trans2.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                if (trans2 != null)
                    trans.Rollback();
            }
           finally
            {
                if (trans != null)
                    Database.ReleaseConnection(trans.Connection);
            }

            return rowsaffected;
        }

        public static bool IsAvailableRegister(int registerId)
        {
            string sql = "SELECT RegisterId FROM Register_Organisation WHERE RegisterId = @RegisterId";
            DbParameter registeridParam = Database.AddParam(CONNECTIONSTRING, "@RegisterId", registerId);

            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql, registeridParam);
            if (!reader.HasRows)
                return true;
            else
                return false;
        }
    }
}