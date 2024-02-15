// Decompiled with JetBrains decompiler
// Type: LOB.Model.venueprep.WebWriterMap
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace CETAP_LOB.Model.venueprep
{
  public sealed class WebWriterMap : CsvClassMap<WebWriters>
  {
    public override void CreateMap()
    {
      Map(m => m.Reference).Index(0);
      Map(m => m.Surname).Index(1);
      Map(m => m.FirstName).Name("First Name");
      Map(m => m.initials).Index(3);
      Map(m => m.SAID).Index(4);
      Map(m => m.ForeignID).Index(5);
      Map(m => m.DOB).Index(6).TypeConverterOption(DateTimeStyles.AdjustToUniversal);
      Map(m => m.Gender).Index(7);
      Map(m => m.Classification).Index(8);
      Map(m => m.Tests).Index(9);
      Map(m => m.Language).Index(10);
      Map(m => m.Venue).Index(11);
      Map(m => m.DOT).Index(12).TypeConverterOption(DateTimeStyles.AdjustToUniversal);
      Map(m => m.Mobile).Index(13);
      Map(m => m.HTelephone).Index(14);
      Map(m => m.Email).Index(15);
      Map(m =>  m.RegDate).Index(16);
      Map(m =>  m.Paid).Index(17);
      Map(m =>  m.CreationDate).Index(18);
    }
  }
}
