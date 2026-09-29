

using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private WritersBDO _writerRecord;
    private CompositBDO _compositRecord;
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
        ValidateReference();
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
        ValidateSurname();
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
        ValidateFirstName();
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
        ValidateSAID();
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
        ValidateForeignID();
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
        ValidateDOB();
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

    public void AttachWriterRecord(WritersBDO writer)
    {
      _writerRecord = writer;
      ValidateReference();
      ValidateFirstName();
      ValidateSurname();
      ValidateSAID();
      ValidateForeignID();
      ValidateDOB();
    }

    public void AttachCompositRecord(CompositBDO composit)
    {
      _compositRecord = composit;
      ValidateReference();
      ValidateFirstName();
      ValidateSurname();
      ValidateSAID();
      ValidateForeignID();
      ValidateDOB();
    }

    public bool HasWriterRecord
    {
      get { return _writerRecord != null; }
    }

    public bool HasCompositRecord
    {
      get { return _compositRecord != null; }
    }

    public List<string> GetWriterRecordLines()
    {
      List<string> lines = new List<string>();
      if (_writerRecord == null)
        return lines;

      lines.Add("WriterList record");
      AddLine(lines, "Name", _writerRecord.Name);
      AddLine(lines, "Surname", _writerRecord.Surname);
      AddLine(lines, "Initials", _writerRecord.Initials);
      if (_writerRecord.NBT != 0)
        lines.Add("Reference: " + _writerRecord.NBT);
      if (_writerRecord.SAID.HasValue)
        lines.Add("South African ID: " + _writerRecord.SAID.Value.ToString("D13"));
      AddLine(lines, "Foreign ID", _writerRecord.ForeignID);
      if (_writerRecord.DOB != default(DateTime))
        lines.Add("Date of Birth: " + _writerRecord.DOB.ToString("yyyy/MM/dd"));
      AddLine(lines, "Gender", _writerRecord.Gender);
      if (_writerRecord.DOT != default(DateTime))
        lines.Add("Date of Test: " + _writerRecord.DOT.ToString("yyyy/MM/dd"));
      AddLine(lines, "Classification", _writerRecord.Classification);
      AddLine(lines, "Test Language", _writerRecord.TestLanguage);
      AddLine(lines, "Test Type", _writerRecord.TestType);
      if (_writerRecord.VenueID != 0)
        lines.Add("Venue ID: " + _writerRecord.VenueID);
      AddLine(lines, "Mobile", _writerRecord.Mobile);
      AddLine(lines, "Home Telephone", _writerRecord.HomeTelephone);
      AddLine(lines, "Email", _writerRecord.EMail);
      return lines;
    }

    public List<string> GetCompositRecordLines()
    {
      List<string> lines = new List<string>();
      if (_compositRecord == null)
        return lines;

      lines.Add("Composit record");
      if (_compositRecord.RefNo != 0)
        lines.Add("Reference: " + _compositRecord.RefNo);
      AddLine(lines, "Name", _compositRecord.Name);
      AddLine(lines, "Surname", _compositRecord.Surname);
      AddLine(lines, "Initials", _compositRecord.Initials);
      if (_compositRecord.SAID.HasValue)
        lines.Add("South African ID: " + _compositRecord.SAID.Value.ToString("D13"));
      AddLine(lines, "Foreign ID", _compositRecord.ForeignID);
      if (_compositRecord.DOB != default(DateTime))
        lines.Add("Date of Birth: " + _compositRecord.DOB.ToString("yyyy/MM/dd"));
      AddLine(lines, "Gender", _compositRecord.Gender);
      AddLine(lines, "Classification", _compositRecord.Classification);
      AddLine(lines, "Venue", _compositRecord.VenueName);
      if (_compositRecord.DOT != default(DateTime))
        lines.Add("Date of Test: " + _compositRecord.DOT.ToString("yyyy/MM/dd"));
      return lines;
    }

    public void ApplyWriterValue(string field)
    {
      if (_writerRecord == null || string.IsNullOrWhiteSpace(field))
        return;

      switch (field)
      {
        case "Name":
          FirstName = _writerRecord.Name;
          if (_compositRecord != null && AreDifferent(_compositRecord.Name, _writerRecord.Name))
            _compositRecord.Name = _writerRecord.Name;
          break;
        case "Surname":
          Surname = _writerRecord.Surname;
          if (_compositRecord != null && AreDifferent(_compositRecord.Surname, _writerRecord.Surname))
            _compositRecord.Surname = _writerRecord.Surname;
          break;
        case "Reference":
          SyncReferenceValue(_writerRecord.NBT.ToString());
          break;
        case "SAID":
          SAID = _writerRecord.SAID.HasValue ? _writerRecord.SAID.Value.ToString("D13") : "";
          if (_compositRecord != null && _compositRecord.SAID != _writerRecord.SAID)
            _compositRecord.SAID = _writerRecord.SAID;
          break;
        case "ForeignID":
          ForeignID = _writerRecord.ForeignID;
          if (_compositRecord != null && AreDifferent(_compositRecord.ForeignID, _writerRecord.ForeignID))
            _compositRecord.ForeignID = _writerRecord.ForeignID;
          break;
        case "DOB":
          DOB = _writerRecord.DOB;
          if (_compositRecord != null && _compositRecord.DOB != _writerRecord.DOB)
            _compositRecord.DOB = _writerRecord.DOB;
          break;
        case "Gender":
          Gender = _writerRecord.Gender;
          if (_compositRecord != null && AreDifferent(_compositRecord.Gender, _writerRecord.Gender))
            _compositRecord.Gender = _writerRecord.Gender;
          break;
      }

      SyncWalkInReferenceIfNeeded();
      RevalidateIdentityFields();
    }

    public void ApplyCompositValue(string field)
    {
      if (_compositRecord == null || string.IsNullOrWhiteSpace(field))
        return;

      switch (field)
      {
        case "Name":
          FirstName = _compositRecord.Name;
          if (_writerRecord != null && AreDifferent(_writerRecord.Name, _compositRecord.Name))
            _writerRecord.Name = _compositRecord.Name;
          break;
        case "Surname":
          Surname = _compositRecord.Surname;
          if (_writerRecord != null && AreDifferent(_writerRecord.Surname, _compositRecord.Surname))
            _writerRecord.Surname = _compositRecord.Surname;
          break;
        case "Reference":
          SyncReferenceValue(_compositRecord.RefNo.ToString());
          break;
        case "SAID":
          SAID = _compositRecord.SAID.HasValue ? _compositRecord.SAID.Value.ToString("D13") : "";
          if (_writerRecord != null && _writerRecord.SAID != _compositRecord.SAID)
            _writerRecord.SAID = _compositRecord.SAID;
          break;
        case "ForeignID":
          ForeignID = _compositRecord.ForeignID;
          if (_writerRecord != null && AreDifferent(_writerRecord.ForeignID, _compositRecord.ForeignID))
            _writerRecord.ForeignID = _compositRecord.ForeignID;
          break;
        case "DOB":
          DOB = _compositRecord.DOB;
          if (_writerRecord != null && _writerRecord.DOB != _compositRecord.DOB)
            _writerRecord.DOB = _compositRecord.DOB;
          break;
        case "Gender":
          Gender = _compositRecord.Gender;
          if (_writerRecord != null && AreDifferent(_writerRecord.Gender, _compositRecord.Gender))
            _writerRecord.Gender = _compositRecord.Gender;
          break;
      }

      SyncWalkInReferenceIfNeeded();
      RevalidateIdentityFields();
    }

    public bool CanApplyWriterValue(string field)
    {
      if (_writerRecord == null || string.IsNullOrWhiteSpace(field))
        return false;

      switch (field)
      {
        case "Name":
          return !string.IsNullOrWhiteSpace(_writerRecord.Name) && NormaliseText(_myname) != NormaliseText(_writerRecord.Name);
        case "Surname":
          return !string.IsNullOrWhiteSpace(_writerRecord.Surname) && NormaliseText(_surname) != NormaliseText(_writerRecord.Surname);
        case "Reference":
          return _writerRecord.NBT != 0 && NormaliseText(_NBT) != NormaliseText(_writerRecord.NBT.ToString());
        case "SAID":
          return _writerRecord.SAID.HasValue && NormaliseText(_said) != NormaliseText(_writerRecord.SAID.Value.ToString("D13"));
        case "ForeignID":
          return !string.IsNullOrWhiteSpace(_writerRecord.ForeignID) && NormaliseText(_foreignID) != NormaliseText(_writerRecord.ForeignID);
        case "DOB":
          return _writerRecord.DOB != default(DateTime) && _dob.Date != _writerRecord.DOB.Date;
        case "Gender":
          return !string.IsNullOrWhiteSpace(_writerRecord.Gender) && NormaliseText(_gender) != NormaliseText(_writerRecord.Gender);
        default:
          return false;
      }
    }

    public bool CanApplyCompositValue(string field)
    {
      if (_compositRecord == null || string.IsNullOrWhiteSpace(field))
        return false;

      switch (field)
      {
        case "Name":
          return !string.IsNullOrWhiteSpace(_compositRecord.Name) && NormaliseText(_myname) != NormaliseText(_compositRecord.Name);
        case "Surname":
          return !string.IsNullOrWhiteSpace(_compositRecord.Surname) && NormaliseText(_surname) != NormaliseText(_compositRecord.Surname);
        case "Reference":
          return _compositRecord.RefNo != 0 && NormaliseText(_NBT) != NormaliseText(_compositRecord.RefNo.ToString());
        case "SAID":
          return _compositRecord.SAID.HasValue && NormaliseText(_said) != NormaliseText(_compositRecord.SAID.Value.ToString("D13"));
        case "ForeignID":
          return !string.IsNullOrWhiteSpace(_compositRecord.ForeignID) && NormaliseText(_foreignID) != NormaliseText(_compositRecord.ForeignID);
        case "DOB":
          return _compositRecord.DOB != default(DateTime) && _dob.Date != _compositRecord.DOB.Date;
        case "Gender":
          return !string.IsNullOrWhiteSpace(_compositRecord.Gender) && NormaliseText(_gender) != NormaliseText(_compositRecord.Gender);
        default:
          return false;
      }
    }

    public bool IsWriterValueDifferent(string field)
    {
      if (_writerRecord == null || string.IsNullOrWhiteSpace(field))
        return false;

      switch (field)
      {
        case "Name":
          return AreDifferent(_myname, _writerRecord.Name);
        case "Surname":
          return AreDifferent(_surname, _writerRecord.Surname);
        case "Reference":
          return AreDifferent(_NBT, _writerRecord.NBT == 0 ? "" : _writerRecord.NBT.ToString());
        case "SAID":
          return AreDifferent(_said, _writerRecord.SAID.HasValue ? _writerRecord.SAID.Value.ToString("D13") : "");
        case "ForeignID":
          return AreDifferent(_foreignID, _writerRecord.ForeignID);
        case "DOB":
          return AreDifferentDate(_dob, _writerRecord.DOB);
        case "Gender":
          return AreDifferent(_gender, _writerRecord.Gender);
        default:
          return false;
      }
    }

    public bool IsCompositValueDifferent(string field)
    {
      if (_compositRecord == null || string.IsNullOrWhiteSpace(field))
        return false;

      switch (field)
      {
        case "Name":
          return AreDifferent(_myname, _compositRecord.Name);
        case "Surname":
          return AreDifferent(_surname, _compositRecord.Surname);
        case "Reference":
          return AreDifferent(_NBT, _compositRecord.RefNo == 0 ? "" : _compositRecord.RefNo.ToString());
        case "SAID":
          return AreDifferent(_said, _compositRecord.SAID.HasValue ? _compositRecord.SAID.Value.ToString("D13") : "");
        case "ForeignID":
          return AreDifferent(_foreignID, _compositRecord.ForeignID);
        case "DOB":
          return AreDifferentDate(_dob, _compositRecord.DOB);
        case "Gender":
          return AreDifferent(_gender, _compositRecord.Gender);
        default:
          return false;
      }
    }

    public void AcceptFileValueForWriter(string field)
    {
      if (string.IsNullOrWhiteSpace(field))
        return;

      switch (field)
      {
        case "Name":
          if (_writerRecord != null && AreDifferent(_writerRecord.Name, _myname)) _writerRecord.Name = _myname;
          break;
        case "Surname":
          if (_writerRecord != null && AreDifferent(_writerRecord.Surname, _surname)) _writerRecord.Surname = _surname;
          break;
        case "Reference":
          SyncReferenceValue(_NBT);
          break;
        case "SAID":
          long? saidValue = ToLong(_said);
          if (_writerRecord != null && _writerRecord.SAID != saidValue) _writerRecord.SAID = saidValue;
          break;
        case "ForeignID":
          if (_writerRecord != null && AreDifferent(_writerRecord.ForeignID, _foreignID)) _writerRecord.ForeignID = _foreignID;
          break;
        case "DOB":
          if (_writerRecord != null && _writerRecord.DOB != _dob) _writerRecord.DOB = _dob;
          break;
        case "Gender":
          if (_writerRecord != null && AreDifferent(_writerRecord.Gender, _gender)) _writerRecord.Gender = _gender;
          break;
      }

      SyncWalkInReferenceIfNeeded();
      RevalidateIdentityFields();
    }

    public void AcceptFileValueForComposit(string field)
    {
      if (string.IsNullOrWhiteSpace(field))
        return;

      switch (field)
      {
        case "Name":
          if (_compositRecord != null && AreDifferent(_compositRecord.Name, _myname)) _compositRecord.Name = _myname;
          break;
        case "Surname":
          if (_compositRecord != null && AreDifferent(_compositRecord.Surname, _surname)) _compositRecord.Surname = _surname;
          break;
        case "Reference":
          if (_compositRecord != null)
          {
            long? referenceValue = ToLong(_NBT);
            if (_compositRecord.RefNo != (referenceValue ?? 0))
              _compositRecord.RefNo = referenceValue ?? 0;
          }
          break;
        case "SAID":
          long? saidValue = ToLong(_said);
          if (_compositRecord != null && _compositRecord.SAID != saidValue) _compositRecord.SAID = saidValue;
          break;
        case "ForeignID":
          if (_compositRecord != null && AreDifferent(_compositRecord.ForeignID, _foreignID)) _compositRecord.ForeignID = _foreignID;
          break;
        case "DOB":
          if (_compositRecord != null && _compositRecord.DOB != _dob) _compositRecord.DOB = _dob;
          break;
        case "Gender":
          if (_compositRecord != null && AreDifferent(_compositRecord.Gender, _gender)) _compositRecord.Gender = _gender;
          break;
      }

      SyncWalkInReferenceIfNeeded();
      RevalidateIdentityFields();
    }

    public string GetCurrentValue(string field)
    {
      switch (field)
      {
        case "Name":
          return _myname;
        case "Surname":
          return _surname;
        case "Reference":
          return _NBT;
        case "SAID":
          return _said;
        case "ForeignID":
          return _foreignID;
        case "DOB":
          return _dob == default(DateTime) ? "" : _dob.ToString("yyyy/MM/dd");
        case "Gender":
          return _gender;
        default:
          return "";
      }
    }

    public string GetWriterValue(string field)
    {
      if (_writerRecord == null)
        return "";

      switch (field)
      {
        case "Name":
          return _writerRecord.Name;
        case "Surname":
          return _writerRecord.Surname;
        case "Reference":
          return _writerRecord.NBT == 0 ? "" : _writerRecord.NBT.ToString();
        case "SAID":
          return _writerRecord.SAID.HasValue ? _writerRecord.SAID.Value.ToString("D13") : "";
        case "ForeignID":
          return _writerRecord.ForeignID;
        case "DOB":
          return _writerRecord.DOB == default(DateTime) ? "" : _writerRecord.DOB.ToString("yyyy/MM/dd");
        case "Gender":
          return _writerRecord.Gender;
        default:
          return "";
      }
    }

    public string GetCompositValue(string field)
    {
      if (_compositRecord == null)
        return "";

      switch (field)
      {
        case "Name":
          return _compositRecord.Name;
        case "Surname":
          return _compositRecord.Surname;
        case "Reference":
          return _compositRecord.RefNo == 0 ? "" : _compositRecord.RefNo.ToString();
        case "SAID":
          return _compositRecord.SAID.HasValue ? _compositRecord.SAID.Value.ToString("D13") : "";
        case "ForeignID":
          return _compositRecord.ForeignID;
        case "DOB":
          return _compositRecord.DOB == default(DateTime) ? "" : _compositRecord.DOB.ToString("yyyy/MM/dd");
        case "Gender":
          return _compositRecord.Gender;
        default:
          return "";
      }
    }

    private static void AddLine(List<string> lines, string label, string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
        lines.Add(label + ": " + value.Trim());
    }

    private static string NormaliseText(string value)
    {
      return string.IsNullOrWhiteSpace(value) ? "" : value.Trim().ToUpperInvariant();
    }

    private static bool AreDifferent(string left, string right)
    {
      return NormaliseText(left) != NormaliseText(right);
    }

    private static bool AreDifferentDate(DateTime left, DateTime right)
    {
      bool leftEmpty = left == default(DateTime);
      bool rightEmpty = right == default(DateTime);
      if (leftEmpty && rightEmpty)
        return false;
      if (leftEmpty != rightEmpty)
        return true;
      return left.Date != right.Date;
    }

    private static long? ToLong(string value)
    {
      long parsed;
      if (!string.IsNullOrWhiteSpace(value) && long.TryParse(value.Trim(), out parsed))
        return parsed;
      return null;
    }

    private static bool IsWalkInReference(string reference)
    {
      return !string.IsNullOrWhiteSpace(reference)
             && reference.Trim().Length > 7
             && reference.Trim()[7] == '9';
    }

    private void SyncReferenceValue(string reference)
    {
      string resolved = ResolveReference(reference);
      Reference = resolved;
      long? resolvedValue = ToLong(resolved);
      if (_writerRecord != null && _writerRecord.NBT != (resolvedValue ?? 0))
        _writerRecord.NBT = resolvedValue ?? 0;
      if (_compositRecord != null && _compositRecord.RefNo != (resolvedValue ?? 0))
        _compositRecord.RefNo = resolvedValue ?? 0;
    }

    private string ResolveReference(string preferredReference)
    {
      string candidate = NormaliseText(preferredReference);
      if (candidate.Length > 0 && !IsWalkInReference(candidate))
        return preferredReference;

      string writerReference = _writerRecord == null || _writerRecord.NBT == 0 ? "" : _writerRecord.NBT.ToString();
      if (!IsWalkInReference(writerReference) && !string.IsNullOrWhiteSpace(writerReference))
        return writerReference;

      string compositReference = _compositRecord == null || _compositRecord.RefNo == 0 ? "" : _compositRecord.RefNo.ToString();
      if (!IsWalkInReference(compositReference) && !string.IsNullOrWhiteSpace(compositReference))
        return compositReference;

      return preferredReference;
    }

    private void SyncWalkInReferenceIfNeeded()
    {
      if (!IsWalkInReference(_NBT))
        return;

      string resolved = ResolveReference(_NBT);
      if (string.IsNullOrWhiteSpace(resolved) || NormaliseText(resolved) == NormaliseText(_NBT))
        return;

      Reference = resolved;
      if (_writerRecord != null)
        _writerRecord.NBT = ToLong(resolved) ?? 0;
      if (_compositRecord != null)
        _compositRecord.RefNo = ToLong(resolved) ?? 0;
    }

    private void RevalidateIdentityFields()
    {
      ValidateReference();
      ValidateFirstName();
      ValidateSurname();
      ValidateSAID();
      ValidateForeignID();
      ValidateDOB();
    }

    private void ValidateReference()
    {
      List<string> errors = new List<string>();
      string reference = (_NBT ?? "").Trim();
      if (string.IsNullOrWhiteSpace(reference))
      {
        errors.Add("NBT number cannot be empty");
      }
      else
      {
        if (reference.Length != 14)
          errors.Add("Not proper length for NBT number");
        else if (!HelperUtils.IsValidChecksum(reference.Substring(1, 13)))
          errors.Add("Not a Valid NBT number");

        if (HasAttachedTextMismatch(reference, _writerRecord == null ? null : _writerRecord.NBT.ToString(), _compositRecord == null ? null : _compositRecord.RefNo.ToString()))
          errors.Add("Reference differs from database");
      }

      ApplyErrors("Reference", errors);
    }

    private void ValidateFirstName()
    {
      List<string> errors = new List<string>();
      string firstName = (_myname ?? "").Trim();
      if (string.IsNullOrEmpty(firstName))
      {
        errors.Add("FirstName cannot be empty");
      }
      else
      {
        if (Regex.IsMatch(firstName, "\\d"))
          errors.Add("First name cannot have digits");
        else if (!Regex.IsMatch(firstName, "^[^\\s=!@#](?:[^!@#;'`èëéáàãíìïòôöúüç©]*[^\\s!@#])?$"))
          errors.Add("cannot start/end with space or have funny characters");
        else if (firstName.Length > 18)
          errors.Add("To many characters for Name (max is 18)");
        else if (firstName.Contains("é"))
          errors.Add("cannot have funny characters");

        if (HasAttachedTextMismatch(firstName, _writerRecord == null ? null : _writerRecord.Name, _compositRecord == null ? null : _compositRecord.Name))
          errors.Add("Name differs from database");
      }

      ApplyErrors("FirstName", errors);
    }

    private void ValidateSurname()
    {
      List<string> errors = new List<string>();
      string surname = (_surname ?? "").Trim();
      if (string.IsNullOrEmpty(surname))
      {
        errors.Add("Surname cannot be empty");
      }
      else
      {
        if (Regex.IsMatch(surname, "\\d"))
          errors.Add("Surname cannot have digits");
        else if (!Regex.IsMatch(surname, "^[^\\s=!@#](?:[^!@#;'`èëéáàãíìïòôöúüç©]*[^\\s!@#])?$"))
          errors.Add("cannot start/end with space or have funny characters");
        else if (surname.Length > 30)
          errors.Add("Too many characters for Surname");
        else if (surname.Contains("é"))
          errors.Add("cannot have funny characters");

        if (HasAttachedTextMismatch(surname, _writerRecord == null ? null : _writerRecord.Surname, _compositRecord == null ? null : _compositRecord.Surname))
          errors.Add("Surname differs from database");
      }

      ApplyErrors("Surname", errors);
    }

    private void ValidateSAID()
    {
      List<string> errors = new List<string>();
      string said = (_said ?? "").Trim();
      if (!string.IsNullOrWhiteSpace(said))
      {
        if (!Regex.IsMatch(said, "^[0-9]+$"))
          errors.Add("SA Id must contain only digits");
        else if (said.Length != 13)
          errors.Add("SA Id must be 13 digits");
        else if (!HelperUtils.IsValidSAIDDateOfBirth(said))
          errors.Add("SA Id date of birth is not valid");
        else if (!HelperUtils.IsValidSAIDCitizenshipDigit(said))
          errors.Add("SA Id citizenship digit (11th) must be 0 or 1");
        else if (!HelperUtils.IsValidSAIDChecksum(said))
          errors.Add("SA Id check digit is not valid");

        string writerSaid = _writerRecord != null && _writerRecord.SAID.HasValue ? _writerRecord.SAID.Value.ToString("D13") : null;
        string compositSaid = _compositRecord != null && _compositRecord.SAID.HasValue ? _compositRecord.SAID.Value.ToString("D13") : null;
        if (HasAttachedTextMismatch(said, writerSaid, compositSaid))
          errors.Add("SA ID differs from database");
      }

      ApplyErrors("SAID", errors);
    }

    private void ValidateForeignID()
    {
      List<string> errors = new List<string>();
      string foreignId = (_foreignID ?? "").Trim();
      if (!string.IsNullOrWhiteSpace(foreignId))
      {
        if (foreignId.Length > 15)
          errors.Add("ForeignID has too many characters");

        if (HasAttachedTextMismatch(foreignId, _writerRecord == null ? null : _writerRecord.ForeignID, _compositRecord == null ? null : _compositRecord.ForeignID))
          errors.Add("Foreign ID differs from database");
      }

      ApplyErrors("ForeignID", errors);
    }

    private void ValidateDOB()
    {
      List<string> errors = new List<string>();
      DateTime today = DateTime.Today;
      if (_dob != default(DateTime) && (_dob.Date > today.AddYears(-10) || _dob.Date < today.AddYears(-99)))
        errors.Add("Wrong age for Matric");

      if (HasAttachedDateMismatch(_dob, _writerRecord == null ? (DateTime?)null : _writerRecord.DOB, _compositRecord == null ? (DateTime?)null : _compositRecord.DOB))
        errors.Add("Date of birth differs from database");

      ApplyErrors("DOB", errors);
    }

    private void ApplyErrors(string propertyName, IEnumerable<string> errors)
    {
      List<string> list = errors == null ? new List<string>() : errors.Where((string value) => !string.IsNullOrWhiteSpace(value)).ToList();
      if (list.Count > 0)
        AddError(propertyName, string.Join("; ", list));
      else
        RemoveError(propertyName);
      checkerrors();
    }

    private static bool MatchesText(string left, string right)
    {
      string a = string.IsNullOrWhiteSpace(left) ? "" : left.Trim().ToUpperInvariant();
      string b = string.IsNullOrWhiteSpace(right) ? "" : right.Trim().ToUpperInvariant();
      if (a.Length == 0 || b.Length == 0)
        return true;
      return a == b;
    }

    private static bool MatchesDate(DateTime left, DateTime right)
    {
      if (left == default(DateTime) || right == default(DateTime))
        return true;
      return left.Date == right.Date;
    }

    private static bool HasAttachedTextMismatch(string value, string writerValue, string compositValue)
    {
      bool writerMatches = string.IsNullOrWhiteSpace(writerValue) || MatchesText(value, writerValue);
      bool compositMatches = string.IsNullOrWhiteSpace(compositValue) || MatchesText(value, compositValue);
      return !writerMatches && !compositMatches;
    }

    private static bool HasAttachedDateMismatch(DateTime value, DateTime? writerValue, DateTime? compositValue)
    {
      bool writerMatches = !writerValue.HasValue || MatchesDate(value, writerValue.Value);
      bool compositMatches = !compositValue.HasValue || MatchesDate(value, compositValue.Value);
      return !writerMatches && !compositMatches;
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
