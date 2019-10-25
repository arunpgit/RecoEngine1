using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;

namespace RecoEngine
{
    public partial class CntrlchkDropDown : UserControl
    {
        private RadPanel m_Panel;
        private RadGridView m_Grid;
        public RadTextBoxElement m_TextBox;
        private System.Drawing.Size m_Size = new System.Drawing.Size(200, 200);
        //string strVlaueColName = "";
        //string strDisplayColName = "";
        string strSelColName = "";
        String strSelectedValue = "";
        private RadSplitButton radChkdropDown;
        String strSelectedItems = "";
        bool bIsmultipleSelection = false;
        bool bIscntrlforTimePeriod = true;

        public bool TimePeriodControl
        {
            get 
            {
                return bIscntrlforTimePeriod;
           }
            set 
            {
                bIscntrlforTimePeriod = value;
            }
         }

        public bool MultipleSelection
        {
            get
            {
                return bIsmultipleSelection;
            }
            set
            {
                bIsmultipleSelection = value;
            }
        }

        public CntrlchkDropDown()
        {
            InitializeComponent();
            m_TextBox = new RadTextBoxElement();            
            m_TextBox.TextBoxItem.Alignment = ContentAlignment.MiddleLeft;
            m_TextBox.TextBoxItem.BorderThickness = new System.Windows.Forms.Padding(0, 0, 0, 0);
            m_TextBox.Border.Visibility = ElementVisibility.Collapsed;
            m_TextBox.TextBoxItem.ReadOnly = true;
            m_TextBox.AutoSize = false;
            this.Load += new EventHandler(chkDropDown_Load);
            
        }
       
        void chkDropDown_Load(object sender, EventArgs e)
        {
            m_TextBox.Size = new System.Drawing.Size(this.radChkdropDown.DropDownButtonElement.ActionButton.Size.Width, this.radChkdropDown.DropDownButtonElement.ActionButton.Size.Height + 1);
            m_TextBox.TextBoxItem.Alignment = m_TextBox.TextBoxItem.Parent.Alignment;
            this.radChkdropDown.DropDownButtonElement.ActionButton.Children.Add(m_TextBox);
        }

