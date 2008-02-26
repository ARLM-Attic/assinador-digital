using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenXML;
using System.Security.Cryptography.X509Certificates;

namespace AssinadorDigital
{
    public partial class frmAddDigitalSignature : Form
    {
        #region Constructor

        public frmAddDigitalSignature(string[] args)
        {
            InitializeComponent();
            documentsToSign = args;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Object of DigitalSignature
        /// </summary>
        private DigitalSignature digitalSignature;

        List<string> compatibleDocuments = new List<string>();

        private string[] documentsToSign;

        #endregion

        #region Events

        private void frmAddDigitalSignature_Load(object sender, EventArgs e)
        {
            selectCompatibleDocuments(documentsToSign, true);
        }

        private void chkCopyDocuments_CheckedChanged(object sender, EventArgs e)
        {
            gpbCopyDoduments.Enabled = chkCopyDocuments.Checked;
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            fbdSelectNewPath.ShowDialog();
            txtPath.Text = fbdSelectNewPath.SelectedPath;
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            frmViewDigitalSignature FormViewDigitalSignature = new frmViewDigitalSignature(signDocuments(), chkViewDocuments.Checked);
            FormViewDigitalSignature.Show();
        }

        #endregion

        #region Private Methods

        private void selectCompatibleDocuments(string[] filenames, bool openSubFolders)
        {
            int length = filenames.Length;
            for (int i = 0; i < length; i++)
            {
                if (Path.HasExtension(filenames[i]))
                {
                    string fileExtension = Path.GetExtension(filenames[i]);
                    if ((fileExtension == ".docx") || (fileExtension == ".docm")
                        || (fileExtension == ".pptx") || (fileExtension == ".pptm")
                        || (fileExtension == ".xlsx") || (fileExtension == ".xlsm")
                        || (fileExtension == ".xps"))
                    {
                        compatibleDocuments.Add(filenames[i]);
                    }
                }
                else
                {
                    if (openSubFolders)
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(filenames[i]);
                        string[] files = new string[dInfo.GetFiles().Length];
                        int j = 0;

                        foreach (FileInfo file in dInfo.GetFiles())
                        {
                            files[j] = file.FullName.ToString();
                            j++;
                        }
                        selectCompatibleDocuments(files, true);
                    }
                }
            }
        }

        private string[] signDocuments()
        {
            //String that receives an specific XML object file (Office 2007 compatibility)
            String officeDocument = Properties.Resources.OfficeObject;

            loadDigitalSignature(compatibleDocuments[0]);
            if (digitalSignature != null)
            {
                //Call the method that get the digital certificate
                X509Certificate2 certificate = digitalSignature.GetCertificate();
                //Call the method that stores the XML in a properties
                digitalSignature.SetOfficeDocument(officeDocument);

                String key = null;
                List<string> signedDocuments = new List<string>();

                foreach (string documentToSign in compatibleDocuments)
                {
                    string newPath = documentToSign;
                    if (chkCopyDocuments.Checked)
                    {
                        if (txtPath.Text.EndsWith("\\"))
                            newPath = txtPath.Text + Path.GetFileName(documentToSign);
                        else
                            newPath = txtPath.Text + "\\" + Path.GetFileName(documentToSign);

                        digitalSignature.package.Close();
                        File.Copy(documentToSign, newPath);
                    }
                    key = newPath;
                    loadDigitalSignature(newPath);
                    signedDocuments.Add(newPath);

                    if (certificate != null)
                        //Call the method that Sign the open package
                        digitalSignature.SignPackage(certificate);
                }
                string[] documents = new string[signedDocuments.Count];
                int i = 0;
                foreach (string signedDocument in signedDocuments)
                {
                    documents[i] = signedDocument;
                    i++;
                }
                digitalSignature.package.Close();
                return documents;
            }
            return null;
        }

        private void loadDigitalSignature(string filepath)
        {
            string fileextension = Path.GetExtension(filepath);
            try
            {
                digitalSignature.package.Close();
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Memória insuficiênte para executar a aplicação", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException)
            {
                System.Windows.Forms.MessageBox.Show("O Documento foi fechado ou descartado, portanto não é possível realizar operações com o mesmo. ", "Erro",
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
            }
        }
        
        #endregion
    }
}
