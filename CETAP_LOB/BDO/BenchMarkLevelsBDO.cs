﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.BDO
{
    public class BenchMarkLevelsBDO
    {
        public int yearID { get; set; }
        public int YearSet { get; set; }
        public int StartIntakeYear { get; set; }
        public int EndIntakeYear { get; set; }
        public int AL_PU { get; set; }
        public int AL_PL { get; set; }
        public int AL_IU { get; set; }
        public int AL_IL { get; set; }
        public int AL_BU { get; set; }
        public int AL_BL { get; set; }
        public int QL_PU { get; set; }
        public int QL_PL { get; set; }
        public int QL_IU { get; set; }
        public int QL_IL { get; set; }
        public int QL_BU { get; set; }
        public int QL_BL { get; set; }
        public int MAT_PU { get; set; }
        public int MAT_PL { get; set; }
        public int MAT_IU { get; set; }
        public int MAT_IL { get; set; }
        public int MAT_BU { get; set; }
        public int MAT_BL { get; set; }
        public System.DateTime DateModified { get; set; }
        public string Type { get; set; }
    }
}