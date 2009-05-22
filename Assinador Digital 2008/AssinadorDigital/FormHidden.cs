using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AssinadorDigital
{
    public partial class FormHidden : Form
    {
        public FormHidden()
        {
            InitializeComponent();
        }

        public FormHidden(string[] args)
        {
            InitializeComponent();
            ConsultCRL = Registry.CurrentUser.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
            consultarLCRToolStripMenuItem.Checked = Convert.ToBoolean(ConsultCRL.GetValue("ConsultCRL"));
            this.Hide();
            if (args.Length > 0)
            {
                string[] paths = args[1].Split('|');
                if (args[0] == "/v")
                {
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                    {
                        frmManageDigitalSignature frmManage = new frmManageDigitalSignature(paths, false);
                        frmManage.ShowDialog();
                    }
                    else
                    {
                        frmIncludeSubFolders frmInclude = new frmIncludeSubFolders(paths, args[0]);
                        frmInclude.ShowDialog();
                    }
                }
                else if (args[0] == "/r")
                {
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                    {
                        frmSelectDigitalSignatureToRemove frmSelect = new frmSelectDigitalSignatureToRemove(paths, false);
                        frmSelect.ShowDialog();
                    }
                    else
                    {
                        frmIncludeSubFolders frmInclude = new frmIncludeSubFolders(paths, args[0]);
                        frmInclude.ShowDialog();
                    }
                }
                else if (args[0] == "/a")
                {
                    frmAddDigitalSignature frmAdd = new frmAddDigitalSignature(paths, true);
                    frmAdd.ShowDialog();
                }
            }
        }

        private RegistryKey ConsultCRL = null;

        private void consultarLCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConsultCRL != null)
            {
                ConsultCRL.SetValue("ConsultCRL", consultarLCRToolStripMenuItem.Checked);
            }
        }

        private void FormHidden_Shown(object sender, EventArgs e)
        {
            if (!consultarLCRToolStripMenuItem.Checked)
                MessageBox.Show("A Verificação de Certificados Revogados está desabilitada!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sobreOAssinadorDigitalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

    }
}
