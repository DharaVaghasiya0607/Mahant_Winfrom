using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;
namespace BusLib.TPV
{
    public  class BOMessage
    {
        public static string AddConfirmationMsg = "Are You Sure You Want Add This Record ?";
        public static string EditConfirmationMsg = "Are You Sure You Want Edit This Record ?";
        public static string DeleteConfirmationMsg = "Are You Sure You Want Delete This Record ?";
        public static string ExportConfirmationMsg = "Are You Sure You Want Export Data In To The Excel ?";
        public static string GPrintPdfConfirmationMsg = "Are You Sure You Want Print Data In To The Pdf ?";
        public static string HistoryConfirmationMsg = "Are You Sure You Want To Open Record History ?";
        public static string ExitConfirmationMsg = "Are You Sure You Want Close This Form ?";

        public static string MasterBlankDeniedMsg = "Without Selection Of Master This Form Can't Open?";
        public static string FormOpenDeniedMsg = "You Have No Rights For Open This Form?";
        public static string ViewDeniedMsg = "You Have No Rights For View Record List ?";
        public static string AddDeniedMsg = "You Have No Rights For New Record Entry ?";
        public static string EditDeniedMsg = "You Have No Rights For Update This Record ?";
        public static string DeleteDeniedMsg = "You Have No Rights For Delete This Record ?";
        

     }
}
