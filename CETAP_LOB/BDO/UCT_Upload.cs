// Decompiled with JetBrains decompiler
// Type: LOB.BDO.UCT_Upload
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using LINQtoCSV;

namespace CETAP_LOB.BDO
{
  public class UCT_Upload
  {
    [CsvColumn(FieldIndex = 1, Name = "NBT_Ref_No")]
    public string NBT { get; set; }

    [CsvColumn(FieldIndex = 2, Name = "Surname")]
    public string Surname { get; set; }

    [CsvColumn(FieldIndex = 3, Name = "First_Name")]
    public string Name { get; set; }

    [CsvColumn(FieldIndex = 4, Name = "Initials")]
    public string Initials { get; set; }

    [CsvColumn(FieldIndex = 5, Name = "South_African_ID_No")]
    public string SAID { get; set; }

    [CsvColumn(FieldIndex = 6, Name = "Foreign_Passport")]
    public string ForeignID { get; set; }

    [CsvColumn(FieldIndex = 7, Name = "Date_of_Birth")]
    public string DOB { get; set; }

    [CsvColumn(FieldIndex = 8, Name = "Date_of_Test")]
    public string DOT { get; set; }

    [CsvColumn(FieldIndex = 9, Name = "Venue")]
    public string Venue { get; set; }

    [CsvColumn(FieldIndex = 10, Name = "AL Score")]
    public int? ALScore { get; set; }

    [CsvColumn(FieldIndex = 11, Name = "AL Performance Level")]
    public string ALLevel { get; set; }

    [CsvColumn(FieldIndex = 12, Name = "QL Score")]
    public int? QLScore { get; set; }

    [CsvColumn(FieldIndex = 13, Name = "QL Performance Level")]
    public string QLLevel { get; set; }

    [CsvColumn(FieldIndex = 14, Name = "Maths Score")]
    public int? MATScore { get; set; }

    [CsvColumn(FieldIndex = 15, Name = "Maths Performance Level")]
    public string MATLevel { get; set; }

    [CsvColumn(FieldIndex = 16, Name = "AQL Language")]
    public string AQLLang { get; set; }

    [CsvColumn(FieldIndex = 17, Name = "MAT Language")]
    public string MATLANG { get; set; }
  }
}