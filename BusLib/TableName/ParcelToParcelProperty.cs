using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParcelToParcelProperty
    {
        public Guid FROMSTOCK_ID { get; set; }
        public Guid TOSTOCK_ID { get; set; }

        public string FROMSTOCKNO { get; set; }
        public string TOSTOCKNO { get; set; }

        public Int32 FROMSHAPE_ID { get; set; }
        public Int32 TOSHAPE_ID { get; set; }

        public string TOSIZENAME { get; set; }
        public Int32 TOSIZE_ID { get; set; }

        public string TOCLARITYNAME { get; set; }
        public Int32 TOCLARITY_ID { get; set; }

        public Int32 FROMDEPARTMENT_ID { get; set; }
        public Int32 TODEPARTMENT_ID { get; set; }

        public double FROMCARATBEFORE { get; set; }
        public double FROMCARATAFTER { get; set; }

        public double FROMPRICEPERCARATBEFORE { get; set; }
        public double FROMPRICEPERCARATAFTER { get; set; }

        public double FROMAMOUNTBEFORE { get; set; }
        public double FROMAMOUNTAFTER { get; set; }

        public double TOCARATBEFORE { get; set; }
        public double TOCARATAFTER { get; set; }

        public double TOPRICEPERCARATBEFORE { get; set; }
        public double TOPRICEPERCARATAFTER { get; set; }

        public double TOAMOUNTBEFORE { get; set; }
        public double TOAMOUNTAFTER { get; set; }

        public double TRANSFERCARAT { get; set; }
        public double TRANSFERPRICEPERCARAT { get; set; }
        public double TRAMSFERAMOUNT { get; set; }

        public string TYPE { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
