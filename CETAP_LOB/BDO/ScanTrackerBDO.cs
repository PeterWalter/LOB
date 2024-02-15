// Decompiled with JetBrains decompiler
// Type: LOB.BDO.ScanTrackerBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class ScanTrackerBDO
  {
    public int Id { get; set; }

    public string FileName { get; set; }

    public string BatchedBy { get; set; }

    public DateTime? DateBatched { get; set; }

    public int? Records { get; set; }

    public DateTime? DateScanned { get; set; }

    public int? ScanRecords { get; set; }

    public DateTime? DateEdited { get; set; }

    public int? EditRecords { get; set; }

    public DateTime? DateQA { get; set; }

    public int? QARecords { get; set; }

    public DateTime? DateSentForScoring { get; set; }

    public int? SentCount { get; set; }

    public DateTime? DateScored { get; set; }

    public int? ScoredRecords { get; set; }

    public DateTime? TestDate { get; set; }

    public DateTime DateModified { get; set; }

    public string Description { get; set; }

    public byte[] RowVersion { get; set; }

    public override string ToString()
    {
      return FileName;
    }
  }
}
