// Decompiled with JetBrains decompiler
// Type: LOB.BDO.TestAllocationBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;
using System;

namespace CETAP_LOB.BDO
{
  public class TestAllocationBDO : ModelBase
  {
    public const string TestDatePropertyName = "TestDate";
    public const string ClientTypePropertyName = "ClientType";
    public const string TestIDPropertyName = "TestID";
    public const string TestNamePropertyName = "TestName";
    public const string ClientPropertyName = "Client";
    public const string EstimatedPropertyName = "Estimated";
    public const string ActualUsedPropertyName = "ActualUsed";
    private DateTime _testDate;
    private string _clientType;
    private int _testID;
    private string _mytestName;
    private string _client;
    private int _estimated;
    private int _actualUsed;

    public int ID { get; set; }

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
      }
    }

    public string ClientType
    {
      get
      {
        return _clientType;
      }
      set
      {
        if (_clientType == value)
          return;
        _clientType = value;
        RaisePropertyChanged("ClientType");
      }
    }

    public int TestID
    {
      get
      {
        return _testID;
      }
      set
      {
        if (_testID == value)
          return;
        _testID = value;
        RaisePropertyChanged("TestID");
      }
    }

    public string TestName
    {
      get
      {
        return _mytestName;
      }
      set
      {
        if (_mytestName == value)
          return;
        _mytestName = value;
        RaisePropertyChanged("TestName");
      }
    }

    public string Client
    {
      get
      {
        return _client;
      }
      set
      {
        if (_client == value)
          return;
        _client = value;
        RaisePropertyChanged("Client");
      }
    }

    public int Estimated
    {
      get
      {
        return _estimated;
      }
      set
      {
        if (_estimated == value)
          return;
        _estimated = value;
        RaisePropertyChanged("Estimated");
      }
    }

    public int ActualUsed
    {
      get
      {
        return _actualUsed;
      }
      set
      {
        if (_actualUsed == value)
          return;
        _actualUsed = value;
        RaisePropertyChanged("ActualUsed");
      }
    }

    public Guid RowGuid { get; set; }

    public DateTime DateModified { get; set; }

    public byte[] RowVersion { get; set; }

    public override string ToString()
    {
      return TestDate.ToShortDateString() + " " + Client + " Amount : " + (object) Estimated;
    }
  }
}
