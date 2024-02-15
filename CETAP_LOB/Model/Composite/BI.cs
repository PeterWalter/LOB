// Decompiled with JetBrains decompiler
// Type: LOB.Model.Composite.BI
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Helper;
using System;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Model.Composite
{
  public class BI : ModelBase
  {
    public const string errorCountPropertyName = "errorCount";
    public const string NBTPropertyName = "NBT";
    public const string SurnamePropertyName = "Surname";
    public const string NamePropertyName = "Name";
    public const string SAIDPropertyName = "SAID";
    public const string DOBPropertyName = "DOB";
    public const string DOTPropertyName = "DOT";
    private int _errorCount;
    private long _refNo;
    private string _surname;
    private string _name;
    private string _said;
    private DateTime _dob;
    private DateTime _dot;

    public int errorCount
    {
      get
      {
        return _errorCount;
      }
      set
      {
        if (_errorCount == value)
          return;
        _errorCount = value;
        RaisePropertyChanged("errorCount");
      }
    }

    public long NBT
    {
      get
      {
        return _refNo;
      }
      set
      {
        if (_refNo == value)
          return;
        _refNo = value;
        if (!string.IsNullOrEmpty(_refNo.ToString()))
        {
          if (_refNo.ToString().Length != 14)
            AddError("NBT", "Not proper length for NBT number");
          else if (!HelperUtils.IsValidChecksum(_refNo.ToString().Substring(1, 13)))
            AddError("NBT", "Not a Valid NBT number");
          else
            RemoveError("NBT");
        }
        else
          AddError("NBT", "NBT number cannot be empty");
        checkerrors();
        RaisePropertyChanged("NBT");
      }
    }

    public string Surname
    {
      get
      {
        return _surname;
      }
      set
      {
        if (_surname == value)
          return;
        _surname = value;
        if (string.IsNullOrEmpty(_surname))
          AddError("Surname", "Surname cannot be empty");
        else
          RemoveError("Surname");
        if (!string.IsNullOrEmpty(_surname))
        {
          if (Regex.IsMatch(_surname, "\\d"))
            AddError("Surname", "Surname cannot have digits");
          else if (!Regex.IsMatch(_surname, "^[^\\s=!@#](?:[^!@#]*[^\\s!@#])?$"))
            AddError("Surname", "cannot start/end with space or have funny characters");
          else
            RemoveError("Surname");
        }
        if (new Regex("\\s").Matches(_surname).Count > 2)
          AddError("Surname", "Surname has too many spaces");
        else
          RemoveError("Surname");
        checkerrors();
        RaisePropertyChanged("Surname");
      }
    }

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
        if (string.IsNullOrEmpty(_name))
          AddError("Name", "Name cannot be empty");
        else
          RemoveError("Name");
        if (!string.IsNullOrEmpty(_surname))
        {
          if (Regex.IsMatch(_surname, "\\d"))
            AddError("Name", "Name cannot have digits");
          else if (!Regex.IsMatch(_name, "^[^\\s=!@#](?:[^!@#]*[^\\s!@#])?$"))
            AddError("Name", "cannot start/end with space or have funny characters");
          else
            RemoveError("Name");
        }
        if (new Regex("\\s").Matches(_name).Count > 2)
          AddError("Name", "Surname has too many spaces");
        else
          RemoveError("Name");
        checkerrors();
        RaisePropertyChanged("Name");
      }
    }

    public string SAID
    {
      get
      {
        return _said;
      }
      set
      {
        if (_said == value)
          return;
        _said = value;
        _said = _said.Trim();
        if (!string.IsNullOrEmpty(_said))
        {
          if (_said.Length != 13)
            AddError("SAID", "Not proper length for SA ID");
          else if (!Regex.IsMatch(_said, "^[0-9]+$"))
            AddError("SAID", "SA Id does not have characters");
          else if (!HelperUtils.IsValidChecksum(_said))
            AddError("SAID", "Not a Valid South African ID number");
          else
            RemoveError("SAID");
        }
        checkerrors();
        RaisePropertyChanged("SAID");
      }
    }

    public DateTime DOB
    {
      get
      {
        return _dob;
      }
      set
      {
        if (_dob == value)
          return;
        _dob = value;
        TimeSpan timeSpan = DateTime.Now - _dob;
        if (timeSpan.TotalDays < 3650.0 || timeSpan.TotalDays > 29100.0)
          AddError("DOB", "Wrong age for Matric");
        else
          RemoveError("DOB");
        if (!string.IsNullOrWhiteSpace(_said) || !string.IsNullOrEmpty(_said))
        {
          if (HelperUtils.DOBfromSAID(_said) != string.Format("{0:dd/MM/yyyy}", (object) _dob))
            AddError("DOB", "ID and DOB not the same");
          else
            RemoveError("DOB");
        }
        checkerrors();
        RaisePropertyChanged("DOB");
      }
    }

    public DateTime DOT
    {
      get
      {
        return _dot;
      }
      set
      {
        if (_dot == value)
          return;
        _dot = value;
        RaisePropertyChanged("DOT");
      }
    }

    private void checkerrors()
    {
      if (HasErrors)
        errorCount = _errors.Count;
      else
        errorCount = 0;
    }
  }
}
