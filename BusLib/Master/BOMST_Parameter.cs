using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Master
{
    public class BOMST_Parameter
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(string pStrGroup)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("PARATYPE", pStrGroup, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, SProc.MST_ParameterGetData, CommandType.StoredProcedure);
            return DTab;
        }
       
        public ParameterMasterProperty Save(ParameterMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PARA_ID", pClsProperty.PARA_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PARACODE", pClsProperty.PARACODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARANAME", pClsProperty.PARANAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RAPAVALUE", pClsProperty.RAPAVALUE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RAPNETUPLOADCODE", pClsProperty.RAPNETUPLOADCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHORTNAME", pClsProperty.SHORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBDISPLAY", pClsProperty.WEBDISPLAY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARATYPE", pClsProperty.PARATYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SEQUENCENO", pClsProperty.SEQUENCENO, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("NIKUNJSEQNO", pClsProperty.NIKUNJSEQNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ANKITSEQNO", pClsProperty.ANKITSEQNO, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LABCODE", pClsProperty.LABCODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PARAGROUP", pClsProperty.PARAGROUP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("IMAGEPATH", pClsProperty.IMAGEPATH, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SELIMAGEPATH", pClsProperty.SELIMAGEPATH, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DEPT_ID", pClsProperty.DEPT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("JACCODE", pClsProperty.JACCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITYWISEDEPARTMENT_ID", pClsProperty.CLARITYWISEDEPARTMENT_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ACC_CODE", pClsProperty.ACCCODE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ACC_DESC", pClsProperty.ACCDESC, DbType.String, ParameterDirection.Input);

                Ope.AddParams("EXCELNAME", pClsProperty.EXCELNAME, DbType.String, ParameterDirection.Input); //DHARA : 01-01-2024

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, SProc.MST_ParameterSave, CommandType.StoredProcedure);

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

        
        public ParameterMasterProperty Delete(ParameterMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PARA_ID", pClsProperty.PARA_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, SProc.MST_ParameterDelete, CommandType.StoredProcedure);

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
        public DataTable GetParameterData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT PARA_ID,PARACODE,PARANAME,SHORTNAME,REMARK,LABCODE,PARATYPE,PARAGROUP,RAPAVALUE,SEQUENCENO FROM MST_PARA WITH(NOLOCK) WHERE 1=1 ";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }
        public DataTable GetFancyColorData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT FANCYCOLOR_ID,REMARK FROM MST_FANCYCOLOR  WITH(NOLOCK) WHERE ISACTIVE = 1 ";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataTable GetParameterDataParcel()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT ISNULL(PARA_ID,0) AS PARA_ID,ISNULL(PARACODE,'') AS PARACODE ,ISNULL(PARANAME,'') AS PARANAME ,ISNULL(LABCODE,'') AS LABCODE,PARATYPE ";
            StrQuery += " FROM MST_PARA WITH(NOLOCK) WHERE PARATYPE IN('SHAPE','COLOR','CLARITY','CUT','POLISH','SYMMETRY','FLUORESCENCE','LOCATION','MILKY','FANCYCOLOR','LAB','MIX_CLARITY' ";
            StrQuery += " ) UNION ALL SELECT SIZE_ID as PARA_ID, SIZENAME as PARACODE, SIZENAME AS PARANAME, remark AS LABCODE, 'MIX_SIZE' as PARATYPE ";
            StrQuery += " FROM MST_MIXSIZE WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataTable GetColorClarityComment()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ColorClarityComment_GetData", CommandType.StoredProcedure);
            return DTab;
        }

        //Added by Daksha on 4/05/2023
        public DataTable ParcelParameter_GetData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ParcelParameter_GetData", CommandType.StoredProcedure);
            return DTab;
        }
        //End as Daksha

        public DataSet GetDataForDashBoard()
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet , "Temp", "TRN_DashBoardGetData", CommandType.StoredProcedure);
            return DSet;
        }
        public NotificationSendAndReceive SaveSendAndReceive(NotificationSendAndReceive pClsProperty,string StrOPE)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STONENO", pClsProperty.STONENO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OPE", StrOPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DISCRIPTION", pClsProperty.DISCRIPTION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISRECEIVE", pClsProperty.ISRECEIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISCLEAR", pClsProperty.ISCLEAR, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISSEND", pClsProperty.ISSEND, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("NOTIFICATION_ID", pClsProperty.Notification_ID, DbType.Guid, ParameterDirection.Input);


                Ope.AddParams("LOCATION_ID", pClsProperty.LOACTION_ID, DbType.Int32, ParameterDirection.Input);          
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
              
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_SaleTransferNotificationSave", CommandType.StoredProcedure);

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

    }
}
