// Decompiled with JetBrains decompiler
// Type: LOB.Model.venueprep.WebWriters1
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using System;

namespace CETAP_LOB.Model.venueprep
{
  public class WebWriters1 : ObservableObject
  {
    private string _surname = "";
    private string _myname = "";
    private string _initials = "";
    private string _said = "";
    private string _foreignID = "";
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
    public const string LastPropertyName = "Last";
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
    private string _mylast;

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

    public string Last
    {
      get
      {
        return _mylast;
      }
      set
      {
        if (_mylast == value)
          return;
        _mylast = value;
        RaisePropertyChanged("Last");
      }
    }
  }
}
