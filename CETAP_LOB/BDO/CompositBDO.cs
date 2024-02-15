// Decompiled with JetBrains decompiler
// Type: LOB.BDO.CompositBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class CompositBDO
  {
    public long RefNo { get; set; }

    public long Barcode { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public string Initials { get; set; }

    public long? SAID { get; set; }

    public string ForeignID { get; set; }

    public DateTime DOB { get; set; }

    public string ID_Type { get; set; }

    public int? Citizenship { get; set; }

    public string Classification { get; set; }

    public string Gender { get; set; }

    public string Faculty { get; set; }

    public DateTime DOT { get; set; }

    public int VenueCode { get; set; }

    public string VenueName { get; set; }

    public int? HomeLanguage { get; set; }

    public string GR12Language { get; set; }

    public string AQLLanguage { get; set; }

    public int? AQLCode { get; set; }

    public string Batch { get; set; }

    public string MatLanguage { get; set; }

    public int? MatCode { get; set; }

    public string WroteAL { get; set; }

    public string WroteQL { get; set; }

    public string WroteMat { get; set; }

    public int? ALScore { get; set; }

    public string ALLevel { get; set; }

    public int? QLScore { get; set; }

    public string QLLevel { get; set; }

    public int? MATScore { get; set; }

    public string MATLevel { get; set; }

    public string Faculty2 { get; set; }

    public string Faculty3 { get; set; }

    public Guid RowGuid { get; set; }

    public byte[] RowVersion { get; set; }

    public DateTime DateModified { get; set; }
  }
}
