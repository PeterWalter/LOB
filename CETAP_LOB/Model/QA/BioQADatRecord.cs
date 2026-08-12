// Decompiled with JetBrains decompiler
// Type: LOB.Model.QA.BioQADatRecord
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Helper;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Model.QA
{
  public class BioQADatRecord : ModelBase
  {
    private string _said = "";
    private string _foreignID = "";
    public const string mCSX_NumberPropertyName = "mCSX_Number";
    public const string CSXPropertyName = "CSX";
    public const string NBTPropertyName = "NBT";
    public const string BarcodePropertyName = "Barcode";
    public const string SAIDPropertyName = "SAID";
    public const string ForeignIDPropertyName = "ForeignID";
    public const string GenderPropertyName = "Gender";
    public const string CitizenshipPropertyName = "Citizenship";
    public const string HomeLanguagePropertyName = "HomeLanguage";
    public const string ClassificationPropertyName = "Classification";
    public const string PhonePropertyName = "Phone";
    public const string HomeProvincePropertyName = "HomeProvince";
    public const string DOTPropertyName = "DOT";
    public const string Grade12LanguagePropertyName = "Grade12Language";
    public const string SchoolTypePropertyName = "SchoolType";
    public const string Faculty1PropertyName = "Faculty1";
    public const string Faculty2PropertyName = "Faculty2";
    public const string Faculty3PropertyName = "Faculty3";
    public const string DatFilePropertyName = "DatFile";
    public const string ErrorCountPropertyName = "ErrorCount";
    public const string IsSelectedPropertyName = "IsSelected";
    private string _myCSXNum;
    private string _csx;
    private string _nbt;
    private string _barcode;
    private string _gender;
    private string _myCitizenship;
    private string _homeLanguage;
    private string _classi;
    private string _phone;
    private string _mProvince;
    private DateTime _dot;
    private string mHomeType;
    private string mHelectricity;
    private string mSchool10Km;
    private string mHomestudy;
    private string mSiblingsAttendedUni;
    private string mSiblingsAtUni;
    private string mVenueTime;
    private string mVenueCost;
    private string mVenueDist;
    private string mVenueModeOfTransport;
    private string mSchComputers;
    private string mSchLibrary;
    private string mSchLabs;
    private string mSchElectricity;
    private string mSchWater;
    private string mSchHall;
    private string mSchFields;
    private string mSchHostel;
    private string mUsedComputers;
    private string mUsedLibrary;
    private string mUsedLabs;
    private string mUsedElectricity;
    private string mUsedRWater;
    private string mUsedHall;
    private string mUsedFields;
    private string mUsedHostel;
    private string mSchoolName;
    private string mYrGr12;
    private string mPostalCode;
    private string mSchProvince;
    private int _myGr12L;
    private int schoolType;
    private string _myfaculty1;
    private string _myfaculty2;
    private string _myFaculty3;
    private string mEOL;
    private datFileAttributes _mydatFile;
    private int _myerrorcount;
    private bool _isSelected;

    public string mCSX_Number
    {
      get
      {
        return _myCSXNum;
      }
      set
      {
        if (_myCSXNum == value)
          return;
        _myCSXNum = value;
        RaisePropertyChanged("mCSX_Number");
      }
    }

    public string CSX
    {
      get
      {
        return _csx;
      }
      set
      {
        if (_csx == value)
          return;
        _csx = value;
        RaisePropertyChanged("CSX");
      }
    }

    public string NBT
    {
      get
      {
        return _nbt;
      }
      set
      {
        if (_nbt == value)
          return;
        _nbt = value;
        string str = _nbt.Substring(1, 13);
        if (!string.IsNullOrEmpty(_nbt))
        {
          if (_nbt.Length != 14)
            AddError("NBT", "Not proper length for NBT number");
          else if (!HelperUtils.IsValidChecksum(str))
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

    public string Barcode
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
        _barcode = _barcode.Trim();
        string barcode = _barcode;
        if (!string.IsNullOrEmpty(_barcode))
        {
          if (_barcode.Length != 12)
            AddError("Barcode", "Not proper length for Session ID number");
          else if (!HelperUtils.IsValidChecksum(barcode))
            AddError("Barcode", "Not a Valid Barcode number");
          else
            RemoveError("Barcode");
        }
        else
          AddError("Barcode", "Barcode number cannot be empty");
        checkerrors();
        RaisePropertyChanged("Barcode");
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
        MatchCollection matchCollection = new Regex("\\s").Matches(_said);
        if (!HelperUtils.IsValidChecksum(_said))
          AddError("SAID", "Not a Valid South African ID number");
        else
          RemoveError("SAID");
        if (matchCollection.Count > 0)
          AddError("SAID", "South African ID number cannot have spaces");
        else if (!Regex.IsMatch(_said, "[0-9]"))
          AddError("SAID", "SA Id does not have characters");
        if (!HelperUtils.IsNumeric(_said))
          AddError("SAID", "SA ID cannot have characters");
        else if (!string.IsNullOrWhiteSpace(_said))
          RemoveError("ForeignID");
        else
          RemoveError("SAID");
        checkerrors();
        RaisePropertyChanged("SAID");
        RaisePropertyChanged("ForeignID");
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
        if (string.IsNullOrWhiteSpace(_said) && string.IsNullOrWhiteSpace(_foreignID))
        {
          AddError("SAID", "SA Id missing");
          AddError("ForeignID", "Foreign Id missing");
        }
        else if (string.IsNullOrWhiteSpace(_said) && !string.IsNullOrWhiteSpace(_foreignID))
        {
          RemoveError("SAID");
          RemoveError("ForeignID");
        }
        else if (!string.IsNullOrWhiteSpace(_said))
        {
          RemoveError("ForeignID");
        }
        else
        {
          RemoveError("ForeignID");
          RemoveError("SAID");
        }
        checkerrors();
        RaisePropertyChanged("ForeignID");
        RaisePropertyChanged("SAID");
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
        if (!string.IsNullOrWhiteSpace(_gender))
        {
          if (_said.Length == 13)
          {
            int int32 = Convert.ToInt32(_said.Substring(6, 1));
            if (int32 < 5 && _gender != "F")
              AddError("Gender", "Gender should be F");
            else if (int32 > 4 && _gender != "M")
              AddError("Gender", "Gender should be M");
          }
          else
            RemoveError("Gender");
        }
        checkerrors();
        RaisePropertyChanged("Gender");
      }
    }

    public string Citizenship
    {
      get
      {
        return _myCitizenship;
      }
      set
      {
        if (_myCitizenship == value)
          return;
        _myCitizenship = value;
        RaisePropertyChanged("Citizenship");
      }
    }

    public string HomeLanguage
    {
      get
      {
        return _homeLanguage;
      }
      set
      {
        if (_homeLanguage == value)
          return;
        _homeLanguage = value;
        RaisePropertyChanged("HomeLanguage");
      }
    }

    public string Classification
    {
      get
      {
        return _classi;
      }
      set
      {
        if (_classi == value)
          return;
        _classi = value;
        bool flag = HelperUtils.IsNumeric(_classi);
        if (string.IsNullOrWhiteSpace(_classi))
          AddError("Classification", "There should be a value");
        else if (flag)
        {
          if (Convert.ToInt32(_classi) > 5)
            AddError("Classification", "Value should be less than 5");
        }
        else
          RemoveError("Classification");
        checkerrors();
        RaisePropertyChanged("Classification");
      }
    }

    public string Phone
    {
      get
      {
        return _phone;
      }
      set
      {
        if (_phone == value)
          return;
        _phone = value;
        RaisePropertyChanged("Phone");
      }
    }

    public string HomeProvince
    {
      get
      {
        return _mProvince;
      }
      set
      {
        if (_mProvince == value)
          return;
        _mProvince = value;
        if (!string.IsNullOrWhiteSpace(_mProvince))
        {
          if (Convert.ToInt32(_mProvince) > 10)
            AddError("HomeProvince", "No such Province exists");
          else
            RemoveError("HomeProvince");
        }
        checkerrors();
        RaisePropertyChanged("HomeProvince");
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
        checkerrors();
        RaisePropertyChanged("DOT");
      }
    }

    public string HomeType
    {
      get
      {
        return mHomeType;
      }
      set
      {
        mHomeType = value;
      }
    }

    public string Helectricity
    {
      get
      {
        return mHelectricity;
      }
      set
      {
        mHelectricity = value;
      }
    }

    public string School10Km
    {
      get
      {
        return mSchool10Km;
      }
      set
      {
        mSchool10Km = value;
      }
    }

    public string Homestudy
    {
      get
      {
        return mHomestudy;
      }
      set
      {
        mHomestudy = value;
      }
    }

    public string SiblingsAttendedUni
    {
      get
      {
        return mSiblingsAttendedUni;
      }
      set
      {
        mSiblingsAttendedUni = value;
      }
    }

    public string SiblingsAtUni
    {
      get
      {
        return mSiblingsAtUni;
      }
      set
      {
        mSiblingsAtUni = value;
      }
    }

    public string VenueTime
    {
      get
      {
        return mVenueTime;
      }
      set
      {
        mVenueTime = value;
      }
    }

    public string VenueCost
    {
      get
      {
        return mVenueCost;
      }
      set
      {
        mVenueCost = value;
      }
    }

    public string VenueDist
    {
      get
      {
        return mVenueDist;
      }
      set
      {
        mVenueDist = value;
      }
    }

    public string VenueModeOfTransport
    {
      get
      {
        return mVenueModeOfTransport;
      }
      set
      {
        mVenueModeOfTransport = value;
      }
    }

    public string SchComputers
    {
      get
      {
        return mSchComputers;
      }
      set
      {
        mSchComputers = value;
      }
    }

    public string SchLibrary
    {
      get
      {
        return mSchLibrary;
      }
      set
      {
        mSchLibrary = value;
      }
    }

    public string SchLabs
    {
      get
      {
        return mSchLabs;
      }
      set
      {
        mSchLabs = value;
      }
    }

    public string SchElectricity
    {
      get
      {
        return mSchElectricity;
      }
      set
      {
        mSchElectricity = value;
      }
    }

    public string SchWater
    {
      get
      {
        return mSchWater;
      }
      set
      {
        mSchWater = value;
      }
    }

    public string SchHall
    {
      get
      {
        return mSchHall;
      }
      set
      {
        mSchHall = value;
      }
    }

    public string SchFields
    {
      get
      {
        return mSchFields;
      }
      set
      {
        mSchFields = value;
      }
    }

    public string SchHostel
    {
      get
      {
        return mSchHostel;
      }
      set
      {
        mSchHostel = value;
      }
    }

    public string UsedComputers
    {
      get
      {
        return mUsedComputers;
      }
      set
      {
        mUsedComputers = value;
      }
    }

    public string UsedLibrary
    {
      get
      {
        return mUsedLibrary;
      }
      set
      {
        mUsedLibrary = value;
      }
    }

    public string UsedLabs
    {
      get
      {
        return mUsedLabs;
      }
      set
      {
        mUsedLabs = value;
      }
    }

    public string UsedElectricity
    {
      get
      {
        return mUsedElectricity;
      }
      set
      {
        mUsedElectricity = value;
      }
    }

    public string UsedRWater
    {
      get
      {
        return mUsedRWater;
      }
      set
      {
        mUsedRWater = value;
      }
    }

    public string UsedHall
    {
      get
      {
        return mUsedHall;
      }
      set
      {
        mUsedHall = value;
      }
    }

    public string UsedFields
    {
      get
      {
        return mUsedFields;
      }
      set
      {
        mUsedFields = value;
      }
    }

    public string UsedHostel
    {
      get
      {
        return mUsedHostel;
      }
      set
      {
        mUsedHostel = value;
      }
    }

    public string SchoolName
    {
      get
      {
        return mSchoolName;
      }
      set
      {
        mSchoolName = value;
      }
    }

    public string YrGr12
    {
      get
      {
        return mYrGr12;
      }
      set
      {
        mYrGr12 = value;
      }
    }

    public string PostalCode
    {
      get
      {
        return mPostalCode;
      }
      set
      {
        mPostalCode = value;
      }
    }

    public string SchProvince
    {
      get
      {
        return mSchProvince;
      }
      set
      {
        mSchProvince = value;
      }
    }

    public int Grade12Language
    {
      get
      {
        return _myGr12L;
      }
      set
      {
        if (_myGr12L == value)
          return;
        _myGr12L = value;
        RaisePropertyChanged("Grade12Language");
      }
    }

    public int SchoolType
    {
      get
      {
        return schoolType;
      }
      set
      {
        if (schoolType == value)
          return;
        schoolType = value;
        RaisePropertyChanged("SchoolType");
      }
    }

    public string Faculty1
    {
      get
      {
        return _myfaculty1;
      }
      set
      {
        if (_myfaculty1 == value)
          return;
        _myfaculty1 = value;
        RaisePropertyChanged("Faculty1");
      }
    }

    public string Faculty2
    {
      get
      {
        return _myfaculty2;
      }
      set
      {
        if (_myfaculty2 == value)
          return;
        _myfaculty2 = value;
        RaisePropertyChanged("Faculty2");
      }
    }

    public string Faculty3
    {
      get
      {
        return _myFaculty3;
      }
      set
      {
        if (_myFaculty3 == value)
          return;
        _myFaculty3 = value;
        RaisePropertyChanged("Faculty3");
      }
    }

    public string EOL
    {
      get
      {
        return mEOL;
      }
      set
      {
        mEOL = value;
      }
    }

    public datFileAttributes DatFile
    {
      get
      {
        return _mydatFile;
      }
      set
      {
        if (_mydatFile == value)
          return;
        _mydatFile = value;
        RaisePropertyChanged("DatFile");
      }
    }

    public string CSX_Number { get; set; }

    public string CSX_Part { get; set; }

    public int ErrorCount
    {
      get
      {
        return _myerrorcount;
      }
      set
      {
        if (_myerrorcount == value)
          return;
        _myerrorcount = value;
        RaisePropertyChanged("ErrorCount");
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

    public BioQADatRecord()
    {
    }

    public BioQADatRecord(string file)
    {
      DatFile = new datFileAttributes();
      DatFile.SName = Path.GetFileNameWithoutExtension(file).ToUpper();
      CSX_Number = DatFile.CSX.ToString();
    }

    private void checkerrors()
    {
      if (HasErrors)
        ErrorCount = _errors.Count;
      else
        ErrorCount = 0;
    }
  }
}
