using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnReportProperty
    {

        public string FROM_PACKET_NO { get; set; }
        public string TO_PACKET_NO { get; set; }
        public string ACTIVITY_TYPE { get; set; }

       

        int Color_ID { get; set; }
        public string Color_Code { get; set; }
        public string Color_Name { get; set; }
        public bool Active { get; set; }
        public int Sequence_No { get; set; }
        public string Note { get; set; }
        public Guid Record_ID { get; set; }

        public string Transaction_Remark { get; set; }
        public Guid Parent_Record_Code { get; set; }

        public string KAPAN_ID { get; set; }
        public string KAPANNAME { get; set; }

        public string PACKETNO { get; set; }
        public string PACKETTAG { get; set; }

        public string JANGED_NO { get; set; }
        public string EMPLOYEE_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
    
        public string FROM_DEPARTMENT_ID { get; set; }
        public string TO_DEPARTMENT_ID { get; set; }

        public string FROM_EMPLOYEE_ID { get; set; }
        public string TO_EMPLOYEE_ID { get; set; }

        public string MAINMARKER_ID { get; set; }

        public string COMPANY_ID { get; set; }
        public string FROM_COMPANY_ID { get; set; }
        public string TO_COMPANY_ID { get; set; }
        public string LOCATION_ID { get; set; }
        public string FROM_LOCATION_ID { get; set; }
        public string TO_LOCATION_ID { get; set; }
        public string FROM_PROCESS_ID { get; set; }
        public string TO_PROCESS_ID { get; set; }
        public string ISSUEFROM_DATE { get; set; }
        public string ISSUETO_DATE { get; set; }
        public string CONFFROM_DATE { get; set; }
        public string CONFTO_DATE { get; set; }
        public string STOCKFROM_DATE { get; set; }
        public string STOCKTO_DATE { get; set; }
        public string RETURNFROM_DATE { get; set; }
        public string RETURNTO_DATE { get; set; }
        public string SUPPLIER_ID { get; set; }
        public string MANAGER_ID { get; set; }
        public string FROM_MANAGER_ID { get; set; }
        public string TO_MANAGER_ID { get; set; }
        public string MACHINE_ID { get; set; }
        public string SHIFT_ID { get; set; }
        public string IMPORT_TYPE { get; set; }
        public string INVOICENO { get; set; }
        public string IMPORTFROMDATE { get; set; }
        public string IMPORTTODATE { get; set; }
        public string GROUP_BY { get; set; }
        public string PARTY_INVOICE_NO { get; set; }
        public string SHAPE_ID { get; set; }
        public string SOURCE_ID { get; set; }
        public string MSIZE_ID { get; set; }
        public string SIGHT_ID { get; set; }
        public string QUALITY_ID { get; set; }
        
        public string PACKET_TYPE { get; set; }
        public string FROMDEPT_TYPE { get; set; }
        public string TODEPT_TYPE { get; set; }
        public string CUTTYPE_ID { get; set; }
        public string CUT_ID { get; set; }
        public string POL_ID { get; set; }
        public string SYM_ID { get; set; }
        public string FL_ID { get; set; }
        public string CLARITY_ID { get; set; }
        public string ARTICLE_ID { get; set; }
        public string COLOR_ID { get; set; }
        public string MODEL_ID { get; set; }
        public string LOT_NO { get; set; }
        public string SIEVE_ID { get; set; }
        public string REJECTION_ID { get; set; }
     
        public string BARCODE { get; set; }
        public string Ope { get; set; }
    }
}
