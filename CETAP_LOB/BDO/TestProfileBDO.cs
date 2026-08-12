// Decompiled with JetBrains decompiler
// Type: LOB.BDO.TestProfileBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;
using System;

namespace CETAP_LOB.BDO
{
  public class TestProfileBDO : ModelBase
  {
    public const string ProfilePropertyName = "Profile";
    public const string IntakePropertyName = "Intake";
    public const string AllocationIDPropertyName = "AllocationID";
    public const string ModifiedDatePropertyName = "ModifiedDate";
    private int _profile;
    private int _myIntake;
    private int _myAllocID;
    private DateTime _mDate;

    public int ProfileID { get; set; }

    public int Profile
    {
      get
      {
        return _profile;
      }
      set
      {
        if (_profile == value)
          return;
        _profile = value;
        RaisePropertyChanged("Profile");
      }
    }

    public int Intake
    {
      get
      {
        return _myIntake;
      }
      set
      {
        if (_myIntake == value)
          return;
        _myIntake = value;
        RaisePropertyChanged("Intake");
      }
    }

    public int AllocationID
    {
      get
      {
        return _myAllocID;
      }
      set
      {
        if (_myAllocID == value)
          return;
        _myAllocID = value;
        RaisePropertyChanged("AllocationID");
      }
    }

    public Guid RowGuid { get; set; }

    public DateTime ModifiedDate
    {
      get
      {
        return _mDate;
      }
      set
      {
        if (_mDate == value)
          return;
        _mDate = value;
        RaisePropertyChanged("ModifiedDate");
      }
    }

    public byte[] RowVersion { get; set; }

    public override string ToString()
    {
      return Profile.ToString();
    }
  }
}
