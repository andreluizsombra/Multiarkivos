namespace SisMultiarkivos
{
    partial class frmPrincipal
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
            this.btn_selecione = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbx_arqs = new System.Windows.Forms.ListBox();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btn_selecione
            // 
            this.btn_selecione.Location = new System.Drawing.Point(12, 12);
            this.btn_selecione.Name = "btn_selecione";
            this.btn_selecione.Size = new System.Drawing.Size(75, 23);
            this.btn_selecione.TabIndex = 0;
            this.btn_selecione.Text = "Localizar";
            this.btn_selecione.UseVisualStyleBackColor = true;
            this.btn_selecione.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // lbx_arqs
            // 
            this.lbx_arqs.FormattingEnabled = true;
            this.lbx_arqs.Location = new System.Drawing.Point(12, 65);
            this.lbx_arqs.Name = "lbx_arqs";
            this.lbx_arqs.Size = new System.Drawing.Size(1131, 459);
            this.lbx_arqs.TabIndex = 1;
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(12, 36);
            this.pBar.Maximum = 1000000;
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(1131, 23);
            this.pBar.TabIndex = 2;
            this.pBar.UseWaitCursor = true;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 539);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.lbx_arqs);
            this.Controls.Add(this.btn_selecione);
            this.Name = "frmPrincipal";
            this.Text = "Multiarkivos";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_selecione;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox lbx_arqs;
        private System.Windows.Forms.ProgressBar pBar;
    }
}

