// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.composite.CompositeViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FeserWard.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel.composite
{
  public class CompositeViewModel : ViewModelBase
  {
    public const string TotalRecPropertyName = "TotalRec";
    public const string IntakeYearPropertyName = "IntakeYear";
    public const string PeriodsPropertyName = "Periods";
    public const string PagesPropertyName = "Pages";
    public const string PagePropertyName = "Page";
    public const string SelectedWritersPropertyName = "SelectedWriters";
    public const string ScoreFolderPropertyName = "ScoreFolder";
    public const string SelectedWriterPropertyName = "SelectedWriter";
    public const string WritersCompositPropertyName = "WritersComposit";
    public const string Composit1PropertyName = "Composit1";
    public const string NBTScoresPropertyName = "NBTScores";
    private const int RecordsLength = 5000;
    private int recPage;
    private int _totalRec;
    private IntakeYearsBDO _IntakeYear;
    private List<IntakeYearsBDO> _periods;
    private int _pages;
    private int _page;
    private ObservableCollection<CompositBDO> _mySelectedWriters;
    private string _myscoreFolder;
    private CompositBDO _mywriter;
    private ObservableCollection<CompositBDO> _myWriters;
    private ObservableCollection<CompositBDO> _mycomposit;
    private ObservableCollection<CompositBDO> _myscores;
    private IDataService _service;

    public RelayCommand LoadNBTCommand { get; private set; }

    public RelayCommand LoadUpdatesCommand { get; private set; }

    public RelayCommand GetIDCommand { get; private set; }

    public RelayCommand GetDOBCommand { get; private set; }

    public RelayCommand AutoCleanCommand { get; private set; }

    public RelayCommand PreviousCommand { get; private set; }

    public RelayCommand GenerateFileCompositeCommand { get; private set; }

    public RelayCommand RefreshCommand { get; private set; }

    public RelayCommand SaveDatFileCommand { get; private set; }

    public RelayCommand NextCommand { get; private set; }

    public RelayCommand IndividualReportCommand { get; private set; }

    public RelayCommand<ObservableCollection<CompositBDO>> SelectionChangedCommand { get; private set; }

    public IIntelliboxResultsProvider ResultsProvider { get; private set; }

    public int TotalRec
    {
      get
      {
        return _totalRec;
      }
      set
      {
        if (_totalRec == value)
          return;
        _totalRec = value;
        RaisePropertyChanged("TotalRec");
      }
    }

    public IntakeYearsBDO IntakeYear
    {
      get
      {
        return _IntakeYear;
      }
      set
      {
        if (_IntakeYear == value)
          return;
        _IntakeYear = value;
        RaisePropertyChanged("IntakeYear");
      }
    }

    public List<IntakeYearsBDO> Periods
    {
      get
      {
        return _periods;
      }
      set
      {
        if (_periods == value)
          return;
        _periods = value;
        RaisePropertyChanged("Periods");
      }
    }

    public int Pages
    {
      get
      {
        return _pages;
      }
      set
      {
        if (_pages == value)
          return;
        _pages = value;
        RaisePropertyChanged("Pages");
      }
    }

    public int Page
    {
      get
      {
        return _page;
      }
      set
      {
        if (_page == value)
          return;
        _page = value;
        RaisePropertyChanged("Page");
        PreviousCommand.RaiseCanExecuteChanged();
        NextCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<CompositBDO> SelectedWriters
    {
      get
      {
        return _mySelectedWriters;
      }
      set
      {
        if (_mySelectedWriters == value)
          return;
        _mySelectedWriters = value;
        RaisePropertyChanged("SelectedWriters");
      }
    }

    public string ScoreFolder
    {
      get
      {
        return _myscoreFolder;
      }
      set
      {
        if (_myscoreFolder == value)
          return;
        _myscoreFolder = value;
        RaisePropertyChanged("ScoreFolder");
      }
    }

    public CompositBDO SelectedWriter
    {
      get
      {
        return _mywriter;
      }
      set
      {
        if (_mywriter == value)
          return;
        _mywriter = value;
        if (_mywriter != null)
          SelectWriters(_mywriter);
        RaisePropertyChanged("SelectedWriter");
      }
    }

    public ObservableCollection<CompositBDO> WritersComposit
    {
      get
      {
        return _myWriters;
      }
      set
      {
        if (_myWriters == value)
          return;
        _myWriters = value;
        RaisePropertyChanged("WritersComposit");
      }
    }

    public ObservableCollection<CompositBDO> Composit1
    {
      get
      {
        return _mycomposit;
      }
      set
      {
        if (_mycomposit == value)
          return;
        _mycomposit = value;
        RaisePropertyChanged("Composit1");
      }
    }

    public ObservableCollection<CompositBDO> NBTScores
    {
      get
      {
        return _myscores;
      }
      set
      {
        if (_myscores == value)
          return;
        _myscores = value;
        RaisePropertyChanged("NBTScores");
      }
    }

    public CompositeViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private async void InitializeModels()
    {
      ResultsProvider = (IIntelliboxResultsProvider) new CompositResultsProvider(_service);
      Periods = _service.GetAllIntakeYears();
      IntakeYear = Periods.Where<IntakeYearsBDO>((Func<IntakeYearsBDO, bool>) (x => x.Year == ApplicationSettings.Default.IntakeYear)).FirstOrDefault<IntakeYearsBDO>();
      TotalRec = _service.GetCompositCount(IntakeYear);
      int pp = TotalRec;
      if (TotalRec < 5000)
        pp = 5000;
      Pages = pp / 5000 + 1;
      await Refresh(_page);
      Composit1 = new ObservableCollection<CompositBDO>();
      ScoreFolder = ApplicationSettings.Default.ScoreFolder;
      Composit1 = _service.GetAllScores(ScoreFolder);
    }

    private async Task Refresh(int page)
    {
      NBTScores = new ObservableCollection<CompositBDO>(await _service.GetAllNBTScoresAsync(recPage, 5000, IntakeYear));
      Page = GetPage();
    }

    private void RegisterCommands()
    {
      LoadNBTCommand = new RelayCommand(AddScoresToDB);
      LoadUpdatesCommand = new RelayCommand(UpdateCompositScoresOnDB);
      PreviousCommand = new RelayCommand(() => GetPrevious(), () => canGetPrevious());
      NextCommand = new RelayCommand((Action) (() => GetNext()), (Func<bool>) (() => canGetNext()));
      IndividualReportCommand = new RelayCommand((Action) (() => GenerateIndividualReport()), (Func<bool>) (() => canGenerate()));
      GenerateFileCompositeCommand = new RelayCommand((Action) (() => GenerateCompositeSelectedRecords()), (Func<bool>) (() => canRun()));
      SelectionChangedCommand = new RelayCommand<ObservableCollection<CompositBDO>>((Action<ObservableCollection<CompositBDO>>) (SelectedWriters =>
      {
        if (SelectedWriters == null)
          return;
        Composit1 = SelectedWriters;
      }));
    }

    private int GetPage()
    {
      return recPage + 1;
    }

    private bool canRun()
    {
      bool flag = false;
      if (SelectedWriters != null && SelectedWriters.Count<CompositBDO>() > 0)
        flag = true;
      return flag;
    }

    private void GenerateCompositeSelectedRecords()
    {
      bool flag = false;
      string folder = "";
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        folder = folderBrowserDialog.SelectedPath;
      if (!string.IsNullOrWhiteSpace(folder))
        flag = _service.GenerateSelectedComposite(SelectedWriters, folder);
      if (!flag)
        return;
      ModernDialog.ShowMessage("Composite of Selected Records created", " Selected Records!!!", MessageBoxButton.OK);
    }

    private void SelectWriters(CompositBDO writer)
    {
      if (SelectedWriters != null)
      {
        if (SelectedWriters.Contains(writer))
          SelectedWriters.Remove(writer);
        else
          SelectedWriters.Add(writer);
      }
      else
      {
        SelectedWriters = new ObservableCollection<CompositBDO>();
        SelectedWriters.Add(writer);
      }
    }

    private void GenerateIndividualReport()
    {
    }

    private bool canGenerate()
    {
      bool flag = false;
      if (SelectedWriter != null)
        flag = true;
      return flag;
    }

    private bool canGetNext()
    {
      return _page < Pages;
    }

    private async void GetNext()
    {
      ++recPage;
      Page = GetPage();
      NBTScores = new ObservableCollection<CompositBDO>(await _service.GetAllNBTScoresAsync(recPage, 5000, IntakeYear));
    }

    private async void GetPrevious()
    {
      --recPage;
      Page = GetPage();
      NBTScores = new ObservableCollection<CompositBDO>(await _service.GetAllNBTScoresAsync(recPage, 5000, IntakeYear));
    }

    private bool canGetPrevious()
    {
      return _page > 1;
    }

    private async void UpdateCompositScoresOnDB()
    {
      List<string> results = new List<string>();
      string Messages = "";
      foreach (CompositBDO results1 in (Collection<CompositBDO>) Composit1)
      {
        bool isIn = false;
        isIn = _service.updateComposit(results1, ref Messages);
        if (!isIn)
        {
          results1.Name = results1.Name.Trim();
          results1.Surname = results1.Surname.Trim();
          results1.Initials = results1.Initials.Trim();
          isIn = await _service.addComposit(results1);
          results.Add(Messages);
        }
      }
      ModernDialog.ShowMessage("Scores loaded to DB !!!", "Scores Update", MessageBoxButton.OK);
    }

    private async void AddScoresToDB()
    {
      DeleteInQueueRecords();
      List<string> results = new List<string>();
      string Messages = "Add to Composite";
      foreach (CompositBDO results1 in (Collection<CompositBDO>) Composit1)
      {
        bool isIn = false;
        results1.Name = results1.Name.Trim();
        results1.Surname = results1.Surname.Trim();
        results1.Initials = results1.Initials.Trim();
        isIn = await _service.addComposit(results1);
        if (!isIn)
          results.Add(Messages);
      }
      await Refresh(_page);
      ModernDialog.ShowMessage("Scores loaded to DB !!!", Messages, MessageBoxButton.OK);
    }

    private void DeleteInQueueRecords()
    {
      List<long> longList = new List<long>();
            string msg = "";
      foreach (CompositBDO compositBdo in  Composit1)
        longList.Add(compositBdo.Barcode);
      if (_service.RemoveRecordsInQueue(longList))
      {
        msg = longList.Count().ToString() + " records in Queue removed";
                ModernDialog.ShowMessage(msg, "Remove Records in Queue", MessageBoxButton.OK);
               // System.Windows.Forms.MessageBox.Show(msg);
      }
      else
      {
                ModernDialog.ShowMessage("No records found in Queue","", MessageBoxButton.OK);
                //System.Windows.Forms.MessageBox.Show("No records found in Queue");
      }
    }
  }
}
