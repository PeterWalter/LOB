

using GalaSoft.MvvmLight;
using CETAP_LOB.BDO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Spatial;

namespace CETAP_LOB.ViewModel.writers
{
  public class VenueViewModel : ViewModelBase, INotifyDataErrorInfo
  {
    public Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
    public const string IsDirtyPropertyName = "IsDirty";
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
    private bool _isDirty;
    private int _code;
    private string _venue;
    private string _sname;
    private string _webname;
    private string _room;
    private string _venueType;
    private int? _province;
    private bool _available;
    private int? _capacity;
    private string _comments;

    public bool IsDirty
    {
      get
      {
        return _isDirty;
      }
      set
      {
        if (_isDirty == value)
          return;
        bool isDirty = _isDirty;
        _isDirty = value;
        RaisePropertyChanged<bool>("IsDirty", isDirty, value, true);
      }
    }

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
        int code = _code;
        _code = value;
        IsDirty = true;
        if (_code <= 0)
          AddError("VenueCode", "Venue Codes should be positive numbers");
        else
          RemoveError("VenueCode");
        RaisePropertyChanged<int>("VenueCode", code, value, true);
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
        string venue = _venue;
        _venue = value;
        IsDirty = true;
        RaisePropertyChanged<string>("VenueName", venue, value, true);
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
        IsDirty = true;
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
        IsDirty = true;
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
        IsDirty = true;
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
        IsDirty = true;
        RaisePropertyChanged("VenueType");
      }
    }

    public int? ProvinceID
    {
      get
      {
        return _province;
      }
      set
      {
        int? province = _province;
        int? nullable = value;
        if ((province.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (province.HasValue == nullable.HasValue ? 1 : 0)) != 0)
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

    public DateTime DateModified { get; set; }

    public DbGeography Place { get; set; }

    public VenueBDO Model { get; private set; }

    public bool HasErrors
    {
      get
      {
        return _errors.Count > 0;
      }
    }

    public bool IsValid
    {
      get
      {
        return !HasErrors;
      }
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public VenueViewModel(VenueBDO model)
    {
      Model = model;
    }

    public IEnumerable GetErrors(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
        return (IEnumerable) null;
      return (IEnumerable) _errors[propertyName];
    }

    public void AddError(string propertyName, string error)
    {
      _errors[propertyName] = new List<string>()
      {
        error
      };
      NotifyErrorsChanged(propertyName);
    }

    public void RemoveError(string propertyName)
    {
      if (_errors.ContainsKey(propertyName))
        _errors.Remove(propertyName);
      NotifyErrorsChanged(propertyName);
    }

    private void NotifyErrorsChanged(string propertyName)
    {
      if (ErrorsChanged == null)
        return;
      ErrorsChanged((object) this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
}
