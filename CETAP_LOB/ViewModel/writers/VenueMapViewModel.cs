// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.writers.VenueMapViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CETAP_LOB.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace CETAP_LOB.ViewModel.writers
{
  public class VenueMapViewModel : ViewModelBase
  {
    public const string TileLayerPropertyName = "TileLayer";
    public const string LocationPropertyName = "Location";
    public const string PinPropertyName = "Pin";
    public const string ZoomLevelPropertyName = "ZoomLevel";
    private MapTileLayer _tileLayer;
    private Location _location;
    private Pushpin _pin;
    private double _zoomLevel;
    private IDataService _service;

    public RelayCommand SelectVenueCommand { get; private set; }

    public RelayCommand<MouseButtonEventArgs> NewVenueLocationCommand { get; private set; }

    public MapTileLayer TileLayer
    {
      get
      {
        return _tileLayer;
      }
      set
      {
        if (_tileLayer == value)
          return;
        _tileLayer = value;
        RaisePropertyChanged("TileLayer");
      }
    }

    public Location Location
    {
      get
      {
        return _location;
      }
      set
      {
        if (_location == value)
          return;
        _location = value;
        RaisePropertyChanged("Location");
      }
    }

    public Pushpin Pin
    {
      get
      {
        return _pin;
      }
      set
      {
        if (_pin == value)
          return;
        _pin = value;
        RaisePropertyChanged("Pin");
      }
    }

    public double ZoomLevel
    {
      get
      {
        return _zoomLevel;
      }
      set
      {
        if (_zoomLevel == value)
          return;
        _zoomLevel = value;
        RaisePropertyChanged("ZoomLevel");
      }
    }

    public VenueMapViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void RegisterCommands()
    {
      SelectVenueCommand = new RelayCommand(new Action(SelectVenue));
      NewVenueLocationCommand = new RelayCommand<MouseButtonEventArgs>((Action<MouseButtonEventArgs>) (e => NewVenueLocation(e)));
    }

    private void NewVenueLocation(MouseButtonEventArgs e)
    {
      Map source = (Map) e.Source;
      Point position = e.GetPosition((IInputElement) source);
      Location location = source.ViewportPointToLocation(position);
      int num = (int) MessageBox.Show("Latitude: " + (object) location.Latitude + " Longitude: " + (object) location.Longitude);
    }

    private void SelectVenue()
    {
      throw new NotImplementedException();
    }

    private void InitializeModels()
    {
    }
  }
}
