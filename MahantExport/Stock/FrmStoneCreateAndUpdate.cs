using BusLib.Configuration;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MahantExport.Utility;
using BusLib;
using DevExpress.XtraGrid.Columns;

namespace MahantExport.Stock
{
    public partial class FrmStoneCreateAndUpdate : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BODevGridSelection ObjGridSelection;

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();

        DataTable DTabParameter = new DataTable();
        DataTable DTabStcok = new DataTable();
        DataTable DTabStcokGrid = new DataTable();


        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.WhiteSmoke;

        #region Property Setting

        public FrmStoneCreateAndUpdate()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            DataTable DTabRapDate = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.RAPDATE);
            DTabRapDate.DefaultView.Sort = "RAPDATE DESC";
            DTabRapDate = DTabRapDate.DefaultView.ToTable();

            CmbRapDate.Items.Clear();
            foreach (DataRow DRow in DTabRapDate.Rows)
            {
                CmbRapDate.Items.Add(DateTime.Parse(Val.ToString(DRow["RAPDATE"])).ToString("dd/MM/yyyy"));
            }
            CmbRapDate.SelectedIndex = 0;

            DataRow[] DR = null;
            DataTable DTabPrdType = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PRDTYPE);
            DR = DTabPrdType.Select("PRDTYPE_ID IN(3,4,5,6)");
            foreach (DataRow DRow in DR)
            {
                DTabPrdType.Rows.Remove(DRow);
            }
            //End: Dhara: 18-06-2020

            DTabPrdType.DefaultView.Sort = "SEQUENCENO";
            DTabPrdType = DTabPrdType.DefaultView.ToTable();

            CmbPrdType.DataSource = DTabPrdType;
            CmbPrdType.DisplayMember = "PRDTYPENAME";
            CmbPrdType.ValueMember = "PRDTYPE_ID";

            CmbPrdType_SelectedIndexChanged(null, null);

            this.Show();
            SetControl();

            //Added By Gunjan:28/02/2024

            if (MainGrdDetail.RepositoryItems.Count == 9)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetail;
                ObjGridSelection.ISBoolApplicableForPageConcept = true;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            }
            else
            {
                if (ObjGridSelection != null)
                    ObjGridSelection.ClearSelection();
            }
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            DTabStcokGrid.Columns.Add("STOCK_ID", typeof(string));
            DTabStcokGrid.Columns.Add("SRNO", typeof(Int32));
            DTabStcokGrid.Columns.Add("PRDTYPE", typeof(string));
            DTabStcokGrid.Columns.Add("EMPLOYEE", typeof(string));
            DTabStcokGrid.Columns.Add("PARTYSTOCKNO", typeof(string));
            DTabStcokGrid.Columns.Add("CARAT", typeof(double));
            DTabStcokGrid.Columns.Add("DIAMONDTYPE", typeof(string));
            DTabStcokGrid.Columns.Add("RAPPOPRT", typeof(double));
            DTabStcokGrid.Columns.Add("DISCOUNT", typeof(double));
            DTabStcokGrid.Columns.Add("PRICEPERCTS", typeof(double));
            DTabStcokGrid.Columns.Add("AMOUNT", typeof(double));
            DTabStcokGrid.Columns.Add("SHAPE", typeof(string));
            DTabStcokGrid.Columns.Add("COLOR", typeof(string));
            DTabStcokGrid.Columns.Add("CLARITY", typeof(string));
            DTabStcokGrid.Columns.Add("CUT", typeof(string));
            DTabStcokGrid.Columns.Add("POL", typeof(string));
            DTabStcokGrid.Columns.Add("SYM", typeof(string));
            DTabStcokGrid.Columns.Add("FLR", typeof(string));
            DTabStcokGrid.Columns.Add("BLACKINC", typeof(string));
            DTabStcokGrid.Columns.Add("TABLEINC", typeof(string));
            DTabStcokGrid.Columns.Add("MILKY", typeof(string));
            DTabStcokGrid.Columns.Add("LUSTER", typeof(string));
            DTabStcokGrid.Columns.Add("PAVILIONOPEN", typeof(string));
            DTabStcokGrid.Columns.Add("COLORSHADE", typeof(string));
            DTabStcokGrid.Columns.Add("TABLEOPENINC", typeof(string));
            DTabStcokGrid.Columns.Add("CROWNOPEN", typeof(string));
            DTabStcokGrid.Columns.Add("PAVOPEN", typeof(string));
            //End As Gunjan
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion

        #region Buttion Desion

        public void DesignSystemButtion(Panel PNL, string pStrParaType, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {
            DataRow[] UDRow = DTabParameter.Select("ParaType = '" + pStrParaType + "'");

            if (UDRow.Length == 0)
            {
                return;
            }

            DataTable DTab = UDRow.CopyToDataTable();
            DTab.DefaultView.Sort = "SequenceNo";
            DTab = DTab.DefaultView.ToTable();

            PNL.Controls.Clear();

            int IntI = 0;           
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;

                // AutoSize property should be set to true to adjust size based on content
                ValueList.AutoSize = true;
                ValueList.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                // You can still set the height manually, as you want
                ValueList.Height = pIntHeight;

                ValueList.Tag = DRow["PARA_ID"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["PARACODE"]);
                ValueList.ToolTips = toolTips;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                if (IntI == 0)
                {
                    // Set selected style for the first button
                    ValueList.ForeColor = mSelectedColor; // Define `mSelectedColor` for selected text color
                    ValueList.BackColor = mSelectedBackColor; // Define `mSelectedBackColor` for selected background
                    ValueList.AccessibleName = "true";
                }
                else
                {
                    // Set default style for non-selected buttons
                    ValueList.ForeColor = mDeSelectColor;
                    ValueList.BackColor = mDSelectedBackColor;
                    ValueList.AccessibleName = "false";
                }


                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            try
            {
                //added and comment by Gunjan
                try
                {

                    AxonContLib.cButton btn = (AxonContLib.cButton)sender;

                    if (btn.ForeColor == mSelectedColor)
                    {
                        btn.ForeColor = mDeSelectColor;
                        btn.BackColor = mDSelectedBackColor;
                        btn.AccessibleName = "true";
                    }
                    else
                    {
                        btn.ForeColor = mSelectedColor;
                        btn.BackColor = mSelectedBackColor;
                        btn.AccessibleName = "true";

                        AxonContLib.cButton rd = (AxonContLib.cButton)sender;
                        if (rd.ToolTips == "SHAPE")
                        {
                            PanelShape.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });

                        }
                        else if (rd.ToolTips == "COLOR")
                        {
                            PanelColor.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "CLARITY")
                        {
                            PanelClarity.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }

                        else if (rd.ToolTips == "CUT")
                        {
                            PanelCut.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });

                        }
                        else if (rd.ToolTips == "POL")
                        {
                            PanelPol.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "SYM")
                        {
                            PanelSym.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "FL")
                        {
                            PanelFL.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "TableInc")
                        {
                            PanelTableInc.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "TABLEBLACKINC")
                        {
                            PanelBlackInc.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "TABLEOPENINC")
                        {
                            PanelTableOpen.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "LUSTER")
                        {
                            PanelLuster.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "MILKY")
                        {
                            PanelMilky.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "Lab" +
                            "")
                        {
                            PanelLab.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "COLORSHADE" +
                            "")
                        {
                            PanelColorShade.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "CROWN_OPEN" +
                            "")
                        {
                            PanelCrownOpen.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }
                        else if (rd.ToolTips == "PAVILION_OPEN" +
                            "")
                        {
                            PanelCrownOpen.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                            {
                                a.AccessibleName = "false";
                                a.ForeColor = mDeSelectColor;
                                a.BackColor = mDSelectedBackColor;
                            });
                        }

                        FindRap();

                    }

                }
                catch (Exception EX)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Global.MessageError(EX.Message);
                    return;
                }

                #region Comment

                //try
                //{
                //    AxonContLib.cButton btn = (AxonContLib.cButton)sender;

                //    if (btn.ForeColor == mSelectedColor)
                //    {
                //        btn.ForeColor = mDeSelectColor;
                //        btn.BackColor = mDSelectedBackColor;
                //        btn.AccessibleName = "true";
                //    }
                //    else
                //    {
                //        btn.ForeColor = mSelectedColor;
                //        btn.BackColor = mSelectedBackColor;
                //        btn.AccessibleName = "true";
                //    }
                //    if (MainGrdDetail.Enabled == false)
                //    {
                //        Global.MessageError("Grid Is Unable To Update");
                //        return;
                //    }
                //}
                //catch (Exception EX)
                //{
                //    this.Cursor = Cursors.WaitCursor;
                //    Global.MessageError(EX.Message);
                //    return;
                //}

                //AxonContLib.cButton btn = (AxonContLib.cButton)sender;
                //btn.ForeColor = mSelectedColor;
                //btn.BackColor = mSelectedBackColor;
                //btn.AccessibleName = "true";

                //AxonContLib.cButton rd = (AxonContLib.cButton)sender;
                //// //if (rd.ToolTips == "SHAPE")
                //// //{
                ////     PanelShape.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                ////     {
                ////         a.AccessibleName = "false";
                ////         a.ForeColor = mDeSelectColor;
                ////         a.BackColor = mDSelectedBackColor;
                ////     });

                ////     if (btn.Text == "R" || btn.Text == "BR")
                ////     {
                ////         PanelCut.Controls.OfType<AxonContLib.cButton>().ToList().ForEach(a =>
                ////         {
                ////             a.AccessibleName = "false";
                ////             a.ForeColor = mDeSelectColor;
                ////             a.BackColor = mDSelectedBackColor;
                ////         });
                ////         PanelPol.Controls.OfType<AxonContLib.cButton>().ToList().ForEach(a =>
                ////         {
                ////             a.AccessibleName = "false";
                ////             a.ForeColor = mDeSelectColor;
                ////             a.BackColor = mDSelectedBackColor;
                ////         });
                ////         PanelSym.Controls.OfType<AxonContLib.cButton>().ToList().ForEach(a =>
                ////         {
                ////             a.AccessibleName = "false";
                ////             a.ForeColor = mDeSelectColor;
                ////             a.BackColor = mDSelectedBackColor;
                ////         });

                ////         AxonContLib.cButton rbCut = PanelCut.Controls.OfType<AxonContLib.cButton>().FirstOrDefault();
                ////         rbCut.AccessibleName = "true";
                ////         rbCut.ForeColor = mSelectedColor;
                ////         rbCut.BackColor = mSelectedBackColor;

                ////         AxonContLib.cButton rbPol = PanelPol.Controls.OfType<AxonContLib.cButton>().FirstOrDefault();
                ////         rbPol.AccessibleName = "true";
                ////         rbPol.ForeColor = mSelectedColor;
                ////         rbPol.BackColor = mSelectedBackColor;

                ////         AxonContLib.cButton rbSym = PanelSym.Controls.OfType<AxonContLib.cButton>().FirstOrDefault();
                ////         rbSym.AccessibleName = "true";
                ////         rbSym.ForeColor = mSelectedColor;
                ////         rbSym.BackColor = mSelectedBackColor;
                ////     }
                ////     //else //If Fancy Shape then Select Vg-Vg-Vg in CPS AS per Discuss with client : 17-06-2022
                ////     //{
                ////     //    Fetch_SetRadioButton(PanelCut, Val.ToInt(110), true); // 373 = VG
                ////     //    Fetch_SetRadioButton(PanelPol, Val.ToInt(346), true); // 373 = VG
                ////     //    Fetch_SetRadioButton(PanelSym, Val.ToInt(373), true); // 373 = VG
                ////     //}
                ////// }
                /////
                //if (rd.ToolTips == "SHAPE")
                //{
                //    PanelColor.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "COLOR")
                //{
                //    PanelColor.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "CLARITY")
                //{
                //    PanelClarity.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}

                //else if (rd.ToolTips == "CUT")
                //{
                //    PanelCut.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });

                //}
                //else if (rd.ToolTips == "POL")
                //{
                //    PanelPol.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "SYM")
                //{
                //    PanelSym.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "FL")
                //{
                //    PanelFL.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "TableInc")
                //{
                //    PanelTableInc.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "TABLEBLACKINC")
                //{
                //    PanelBlackInc.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "TABLEOPENINC")
                //{
                //    PanelTableOpen.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "LUSTER")
                //{
                //    PanelLuster.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "MILKY")
                //{
                //    PanelMilky.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                //else if (rd.ToolTips == "Lab" +
                //    "")
                //{
                //    PanelLab.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Val.ToInt(btn.Tag)).ToList().ForEach(a =>
                //    {
                //        a.AccessibleName = "false";
                //        a.ForeColor = mDeSelectColor;
                //        a.BackColor = mDSelectedBackColor;
                //    });
                //}
                #endregion
                //End as Gunjan


            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }

        }

        public void Fetch_SetRadioButton(FlowLayoutPanel pn, int Value, bool ISReset)
        {


            pn.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Value).ToList().ForEach(a =>
            {
                a.AccessibleName = "false";
                a.ForeColor = mDeSelectColor;
                a.BackColor = mDSelectedBackColor;
            });

            pn.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) == Value).ToList().ForEach(a =>
            {
                a.AccessibleName = "true";
                a.ForeColor = mSelectedColor;
                a.BackColor = mSelectedBackColor;
            });



        }
        //Gunjan:29/02/2024
        public void DeSelectButtons(FlowLayoutPanel pn, int Value)
        {
            if (Value == 0)
            {
                pn.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToInt(i.Tag) != Value).ToList().ForEach(a =>
                {
                    a.AccessibleName = "false";
                    a.ForeColor = mDeSelectColor;
                    a.BackColor = mDSelectedBackColor;
                });
            }
        }
        //End As Gunjan
        private void SetControl()
        {
            DTabParameter = ObjRap.GetAllParameterTable();

            DesignSystemButtion(PanelShape, "SHAPE", "SHORTNAME", "SHAPE", 30, 45);
            DesignSystemButtion(PanelColor, "COLOR", "SHORTNAME", "COLOR", 30, 45);
            DesignSystemButtion(PanelClarity, "CLARITY", "SHORTNAME", "CLARITY", 30, 45);
            DesignSystemButtion(PanelCut, "CUT", "SHORTNAME", "CUT", 30, 45);
            DesignSystemButtion(PanelPol, "POLISH", "SHORTNAME", "POL", 30, 45);
            DesignSystemButtion(PanelSym, "SYMMETRY", "SHORTNAME", "SYM", 30, 45);
            DesignSystemButtion(PanelFL, "FLUORESCENCE", "SHORTNAME", "FL", 30, 40);
            DesignSystemButtion(PanelTableInc, "TABLEINC", "SHORTNAME", "TableInc", 30, 45);
            DesignSystemButtion(PanelMilky, "MILKY", "SHORTNAME", "MILKY", 30, 45);
            DesignSystemButtion(PanelLuster, "LUSTER", "SHORTNAME", "LUSTER", 30, 45);
            DesignSystemButtion(PanelLab, "LAB", "SHORTNAME", "Lab", 30, 45);
            DesignSystemButtion(PanelTableOpen, "TABLEOPENINC", "SHORTNAME", "TABLEOPENINC", 30, 45);
            DesignSystemButtion(PanelBlackInc, "BLACKINC", "SHORTNAME", "BLACKINC", 30, 45);
            DesignSystemButtion(PanelColorShade, "COLORSHADE", "SHORTNAME", "COLORSHADE", 30, 45);
            DesignSystemButtion(PanelCrownOpen, "CROWN_OPEN", "SHORTNAME", "CROWN_OPEN", 30, 45);
            DesignSystemButtion(PanelPavOpen, "PAVILION_OPEN", "SHORTNAME", "PAVILION_OPEN", 30, 45);

            cmbDiamondType.SelectedIndex = 1;


        }

        //cpmment and added by Gunjan:29/02/2024
        //public string GetSelectedBtnID(Panel pnlPanel)
        //{
        //    AxonContLib.cButton rb = pnlPanel.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToString(i.AccessibleName).ToLower() == "true").ToList().FirstOrDefault();
        //    return Val.ToString(rb.Tag);

        //}
        public string GetSelectedBtnID(Panel StrPanel)
        {
            AxonContLib.cButton rb = StrPanel.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToString(i.AccessibleName).ToLower() == "true").ToList().FirstOrDefault();

            if (rb != null)
            {
                return Val.ToString(rb.Tag);
            }
            else
            {
                return "";
            }
        }

        public string GetSelectedBtnName(Panel StrPanel)
        {
            AxonContLib.cButton rb = StrPanel.Controls.OfType<AxonContLib.cButton>().Where(i => Val.ToString(i.AccessibleName).ToLower() == "true").ToList().FirstOrDefault();

            if (rb != null)
            {
                return Val.ToString(rb.Text);
            }
            else
            {
                return "";
            }
        }
        //End As Gunjan
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            LiveStockProperty Property = new LiveStockProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {

                    Property.STOCKNO = Val.ToString(txtStoneNo.Text);
                    Property = ObjStock.Delete(Property);
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);
                    }
                    else
                    {
                        txtStoneNo.Focus();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }

        #endregion

        #region Save

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LiveStockProperty Property = new LiveStockProperty();
                this.Cursor = Cursors.WaitCursor;

                if (Val.ToInt32(GetSelectedBtnID(PanelShape)) == 0)
                {
                    Global.Message("Shape is Required");
                    return;
                }

                if (Val.Val(txtCarat.Text) == 0)
                {
                    Global.Message("Carat is Required");
                    txtCarat.Focus();
                    return;
                }

                if (Val.ToString(cmbDiamondType.SelectedItem) == "")
                {
                    Global.Message("Diamond Type is Required");
                    cmbDiamondType.Focus();
                    return;
                }

                Property.STOCK_ID = Val.ToString(txtStoneNo.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtStoneNo.Tag));
                Property.STOCKNO = Val.ToString(txtStoneNo.Text);
                Property.CARAT = Val.Val(txtCarat.Text);
                //Property.PAVANGLE = Val.Val(txtPavAngle.Text);
                Property.MEASUREMENT = Val.ToString(txtMeasurement.Text);

                Property.SHAPE_ID = Val.ToInt32(GetSelectedBtnID(PanelShape));
                Property.COLOR_ID = Val.ToInt32(GetSelectedBtnID(PanelColor));
                Property.CUT_ID = Val.ToInt32(GetSelectedBtnID(PanelCut));
                Property.POL_ID = Val.ToInt32(GetSelectedBtnID(PanelPol));
                Property.SYM_ID = Val.ToInt32(GetSelectedBtnID(PanelSym));
                Property.FL = Val.ToInt32(GetSelectedBtnID(PanelFL));
                Property.LAB_ID = Val.ToInt32(GetSelectedBtnID(PanelLab));
                Property.BLACKINC_ID = Val.ToInt32(GetSelectedBtnID(PanelBlackInc));
                Property.TABLEINC_ID = Val.ToInt32(GetSelectedBtnID(PanelTableInc));
                Property.MILKY_ID = Val.ToInt32(GetSelectedBtnID(PanelMilky));
                Property.LUSTER_ID = Val.ToInt32(GetSelectedBtnID(PanelLuster));
                Property.TABLEOPEN_ID = Val.ToInt32(GetSelectedBtnID(PanelTableOpen));
                Property.CLARITY_ID = Val.ToInt32(GetSelectedBtnID(PanelClarity));//Gunjan:29/02/2024
                Property.COLORSHADE_ID = Val.ToInt32(GetSelectedBtnID(PanelColorShade));
                Property.CROWNOPEN_ID = Val.ToInt32(GetSelectedBtnID(PanelCrownOpen));
                Property.PAVOPEN_ID = Val.ToInt32(GetSelectedBtnID(PanelPavOpen));

                Property.COSTRAPAPORT = Val.Val(txtRapaport.Text);
                Property.COSTDISCOUNT = Val.Val(txtDisc.Text);
                Property.COSTPRICEPERCARAT = Val.Val(txtPricePerCarat.Text);
                Property.COSTAMOUNT = Val.Val(txtAmount.Text);
                Property.PRDTYPE_ID = Val.ToInt32(CmbPrdType.SelectedValue);

                Property.EMPLOYEE_ID = (Val.ToString(txtEmpName.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtEmpName.Tag))).ToString();
                //Property.EMPLOYEECODE = Val.ToString(txtEmpCode.Text);
                Property.DIAMONDTYPE = Val.ToString(cmbDiamondType.SelectedItem);

                Property = ObjStock.SingleStoneCreateAndUpdate(Property);
                this.Cursor = Cursors.Default;
                string StrReturnDesc = Property.ReturnMessageDesc;
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(Property.ReturnMessageDesc);

                    this.Cursor = Cursors.Default;
                    //BtnClear_Click(null,null);
                    DataRow Ds = DTabStcokGrid.NewRow();
                   
                    Ds["SRNO"] = Val.ToInt32(DTabStcokGrid.Rows.Count)+1;
                    Ds["STOCK_ID"] = Property.STOCK_ID;
                    Ds["PRDTYPE"] = Val.ToString(CmbPrdType.Text);
                    Ds["EMPLOYEE"] = Val.ToString(txtEmpName.Text);
                    Ds["PARTYSTOCKNO"] = Val.ToString(txtStoneNo.Text);
                    Ds["CARAT"] = Val.ToDouble(txtCarat.Text);
                    Ds["DIAMONDTYPE"] = Val.ToString(cmbDiamondType.SelectedItem);
                    Ds["RAPPOPRT"] = Val.ToDouble(txtRapaport.Text);
                    Ds["DISCOUNT"] = Val.ToDouble(txtDisc.Text);
                    Ds["PRICEPERCTS"] = Val.ToDouble(txtPricePerCarat.Text);
                    Ds["AMOUNT"] = Val.ToDouble(txtAmount.Text);
                    Ds["SHAPE"] = Val.ToString(GetSelectedBtnName(PanelShape));
                    Ds["COLOR"] = Val.ToString(GetSelectedBtnName(PanelColor));
                    Ds["CLARITY"] = Val.ToString(GetSelectedBtnName(PanelClarity));
                    Ds["CUT"] = Val.ToString(GetSelectedBtnName(PanelCut));
                    Ds["POL"] = Val.ToString(GetSelectedBtnName(PanelPol));
                    Ds["SYM"] = Val.ToString(GetSelectedBtnName(PanelSym));
                    Ds["FLR"] = Val.ToString(GetSelectedBtnName(PanelFL));
                    Ds["BLACKINC"] = Val.ToString(GetSelectedBtnName(PanelBlackInc));
                    Ds["TABLEINC"] = Val.ToString(GetSelectedBtnName(PanelTableInc));
                    Ds["MILKY"] = Val.ToString(GetSelectedBtnName(PanelMilky));
                    Ds["LUSTER"] = Val.ToString(GetSelectedBtnName(PanelLuster));
                    Ds["TABLEOPENINC"] = Val.ToString(GetSelectedBtnName(PanelTableOpen));
                    Ds["COLORSHADE"] = Val.ToString(GetSelectedBtnName(PanelColorShade));
                    Ds["CROWNOPEN"] = Val.ToString(GetSelectedBtnName(PanelCrownOpen));
                    Ds["PAVOPEN"] = Val.ToString(GetSelectedBtnName(PanelPavOpen));

                    DTabStcokGrid.Rows.Add(Ds);
                    MainGrdDetail.DataSource = DTabStcokGrid;
                    GrdDetail.RefreshData();
                    Clear();
                    txtStoneNo.Focus();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);
                    txtStoneNo.Focus();
                }

                Property = null;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }

        #endregion

        #region Other Operation



        private void txtEmpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                txtEmpName.Text = string.Empty;
                txtEmpName.Tag = string.Empty;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "LOCATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtEmpName.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
                        txtEmpName.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        if (txtEmpName.Text == "COSTING")
                        {
                            BtnGIAControlMap.Enabled = true;
                        }
                        else
                        {
                            BtnGIAControlMap.Enabled = false;
                        }


                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }


        public void Clear()
        {
            txtStoneNo.Text = string.Empty;
            txtCarat.Text = string.Empty;

            DeSelectButtons(PanelShape, 0);
            DeSelectButtons(PanelColor, 0);
            DeSelectButtons(PanelCut, 0);
            DeSelectButtons(PanelPol, 0);
            DeSelectButtons(PanelSym, 0);
            DeSelectButtons(PanelFL, 0);
            DeSelectButtons(PanelLab, 0);
            DeSelectButtons(PanelBlackInc, 0);
            DeSelectButtons(PanelTableInc, 0);
            DeSelectButtons(PanelMilky, 0);
            DeSelectButtons(PanelLuster, 0);
            DeSelectButtons(PanelTableOpen, 0);
            DeSelectButtons(PanelClarity, 0);
            DeSelectButtons(PanelColorShade, 0);
            DeSelectButtons(PanelCrownOpen, 0);
            DeSelectButtons(PanelPavOpen, 0);

            txtRapaport.Text = string.Empty;
            txtDisc.Text = string.Empty;
            txtPricePerCarat.Text = string.Empty;
            txtAmount.Text = string.Empty;

            //txtEmpName.Tag = string.Empty;
            //txtEmpName.Text = string.Empty;

            cmbDiamondType.SelectedIndex = 0;
        }
        #endregion

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                DTabStcokGrid.Rows.Clear();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                DTabStcok = ObjStock.GetStockDetail(Val.ToInt32(CmbPrdType.SelectedValue), Val.ToString(txtStoneNo.Text), Val.ToGuid(txtEmpName.Tag));
                if (DTabStcok.Rows.Count > 0)

                {
                  
                    txtCarat.Text = Val.ToString(DTabStcok.Rows[0]["CARAT"]);
                    cmbDiamondType.Text = Val.ToString(DTabStcok.Rows[0]["DIAMONDTYPE"]);

                    txtMeasurement.Text = Val.ToString(DTabStcok.Rows[0]["MEASUREMENT"]);

                    txtRapaport.Text = Val.ToString(DTabStcok.Rows[0]["RAPAPORT"]);
                    txtDisc.Text = Val.ToString(DTabStcok.Rows[0]["DISCOUNT"]);
                    txtPricePerCarat.Text = Val.ToString(DTabStcok.Rows[0]["PRICEPERCARAT"]);
                    txtAmount.Text = Val.ToString(DTabStcok.Rows[0]["AMOUNT"]);

                    Fetch_SetRadioButton(PanelShape, Val.ToInt(DTabStcok.Rows[0]["SHAPE_ID"]), true);
                    Fetch_SetRadioButton(PanelColor, Val.ToInt(DTabStcok.Rows[0]["COLOR_ID"]), true);
                    Fetch_SetRadioButton(PanelClarity, Val.ToInt(DTabStcok.Rows[0]["CLARITY_ID"]), true);
                    Fetch_SetRadioButton(PanelCut, Val.ToInt(DTabStcok.Rows[0]["CUT_ID"]), true);
                    Fetch_SetRadioButton(PanelPol, Val.ToInt(DTabStcok.Rows[0]["POL_ID"]), true);
                    Fetch_SetRadioButton(PanelSym, Val.ToInt(DTabStcok.Rows[0]["SYM_ID"]), true);
                    Fetch_SetRadioButton(PanelFL, Val.ToInt(DTabStcok.Rows[0]["FL_ID"]), true);
                    Fetch_SetRadioButton(PanelLab, Val.ToInt(DTabStcok.Rows[0]["LAB_ID"]), true);
                    Fetch_SetRadioButton(PanelBlackInc, Val.ToInt(DTabStcok.Rows[0]["TABLEBLACKINC_ID"]), true);
                    Fetch_SetRadioButton(PanelTableInc, Val.ToInt(DTabStcok.Rows[0]["TABLEINC_ID"]), true);
                    Fetch_SetRadioButton(PanelTableOpen, Val.ToInt(DTabStcok.Rows[0]["TABLEOPENINC_ID"]), true);
                    Fetch_SetRadioButton(PanelMilky, Val.ToInt(DTabStcok.Rows[0]["MILKY_ID"]), true);
                    Fetch_SetRadioButton(PanelLuster, Val.ToInt(DTabStcok.Rows[0]["LUSTER_ID"]), true);
                    Fetch_SetRadioButton(PanelColorShade, Val.ToInt(DTabStcok.Rows[0]["COLORSHADE_ID"]), true);
                    Fetch_SetRadioButton(PanelCrownOpen, Val.ToInt(DTabStcok.Rows[0]["CROWNOPEN_ID"]), true);
                    Fetch_SetRadioButton(PanelPavOpen, Val.ToInt(DTabStcok.Rows[0]["PAVILIONOPEN_ID"]), true);

                    //Comemnt By Gunjan:28/03/2024
                    //MainGrdDetail.DataSource = DTabStcok;
                    //GrdDetail.RefreshData();
                    //End As Gunjan

                }
                else
                {
                    DeSelectButtons(PanelShape, 0);
                    DeSelectButtons(PanelColor, 0);
                    DeSelectButtons(PanelCut, 0);
                    DeSelectButtons(PanelPol, 0);
                    DeSelectButtons(PanelSym, 0);
                    DeSelectButtons(PanelFL, 0);
                    DeSelectButtons(PanelLab, 0);
                    DeSelectButtons(PanelBlackInc, 0);
                    DeSelectButtons(PanelTableInc, 0);
                    DeSelectButtons(PanelMilky, 0);
                    DeSelectButtons(PanelLuster, 0);
                    DeSelectButtons(PanelTableOpen, 0);
                    DeSelectButtons(PanelClarity, 0);

                    txtRapaport.Text = string.Empty;
                    txtDisc.Text = string.Empty;
                    txtPricePerCarat.Text = string.Empty;
                    txtAmount.Text = string.Empty;

                    txtCarat.Text = string.Empty;
                    cmbDiamondType.SelectedIndex = 0;


                    SetControl();


                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void CmbPrdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToInt32(CmbPrdType.SelectedValue) == 2)
                {
                    txtEmpName.Enabled = true;
                }
                else
                {
                    txtEmpName.Enabled = false;
                }
                Clear();
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnGIAControlMap_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DtInvDetail == null)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string strStoneNo = "";

                DtInvDetail.DefaultView.Sort = "SRNO";
                if (DtInvDetail.Rows.Count > 0)
                {
                    var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                    strStoneNo = string.Join(",", list);
                }

               
                FrmGIAControlNoMap FrmGIAControlNoMap = new FrmGIAControlNoMap();
                FrmGIAControlNoMap.MdiParent = Global.gMainRef;
                FrmGIAControlNoMap.ShowForm(strStoneNo);

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        public void FindRap()
        {
            Trn_RapSaveProperty Property = new Trn_RapSaveProperty();
            Property.STOCKNO = Val.ToString(txtStoneNo.Text);
            Property.SHAPE_ID = Val.ToInt32(GetSelectedBtnID(PanelShape));
            Property.COLOR_ID = Val.ToInt32(GetSelectedBtnID(PanelColor));
            Property.CUT_ID = Val.ToInt32(GetSelectedBtnID(PanelCut));
            Property.POL_ID = Val.ToInt32(GetSelectedBtnID(PanelPol));
            Property.SYM_ID = Val.ToInt32(GetSelectedBtnID(PanelSym));
            Property.FL_ID = Val.ToInt32(GetSelectedBtnID(PanelFL));
            Property.LAB_ID = Val.ToInt32(GetSelectedBtnID(PanelLab));
            Property.SIDEBLACKINC_ID = Val.ToInt32(GetSelectedBtnID(PanelBlackInc));
            Property.TABLEINC_ID = Val.ToString(GetSelectedBtnID(PanelTableInc));
            Property.MILKY_ID = Val.ToInt32(GetSelectedBtnID(PanelMilky));
            Property.LUSTER_ID = Val.ToInt32(GetSelectedBtnID(PanelLuster));
            Property.TABLEINC_ID = Val.ToString(GetSelectedBtnID(PanelTableOpen));
            Property.CLARITY_ID = Val.ToInt32(GetSelectedBtnID(PanelClarity));
            Property.CARAT = Val.Val(txtCarat.Text);
            Property = ObjRap.FindRap(Property);
            txtRapaport.Text = Val.ToString(Property.RAPAPORT);
            txtPricePerCarat.Text = Math.Round(Val.Val(txtRapaport.Text) + (Val.Val(txtRapaport.Text) * Val.Val(txtDisc.Text) / 100)).ToString();
            txtAmount.Text = Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtPricePerCarat.Text), 2).ToString();
        }

        private void txtDisc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    FindRap();
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtPricePerCarat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                    Property.RAPAPORT = Val.Val(txtRapaport.Text);
                    Property.MGPRICEPERCARAT = Val.Val(txtPricePerCarat.Text);
                    Property.CARAT = Val.Val(txtCarat.Text);
                    if (Property.RAPAPORT != 0)
                    {
                        txtDisc.Text = Math.Round(((Property.MGPRICEPERCARAT - Property.RAPAPORT  ) / Property.RAPAPORT) * 100, 2).ToString(); //#P:23-04-2021
                    }
                    else
                        txtDisc.Text = string.Empty;

                    txtAmount.Text = Math.Round(Property.CARAT * Val.Val(Property.MGPRICEPERCARAT), 2).ToString();
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtEmpName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}