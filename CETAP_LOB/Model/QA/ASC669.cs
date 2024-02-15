// Decompiled with JetBrains decompiler
// Type: LOB.Model.QA.ASC669
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FileHelpers;

namespace CETAP_LOB.Model.QA
{
  [IgnoreLast(1)]
  [FixedLengthRecord(FixedMode.ExactLength)]
  public sealed class ASC669
  {
    [FieldFixedLength(3)]
    private string mCSX_Number;
    [FieldFixedLength(37)]
    private string mCSX;
    [FieldFixedLength(14)]
    private string mNBT;
    [FieldFixedLength(12)]
    private string mSessionID;
    [FieldFixedLength(13)]
    private string mSAID;
    [FieldFixedLength(15)]
    private string mForeignID;
    [FieldFixedLength(1)]
    private string mGender;
    [FieldFixedLength(1)]
    private string mIDType;
    [FieldFixedLength(2)]
    private string mHomeLanguage;
    [FieldFixedLength(1)]
    private string mClassification;
    [FieldFixedLength(10)]
    private string mContactNo;
    [FieldFixedLength(2)]
    private string mHomeProvince;
    [FieldFixedLength(8)]
    private string mDOT;
    [FieldFixedLength(1)]
    private string mHomeType;
    [FieldFixedLength(1)]
    private string mHelectricity;
    [FieldFixedLength(1)]
    private string mSchool10Km;
    [FieldFixedLength(1)]
    private string mHomestudy;
    [FieldFixedLength(1)]
    private string mSiblingsAttendedUni;
    [FieldFixedLength(1)]
    private string mSiblingsAtUni;
    [FieldFixedLength(1)]
    private string mVenueTime;
    [FieldFixedLength(1)]
    private string mVenueCost;
    [FieldFixedLength(1)]
    private string mVenueDist;
    [FieldFixedLength(1)]
    private string mVenueModeOfTransport;
    [FieldFixedLength(1)]
    private string mSchComputers;
    [FieldFixedLength(1)]
    private string mSchLibrary;
    [FieldFixedLength(1)]
    private string mSchLabs;
    [FieldFixedLength(1)]
    private string mSchElectricity;
    [FieldFixedLength(1)]
    private string mSchWater;
    [FieldFixedLength(1)]
    private string mSchHall;
    [FieldFixedLength(1)]
    private string mSchFields;
    [FieldFixedLength(1)]
    private string mSchHostel;
    [FieldFixedLength(1)]
    private string mUsedComputers;
    [FieldFixedLength(1)]
    private string mUsedLibrary;
    [FieldFixedLength(1)]
    private string mUsedLabs;
    [FieldFixedLength(1)]
    private string mUsedElectricity;
    [FieldFixedLength(1)]
    private string mUsedRWater;
    [FieldFixedLength(1)]
    private string mUsedHall;
    [FieldFixedLength(1)]
    private string mUsedFields;
    [FieldFixedLength(1)]
    private string mUsedHostel;
    [FieldFixedLength(44)]
    private string mSchoolName;
    [FieldFixedLength(4)]
    private string mYrGr12;
    [FieldFixedLength(4)]
    private string mPostalCode;
    [FieldFixedLength(2)]
    private string mSchProvince;
    [FieldFixedLength(1)]
    private string mGr12Language;
    [FieldFixedLength(1)]
    private string mSchType;
    [FieldFixedLength(1)]
    private string mFaculty1;
    [FieldFixedLength(1)]
    private string mFaculty2;
    [FieldFixedLength(1)]
    private string mFaculty3;
    [FieldFixedLength(1)]
    private string mEOL;

    public string CSX_Number
    {
      get
      {
        return mCSX_Number;
      }
      set
      {
        mCSX_Number = value;
      }
    }

    public string CSX
    {
      get
      {
        return mCSX;
      }
      set
      {
        mCSX = value;
      }
    }

    public string NBT
    {
      get
      {
        return mNBT;
      }
      set
      {
        mNBT = value;
      }
    }

    public string SessionID
    {
      get
      {
        return mSessionID;
      }
      set
      {
        mSessionID = value;
      }
    }

    public string SAID
    {
      get
      {
        return mSAID;
      }
      set
      {
        mSAID = value;
      }
    }

    public string ForeignID
    {
      get
      {
        return mForeignID;
      }
      set
      {
        mForeignID = value;
      }
    }

    public string Gender
    {
      get
      {
        return mGender;
      }
      set
      {
        mGender = value;
      }
    }

    public string IDType
    {
      get
      {
        return mIDType;
      }
      set
      {
        mIDType = value;
      }
    }

    public string HomeLanguage
    {
      get
      {
        return mHomeLanguage;
      }
      set
      {
        mHomeLanguage = value;
      }
    }

    public string Classification
    {
      get
      {
        return mClassification;
      }
      set
      {
        mClassification = value;
      }
    }

    public string ContactNo
    {
      get
      {
        return mContactNo;
      }
      set
      {
        mContactNo = value;
      }
    }

    public string HomeProvince
    {
      get
      {
        return mHomeProvince;
      }
      set
      {
        mHomeProvince = value;
      }
    }

    public string DOT
    {
      get
      {
        return mDOT;
      }
      set
      {
        mDOT = value;
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

    public string Gr12Language
    {
      get
      {
        return mGr12Language;
      }
      set
      {
        mGr12Language = value;
      }
    }

    public string SchType
    {
      get
      {
        return mSchType;
      }
      set
      {
        mSchType = value;
      }
    }

    public string Faculty1
    {
      get
      {
        return mFaculty1;
      }
      set
      {
        mFaculty1 = value;
      }
    }

    public string Faculty2
    {
      get
      {
        return mFaculty2;
      }
      set
      {
        mFaculty2 = value;
      }
    }

    public string Faculty3
    {
      get
      {
        return mFaculty3;
      }
      set
      {
        mFaculty3 = value;
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
  }
}
