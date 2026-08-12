

using CETAP_LOB.Model;
using System;

namespace CETAP_LOB.BDO
{
  public class BatchBDO : ModelBase
  {
    public const string errorCountPropertyName = "errorCount";
    public const string BatchNamePropertyName = "BatchName";
    public const string TestDatePropertyName = "TestDate";
    public const string TestCombinationPropertyName = "TestCombination";
    public const string BatchedByPropertyName = "BatchedBy";
    public const string BatchDatePropertyName = "BatchDate";
    public const string RandomTestNumberPropertyName = "RandomTestNumber";
    public const string CountPropertyName = "Count";
    public const string TestVenueIDPropertyName = "TestVenueID";
    public const string TestProfileIDPropertyName = "TestProfileID";
    public const string DescriptionPropertyName = "Description";
    private int _myecount;
    private string _mybatch;
    private DateTime _testDate;
    private string _testCombination;
    private string _batchedBy;
    private DateTime _batchDate;
    private int _randNumber;
    private int _mycount;
    private int _venueID;
    private int _myProfileID;
    private string _desc;

    public int BatchID { get; set; }

    public int errorCount
    {
      get
      {
        return _myecount;
      }
      set
      {
        if (_myecount == value)
          return;
        _myecount = value;
        RaisePropertyChanged("errorCount");
      }
    }

    public string BatchName
    {
      get
      {
        return _mybatch;
      }
      set
      {
        if (_mybatch == value)
          return;
        _mybatch = value;
        if (string.IsNullOrWhiteSpace(_mybatch))
          AddError("BatchName", "Batch Name is required");
        else
          RemoveError("BatchName");
        checkerrors();
        RaisePropertyChanged("BatchName");
      }
    }

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
        if (_testDate > DateTime.Now)
          AddError("TestDate", "Impossible date, test already written");
        else
          RemoveError("TestDate");
        checkerrors();
        RaisePropertyChanged("TestDate");
      }
    }

    public string TestCombination
    {
      get
      {
        return _testCombination;
      }
      set
      {
        if (_testCombination == value)
          return;
        _testCombination = value;
        RaisePropertyChanged("TestCombination");
      }
    }

    public string BatchedBy
    {
      get
      {
        return _batchedBy;
      }
      set
      {
        if (_batchedBy == value)
          return;
        _batchedBy = value;
        RaisePropertyChanged("BatchedBy");
      }
    }

    public DateTime BatchDate
    {
      get
      {
        return _batchDate;
      }
      set
      {
        if (_batchDate == value)
          return;
        _batchDate = value;
        RaisePropertyChanged("BatchDate");
      }
    }

    public int RandomTestNumber
    {
      get
      {
        return _randNumber;
      }
      set
      {
        if (_randNumber == value)
          return;
        _randNumber = value;
        RaisePropertyChanged("RandomTestNumber");
      }
    }

    public int Count
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
        RaisePropertyChanged("Count");
      }
    }

    public int TestVenueID
    {
      get
      {
        return _venueID;
      }
      set
      {
        if (_venueID == value)
          return;
        _venueID = value;
        RaisePropertyChanged("TestVenueID");
      }
    }

    public int TestProfileID
    {
      get
      {
        return _myProfileID;
      }
      set
      {
        if (_myProfileID == value)
          return;
        _myProfileID = value;
        RaisePropertyChanged("TestProfileID");
      }
    }

    public string Description
    {
      get
      {
        return _desc;
      }
      set
      {
        if (_desc == value)
          return;
        _desc = value;
        RaisePropertyChanged("Description");
      }
    }

    public DateTime DateModified { get; set; }

    public byte[] RowVersion { get; set; }

    public override string ToString()
    {
      return BatchName;
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
