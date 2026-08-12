// Decompiled with JetBrains decompiler
// Type: LOB.BDO.ScannedFileBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;

using System;

namespace CETAP_LOB.BDO
{
  public class ScannedFileBDO : ModelBase
  {
    public const string IsSelectedPropertyName = "IsSelected";
    private bool _selected;

    public int FileID { get; set; }

    public string Filename { get; set; }

    public DateTime DateScanned { get; set; }

    public byte[] FileData { get; set; }

    public DateTime DateModified { get; set; }

    public string Filepath { get; set; }

    public int? BatchID { get; set; }

    public bool IsSelected
    {
      get
      {
        return _selected;
      }
      set
      {
        if (_selected == value)
          return;
        _selected = value;
        RaisePropertyChanged("IsSelected");
      }
    }

    public override string ToString()
    {
      return Filename;
    }
  }
}
