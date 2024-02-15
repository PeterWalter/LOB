

using CETAP_LOB.Helper;
using System;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Model.scoring
{
  public class AnswerSheetBio : ModelBase
  {
    private string _initials = "";
    private int? _mcode = new int?();
    public const string errorCountPropertyName = "errorCount";
    public const string NBTPropertyName = "NBT";
    public const string BarcodePropertyName = "Barcode";
    public const string SurnamePropertyName = "Surname";
    public const string NamePropertyName = "Name";
    public const string InitialsPropertyName = "Initials";
    public const string SAIDPropertyName = "SAID";
    public const string ForeignIDPropertyName = "ForeignID";
    public const string DOBPropertyName = "DOB";
    public const string ID_TypePropertyName = "ID_Type";
    public const string CitizenshipPropertyName = "Citizenship";
    public const string ClassificationPropertyName = "Classification";
    public const string GenderPropertyName = "Gender";
    public const string Faculty1PropertyName = "Faculty1";
    public const string DOTPropertyName = "DOT";
    public const string VenueCodePropertyName = "VenueCode";
    public const string VenueNamePropertyName = "VenueName";
    public const string Grade12_LanguagePropertyName = "Grade12_Language";
    public const string Mat_LanguagePropertyName = "Mat_Language";
    public const string AQL_LanguagePropertyName = "AQL_Language";
    public const string HomeLanguagePropertyName = "HomeLanguage";
    public const string MatCodePropertyName = "MatCode";
    public const string AQLCodePropertyName = "AQLCode";
    public const string Faculty2PropertyName = "Faculty2";
    public const string faculty3PropertyName = "faculty3";
    public const string BatchFilePropertyName = "BatchFile";
    public const string aqltestnamePropertyName = "aqltestname";
    public const string matTestnamePropertyName = "matTestname";
    private int _errorCount;
    private long _refNo;
    private long _barcode;
    private string _surname;
    private string _name;
    private string _said;
    private string _foreignID;
    private DateTime _dob;
    private string _idtype;
    private int? _citizen;
    private string _class;
    private string _gender;
    private string _faculty1;
    private DateTime _dot;
    private int _venuecode;
    private string _venueShortName;
    private string _school_lang;
    private string _mat_lang;
    private string _AQL_Lang;
    private int? _hlanguage;
    private int _acode;
    private string _faculty2;
    private string _faculty3;
    private string _batch;
    private string _aqltestname;
    private string _matTestname;

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

    public long Barcode
    {
      get
      {
        return _barcode;
      }
      set
      {
        if (_barcode == value)
          return;
        _barcode = value;
        if (!string.IsNullOrEmpty(_barcode.ToString()))
        {
          string str = _barcode.ToString();
          if (str.Length != 12)
            AddError("Barcode", "Barcode length is wrong");
          else
            RemoveError("Barcode");
          if (!HelperUtils.IsValidChecksum(str))
            AddError("Barcode", "Not a Valid Barcode number");
          else
            RemoveError("Barcode");
        }
        RaisePropertyChanged("Barcode");
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

    public string Initials
    {
      get
      {
        return _initials;
      }
      set
      {
        if (_initials == value)
          return;
        _initials = value;
        RaisePropertyChanged("Initials");
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

    public string ForeignID
    {
      get
      {
        return _foreignID;
      }
      set
      {
        if (_foreignID == value)
          return;
        _foreignID = value;
        RaisePropertyChanged("ForeignID");
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

    public string ID_Type
    {
      get
      {
        return _idtype;
      }
      set
      {
        if (_idtype == value)
          return;
        _idtype = value;
        checkerrors();
        RaisePropertyChanged("ID_Type");
      }
    }

    public int? Citizenship
    {
      get
      {
        return _citizen;
      }
      set
      {
        int? citizen = _citizen;
        int? nullable = value;
        if ((citizen.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (citizen.HasValue == nullable.HasValue ? 1 : 0)) != 0)
          return;
        _citizen = value;
        RaisePropertyChanged("Citizenship");
      }
    }

    public string Classification
    {
      get
      {
        return _class;
      }
      set
      {
        if (_class == value)
          return;
        _class = value;
        RaisePropertyChanged("Classification");
      }
    }

    public string Gender
    {
      get
      {
        return _gender;
      }
      set
      {
        if (_gender == value)
          return;
        _gender = value;
        RaisePropertyChanged("Gender");
      }
    }

    public string Faculty1
    {
      get
      {
        return _faculty1;
      }
      set
      {
        if (_faculty1 == value)
          return;
        _faculty1 = value;
        RaisePropertyChanged("Faculty1");
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

    public int VenueCode
    {
      get
      {
        return _venuecode;
      }
      set
      {
        if (_venuecode == value)
          return;
        _venuecode = value;
        RaisePropertyChanged("VenueCode");
      }
    }

    public string VenueName
    {
      get
      {
        return _venueShortName;
      }
      set
      {
        if (_venueShortName == value)
          return;
        _venueShortName = value;
        RaisePropertyChanged("VenueName");
      }
    }

    public string Grade12_Language
    {
      get
      {
        return _school_lang;
      }
      set
      {
        if (_school_lang == value)
          return;
        _school_lang = value;
        RaisePropertyChanged("Grade12_Language");
      }
    }

    public string Mat_Language
    {
      get
      {
        return _mat_lang;
      }
      set
      {
        if (_mat_lang == value)
          return;
        _mat_lang = value;
        RaisePropertyChanged("Mat_Language");
      }
    }

    public string AQL_Language
    {
      get
      {
        return _AQL_Lang;
      }
      set
      {
        if (_AQL_Lang == value)
          return;
        _AQL_Lang = value;
        RaisePropertyChanged("AQL_Language");
      }
    }

    public int? HomeLanguage
    {
      get
      {
        return _hlanguage;
      }
      set
      {
        int? hlanguage = _hlanguage;
        int? nullable = value;
        if ((hlanguage.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (hlanguage.HasValue == nullable.HasValue ? 1 : 0)) != 0)
          return;
        _hlanguage = value;
        RaisePropertyChanged("HomeLanguage");
      }
    }

    public int? MatCode
    {
      get
      {
        return _mcode;
      }
      set
      {
        int? mcode = _mcode;
        int? nullable = value;
        if ((mcode.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (mcode.HasValue == nullable.HasValue ? 1 : 0)) != 0)
          return;
        _mcode = value;
        RaisePropertyChanged("MatCode");
      }
    }

    public int AQLCode
    {
      get
      {
        return _acode;
      }
      set
      {
        if (_acode == value)
          return;
        _acode = value;
        RaisePropertyChanged("AQLCode");
      }
    }

    public string Faculty2
    {
      get
      {
        return _faculty2;
      }
      set
      {
        if (_faculty2 == value)
          return;
        _faculty2 = value;
        RaisePropertyChanged("Faculty2");
      }
    }

    public string faculty3
    {
      get
      {
        return _faculty3;
      }
      set
      {
        if (_faculty3 == value)
          return;
        _faculty3 = value;
        RaisePropertyChanged("faculty3");
      }
    }

    public string BatchFile
    {
      get
      {
        return _batch;
      }
      set
      {
        if (_batch == value)
          return;
        _batch = value;
        RaisePropertyChanged("BatchFile");
      }
    }

    public string aqltestname
    {
      get
      {
        return _aqltestname;
      }
      set
      {
        if (_aqltestname == value)
          return;
        _aqltestname = value;
        RaisePropertyChanged("aqltestname");
      }
    }

    public string matTestname
    {
      get
      {
        return _matTestname;
      }
      set
      {
        if (_matTestname == value)
          return;
        _matTestname = value;
        RaisePropertyChanged("matTestname");
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
