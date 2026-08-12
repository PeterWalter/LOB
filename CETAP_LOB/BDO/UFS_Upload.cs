using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.BDO
{
    public class UFS_Upload
    {
        [CsvColumn(FieldIndex = 2, Name = "NBT Reference")]
        public long NBT { get; set; }

        [CsvColumn(FieldIndex = 1, Name = "Test Session ID")]
        public long Barcode { get; set; }

        [CsvColumn(FieldIndex = 3, Name = "Surname")]
        public string Surname { get; set; }

        [CsvColumn(FieldIndex = 4, Name = "First Name")]
        public string Name { get; set; }

        [CsvColumn(FieldIndex = 5, Name = "Middle Initials")]
        public string Initials { get; set; }

        [CsvColumn(FieldIndex = 6, Name = "South African ID")]
        public string SAID { get; set; }

        [CsvColumn(FieldIndex = 7, Name = "Foreign ID")]
        public string ForeignID { get; set; }

        [CsvColumn(FieldIndex = 8, Name = "Date of Birth")]
        public string DOB { get; set; }

      
        [CsvColumn(FieldIndex = 9, Name = "Wrote AL")]
        public string WroteAL { get; set; }

        [CsvColumn(FieldIndex = 10, Name = "Wrote QL")]
        public string WroteQL { get; set; }

        [CsvColumn(FieldIndex = 11, Name = "Wrote Mat")]
        public string WroteMat { get; set; }

        [CsvColumn(FieldIndex = 12, Name = "Student Number")]
        public string student_number { get; set; }

        [CsvColumn(FieldIndex = 13, Name = "Faculty")]
        public string Faculty4 { get; set; }

        [CsvColumn(FieldIndex = 14, Name = "Programme")]
        public string Programme { get; set; }

        [CsvColumn(FieldIndex = 15, Name = "Date_Of_Test")]
        public string DOT { get; set; }      

        [CsvColumn(FieldIndex = 16, Name = "Venue")]
        public string VenueName { get; set; }
        
        [CsvColumn(FieldIndex = 17, Name = "Gender")]
        public string Gender { get; set; }

        [CsvColumn(FieldIndex = 18, Name = "Street and Number")]
        public string street { get; set; }

        [CsvColumn(FieldIndex = 19, Name = "Street Name")]
        public string streetName { get; set; }

        [CsvColumn(FieldIndex = 20, Name = "Suburb")]
        public string suburb { get; set; }
        
        [CsvColumn(FieldIndex = 21, Name = "City/Town")]
        public string city { get; set; }

        [CsvColumn(FieldIndex = 22, Name = "Province/Region")]
        public string province { get; set; }

        [CsvColumn(FieldIndex = 23, Name = "Postal Code")]
        public string zip { get; set; }

        [CsvColumn(FieldIndex = 24, Name = "e-mail Address")]
        public string e_mail { get; set; }

        [CsvColumn(FieldIndex = 25, Name = "Landline Number")]
        public string phone { get; set; }

        [CsvColumn(FieldIndex = 26, Name = "Mobile Number")]
        public string mobile { get; set; }

        //[CsvColumn(FieldIndex = 22, Name = "MatCode")]
        //public string MATCode { get; set; }

        [CsvColumn(FieldIndex = 27, Name = "AL Score")]
        public int? ALScore { get; set; }

        [CsvColumn(FieldIndex = 28, Name = "AL Performance")]
        public string ALLevel { get; set; }

        [CsvColumn(FieldIndex = 29, Name = "QL Score")]
        public int? QLScore { get; set; }

        [CsvColumn(FieldIndex = 30, Name = "QL Performance")]
        public string QLLevel { get; set; }

        [CsvColumn(FieldIndex = 31, Name = "Maths Score")]
        public string MATScore { get; set; }

        [CsvColumn(FieldIndex = 32, Name = "Maths Performance")]
        public string MATLevel { get; set; }

        [CsvColumn(FieldIndex = 33, Name = "AQL TEST LANGUAGE")]
        public string AQLLanguage { get; set; }

        [CsvColumn(FieldIndex = 34, Name = "MATHS TEST LANGUAGE")]
        public string MATLanguage { get; set; }


        //[CsvColumn(FieldIndex = 28, Name = "Maths Performance")]
        //public string MATLevel { get; set; }
    }
}
