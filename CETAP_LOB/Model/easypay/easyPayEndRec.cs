using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class easyPayEndRec
    {
        public easyPayEndRec()
        {
            
        }
        private long _numPayments;

        private decimal _pTotal;

        private decimal _fees;

        private long _numTenders;

        private decimal _tenders;

        private decimal _bankCosts;

        public decimal TotalPayments
        {
            get
            {
                return _pTotal;
            }
            set
            {
                _pTotal = value;
            }
        }

        public long NumberOfPayments
        {
            get
            {
                return _numPayments;
            }
            set
            {
                _numPayments = value;
            }
        }

        public long NumberOfTenders
        {
            get
            {
                return _numTenders;
            }
            set
            {
                _numTenders = value;
            }
        }

        public decimal Fees
        {
            get
            {
                return _fees;
            }
            set
            {
                _fees = value;
            }
        }

        public decimal TotalTenders
        {
            get
            {
                return _tenders;
            }
            set
            {
                _tenders = value;
            }
        }

        public decimal BankCosts
        {
            get
            {
                return _bankCosts;
            }
            set
            {
                _bankCosts = value;
            }
        }
    }
}
