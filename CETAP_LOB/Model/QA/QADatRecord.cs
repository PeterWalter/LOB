

using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Model.QA
{
  public class QADatRecord : ModelBase
  {
    private string _surname = "";
    private string _myname = "";
    private string _initials = "";
    private string _said = "";
    private string _foreignID = "";
    public const string TestDatePropertyName = "TestDate";
    public const string errorCountPropertyName = "errorCount";
    public const string DatFilePropertyName = "DatFile";
    public const string ReferencePropertyName = "Reference";
    public const string BarcodePropertyName = "Barcode";
    public const string SurnamePropertyName = "Surname";
    public const string FirstNamePropertyName = "FirstName";
    public const string initialsPropertyName = "initials";
    public const string SAIDPropertyName = "SAID";
    public const string ForeignIDPropertyName = "ForeignID";
    public const string DOBPropertyName = "DOB";
    public const string IDTypePropertyName = "IDType";
    public const string GenderPropertyName = "Gender";
    public const string CitizenshipPropertyName = "Citizenship";
    public const string ClassificationPropertyName = "Classification";
    public const string VenueCodePropertyName = "VenueCode";
    public const string DOTPropertyName = "DOT";
    public const string HomeLanguagePropertyName = "HomeLanguage";
    public const string SchoolLanguagePropertyName = "SchoolLanguage";
    public const string AQL_LanguagePropertyName = "AQL_Language";
    public const string AQL_CodePropertyName = "AQL_Code";
    public const string Section1PropertyName = "Section1";
    public const string Section2PropertyName = "Section2";
    public const string Section3PropertyName = "Section3";
    public const string Section4PropertyName = "Section4";
    public const string Section5PropertyName = "Section5";
    public const string Section6PropertyName = "Section6";
    public const string Section7PropertyName = "Section7";
    public const string Mat_LanguagePropertyName = "Mat_Language";
    public const string MatCodePropertyName = "MatCode";
    public const string MathSectionPropertyName = "MathSection";
    public const string Faculty1PropertyName = "Faculty1";
    public const string Faculty2PropertyName = "Faculty2";
    public const string Faculty3PropertyName = "Faculty3";
    public const string EOLPropertyName = "EOL";
    public const string EditedPropertyName = "Edited";
    public const string ScanNoPropertyName = "ScanNo";
    public const string IsSelectedPropertyName = "IsSelected";
    public const string BioInfoMismatchPropertyName = "BioInfoMismatch";
    public const string BioInfoMismatchDetailPropertyName = "BioInfoMismatchDetail";
    private DateTime _testDate;
    private int _mycount;
    private datFileAttributes _mydatFile;
    private string _nbt;
    private string _barcode;
    private DateTime _dob;
    private string _myIdType;
    private string _gender;
    private string _myCitizenship;
    private string _classi;
    private string _myVenueCode;
    private DateTime _dot;
    private string _myHomeLanguage;
    private string _Slanguage;
    private string _myAQL_Language;
    private string _myAQLCode;
    private ObservableCollection<DatAnswer> _mysection1;
    private ObservableCollection<DatAnswer> _mysection2;
    private ObservableCollection<DatAnswer> _mySection3;
    private ObservableCollection<DatAnswer> _mySection4;
    private ObservableCollection<DatAnswer> mySection5;
    private ObservableCollection<DatAnswer> _mySection6;
    private ObservableCollection<DatAnswer> _mySection7;
    private string _myMat_Language;
    private string _myMatCode;
    private ObservableCollection<DatAnswer> _myMathSection;
    private string _myfaculty1;
    private string _myFaculty2;
    private string _myFaculty3;
    private string _myEOL;
    private string _myEdited;
    private int _myScanNo;
    private bool _isSelected;
    private bool _bioInfoMismatch;
    private string _bioInfoMismatchDetail = "";
    private WritersBDO _writerRecord;
    private bool _nameMismatch;
    private bool _surnameMismatch;
    private bool _referenceMismatch;
    private bool _saidMismatch;
    private bool _foreignIdMismatch;
    private bool _dobMismatch;
    private bool _genderMismatch;
    private bool _dotMismatch;
        private int MathsOnly;

    public string CSX_Number { get; set; }

    public string CSX_Part { get; set; }

    public DateTime TestDate
    {
      get
      {
        return _testDate;
      }
      set
      {
        if (_testDate == value)
          return;
        _testDate = value;
        RaisePropertyChanged("TestDate");
        RaisePropertyChanged("DOT");
      }
    }

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
                if (_mydatFile.FileCombination == "MATE" || _mydatFile.FileCombination == "MATE") MathsOnly = 1;
        RaisePropertyChanged("DatFile");
      }
    }

    public string Reference
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
            AddError("Reference", "Not proper length for NBT number");
          else if (!HelperUtils.IsValidChecksum(str))
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

        public string Surname
        {
              get
              {
                    return _surname;
              }
              set
              {
                    if (_surname == value) return;
                    _surname = value;
                    _surname = _surname.Trim();
                    if (string.IsNullOrEmpty(_surname))
                        AddError("Surname", "Surname cannot be empty");
                    else
                        RemoveError("Surname");
                    MatchCollection matchCollection = new Regex("\\s").Matches(_surname);
                    if (!string.IsNullOrEmpty(_surname))
                    {
                          if (Regex.IsMatch(_surname, "\\d"))
                                AddError("Surname", "Surname cannot have digits");
                          else if (Regex.IsMatch(_surname, "[\\.\\*=!@#%\\&\\$]"))
                                AddError("Surname", "First name cannot have special characters");
                          else if (matchCollection.Count < 2)
                          {
                                string surname = _surname;
                                char[] chArray = new char[1]{ ' ' };
                                foreach (string str in surname.Split(chArray))
                                {
                                      if (str.Length < 2) AddError("Surname", "Part of Surname has few letters");
                                }
                          }
                          else if (matchCollection.Count > 2)
                                AddError("Surname", "Surname has too many spaces");
                          else
                                RemoveError("Surname");
                          ListSurnames.surname = _surname;
                          if (ListSurnames.IsFound)
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
        _myname = _myname.Trim();
        MatchCollection matchCollection = new Regex("\\s").Matches(_myname);
        if (string.IsNullOrEmpty(_myname))
          AddError("FirstName", "FirstName cannot be empty");
        else
          RemoveError("FirstName");
        if (!string.IsNullOrWhiteSpace(_myname))
        {
          if (Regex.IsMatch(_myname, "\\d"))
            AddError("FirstName", "First name cannot have digits");
          else if (Regex.IsMatch(_myname, "[\\.\\*=!@#%\\&\\$]"))
            AddError("FirstName", "First name cannot have special characters");
          else if (matchCollection.Count > 2)
            AddError("FirstName", "Name has too many spaces");
          else if (matchCollection.Count < 3)
          {
            string myname = _myname;
            char[] chArray = new char[1]{ ' ' };
            foreach (string str in myname.Split(chArray))
            {
              if (str.Length < 2)
                AddError("FirstName", "Part of Name has few letters");
            }
          }
          else
            RemoveError("FirstName");
          ApplicantNames.FirstName = _myname;
          if (ApplicantNames.IsFound)
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
        checkerrors();
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
        CheckDOB();
        checkerrors();
        RaisePropertyChanged("SAID");
        RaisePropertyChanged("DOB");
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
        if (timeSpan.TotalDays < 3650.0 || timeSpan.TotalDays > 36160.0)
          AddError("DOB", "Wrong age for Matric");
        else if (!string.IsNullOrWhiteSpace(_said) || !string.IsNullOrEmpty(_said))
          CheckDOB();
        else
          RemoveError("DOB");
        checkerrors();
        RaisePropertyChanged("DOB");
      }
    }

    public string IDType
    {
      get
      {
        return _myIdType;
      }
      set
      {
        if (_myIdType == value)
          return;
        _myIdType = value;
        string str = _myIdType.Trim();
        if (!string.IsNullOrWhiteSpace(_said) || !string.IsNullOrEmpty(_said))
        {
          if (CSX_Number == "761" || CSX_Number =="886" || CSX_Number == "909")
          {
            if (str != "1")
              AddError("IDType", "type should be 1");
            else
              RemoveError("IDType");
          }
          else if (CSX_Number == "667")
          {
            if (_myIdType != "S")
              AddError("IDType", "type should be S");
            else
              RemoveError("IDType");
          }
        }
        else if (CSX_Number == "761" || CSX_Number == "886" || CSX_Number == "909")
        {
          if (str != "2")
            AddError("IDType", "type should be 2");
          else
            RemoveError("IDType");
        }
        else if (CSX_Number == "667")
        {
          if (str != "F")
            AddError("IDType", "type should be F");
          else
            RemoveError("IDType");
        }
        else if(string.IsNullOrWhiteSpace(_said) || string.IsNullOrEmpty(_said))
        {
                    if (CSX_Number == "761" || CSX_Number == "886" || CSX_Number == "909")
                    {
                        if (str != "2")
                            AddError("IDType", "type should be 2");
                        else
                            RemoveError("IDType");
                    }
                    else if (CSX_Number == "667")
                    {
                        if (_myIdType == "S")
                            AddError("IDType", "type should be F");
                        else
                            RemoveError("IDType");
                    }
                }
                    checkerrors();
        RaisePropertyChanged("IDType");
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
        if (!string.IsNullOrWhiteSpace(_said) || !string.IsNullOrEmpty(_said))
        {
          if (_said.Length == 13)
          {
            int int32 = Convert.ToInt32(_said.Substring(6, 1));
            if (CSX_Number == "761" || CSX_Number == "886" || CSX_Number == "909")
            {
              if (int32 < 5 && _gender != "2")
                AddError("Gender", "Gender should be 2");
              else if (int32 > 4 && _gender != "1")
                AddError("Gender", "Wrong Gender should be 1");
              else
                RemoveError("Gender");
            }
            if (CSX_Number == "667")
            {
              if (int32 < 5 && _gender != "F")
                AddError("Gender", "Gender should be F");
              else if (int32 > 4 && _gender != "M")
                AddError("Gender", "Wrong Gender, should be M");
              else
                RemoveError("Gender");
            }
          }
          else
            RemoveError("Gender");
        }
        else
          RemoveError("Gender");
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
        _myCitizenship = (_myCitizenship ?? "").Trim();
        bool flag = HelperUtils.IsNumeric(_myCitizenship);
        if (string.IsNullOrWhiteSpace(_myCitizenship))
          AddError("Citizenship", "There should be a value");
        else if (flag)
        {
          if (Convert.ToInt32(_myCitizenship) > 4)
            AddError("Citizenship", "Value should be less than 5");
          else
            RemoveError("Citizenship");
        }
        else
          RemoveError("Citizenship");
        checkerrors();
        RaisePropertyChanged("Citizenship");
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
        _classi = (value ?? "").Trim();
        bool flag = HelperUtils.IsNumeric(_classi);

        if (string.IsNullOrWhiteSpace(_classi))
          AddError("Classification", "There should be a value");
        else if (!flag)
          AddError("Classification", "This should be a number");
        else if (Convert.ToInt32(_classi) > 5)
          AddError("Classification", "Value should be less than 6");
        else
          RemoveError("Classification");
        checkerrors();
        RaisePropertyChanged("Classification");
      }
    }

    public string VenueCode
    {
      get
      {
        return _myVenueCode;
      }
      set
      {
        if (_myVenueCode == value)
          return;
        _myVenueCode = value;
        if (_myVenueCode != _mydatFile.VenueCode.ToString("D5"))
          AddError("VenueCode", "Wrong Venue Code");
        else
          RemoveError("VenueCode");
        checkerrors();
        RaisePropertyChanged("VenueCode");
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
        if (_dot != _testDate)
          AddError("DOT", "Wrong Test date");
        else
          RemoveError("DOT");
        checkerrors();
        RaisePropertyChanged("DOT");
      }
    }

    public string HomeLanguage
    {
      get
      {
        return _myHomeLanguage;
      }
      set
      {
        if (_myHomeLanguage == value)
          return;
        _myHomeLanguage = value;
        bool flag = HelperUtils.IsNumeric(_myHomeLanguage);
        if (string.IsNullOrWhiteSpace(_myHomeLanguage))
          AddError("HomeLanguage", "Language cannot be empty");
        else if (flag)
        {
          if (Convert.ToInt32(_myHomeLanguage) > 12)
            AddError("HomeLanguage", "Language should be below 13");
          else
            RemoveError("HomeLanguage");
        }
        else
          RemoveError("HomeLanguage");
        checkerrors();
        RaisePropertyChanged("HomeLanguage");
      }
    }

    public string SchoolLanguage
    {
      get
      {
        return _Slanguage;
      }
      set
      {
        if (_Slanguage == value)
          return;
        _Slanguage = value;
        bool flag = HelperUtils.IsNumeric(_Slanguage);
        if (string.IsNullOrWhiteSpace(_Slanguage))
          AddError("SchoolLanguage", "Language cannot be empty");
        else if (flag)
        {
          if (Convert.ToInt32(_Slanguage) > 3)
            AddError("SchoolLanguage", "Language should be 01, 02 or 03");
          else
            RemoveError("SchoolLanguage");
        }
        else
          RemoveError("SchoolLanguage");
        checkerrors();
        RaisePropertyChanged("SchoolLanguage");
      }
    }

    public string AQL_Language
    {
      get
      {
        return _myAQL_Language;
      }
      set
      {
        if (_myAQL_Language == value)
          return;
       // DatFile.
          _myAQL_Language = value;
                if (_myAQL_Language != null)  _myAQL_Language = _myAQL_Language.Trim();
         // _myAQL_Language = _myAQL_Language.Trim();
        if (_myAQL_Language != DatFile.AQL_Language)
          AddError("AQL_Language", "Wrong Language");
        else
          RemoveError("AQL_Language");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    if (string.IsNullOrWhiteSpace(_myAQL_Language))
                        RemoveError("AQL_Language");
                    else
                        AddError("AQL_Language", "Wrong Language");
                }
                   
               checkerrors();
        RaisePropertyChanged("AQL_Language");
      }
    }

    public string AQL_Code
    {
      get
      {
        return _myAQLCode;
      }
      set
      {
        if (_myAQLCode == value)
          return;
        _myAQLCode = value;
        string str = (_myAQLCode ?? "").Trim();
        if (string.IsNullOrEmpty(AQLCOD))
          AQLCOD = "";
        else
          AQLCOD = Convert.ToInt32(AQLCOD).ToString("000");
        string expected = AQLCOD;

        int parsed;
        if (str.Length == 0)
        {
          if (expected != "")
            AddError("AQL_Code", "AQL code should be " + expected);
          else
            RemoveError("AQL_Code");
        }
        else if (!int.TryParse(str, out parsed))
        {
          AddError("AQL_Code", "AQL code cannot have characters");
        }
        else if (expected != "")
        {
          if (parsed.ToString("000") != expected)
            AddError("AQL_Code", "AQL code should be " + expected);
          else
            RemoveError("AQL_Code");
        }
        else if (IsMathsOnlyFile())
        {
          AddError("AQL_Code", "AQL code should be blank");
        }
        else
        {
          // No expected AQL code is available for this file, so the scanned
          // value cannot be judged - do not flag it.
          RemoveError("AQL_Code");
        }
        checkerrors();
        RaisePropertyChanged("AQL_Code");
      }
    }

    public ObservableCollection<DatAnswer> Section1
    {
      get
      {
        return _mysection1;
      }
      set
      {
        if (_mysection1 == value)
          return;
        _mysection1 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mysection1)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section1", "Errors in section 1");
        else
          RemoveError("Section1");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mysection1)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section1", "AQL section should be empty");
                    else
                        RemoveError("Section1");
                }
                checkerrors();
        RaisePropertyChanged("Section1");
      }
    }

    public ObservableCollection<DatAnswer> Section2
    {
      get
      {
        return _mysection2;
      }
      set
      {
        if (_mysection2 == value)
          return;
        _mysection2 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mysection2)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section2", "Errors in section 2");
        else
          RemoveError("Section2");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mysection2)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section2", "AQL section should be empty");
                    else
                        RemoveError("Section2");
                }
                checkerrors();
        RaisePropertyChanged("Section2");
      }
    }

    public ObservableCollection<DatAnswer> Section3
    {
      get
      {
        return _mySection3;
      }
      set
      {
        if (_mySection3 == value)
          return;
        _mySection3 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mySection3)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section3", "Errors in section 3");
        else
          RemoveError("Section3");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mySection3)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section3", "AQL section should be empty");
                    else
                        RemoveError("Section3");
                }
                checkerrors();
        RaisePropertyChanged("Section3");
      }
    }

    public ObservableCollection<DatAnswer> Section4
    {
      get
      {
        return _mySection4;
      }
      set
      {
        if (_mySection4 == value)
          return;
        _mySection4 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mySection4)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section4", "Errors in section 4");
        else
          RemoveError("Section4");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mySection4)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section4", "AQL section should be empty");
                    else
                        RemoveError("MSection4");
                }
                checkerrors();
        RaisePropertyChanged("Section4");
      }
    }

    public ObservableCollection<DatAnswer> Section5
    {
      get
      {
        return mySection5;
      }
      set
      {
        if (mySection5 == value)
          return;
        mySection5 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) mySection5)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section5", "Errors in section 5");
        else
          RemoveError("Section5");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>) mySection5)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section5", "AQL section should be empty");
                    else
                        RemoveError("Section5");
                }
                checkerrors();
        RaisePropertyChanged("Section5");
      }
    }

    public ObservableCollection<DatAnswer> Section6
    {
      get
      {
        return _mySection6;
      }
      set
      {
        if (_mySection6 == value)
          return;
        _mySection6 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mySection6)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section6", "Errors in section 6");
        else
          RemoveError("Section6");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mySection6)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section6", "AQL section should be empty");
                    else
                        RemoveError("Section6");
                }
                checkerrors();
        RaisePropertyChanged("Section6");
      }
    }

    public ObservableCollection<DatAnswer> Section7
    {
      get
      {
        return _mySection7;
      }
      set
      {
        if (_mySection7 == value)
          return;
        _mySection7 = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _mySection7)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("Section7", "Errors in section 7");
        else
          RemoveError("Section7");
                if (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116")
                {
                    foreach (DatAnswer datAnswer in (Collection<DatAnswer>)_mySection7)
                    {
                        if ((int)datAnswer.Value != 32)
                            flag = true;
                    }
                    if (flag)
                        AddError("Section7", "AQL section should be empty");
                    else
                        RemoveError("Section7");
                }
                checkerrors();
        RaisePropertyChanged("Section7");
      }
    }

    public string Mat_Language
    {
      get
      {
        return _myMat_Language;
      }
      set
      {
        if (_myMat_Language == value)
          return;
        _myMat_Language = value;
        _myMat_Language = _myMat_Language.Trim();
        if (_myMat_Language != DatFile.MAT_Language)
          AddError("Mat_Language", "Wrong Language");
        else
          RemoveError("Mat_Language");
        checkerrors();
        RaisePropertyChanged("Mat_Language");
      }
    }

    public string MatCode
    {
      get
      {
        return _myMatCode;
      }
      set
      {
        if (_myMatCode == value)
          return;
        _myMatCode = value;
        string str = (_myMatCode ?? "").Trim();
        if (string.IsNullOrEmpty(MATCOD))
          MATCOD = "";
        else
          MATCOD = Convert.ToInt32(MATCOD).ToString("000");
        string expected = MATCOD;

        int parsed;
        if (str.Length == 0)
        {
          if (expected != "")
            AddError("MatCode", "MAT code should be " + expected);
          else
            RemoveError("MatCode");
        }
        else if (!int.TryParse(str, out parsed))
        {
          AddError("MatCode", "MAT code cannot have characters");
        }
        else if (expected != "")
        {
          if (parsed.ToString("000") != expected)
            AddError("MatCode", "MAT code should be " + expected);
          else
            RemoveError("MatCode");
        }
        else if (IsAqlOnlyFile())
        {
          // An AQL only file must not carry a Maths code.
          AddError("MatCode", "MAT code should be blank");
        }
        else
        {
          // No expected MAT code is available for this file, so the scanned
          // value cannot be judged - do not flag it.
          RemoveError("MatCode");
        }
        checkerrors();
        RaisePropertyChanged("MatCode");
      }
    }

    public ObservableCollection<DatAnswer> MathSection
    {
      get
      {
        return _myMathSection;
      }
      set
      {
        if (_myMathSection == value)
          return;
        _myMathSection = value;
        bool flag = false;
        foreach (ModelBase modelBase in (Collection<DatAnswer>) _myMathSection)
        {
          if (modelBase.HasErrors)
            flag = true;
        }
        if (flag)
          AddError("MathSection", "Errors in MAT section");
        else
          RemoveError("MathSection");
        if (_mydatFile.TestCode == "0105" || _mydatFile.TestCode == "0115")
        {
          foreach (DatAnswer datAnswer in (Collection<DatAnswer>) _myMathSection)
          {
            if ((int) datAnswer.Value != 32)
              flag = true;
          }
          if (flag)
            AddError("MathSection", "MAT section should be empty");
          else
            RemoveError("MathSection");
        }
        checkerrors();
        RaisePropertyChanged("MathSection");
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
        _myfaculty1 = _myfaculty1.Trim();
        if (string.IsNullOrEmpty(_myfaculty1))
          AddError("Faculty1", "Faculty cannot be empty");
        else if (_myfaculty1 == "*")
          AddError("Faculty1", "Faculty cannot have (*)");
        else
          RemoveError("Faculty1");
        checkerrors();
        RaisePropertyChanged("Faculty1");
      }
    }

    public string Faculty2
    {
      get
      {
        return _myFaculty2;
      }
      set
      {
        if (_myFaculty2 == value)
          return;
        _myFaculty2 = value;
        if (!string.IsNullOrEmpty(_myFaculty2.Trim()))
        {
          if (_myFaculty2 == "*")
            AddError("Faculty2", "Faculty cannot have (*)");
          else
            RemoveError("Faculty2");
        }
        else
          RemoveError("Faculty2");
        checkerrors();
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
        if (!string.IsNullOrEmpty(_myFaculty3.Trim()))
        {
          if (_myFaculty3 == "*")
            AddError("Faculty3", "Faculty cannot have (*)");
          else
            RemoveError("Faculty3");
        }
        else
          RemoveError("Faculty3");
        checkerrors();
        RaisePropertyChanged("Faculty3");
      }
    }

    public string EOL
    {
      get
      {
        return _myEOL;
      }
      set
      {
        if (_myEOL == value)
          return;
        _myEOL = value;
        RaisePropertyChanged("EOL");
      }
    }

    public string Edited
    {
      get
      {
        return _myEdited;
      }
      set
      {
        if (_myEdited == value)
          return;
        _myEdited = value;
        RaisePropertyChanged("Edited");
      }
    }

    public int ScanNo
    {
      get
      {
        return _myScanNo;
      }
      set
      {
        if (_myScanNo == value)
          return;
        _myScanNo = value;
        RaisePropertyChanged("ScanNo");
      }
    }

    public string AQLCOD { get; set; }

    public string MATCOD { get; set; }

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

    /// <summary>
    /// True when a matching WriterList record was found and one or more of
    /// the candidate's biographical fields differ from it.
    /// </summary>
    public bool BioInfoMismatch
    {
      get
      {
        return _bioInfoMismatch;
      }
      private set
      {
        if (_bioInfoMismatch == value)
          return;
        _bioInfoMismatch = value;
        RaisePropertyChanged("BioInfoMismatch");
      }
    }

    /// <summary>
    /// Comma separated list of the biographical fields that differ from the
    /// matching WriterList record. Empty when there is no mismatch.
    /// </summary>
    public string BioInfoMismatchDetail
    {
      get
      {
        return _bioInfoMismatchDetail;
      }
      private set
      {
        if (_bioInfoMismatchDetail == value)
          return;
        _bioInfoMismatchDetail = value;
        RaisePropertyChanged("BioInfoMismatchDetail");
      }
    }

    /// <summary>True when the first name differs from the matching WriterList record.</summary>
    public bool NameMismatch
    {
      get { return _nameMismatch; }
      private set
      {
        if (_nameMismatch == value)
          return;
        _nameMismatch = value;
        RaisePropertyChanged("NameMismatch");
      }
    }

    /// <summary>True when the surname differs from the matching WriterList record.</summary>
    public bool SurnameMismatch
    {
      get { return _surnameMismatch; }
      private set
      {
        if (_surnameMismatch == value)
          return;
        _surnameMismatch = value;
        RaisePropertyChanged("SurnameMismatch");
      }
    }

    /// <summary>True when the Reference/NBT differs from the matching WriterList record.</summary>
    public bool ReferenceMismatch
    {
      get { return _referenceMismatch; }
      private set
      {
        if (_referenceMismatch == value)
          return;
        _referenceMismatch = value;
        RaisePropertyChanged("ReferenceMismatch");
      }
    }

    /// <summary>True when the SA ID differs from the matching WriterList record.</summary>
    public bool SAIDMismatch
    {
      get { return _saidMismatch; }
      private set
      {
        if (_saidMismatch == value)
          return;
        _saidMismatch = value;
        RaisePropertyChanged("SAIDMismatch");
      }
    }

    /// <summary>True when the foreign ID differs from the matching WriterList record.</summary>
    public bool ForeignIDMismatch
    {
      get { return _foreignIdMismatch; }
      private set
      {
        if (_foreignIdMismatch == value)
          return;
        _foreignIdMismatch = value;
        RaisePropertyChanged("ForeignIDMismatch");
      }
    }

    /// <summary>True when the date of birth differs from the matching WriterList record.</summary>
    public bool DOBMismatch
    {
      get { return _dobMismatch; }
      private set
      {
        if (_dobMismatch == value)
          return;
        _dobMismatch = value;
        RaisePropertyChanged("DOBMismatch");
      }
    }

    /// <summary>True when the gender differs from the matching WriterList record.</summary>
    public bool GenderMismatch
    {
      get { return _genderMismatch; }
      private set
      {
        if (_genderMismatch == value)
          return;
        _genderMismatch = value;
        RaisePropertyChanged("GenderMismatch");
      }
    }

    /// <summary>True when the date of test differs from the matching WriterList record.</summary>
    public bool DOTMismatch
    {
      get { return _dotMismatch; }
      private set
      {
        if (_dotMismatch == value)
          return;
        _dotMismatch = value;
        RaisePropertyChanged("DOTMismatch");
      }
    }

    /// <summary>True when a matching WriterList record is attached to this record.</summary>
    public bool HasWriterRecord
    {
      get { return _writerRecord != null; }
    }

    /// <summary>WriterList first name, for display next to a Name mismatch.</summary>
    public string WriterName
    {
      get { return _writerRecord == null ? "" : _writerRecord.Name; }
    }

    /// <summary>WriterList surname, for display next to a Surname mismatch.</summary>
    public string WriterSurname
    {
      get { return _writerRecord == null ? "" : _writerRecord.Surname; }
    }

    /// <summary>WriterList NBT reference, for display next to a Reference mismatch.</summary>
    public string WriterReference
    {
      get { return _writerRecord == null ? "" : _writerRecord.NBT.ToString(); }
    }

    /// <summary>WriterList SA ID, for display next to an SA ID mismatch.</summary>
    public string WriterSAID
    {
      get { return _writerRecord == null || !_writerRecord.SAID.HasValue ? "" : _writerRecord.SAID.Value.ToString("D13"); }
    }

    /// <summary>WriterList foreign ID, for display next to a Foreign ID mismatch.</summary>
    public string WriterForeignID
    {
      get { return _writerRecord == null ? "" : _writerRecord.ForeignID; }
    }

    /// <summary>WriterList date of birth, for display next to a DOB mismatch.</summary>
    public string WriterDOB
    {
      get { return _writerRecord == null || _writerRecord.DOB == default(DateTime) ? "" : _writerRecord.DOB.ToString("yyyy/MM/dd"); }
    }

    /// <summary>WriterList gender, for display next to a Gender mismatch.</summary>
    public string WriterGender
    {
      get { return _writerRecord == null ? "" : _writerRecord.Gender; }
    }

    /// <summary>WriterList date of test, for display next to a Date of Test mismatch.</summary>
    public string WriterDOT
    {
      get { return _writerRecord == null || _writerRecord.DOT == default(DateTime) ? "" : _writerRecord.DOT.ToString("yyyy/MM/dd"); }
    }

    /// <summary>
    /// The matching WriterList record as display lines for the grid context menu.
    /// Only populated values are listed, so a sparse record stays compact.
    /// </summary>
    public List<string> GetWriterRecordLines()
    {
      List<string> lines = new List<string>();
      if (_writerRecord == null)
        return lines;

      lines.Add("WriterList record");
      AddWriterLine(lines, "Name", _writerRecord.Name);
      AddWriterLine(lines, "Surname", _writerRecord.Surname);
      AddWriterLine(lines, "Initials", _writerRecord.Initials);
      if (_writerRecord.NBT != 0)
        lines.Add("NBT: " + _writerRecord.NBT);
      if (_writerRecord.SAID.HasValue)
        lines.Add("SA ID: " + _writerRecord.SAID.Value.ToString("D13"));
      AddWriterLine(lines, "Foreign ID", _writerRecord.ForeignID);
      if (_writerRecord.DOB != default(DateTime))
        lines.Add("Date of Birth: " + _writerRecord.DOB.ToString("yyyy/MM/dd"));
      AddWriterLine(lines, "Gender", _writerRecord.Gender);
      if (_writerRecord.DOT != default(DateTime))
        lines.Add("Date of Test: " + _writerRecord.DOT.ToString("yyyy/MM/dd"));
      AddWriterLine(lines, "Classification", _writerRecord.Classification);
      AddWriterLine(lines, "Test Language", _writerRecord.TestLanguage);
      AddWriterLine(lines, "Test Type", _writerRecord.TestType);
      if (_writerRecord.VenueID != 0)
        lines.Add("Venue ID: " + _writerRecord.VenueID);
      AddWriterLine(lines, "Mobile", _writerRecord.Mobile);
      AddWriterLine(lines, "Home Telephone", _writerRecord.HomeTelephone);
      AddWriterLine(lines, "Email", _writerRecord.EMail);
      return lines;
    }

    private static void AddWriterLine(List<string> lines, string label, string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
        lines.Add(label + ": " + value.Trim());
    }

    /// <summary>
    /// Corrects a single biography field from the attached WriterList record.
    /// The field name matches the context menu Tag used in QAView. Setting the
    /// field re-runs validation, so the red highlight clears when it now matches.
    /// </summary>
    public void ApplyWriterValue(string field)
    {
      if (_writerRecord == null || string.IsNullOrEmpty(field))
        return;

      switch (field)
      {
        case "Name":
          FirstName = _writerRecord.Name;
          break;
        case "Surname":
          Surname = _writerRecord.Surname;
          break;
        case "Reference":
          string writerReference = _writerRecord.NBT.ToString();
          if (writerReference.Length == 14)
            Reference = writerReference;
          break;
        case "SAID":
          SAID = _writerRecord.SAID.HasValue ? _writerRecord.SAID.Value.ToString("D13") : "";
          break;
        case "ForeignID":
          ForeignID = _writerRecord.ForeignID;
          break;
        case "DOB":
          DOB = _writerRecord.DOB;
          break;
        case "Gender":
          Gender = _writerRecord.Gender;
          break;
        case "DOT":
          DOT = _writerRecord.DOT;
          break;
      }
    }

    /// <summary>
    /// True when applying the WriterList value would actually change this field.
    /// Unlike the mismatch flags this also returns true when the scanned value is
    /// missing or unreadable (for example a Reference of "*"), so the field can
    /// still be corrected from the WriterList. Used to enable the
    /// "Use WriterList value" context menu entry.
    /// </summary>
    public bool CanApplyWriterValue(string field)
    {
      if (_writerRecord == null || string.IsNullOrEmpty(field))
        return false;

      switch (field)
      {
        case "Name":
          return !string.IsNullOrWhiteSpace(_writerRecord.Name) && NormaliseText(_myname) != NormaliseText(_writerRecord.Name);
        case "Surname":
          return !string.IsNullOrWhiteSpace(_writerRecord.Surname) && NormaliseText(_surname) != NormaliseText(_writerRecord.Surname);
        case "Reference":
          string writerReference = _writerRecord.NBT.ToString();
          return writerReference.Length == 14 && NormaliseText(_nbt) != NormaliseText(writerReference);
        case "SAID":
          return _writerRecord.SAID.HasValue && NormaliseText(_said) != NormaliseText(_writerRecord.SAID.Value.ToString("D13"));
        case "ForeignID":
          return !string.IsNullOrWhiteSpace(_writerRecord.ForeignID) && NormaliseText(_foreignID) != NormaliseText(_writerRecord.ForeignID);
        case "DOB":
          return _writerRecord.DOB != default(DateTime) && _dob.Date != _writerRecord.DOB.Date;
        case "Gender":
          if (string.IsNullOrWhiteSpace(_writerRecord.Gender))
            return false;
          string mine = NormaliseText(_gender);
          string theirs = NormaliseText(_writerRecord.Gender);
          string codeMine = GenderCode(mine);
          string codeTheirs = GenderCode(theirs);
          if (codeMine.Length > 0 && codeTheirs.Length > 0)
            return codeMine != codeTheirs;
          return mine != theirs;
        case "DOT":
          return _writerRecord.DOT != default(DateTime) && _dot.Date != _writerRecord.DOT.Date;
        default:
          return false;
      }
    }

    /// <summary>
    /// Attaches the WriterList record matched by Reference (NBT) or, when
    /// there is no NBT match, by SA ID. A null snapshot leaves this record
    /// untouched by the bio information validation.
    /// </summary>
    public void AttachWriterRecord(WritersBDO writer)
    {
      _writerRecord = writer;
      RaisePropertyChanged("HasWriterRecord");
      RaisePropertyChanged("WriterName");
      RaisePropertyChanged("WriterSurname");
      RaisePropertyChanged("WriterReference");
      RaisePropertyChanged("WriterSAID");
      RaisePropertyChanged("WriterForeignID");
      RaisePropertyChanged("WriterDOB");
      RaisePropertyChanged("WriterGender");
      RaisePropertyChanged("WriterDOT");
      checkerrors();
    }

    /// <summary>
    /// Compares this record's biographical information with the attached
    /// WriterList record. Only records that matched a WriterList entry are
    /// checked, so the existing "not found in WriterList" handling is left
    /// untouched. A blank WriterList field is not comparable and never a
    /// mismatch.
    /// </summary>
    private void ValidateBioInfo()
    {
      if (_writerRecord == null)
      {
        ClearMismatchFlags();
        BioInfoMismatch = false;
        BioInfoMismatchDetail = "";
        if (_errors.ContainsKey("BioInfoMismatch"))
          RemoveError("BioInfoMismatch");
        return;
      }

      NameMismatch = !MatchesText(_myname, _writerRecord.Name);
      SurnameMismatch = !MatchesText(_surname, _writerRecord.Surname);
      ReferenceMismatch = !MatchesReference(_nbt, _writerRecord.NBT);
      SAIDMismatch = !MatchesSAID(_said, _writerRecord.SAID);
      ForeignIDMismatch = !MatchesText(_foreignID, _writerRecord.ForeignID);
      DOBMismatch = !MatchesDate(_dob, _writerRecord.DOB);
      GenderMismatch = !MatchesGender(_gender, _writerRecord.Gender);
      DOTMismatch = !MatchesDate(_dot, _writerRecord.DOT);

      List<string> mismatched = new List<string>();
      if (NameMismatch)
        mismatched.Add("Name");
      if (SurnameMismatch)
        mismatched.Add("Surname");
      if (ReferenceMismatch)
        mismatched.Add("NBT Reference");
      if (SAIDMismatch)
        mismatched.Add("SA ID");
      if (ForeignIDMismatch)
        mismatched.Add("Foreign ID");
      if (DOBMismatch)
        mismatched.Add("Date of Birth");
      if (GenderMismatch)
        mismatched.Add("Gender");
      if (DOTMismatch)
        mismatched.Add("Date of Test");

      if (mismatched.Count > 0)
      {
        BioInfoMismatchDetail = string.Join(", ", mismatched);
        BioInfoMismatch = true;
        AddError("BioInfoMismatch", "Candidate's biographical information does not match the WriterList record. Fields: " + BioInfoMismatchDetail + ".");
      }
      else
      {
        BioInfoMismatchDetail = "";
        BioInfoMismatch = false;
        if (_errors.ContainsKey("BioInfoMismatch"))
          RemoveError("BioInfoMismatch");
      }
    }

    private void ClearMismatchFlags()
    {
      NameMismatch = false;
      SurnameMismatch = false;
      ReferenceMismatch = false;
      SAIDMismatch = false;
      ForeignIDMismatch = false;
      DOBMismatch = false;
      GenderMismatch = false;
      DOTMismatch = false;
    }

    private static string NormaliseText(string value)
    {
      return string.IsNullOrWhiteSpace(value) ? "" : value.Trim().ToUpperInvariant();
    }

    /// <summary>Blank on either side is not comparable and never a mismatch.</summary>
    private static bool MatchesText(string left, string right)
    {
      string a = NormaliseText(left);
      string b = NormaliseText(right);
      if (a.Length == 0 || b.Length == 0)
        return true;
      return a == b;
    }

    private static long? ToLong(string value)
    {
      long parsed;
      if (!string.IsNullOrWhiteSpace(value) && long.TryParse(value.Trim(), out parsed))
        return parsed;
      return null;
    }

    private static bool MatchesSAID(string said, long? writerSAID)
    {
      long? mine = ToLong(said);
      if (!mine.HasValue || !writerSAID.HasValue)
        return true;
      return mine.Value == writerSAID.Value;
    }

    /// <summary>
    /// Compares the Reference/NBT with the WriterList NBT. An unparseable
    /// reference is not comparable and never a mismatch.
    /// </summary>
    private static bool MatchesReference(string reference, long writerNBT)
    {
      long? mine = ToLong(reference);
      if (!mine.HasValue)
        return true;
      return mine.Value == writerNBT;
    }

    private static string GenderCode(string value)
    {
      switch (NormaliseText(value))
      {
        case "1":
        case "M":
        case "MALE":
          return "M";
        case "2":
        case "F":
        case "FEMALE":
          return "F";
        default:
          return "";
      }
    }

    /// <summary>
    /// Gender is stored as 1/2 on some CSX files and M/F on others, so the
    /// equivalent encodings are normalised before comparing.
    /// </summary>
    private static bool MatchesGender(string mine, string writerGender)
    {
      string a = NormaliseText(mine);
      string b = NormaliseText(writerGender);
      if (a.Length == 0 || b.Length == 0)
        return true;
      string codeA = GenderCode(a);
      string codeB = GenderCode(b);
      if (codeA.Length == 0 || codeB.Length == 0)
        return true;
      return codeA == codeB;
    }

    private static bool MatchesDate(DateTime mine, DateTime writerDate)
    {
      if (mine == default(DateTime) || writerDate == default(DateTime))
        return true;
      return mine.Date == writerDate.Date;
    }

    /// <summary>True when the file carries AQL only, so its Maths code must be blank.</summary>
    private bool IsAqlOnlyFile()
    {
      return _mydatFile != null && (_mydatFile.TestCode == "0105" || _mydatFile.TestCode == "0115");
    }

    /// <summary>True when the file carries Maths only, so its AQL code must be blank.</summary>
    private bool IsMathsOnlyFile()
    {
      return _mydatFile != null && (_mydatFile.TestCode == "0106" || _mydatFile.TestCode == "0116");
    }

    public QADatRecord()
    {
    }

    public QADatRecord(string file)
    {
      DatFile = new datFileAttributes();
      DatFile.SName = Path.GetFileNameWithoutExtension(file).ToUpper();
      CSX_Number = DatFile.CSX.ToString();
    }

    private void checkerrors()
    {
      ValidateBioInfo();
      if (HasErrors)
        errorCount = _errors.Count;
      else
        errorCount = 0;
    }

    public void CheckDOB()
    {
      if (HelperUtils.DOBfromSAID(_said) != string.Format("{0:dd/MM/yyyy}", (object) _dob))
        AddError("DOB", "ID and DOB not the same");
      else
        RemoveError("DOB");
    }

    /// <summary>
    /// Fields that are constants for a whole QA file. Every record in a file
    /// must carry the same venue, test codes, test date and test languages.
    /// </summary>
    public static readonly string[] FileLevelFieldNames = { "VenueCode", "AQL_Code", "MatCode", "DOT", "AQL_Language", "Mat_Language" };

    public static bool IsFileLevelField(string propertyName)
    {
      return !string.IsNullOrEmpty(propertyName) && Array.IndexOf(FileLevelFieldNames, propertyName) >= 0;
    }

    /// <summary>Reads one of the file level fields.</summary>
    public object GetFileLevelValue(string propertyName)
    {
      switch (propertyName)
      {
        case "VenueCode":
          return VenueCode;
        case "AQL_Code":
          return AQL_Code;
        case "MatCode":
          return MatCode;
        case "DOT":
          return DOT;
        case "AQL_Language":
          return AQL_Language;
        case "Mat_Language":
          return Mat_Language;
        default:
          return null;
      }
    }

    /// <summary>Writes one of the file level fields.</summary>
    public void SetFileLevelValue(string propertyName, object value)
    {
      switch (propertyName)
      {
        case "VenueCode":
          VenueCode = value as string;
          break;
        case "AQL_Code":
          AQL_Code = value as string;
          break;
        case "MatCode":
          MatCode = value as string;
          break;
        case "DOT":
          if (value is DateTime)
            DOT = (DateTime)value;
          break;
        case "AQL_Language":
          AQL_Language = value as string;
          break;
        case "Mat_Language":
          Mat_Language = value as string;
          break;
      }
    }

    /// <summary>
    /// Copies a file level field from one record to every other record so the
    /// whole file keeps a single value for the venue, test codes, test date and
    /// test languages. Correcting one record therefore corrects the file.
    /// </summary>
    public static void PropagateFileLevelField(IEnumerable<QADatRecord> records, QADatRecord source, string propertyName)
    {
      if (records == null || source == null || !IsFileLevelField(propertyName))
        return;
      object value = source.GetFileLevelValue(propertyName);
      foreach (QADatRecord record in records)
      {
        if (record == null || ReferenceEquals(record, source))
          continue;
        record.SetFileLevelValue(propertyName, value);
      }
    }
  }
}
