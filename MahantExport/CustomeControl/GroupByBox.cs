using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;

namespace MahantExport.CustomeControl
{
    public partial class GroupByBox : DevExpress.XtraEditors.XtraUserControl
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public GroupByBox()
        {
            InitializeComponent();
        }
        private DataTable _DTab = new DataTable();
        public DataTable DTab
        {
            get { return _DTab; }
            set
            {
                _DTab = value;

                if (_DTab == null)
                {
                    return;
                }

                ListTo.Items.Clear();
                ListFrom.Items.Clear();

                ListFrom.Columns.Clear();
                ListTo.Columns.Clear();

                if (DTab == null)
                {
                    return;
                }

                ListFrom.Columns.Add("Col", ListFrom.Width);
                ListTo.Columns.Add("Col1", ListFrom.Width);
                foreach (DataRow DRow in _DTab.Rows)
                {
                    if (Val.ToBoolean(DRow["ISGROUP"].ToString()) == true)
                    {
                        ListViewItem ListViewItem = new ListViewItem();
                        ListViewItem.Text = DRow["CAPTION"].ToString().ToUpper();
                        ListViewItem.Tag = DRow["FIELDNAME"].ToString();
                        ListFrom.Items.Add(ListViewItem);
                    }
                }

            }

        }


        private string _HeaderText;
        public string HeaderText
        {
            get
            {
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
                grpBack.Text = _HeaderText;
            }
        }

        public string GetTextValue
        {
            get
            {

                string StrValue = "";
                foreach (ListViewItem Item in ListTo.Items)
                {
                    StrValue = StrValue + Item.Text.ToString() + ",";
                }
                if (StrValue != "")
                {
                    StrValue = StrValue.Substring(0, StrValue.Length - 1);
                }
                return StrValue;
            }
        }

        public string GetTagValue
        {
            get
            {
                string StrValue = "";
                foreach (ListViewItem Item in ListTo.Items)
                {
                    if (Item.Tag.ToString() != "")
                    {
                        StrValue = StrValue + Item.Tag.ToString() + ",";
                    }
                }
                if (StrValue != "")
                {
                    StrValue = StrValue.Substring(0, StrValue.Length - 1);
                }
                return StrValue;
            }
        }

        private string _Default;

        public string Default
        {
            get { return _Default; }
            set
            {
                _Default = value;
                if (Default == null)
                {
                    return;
                }
                string[] StrSplit = Default.Split(',');

                for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                {
                    foreach (ListViewItem iTem in ListFrom.Items)
                    {
                        if (iTem.Text.ToUpper() == StrSplit[IntI].ToUpper() || iTem.Tag.ToString().ToUpper() == StrSplit[IntI].ToUpper())
                        {
                            iTem.Selected = true;
                            BtnForward_Click(null, null);
                            break;
                        }
                    }
                }
            }
        }
        public void SetDefauleGroupBy(string pStrGroupByTag, string pStrGroupByCaption)
        {
            if (pStrGroupByTag != null)
            {
                ListTo.Items.Clear();
                string[] StrCaption = pStrGroupByCaption.Split(',');
                string[] StrTag = pStrGroupByTag.Split(',');

                for (int IntI = 0; IntI < StrTag.Length; IntI++)
                {
                    if (StrTag[IntI] != "")
                    {
                        ListViewItem ToITem = new ListViewItem();
                        ToITem.Text = StrCaption[IntI];
                        ToITem.Tag = StrTag[IntI];
                        ListTo.Items.Add(ToITem);
                    }
                   
                }
            }

        }

        private void BtnForward_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem iTem in ListFrom.SelectedItems)
            {
                ListViewItem ToITem = new ListViewItem();
                ToITem.Text = iTem.Text;
                ToITem.Tag = iTem.Tag;

                bool ISExists = false;
                foreach (ListViewItem iTem2 in ListTo.Items)
                {
                    if (ToITem.Text == iTem2.Text)
                    {
                        ISExists = true;
                        break;
                    }
                }
                if (ISExists == false)
                {
                    ListTo.Items.Add(ToITem);
                }
            }
        }

        private void BtnForwardAll_Click(object sender, EventArgs e)
        {
            ListTo.Items.Clear();
            foreach (ListViewItem iTem in ListFrom.Items)
            {
                ListViewItem ToITem = new ListViewItem();
                ToITem.Text = iTem.Text;
                ToITem.Tag = iTem.Tag;
                ListTo.Items.Add(ToITem);
            }
        }


        private void lstFrom_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnForward_Click(null, null);
        }

        private void BtnBackward_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem iTem in ListTo.SelectedItems)
            {
                //ListViewItem FromITem = new ListViewItem();
                //FromITem.Text = iTem.Text;

                ListTo.Items.Remove(iTem);
                //ListFrom.Items.Add(FromITem);
            }
        }

        private void BtnBackWardAll_Click(object sender, EventArgs e)
        {
            ListTo.Items.Clear();
        }

        private void lstTo_DoubleClick(object sender, EventArgs e)
        {
            BtnBackward_Click(null, null);
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (ListTo.SelectedItems.Count == 0)
            {
                return;
            }
            var currentIndex = ListTo.SelectedItems[0].Index;
            var item = ListTo.SelectedItems[0];
            if (currentIndex > 0)
            {
                ListTo.Items.RemoveAt(currentIndex);
                ListTo.Items.Insert(currentIndex - 1, item);
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (ListTo.SelectedItems.Count == 0)
            {
                return;
            }
            var currentIndex = ListTo.SelectedItems[0].Index;
            var item = ListTo.SelectedItems[0];
            if (currentIndex < ListTo.Items.Count - 1)
            {
                ListTo.Items.RemoveAt(currentIndex);
                ListTo.Items.Insert(currentIndex + 1, item);
            }
        }
        private void ListFrom_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnForward_Click(sender, null);
        }

        private void ListTo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnBackward_Click(sender, null);
        }

    }


}
