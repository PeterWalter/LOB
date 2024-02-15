using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class Payment
    {
        private string id;

        private decimal _amount;

        private decimal _fee;

        private long _paytag;

        public string Identifier
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        public decimal Fee
        {
            get
            {
                return _fee;
            }
            set
            {
                _fee = value;
            }
        }

        public long PayTag
        {
            get
            {
                return _paytag;
            }
            set
            {
                _paytag = value;
            }
        }
    }
}
