
using LINQtoCSV;

namespace CETAP_LOB.BDO
{
  public class NBTWebUpload
  {
    [CsvColumn(FieldIndex = 1, Name = "RefNo")]
    public long NBT { get; set; }
    
    [CsvColumn(FieldIndex = 2, Name = "Barcode")]
    public long Barcode { get; set; }

    [CsvColumn(FieldIndex = 3, Name = "Surname")]
    public string Surname { get; set; }

    [CsvColumn(FieldIndex = 4, Name = "Name")]
    public string Name { get; set; }

    [CsvColumn(FieldIndex = 5, Name = "Initials")]
    public string Initials { get; set; }

    [CsvColumn(FieldIndex = 6, Name = "SAID")]
    public long? SAID { get; set; }

    [CsvColumn(FieldIndex = 7, Name = "ForeignID")]
    public string ForeignID { get; set; }

    [CsvColumn(FieldIndex = 8, Name = "DOB")]
    public string DOB { get; set; }

    [CsvColumn(FieldIndex = 9, Name = "ID_Type")]
    public string ID_Type { get; set; }

    [CsvColumn(FieldIndex = 10, Name = "Citizenship")]
    public string Citizenship { get; set; }

    [CsvColumn(FieldIndex = 11, Name = "Classification")]
    public string Classification { get; set; }

    [CsvColumn(FieldIndex = 12, Name = "Gender")]
    public string Gender { get; set; }

    [CsvColumn(FieldIndex = 13, Name = "Faculty")]
    public string Faculty { get; set; }

        [CsvColumn(FieldIndex = 14, Name = "DOT")]
        public string DOT { get; set; }

        [CsvColumn(FieldIndex = 15, Name = "VenueCode")]
        public string VenueCode { get; set; }

        [CsvColumn(FieldIndex = 16, Name = "VenueName")]
        public string VenueName { get; set; }

        [CsvColumn(FieldIndex = 17, Name = "HomeLanguage")]
        public string HomeLanguage { get; set; }

        [CsvColumn(FieldIndex = 18, Name = "GR12Language")]
        public string GR12Language { get; set; }

        [CsvColumn(FieldIndex = 19, Name = "AQLLanguage")]
        public string AQLLanguage { get; set; }

        [CsvColumn(FieldIndex = 20, Name = "AQLCode")]
        public string AQLCode { get; set; }

        [CsvColumn(FieldIndex = 21, Name = "MatLanguage")]
        public string MATLanguage { get; set; }

        [CsvColumn(FieldIndex = 22, Name = "MatCode")]
        public string MATCode { get; set; }

        [CsvColumn(FieldIndex = 23, Name = "ALScore")]
        public int? ALScore { get; set; }

        [CsvColumn(FieldIndex = 24, Name = "ALLevel")]
        public string ALLevel { get; set; }

        [CsvColumn(FieldIndex = 25, Name = "QLScore")]
        public int? QLScore { get; set; }

        [CsvColumn(FieldIndex = 26, Name = "QLLevel")]
        public string QLLevel { get; set; }

        [CsvColumn(FieldIndex = 27, Name = "MATScore")]
        public string MATScore { get; set; }

        [CsvColumn(FieldIndex = 28, Name = "MATLevel")]
        public string MATLevel { get; set; }

        [CsvColumn(FieldIndex = 29, Name = "WroteAL")]
        public string WroteAL { get; set; }

        [CsvColumn(FieldIndex = 30, Name = "WroteQL")]
        public string WroteQL { get; set; }

        [CsvColumn(FieldIndex = 31, Name = "WroteMat")]
        public string WroteMat { get; set; }

        [CsvColumn(FieldIndex = 32, Name = "Faculty2")]
        public string Faculty2 { get; set; }

        [CsvColumn(FieldIndex = 33, Name = "Faculty3")]
        public string Faculty3 { get; set; }

        [CsvColumn(FieldIndex = 34, Name = "I_Barcode")]
        public long I_Barcode { get; set; }

        [CsvColumn(FieldIndex = 35, Name = "TestName")]
        public string TestName { get; set; }

        [CsvColumn(FieldIndex = 36, Name = "AL Cohesion")]
        public string AL_Cohesion { get; set; }

        [CsvColumn(FieldIndex = 37, Name = "AL CommunicativeFunction")]
        public string AL_CommunicativeFunction { get; set; }

        [CsvColumn(FieldIndex = 38, Name = "AL Discourse")]
        public string AL_Discourse { get; set; }

        [CsvColumn(FieldIndex = 39, Name = "AL Essential")]
        public string AL_Essential { get; set; }

        [CsvColumn(FieldIndex = 40, Name = "AL Grammar")]
        public string AL_Grammar { get; set; }

        [CsvColumn(FieldIndex = 41, Name = "AL Inference")]
        public string AL_Inference { get; set; }

        [CsvColumn(FieldIndex = 42, Name = "AL Metaphor")]
        public string AL_Metaphor { get; set; }

        [CsvColumn(FieldIndex = 43, Name = "AL TextGenre")]
        public string AL_TextGenre { get; set; }

        [CsvColumn(FieldIndex = 44, Name = "AL Vocabulary")]
        public string AL_Vocabulary { get; set; }

        [CsvColumn(FieldIndex = 45, Name = "QL C")]
        public string QL_C { get; set; }

        [CsvColumn(FieldIndex = 46, Name = "QL D")]
        public string QL_D { get; set; }

        [CsvColumn(FieldIndex = 47, Name = "QL P")]
        public string QL_P { get; set; }

        [CsvColumn(FieldIndex = 48, Name = "QL Q")]
        public string QL_Q { get; set; }

        [CsvColumn(FieldIndex = 49, Name = "QL R")]
        public string QL_R { get; set; }

        [CsvColumn(FieldIndex = 50, Name = "QL S")]
        public string QL_S { get; set; }

        [CsvColumn(FieldIndex = 51, Name = "MATTestName")]
        public string MatTestName { get; set; }

        [CsvColumn(FieldIndex = 52, Name = "MAT M1")]
        public string MAT_M1 { get; set; }

        [CsvColumn(FieldIndex = 53, Name = "MAT M2")]
        public string MAT_M2 { get; set; }

        [CsvColumn(FieldIndex = 54, Name = "MAT M3")]
        public string MAT_M3 { get; set; }

        [CsvColumn(FieldIndex = 55, Name = "MAT M4")]
        public string MAT_M4 { get; set; }

        [CsvColumn(FieldIndex = 56, Name = "MAT M5")]
        public string MAT_M5 { get; set; }

  }
}
