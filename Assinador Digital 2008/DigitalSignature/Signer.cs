using System;
using System.Collections.Generic;
using System.Text;

namespace OPC
{
    public class Signer
    {
        #region Public Fields and Constants

        public string name;
        public string uri;
        public string issuer;
        public string serialNumber;
        public string date;
        public bool isValid;

        #endregion

        #region Constructor

        public Signer()
        {

        }
        #endregion
    }
}
