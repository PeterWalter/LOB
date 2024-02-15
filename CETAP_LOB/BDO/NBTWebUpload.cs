// Decompiled with JetBrains decompiler
// Type: LOB.BDO.NBTWebUpload
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using LINQtoCSV;

namespace CETAP_LOB.BDO
{
  public class NBTWebUpload
  {
    [CsvColumn(FieldIndex = 1, Name = "Barcode")]
    public string Barcode { get; set; }

    [CsvColumn(FieldIndex = 2, Name = "Test Session ID")]
    public string SessionID { get; set; }

    [CsvColumn(FieldIndex = 3, Name = "NBT Reference")]
    public string NBT { get; set; }

    [CsvColumn(FieldIndex = 4, Name = "Surname")]
    public string Surname { get; set; }

    [CsvColumn(FieldIndex = 5, Name = "First Name")]
    public string Name { get; set; }

    [CsvColumn(FieldIndex = 6, Name = "Middle Initials")]
    public string Initials { get; set; }

    [CsvColumn(FieldIndex = 7, Name = "South African ID")]
    public string SAID { get; set; }

    [CsvColumn(FieldIndex = 8, Name = "Foreign ID")]
    public string ForeignID { get; set; }

    [CsvColumn(FieldIndex = 9, Name = "Date of Birth")]
    public string DOB { get; set; }

    [CsvColumn(FieldIndex = 10, Name = "Wrote AL")]
    public string Wrote_AL { get; set; }

    [CsvColumn(FieldIndex = 11, Name = "Wrote QL")]
    public string Wrote_QL { get; set; }

    [CsvColumn(FieldIndex = 12, Name = "Wrote Maths")]
    public string Wrote_Mat { get; set; }

    [CsvColumn(FieldIndex = 13, Name = "Student Number")]
    public string StudentID { get; set; }

    [CsvColumn(FieldIndex = 14, Name = "Faculty")]
    public string Faculty { get; set; }

    [CsvColumn(FieldIndex = 15, Name = "Programme")]
    public string Programme { get; set; }

    [CsvColumn(FieldIndex = 16, Name = "Date_of_Test")]
    public string DOT { get; set; }

    [CsvColumn(FieldIndex = 17, Name = "Venue")]
    public string Venue { get; set; }

    [CsvColumn(FieldIndex = 18, Name = "Gender")]
    public string Gender { get; set; }

    [CsvColumn(FieldIndex = 19, Name = "Street and Number")]
    public string StreetNo { get; set; }

    [CsvColumn(FieldIndex = 20, Name = "Street Name")]
    public string StreetName { get; set; }

    [CsvColumn(FieldIndex = 21, Name = "Suburb")]
    public string Suburb { get; set; }

    [CsvColumn(FieldIndex = 22, Name = "City/Town")]
    public string City { get; set; }

    [CsvColumn(FieldIndex = 23, Name = "Province/Region")]
    public string Province { get; set; }

    [CsvColumn(FieldIndex = 24, Name = "Postal Code")]
    public string PostCode { get; set; }

    [CsvColumn(FieldIndex = 25, Name = "e-mail Address")]
    public string EMail { get; set; }

    [CsvColumn(FieldIndex = 26, Name = "Landline Number")]
    public string Telephone { get; set; }

    [CsvColumn(FieldIndex = 27, Name = "Mobile Number")]
    public string Celephone { get; set; }

    [CsvColumn(FieldIndex = 28, Name = "AL Score")]
    public int? ALScore { get; set; }

    [CsvColumn(FieldIndex = 29, Name = "AL Performance")]
    public string ALLevel { get; set; }

    [CsvColumn(FieldIndex = 30, Name = "QL Score")]
    public int? QLScore { get; set; }

    [CsvColumn(FieldIndex = 31, Name = "QL Performance")]
    public string QLLevel { get; set; }

    [CsvColumn(FieldIndex = 32, Name = "Maths Score")]
    public int? MATScore { get; set; }

    [CsvColumn(FieldIndex = 33, Name = "Maths Performance")]
    public string MATLevel { get; set; }

    [CsvColumn(FieldIndex = 34, Name = "AQL TEST LANGUAGE")]
    public string AQLLang { get; set; }

    [CsvColumn(FieldIndex = 35, Name = "Maths TEST LANGUAGE")]
    public string MATLANG { get; set; }
  }
}
