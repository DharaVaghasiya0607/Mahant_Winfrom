using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class EmailSettingProperty
    {
        public int EMAIL_ID { get; set; }
        public string EMAILTYPE { get; set; }
        public string SMTPEMAILUSERNAME { get; set; }
        public string SMTPEMAILPASSWORD { get; set; }
        public string SMTPDISPLAYNAME { get; set; }
        public string SMTPHOST { get; set; }
        public int SMTPPORT { get; set; }
        public bool SMTPENABLESSL { get; set; }
        public string TOEMAIL { get; set; }
        public string CCEMAIL { get; set; }
        public string BCCEMAIL { get; set; }
        public string SUBJECT { get; set; }
        public string HTMLBODY { get; set; }
        public bool ISACTIVE { get; set; }

    }

}
