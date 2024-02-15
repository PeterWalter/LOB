

using CETAP_LOB.Helper;
using System;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Model.venueprep
{
  public class WebWriters : ModelBase
  {
    private string _surname = "";
    private string _myname = "";
    private string _initials = "";
    private string _said = "";
    private string _foreignID = "";
    public const string errorCountPropertyName = "errorCount";
    public const string ReferencePropertyName = "Reference";
    public const string SurnamePropertyName = "Surname";
    public const string FirstNamePropertyName = "FirstName";
    public const string initialsPropertyName = "initials";
    public const string SAIDPropertyName = "SAID";
    public const string ForeignIDPropertyName = "ForeignID";
    public const string DOBPropertyName = "DOB";
    public const string GenderPropertyName = "Gender";
    public const string ClassificationPropertyName = "Classification";
    public const string TestsPropertyName = "Tests";
    public const string LanguagePropertyName = "Language";
    public const string VenuePropertyName = "Venue";
    public const string DOTPropertyName = "DOT";
    public const string MobilePropertyName = "Mobile";
    public const string HTelephonePropertyName = "HTelephone";
    public const string EmailPropertyName = "Email";
    public const string RegDatePropertyName = "RegDate";
    public const string PaidPropertyName = "Paid";
    public const string CreationDatePropertyName = "CreationDate";
    public const string IsSelectedPropertyName = "IsSelected";
    private int _mycount;
    private string _NBT;
    private DateTime _dob;
    private string _gender;
    private string _classification;
    private string _tests;
    private string _language;
    private string _venue;
    private DateTime _dot;
    private string _mobile;
    private string _telephone;
    private string _email;
    private DateTime _regdate;
    private double _payment;
    private DateTime _CreateDate;
    private bool _isSelected;

    public int errorCount
    {
      get
      {
        return _mycount;
      }
      set
      {
        if (_mycount == value)
          return;
        _mycount = value;
        RaisePropertyChanged("errorCount");
      }
    }

    public string Reference
    {
      get
      {
        return _NBT;
      }
      set
      {
        if (_NBT == value)
          return;
        _NBT = value;
        if (!string.IsNullOrEmpty(_NBT))
        {
          if (_NBT.Length != 14)
            AddError("Reference", "Not proper length for NBT number");
          else if (!HelperUtils.IsValidChecksum(_NBT.Substring(1, 13)))
            AddError("Reference", "Not a Valid NBT number");
          else
            RemoveError("Reference");
        }
        else
          AddError("Reference", "NBT number cannot be empty");
        checkerrors();
        RaisePropertyChanged("Reference");
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
          else if (_surname.Length > 20)
            AddError("Surname", "Too many characters for Surname");
          else
            RemoveError("Surname");
        }
        checkerrors();
        RaisePropertyChanged("Surname");
      }
    }

    public string FirstName
    {
      get
      {
        return _myname;
      }
      set
      {
        if (_myname == value)
          return;
        _myname = value;
        if (string.IsNullOrEmpty(_myname))
          AddError("FirstName", "FirstName cannot be empty");
        else
          RemoveError("FirstName");
        if (!string.IsNullOrWhiteSpace(_myname))
        {
          if (Regex.IsMatch(_myname, "\\d"))
            AddError("FirstName", "First name cannot have digits");
          else if (!Regex.IsMatch(_myname, "^[^\\s=!@#](?:[^!@#]*[^\\s!@#])?$"))
            AddError("FirstName", "cannot start/end with space or have funny characters");
          else if (_myname.Length > 18)
            AddError("FirstName", "To many characters for Name (max is 18)");
          else
            RemoveError("FirstName");
        }
        checkerrors();
        RaisePropertyChanged("FirstName");
      }
    }

    public string initials
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
        RaisePropertyChanged("initials");
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
        if (!string.IsNullOrEmpty(_said))
        {
          if (!Regex.IsMatch(_said, "^[0-9]+$"))
            AddError("SAID", "SA Id does not have characters");
          else if (!HelperUtils.IsValidChecksum(_said))
            AddError("SAID", "Not a Valid South African ID number");
          else
            RemoveError("SAID");
        }
        else
          RemoveError("SAID");
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
        if (!string.IsNullOrEmpty(_foreignID))
        {
          if (_foreignID.Length > 15)
            AddError("ForeignID", "ForeignID has too many characters");
          else
            RemoveError("ForeignID");
        }
        checkerrors();
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
        checkerrors();
        RaisePropertyChanged("DOB");
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

    public string Classification
    {
      get
      {
        return _classification;
      }
      set
      {
        if (_classification == value)
          return;
        _classification = value;
        RaisePropertyChanged("Classification");
      }
    }

    public string Tests
    {
      get
      {
        return _tests;
      }
      set
      {
        if (_tests == value)
          return;
        _tests = value;
        RaisePropertyChanged("Tests");
      }
    }

    public string Language
    {
      get
      {
        return _language;
      }
      set
      {
        if (_language == value)
          return;
        _language = value;
        RaisePropertyChanged("Language");
      }
    }

    public string Venue
    {
      get
      {
        return _venue;
      }
      set
      {
        if (_venue == value)
          return;
        _venue = value;
        RaisePropertyChanged("Venue");
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

    public string Mobile
    {
      get
      {
        return _mobile;
      }
      set
      {
        if (_mobile == value)
          return;
        _mobile = value;
        if (!string.IsNullOrEmpty(_mobile))
        {
          if (_mobile.Length > 15)
            AddError("Mobile", "Cellphone number has too many characters");
          else
            RemoveError("Mobile");
        }
        checkerrors();
        RaisePropertyChanged("Mobile");
      }
    }

    public string HTelephone
    {
      get
      {
        return _telephone;
      }
      set
      {
        if (_telephone == value)
          return;
        _telephone = value;
        if (!string.IsNullOrEmpty(_telephone))
        {
          if (_telephone.Length > 15)
            AddError("HTelephone", "Telephone number has too many characters");
          else
            RemoveError("HTelephone");
        }
        checkerrors();
        RaisePropertyChanged("HTelephone");
      }
    }

    public string Email
    {
      get
      {
        return _email;
      }
      set
      {
        if (_email == value)
          return;
        _email = value;
        if (!string.IsNullOrEmpty(_email))
        {
          if (!HelperUtils.IsValidEmail(_email))
            AddError("Email", "Wrong email address");
          else if (_email.Length > 50)
            AddError("Email", "Email is to long");
          else
            RemoveError("Email");
        }
        else
          RemoveError("Email");
        checkerrors();
        RaisePropertyChanged("Email");
      }
    }

    public DateTime RegDate
    {
      get
      {
        return _regdate;
      }
      set
      {
        if (_regdate == value)
          return;
        _regdate = value;
        RaisePropertyChanged("RegDate");
      }
    }

    public double Paid
    {
      get
      {
        return _payment;
      }
      set
      {
        if (_payment == value)
          return;
        _payment = value;
        RaisePropertyChanged("Paid");
      }
    }

    public DateTime CreationDate
    {
      get
      {
        return _CreateDate;
      }
      set
      {
        if (_CreateDate == value)
          return;
        _CreateDate = value;
        RaisePropertyChanged("CreationDate");
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

    private void checkerrors()
    {
      if (HasErrors)
        errorCount = _errors.Count;
      else
        errorCount = 0;
    }
  }
}
