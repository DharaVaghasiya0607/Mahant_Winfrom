using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;


namespace BusLib.Utility
{
    public class BOMST_ImageCertiFlagUpdate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public int ImageVedioCertiFlagUpdate()
        {
            try
            {
                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_ImageVideoCertiFlagUpdate", CommandType.Text);

            }
            catch (Exception ex)
            {

                return 0;

            }

        }

        public DataTable GetImgCertiVideoStoneDetail()
        {
            DataTable DTab = new DataTable();
            try
            {
                string StrQuery = "Select * From Trn_Stock With(Nolock) Where ISnull(ISCerti,0) = 0 Or ISImage = 0 Or ISVideo = 0";
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
                //return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_ImageVideoCertiFlagUpdate", CommandType.Text);
            }
            catch (Exception ex)
            {

            }
            return DTab;
        }

        //Add : #P : 29-06-2020 //Used In Image/Certi/Video Download
        public string GetCertiDownloadConnection()
        {
            try
            {
                Ope.ClearParams();
                string StrQuery = "SELECT ACTIVE FROM Win_Services WHERE WinEventName='Certificate Download";
                return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public DataTable GetCertImageVideoFlagUpdateStoneDetail(string pStrOpe)
        {
            try
            {
                DataTable Dtab = new DataTable();
                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "Win_ImageVideoCertDownload", CommandType.StoredProcedure);
                return Dtab;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        //public int UpdateImageCertiVideoFlag( string pStrStock_ID)
        //{
        //    Ope.ClearParams();
        //    string StrQuery = "";
        //    StrQuery = "Update TRN_Stock With(RowLock) SET  ISCerti = 1 Where Stock_ID = '" + Val.ToString(pStrStock_ID) + "'";
        //    return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        //}
        public int UpdateImageCertiVideoFlag(string pStrSql)
        {
            Ope.ClearParams();
            string StrQuery = pStrSql;
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        //End : #P : 29-06-2020
    }
}
