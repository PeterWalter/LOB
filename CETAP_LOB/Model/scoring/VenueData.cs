using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.scoring
{
    public class VenueData
    {
        public int VenueCode { get; set; }
        public string VenueName { get; set; }
        public int FullCount { get; set; }
        public int ModerationCount { get; set; }
        public string Percentage { get; set; }
    }
}
