using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Xps.Packaging;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;

namespace XPSDocument
{
    public class XpsManager
    {
        #region Public properties
        public XpsDocument xpsDocument;
        public ArrayList _XpsProperties = new ArrayList();

        public ArrayList XpsProperties
        {
            get { return _XpsProperties; }
            set { }
        }
        #endregion

        #region Prvate properties
        #endregion

        #region Constructor
        public XpsManager(string filePath)
        {
            xpsDocument = new XpsDocument(filePath, FileAccess.ReadWrite);
        }
        #endregion

        #region Public Methods
        public X509Certificate2 GetCertificate()
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

        public void SignXps(XpsDocument xpsDocument, X509Certificate cert)
        {
            xpsDocument.SignDigitally(
                cert, true, XpsDigSigPartAlteringRestrictions.None
                );
        }

        public void RemoveSignXps(XpsDigitalSignature signature)
        {
            xpsDocument.RemoveSignature(signature);
        }

        public void CorePropertiesSignXps()
        {
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Creator.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.LastModifiedBy.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Title.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Description.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Subject.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Created.ToString());
            XpsProperties.Add(xpsDocument.CoreDocumentProperties.Modified.ToString());
        }

        public List<XpsDigitalSignature> Signatures()
        {
            return xpsDocument.Signatures.ToList();            
        }

        #endregion
    }
}
