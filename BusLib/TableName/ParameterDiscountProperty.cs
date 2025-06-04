using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParameterDiscountProperty
    {
        public double F_CARAT { get; set; }
        public double T_CARAT { get; set; }
        public string S_CODE { get; set; }
        public int SHAPE_ID { get; set; }

        public string C_CODE { get; set; }
        public int COLOR_ID { get; set; }
        
        public string C_NAME { get; set; }
        public string Q_CODE { get; set; }
        public string Q_NAME { get; set; }
        public string RAPDATE { get; set; }
        public string PARAMETER_ID { get; set; }
        public string PARAMETER_VALUE { get; set; }
        public string XML { get; set; }
        public double OLDVALUE { get; set; }
        public double NEWVALUE { get; set; }

        public string PRICINGTYPE { get; set; }

        public string FC_CODE { get; set; }
        public string FC_NAME { get; set; }
        public string FQ_CODE { get; set; }
        public string FQ_NAME { get; set; }

        public string TC_CODE { get; set; }
        public string TC_NAME { get; set; }
        public string TQ_CODE { get; set; }
        public string TQ_NAME { get; set; }

        //#P : 11-02-2020
        public string FFL_CODE { get; set; }
        public string FFL_NAME { get; set; }
        public string TFL_CODE { get; set; }
        public string TFL_NAME { get; set; }


        public string OPE { get; set; }

        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
