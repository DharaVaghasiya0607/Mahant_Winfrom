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
    public class CRM_CustomerAutoMailCriteriaSave
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public CRMCustomerAutoMailCriteriaProperty Save(CRMCustomerAutoMailCriteriaProperty pClsProperty)
        {
            try
            
            {
                Ope.ClearParams();
               // Ope.AddParams("CRITERIA_ID", pClsProperty.CRITERIA_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COUSTOMER_ID", pClsProperty.CUSTOMER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EMAILTYPE", pClsProperty.EMAILTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("VALIDDATE", pClsProperty.VALIDDATE, DbType.Date, ParameterDirection.Input);     
                Ope.AddParams("LABFROMDATE", pClsProperty.LABFROMDATE, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("LABTODATE", pClsProperty.LABTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("AVAILBLEFROMDATE", pClsProperty.AVAILBLEFROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("AVAILBLETODATE", pClsProperty.AVAILBLETODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("UPLOADFROMDATE", pClsProperty.UPLOADFROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("UPLOADTODATE", pClsProperty.UPLOADTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("FROMLABRETURNDATE", pClsProperty.FROMLABRETURNDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TOLABRETURNDATE", pClsProperty.TOLABRETURNDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("FROMLABRESULTDATE", pClsProperty.FROMLABRESULTDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABRESULTTODATE", pClsProperty.LABRESULTTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("SALESFROMDATE", pClsProperty.SALESFROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("SALESTODATE", pClsProperty.SALESTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PRICEREVICESFROMDATE", pClsProperty.PRICEREVICESFROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PRICEREVICESTODATE", pClsProperty.PRICEREVICESTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DELIVERYFROMDATE", pClsProperty.DELIVERYFROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DELIVERYTODATE", pClsProperty.DELIVERYTODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPENAME", pClsProperty.MULTISHAPENAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORNAME", pClsProperty.MULTYCOLORNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITYNAME", pClsProperty.MULTYCLARITYNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUTNAME", pClsProperty.MULTYCUTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POLNAME", pClsProperty.MULTYPOLNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYMNAME", pClsProperty.MULTYSYMNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FLNAME", pClsProperty.MULTYFLNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOR_ID", pClsProperty.MULTYFANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLORNAME", pClsProperty.MULTYFANCYCOLORNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", pClsProperty.MULTYLOCATION_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOCATIONNAME", pClsProperty.MULTYLOCATIONNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MILKY_ID", pClsProperty.MULTYMILKY_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MILKYNAME", pClsProperty.MULTYMILKYNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsProperty.MULTYCOLORSHADE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORSHADENAME", pClsProperty.MULTYCOLORSHADENAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TABLEBLACKINC_ID", pClsProperty.MULTYTABLEBLACKINC_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TABLEBLACKINCNAME", pClsProperty.MULTYTABLEBLACKINCNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SIDEBLACKINC_ID", pClsProperty.MULTYSIDEBLACKINC_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SIDEBLACKINCNAME", pClsProperty.MULTYSIDEBLACKINCNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DAYXML", pClsProperty.DAYXML, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LABNAME", pClsProperty.MULTYLABNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOX_ID", pClsProperty.MULTYBOX_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOXNAME", pClsProperty.MULTYBOXNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSTATUS_ID", pClsProperty.WEBSTATUS_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSTATUSNAME", pClsProperty.WEBSTATUSNAME, DbType.String, ParameterDirection.Input);


                Ope.AddParams("KAPAN_ID", pClsProperty.MULTYKAPAN_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.MULTIKAPANNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CERTINO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MFG_ID", pClsProperty.MFG_ID, DbType.String, ParameterDirection.Input); //Added by Daksha on 27/02/2023

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

                Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

                //Ope.AddParams("DAY", pClsProperty.MULTYDAYS_ID, DbType.String, ParameterDirection.Input);
              
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CRM_CustomerAutoEmailSettingSave", CommandType.StoredProcedure);

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

        //Added by Daksha on 12/01/2023
        public CRMCustomerAutoMailCriteriaProperty Delete(CRMCustomerAutoMailCriteriaProperty pClsProperty)
        {
            try

            {
                Ope.ClearParams();
                Ope.AddParams("CUSTOMER_ID", pClsProperty.CUSTOMER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Crm_CustomerAutoEmailSetting_Delete", CommandType.StoredProcedure);

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
        //End as Daksha

    }
}
