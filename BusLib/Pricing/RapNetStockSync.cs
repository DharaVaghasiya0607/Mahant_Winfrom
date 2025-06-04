using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using System.Collections;
using BusLib.TableName;

namespace BusLib.CRM
{
    public class RapNetStockSync
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();



        public RapNetStockSyncProperty Save(RapNetStockSyncProperty pClsProperty)
        {
            try
            {
             Ope.ClearParams();
                Ope.AddParams("ID", pClsProperty.ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("USERNAME", pClsProperty.USERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PASSWORD", pClsProperty.PASSWORD, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPENAME", pClsProperty.SHAPENAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORNAME", pClsProperty.COLORNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITYNAME", pClsProperty.CLARITYNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsProperty.CUT_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUTNAME", pClsProperty.CUTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsProperty.POL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POLNAME", pClsProperty.POLNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsProperty.SYM_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYMNAME", pClsProperty.SYMNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FL_ID", pClsProperty.FL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FLNAME", pClsProperty.FLNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOR_ID", pClsProperty.FANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLORNAME", pClsProperty.FANCYCOLORNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", pClsProperty.LOCATION_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOCATIONNAME", pClsProperty.LOCATIONNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MILKY_ID", pClsProperty.MILKY_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MILKYNAME", pClsProperty.MILKYNAME, DbType.String, ParameterDirection.Input);
               

                Ope.AddParams("LAB_ID", pClsProperty.LAB_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LABNAME", pClsProperty.LABNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOXNAME", pClsProperty.BOXNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSTATUS_ID", pClsProperty.WEBSTATUS_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSTATUSNAME", pClsProperty.WEBSTATUSNAME, DbType.String, ParameterDirection.Input);



                Ope.AddParams("STONENO", pClsProperty.STONENO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CERTINO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT1, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT1, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMCARAT2", pClsProperty.FROMCARAT2, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT2", pClsProperty.TOCARAT2, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMCARAT3", pClsProperty.FROMCARAT3, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT3", pClsProperty.TOCARAT3, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMCARAT4", pClsProperty.FROMCARAT4, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT4", pClsProperty.TOCARAT4, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMCARAT5", pClsProperty.FROMCARAT5, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT5", pClsProperty.TOCARAT5, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMLENGTHPER", pClsProperty.FROMLENGTHPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOLENGTHPER", pClsProperty.TOLENGTHPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMWIDTHPER", pClsProperty.FROMWIDTHPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOWIDTHPER", pClsProperty.TOWIDTHPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMHEIGHTPER", pClsProperty.FROMHEIGHTPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOHEIGHTPER", pClsProperty.TOHEIGHTPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
              
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("UPDATEBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("UPDATEIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
               
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Rapnet_Stock_Sync_InsertUpdate", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public DataTable GetData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rapnet_Stock_Sync_GetData", CommandType.StoredProcedure);
            return DTab; ;
        }
    }
    }

