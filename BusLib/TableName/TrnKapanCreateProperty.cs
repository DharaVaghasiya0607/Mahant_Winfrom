using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnKapanCreateProperty
    {
        public Guid KAPAN_ID { get; set; }
        public string KAPANDATE { get; set; }
        public string KAPANNAME { get; set; }
        public string KAPANCATEGORY { get; set; }
        public string COMPARMEMO { get; set; }

        public string SUBLOT { get; set; }
        public string SUBLOT1 { get; set; }

        public bool ISHIDE { get; set; }

        public double KAPANRATE { get; set; }
        public double KAPANAMOUNT { get; set; }

        public double EXPMAKPER { get; set; }
        public double EXPMAKCARAT { get; set; }
        public double EXPPOLPER { get; set; }
        public double EXPPOLCARAT { get; set; }
        public double EXPDOLLAR { get; set; }

        public Guid LOT_ID { get; set; }
        public string COMPLETEDATE { get; set; }

        public Int32 KAPANPCS { get; set; }
        public double KAPANCARAT { get; set; }
        public string STATUS { get; set; }

        public bool ISNOTAPPLYANYLOCK { get; set; }
        public double LABOURAMOUNT { get; set; }

        public string REMARK { get; set; }
        public string KAPANTYPE { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string Ope { get; set; }     //Used in KapanLiveStock Form(On Delete)

    }

}
