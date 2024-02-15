// Decompiled with JetBrains decompiler
// Type: LOB.BDO.SaveScanFileBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

namespace CETAP_LOB.BDO
{
  public class SaveScanFileBDO
  {
    public int Id { get; set; }

    public string Filename { get; set; }

    public byte[] Scandata { get; set; }
  }
}
