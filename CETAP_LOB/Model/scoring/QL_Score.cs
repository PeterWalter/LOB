using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.scoring
{
    public class QL_Score
    {
        private long _id;
        private int? ql;
        private string _language;
        private int? _testcode;
        public long ID
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

     
        public int? TestCode
        {
            get
            {
                return _testcode;
            }
            set
            {
                _testcode = value;
            }
        }
        public int? QL
        {
            get
            {
                return ql;
            }
            set
            {
                ql = value;
            }
        }
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        public int? S { get; set; }
        public int? C { get; set; }
        public int? R { get; set; }
        public int? P { get; set; }
        public int? Q { get; set; }
        public int? D { get; set; }
    }
}
