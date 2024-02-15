using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
   public class Transaction
    {
        private string _id;

        private long _collectorID;

        private DateTime _date;

        private DateTime _payTime;

        private string _pos;

        private string _trace;

        public DateTime PayDay
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public string Identifier
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string TracePayee
        {
            get
            {
                return _trace;
            }
            set
            {
                _trace = value;
            }
        }

        public long CollectorID
        {
            get
            {
                return _collectorID;
            }
            set
            {
                _collectorID = value;
            }
        }

        public DateTime PayTime
        {
            get
            {
                return _payTime;
            }
            set
            {
                _payTime = value;
            }
        }

        public string PointOfService
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
            }
        }
    }
}
