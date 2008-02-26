using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OPC;
using FileUtils;
using System.Security.Cryptography.X509Certificates;

namespace AssinadorDigital
{
    public partial class frmAddDigitalSignature : Form
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorerSelectedItens"></param>
        /// <param name="showCheckBoxViewDocuments"></param>
        public frmAddDigitalSignature(string[] explorerSelectedItens, bool showCheckBoxViewDocuments)
        {
            InitializeComponent();
            documentsToSign = explorerSelectedItens;

            chkViewDocuments.Checked = showCheckBoxViewDocuments;
            chkViewDocuments.Visible = showCheckBoxViewDocuments;
        }

        #endregion

        #region Private Properties

        private DigitalSignature digitalSignature;
        private List<string> compatibleDocuments = new List<string>();
        private string[] documentsToSign;
        private List<FileStatus> documentsSignStatus = new List<FileStatus>();

        #endregion

        #region Private Methods

        private string[] signDocuments()
        {
            String officeDocument = Properties.Resources.OfficeObject;

            X509Certificate2 certificate = GetCertificate();

            if (certificate != null)
            {
                List<string> signedDocuments = new List<string>();
                if (chkCopyDocuments.Checked)
                {
                    documentsSignStatus = FileOperations.Copy(compatibleDocuments, txtPath.Text, chkOverwrite.Checked);
                    foreach (FileStatus documentToSign in documentsSignStatus)
                    {
                        if (documentToSign.Status == Status.Success ||
                            documentToSign.Status == Status.Unmodified)
                        {
                            try
                            {
                                loadDigitalSignature(documentToSign.Path);
                                digitalSignature.SetOfficeDocument(officeDocument);
                                digitalSignature.SignDocument(certificate);
                                signedDocuments.Add(documentToSign.Path);
                            }
                            catch (NullReferenceException)
                            {
                                documentToSign.Status = Status.GenericError;
                            }
                            catch (FileFormatException)
                            {
                                documentToSign.Status = Status.CorruptedContent;
                            }
                            catch (IOException e)
                            {
                                documentToSign.Status = Status.InUseByAnotherProcess;
                            }
                            catch (Exception e)
                            {
                                documentToSign.Status = Status.GenericError;
                            }

                            if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                            {
                                digitalSignature.xpsDocument.Close();
                            }
                            else
                            {
                                digitalSignature.package.Close();
                            }
                        }
                    }
                }
                else
                {
                    foreach (string documentToSign in compatibleDocuments)
                    {
                        FileStatus documentSignStatus = null;
                        try
                        {
                            loadDigitalSignature(documentToSign);
                            digitalSignature.SetOfficeDocument(officeDocument);
                            digitalSignature.SignDocument(certificate);
                            signedDocuments.Add(documentToSign);

                            documentSignStatus = new FileStatus(documentToSign, FileUtils.Status.ModifiedButNotBackedUp);
                        }
                        catch
                        {
                            documentSignStatus = new FileStatus(documentToSign, FileUtils.Status.NotSigned);
                        }
                        documentsSignStatus.Add(documentSignStatus);
                        if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                        {
                            digitalSignature.xpsDocument.Close();
                        }
                        else
                        {
                            digitalSignature.package.Close();
                        }
                    }
                }
                return signedDocuments.ToArray();
            }
            return null;
        }

        private X509Certificate2 GetCertificate()
        {
            X509Store certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs =
                X509Certificate2UI.SelectFromCollection(
                    certStore.Certificates,
                    "Selecionar um Certificado Digital.",
                    "Por favor selecione um certificado para assinatura.",
                    X509SelectionFlag.SingleSelection);
            return certs.Count > 0 ? certs[0] : null;
        }

        private void loadDigitalSignature(string filepath)
        {
            string fileextension = Path.GetExtension(filepath);
            try
            {
                if ((fileextension == ".docx") || (fileextension == ".docm"))
                    digitalSignature = new DigitalSignature(filepath, Types.WordProcessingML);
                else if ((fileextension == ".pptx") || (fileextension == ".pptm"))
                    digitalSignature = new DigitalSignature(filepath, Types.PresentationML);
                else if ((fileextension == ".xlsx") || (fileextension == ".xlsm"))
                    digitalSignature = new DigitalSignature(filepath, Types.SpreadSheetML);
                else if (fileextension == ".xps")
                    digitalSignature = new DigitalSignature(filepath, Types.XpsDocument);
            }
            catch (IOException e)
            {
                throw new IOException(e.Message, e.InnerException);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message,e.InnerException);
            }
        }
        
        #endregion

        #region Events

        private void chkCopyDocuments_CheckedChanged(object sender, EventArgs e)
        {
            gpbCopyDoduments.Enabled = chkCopyDocuments.Checked;
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            fbdSelectNewPath.ShowDialog();
            txtPath.Text = fbdSelectNewPath.SelectedPath;
        }

        private void chkOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkOverwrite.Checked)
            {
                if (MessageBox.Show("Você optou por não sobrescrever as cópias de segurança caso elas já existam.\nDeseja ainda assim prosseguir em assinar todos os documentos originais?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    chkOverwrite.Checked = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

            this.Close();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            compatibleDocuments.Clear();
            compatibleDocuments = FileOperations.ListAllowedFilesAndSubfolders(documentsToSign, true, chkIncludeSubfolders.Checked);

            if (compatibleDocuments.Count > 0)
            {
                string[] signedDocuments = signDocuments();
                if (signedDocuments != null)
                {
                    if (chkViewDocuments.Checked)
                    {
                        frmViewDigitalSignature FormViewDigitalSignature = new frmViewDigitalSignature(compatibleDocuments.ToArray());
                        FormViewDigitalSignature.Show();
                    }

                    this.Visible = false;

                    frmReport FormReport = new frmReport(documentsSignStatus, "sign");
                    FormReport.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Os arquivos selecionados não são pacotes válidos.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #endregion
    }
}
