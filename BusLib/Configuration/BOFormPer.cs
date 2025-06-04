using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

namespace BusLib.Configuration
{
    public class BOFormPer
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        public string FORMGROUP = string.Empty;
        public string FORMNAME = string.Empty;
        public string FORMDESC = string.Empty;
        public bool ISVIEW = false;
        public bool ISINSERT = false;
        public bool ISUPDATE = false;
        public bool ISDELETE = false;
        public string PASSWORD = string.Empty;
        
        public void GetFormPermission(string pStrFormName)
        {
            AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

            try
            {
                Ope.ClearParams();
                Ope.AddParams("EMPLOYEE_ID", BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                
                Ope.AddParams("FORMNAME", pStrFormName.Trim(), DbType.String, ParameterDirection.Input);
                DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, "MST_FormPermissionGetDataFormDetail", CommandType.StoredProcedure);
                if (DRow == null)
                {
                    FORMGROUP = string.Empty;
                    FORMNAME = string.Empty;
                    FORMDESC = string.Empty;
                    ISVIEW = false;
                    ISINSERT = false;
                    ISUPDATE = false;
                    ISDELETE = false;
                    PASSWORD = string.Empty;
                }
                else
                {
                    FORMGROUP = Val.ToString(DRow["FORMGROUP"]);
                    FORMNAME = Val.ToString(DRow["FORMNAME"]);
                    FORMDESC = Val.ToString(DRow["FORMDESC"]);
                    ISVIEW = Val.ToBoolean(DRow["ISVIEW"]);
                    ISINSERT = Val.ToBoolean(DRow["ISINSERT"]);
                    ISUPDATE = Val.ToBoolean(DRow["ISUPDATE"]);
                    ISDELETE = Val.ToBoolean(DRow["ISDELETE"]);
                    PASSWORD = Val.ToString(DRow["PASSWORD"]);
                }

                Ope = null;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }

        }

      

    }
}
