// Decompiled with JetBrains decompiler
// Type: LOB.Model.QA.ASC761
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FileHelpers;

namespace CETAP_LOB.Model.QA
{
  [IgnoreLast(1)]
  [FixedLengthRecord(FixedMode.ExactLength)]
  public sealed class ASC761
  {
    [FieldFixedLength(3)]
    public string CSX_Number;
    [FieldFixedLength(37)]
    public string CSX;
    [FieldFixedLength(14)]
    public string NBT;
    [FieldFixedLength(12)]
    public string SessionID;
    [FieldFixedLength(13)]
    public string SAID;
    [FieldFixedLength(15)]
    public string ForeignID;
    [FieldFixedLength(1)]
    public string IDType;
    [FieldFixedLength(1)]
    public string Gender;
    [FieldFixedLength(1)]
    public string Citizenship;
    [FieldFixedLength(1)]
    public string Faculty;
    [FieldFixedLength(8)]
    public string DOB;
    [FieldFixedLength(20)]
    public string Surname;
    [FieldFixedLength(18)]
    public string Name;
    [FieldFixedLength(3)]
    public string Initials;
    [FieldFixedLength(5)]
    public string VenueCode;
    [FieldFixedLength(8)]
    public string DOT;
    [FieldFixedLength(2)]
    public string HomeLanguage;
    [FieldFixedLength(1)]
    public string SchoolLanguage;
    [FieldFixedLength(1)]
    public string Classification;
    [FieldFixedLength(1)]
    public string AQL_Language;
    [FieldFixedLength(3)]
    public string AQL_Code;
    [FieldFixedLength(20)]
    public string AQL_Section1;
    [FieldFixedLength(20)]
    public string AQL_Section2;
    [FieldFixedLength(25)]
    public string AQL_Section3;
    [FieldFixedLength(25)]
    public string AQL_section4;
    [FieldFixedLength(20)]
    public string AQL_Section5;
    [FieldFixedLength(25)]
    public string AQL_Section6;
    [FieldFixedLength(25)]
    public string AQL_Section7;
    [FieldFixedLength(1)]
    public string Maths_Language;
    [FieldFixedLength(3)]
    public string Maths_Code;
    [FieldFixedLength(60)]
    public string Maths_Answers;
    [FieldFixedLength(1)]
    public string Faculty1;
    [FieldFixedLength(1)]
    public string Faculty2;
    [FieldFixedLength(1)]
    public string Faculty3;
    [FieldFixedLength(1)]
    public string EndofLine;
  }
}
