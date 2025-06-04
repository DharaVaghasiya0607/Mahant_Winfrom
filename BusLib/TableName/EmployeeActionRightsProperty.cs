using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class EmployeeActionRightsProperty
    {
        public Guid EMPLOYEE_ID { get; set; }
        public string PRDTYPE_ID { get; set; }
        public bool ISFULLSTOCK { get; set; }
        public bool ISDEPTSTOCK { get; set; }
        public bool ISMYSTOCK { get; set; }
        public bool ISOTHERSTOCK { get; set; }
        public bool DEPTTRANSFER { get; set; }
        public bool EMPISSUE { get; set; }
        public bool REJECTIONTRANSFER { get; set; }
        public bool EMPRETURN { get; set; }
        public bool RETURNWITHSPLIT { get; set; }
        public string IPADDRESS { get; set; }
        public bool ALLOWALLIP { get; set; }
        public bool ISALLOWEXTRAMIN { get; set; }
        public double EXTRAMINPER { get; set; }

        public bool ISCONFIRMGRADER { get; set; }
        public bool ISGROUPJANGADNO { get; set; }

        public string BPRINTTYPE { get; set; }


        // Dhara : 21-04-2022
        public bool ISMAINFULLSTOCK { get; set; }
        public bool ISMAINDEPTSTOCK { get; set; }
        public bool ISMAINMYSTOCK { get; set; }
        public bool ISMAINOTHERSTOCK { get; set; }

        public bool ISSUBDEPTTRANSFER { get; set; }
        public bool ISSUBEMPISSUE { get; set; }
        public bool ISSUBREJECTIONTRANSFER { get; set; }
        public bool ISSUBEMPRETURN { get; set; }
        public bool ISSUBRETURNWITHSPLIT { get; set; }

        public bool ISSUBFULLSTOCK { get; set; }
        public bool ISSUBDEPTSTOCK { get; set; }
        public bool ISSUBMYSTOCK { get; set; }
        public bool ISSUBOTHERSTOCK { get; set; }
        // Dhara : 21-04-2022


        //#Krina : 19-10-2022
        public string UPLOADSERVERPATH { get; set; }
        public string UPLOADSERVERUSERNAME { get; set; }
        public string UPLOADSERVERPASSWORD { get; set; }
        //End : #Krina : 19-10-2022
        //#Krina : 17-11-2022
        public string QCMAINSERVERPATH { get; set; }
        public string QCMAINSERVERUSERNAME { get; set; }
        public string QCMAINSERVERPASSWORD { get; set; }
        //End : #Krina : 17-11-2022

        public string RAPPASSFORDISPDISC { get; set; }
        public bool RAPCHANGEEMPLOYEE { get; set; }
        public bool RAPCHANGEPACKETS { get; set; }
        public bool RAPUPDATEPREDICTION { get; set; }
        public int MAXPACKETSTOCK { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public bool ISALLOWINWARDINFO { get; set; }

        // #Dhara : 14-04-2023
        public bool ISDOLLARLOCK { get; set; }
        // End #Dhara : 14-04-2023 

        public string QCUSERWISESERVERPATH { get; set; }
        public string QCUSERWISEUSERNAME { get; set; }
        public string QCUSERWISEPASSWARD { get; set; }

    }
}

