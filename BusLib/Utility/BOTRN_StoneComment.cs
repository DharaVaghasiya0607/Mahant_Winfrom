using System;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;


namespace BusLib.Utility
{
    public class BOTRN_StockComment
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        

        #region Other Function

        public DataTable GetData(string pStrStockID)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pStrStockID, DbType.Guid, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockCommentGetData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {
               
                return null;
            }
        }

        public int Save(string pStrStockID, string pStrComment)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pStrStockID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COMMENT", pStrComment, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", BusLib.Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Trn_StockCommentSave", CommandType.StoredProcedure);

            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        public int Delete(string pStrCommentID)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("COMMENT_ID", pStrCommentID, DbType.Guid, ParameterDirection.Input);
             
                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Trn_StockCommentDelete", CommandType.StoredProcedure);

            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        //End As
        #endregion

        #region Album Create


        public string SaveAlbum(string pStrAlbumID, string pStrPartyID, string pStrTitle, string pStrDescription, string pStrXML)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("ALBUM_ID", pStrAlbumID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTY_ID", pStrPartyID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ALBUMTITLE", pStrTitle, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ALBUMDESCRIPTION", pStrDescription, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StockAlbumSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    return Val.ToString(AL[0]);                    
                }
                return "";
            }
            catch (Exception Ex)
            {
                return "";
            }
        }

        public int DeleteAlbum(string pStrAlbumID)
        {
            try
            {
                Ope.ClearParams();
                string Str = "Delete From Trn_StockAlbum With(RowLock) Where Album_ID = '" + pStrAlbumID + "' ";
                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        public DataTable GetDataAlbum(string pStrFromDate, string pStrToDate)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockAlbumGetData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        #endregion
    }
}

