// Decompiled with JetBrains decompiler
// Type: LOB.Model.scoring.ScoreDifference
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

namespace CETAP_LOB.Model.scoring
{
  public class ScoreDifference
  {
    public long Barcode { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public int? ALScore { get; set; }

    public int? QLScore { get; set; }

    public int? MATScore { get; set; }

    public int? M_ALScore { get; set; }

    public int? M_QLScore { get; set; }

    public int? M_MATScore { get; set; }

    public int? Diff_ALScore { get; set; }

    public int? Diff_QLScore { get; set; }

    public int? Diff_MATScore { get; set; }

    public string Accepted { get; set; }

    public string Batch { get; set; }

    public string M_Batch { get; set; }
  }
}
