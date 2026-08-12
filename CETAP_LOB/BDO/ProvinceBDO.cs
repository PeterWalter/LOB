// Decompiled with JetBrains decompiler
// Type: LOB.BDO.ProvinceBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;

namespace CETAP_LOB.BDO
{
  public class ProvinceBDO : ModelBase
  {
    public const string IdPropertyName = "Id";
    public const string NamePropertyName = "Name";
    private int _myID;
    private string _name;

    public int Id
    {
      get
      {
        return _myID;
      }
      set
      {
        if (_myID == value)
          return;
        _myID = value;
        RaisePropertyChanged("Id");
      }
    }

    public byte[] RowVersion { get; set; }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        if (_name == value)
          return;
        _name = value;
        RaisePropertyChanged("Name");
      }
    }

    public int Code { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}
