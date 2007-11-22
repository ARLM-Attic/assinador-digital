using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace OpenXML
{
    public class Signers : CollectionBase, IEnumerable, IEnumerator 
    {
        #region Private Constants, Fields  and Properties

        private int index = -1;
        private string path;

        #endregion

        #region Public Constants and Fields

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        #endregion

        #region Constructor

        public Signers()
        {

        }

        #endregion

        #region Public Methods

        public Signer this[int index]
        {
            get { return (Signer)this.List[index]; }
            set { this.List[index] = value; }
        }

        public void Add(string name, string uri, string issuer, string date, string serial)
        {
            MoveNext();
            Signer sig = new Signer();
            sig.name = name;
            sig.date = date;
            sig.uri = uri;
            sig.issuer = issuer;
            sig.serialNumber = serial;
            this.List.Add(sig);
        }

        public void Add(Signer signer)
        {
            MoveNext();
            this.List.Add(signer);
        }

        public void Remove(Signer signer)
        {
            this.List.Remove(signer);
        }

        public bool Contains(Signer signer)
        {
            foreach (Signer sgn in this.InnerList)
            {
                if ((sgn.serialNumber == signer.serialNumber) &&
                    (sgn.issuer == signer.issuer) &&
                    (sgn.name == signer.name))
                    return true;
            }
            return false;
        }

        public bool HasSerialNumber(string serial)
        {
            foreach (Signer sgn in this.InnerList)
            {
                if (sgn.serialNumber == serial)
                    return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerator Members

        public Object Current
        {
            get
            {
                return this.List[index];
            }
        }

        public bool MoveNext()
        {
            this.index++;
            return (this.index < this.List.Count);
        }

        public void Reset()
        {
            this.index = -1;
        }

        #endregion
    }
}
