// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.writers.NewVenueViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using System;
using System.Collections.Generic;

namespace CETAP_LOB.ViewModel.writers
{
  public class NewVenueViewModel : ViewModelBase
  {
    public const string ProvincesPropertyName = "Provinces";
    public const string VenuePropertyName = "Venue";
    private List<ProvinceBDO> _myprovs;
    private VenueBDO _myvenue;
    private IDataService _service;

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

    public VenueBDO Venue
    {
      get
      {
        return _myvenue;
      }
      set
      {
        if (_myvenue == value)
          return;
        _myvenue = value;
        RaisePropertyChanged("Venue");
      }
    }

    public RelayCommand SaveVenueCommand { get; private set; }

    public NewVenueViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModel();
      registerCommands();
    }

    private void InitializeModel()
    {
      Venue = new VenueBDO();
      Provinces = _service.getAllProvinces();
    }

    private void registerCommands()
    {
      SaveVenueCommand = new RelayCommand((Action) (() => SaveVenue()), (Func<bool>) (() => canSaveVenue()));
    }

    private bool canSaveVenue()
    {
      return Venue.VenueCode > 0;
    }

    private void SaveVenue()
    {
      string message = "";
      _service.addTestVenue(Venue, ref message);
      Messenger.Default.Send<NotificationMessageAction<string>>(new NotificationMessageAction<string>(message, new Action<string>(SendMessageCallback)));
    }

    private void SendMessageCallback(string message)
    {
      int num = message == "Refresh Venues" ? 1 : 0;
    }
  }
}
