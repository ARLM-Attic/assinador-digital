namespace AssinadorDigital
{
    partial class frmAddDigitalSignature
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
            this.btnSign = new System.Windows.Forms.Button();
            this.chkCopyDocuments = new System.Windows.Forms.CheckBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnChangeFolder = new System.Windows.Forms.Button();
            this.fbdSelectNewPath = new System.Windows.Forms.FolderBrowserDialog();
            this.chkViewDocuments = new System.Windows.Forms.CheckBox();
            this.gpbCopyDoduments = new System.Windows.Forms.GroupBox();
            this.gpbCopyDoduments.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(251, 115);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(75, 23);
            this.btnSign.TabIndex = 0;
            this.btnSign.Text = "Assinar";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // chkCopyDocuments
            // 
            this.chkCopyDocuments.AutoSize = true;
            this.chkCopyDocuments.Location = new System.Drawing.Point(12, 12);
            this.chkCopyDocuments.Name = "chkCopyDocuments";
            this.chkCopyDocuments.Size = new System.Drawing.Size(117, 17);
            this.chkCopyDocuments.TabIndex = 1;
            this.chkCopyDocuments.Text = "Copiar documentos";
            this.chkCopyDocuments.UseVisualStyleBackColor = true;
            this.chkCopyDocuments.CheckedChanged += new System.EventHandler(this.chkCopyDocuments_CheckedChanged);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(6, 19);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(293, 20);
            this.txtPath.TabIndex = 2;
            // 
            // btnChangeFolder
            // 
            this.btnChangeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeFolder.Location = new System.Drawing.Point(224, 45);
            this.btnChangeFolder.Name = "btnChangeFolder";
            this.btnChangeFolder.Size = new System.Drawing.Size(75, 23);
            this.btnChangeFolder.TabIndex = 3;
            this.btnChangeFolder.Text = "Procurar";
            this.btnChangeFolder.UseVisualStyleBackColor = true;
            this.btnChangeFolder.Click += new System.EventHandler(this.btnChangeFolder_Click);
            // 
            // chkViewDocuments
            // 
            this.chkViewDocuments.AutoSize = true;
            this.chkViewDocuments.Checked = true;
            this.chkViewDocuments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkViewDocuments.Location = new System.Drawing.Point(12, 115);
            this.chkViewDocuments.Name = "chkViewDocuments";
            this.chkViewDocuments.Size = new System.Drawing.Size(181, 17);
            this.chkViewDocuments.TabIndex = 4;
            this.chkViewDocuments.Text = "Visualizar documentos assinados";
            this.chkViewDocuments.UseVisualStyleBackColor = true;
            // 
            // gpbCopyDoduments
            // 
            this.gpbCopyDoduments.Controls.Add(this.txtPath);
            this.gpbCopyDoduments.Controls.Add(this.btnChangeFolder);
            this.gpbCopyDoduments.Enabled = false;
            this.gpbCopyDoduments.Location = new System.Drawing.Point(12, 35);
            this.gpbCopyDoduments.Name = "gpbCopyDoduments";
            this.gpbCopyDoduments.Size = new System.Drawing.Size(314, 74);
            this.gpbCopyDoduments.TabIndex = 5;
            this.gpbCopyDoduments.TabStop = false;
            this.gpbCopyDoduments.Text = "Salvar em";
            // 
            // frmAddDigitalSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 150);
            this.Controls.Add(this.gpbCopyDoduments);
            this.Controls.Add(this.chkViewDocuments);
            this.Controls.Add(this.chkCopyDocuments);
            this.Controls.Add(this.btnSign);
            this.Name = "frmAddDigitalSignature";
            this.Text = "FormSelectPath";
            this.Load += new System.EventHandler(this.frmAddDigitalSignature_Load);
            this.gpbCopyDoduments.ResumeLayout(false);
            this.gpbCopyDoduments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.CheckBox chkCopyDocuments;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnChangeFolder;
        private System.Windows.Forms.FolderBrowserDialog fbdSelectNewPath;
        private System.Windows.Forms.CheckBox chkViewDocuments;
        private System.Windows.Forms.GroupBox gpbCopyDoduments;
    }
}