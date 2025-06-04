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
    public partial class Activation : Form
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

        int IntActive = 0;
        
        public Activation()
        {
            InitializeComponent();
        }

        public string BoardID { get; set; }


        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoardID.Text.Trim().Length == 0)
                {
                    Licence.ActivationMsg frm = new ActivationMsg("Board ID Is Required");
                    frm.ShowDialog();
                    return;
                }
                if (Val.ToString(CmbMode.SelectedItem).Length == 0)
                {
                    Licence.ActivationMsg frm = new ActivationMsg("Mode Is Required");
                    frm.ShowDialog();
                    return;
                }
                if (Val.ToInt(txtDays.Text) == 0)
                {
                    Licence.ActivationMsg frm = new ActivationMsg("Days Is Required For Activation");
                    frm.ShowDialog();
                    txtDays.Focus();
                    return;
                }

                //Axoneinfotech@Raj@Vipul@623
                if (txtActivation.Text == Val.ToString(txtActivation.Tag))
                {
                    if (BoardID == "")
                    {
                        Licence.ActivationMsg frm = new ActivationMsg("Device ID Not Exists. Please Contact System Administrator");
                        frm.ShowDialog();
                        return;
                    }

                    string Str = "Delete From MST_Axone With(RowLock) Where Board_ID = '" + BoardID + "'";
                    Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

                    string Days = txtDays.Text;
                    string EntryDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string ExpiryDate = DateTime.Now.AddDays(Val.ToInt(txtDays.Text)).ToString("dd/MM/yyyy");
                    string Mode = CmbMode.SelectedItem.ToString();

                    Str = "Insert Into MST_Axone (Board_ID,EntryDate,Days,ExpiryDate,Mode,ISActive) Values ('" + BoardID + "',Convert(varbinary(1000),'" + EntryDate + "'),Convert(varbinary(1000),'" + Days + "'),Convert(varbinary(1000),'" + ExpiryDate + "'),Convert(varbinary(100),'" + Mode + "' ),1)";
                    Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

                    if (System.IO.File.Exists(Application.StartupPath + "\\key.lic"))
                    {
                        System.IO.File.Delete(Application.StartupPath + "\\key.lic");
                    }
                    string[] createText = { BoardID };

                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(Application.StartupPath + "\\key.lic"))
                    {
                        sw.WriteLine(BoardID);
                    }
                    Licence.ActivationMsg frm1 = new ActivationMsg("Exe Activation For " + txtDays.Text + " Days Done Successfully");
                    frm1.ShowDialog();
                        
                    Application.Exit();
                }
                else
                {
                    Licence.ActivationMsg frm1 = new ActivationMsg("Sorry....You Have Invalid Activation Key");
                    frm1.ShowDialog();
                    txtActivation.Focus();
                }
            }
            catch (Exception ex)
            {
                Licence.ActivationMsg frm1 = new ActivationMsg(ex.Message);
                frm1.ShowDialog();
            }

        }

        public Boolean CheckActivation()
        {
            try
            {
                if (System.IO.File.Exists(Application.StartupPath + "\\key.lic") == false)
                {
                    Licence.ActivationMsg frm1 = new ActivationMsg("Licence File Generated From Server\n\nKindly Contact to Software Administrator\n\nThank you");
                    frm1.ShowDialog();
                    return false;
                }

                string[] StrBoard = System.IO.File.ReadAllLines(Application.StartupPath + "\\key.lic");
                if (StrBoard.Length == 0)
                {
                    Licence.ActivationMsg frm1 = new ActivationMsg("Licence Key Generated From Server\n\nKindly Contact to Software Administrator\n\nThank you");
                    frm1.ShowDialog();

                    return false;
                }

                string Str = "Select Convert(Varchar(100),EntryDate) As EntryDate, Convert(Varchar(100),ExpiryDate) As ExpiryDate, Convert(Varchar(100),Days) As Days,ISActive From MST_Axone With(RowLock) Where Board_ID = '" + BoardID + "'";
                DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
                if (DRow == null)
                {
                    Licence.ActivationMsg frm1 = new ActivationMsg("No Activation Found For This Software. Please Contact System Administrator");
                    frm1.ShowDialog();
                    return false;
                }
                else
                {
                    Str = "Select GETDATE() ServerDate";
                    DataRow DRServerDate = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

                    string EntryDate = Val.ToString(DRow["EntryDate"]);
                    string ExpiryDate = Val.ToString(DRow["ExpiryDate"]);
                    string CurrentDate = DateTime.Parse(Val.ToString(DRServerDate["ServerDate"])).ToString("dd/MM/yyyy");
                    bool ISActive = Val.ToBoolean(DRow["ISActive"]);
                    double PendingDays = Val.DateDiff(DateInterval.Day, CurrentDate, ExpiryDate);
                    if (ISActive == false)
                    {
                        Licence.ActivationMsg frm1 = new ActivationMsg("Your Exe Trial Period For Activate\n\nKindly Contact to Software Administrator\n\nThank you");
                        frm1.ShowDialog();
                        return false;
                    }

                    if (PendingDays < 0)
                    {
                        Str = "Update MST_Axone With(RowLock) Set ISActive = 0 Where Board_ID = '" + BoardID + "'";
                        Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
                        Licence.ActivationMsg frm1 = new ActivationMsg("Your Trial Period Is Expired\n\nKindly Contact to Software Administrator\n\nThank you");
                        frm1.ShowDialog();
                        return false;
                    }
                    else if (PendingDays < 5)
                    {
                        Licence.ActivationMsg frm1 = new ActivationMsg("Your Application Subscription Is Expired In (" + PendingDays.ToString() + " days)");
                        frm1.ShowDialog();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                
            }
            catch (Exception EX)
            {
                Licence.ActivationMsg frm1 = new ActivationMsg("Activation Is Pending");
                frm1.ShowDialog();
                           
                return false;
            }
            
        }

        private void Activation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Activation_Load(object sender, EventArgs e)
        {
            txtBoardID.Text = BoardID;
            CmbMode.SelectedIndex = 0;
            txtActivation.Focus();
        }

    }
}
