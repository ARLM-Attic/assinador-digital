using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace AssinadorDigital
{
    public partial class FormSignatureDetails : Form
    {
        private X509Certificate2 signatureCertificate = new X509Certificate2();
        private X509ChainStatus chainStatus = new X509ChainStatus();

        public FormSignatureDetails(X509Certificate2 certificate, X509ChainStatus status)
        {
            signatureCertificate = certificate;
            chainStatus = status;

            InitializeComponent();
        }

        private void statusUpdate()
        {
            if (chainStatus.Status == X509ChainStatusFlags.NoError)
            {
                txtStatus.Text = "Assinatura válida.\r\nEsta assinatura e o conteúdo assinado não foram alterados desde a aplicação da assinatura.\r\n" +
                   chainStatus.StatusInformation;
            }
            else
            {
                txtStatus.Text = chainStatus.Status.ToString() + "\r\n" +
                   chainStatus.StatusInformation;
            }
        }

        private void certificateUpdate()
        {
            ListViewGroup standardGroup = new ListViewGroup("standard", "Extensões padrão");
            lstDetails.Groups.Insert(0, standardGroup);

            ListViewItem itemSubject = new ListViewItem("Requerente");
            itemSubject.SubItems.Add(signatureCertificate.Subject.Replace("CN=", "").Replace("OU=", "").Replace("DC=", "").Replace("O=", "").Replace("C=", ""));
            itemSubject.Group = standardGroup;
            lstDetails.Items.Add(itemSubject);

            ListViewItem itemFriendlyName = new ListViewItem("Nome amigável");
            itemFriendlyName.SubItems.Add(signatureCertificate.FriendlyName);
            itemFriendlyName.Group = standardGroup;
            lstDetails.Items.Add(itemFriendlyName);

            ListViewItem itemIssuerName = new ListViewItem("Emissor");
            itemIssuerName.SubItems.Add(signatureCertificate.IssuerName.Name.Replace("CN=", "").Replace("OU=", "").Replace("DC=", "").Replace("O=", "").Replace("C=", ""));
            itemIssuerName.Group = standardGroup;
            lstDetails.Items.Add(itemIssuerName);

            ListViewItem itemSerialNumber = new ListViewItem("Número de série");
            itemSerialNumber.SubItems.Add(signatureCertificate.SerialNumber);
            itemSerialNumber.Group = standardGroup;
            lstDetails.Items.Add(itemSerialNumber);

            ListViewItem itemNotBefore = new ListViewItem("Válido de");
            itemNotBefore.SubItems.Add(signatureCertificate.NotBefore.ToString());
            itemNotBefore.Group = standardGroup;
            lstDetails.Items.Add(itemNotBefore);

            ListViewItem itemNotAfter = new ListViewItem("Válido até");
            itemNotAfter.SubItems.Add(signatureCertificate.NotAfter.ToString());
            itemNotAfter.Group = standardGroup;
            lstDetails.Items.Add(itemNotAfter);

            ListViewGroup customGroup = new ListViewGroup("custom", "Extensões adicionais");
            lstDetails.Groups.Insert(1, customGroup);
            foreach (X509Extension ext in signatureCertificate.Extensions)
            {
                ListViewItem item = new ListViewItem(ext.Oid.FriendlyName);
                item.SubItems.Add(ext.Format(true));
                item.Group = customGroup;

                lstDetails.Items.Add(item);
            }
        }

        private void FormSignatureDetails_Load(object sender, EventArgs e)
        {
            certificateUpdate();
            statusUpdate();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            X509Certificate2UI.DisplayCertificate((X509Certificate2)signatureCertificate);
        }
    }
}
