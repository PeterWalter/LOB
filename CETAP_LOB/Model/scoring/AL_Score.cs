using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.scoring
{
    public class AL_Score
    {
        private long _id;
        private int? al;       
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

        public int? AL
        {
            get
            {
                return al;
            }
            set
            {
                al = value;
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
        public int? AL1 { get; set; }
        public int? AL2 { get; set; }
        public int? AL3 { get; set; }
        public int? AL4 { get; set; }
        public int? AL5 { get; set; }
        public int? AL6 { get; set; }
        public int? AL7 { get; set; }
        public int? AL8 { get; set; }
        public int? AL9 { get; set; }

    }
}
