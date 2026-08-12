// Decompiled with JetBrains decompiler
// Type: LOB.BDO.UserBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;

namespace CETAP_LOB.BDO
{
  public class UserBDO : ModelBase
  {
    public const string StaffIDPropertyName = "StaffID";
    public const string NamePropertyName = "Name";
    public const string AreasPropertyName = "Areas";
    private string _staffno;
    private string _username;
    private string _areas;

    public string StaffID
    {
      get
      {
        return _staffno;
      }
      set
      {
        if (_staffno == value)
          return;
        _staffno = value;
        RaisePropertyChanged("StaffID");
      }
    }

    public string Name
    {
      get
      {
        return _username;
      }
      set
      {
        if (_username == value)
          return;
        _username = value;
        RaisePropertyChanged("Name");
      }
    }

    public string Areas
    {
      get
      {
        return _areas;
      }
      set
      {
        if (_areas == value)
          return;
        _areas = value;
        RaisePropertyChanged("Areas");
      }
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
