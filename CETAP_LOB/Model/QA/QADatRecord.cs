// Decompiled with JetBrains decompiler
// Type: LOB.Model.QA.QADatRecord
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Helper;
using System;
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
                          else if (matchCollection.Count < 3)
                          {
                                string surname = _surname;
                                char[] chArray = new char[1]{ ' ' };
                                foreach (string str in surname.Split(chArray))
                                {
                                      if (str.Length < 3) AddError("Surname", "Part of Surname has few letters");
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
              if (str.Length < 3)
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
        if (timeSpan.TotalDays < 3650.0 || timeSpan.TotalDays > 29100.0)
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
            }
            if (CSX_Number == "667")
            {
              if (int32 < 5 && _gender != "F")
                AddError("Gender", "Gender should be F");
              else if (int32 > 4 && _gender != "M")
                AddError("Gender", "Wrong Gender, should be M");
            }
          }
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
        _myAQL_Language = value;
        if (_myAQL_Language.Trim() != DatFile.AQL_Language)
          AddError("AQL_Language", "Wrong Language");
        else
          RemoveError("AQL_Language");
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
        string str = _myAQLCode.Trim();
        if (AQLCOD.Length > 0)
          AQLCOD = Convert.ToInt32(AQLCOD).ToString("000");
        if (AQLCOD != str)
          AddError("AQL_Code", "Wrong AQL Code");
        else
          RemoveError("AQL_Code");
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
        string str = _myMatCode.Trim();
        if (MATCOD.Length > 0)
          MATCOD = Convert.ToInt32(MATCOD).ToString("000");
        if (string.IsNullOrWhiteSpace(str))
        {
          if (MATCOD != "")
            AddError("MatCode", "MAT code should be " + MATCOD);
        }
        else if (!string.IsNullOrWhiteSpace(str))
        {
          if (Convert.ToInt32(str).ToString("000") != MATCOD)
            AddError("MatCode", "MAT code should be " + MATCOD);
        }
        else
          RemoveError("MatCode");
        if (MATCOD == "" && str == "")
          RemoveError("MatCode");
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
  }
}