        private void ctlMultiCheckedDropdown_FontChanged(object sender, EventArgs e)
        {
            try
            {
                radChkdropDown.Font = this.Font;
                ctlMultiCheckedDropdown_Resize(null, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void ctlMultiCheckedDropdown_ForeColorChanged(object sender, EventArgs e)
        {
            radChkdropDown.ForeColor = this.ForeColor;
        }

        private void ctlMultiCheckedDropdown_Resize(object sender, EventArgs e)
        {
            this.Height = radChkdropDown.Height;
        }

        public void ConfigureDropDown(DataSet ds,  string strSelName)
        {
            strSelColName = strSelName;
            strSelectedValue = "";
            m_Grid = new RadGridView();
            m_Grid.AllowAddNewRow = false;
            m_Grid.ShowRowHeaderColumn = false;
            m_Grid.EnableGrouping = false;
            m_Grid.ShowFilteringRow = false;
            m_Grid.AutoSizeRows = false;
            m_Grid.DataBindingComplete += new GridViewBindingCompleteEventHandler(m_Grid_DataBindingComplete);
            m_Grid.ShowColumnHeaders = true;
            m_Grid.DataSource = ds.Tables[0];
            m_Panel = new RadPanel();
            m_Panel.Controls.Add(m_Grid);
            m_Grid.Dock = DockStyle.Fill;
            RadMenuItem item = new RadMenuItem();
            item.MaxSize = m_Size;
            this.radChkdropDown.Items.Add(item);
            m_Grid.ValueChanged+=new EventHandler(m_Grid_ValueChanged);
            this.radChkdropDown.DropDownButtonElement.DropDownMenu.PopupOpened += new RadPopupOpenedEventHandler(DropDownMenu_PopupOpened);
            this.radChkdropDown.DropDownButtonElement.DropDownMenu.PopupClosing += new RadPopupClosingEventHandler(DropDownMenu_PopupClosing);
            this.radChkdropDown.DropDownButtonElement.ShowDropDown();
            this.radChkdropDown.DropDownButtonElement.HideDropDown();
            bConfigured = true;
        }

       

        void m_Grid_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!MultipleSelection)
                {
                    RadCheckBoxEditor editor = sender as RadCheckBoxEditor;
                    if (editor != null && editor.Value.ToString() == "On")
                    {
                        m_Grid.GridElement.BeginUpdate();
                        foreach (GridViewDataRowInfo row in this.m_Grid.Rows)
                        {
                            if (row != this.m_Grid.CurrentRow)
                            {
                                row.Cells["Selected"].Value = false;
                            }
                        }
                        m_Grid.GridElement.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.Message, ex.TargetSite.Name.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        void DropDownMenu_PopupClosing(object sender, RadPopupClosingEventArgs args)
        {
            try
            {
                m_Grid.EndEdit();
                SetText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void m_Grid_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            m_Grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            m_Grid.MasterTemplate.Columns[3].IsVisible = false;
            m_Grid.MasterTemplate.Columns[2].IsVisible = false;
            m_Grid.MasterTemplate.Columns[1].ReadOnly = true;
            m_Grid.MasterTemplate.Columns[0].BestFit();
            //m_Grid.MasterTemplate.Columns[1].Width = 5;
            SetText();
        }

        void DropDownMenu_PopupOpened(object sender, System.EventArgs args)
        {
            this.radChkdropDown.DropDownButtonElement.DropDownMenu.PopupElement.MaxSize = m_Size;
            this.radChkdropDown.Items.Clear();
            RadMenuItem item = new RadMenuItem();
            m_Panel.Size = m_Size;
            item.AutoSize = false;
            item.BackColor = System.Drawing.Color.Transparent;
            item.Size = m_Panel.Size;
            item.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            item.Children.Add(new RadHostItem(m_Panel));
            this.radChkdropDown.Items.Add(item);
        }

        private void SetText()
        {
            try
            {
                int count = 0;
                strSelectedItems = "";
                for (int i = 0; i < m_Grid.ChildRows.Count; i++)
                {
                    GridViewRowInfo row = m_Grid.ChildRows[i];
                    if (row is GridViewDataRowInfo)
                    {
                        if (((bool)row.Cells[strSelColName].Value) == true)
                        {
                            if (strSelectedItems != "")
                                strSelectedItems += ";";
                            if (TimePeriodControl)
                            {
                                strSelectedValue += m_Grid.Rows[i].Cells["timeperiod"].Value;
                                strSelectedItems += m_Grid.Rows[i].Cells["timeperiod"].Value;
                                count++;
                            }
                            else
                            {
                                strSelectedValue += m_Grid.Rows[i].Cells["status"].Value;
                                strSelectedItems += m_Grid.Rows[i].Cells["status"].Value;
                                count++;
                            
                            }
                        }
                    }

                }
                this.m_TextBox.Text = strSelectedItems;
                this.radChkdropDown.Tag = strSelectedValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        private void ctlMultiCheckedDropdown_Load(object sender, EventArgs e)
        {
            m_TextBox.AutoSize = false;
            m_TextBox.Size = new System.Drawing.Size(this.radChkdropDown.DropDownButtonElement.ActionButton.Size.Width, this.radChkdropDown.DropDownButtonElement.ActionButton.Size.Height + 1);
            this.radChkdropDown.DropDownButtonElement.ActionButton.Children.Add(m_TextBox);
        }

        private bool bConfigured = false;
        public string GetValue()
        {
            if (!bConfigured)
                return "";

            if (radChkdropDown.Tag == null || radChkdropDown.Tag.ToString() == "")
                return "0";
            else
                return radChkdropDown.Tag.ToString();
            //aa

        }


        public string GetText()
        {
            if (!bConfigured)
                return "";

            if (m_TextBox.Text == null || m_TextBox.Text.ToString() == "")
                return "";
            else
                return m_TextBox.Text;
        }

        private void ctlMultiCheckedDropdown_Leave(object sender, EventArgs e)
        {
            SetText();
        }

        private void InitializeComponent()
        {
            this.radChkdropDown = new Telerik.WinControls.UI.RadSplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.radChkdropDown)).BeginInit();
            this.SuspendLayout();
            // 
            // radChkdropDown
            // 
            this.radChkdropDown.DefaultItem = null;
            this.radChkdropDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChkdropDown.Location = new System.Drawing.Point(0, 0);
            this.radChkdropDown.Name = "radChkdropDown";
            this.radChkdropDown.Size = new System.Drawing.Size(207, 30);
            this.radChkdropDown.TabIndex = 3;
            // 
            // CntrlchkDropDown
            // 
            this.Controls.Add(this.radChkdropDown);
            this.Name = "CntrlchkDropDown";
            this.Size = new System.Drawing.Size(207, 30);
            ((System.ComponentModel.ISupportInitialize)(this.radChkdropDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}

