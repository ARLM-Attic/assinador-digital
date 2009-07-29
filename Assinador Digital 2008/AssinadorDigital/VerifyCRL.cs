using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;

namespace AssinadorDigital
{
    public static class CertificateUtils
    {
        public static X509ChainStatus buildStatus;

        public static void VerifyConsultCRL()
        { 
            RegistryKey ConsultCRL = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
            if (!(Convert.ToBoolean(ConsultCRL.GetValue("ConsultCRL"))))
                if (MessageBox.Show("A Verificação de Certificados Revogados está desabilitada!\n\n                            " + 
                    "Deseja ativar agora?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    ConsultCRL.SetValue("ConsultCRL", true);
                }
        }

        public static X509Certificate2 GetCertificate()
        {
            X509Store certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs =
                X509Certificate2UI.SelectFromCollection(
                    certStore.Certificates,
                    "Selecionar um Certificado Digital.",
                    "Por favor selecione um certificado para assinatura.",
                    X509SelectionFlag.SingleSelection);

            if (certs.Count > 0)
            {
                RegistryKey ConsultCRL = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
                bool? valid = ValidateCertificate(certs[0], Convert.ToBoolean(ConsultCRL.GetValue("ConsultCRL")), true);
                ConsultCRL.Close();
                if (valid == null)
                {
                    return null;
                }
            }
            return certs.Count > 0 ? certs[0] : null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificate">X509Certificate2 to validate</param>
        /// <param name="checkCRL"></param>
        /// <param name="showAlert">Show a message box case the Certificate is not valid</param>
        /// <returns></returns>
        public static bool? ValidateCertificate(X509Certificate2 certificate, bool checkCRL, bool showAlert)
        {
            buildStatus = new X509ChainStatus();

            var chain = new X509Chain();
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
            chain.ChainPolicy.VerificationTime = DateTime.Now;

            if (checkCRL)
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            else
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;

            bool valid = chain.Build(certificate);

            if (!valid)
            {
                buildStatus = chain.ChainStatus[0];
                if (showAlert)
                {
                    if (MessageBox.Show("O Certificado Digital selecionado apresentou problemas de não conformidade.\n" +
                        "O seguinte erro foi apresentado:\n\n" +
                        buildStatus.StatusInformation + "\n" +
                        "Deseja utilizá-lo mesmo assim?", "Problema de não conformidade - " + buildStatus.Status.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return null;
                    }
                }
            }
            return valid;
        }
    }
}
