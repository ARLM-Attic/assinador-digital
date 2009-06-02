using System;
using System.Windows.Forms;

namespace AssinadorDigital
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Diagnostics.Debug.Assert(true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string[] paths = args[1].Split('|');
                if (args[0] == "/v")
                {
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                    {
                        frmManageDigitalSignature frmManage = new frmManageDigitalSignature(paths, false);
                      //  notifyInconInstance(frmManage.components);
                        Application.Run(frmManage);
                    }
                    else
                    {
                        frmIncludeSubFolders frmInclude = new frmIncludeSubFolders(paths, args[0]);
                        notifyInconInstance(frmInclude.components);
                        Application.Run(frmInclude);
                    }
                }
                else if (args[0] == "/r")
                {
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                    {
                        frmSelectDigitalSignatureToRemove frmSelect = new frmSelectDigitalSignatureToRemove(paths, false);
                        notifyInconInstance(frmSelect.components);
                        Application.Run(frmSelect);
                    }
                    else
                    {
                        frmIncludeSubFolders frmInclude = new frmIncludeSubFolders(paths, args[0]);
                        notifyInconInstance(frmInclude.components);
                        Application.Run(frmInclude);
                    }
                }
                else if (args[0] == "/a")
                {
                    frmAddDigitalSignature frmAdd = new frmAddDigitalSignature(paths, true);
                    notifyInconInstance(frmAdd.components);
                    Application.Run(frmAdd);
                }
            }
        }

        #region NotifyIcon

        private static Microsoft.Win32.RegistryKey ConsultCRL = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
        private static System.Windows.Forms.ContextMenuStrip ctxTray;
        private static System.Windows.Forms.ToolStripMenuItem configuraçõesToolStripMenuItem;
        private static System.Windows.Forms.ToolStripMenuItem consultarLCRToolStripMenuItem;
        private static System.Windows.Forms.ToolStripMenuItem sobreOAssinadorDigitalToolStripMenuItem;
        private static System.Windows.Forms.ToolStripSeparator toolStripMenuItem;
        private static System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;


        static void notifyInconInstance(System.ComponentModel.IContainer components)
        {
            ctxTray = new System.Windows.Forms.ContextMenuStrip(components);
            configuraçõesToolStripMenuItem = new ToolStripMenuItem();
            consultarLCRToolStripMenuItem = new ToolStripMenuItem();
            sobreOAssinadorDigitalToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem = new ToolStripSeparator();
            sairToolStripMenuItem = new ToolStripMenuItem();
            // 
            // ctxTray
            // 
            ctxTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            configuraçõesToolStripMenuItem,
            sobreOAssinadorDigitalToolStripMenuItem,
            toolStripMenuItem,
            sairToolStripMenuItem});
            ctxTray.Name = "ctxTray";
            ctxTray.Size = new System.Drawing.Size(205, 76);
            ctxTray.Opening += new System.ComponentModel.CancelEventHandler(ctxTray_Opening);
            // 
            // configuraçõesToolStripMenuItem
            // 
            configuraçõesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            consultarLCRToolStripMenuItem});
            configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            configuraçõesToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            configuraçõesToolStripMenuItem.Text = "Configurações";
            // 
            // consultarLCRToolStripMenuItem
            // 
            consultarLCRToolStripMenuItem.CheckOnClick = true;
            consultarLCRToolStripMenuItem.Name = "consultarLCRToolStripMenuItem";
            consultarLCRToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            consultarLCRToolStripMenuItem.Text = "Consultar LCR";
            consultarLCRToolStripMenuItem.Click += new EventHandler(consultarLCRToolStripMenuItem_Click);
            // 
            // sobreOAssinadorDigitalToolStripMenuItem
            // 
            sobreOAssinadorDigitalToolStripMenuItem.Name = "sobreOAssinadorDigitalToolStripMenuItem1";
            sobreOAssinadorDigitalToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            sobreOAssinadorDigitalToolStripMenuItem.Text = "Sobre o Assinador Digital";
            sobreOAssinadorDigitalToolStripMenuItem.Click += new EventHandler(sobreOAssinadorDigitalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem
            // 
            toolStripMenuItem.Name = "toolStripMenuItem";
            toolStripMenuItem.Size = new System.Drawing.Size(201, 6);
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            sairToolStripMenuItem.Text = "Fechar";
            sairToolStripMenuItem.Click += new EventHandler(sairToolStripMenuItem_Click);

            NotifyIcon niAssinador = new NotifyIcon(components);
            niAssinador.ContextMenuStrip = ctxTray;
            niAssinador.Icon = ((System.Drawing.Icon)(AssinadorDigital.Properties.Resources.icone));
            niAssinador.Visible = true;
        }

        static void ctxTray_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ConsultCRL != null)
            {
                consultarLCRToolStripMenuItem.Checked = Convert.ToBoolean(ConsultCRL.GetValue("ConsultCRL"));
            }
        }


        static void consultarLCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConsultCRL != null)
            {
                ConsultCRL.SetValue("ConsultCRL", ((ToolStripMenuItem)sender).Checked);
            }
        }

        static void sobreOAssinadorDigitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        static void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}