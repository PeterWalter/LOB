using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class easyPayRec
    {
        private Transaction _Transaction;

        private Payment _payment;

        private List<Tender> _tenders;

        private Tender _tender;

        public List<Tender> Tenders
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

        public Transaction Transaction
        {
            get
            {
                return _Transaction;
            }
            set
            {
                _Transaction = value;
            }
        }

        public Payment Payment
        {
            get
            {
                return _payment;
            }
            set
            {
                _payment = value;
            }
        }

        public Tender Tender
        {
            get
            {
                return _tender;
            }
            set
            {
                _tender = value;
            }
        }
        public easyPayRec()
        {
            _payment = new Payment();
            _tenders = new List<Tender>();
            _tender = new Tender();
            _Transaction = new Transaction();
        }
    }
}
