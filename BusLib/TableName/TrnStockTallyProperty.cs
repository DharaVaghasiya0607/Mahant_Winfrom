using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnStockTallyProperty
    {

        public Guid STOCKTALLY_ID { get; set; }
        public string STOCKTALLYDATE { get; set; }
        public Guid PACKET_ID { get; set; }
        public string PARTYSTOCKNO { get; set; }
        public Guid STOCK_ID { get; set; }
        public string GIACONTROLNO { get; set; }
        public string RFIDTAGNO { get; set; } 
        public double CARAT { get; set; }
        public int PROCESS_ID { get; set; }
        public int BOX_ID { get; set; }

        public string BOXNAME { get; set; }

        public string FOUNDSTATUS { get; set; }
        public string STATUS { get; set; }
        public string MAINCATAGORY { get; set; }

        public int SERIALNO { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
