using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TPV
{
    public class BOSProc
    {
        public static string usp_CheckLogin = "usp_CheckLogin";
        public static string usp_Mast_CmbFill = "usp_Mast_CmbFill";

        public static string MST_CountryGetData = "MST_CountryGetData";
        public static string MST_StateGetData = "MST_StateGetData";
        public static string MST_CityGetData = "MST_CityGetData";
    
        public static string MST_LedgerGetData = "MST_LedgerGetData";
        public static string MST_LedgerGetDataByPK = "MST_LedgerGetDataByPK";
        public static string MST_LedgerSave = "MST_LedgerSave";
        public static string MST_LedgerDelete = "MST_LedgerDelete";


        public static string MST_ParameterGetData = "MST_ParameterGetData";      
        public static string MST_ParameterSave = "MST_ParameterSave";
        public static string MST_ParameterDelete = "MST_ParameterDelete";

        public static string MST_ItemGetData = "MST_ItemGetData";
        public static string MST_ItemSave = "MST_ItemSave";
        public static string MST_ItemDelete = "MST_ItemDelete";

        public static string MST_UserGetData = "MST_UserGetData";
        public static string MST_UserSave = "MST_UserSave";
        public static string MST_UserDelete = "MST_UserDelete";
        public static string CheckLogin = "CheckLogin";


        public static string MST_YearGetData = "MST_YearGetData";
        public static string MST_YearSave = "MST_YearSave";
        public static string MST_YearDelete = "MST_YearDelete";

        public static string MST_ItemGroupGetData = "MST_ItemGroupGetData";
        public static string MST_ItemGroupSave = "MST_ItemGroupSave";
        public static string MST_ItemGroupDelete = "MST_ItemGroupDelete";

        public static string MST_LedgerGroupGetData = "MST_LedgerGroupGetData";
        public static string MST_LedgerGroupGetDataByPK = "MST_LedgerGroupGetDataByPK";
        public static string MST_LedgerGroupSave = "MST_LedgerGroupSave";
        public static string MST_LedgerGroupDelete = "MST_LedgerGroupDelete";

        public static string MST_CompanyGetData = "MST_CompanyGetData";
        public static string MST_CompanyGetDataByPK = "MST_CompanyGetDataByPK";
        public static string MST_CompanySave = "MST_CompanySave";
        public static string MST_CompanyDelete = "MST_CompanyDelete";

        public static string MST_ParamGetData = "MST_ParamGetData";
        public static string MST_ParamGetDataByPK = "MST_ParamGetDataByPK";
        public static string MST_ParamSave = "MST_ParamSave";
        public static string MST_ParamDelete = "MST_ParamDelete";

        public static string Hr_AttendanceEntryGetData = "Hr_AttendanceEntryGetData";
        public static string HR_AttendanceEntrySave = "HR_AttendanceEntrySave";
        public static string HR_AttendanceEntryDelete = "HR_AttendanceEntryDelete";


        public static string Hr_SalaryEntryGetData = "Hr_SalaryEntryGetData";
        public static string HR_SalaryEntrySave = "HR_SalaryEntrySave";
        public static string HR_SalaryEntryDelete = "HR_SalaryEntryDelete";


        public static string MST_PriceListGetData = "MST_PriceListGetData";
        public static string MST_PriceListSave = "MST_PriceListSave";

        public static string MST_LedgerContactGetData = "MST_LedgerContactGetData";
        public static string MST_LedgerContactSave = "MST_LedgerContactSave";

        public static string Fill_Combo = "Fill_Combo";
        public static string Fill_ComboParam = "Fill_ComboParam";

        public static string MST_EmployeeGetData = "MST_EmployeeGetData";
        public static string MST_EmployeeGetDataByPK = "MST_EmployeeGetDataByPK";
        public static string MST_EmployeeSave = "MST_EmployeeSave";
        public static string MST_EmployeeDelete = "MST_EmployeeDelete";

        public static string TRN_ProductionGetData = "TRN_ProductionGetData";
        public static string TRN_ProductionSave = "TRN_ProductionSave";

        #region Master

        public static string TRN_TaskSave = "TRN_TaskSave";
        public static string TRN_TaskUpdate = "TRN_TaskUpdate";

        public static string TRN_TaskDelete = "TRN_TaskDelete";
        public static string TRN_TaskGetData = "TRN_TaskGetData";

        public static string DASH_TaskList = "DASH_TaskList";
        public static string DASH_TaskLabour = "DASH_TaskLabour";

        public static string DASH_TaskEmployeeWisePendingSum = "DASH_TaskEmployeeWisePendingSum";
        public static string DASH_TaskEmployeeWisePerformance = "DASH_TaskEmployeeWisePerformance";

        public static string TRN_TaskExpenseSave = "TRN_TaskExpenseSave";
        public static string TRN_TaskExpenseDelete = "TRN_TaskExpenseDelete";
        public static string TRN_TaskExpenseGetData = "TRN_TaskExpenseGetData";

        public static string TRN_TaskCommentSave = "TRN_TaskCommentSave";
        public static string TRN_TaskCommentDelete = "TRN_TaskCommentDelete";
        public static string TRN_TaskCommentGetData = "TRN_TaskCommentGetData";

        public static string TRN_TaskAttachmentSave = "TRN_TaskAttachmentSave";
        public static string TRN_TaskAttachmentDelete = "TRN_TaskAttachmentDelete";
        public static string TRN_TaskAttachmentGetData = "TRN_TaskAttachmentGetData";

        public static string TRN_TaskStatusGetData = "TRN_TaskStatusGetData";
        

        public static string usp_Mast_ItemMasterDisp = "usp_Mast_ItemMasterDisp";
        public static string usp_Mast_AccountTypeMasterDisp = "usp_Mast_AccountTypeMasterDisp";
        public static string usp_Mast_EmployeeMasterDisp = "usp_Mast_EmployeeMasterDisp";
        public static string usp_Mast_LedgerMasterDisp = "usp_Mast_LedgerMasterDisp";
        public static string usp_Mast_LedgerMasterPopDisp = "usp_Mast_LedgerMasterPopDisp";

        public static string usp_GetCustomerOpening = "usp_GetCustomerOpening";
        public static string usp_GetPartyOpening = "usp_GetPartyOpening";

        public static string usp_Mast_ConfigMasterDisp = "usp_Mast_ConfigMasterDisp";
        public static string usp_Mast_LocationMasterDisp = "usp_Mast_LocationMasterDisp";

        #endregion

        #region Purchase

        public static string usp_ReturnPurchaseDetailDisp = "usp_ReturnPurchaseDetailDisp";
        public static string usp_ReturnPurchaseDetailPopUp = "usp_ReturnPurchaseDetailPopUp";

        public static string usp_PurchaseMasterDisp = "usp_PurchaseMasterDisp";
        public static string usp_PurchaseDetailSave = "usp_PurchaseDetailSave";
        public static string usp_PurchaseMasterSave = "usp_PurchaseMasterSave";
        public static string usp_PurchaseDelete = "usp_PurchaseDelete";

        #endregion

        #region Payment

        public static string usp_PaymentDetailDisp = "usp_PaymentDetailDisp";
        public static string usp_PaymentDetailSave = "usp_PaymentDetailSave";
        public static string usp_PaymentDetailValSave = "usp_PaymentDetailValSave";
        public static string usp_PaymentDetailDelete = "usp_PaymentDetailDelete";

        #endregion

        #region View

        public static string Viw_Purchase_Detail = "Viw_Purchase_Detail";
        public static string Viw_Sale_Detail = "Viw_Sale_Detail";
        public static string Viw_TotalStock = "Viw_TotalStock";

        public static string Viw_Payment_PartyView = "Viw_Payment_PartyView";
        public static string Viw_Payment_CustView = "Viw_Payment_CustView";
        public static string Viw_Payment_CustViewDetail = "Viw_Payment_CustViewDetail";
        public static string Viw_Payment_PartyViewDetail = "Viw_Payment_PartyViewDetail";
        public static string Viw_Payment_PartyPaymentFullDetail = "Viw_Payment_PartyPaymentFullDetail";
        public static string Viw_Error_Check = "Viw_Error_Check";

        #endregion

        #region Sale

        public static string FN_CHECKSTOCK = "DBO.FN_CHECKSTOCK";

        public static string usp_Return_Sale_MasterISExists = "usp_Return_Sale_MasterISExists";
        public static string usp_Sale_MasterISExists = "usp_Sale_MasterISExists";

        public static string usp_Sale_MasterDetailDisp = "usp_Sale_MasterDetailDisp";
        public static string usp_Sale_MasterDetailView = "usp_Sale_MasterDetailView";
        public static string usp_Sale_DetailCmbFill = "usp_Sale_DetailCmbFill";

        public static string usp_Sale_DetailSave = "usp_Sale_DetailSave";
        public static string usp_Sale_CompleteSave = "usp_Sale_CompleteSave";

        public static string usp_Sale_MasterSave = "usp_Sale_MasterSave";
        
        public static string usp_Sale_MasterDetailDelete = "usp_Sale_MasterDetailDelete";
        public static string usp_Sale_DetailDelete = "usp_Sale_DetailDelete";


        public static string usp_Return_Sale_MasterDetailDisp = "usp_Return_Sale_MasterDetailDisp";
        public static string usp_Return_Sale_MasterDetailView = "usp_Return_Sale_MasterDetailView";
        public static string usp_Return_Sale_DetailCmbFill = "usp_Return_Sale_DetailCmbFill";

        public static string usp_Return_Sale_DetailSave = "usp_Return_Sale_DetailSave";
        public static string usp_Return_Sale_MasterSave = "usp_Return_Sale_MasterSave";

        public static string usp_Return_Sale_CompleteSave = "usp_Return_Sale_CompleteSave";

        public static string usp_Return_Sale_MasterDetailDelete = "usp_Return_Sale_MasterDetailDelete";
        public static string usp_Return_Sale_DetailDelete = "usp_Return_Sale_DetailDelete";

        #endregion
    }
}
