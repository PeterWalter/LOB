// Decompiled with JetBrains decompiler
// Type: LOB.Model.easypay.PayRecord
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;

namespace CETAP_LOB.Model.easypay
{
  public class PayRecord
  {
    private double amount;
    private string _Id;
    private long _nbt;
    private string _date;
    private string _time;
    private string _xRecord;
    private string _pRecord;
    private string myRecord;

    public long NBT
    {
      get
      {
        return _nbt;
      }
      set
      {
        _nbt = value;
      }
    }

    public double Amount
    {
      get
      {
        return amount;
      }
      set
      {
        amount = value;
      }
    }

    public string ID
    {
      get
      {
        return _Id;
      }
      set
      {
        _Id = value;
      }
    }

    public string Record
    {
      get
      {
        CreateXrecord();
        CreateTPrecord();
        myRecord = _xRecord + _pRecord;
        return myRecord;
      }
    }

    private void CreateXrecord()
    {
      long num = 2630099999;
      DateTime now = DateTime.Now;
      _date = now.ToString("yyyyMMdd");
      _time = now.ToString("HHMMss");
      _xRecord = "X," + (object) num + "," + _date + "," + _time + ",0263," + _Id;
      _xRecord += Environment.NewLine;
    }

    private void CreateTPrecord()
    {
      string str1 = "P,";
      string str2 = string.Format("{0,10:######0.00}", (object) amount);
      string str3 = str1 + str2;
      string str4 = string.Format("{0,10:######0.00}", (object) 0.0);
      _pRecord = str3 + "," + str4 + "," + (object) _nbt + Environment.NewLine + ("T," + str2 + "," + str4 + ",Cash" + Environment.NewLine);
    }
  }
}
