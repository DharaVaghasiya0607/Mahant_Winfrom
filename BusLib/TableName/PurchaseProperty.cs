using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class PurchaseProperty
    {
        public int INWARDNO { get; set; }
        public string INWARDDATE { get; set; }
        public Guid PARTY_ID { get; set; }
        public double CONVERSIONRATE { get; set; }
        public int TERMS_ID { get; set; }
        public int TERMS { get; set; }
        public double DISCOUNTPER { get; set; }
        public double FDISCOUNTPER { get; set; }
        public double DISCOUNTAMT { get; set; }
        public double FDISCOUNTAMT { get; set; }
        public double BROKERAGEPER { get; set; }
        public double FBROKRAGEPER { get; set; }
        public double BROKERAGEAMT { get; set; }
        public double FBROKRAGEAMT { get; set; }
        public Guid BROKER_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string XMLDETSTR { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnValue { get; internal set; }
        public string DUEDATE { get; set; }
        public Guid PURCHASE_ID { get; set; }
        public double GROSSAMOUNT { get; set; }
        public double NETAMOUNT { get; set; }
        public int CURRENCY_ID { get; set; }
        public Guid Packet_ID { get; set; }
        public int PacketNo { get; set; }
        public Guid TRN_ID { get; set; }
        public string TRANSFERTYPE { get; set; }
        public string PacketDate { get; set; }
        public string FILENAME { get; set; }
        public Guid UPLOAD_ID { get; set; }
    }
}
