using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
	public class StoneDayProperty
	{
		public int STONEDAY_ID { get; set; }
		public int SEQNO { get; set; }
		public decimal FROMDAY { get; set; }
		public decimal TODAY { get; set; }
		public string DAYNAME { get; set; }
		public string REMARK { get; set; }
		public Int64 ENTRYBY { get; set; }
		public string ENTRY_IP { get; set; }

		public string RETURNVALUE { get; set; }
		public string RETURNMESSAGETYPE { get; set; }
		public string RETURNMESSAGEDESC { get; set; }


	}
}
