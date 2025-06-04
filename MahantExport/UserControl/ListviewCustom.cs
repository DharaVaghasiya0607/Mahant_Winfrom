using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MahantExport.UserControl
{
    public partial class ListviewCustom : DevExpress.XtraEditors.XtraUserControl
    {
        public ListviewCustom()
        {
            InitializeComponent();
        }
        private DataTable _DTab = new DataTable();

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
                GrpList.Text = _HeaderText;
            }
        }

        private bool _AllCheckBoxVisible;
        public bool AllCheckBoxVisible
        {
            get
            {
                return _AllCheckBoxVisible;
            }
            set
            {
                _AllCheckBoxVisible = value;
                ChkDelSelAll.Visible = _AllCheckBoxVisible;
            }
        }

        private string _DisplayMember;
        public string DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                _DisplayMember = value;
                CheckedList.DisplayMember = _DisplayMember;
            }
        }
        private string _ValueMember;
        public string ValueMember
        {
            get
            {
                return _ValueMember;
            }
            set
            {
                _ValueMember = value;
                CheckedList.ValueMember = _ValueMember;
            }
        }

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

                CheckedList.Items.Clear();

                if (DTab == null)
                {
                    return;
                }
                if (DTab.Columns.Count > 1)
                {
                    CheckedList.DataSource = _DTab;
                    //CheckedList.DisplayMember = _DTab.Columns[2].ColumnName.ToString();
                    //CheckedList.ValueMember = _DTab.Columns[0].ColumnName.ToString();
                }
                else
                {
                    CheckedList.DataSource = _DTab;
                    //CheckedList.DisplayMember = _DTab.Columns[0].ColumnName.ToString();
                    //CheckedList.ValueMember = _DTab.Columns[0].ColumnName.ToString();
                }
                // CheckedList.SelectedIndex = -1;
                //DevCustomListView.SetColumn("", DevCustomListView.Width.ToString());
                //foreach (DataRow DRow in _DTab.Rows)
                //{
                //    ListViewItem ListViewItem = new ListViewItem();
                //    ListViewItem.Text = DRow[1].ToString();
                //    ListViewItem.Tag = DRow[0].ToString();
                //    DevCustomListView.Items.Add(ListViewItem);
                //}
            }

        }


        public void ClearFocus()
        {

            //CheckedList.Select();
            //   CheckedList.SelectedIndex = -1;
        }

        public string GetSelectedReportTagValues()
        {
            string Str = "";

            if (CheckedList.DataSource != null)
            {
                foreach (object itemChecked in CheckedList.CheckedItems)
                {

                    DataRowView castedItem = itemChecked as DataRowView;
                    if (Str == string.Empty)
                    {
                        Str = castedItem[CheckedList.ValueMember].ToString();
                    }
                    else
                    {
                        Str = Str + "," + castedItem[CheckedList.ValueMember].ToString();
                    }

                }
            }

            return Str;

        }
        public void DeSelectAll()
        {
            CheckedList.UnCheckAll();
            ChkDelSelAll.Checked = false;
            //  CheckedList.Dispose();


        }

        public string GetSelectedReportTextValues()
        {
            String Str = "";
            if (CheckedList.DataSource != null)
            {
                foreach (object itemChecked in CheckedList.CheckedItems)
                {

                    DataRowView castedItem = itemChecked as DataRowView;
                    if (Str == string.Empty)
                    {
                        Str = "'" + Convert.ToString(castedItem[CheckedList.DisplayMember].ToString()) + "'";
                    }
                    else
                    {
                        Str = Str + "," + "'" + Convert.ToString(castedItem[CheckedList.DisplayMember].ToString()) + "'";
                    }

                }
            }
            return Str;
        }
        public string GetSelectedReportTagValuesWithCoat()
        {
            string Str = "";

            if (CheckedList.DataSource != null)
            {
                foreach (object itemChecked in CheckedList.CheckedItems)
                {

                    DataRowView castedItem = itemChecked as DataRowView;
                    if (Str == string.Empty)
                    {
                        Str = "'" + castedItem[CheckedList.ValueMember].ToString() + "'";
                    }
                    else
                    {
                        Str = Str + ",'" + castedItem[CheckedList.ValueMember].ToString() + "'";
                    }

                }
            }

            return Str;

        }

        #region Property Settings

        private Boolean _AllowTabKeyOnEnter = false;

        public Boolean AllowTabKeyOnEnter
        {
            get { return _AllowTabKeyOnEnter; }
            set { _AllowTabKeyOnEnter = value; }
        }

        private string _ToolTips = "";
        /// <summary>
        /// Tool Tips
        /// </summary>
        public string ToolTips
        {
            get { return _ToolTips; }
            set
            {
                _ToolTips = value;
                DevExpress.Utils.ToolTipController TT1 = new DevExpress.Utils.ToolTipController();

                TT1.SetToolTip(this, _ToolTips);
            }
        }

        #endregion


        #region Opoeration


        public void SelectAll()
        {
            CheckedList.CheckAll();
            //for (int IntI = 0; IntI < this.Items.Count; IntI++)
            //{
            //    this.Items[IntI].Checked = true;
            //}
        }

        public void SetSelectedCheckBox(string pStrValueString)
        {
            DeSelectAll();
            if (pStrValueString == "")
            {
                return;
            }

            string[] StrSplit = pStrValueString.Split(',');

            foreach (string Str in StrSplit)
            {
                for (int IntI = 0; IntI < CheckedList.ItemCount; IntI++)
                {
                    if (Str == CheckedList.GetItemValue(IntI).ToString())
                    {
                        CheckedList.SetItemChecked(IntI, true);
                    }
                }
            }

        }

        public void SetColumn(string pStrColumnNames, string pStrWidths)
        {
            string[] StrColumns = pStrColumnNames.Split(',');
            string[] StrWidth = pStrWidths.Split(',');

            if (StrColumns.Length != StrWidth.Length)
            {
                Global.Message("ListView ...Check Column Names & Width");

                return;
            }

            //for (int IntI = 0; IntI < StrColumns.Length; IntI++)
            //{
            //    this.Columns.Add(StrColumns[IntI], Convert.ToInt32(StrWidth[IntI]));
            //}

        }

        private void ChkDelSelAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (ChkDelSelAll.Checked == true)
            {

                CheckedList.CheckAll();

            }
            else
            {
                CheckedList.UnCheckAll();

            }
        }

        private void CheckedList_Enter(object sender, EventArgs e)
        {

        }



        #endregion

        private void CheckedList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Add By Milan For Selected False 27102021

        public void RemoveSelectedCheckBox(string pStrValueString)
        {
            DeSelectAll();
            if (pStrValueString == "")
            {
                return;
            }

            string[] StrSplit = pStrValueString.Split(',');

            foreach (string Str in StrSplit)
            {
                for (int IntI = 0; IntI < CheckedList.ItemCount; IntI++)
                {
                    if (Str == CheckedList.GetItemValue(IntI).ToString())
                    {
                        CheckedList.SetItemChecked(IntI, false);
                    }
                }
            }

        }
        //End



    }
}
