namespace RecoEngine
{
    partial class CntrlPreviewExpression
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grdPreviewExpression = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreviewExpression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreviewExpression.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPreviewExpression
            // 
            this.grdPreviewExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPreviewExpression.Location = new System.Drawing.Point(0, 0);
            // 
            // grdPreviewExpression
            // 
            this.grdPreviewExpression.MasterTemplate.AllowAddNewRow = false;
            this.grdPreviewExpression.Name = "grdPreviewExpression";
            this.grdPreviewExpression.Size = new System.Drawing.Size(781, 458);
            this.grdPreviewExpression.TabIndex = 0;
            this.grdPreviewExpression.Click += new System.EventHandler(this.grdPreviewExpression_Click);
            // 
            // CntrlPreviewExpression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 458);
            this.Controls.Add(this.grdPreviewExpression);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CntrlPreviewExpression";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CntrlPreviewExpression";
            ((System.ComponentModel.ISupportInitialize)(this.grdPreviewExpression.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreviewExpression)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView grdPreviewExpression;
    }
}