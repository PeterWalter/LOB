using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Helper
{
    public class EasyPayRecord
    {
        private string _date;
        private string _time;
        private string _xRecord;
        private string _pRecord;

        public long NBT { get; set; }
        public long SAID { get; set; }
        public double Amount { get; set; }
        public string EasyRecord
        {
            get
            {
                CreateXrecord();
                createTPrecord();
                return _xRecord + _pRecord;
            }
        }

        private void CreateXrecord()
        {
            long num = 1112299999;
            DateTime now = DateTime.Now;
            _date = now.ToString("yyyyMMdd");
            _time = now.ToString("HHMMss");
            _xRecord = "X," + num + "," + _date + "," + _time + ",0263," + SAID + Environment.NewLine;
        }

        private void createTPrecord()
        {
            string start = "P,";
            string mycash = string.Format("{0,10:######0.00}", Amount);
            string thisline = start + mycash;
            string fake = "," + string.Format("{0,10:######0.00}", 0.0) + ",";
            _pRecord = thisline + fake + NBT + Environment.NewLine;
            string otherline = "" + mycash + fake + "Cash" + Environment.NewLine;
            _pRecord += otherline;
        }
    }
}
