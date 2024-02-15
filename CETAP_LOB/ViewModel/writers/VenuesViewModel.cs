// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.writers.VenuesViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using CETAP_LOB.BDO;
using CETAP_LOB.Search;
using CETAP_LOB.Model;
using FeserWard.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.SqlServer.Types;
using System.Data.Entity.Spatial;

namespace CETAP_LOB.ViewModel.writers
{
    public class VenuesViewModel : ViewModelBase
  {
    private string _mystatus = "";
    public const string StatusPropertyName = "Status";
    public const string ProvincesPropertyName = "Provinces";
    public const string IsDirtyPropertyName = "IsDirty";
    public const string SelectedVenuePropertyName = "SelectedVenue";
    public const string SpecialSessionPropertyName = "SpecialSession";
    public const string RemoteVenuesPropertyName = "RemoteVenues";
    public const string NationalVenuesPropertyName = "NationalVenues";
    public const string TestVenuesPropertyName = "TestVenues";
    private List<ProvinceBDO> _myprovs;
    private bool _isDirty;
    private VenueBDO _selectedVenue;
    private ObservableCollection<VenueBDO> _specialSession;
    private ObservableCollection<VenueBDO> _remotes;
    private ObservableCollection<VenueBDO> _national;
    private ObservableCollection<VenueBDO> _venues;
    private IDataService _service;

    public RelayCommand CreateVenueCommand { get; private set; }

    public RelayCommand DeleteVenueCommand { get; private set; }

    public RelayCommand SaveVenueCommand { get; private set; }

    public RelayCommand UpdateVenueCommand { get; private set; }

    public RelayCommand<VenueBDO> SelectedVenueCommand { get; private set; }

    public IIntelliboxResultsProvider VenueProvider { get; private set; }

    public string Status
    {
      get
      {
        return _mystatus;
      }
      set
      {
        if (_mystatus == value)
          return;
        _mystatus = value;
        RaisePropertyChanged("Status");
      }
    }

    public bool canCreateVenue { get; private set; }

    public List<ProvinceBDO> Provinces
    {
      get
      {
        return _myprovs;
      }
      set
      {
        if (_myprovs == value)
          return;
        _myprovs = value;
        RaisePropertyChanged("Provinces");
      }
    }

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
        _isDirty = value;
        RaisePropertyChanged("IsDirty");
      }
    }

    public VenueBDO SelectedVenue
    {
      get
      {
        return _selectedVenue;
      }
      set
      {
        if (_selectedVenue == value)
          return;
        _selectedVenue = value;
        RaisePropertyChanged("SelectedVenue");
        DeleteVenueCommand.RaiseCanExecuteChanged();
        UpdateVenueCommand.RaiseCanExecuteChanged();
        SaveVenueCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<VenueBDO> SpecialSession
    {
      get
      {
        return _specialSession;
      }
      set
      {
        if (_specialSession == value)
          return;
        _specialSession = value;
        RaisePropertyChanged("SpecialSession");
      }
    }

    public ObservableCollection<VenueBDO> RemoteVenues
    {
      get
      {
        return _remotes;
      }
      set
      {
        if (_remotes == value)
          return;
        _remotes = value;
        RaisePropertyChanged("RemoteVenues");
      }
    }

    public ObservableCollection<VenueBDO> NationalVenues
    {
      get
      {
        return _national;
      }
      set
      {
        if (_national == value)
          return;
        _national = value;
        RaisePropertyChanged("NationalVenues");
      }
    }

    public ObservableCollection<VenueBDO> TestVenues
    {
      get
      {
        return _venues;
      }
      set
      {
        if (_venues == value)
          return;
        _venues = value;
        RaisePropertyChanged("TestVenues");
      }
    }

    public VenuesViewModel(IDataService Service)
    {
      _service = Service;
      registerCommands();
      InitializeModel();
    }

    private void InitializeModel()
    {
      Provinces = new List<ProvinceBDO>();
      VenueProvider = (IIntelliboxResultsProvider) new VenueResultsProvider(_service);
      TestVenues = new ObservableCollection<VenueBDO>();
      Provinces = _service.getAllProvinces();
      canCreateVenue = false;
      TestVenues = new ObservableCollection<VenueBDO>(_service.GetAllvenues());
    }

    private void registerCommands()
    {
      UpdateVenueCommand = new RelayCommand((Action) (() => UpdateVenues()), (Func<bool>) (() => canDelete()));
      DeleteVenueCommand = new RelayCommand((Action) (() => DeleteVenue()), (Func<bool>) (() => canDelete()));
      CreateVenueCommand = new RelayCommand((Action) (() => CreateVenue()));
      SaveVenueCommand = new RelayCommand((Action) (() => SaveVenue()), (Func<bool>) (() => canSaveVenue()));
      SelectedVenueCommand = new RelayCommand<VenueBDO>((Action<VenueBDO>) (e => SelectVenue(e)));
    }

    private void SelectVenue(VenueBDO venue)
    {
      SelectedVenue = venue;
    }

    private bool canDelete()
    {
      if (SelectedVenue != null && SelectedVenue.VenueCode > 0)
        return !string.IsNullOrWhiteSpace(SelectedVenue.VenueName);
      return false;
    }

    private bool canSaveVenue()
    {
      if (SelectedVenue != null && !string.IsNullOrWhiteSpace(SelectedVenue.VenueName) && !string.IsNullOrWhiteSpace(SelectedVenue.ShortName))
        return canCreateVenue;
      return false;
    }

    private void SaveVenue()
    {
      string message = "";
      bool flag = _service.addTestVenue(SelectedVenue, ref message);
      canCreateVenue = false;
      if (!flag)
      {
        int num = (int) ModernDialog.ShowMessage(message, "Add Test Venue", MessageBoxButton.OK, (Window) null);
      }
      Status = message;
      RefreshAsync();
    }

    private void UpdateVenues()
    {
      string message = "";
      if (!_service.updateTestVenue(SelectedVenue, ref message))
      {
        int num = (int) ModernDialog.ShowMessage(message, "Update Test Venue", MessageBoxButton.OK, (Window) null);
      }
      IsDirty = false;
      Status = message;
      RefreshAsync();
    }

    private void DeleteVenue()
    {
      if (ModernDialog.ShowMessage("Do you really want to delete this Test Site?", "Test Venue: " + SelectedVenue.ShortName.ToString() + " | Code: " + SelectedVenue.VenueCode.ToString(), MessageBoxButton.YesNo, (Window) null).ToString() == "Yes")
      {
        string message = "";
        _service.deleteTestVenue(SelectedVenue.VenueCode, ref message);
        Status = message;
      }
      RefreshAsync();
    }

    private void CreateVenue()
    {
      SelectedVenue = (VenueBDO) null;
      SelectedVenue = new VenueBDO();
      canCreateVenue = true;
    }

    public void RefreshAsync()
    {
      TestVenues.Clear();
      TestVenues = new ObservableCollection<VenueBDO>(_service.GetAllvenues());
    }
  }
}
