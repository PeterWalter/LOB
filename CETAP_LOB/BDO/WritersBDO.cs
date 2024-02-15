// Decompiled with JetBrains decompiler
// Type: LOB.BDO.WritersBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class WritersBDO
  {
    public int Id { get; set; }

    public long NBT { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public string Initials { get; set; }

    public long? SAID { get; set; }

    public string ForeignID { get; set; }

    public string Gender { get; set; }

    public DateTime DOB { get; set; }

    public string Classification { get; set; }

    public string TestLanguage { get; set; }

    public string TestType { get; set; }

    public int VenueID { get; set; }

    public DateTime DOT { get; set; }

    public string Mobile { get; set; }

    public string HomeTelephone { get; set; }

    public string EMail { get; set; }

    public Guid RowGuid { get; set; }

    public DateTime DateModified { get; set; }

    public byte[] RowVersion { get; set; }

    public DateTime AccountCreation { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool Wrote { get; set; }

    public Decimal Paid { get; set; }
  }
}
