using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class easyPayStartRec
    {
        private string _identifier;

        private int _version;

        private long _recID;

        private DateTime date;

        private DateTime _creationTime;

        private int _fGN;
        public easyPayStartRec()
        {
            
        }
        public long ReceiverIdentifier
        {
            get
            {
                return _recID;
            }
            set
            {
                _recID = value;
            }
        }
        public int FileVersion
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        public int FileGenerationNumber
        {
            get
            {
                return _fGN;
            }
            set
            {
                _fGN = value;
            }
        }

        public DateTime FileDate
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }
            set
            {
                _creationTime = value;
            }
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                _identifier = value;
            }
        }
    }
}
