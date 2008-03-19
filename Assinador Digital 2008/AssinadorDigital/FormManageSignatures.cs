using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OPC;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using FileUtils;
using System.IO.Packaging;
using System.Xml;
using System.Security;
using System.Windows.Xps.Packaging;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Microsoft.Office.DocumentFormat.OpenXml.Packaging;

namespace AssinadorDigital
{
    public partial class frmManageDigitalSignature : Form
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public frmManageDigitalSignature(string[] paths, bool subfolders)
        {
            InitializeComponent();
            documents = paths;
            includeSubfolders = subfolders;
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Object of DigitalSignature
        /// </summary>
        private DigitalSignature digitalSignature;      
        //private List<string> selectedDocuments = new List<string>();
        private List<FileHistory> selectedDocuments = new List<FileHistory>();
        /// <summary>
        /// String[] of listed documents
        /// </summary>
        private string[] documents;
        private ArrayList signers = new ArrayList();
        private ListViewItem focusedSigner;
        /// <summary>
        /// ArrayList of selected documents in the list
        /// </summary>
        private ArrayList selectedSigners = new ArrayList();
        private ArrayList invalidSignatures = new ArrayList();
        private ArrayList documentProperties = new ArrayList();
        private List<string> compatibleDocuments = new List<string>();
        private bool includeSubfolders;
        private List<FileStatus> documentsPerformedAction = new List<FileStatus>();

        #endregion        

        #region Public Methods

        public void listFiles(List<FileHistory> filenames)
        {
            string[] filetype = new string[2];

            int length = filenames.Count;
            for (int i = 0; i < length; i++)
            {
                string file = filenames[i].NewPath;
                if (Path.HasExtension(file))
                {
                    string fileextension = Path.GetExtension(file);
                    bool documentFound = false;
                    foreach (ListViewItem documentAlreadyInList in lstDocuments.Items)
                    {
                        if (documentAlreadyInList.SubItems[3].Text == filenames[i].OriginalPath
                                || documentAlreadyInList.SubItems[2].Text == filenames[i].OriginalPath)
                        {
                            documentFound = true;
                            documentAlreadyInList.SubItems[2].Text = filenames[i].NewPath;
                            break;
                        }
                    }
                    if (!documentFound)
                    {
                        if (fileextension == ".docx")
                        {
                            filetype[0] = "0";
                            filetype[1] = "Microsoft Office Word Document";
                        }
                        else if (fileextension == ".docm")
                        {
                            filetype[0] = "1";
                            filetype[1] = "Microsoft Office Word Macro-Enabled Document";
                        }
                        else if (fileextension == ".pptx")
                        {
                            filetype[0] = "2";
                            filetype[1] = "Microsoft Office PowerPoint Presentation";
                        }
                        else if (fileextension == ".pptm")
                        {
                            filetype[0] = "3";
                            filetype[1] = "Microsoft Office PowerPoint Macro-Enabled Presentation";
                        }
                        else if (fileextension == ".xlsx")
                        {
                            filetype[0] = "4";
                            filetype[1] = "Microsoft Office Excel Worksheet";
                        }
                        else if (fileextension == ".xlsm")
                        {
                            filetype[0] = "5";
                            filetype[1] = "Microsoft Office Excel Macro-Enabled Worksheet";
                        }
                        else if (fileextension == ".xps")
                        {
                            filetype[0] = "6";
                            filetype[1] = "XPS Document";
                        }
                        else
                        {
                            filetype[0] = "-1";
                            filetype[1] = "Unknow";
                        }

                        if (filetype[0] != "-1")
                        {
                            ListViewItem listItem = new ListViewItem();         //INDEX
                            listItem.Text = Path.GetFileName(file);             //0 filename
                            listItem.ImageIndex = Convert.ToInt32(filetype[0]);
                            listItem.SubItems.Add(filetype[1]);                 //1 filetype
                            listItem.SubItems.Add(file);                        //2 filepath
                            listItem.SubItems.Add(filenames[i].OriginalPath);   //3 originalFilePath

                            lstDocuments.Items.Add(listItem);
                        }
                    }
                }
            }
            selectedDocuments.Clear();
            int count = lstDocuments.Items.Count;

            for (int i = 0; i < count; i++)
            {
                lstDocuments.Items[i].Selected = true;
                FileHistory fh = new FileHistory(lstDocuments.SelectedItems[i].SubItems[3].Text, lstDocuments.SelectedItems[i].SubItems[2].Text);
                selectedDocuments.Add(fh);
                lblSelected.Text = count.ToString();
            }
            if (lstDocuments.Items.Count > 0)
            {
                lstDocuments.Items[0].Focused = true;
            }
            loadSigners();
        }

        #endregion

        #region Private Methods

        private void listFiles(string[] filenames)
        {
            string[] filetype = new string[2];

            int length = filenames.Length;
            for (int i = 0; i < length; i++)
            {
                if (Path.HasExtension(filenames[i]))
                {
                    string fileextension = Path.GetExtension(filenames[i]);
                    bool documentFound = false;
                    foreach (ListViewItem documentAlreadyInList in lstDocuments.Items)
                    {
                        if (documentAlreadyInList.SubItems[3].Text == filenames[i])
                        {
                            documentFound = true;
                            break;
                        }
                    }
                    if (!documentFound)
                    {
                        if (fileextension == ".docx")
                        {
                            filetype[0] = "0";
                            filetype[1] = "Microsoft Office Word Document";
                        }
                        else if (fileextension == ".docm")
                        {
                            filetype[0] = "1";
                            filetype[1] = "Microsoft Office Word Macro-Enabled Document";
                        }
                        else if (fileextension == ".pptx")
                        {
                            filetype[0] = "2";
                            filetype[1] = "Microsoft Office PowerPoint Presentation";
                        }
                        else if (fileextension == ".pptm")
                        {
                            filetype[0] = "3";
                            filetype[1] = "Microsoft Office PowerPoint Macro-Enabled Presentation";
                        }
                        else if (fileextension == ".xlsx")
                        {
                            filetype[0] = "4";
                            filetype[1] = "Microsoft Office Excel Worksheet";
                        }
                        else if (fileextension == ".xlsm")
                        {
                            filetype[0] = "5";
                            filetype[1] = "Microsoft Office Excel Macro-Enabled Worksheet";
                        }
                        else if (fileextension == ".xps")
                        {
                            filetype[0] = "6";
                            filetype[1] = "XPS Document";
                        }
                        else
                        {
                            filetype[0] = "-1";
                            filetype[1] = "Unknow";
                        }

                        if (filetype[0] != "-1")
                        {
                            ListViewItem listItem = new ListViewItem();         //INDEX
                            listItem.Text = Path.GetFileName(filenames[i]);     //0 filename
                            listItem.ImageIndex = Convert.ToInt32(filetype[0]);
                            listItem.SubItems.Add(filetype[1]);                 //1 filetype
                            listItem.SubItems.Add(filenames[i]);                //2 filepath
                            listItem.SubItems.Add(filenames[i]);                //3 originalFilePath

                            lstDocuments.Items.Add(listItem);
                        }
                    }
                }
            }
            selectedDocuments.Clear();
            int count = lstDocuments.Items.Count;

            for (int i = 0; i < count; i++)
            {
                lstDocuments.Items[i].Selected = true;
                FileHistory fh = new FileHistory(lstDocuments.SelectedItems[i].SubItems[3].Text, lstDocuments.SelectedItems[i].SubItems[2].Text);
                selectedDocuments.Add(fh);
                lblSelected.Text = count.ToString();
            }
            if (lstDocuments.Items.Count > 0)
            {
                lstDocuments.Items[0].Focused = true;
            }
            loadSigners();
        }

        private void loadFileDescription()
        {
            if (selectedDocuments.Count > 0)
            {
                loadDigitalSignature(lstDocuments.FocusedItem.SubItems[2].Text);
                DocumentCoreProperties coreProps = new DocumentCoreProperties(digitalSignature.package, digitalSignature.xpsDocument, digitalSignature.DocumentType);
                documentProperties = coreProps.DocumentProperties;

                for (int i = 0; i < documentProperties.Count; i++)
                {
                    if (documentProperties[i].ToString().Length >= 30)
                        documentProperties[i] = documentProperties[i].ToString().Substring(0, 25) + "...";
                }

                string path = digitalSignature.signers.Path;
                tbName.Text = Path.GetFileName(path);
                if (Path.GetDirectoryName(path).EndsWith("\\"))
                    tbPath.Text = Path.GetDirectoryName(path);
                else
                    tbPath.Text = Path.GetDirectoryName(path) + "\\";
                lblCreator.Text = documentProperties[0].ToString();
                lblModifiedBy.Text = documentProperties[1].ToString();
                lblTitle.Text = documentProperties[2].ToString();
                lblDescription.Text = documentProperties[3].ToString();
                lblSubject.Text = documentProperties[4].ToString();
                lblCreated.Text = documentProperties[5].ToString();
                lblModified.Text = documentProperties[6].ToString();
            }
            else
            {
                tbName.Text = "";
                tbPath.Text = "";
                lblCreator.Text = "";
                lblModifiedBy.Text = "";
                lblTitle.Text = "";
                lblDescription.Text = "";
                lblSubject.Text = "";
                lblCreated.Text = "";
                lblModified.Text = "";
            }

        }

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
                MessageBox.Show("Memoria insufici�nte para executar a aplica��o.", "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException)
            {
                System.Windows.Forms.MessageBox.Show("O Documento foi fechado ou descartado, portanto n�o � poss�vel realizar opera��es com o mesmo. ", "Erro",
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

        public bool loadSigners()
        {
            if (documents == null)
                return false;

            this.Cursor = Cursors.WaitCursor;

            lstSigners.Items.Clear();
            lstSigners.Groups.Clear();

            if (lstDocuments.SelectedItems.Count > 0)
            {
                List<string> problematicFoundDocuments = new List<string>();

                Signers commonSigners = new Signers();
                foreach (FileHistory filepath in selectedDocuments)
                {
                    try
                    {
                        loadDigitalSignature(filepath.NewPath);

                        if (digitalSignature.error)
                        {
                            if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                            {
                                digitalSignature.xpsDocument.Close();
                            }
                            else
                            {
                                digitalSignature.package.Close();
                            }
                            this.Cursor = Cursors.Arrow;
                            return false;
                        }

                        invalidSignatures.Clear();
                        if (!digitalSignature.Validate())
                            invalidSignatures = digitalSignature.InvalidDigitalSignatureHolderNames;

                        ListViewGroup sigaturesGroup = new ListViewGroup(Path.GetFileName(filepath.NewPath));
                        lstSigners.Groups.Add(sigaturesGroup);

                        foreach (Signer signer in digitalSignature.signers)
                        {
                            string[] signature = new string[5];
                            signature[0] = signer.name;
                            signature[1] = signer.issuer;
                            signature[2] = signer.uri;
                            signature[3] = signer.date;
                            signature[4] = signer.serialNumber;

                            int signatureIcon = 0;

                            if (invalidSignatures.Contains(signature[0]) &&
                                invalidSignatures.Contains(signature[1]) &&
                                invalidSignatures.Contains(signature[2]) &&
                                invalidSignatures.Contains(signature[3]))
                            {
                                signatureIcon = 0;
                            }
                            else
                            {
                                signatureIcon = 1;
                            }

                            ListViewItem newSignerItem = new ListViewItem();    //INDEX
                            newSignerItem.Text = signature[0].ToString();       //0 signer.name
                            newSignerItem.ImageIndex = signatureIcon;
                            newSignerItem.Group = sigaturesGroup;
                            newSignerItem.SubItems.Add(signature[1]);           //1 signer.issuer
                            newSignerItem.SubItems.Add(signature[3]);           //2 signer.date
                            newSignerItem.SubItems.Add(filepath.NewPath);       //3 signer.path
                            newSignerItem.SubItems.Add(signature[4]);           //4 signer.serialNumber
                            newSignerItem.SubItems.Add(signature[2]);           //5 signer.URI
                            newSignerItem.SubItems.Add(filepath.OriginalPath);  //6 signer.originalPath

                            lstSigners.Items.Add(newSignerItem);

                            if (lstSigners.Groups.Count == 1)
                            {
                                Signer sgn = new Signer();
                                sgn.name = signature[0].ToString();
                                sgn.issuer = signature[1].ToString();
                                sgn.serialNumber = signature[4].ToString();
                                if (!commonSigners.Contains(sgn))
                                    commonSigners.Add(sgn);
                            }

                        }
                        Signers commonRecentlyFoundSigners = new Signers();
                        foreach (ListViewItem lst in sigaturesGroup.Items)
                        {
                            foreach (Signer sgn in commonSigners)
                            {
                                if (lst.SubItems[0].Text == sgn.name &&
                                    lst.SubItems[1].Text == sgn.issuer &&
                                    lst.SubItems[4].Text == sgn.serialNumber &&
                                    !commonRecentlyFoundSigners.Contains(sgn))
                                {
                                    commonRecentlyFoundSigners.Add(sgn);
                                }
                            }
                        }
                        commonSigners = commonRecentlyFoundSigners;
                    }
                    catch (IOException)
                    {
                        if (MessageBox.Show("Erro ao abrir o documento " + System.IO.Path.GetFileName(filepath.NewPath) + ".\nCertifique-se de que o documento n�o foi movido ou est� em uso por outra aplica��o.\n\nDeseja retir�-lo da lista?", System.IO.Path.GetFileName(filepath.NewPath),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    lstDocuments.Items.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    item.Selected = false;
                                    if (lstDocuments.SelectedItems.Count > 0)
                                    {
                                        lstDocuments.FocusedItem = lstDocuments.SelectedItems[0];
                                    }
                                }
                            }
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        if (MessageBox.Show("O Documento " + System.IO.Path.GetFileName(filepath.NewPath) + "n�o est� dispon�vel para abertura.\n\nDeseja retir�-lo da lista?", System.IO.Path.GetFileName(filepath.NewPath),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    lstDocuments.Items.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    item.Selected = false;
                                    if (lstDocuments.SelectedItems.Count > 0)
                                    {
                                        lstDocuments.FocusedItem = lstDocuments.SelectedItems[0];
                                    }
                                }
                            }
                        }
                    }
                    catch (OpenXmlPackageException)
                    {
                        if (MessageBox.Show("O Documento " + System.IO.Path.GetFileName(filepath.NewPath) + " n�o � um pacote Open XML v�lido.\n\nDeseja retir�-lo da lista?", System.IO.Path.GetFileName(filepath.NewPath),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    lstDocuments.Items.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    item.Selected = false;
                                    if (lstDocuments.SelectedItems.Count > 0)
                                    {
                                        lstDocuments.FocusedItem = lstDocuments.SelectedItems[0];
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {

                        if (MessageBox.Show("Houve um problema na listagem do seguinte documento:\n " + System.IO.Path.GetFileName(filepath.NewPath) + "\n" + err.Message.Substring(0, err.Message.IndexOf(".") + 1) + "\n\nDeseja exclu�-lo da lista?", System.IO.Path.GetFileName(filepath.NewPath),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    lstDocuments.Items.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            foreach (ListViewItem item in lstDocuments.Items)
                            {
                                if (item.SubItems[2].Text == filepath.NewPath)
                                {
                                    problematicFoundDocuments.Add(item.SubItems[2].Text);
                                    item.Selected = false;
                                    if (lstDocuments.SelectedItems.Count > 0)
                                    {
                                        lstDocuments.FocusedItem = lstDocuments.SelectedItems[0];
                                    }
                                }
                            }
                        }
                    }
                }

                if (lstDocuments.SelectedItems.Count > 1)
                {
                    ListViewGroup commonSigaturesGroup = new ListViewGroup("commonSignatures", "Assinaturas em Comum");
                    lstSigners.Groups.Insert(0, commonSigaturesGroup);

                    foreach (Signer sig in commonSigners)
                    {
                        ListViewItem newCommonSignerItem = new ListViewItem();
                        newCommonSignerItem.Text = sig.name;                    //0 signer.name
                        newCommonSignerItem.Group = commonSigaturesGroup;
                        newCommonSignerItem.SubItems.Add(sig.issuer);           //1 signer.issuer
                        newCommonSignerItem.SubItems.Add("");                   //2 signer.date
                        newCommonSignerItem.SubItems.Add("");                   //3 signer.path
                        newCommonSignerItem.SubItems.Add(sig.serialNumber);     //4 signer.serialNumber
                        newCommonSignerItem.SubItems.Add("");                   //5 signer.URI
                        newCommonSignerItem.SubItems.Add("");                   //6 signer.originalPath

                        lstSigners.Items.Add(newCommonSignerItem);
                    }
                }
                selectedDocuments.Clear();
                foreach (ListViewItem lvi in lstDocuments.SelectedItems)
                {
                    FileHistory fh = new FileHistory(lvi.SubItems[3].Text, lvi.SubItems[2].Text);
                    selectedDocuments.Add(fh);
                }
            }
            if (lstDocuments.SelectedItems.Count > 0)
            {
                loadFileDescription();
                if (digitalSignature.DocumentType.Equals(Types.XpsDocument))
                {
                    digitalSignature.xpsDocument.Close();
                }
                else
                {
                    digitalSignature.package.Close();
                }
            }
            else if (lstDocuments.Items.Count == 0)
            {
                MessageBox.Show("Os arquivos selecionados n�o s�o pacotes Open XML v�lidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            this.Cursor = Cursors.Arrow;
            return true;
        }

        #endregion                                 

        #region Events

        private void frmManageDigitalSignature_Load(object sender, EventArgs e)
        {
            compatibleDocuments.Clear();
            compatibleDocuments = FileOperations.ListAllowedFilesAndSubfolders(documents, true, includeSubfolders);
            listFiles(compatibleDocuments.ToArray());
        }

        private void frmManageDigitalSignature_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lstFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool status = true;
            if (lstDocuments.SelectedItems.Count < 1)
            {
                lstSigners.Items.Clear();
                lstSigners.Groups.Clear();
                status = false;
            }
            gpbSignatures.Enabled = status;
            btnSign.Enabled = status;
            btnRemoveAll.Enabled = status;
            btnRemove.Enabled = false;
        }

        private void lstSigners_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstSigners.SelectedItems.Count > 0)
                btnRemove.Enabled = true;
            else
                btnRemove.Enabled = false;

            for (int i = 0; i < lstSigners.Items.Count; i++)
            {
                if (lstSigners.Items[i].Selected)
                    signers.Add(lstSigners.Items[i]);
            }            
            focusedSigner = e.Item;       
            
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            List<FileHistory> documentsToAddSignatures = new List<FileHistory>();
            foreach (FileHistory document in selectedDocuments)
            {
                documentsToAddSignatures.Add(document);
            }

            frmAddDigitalSignature FormAddDigitalSignature = new frmAddDigitalSignature(documentsToAddSignatures, false);
            FormAddDigitalSignature.Owner = (frmManageDigitalSignature)this;
            FormAddDigitalSignature.ShowDialog();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<Signers> signaturesInDocumentsToBeRemoved = new List<Signers>();
            foreach (ListViewGroup lvg in lstSigners.Groups)
            {
                if (lvg.Name != "commonSignatures")
                {
                    Signers signers = new Signers();
                    for (int i = 0; i < lvg.Items.Count; i++)
                    {
                        if (lvg.Items[i].Selected)
                        {
                            signers.Path = lvg.Items[0].SubItems[6].Text;

                            Signer sgn = new Signer();
                            sgn.name = lvg.Items[i].SubItems[0].Text;
                            sgn.issuer = lvg.Items[i].SubItems[1].Text;
                            sgn.date = lvg.Items[i].SubItems[2].Text;
                            sgn.serialNumber = lvg.Items[i].SubItems[4].Text;
                            sgn.uri = lvg.Items[i].SubItems[5].Text;

                            if (!signers.Contains(sgn))
                            {
                                signers.Add(sgn);
                            }
                        }
                    }
                    if (signers.Count > 0)
                        signaturesInDocumentsToBeRemoved.Add(signers);
                }
                else
                {
                    Signers signers = new Signers();
                    signers.Path = "commonSignatures";

                    for (int i = 0; i < lvg.Items.Count; i++)
                    {
                        if (lvg.Items[i].Selected)
                        {
                            Signer sgn = new Signer();
                            sgn.name = lvg.Items[i].SubItems[0].Text;
                            sgn.issuer = lvg.Items[i].SubItems[1].Text;
                            sgn.date = lvg.Items[i].SubItems[2].Text;
                            sgn.serialNumber = lvg.Items[i].SubItems[4].Text;

                            if (!signers.Contains(sgn))
                            {
                                signers.Add(sgn);
                            }
                        }
                    }
                    if (signers.Count > 0)
                        signaturesInDocumentsToBeRemoved.Add(signers);
                }
            }
            List<FileHistory> documentsToRemoveSignatures = new List<FileHistory>();
            foreach (FileHistory document in selectedDocuments)
            {
                documentsToRemoveSignatures.Add(document);
            }
            frmRemoveDigitalSignatures FormRemoveDigitalSignatures = new frmRemoveDigitalSignatures(documentsToRemoveSignatures, signaturesInDocumentsToBeRemoved);
            FormRemoveDigitalSignatures.Owner = (frmManageDigitalSignature)this;
            FormRemoveDigitalSignatures.ShowDialog();
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            List<FileHistory> documentsToRemoveAllSignatures = new List<FileHistory>();
            foreach (FileHistory document in selectedDocuments)
            {
                documentsToRemoveAllSignatures.Add(document);
            }
            frmRemoveDigitalSignatures FormRemoveDigitalSignatures = new frmRemoveDigitalSignatures(documentsToRemoveAllSignatures);
            FormRemoveDigitalSignatures.Owner = (frmManageDigitalSignature)this;
            FormRemoveDigitalSignatures.ShowDialog();
        }

        private void lstFiles_MouseUp(object sender, MouseEventArgs e)
        {
            selectedDocuments.Clear();
            int count = lstDocuments.SelectedItems.Count;
            if (count > 0)
                gpbSignatures.Enabled = true;
            int i = 0;
            for (i = 0; i < count; i++)
            {
                FileHistory fh = new FileHistory(lstDocuments.SelectedItems[i].SubItems[3].Text, lstDocuments.SelectedItems[i].SubItems[2].Text);
                selectedDocuments.Add(fh);
            }

            lblSelected.Text = i.ToString();
            loadSigners();
        }

        private void lstFiles_KeyUp(object sender, KeyEventArgs e)
        {
            selectedDocuments.Clear();
            int count = lstDocuments.SelectedItems.Count;
            if (count > 0)
                gpbSignatures.Enabled = true;
            for (int i = 0; i < count; i++)
            {
                FileHistory fh = new FileHistory(lstDocuments.SelectedItems[i].SubItems[3].Text, lstDocuments.SelectedItems[i].SubItems[2].Text);
                selectedDocuments.Add(fh);
            }

            lblSelected.Text = count.ToString();
            loadSigners();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            selectedDocuments.Clear();
            int count = lstDocuments.Items.Count;

            for (int i = 0; i < count; i++)
            {
                lstDocuments.Items[i].Selected = true;
                FileHistory fh = new FileHistory(lstDocuments.SelectedItems[i].SubItems[3].Text, lstDocuments.SelectedItems[i].SubItems[2].Text);
                selectedDocuments.Add(fh);
            }
            if (lstDocuments.Items.Count > 0)
            {
                lstDocuments.Items[0].Focused = true;
            }
            lblSelected.Text = count.ToString();

            loadSigners();
        }

        #endregion
     }
}