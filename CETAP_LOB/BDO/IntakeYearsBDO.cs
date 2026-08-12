// Decompiled with JetBrains decompiler
// Type: LOB.BDO.IntakeYearsBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class IntakeYearsBDO
  {
    public int yearID { get; set; }

    public int Year { get; set; }

    public DateTime yearStart { get; set; }

    public DateTime yearEnd { get; set; }

    public DateTime DateModified { get; set; }

    public override string ToString()
    {
      return Year.ToString();
    }
  }
}
