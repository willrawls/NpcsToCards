namespace NpcsToCards
{
    partial class Form1
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
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.document = new NpcsToCards.NpcPrinter();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.AutoZoom = false;
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Document = this.document;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 0);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(743, 722);
            this.printPreviewControl1.StartPage = 1;
            this.printPreviewControl1.TabIndex = 2;
            this.printPreviewControl1.UseAntiAlias = true;
            this.printPreviewControl1.Zoom = 1D;
            this.printPreviewControl1.DoubleClick += new System.EventHandler(this.printPreviewControl1_DoubleClick);
            // 
            // pageSetupDialog
            // 
            this.pageSetupDialog.AllowMargins = false;
            this.pageSetupDialog.AllowOrientation = false;
            this.pageSetupDialog.AllowPaper = false;
            this.pageSetupDialog.Document = this.document;
            // 
            // printDialog
            // 
            this.printDialog.Document = this.document;
            this.printDialog.UseEXDialog = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 722);
            this.Controls.Add(this.printPreviewControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private NpcPrinter document;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
    }
}

