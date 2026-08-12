// Decompiled with JetBrains decompiler
// Type: LOB.Helper.EasypayFile
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;

namespace CETAP_LOB.Helper
{
  public class EasypayFile : ModelBase
  {
    private string thename = "";
    private string _name = "";
    private string _date = "";
    public const string IsSelectedPropertyName = "IsSelected";
    private string _size;
    private bool _isSelected;

    public string FileDetails
    {
      get
      {
        return thename;
      }
      set
      {
        thename = value;
        SplitDetails(thename);
      }
    }

    public string FileName
    {
      get
      {
        return _name;
      }
      set
      {
        _name = value;
      }
    }

    public string Size
    {
      get
      {
        return _size;
      }
      set
      {
        _size = value;
      }
    }

    public string Date
    {
      get
      {
        return _date;
      }
      set
      {
        _date = value;
      }
    }

    public bool IsSelected
    {
      get
      {
        return _isSelected;
      }
      set
      {
        if (_isSelected == value)
          return;
        _isSelected = value;
        RaisePropertyChanged("IsSelected");
      }
    }

    private void SplitDetails(string filedata)
    {
      char[] chArray = new char[2]{ ' ', ' ' };
      string[] strArray = filedata.Split(chArray);
      int length = strArray.Length;
      _name = strArray[length - 1];
      string str1 = strArray[length - 2];
      string str2 = strArray[length - 3];
      string str3 = strArray[length - 4];
      _date = strArray[length - 5];
      EasypayFile easypayFile = this;
      string str4 = easypayFile._date + " " + str3 + " " + str2 + " " + str1;
      easypayFile._date = str4;
      _size = strArray[length - 6];
    }

    public override string ToString()
    {
      return _name + " " + _date + " " + _size;
    }
  }
}
