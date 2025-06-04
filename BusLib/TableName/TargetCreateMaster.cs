using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
	public class TargetCreateMaster
	{
		public Guid TARGET_ID { get; set; }
		public Guid EMPLOYEE_ID { get; set; }
		public int FROMYEAR { get; set; }
		public int FROMMONTH { get; set; }
		public decimal SALETARGETDOLLAR { get; set; }
		public int NOOFCUST { get; set; }
		public int NOOFNEWCUST { get; set; }

		public string RETURNMESSAGETYPE { get; set; }
		public string RETURNMESSAGEDESC { get; set; }
		public string RETURNVALUE { get; set; }

	}
}
