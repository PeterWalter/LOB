// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.writers.LoadWritersViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Model.venueprep;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CETAP_LOB.ViewModel.writers
{
  public class LoadWritersViewModel : ViewModelBase
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private string _mystatus = "";
    public const string IsStartedPropertyName = "IsStarted";
    public const string isLoadedPropertyName = "isLoaded";
    public const string StatusPropertyName = "Status";
    public const string CleanDataPropertyName = "CleanData";
    public const string DBDuplicatesPropertyName = "DBDuplicates";
    public const string DuplicatesPropertyName = "Duplicates";
    public const string IsDirtyPropertyName = "IsDirty";
    public const string applicantsPropertyName = "applicants";
    public const string SelectedWriterPropertyName = "SelectedWriter";
    public const string AfrikaansPropertyName = "Afrikaans";
    public const string EnglishPropertyName = "English";
    public const string VenuesPropertyName = "Venues";
    public const string FemalePropertyName = "Female";
    public const string MalePropertyName = "Male";
    public const string CountPropertyName = "Count";
    public const string ErrorsPropertyName = "Errors";
    public const string writersPropertyName = "writers";
    public const string FileNamePropertyName = "FileName";
    public const string DBResultsPropertyName = "DBResults";
    public ObservableCollection<WritersBDO> myList;
    private IDataService _service;
    private bool _isStarted;
    private bool _isLoaded;
    private bool _mydata;
    private ObservableCollection<WebWriters> _myDBDuplicates;
    private ObservableCollection<WebWriters> _myduplicates;
    private bool _isDirty;
    private CollectionViewSource WebA;
    private WebWriters _selectedWriter;
    private int _afrikaans;
    private int _myenglish;
    private int _venues;
    private int _female;
    private int _male;
    private int _count;
    private ObservableCollection<string> _myProperty;
    private ObservableCollection<WebWriters> _writers;
    private string _filename;
    private List<string> _myDBResults;

    public RelayCommand OpenFileCommand { get; private set; }

    public RelayCommand RefreshCommand { get; private set; }

    public RelayCommand LoadwritersToDBCommand { get; private set; }

    public RelayCommand CreateFileCommand { get; private set; }

    public RelayCommand DeleteRowCommand { get; private set; }

    public RelayCommand CleanNamesCommand { get; private set; }

    public RelayCommand GetDBDuplicatesCommand { get; private set; }

    public RelayCommand GetNBTCommand { get; private set; }

    public RelayCommand<object> UpDateFilterCommand { get; private set; }

    public bool IsStarted
    {
      get
      {
        return _isStarted;
      }
      set
      {
        if (_isStarted == value)
          return;
        _isStarted = value;
        RaisePropertyChanged("IsStarted");
      }
    }

    public bool isLoaded
    {
      get
      {
        return _isLoaded;
      }
      set
      {
        if (_isLoaded == value)
          return;
        _isLoaded = value;
        RaisePropertyChanged("isLoaded");
        CreateFileCommand.RaiseCanExecuteChanged();
        RefreshCommand.RaiseCanExecuteChanged();
      }
    }

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
        LoadwritersToDBCommand.RaiseCanExecuteChanged();
      }
    }

    public bool CleanData
    {
      get
      {
        return _mydata;
      }
      set
      {
        if (_mydata == value)
          return;
        _mydata = value;
        RaisePropertyChanged("CleanData");
        LoadwritersToDBCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<WebWriters> DBDuplicates
    {
      get
      {
        return _myDBDuplicates;
      }
      set
      {
        if (_myDBDuplicates == value)
          return;
        _myDBDuplicates = value;
        RaisePropertyChanged("DBDuplicates");
      }
    }

    public ObservableCollection<WebWriters> Duplicates
    {
      get
      {
        return _myduplicates;
      }
      set
      {
        if (_myduplicates == value)
          return;
        _myduplicates = value;
        RaisePropertyChanged("Duplicates");
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
        LoadwritersToDBCommand.RaiseCanExecuteChanged();
      }
    }

    public string FilterText { get; set; }

    public bool AscendingChecked { get; set; }

    public WebWriters SelectedWriter
    {
      get
      {
        return _selectedWriter;
      }
      set
      {
        if (_selectedWriter == value)
          return;
        _selectedWriter = value;
        RaisePropertyChanged("SelectedWriter");
      }
    }

    public int Afrikaans
    {
      get
      {
        return _afrikaans;
      }
      set
      {
        if (_afrikaans == value)
          return;
        _afrikaans = value;
        RaisePropertyChanged("Afrikaans");
      }
    }

    public int English
    {
      get
      {
        return _myenglish;
      }
      set
      {
        if (_myenglish == value)
          return;
        _myenglish = value;
        RaisePropertyChanged("English");
      }
    }

    public int Venues
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
        RaisePropertyChanged("Venues");
      }
    }

    public int Female
    {
      get
      {
        return _female;
      }
      set
      {
        if (_female == value)
          return;
        _female = value;
        RaisePropertyChanged("Female");
      }
    }

    public int Male
    {
      get
      {
        return _male;
      }
      set
      {
        if (_male == value)
          return;
        _male = value;
        RaisePropertyChanged("Male");
      }
    }

    public int Count
    {
      get
      {
        return _count;
      }
      set
      {
        if (_count == value)
          return;
        _count = value;
        RaisePropertyChanged("Count");
      }
    }

    public ObservableCollection<string> Errors
    {
      get
      {
        return _myProperty;
      }
      set
      {
        if (_myProperty == value)
          return;
        _myProperty = value;
        RaisePropertyChanged("Errors");
      }
    }

    public ObservableCollection<WebWriters> writers
    {
      get
      {
        return _writers;
      }
      set
      {
        if (_writers == value)
          return;
        _writers = value;
        RaisePropertyChanged("writers");
        if (_writers.Count <= 0)
          return;
        LoadwritersToDBCommand.RaiseCanExecuteChanged();
        RaisePropertyChanged("CleanData");
      }
    }

    public string FileName
    {
      get
      {
        return _filename;
      }
      set
      {
        if (_filename == value)
          return;
        _filename = value;
        RaisePropertyChanged("FileName");
      }
    }

    public List<string> DBResults
    {
      get
      {
        return _myDBResults;
      }
      set
      {
        if (_myDBResults == value)
          return;
        _myDBResults = value;
        RaisePropertyChanged("DBResults");
      }
    }

    public LoadWritersViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      writers = new ObservableCollection<WebWriters>();
      Errors = new ObservableCollection<string>();
      Duplicates = new ObservableCollection<WebWriters>();
      DBDuplicates = new ObservableCollection<WebWriters>();
    }

    private void RegisterCommands()
    {
      OpenFileCommand = new RelayCommand(new Action(OpenCSVFile));
      CreateFileCommand = new RelayCommand((Action) (() => createCsvFile()), (Func<bool>) (() => canManipulateData()));
      DeleteRowCommand = new RelayCommand((Action) (() => DeleteWriter()), (Func<bool>) (() => canManipulateData()));
      GetNBTCommand = new RelayCommand((Action) (() => GetNewNBTNumber()), (Func<bool>) (() => canManipulateData()));
      LoadwritersToDBCommand = new RelayCommand((Action) (() => LoadDB()), (Func<bool>) (() => canSaveToDB()));
      CleanNamesCommand = new RelayCommand((Action) (() => RemoveFunnyCharacters()), (Func<bool>) (() => canManipulateData()));
      GetDBDuplicatesCommand = new RelayCommand((Action) (() => GetDBDuplicates()), (Func<bool>) (() => canSaveToDB()));
      RefreshCommand = new RelayCommand((Action) (() => Refresh()), (Func<bool>) (() => canManipulateData()));
    }

    private bool canManipulateData()
    {
      return _isLoaded;
    }

    private bool canSaveToDB()
    {
      if (_mydata)
        return _isDirty;
      return false;
    }

    private void RemoveFunnyCharacters()
    {
      _service.CleanFunnyChars();
    }

    private void GetNewNBTNumber()
    {
      string NewNBT = "";
      _service.GetNewNBT(SelectedWriter, ref NewNBT);
      Status = NewNBT;
      Refresh();
    }

    private void DeleteWriter()
    {
      if (!_service.DeleteWriterfromList(_selectedWriter))
        return;
      Refresh();
    }

    private async void GetDBDuplicates()
    {
      IsStarted = true;
      DBDuplicates = await _service.GetDuplicatesfromDBAsync();
      IsStarted = false;
    }

    private void GetDuplicates()
    {
      Duplicates = _service.GetDuplicates();
      if (Duplicates.Count<WebWriters>() > 0)
      {
        CleanData = false;
        foreach (WebWriters duplicate in (Collection<WebWriters>) Duplicates)
        {
          WebWriters a = duplicate;
          writers.Where<WebWriters>((Func<WebWriters, bool>) (x => x.Reference == a.Reference)).Select<WebWriters, WebWriters>((Func<WebWriters, WebWriters>) (m => m)).ToList<WebWriters>();
        }
      }
      else
      {
        _mydata = true;
        _isDirty = true;
      }
    }

    private async void LoadDB()
    {
      _isDirty = false;
      IsStarted = true;
      bool saved = await LoadDBAsync();
      IsStarted = false;
      _isDirty = true;
      string title = "Writer List: ";
      if (saved)
        ModernDialog.ShowMessage("Writer list loaded to DB", title, MessageBoxButton.OK, (Window) null);
      else
        ModernDialog.ShowMessage(
          DBDuplicates.Count > 0
            ? "Nothing was saved: settle the records in the DB Duplicates list first."
            : "Writer list was NOT loaded to DB. See the log file for the reason.",
          title, MessageBoxButton.OK, (Window) null);
    }

    /// <summary>
    /// Saves the loaded list to the database and reports whether it was written. The save
    /// is refused while the DB Duplicates list still holds records.
    /// </summary>
    private async Task<bool> LoadDBAsync()
    {
      if (DBDuplicates.Count<WebWriters>() > 0)
      {
        LoadWritersViewModel.log.Warn((object) ("There are records in the DB Duplicates list for " + FileName + " - nothing was saved to the database"));
        return false;
      }

      return await _service.addwriterToDBAsync();
    }

    private async void OpenCSVFile()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.DefaultExt = ".csv";
      openFileDialog.Filter = "CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
      bool? nullable = openFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      FileName = openFileDialog.FileName;
      await LoadDataAsync(FileName);
    }

    private void createCsvFile()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.DefaultExt = ".csv";
      saveFileDialog.Filter = "CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
      bool? nullable = saveFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      FileName = saveFileDialog.FileName;
      _service.generateFile(FileName);
    }

    private void Refresh()
    {
      writers.Clear();
      writers = new ObservableCollection<WebWriters>(_service.Processdata().OrderByDescending<WebWriters, int>((Func<WebWriters, int>) (s => s.errorCount)).ToList<WebWriters>());
      CheckHasErrors();
      GetDuplicates();
    }

    private async Task LoadDataAsync(string filename)
    {
      IsStarted = true;
      try
      {
        ObservableCollection<WebWriters> data = await Task.Run((Func<ObservableCollection<WebWriters>>) (() => _service.GetData(filename)));
        if (data != null)
        {
          List<WebWriters> ordered = data.OrderByDescending<WebWriters, int>((Func<WebWriters, int>) (s => s.errorCount)).ToList<WebWriters>();
          Count = ordered.Count;
          Venues = ordered.GroupBy<WebWriters, string>((Func<WebWriters, string>) (a => a.Venue)).Select<IGrouping<string, WebWriters>, string>((Func<IGrouping<string, WebWriters>, string>) (venueGroup => venueGroup.Key)).Count<string>();
          Female = ordered.Where<WebWriters>((Func<WebWriters, bool>) (a => a.Gender == "Female")).Count<WebWriters>();
          Male = ordered.Where<WebWriters>((Func<WebWriters, bool>) (a => a.Gender == "Male")).Count<WebWriters>();
          English = ordered.Where<WebWriters>((Func<WebWriters, bool>) (a => a.Language == "English")).Count<WebWriters>();
          Afrikaans = ordered.Where<WebWriters>((Func<WebWriters, bool>) (a => a.Language == "Afrikaans")).Count<WebWriters>();
          writers = new ObservableCollection<WebWriters>(ordered);
          isLoaded = true;
          CheckHasErrors();
        }
        else
        {
          int num = (int) ModernDialog.ShowMessage("File is opened by another process or \n file does not exist", "Writer List", MessageBoxButton.OK, (Window) null);
        }
      }
      catch (Exception ex)
      {
        LoadWritersViewModel.log.Error((object) "Failed to load writer file", ex);
        int num = (int) ModernDialog.ShowMessage("File could not be loaded.\n" + ex.Message, "Writer List", MessageBoxButton.OK, (Window) null);
      }
      finally
      {
        IsStarted = false;
      }
    }

    private void CheckHasErrors()
    {
      CleanData = !writers.Any<WebWriters>((Func<WebWriters, bool>) (x => x.HasErrors));
    }

    private void HandleChangeSortDirection(object obj)
    {
      WebA.SortDescriptions.Clear();
      if (AscendingChecked)
        WebA.SortDescriptions.Add(new SortDescription(string.Empty, ListSortDirection.Ascending));
      else
        WebA.SortDescriptions.Add(new SortDescription(string.Empty, ListSortDirection.Descending));
    }

    private void applicants_Filter(object sender, FilterEventArgs e)
    {
      if (e.Item == null || FilterText == null)
        return;
      e.Accepted = ((string) e.Item).ToLower().StartsWith(FilterText.ToLower());
    }
  }
}
