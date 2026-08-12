// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.ErrorViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using CETAP_LOB.Database;
using CETAP_LOB.Model;
using System.Collections.ObjectModel;

namespace CETAP_LOB.ViewModel.processing
{
  public class ErrorViewModel : ViewModelBase
  {
    public const string ErrorsPropertyName = "Errors";
    private ObservableCollection<Log> _myerrors;
    private IDataService _service;

    public ObservableCollection<Log> Errors
    {
      get
      {
        return _myerrors;
      }
      set
      {
        if (_myerrors == value)
          return;
        _myerrors = value;
        RaisePropertyChanged("Errors");
      }
    }

    public ErrorViewModel(IDataService Service)
    {
      _service = Service;
      Errors = _service.GetAllErrors();
    }
  }
}
