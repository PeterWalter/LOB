// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.ErrorViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using CETAP_LOB.Database;
using CETAP_LOB.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace CETAP_LOB.ViewModel.processing
{
  public class ErrorViewModel : ViewModelBase
  {
    public const string ErrorsPropertyName = "Errors";
    public const string IsLoadingPropertyName = "IsLoading";
    private ObservableCollection<Log> _myerrors;
    private IDataService _service;
    private bool _loading;

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

    /// <summary>True while the error log is still being read from the database.</summary>
    public bool IsLoading
    {
      get
      {
        return _loading;
      }
      private set
      {
        if (_loading == value)
          return;
        _loading = value;
        RaisePropertyChanged("IsLoading");
      }
    }

    public ErrorViewModel(IDataService Service)
    {
      _service = Service;

      // dbo.Logs holds well over 140,000 rows. Reading it on the UI thread used to stop the
      // whole window - menu included - for about twenty seconds, so the log is now read in
      // the background and the grid fills in when it arrives.
      Errors = new ObservableCollection<Log>();
      _ = LoadAsync();
    }

    /// <summary>Reads the error log off the UI thread and hands the rows to the grid.</summary>
    private async Task LoadAsync()
    {
      IsLoading = true;
      try
      {
        ObservableCollection<Log> errors = await Task.Run(() => _service.GetAllErrors());
        Errors = errors;
      }
      catch (Exception ex)
      {
        ModernDialog.ShowMessage(ex.ToString(), "Error Logs", MessageBoxButton.OK, (Window)null);
      }
      finally
      {
        IsLoading = false;
      }
    }
  }
}
