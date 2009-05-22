namespace AssinadorDigital
{
    partial class FormHidden
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHidden));
            this.nicAssinador = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configuraçõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarLCRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreOAssinadorDigitalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // nicAssinador
            // 
            this.nicAssinador.ContextMenuStrip = this.ctxTray;
            this.nicAssinador.Icon = ((System.Drawing.Icon)(resources.GetObject("nicAssinador.Icon")));
            this.nicAssinador.Text = "Assinador Digital";
            this.nicAssinador.Visible = true;
            // 
            // ctxTray
            // 
            this.ctxTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraçõesToolStripMenuItem,
            this.sobreOAssinadorDigitalToolStripMenuItem1,
            this.toolStripMenuItem1,
            this.sairToolStripMenuItem});
            this.ctxTray.Name = "ctxTray";
            this.ctxTray.Size = new System.Drawing.Size(205, 98);
            // 
            // configuraçõesToolStripMenuItem
            // 
            this.configuraçõesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultarLCRToolStripMenuItem});
            this.configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            this.configuraçõesToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.configuraçõesToolStripMenuItem.Text = "Configurações";
            // 
            // consultarLCRToolStripMenuItem
            // 
            this.consultarLCRToolStripMenuItem.CheckOnClick = true;
            this.consultarLCRToolStripMenuItem.Name = "consultarLCRToolStripMenuItem";
            this.consultarLCRToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.consultarLCRToolStripMenuItem.Text = "Consultar LCR";
            this.consultarLCRToolStripMenuItem.Click += new System.EventHandler(this.consultarLCRToolStripMenuItem_Click);
            // 
            // sobreOAssinadorDigitalToolStripMenuItem1
            // 
            this.sobreOAssinadorDigitalToolStripMenuItem1.Name = "sobreOAssinadorDigitalToolStripMenuItem1";
            this.sobreOAssinadorDigitalToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.sobreOAssinadorDigitalToolStripMenuItem1.Text = "Sobre o Assinador Digital";
            this.sobreOAssinadorDigitalToolStripMenuItem1.Click += new System.EventHandler(this.sobreOAssinadorDigitalToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(201, 6);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.sairToolStripMenuItem.Text = "Fechar";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // FormHidden
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "FormHidden";
            this.Text = "FormHidden";
            this.Shown += new System.EventHandler(this.FormHidden_Shown);
            this.ctxTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon nicAssinador;
        private System.Windows.Forms.ContextMenuStrip ctxTray;
        private System.Windows.Forms.ToolStripMenuItem configuraçõesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultarLCRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreOAssinadorDigitalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
    }
}