using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
	public class LabChargesUploadProperty
	{
		public Guid LABCHARGEUPLOAD_ID { get; set; }
		public double FROMCARAT { get; set; }
		public double TOCARAT { get; set; }
		public string FROMCOLOR { get; set; }
		public int FROMCOLOR_ID { get; set; }
		public string TOCOLOR { get; set; }
		public int TOCOLOR_ID { get; set; }
		public string SERVICETYPE { get; set; }
		public int SERVICETYPE_ID { get; set; }
		public double AMOUNT { get; set; }
		public string APPLICABLEDATE { get; set; }
		public string TOAPPLICABLEDATE { get; set; }
		public double ADDLESS1 { get; set; }
		public double ADDLESS2 { get; set; }
		public string DIAMONDTYPE { get; set; }
		public string LAB { get; set; }
		public string CALCTYPE { get; set; }
	}
}
