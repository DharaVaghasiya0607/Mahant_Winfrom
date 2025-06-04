using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnPacketCreationProperty
    {
        public Guid PACKET_ID { get; set; }
        public Int32 PROCESS_ID{ get; set; }
        public Guid KAPAN_ID { get; set; }

        public Int32 PACKETNO{ get; set; }

        public Int64 JANGEDNO { get; set; }

        public Int64 EMPLOYEE_ID { get; set; }
        
        public Int32 DEPARTMENT_ID{ get; set; }
        public Int32 DESIGNATION_ID{ get; set; }

        public Int32 SHAPE_ID{ get; set; }
        public Int32 PURITY_ID { get; set; }
        public Int32 CHARNI_ID { get; set; }

        public Int32 ISSUEPCS { get; set; }
        public double ISSUECARAT { get; set; }
        public double SIZE { get; set; }

        public Int32 RETURNPCS { get; set; }
        public double RETURNCARAT { get; set; }


        public string ISSUEDATE { get; set; }

        public Int32 KACHAPCS{ get; set; }
        public double KACHACARAT{ get; set; }

        public Int32 CANCELPCS { get; set; }
        public double CANCELCARAT { get; set; }

        public double LOSSCARAT { get; set; }

        public double EXPPER { get; set; }
        public double EXPCARAT { get; set; }

        public double EXPLOSSPER { get; set; }
        public double EXPLOSSCARAT { get; set; }

        public string LABOURTYPE { get; set; }
        public double LABOURRATE { get; set; }

        public string RETURNDATE { get; set; }

        public double RETURNPER{ get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string ReturnValueJangedNo { get; set; }
        public string ReturnValuePacketID { get; set; }


        public string Ope { get; set; }     //Used in KapanLiveStock Form(On Delete)

    }

}
