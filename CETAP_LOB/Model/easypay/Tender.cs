using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
   public class Tender
    {
        private string _Id;

        private decimal _amount;

        private string _tTpye;

        private string _account;

        private decimal _bankCost;

        public string Identifier
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
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

        public decimal BankCost
        {
            get
            {
                return _bankCost;
            }
            set
            {
                _bankCost = value;
            }
        }

        public string TenderType
        {
            get
            {
                return _tTpye;
            }
            set
            {
                _tTpye = value;
            }
        }

        public string AccountNumber
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }
    }
}
