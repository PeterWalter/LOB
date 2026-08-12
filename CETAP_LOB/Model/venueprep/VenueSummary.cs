

using GalaSoft.MvvmLight;
using CETAP_LOB.Helper;
using System;

namespace CETAP_LOB.Model.venueprep
{
  public class VenueSummary : ObservableObject
  {
    private int _walkin_e = 10;
    private int _walkin_a = 10;
    public const string TestDatePropertyName = "TestDate";
    public const string VenuePropertyName = "Venue";
    public const string TotalWritersPropertyName = "TotalWriters";
    public const string CentreCodePropertyName = "CentreCode";
    public const string AQL_EPropertyName = "AQL_E";
    public const string Walkin_EPropertyName = "Walkin_E";
    public const string AQLE_SPropertyName = "AQLE_S";
    public const string AQLE_BatchPropertyName = "AQLE_Batch";
    public const string Maths_EPropertyName = "Maths_E";
    public const string MATE_SPropertyName = "MATE_S";
    public const string MATE_BatchPropertyName = "MATE_Batch";
    public const string AQL_APropertyName = "AQL_A";
    public const string Walkin_APropertyName = "Walkin_A";
    public const string AQLA_SPropertyName = "AQLA_S";
    public const string AQLA_BatchPropertyName = "AQLA_Batch";
    public const string Maths_APropertyName = "Maths_A";
    public const string MATA_SPropertyName = "MATA_S";
    public const string MATA_BatchPropertyName = "MATA_Batch";
    public const string TotalSentPropertyName = "TotalSent";
    private DateTime _mytestDate;
    private string _venue;
    private int _writers;
    private int _vcode;
    private int _aql_e;
    private int _aqle_s;
    private string _aqle_b;
    private int _maths_e;
    private int _mate_s;
    private string _mate_batch;
    private int _aql_a;
    private int _aqla_s;
    private string _aqla_b;
    private int _math_a;
    private int _mata_s;
    private string _mata_batch;
    private int _totalsent;

    public DateTime TestDate
    {
      get
      {
        return _mytestDate;
      }
      set
      {
        if (_mytestDate == value)
          return;
        _mytestDate = value;
        RaisePropertyChanged("TestDate");
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

    public int TotalWriters
    {
      get
      {
        return _writers;
      }
      set
      {
        if (_writers == value)
          return;
        _writers = value;
        RaisePropertyChanged("TotalWriters");
      }
    }

    public int CentreCode
    {
      get
      {
        return _vcode;
      }
      set
      {
        if (_vcode == value)
          return;
        _vcode = value;
        RaisePropertyChanged("CentreCode");
      }
    }

    public int AQL_E
    {
      get
      {
        return _aql_e;
      }
      set
      {
        if (_aql_e == value)
          return;
        _aql_e = value;
        RaisePropertyChanged("AQL_E");
        _aqle_s = HelperUtils.RoundAmount(_aql_e);
        RaisePropertyChanged("AQLE_S");
      }
    }

    public int Walkin_E
    {
      get
      {
        return _walkin_e;
      }
      set
      {
        if (_walkin_e == value)
          return;
        if (_aql_e == 0)
          value = 0;
        _walkin_e = value;
        RaisePropertyChanged("Walkin_E");
      }
    }

    public int AQLE_S
    {
      get
      {
        return _aqle_s;
      }
      set
      {
        if (_aqle_s == value)
          return;
        _aqle_s = value;
        RaisePropertyChanged("AQLE_S");
        AQLE_Batch = HelperUtils.YourChange(_aqle_s);
        RaisePropertyChanged("AQLE_Batch");
        _totalsent = _aqle_s + _aqla_s;
        RaisePropertyChanged("TotalSent");
      }
    }

    public string AQLE_Batch
    {
      get
      {
        return _aqle_b;
      }
      set
      {
        if (_aqle_b == value)
          return;
        _aqle_b = value;
        RaisePropertyChanged("AQLE_Batch");
      }
    }

    public int Maths_E
    {
      get
      {
        return _maths_e;
      }
      set
      {
        if (_maths_e == value)
          return;
        _maths_e = value;
        _mate_s = HelperUtils.RoundAmount(_maths_e);
        RaisePropertyChanged("Maths_E");
        RaisePropertyChanged("MATE_S");
      }
    }

    public int MATE_S
    {
      get
      {
        return _mate_s;
      }
      set
      {
        if (_mate_s == value)
          return;
        _mate_s = value;
        _mate_batch = HelperUtils.YourChange(_mate_s);
        RaisePropertyChanged("MATE_S");
        RaisePropertyChanged("MATE_Batch");
      }
    }

    public string MATE_Batch
    {
      get
      {
        return _mate_batch;
      }
      set
      {
        if (_mate_batch == value)
          return;
        _mate_batch = value;
        RaisePropertyChanged("MATE_Batch");
      }
    }

    public int AQL_A
    {
      get
      {
        return _aql_a;
      }
      set
      {
        if (_aql_a == value)
          return;
        _aql_a = value;
        _aqla_s = HelperUtils.RoundAmount(_aql_a);
        RaisePropertyChanged("AQL_A");
        RaisePropertyChanged("AQLA_S");
      }
    }

    public int Walkin_A
    {
      get
      {
        return _walkin_a;
      }
      set
      {
        if (_walkin_a == value)
          return;
        if (_aql_a == 0)
          value = 0;
        _walkin_a = value;
        RaisePropertyChanged("Walkin_A");
      }
    }

    public int AQLA_S
    {
      get
      {
        return _aqla_s;
      }
      set
      {
        if (_aqla_s == value)
          return;
        _aqla_s = value;
        _totalsent = _aqle_s + _aqla_s;
        _aqla_b = HelperUtils.YourChange(_aqla_s);
        RaisePropertyChanged("AQLA_S");
        RaisePropertyChanged("AQLA_Batch");
        RaisePropertyChanged("TotalSent");
      }
    }

    public string AQLA_Batch
    {
      get
      {
        return _aqla_b;
      }
      set
      {
        if (_aqla_b == value)
          return;
        _aqla_b = value;
        RaisePropertyChanged("AQLA_Batch");
      }
    }

    public int Maths_A
    {
      get
      {
        return _math_a;
      }
      set
      {
        if (_math_a == value)
          return;
        _math_a = value;
        _mata_s = HelperUtils.RoundAmount(_math_a);
        RaisePropertyChanged("Maths_A");
        RaisePropertyChanged("MATA_S");
      }
    }

    public int MATA_S
    {
      get
      {
        return _mata_s;
      }
      set
      {
        if (_mata_s == value)
          return;
        _mata_s = value;
        _mata_batch = HelperUtils.YourChange(_mata_s);
        RaisePropertyChanged("MATA_S");
        RaisePropertyChanged("MATA_Batch");
      }
    }

    public string MATA_Batch
    {
      get
      {
        return _mata_batch;
      }
      set
      {
        if (_mata_batch == value)
          return;
        _mata_batch = value;
        RaisePropertyChanged("MATA_Batch");
      }
    }

    public int TotalSent
    {
      get
      {
        return _totalsent;
      }
      set
      {
        if (_totalsent == value)
          return;
        _totalsent = value;
        RaisePropertyChanged("TotalSent");
      }
    }
  }
}
