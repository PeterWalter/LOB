// Decompiled with JetBrains decompiler
// Type: LOB.BDO.ProfileAllocationBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class ProfileAllocationBDO
  {
    public DateTime TestDate { get; set; }

    public int Profile { get; set; }

    public string Client { get; set; }

    public string AQLE { get; set; }

    public string MATE { get; set; }

    public string AQLA { get; set; }

    public string MATA { get; set; }

    public override string ToString()
    {
      return Profile.ToString();
    }
  }
}
