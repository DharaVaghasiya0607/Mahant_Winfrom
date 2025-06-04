using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class RapNetStockSyncProperty
    {

        public int ID { get; set; } 

        public string STOCKNO { get; set; }

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }

        public string SHAPE_ID { get; set; }
        public string SHAPENAME { get; set; }
     
        public string COLOR_ID { get; set; }
        public string COLORNAME { get; set; }
        public string CLARITY_ID { get; set; }
        public string CLARITYNAME { get; set; }
        public string CUT_ID { get; set; }
        public string CUTNAME { get; set; }
        public string POL_ID { get; set; }
        public string POLNAME { get; set; }
        public string SYM_ID { get; set; }
        public string SYMNAME { get; set; }
        public string FL_ID { get; set; }
        public string FLNAME { get; set; }
        public string FANCYCOLOR_ID { get; set; }
        public string FANCYCOLORNAME { get; set; }
        public string MILKY_ID { get; set; }
        public string MILKYNAME { get; set; }
        public string LOCATION_ID { get; set; }
        public string LOCATIONNAME { get; set; }

        public double FROMCARAT1 { get; set; }
        public double TOCARAT1 { get; set; }

        public double FROMCARAT2 { get; set; }
        public double TOCARAT2 { get; set; }

        public double FROMCARAT3 { get; set; }
        public double TOCARAT3 { get; set; }

        public double FROMCARAT4 { get; set; }
        public double TOCARAT4 { get; set; }

        public double FROMCARAT5 { get; set; }
        public double TOCARAT5 { get; set; }

        public string STONENO { get; set; }
        public string CERTINO { get; set; }
        public string SERIALNO { get; set; }
        public string MEMONO { get; set; }

        public string LAB_ID { get; set; }
        public string LABNAME { get; set; }

        public string WEBSTATUS_ID { get; set; }
        public string WEBSTATUSNAME { get; set; }

        public string BOX_ID { get; set; }
        public string BOXNAME { get; set; }

        public double FROMLENGTHPER { get; set; }
        public double TOLENGTHPER { get; set; }
        public double FROMWIDTHPER { get; set; }
        public double TOWIDTHPER { get; set; }
        public double FROMHEIGHTPER { get; set; }
        public double TOHEIGHTPER { get; set; }
        public double FROMDEPTHPER { get; set; }
        public double TODEPTHPER { get; set; }
        public double FROMTABLEPER { get; set; }
        public double TOTABLEPER { get; set; }

        public bool ISACTIVE { get; set; }

        public int ENTRYBY { get; set; }
        public string ENTRYDATE { get; set; }
        public string ENTRYIP { get; set; }

        public int UPDATEBY { get; set; }
        public string UPDATEDATE { get; set; }
        public string UPDATEIP { get; set; }

        public string LABREPORTNO { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

       
    }
}
