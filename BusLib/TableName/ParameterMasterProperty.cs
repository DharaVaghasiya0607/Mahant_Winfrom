using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParameterMasterProperty
    {
        public Int32 PARA_ID { get; set; }

        public string PARACODE { get; set; }
        public string PARANAME { get; set; }
        public string PARATYPE { get; set; }
        public string SHORTNAME { get; set; }

        public string PARAGROUP { get; set; }
        public string RAPAVALUE { get; set; }
        public string WEBDISPLAY { get; set; }
        public string RAPNETUPLOADCODE { get; set; }

        public Int32 SEQUENCENO{ get; set; }
        public string REMARK { get; set; }
        public string REQPRDTYPE_ID { get; set; }
        public string LABCODE { get; set; }
        public bool ISACTIVE { get; set; }
        public bool ISFINALISSUE { get; set; }
        public int DEPT_ID { get; set; }

        // #D: 13-01-2021
        public string IMAGEPATH { get; set; }
        public string SELIMAGEPATH { get; set; }
        // #D: 13-01-2021
        public string CLARITYWISEDEPARTMENT_ID { get; set; }

        public Int32 NIKUNJSEQNO { get; set; }
        public Int32 ANKITSEQNO { get; set; }

        public string JACCODE { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        //Add shiv 28-04-2022
        public string ACCCODE { get; set; }

        //Add shiv 16-07-2022
        public string ACCDESC { get; set; }

        //DHARA : 01-01-2024

        public string EXCELNAME { get; set; }

        //DHARA : 01-01-2024
    }

}
