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
using Microsoft.Win32;

namespace AssinadorDigital
{
    public partial class frmRemoveDigitalSignatures : Form
    {
        #region Constructor

        /// <summary>
        /// RemoveAllSignatures from the selected documents
        /// </summary>
        /// <param name="documents"></param>
        public frmRemoveDigitalSignatures(List<string> documents)
        {
            InitializeComponent();

            removeSignaturesActionType = removeSignaturesType.removeAllSignatures;
            selectedDocumentsToRemoveSignature = documents;


            LastBackedUpFolder = Registry.LocalMachine.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
            txtPath.Text = LastBackedUpFolder.GetValue("LastBackUpFolder").ToString();
        }
        /// <summary>
        /// Remove the list os signers from the documents
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="selectedSignatures"></param>
        public frmRemoveDigitalSignatures(List<string> documents, List<Signers> selectedSignatures)
        {
            InitializeComponent();

            removeSignaturesActionType = removeSignaturesType.removeSelectedSignatures;
            selectedDocumentsToRemoveSignature = documents;
            selectedSignaturesInDocuments = selectedSignatures;

            LastBackedUpFolder = Registry.LocalMachine.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
            txtPath.Text = LastBackedUpFolder.GetValue("LastBackUpFolder").ToString();
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Object of DigitalSignature
        /// </summary>
        private DigitalSignature digitalSignature;
        private List<string> selectedDocumentsToRemoveSignature = new List<string>();
        private removeSignaturesType removeSignaturesActionType = new removeSignaturesType();      
        private List<FileStatus> documentsRemoveSignStatus = new List<FileStatus>();
        private List<Signers> selectedSignaturesInDocuments;
        private RegistryKey LastBackedUpFolder = null;

        #endregion

        #region Public Properties

        public enum removeSignaturesType
        {
            removeAllSignatures,
            removeSelectedSignatures
        }

        #endregion

        #region Private Methods

        private void loadDigitalSignature(string filepath)
        {
            string fileextension = Path.GetExtension(filepath);
            try
            {
                if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                {
                    digitalSignature.xpsDocument.Close();
                }
                else
                {
                    digitalSignature.package.Close();
                }
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Memoria insuficiênte para executar o programa", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException)
            {
                System.Windows.Forms.MessageBox.Show("Documento foi fechado ou descartado, portanto não é possível realizar operações com o mesmo. ", "Erro",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch { }
            finally
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
        }

        private void viewReport(List<FileStatus> fileStatusList)
        {
            frmReport FormReport = new frmReport(fileStatusList, "remove");
            FormReport.ShowDialog();
        }

        private void removeSignatures()
        {
            List<string> documentsReadyToRemoveSignature = new List<string>();

            List<string> documentsToInteract = new List<string>();

            switch (removeSignaturesActionType)
            {
                case removeSignaturesType.removeAllSignatures:
                    foreach (string documentsToRemoveSignatures in selectedDocumentsToRemoveSignature)
                    {
                        documentsToInteract.Add(documentsToRemoveSignatures);
                    }
                    break;
                case removeSignaturesType.removeSelectedSignatures:
                    if (selectedSignaturesInDocuments != null)
                    {
                        foreach (Signers signers in selectedSignaturesInDocuments)
                        {
                            if (signers.Path != "commonSignatures")
                            {
                                documentsToInteract.Add(signers.Path);
                            }
                            else
                            {
                                documentsToInteract.Clear();
                                documentsToInteract = selectedDocumentsToRemoveSignature;
                                break;
                            }
                        }
                    }
                    break;
            }

            if (chkCopyDocuments.Checked)
            {
                documentsRemoveSignStatus = FileOperations.Copy(documentsToInteract, txtPath.Text, chkOverwrite.Checked);

                foreach (FileStatus documentToRemoveSign in documentsRemoveSignStatus)
                {
                    if (documentToRemoveSign.Status == Status.Success ||
                        documentToRemoveSign.Status == Status.Unmodified)
                    {
                        documentsReadyToRemoveSignature.Add(documentToRemoveSign.Path);
                    }
                }
            }
            else
            {
                foreach (string documentToRemoveSignature in documentsToInteract)
                {
                    documentsReadyToRemoveSignature.Add(documentToRemoveSignature);
                    FileStatus documentStatus = new FileStatus(documentToRemoveSignature, FileUtils.Status.ModifiedButNotBackedUp);

                    documentsRemoveSignStatus.Add(documentStatus);
                }
            }

            switch (removeSignaturesActionType)
            {
                case removeSignaturesType.removeAllSignatures:
                    foreach (FileStatus documentToRemoveSignature in documentsRemoveSignStatus)
                    {
                        try
                        {
                            loadDigitalSignature(documentToRemoveSignature.Path);
                            digitalSignature._RemoveAllSignatures();
                        }
                        catch (NullReferenceException)
                        {
                            documentToRemoveSignature.Status = Status.GenericError;
                        }
                        catch (FileFormatException)
                        {
                            documentToRemoveSignature.Status = Status.CorruptedContent;
                        }
                        catch (IOException)
                        {
                            documentToRemoveSignature.Status = Status.InUseByAnotherProcess;
                        }
                        catch (Exception)
                        {
                            documentToRemoveSignature.Status = Status.GenericError;
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
                    break;
                case removeSignaturesType.removeSelectedSignatures:
                    List<FileStatus> removeSignatureStatusList = new List<FileStatus>();
                    FileStatus selectedFileStatus = null;
                    foreach (string readyToRemoveSignature in documentsReadyToRemoveSignature)
                    {
                        foreach (Signers document in selectedSignaturesInDocuments)
                        {
                            if ((readyToRemoveSignature == document.Path) || (document.Path == "commonSignatures"))
                            {
                                try
                                {
                                    if (document.Path != "commonSignatures")
                                    {
                                        foreach (Signer signer in document)
                                        {
                                            loadDigitalSignature(document.Path);
                                            Uri signUri = new Uri(signer.uri, UriKind.Relative);
                                            digitalSignature.RemoveUniqueSignatureFromFile(signUri);
                                        }
                                    }
                                    else
                                    {
                                        foreach (string documentReady in documentsReadyToRemoveSignature)
                                        {
                                            loadDigitalSignature(documentReady);
                                            foreach (Signer signer in document)
                                            {
                                                string serialNumber = signer.serialNumber;
                                                digitalSignature.RemoveSignaturesFromFilesBySigner(serialNumber);
                                            }
                                        }
                                    }
                                    if (chkCopyDocuments.Checked)
                                    {
                                        selectedFileStatus = new FileStatus(document.Path, Status.Success);
                                    }
                                    else 
                                    {
                                        selectedFileStatus = new FileStatus(document.Path, Status.ModifiedButNotBackedUp);
                                    }
                                        removeSignatureStatusList.Add(selectedFileStatus);
                                    
                                }
                                catch (NullReferenceException)
                                {
                                    selectedFileStatus = new FileStatus(document.Path, Status.GenericError);
                                    removeSignatureStatusList.Add(selectedFileStatus);
                                }
                                catch (FileFormatException)
                                {
                                    selectedFileStatus = new FileStatus(document.Path, Status.CorruptedContent);
                                    removeSignatureStatusList.Add(selectedFileStatus);
                                }
                                catch (IOException)
                                {
                                    selectedFileStatus = new FileStatus(document.Path, Status.InUseByAnotherProcess);
                                    removeSignatureStatusList.Add(selectedFileStatus);
                                }
                                catch (Exception)
                                {
                                    selectedFileStatus = new FileStatus(document.Path, Status.GenericError);
                                    removeSignatureStatusList.Add(selectedFileStatus);
                                }

                            }
                        }
                        //if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                        //{
                        //    digitalSignature.xpsDocument.Close();
                        //}
                        //else
                        //{
                        //    digitalSignature.package.Close();
                        //}
                    }

                    foreach (FileStatus documentReadyStatus in documentsRemoveSignStatus)
                    {
                        if (removeSignatureStatusList.Contains(documentReadyStatus))
                            documentsRemoveSignStatus.Remove(documentReadyStatus);
                    }
                    break;
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
            LastBackedUpFolder.SetValue("LastBackUpFolder", txtPath.Text, RegistryValueKind.String);
        }

        private void chkOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkOverwrite.Checked)
            {
                if (MessageBox.Show("Você optou por não sobrescrever as cópias de segurança caso elas já existam.\nDeseja ainda assim prosseguir em remover as assinaturas de todos os documentos originais selecionados?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    chkOverwrite.Checked = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeSignatures();
            viewReport(documentsRemoveSignStatus);

            this.Close();
        }

        #endregion
    }
}
