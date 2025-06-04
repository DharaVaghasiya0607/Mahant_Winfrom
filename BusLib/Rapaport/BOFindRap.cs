using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using AxonDataLib;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Rapaport
{
    public class BOFindRap
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        #region FindScal Region

        public Trn_RapSaveProperty FindRap(Trn_RapSaveProperty pClsProperty)
        {
            if (pClsProperty.CARAT == 0 || pClsProperty.SHAPE_ID ==0 || pClsProperty.COLOR_ID == 0 || pClsProperty.CLARITY_ID == 0)
            {
                pClsProperty.FINALBACK = 0;
                pClsProperty.FINALPRICEPERCARAT = 0;
                pClsProperty.RAPAPORT = 0;
                pClsProperty.FINALAMOUNT = 0;
                pClsProperty.XMLDETAIL = string.Empty;
                return pClsProperty;
            }

            DataTable DTab = new DataTable();
            Ope.ClearParams();


            Ope.AddParams("Stock_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
	        Ope.AddParams("StockNo", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Shape_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ShapeCode", pClsProperty.SHAPECODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ShapeRapValue", pClsProperty.SHAPERAPVALUE, DbType.String, ParameterDirection.Input);
			
            Ope.AddParams("Carat", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("Color_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ColorCode", pClsProperty.COLORCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ColorRapValue", pClsProperty.COLORRAPVALUE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Clarity_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ClarityCode", pClsProperty.CLARITYCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ClarityRapValue", pClsProperty.CLARITYRAPVALUE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ColorShade_ID", pClsProperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ColorShadeCode", pClsProperty.COLORSHADECODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Cut_ID", pClsProperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CutCode", pClsProperty.CUTCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Pol_ID", pClsProperty.POL_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PolCode", pClsProperty.POLCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Sym_ID", pClsProperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SymCode", pClsProperty.SYMCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FL_ID", pClsProperty.FL_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("FLCode", pClsProperty.FLCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Milky_ID", pClsProperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MilkyCode", pClsProperty.MILKYCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Girdle_ID", pClsProperty.GIRDLE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("GirdleCode", pClsProperty.GIRDLECODE, DbType.String, ParameterDirection.Input);

            //Ope.AddParams("TableInc_ID", pClsProperty.TABLEINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TableInc_ID", pClsProperty.TABLEINC_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TableIncCode", pClsProperty.TABLEINCCODE, DbType.String, ParameterDirection.Input);

            //Ope.AddParams("TableOpenInc_ID", pClsProperty.TABLEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TableOpenInc_ID", pClsProperty.TABLEOPENINC_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TableOpenIncCode", pClsProperty.TABLEOPENINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SideOpenInc_ID", pClsProperty.SIDEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SideOpenIncCode", pClsProperty.SIDEOPENINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SideTableInc_ID", pClsProperty.SIDETABLEINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SideTableIncCode", pClsProperty.SIDETABLEINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("TableBlackInc_ID", pClsProperty.TABLEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TableBlackIncCode", pClsProperty.TABLEBLACKINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SideBlackInc_ID", pClsProperty.SIDEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SideBlackIncCode", pClsProperty.SIDEBLACKINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RedSportInc_ID", pClsProperty.REDSPORTINC_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("RedSportIncCode", pClsProperty.REDSPORTINCCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Culet_ID", pClsProperty.CULET_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CuletCode", pClsProperty.CULETCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Luster_ID", pClsProperty.LUSTER_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("LusterCode", pClsProperty.LUSTERCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("Lab_ID", pClsProperty.LAB_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("LabCode", pClsProperty.LABCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("HA_ID", pClsProperty.HA_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("HACode", pClsProperty.HACODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("EyeClean_ID", pClsProperty.EYECLEAN_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EyeCleanCode", pClsProperty.EYECLEANCODE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("DepthPer", pClsProperty.DEPTHPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TablePer", pClsProperty.TABLEPER, DbType.Double, ParameterDirection.Input);
			
            Ope.AddParams("Diameter", pClsProperty.DIAMETER, DbType.Double, ParameterDirection.Input);
	        
            Ope.AddParams("Length", pClsProperty.LENGTH, DbType.Double, ParameterDirection.Input);
	        Ope.AddParams("Width", pClsProperty.WIDTH, DbType.Double, ParameterDirection.Input);
	        Ope.AddParams("Height", pClsProperty.HEIGHT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("ISFancy", pClsProperty.ISFANCY, DbType.Double, ParameterDirection.Input);
            
            Ope.AddParams("SaleRapaport", pClsProperty.SALERAPAPORT, DbType.Double, ParameterDirection.Input);
	        Ope.AddParams("SaleDiscount", pClsProperty.SALEDISCOUNT, DbType.Double, ParameterDirection.Input);
	        Ope.AddParams("SalePricePerCarat", pClsProperty.SALEPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
	        Ope.AddParams("SaleAmount", pClsProperty.SALEAMOUNT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("RapDate", pClsProperty.RAPDATE, DbType.Double, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "usp_FindRap", CommandType.StoredProcedure);

            pClsProperty.FINALBACK = 0;
            pClsProperty.FINALPRICEPERCARAT = 0;
            pClsProperty.RAPAPORT = 0;
            pClsProperty.FINALAMOUNT = 0;
            pClsProperty.XMLDETAIL = string.Empty;

            foreach (DataRow DRow in DTab.Rows)
            {
                pClsProperty.FINALBACK = Val.Val(DRow["FINALBACK"]);
                pClsProperty.FINALPRICEPERCARAT = Val.Val(DRow["FINALPRICEPERCARAT"]);
                pClsProperty.RAPAPORT = Val.Val(DRow["RAPAPORT"]);
                pClsProperty.FINALAMOUNT = Val.Val(DRow["FINALAMOUNT"]);
                pClsProperty.XMLDETAIL = Val.ToString(DRow["BACKDETAIL"]);
            }

            return pClsProperty;
        }

        public DataTable GetAllParameterTable()
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();
            string Str = "Select * From MST_Para With(NOLOCK) Where ISActive = 1";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }

        public DataTable GetSizeData()
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();
            string Str = "SELECT * FROM dbo.MST_Size With(NOLOCK) Where ISActive = 1";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }

        public DataTable ExportPackingList(string pStrMemoID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("INVOICE_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LabIssuePrintGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForIGI(string pStrMemoID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STONENO", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LabIssuePrintGetDataForIGI", CommandType.StoredProcedure);
            return DTab;
        }
        #endregion
    }
}
