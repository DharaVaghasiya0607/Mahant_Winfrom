using AxonDataLib;
using BusLib.TableName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Config = BusLib.Configuration.BOConfiguration;
using SProc = BusLib.TPV.BOSProc;
using System.Data;
using System.Collections;

namespace BusLib.Master
{
	public class BOMST_StoneDay
	{
		AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
		AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

		public DataTable Fill()
		{
			DataTable Dtab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "MST_StoneDayMasterGetdata", CommandType.StoredProcedure);
			return Dtab;

		}

		public StoneDayProperty SAVE(StoneDayProperty PlcProperty)
		{
			try
			{
				Ope.AddParams("STONEDAY_ID", PlcProperty.STONEDAY_ID, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("FROMDAY", PlcProperty.FROMDAY, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("TODAY", PlcProperty.TODAY, DbType.Int32, ParameterDirection.Input);
				Ope.AddParams("DAYNAME", PlcProperty.DAYNAME, DbType.String, ParameterDirection.Input);

				Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
				Ope.AddParams("ENTRY_IP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

				Ope.AddParams("RETURNVALUE", PlcProperty.RETURNVALUE, DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGETYPE", PlcProperty.RETURNMESSAGETYPE, DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGEDESC", PlcProperty.RETURNMESSAGEDESC, DbType.String, ParameterDirection.Output);


                ArrayList AR = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_StoneDayMasterSave", CommandType.StoredProcedure);

				if (AR.Count != 0)
				{
					PlcProperty.RETURNVALUE = Val.ToString(AR[0]);
					PlcProperty.RETURNMESSAGETYPE = Val.ToString(AR[1]);
					PlcProperty.RETURNMESSAGEDESC = Val.ToString(AR[2]);
				}
			}
			catch (Exception ex)
			{
				PlcProperty.RETURNVALUE = "";
				PlcProperty.RETURNMESSAGETYPE = "FAIL";
				PlcProperty.RETURNMESSAGEDESC = ex.Message;
			}

			return PlcProperty;

		}

		public StoneDayProperty Delete(StoneDayProperty PlcProperty)
		{
			try
			{
				Ope.AddParams("STONEDAY_ID", PlcProperty.STONEDAY_ID, DbType.Int32, ParameterDirection.Input);

				Ope.AddParams("RETURNVALUE", PlcProperty.RETURNVALUE, DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGETYPE", PlcProperty.RETURNMESSAGETYPE, DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGEDESC", PlcProperty.RETURNMESSAGEDESC, DbType.String, ParameterDirection.Output);

                ArrayList AR = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_StoneMasterDelete", CommandType.StoredProcedure);

				if (AR.Count != 0)
				{
					PlcProperty.RETURNVALUE = Val.ToString(AR[0]);
					PlcProperty.RETURNMESSAGETYPE = Val.ToString(AR[1]);
					PlcProperty.RETURNMESSAGEDESC = Val.ToString(AR[2]);
				}
			}
			catch (Exception ex)
			{
				PlcProperty.RETURNVALUE = "";
				PlcProperty.RETURNMESSAGETYPE = "FAIL";
				PlcProperty.RETURNMESSAGEDESC = ex.Message;
			}

			return PlcProperty;

		}

	}
}
