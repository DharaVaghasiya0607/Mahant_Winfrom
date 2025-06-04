using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using AxonDataLib;
using System.Data;
using BusLib.TableName;
using System.Collections;


namespace BusLib.CRM
{
	public class BOCRM_TargetCreateMaster
	{
		AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
		AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

		public DataTable Fill(int pIntYear, int pIntMonth)
		{
			Ope.ClearParams();
			DataTable DTab = new DataTable();
			Ope.AddParams("YEAR", pIntYear, DbType.Int32, ParameterDirection.Input);
			Ope.AddParams("MONTH", pIntMonth, DbType.Int32, ParameterDirection.Input);

			Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "CRM_TargetCreateMasterGetData", CommandType.StoredProcedure);
			return DTab;
		}

		public TargetCreateMaster Save(TargetCreateMaster pClsProperty)
		{
			try
			{
				Ope.ClearParams();
				Ope.AddParams("TARGET_ID", pClsProperty.TARGET_ID, DbType.Guid, ParameterDirection.Input);
				Ope.AddParams("EMPLOYEE_ID", pClsProperty.EMPLOYEE_ID, DbType.Guid, ParameterDirection.Input);
				Ope.AddParams("YEAR", pClsProperty.FROMYEAR, DbType.Int32, ParameterDirection.Input);

				Ope.AddParams("MONTH", pClsProperty.FROMMONTH, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("SALETARGETDOLLAR", pClsProperty.SALETARGETDOLLAR, DbType.Decimal, ParameterDirection.Input);
				Ope.AddParams("NOOFCUSTOMER", pClsProperty.NOOFCUST, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("NOOFNEWCUSTOMER", pClsProperty.NOOFNEWCUST, DbType.Int32, ParameterDirection.Input);


				Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
				Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

				Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

				ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CRM_TargetCreateMasterSave", CommandType.StoredProcedure);

				if (AL.Count != 0)
				{
					pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
					pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
					pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
				}
			}
			catch (System.Exception ex)
			{
				pClsProperty.RETURNVALUE = "";
				pClsProperty.RETURNMESSAGETYPE = "FAIL";
				pClsProperty.RETURNMESSAGEDESC = ex.Message;

			}
			return pClsProperty;

		}

		public int CopyPastTarget(int pIntFromYear, int pIntFromMonth, int pIntToYear, Int32 pIntToMonth)
		{
			try
			{
				Ope.ClearParams();
				Ope.AddParams("FROMYEAR", pIntFromYear, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("FROMMONTH", pIntFromMonth, DbType.Int32, ParameterDirection.Input);

				Ope.AddParams("TOYEAR", pIntToYear, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("TOMONTH", pIntToMonth, DbType.Int32, ParameterDirection.Input);

				Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
				Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

				return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "CRM_TargetCreateMasterCopyPast", CommandType.StoredProcedure);

			}
			catch (System.Exception ex)
			{
				return -1;
			}
		}

        public TargetCreateMaster Delete(TargetCreateMaster pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TARGET_ID", pClsProperty.TARGET_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CRM_TargetCreateMasterDelete", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;

        }

	}
}
