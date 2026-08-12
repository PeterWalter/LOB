// Decompiled with JetBrains decompiler
// Type: LOB.BDO.ForDuplicatesBarcodesBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.BDO
{
  public class ForDuplicatesBarcodesBDO
  {
    public long Barcode { get; set; }

    public long RefNo { get; set; }

    public string Batch { get; set; }

    public long? SAID { get; set; }

    public string FID { get; set; }

    public string Reason { get; set; }

    public long? RecID { get; set; }

    public DateTime DateModified { get; set; }
  }
}
