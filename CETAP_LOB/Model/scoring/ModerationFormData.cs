using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.scoring
{
    public class ModerationFormData
    {
        public string TestDate { get; set; }
        public string ModerationDate { get; set; }
        public string TestType { get; set; }
        public string TotalTestWriters { get; set; }
        public string TotalModerationRecords { get; set; }
        public string CheckedInBy { get; set; }
        public string ModerationFindings { get; set; }
    }
}
