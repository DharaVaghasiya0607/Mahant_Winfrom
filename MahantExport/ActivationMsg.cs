using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Diagnostics;

using Config = BusLib.Configuration.BOConfiguration;

namespace Licence
{
    public partial class ActivationMsg : Form
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        string mStrMessage = "";
        int IntActive = 0;

        public ActivationMsg(string pStrMessage)
        {
            InitializeComponent();
            mStrMessage = pStrMessage;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Activation_Load(object sender, EventArgs e)
        {
            lblMessage.Text = mStrMessage;
        }

    }
}
