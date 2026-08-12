// Decompiled with JetBrains decompiler
// Type: LOB.BDO.VenueBDO
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;
using System;
using Microsoft.SqlServer.Types;
using System.Data.Entity.Spatial;

namespace CETAP_LOB.BDO
{
  public class VenueBDO : ModelBase
  {
    public const string VenueCodePropertyName = "VenueCode";
    public const string VenueNamePropertyName = "VenueName";
    public const string ShortNamePropertyName = "ShortName";
    public const string WebSiteNamePropertyName = "WebSiteName";
    public const string RoomPropertyName = "Room";
    public const string VenueTypePropertyName = "VenueType";
    public const string ProvinceIDPropertyName = "ProvinceID";
    public const string AvailablePropertyName = "Available";
    public const string CapacityPropertyName = "Capacity";
    public const string DescriptionPropertyName = "Description";
    private int _code;
    private string _venue;
    private string _sname;
    private string _webname;
    private string _room;
    private string _venueType;
    private int _province;
    private bool _available;
    private int? _capacity;
    private string _comments;

    public byte[] RowVersion { get; set; }

    public Guid RowGuid { get; set; }

    public DateTime DateModified { get; set; }

    public DbGeography Place { get; set; }

    public int VenueCode
    {
      get
      {
        return _code;
      }
      set
      {
        if (_code == value)
          return;
        _code = value;
        if (_code < 1)
          AddError("VenueCode", "Venue Codes should be positive numbers");
        else
          RemoveError("VenueCode");
        RaisePropertyChanged("VenueCode");
      }
    }

    public string VenueName
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
        if (string.IsNullOrWhiteSpace(_venue))
          AddError("VenueName", "Venue Name is required");
        else
          RemoveError("VenueName");
        RaisePropertyChanged("VenueName");
      }
    }

    public string ShortName
    {
      get
      {
        return _sname;
      }
      set
      {
        if (_sname == value)
          return;
        _sname = value;
        if (string.IsNullOrWhiteSpace(_venue))
          AddError("ShortName", "Venue Name is required");
        else
          RemoveError("ShortName");
        RaisePropertyChanged("ShortName");
      }
    }

    public string WebSiteName
    {
      get
      {
        return _webname;
      }
      set
      {
        if (_webname == value)
          return;
        _webname = value;
        RaisePropertyChanged("WebSiteName");
      }
    }

    public string Room
    {
      get
      {
        return _room;
      }
      set
      {
        if (_room == value)
          return;
        _room = value;
        RaisePropertyChanged("Room");
      }
    }

    public string VenueType
    {
      get
      {
        return _venueType;
      }
      set
      {
        if (_venueType == value)
          return;
        _venueType = value;
        RaisePropertyChanged("VenueType");
      }
    }

    public int ProvinceID
    {
      get
      {
        return _province;
      }
      set
      {
        if (_province == value)
          return;
        _province = value;
        RaisePropertyChanged("ProvinceID");
      }
    }

    public bool Available
    {
      get
      {
        return _available;
      }
      set
      {
        if (_available == value)
          return;
        _available = value;
        RaisePropertyChanged("Available");
      }
    }

    public int? Capacity
    {
      get
      {
        return _capacity;
      }
      set
      {
        int? capacity = _capacity;
        int? nullable = value;
        if ((capacity.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (capacity.HasValue == nullable.HasValue ? 1 : 0)) != 0)
          return;
        _capacity = value;
        RaisePropertyChanged("Capacity");
      }
    }

    public string Description
    {
      get
      {
        return _comments;
      }
      set
      {
        if (_comments == value)
          return;
        _comments = value;
        RaisePropertyChanged("Description");
      }
    }
  }
}
