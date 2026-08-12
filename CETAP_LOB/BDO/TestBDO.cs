// Decompiled with JetBrains decompiler
// Type: LOB.BDO.TestBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;
using System;

namespace CETAP_LOB.BDO
{
  public class TestBDO : ModelBase
  {
    public const string TestNamePropertyName = "TestName";
    private string _testName;

    public int TestID { get; set; }

    public byte[] RowVersion { get; set; }

    public string TestName
    {
      get
      {
        return _testName;
      }
      set
      {
        if (_testName == value)
          return;
        _testName = value;
        RaisePropertyChanged("TestName");
      }
    }

    public int TestCode { get; set; }

    public short Section7 { get; set; }

    public new bool HasErrors { get; set; }

    public string Description { get; set; }

    public Guid rowguid { get; set; }

    public DateTime DateModified { get; set; }

    public override string ToString()
    {
      return TestName;
    }
  }
}
