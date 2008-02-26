using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FileUtils;

namespace AssinadorDigital
{
    public partial class frmReport : Form
    {
        List<FileStatus> documentsStatus = new List<FileStatus>();
        string actionPerformed;

        public frmReport(List<FileStatus> report, string action)
        {
            documentsStatus = report;
            actionPerformed = action;
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            switch (actionPerformed)
            {
                case "sign":
                    foreach (FileStatus fileStatus in documentsStatus)
                    {
                        ListViewItem lviDocumentStatus = new ListViewItem(fileStatus.Path);
                        switch (fileStatus.Status)
                        {
                            case Status.Success:
                                lviDocumentStatus.SubItems.Add("Backup efetuado. Documento original assinado com sucesso.");
                                break;
                            case Status.Unmodified:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, arquivo já existente. Documento original assinado com sucesso.");
                                break;
                            case Status.ModifiedButNotBackedUp:
                                lviDocumentStatus.SubItems.Add("Backup não efetuado do documento. Documento original assinado com sucesso.");
                                break;
                            case Status.UnauthorizedAccess:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, acesso não autorizado à pasta de destino. Documento original não foi assinado.");
                                break;
                            case Status.PathTooLong:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, caminho do diretório muito longo. Documento original não foi assinado.");
                                break;
                            case Status.InUseByAnotherProcess:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, verifique se ele não está em uso por outra aplicação. Documento original não foi assinado.");
                                break;
                            case Status.CorruptedContent:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, o conteúdo do documento está corrompido (talvez seja um arquivo temporário). Documento original não foi assinado.");
                                break;
                            default:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento. Documento original não foi assinado.");
                                break;
                        }
                        lstReport.Items.Add(lviDocumentStatus);
                    }
                    break;
                case "remove":
                    foreach (FileStatus fileStatus in documentsStatus)
                    {
                        ListViewItem lviDocumentStatus = new ListViewItem(fileStatus.Path);
                        switch (fileStatus.Status)
                        {
                            case Status.Success:
                                lviDocumentStatus.SubItems.Add("Backup efetuado. Assinatura removida do documento original com sucesso.");
                                break;
                            case Status.Unmodified:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, arquivo já existente. Assinatura removida do documento original com sucesso.");
                                break;
                            case Status.ModifiedButNotBackedUp:
                                lviDocumentStatus.SubItems.Add("Backup não efetuado do documento. Assinatura removida do documento original com sucesso.");
                                break;
                            case Status.UnauthorizedAccess:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, acesso não autorizado à pasta de destino. A assinatura não foi removida do documento original.");
                                break;
                            case Status.PathTooLong:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, caminho do diretório muito longo. A assinatura não foi removida do documento original.");
                                break;
                            case Status.InUseByAnotherProcess:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, verifique se ele não está em uso por outra aplicação. Documento original não foi assinado.");
                                break;
                            case Status.CorruptedContent:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento, o conteúdo do documento está corrompido (talvez seja um arquivo temporário). Documento original não foi assinado.");
                                break;
                            default:
                                lviDocumentStatus.SubItems.Add("Não foi possível efetuar o backup do documento. A assinatura não foi removida do documento original.");
                                break;
                        }
                        lstReport.Items.Add(lviDocumentStatus);
                    }
                    break;
            }
        }

        private void frmReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormCollection openedForms = Application.OpenForms;

            int visibleForms = 0;
            foreach (Form form in openedForms)
            {
                if (form.Visible)
                    visibleForms++;
            }
            if (visibleForms < 1)
                Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
