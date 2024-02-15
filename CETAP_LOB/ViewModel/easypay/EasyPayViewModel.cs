// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.easypay.EasyPayViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LINQtoCSV;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace CETAP_LOB.ViewModel.easypay
{
  public class EasyPayViewModel : ViewModelBase
  {
    private static MRUManager<string> _mruManager = new MRUManager<string>("EasyPay", 10);
    public const string InProgressPropertyName = "InProgress";
    public const string EasyPayRecordsPropertyName = "EasyPayRecords";
    public const string DateLoadedPropertyName = "DateLoaded";
    public const string EPFileNamePropertyName = "EPFileName";
    public const string StartDatePropertyName = "StartDate";
    public const string EndDatePropertyName = "EndDate";
    public const string mruListPropertyName = "mruList";
    public const string FolderPropertyName = "Folder";
    public const string SelectedFilesPropertyName = "SelectedFiles";
    public const string DirListPropertyName = "DirList";
    private IDataService _service;
    private bool _inProgress;
    private ObservableCollection<Vw_EasyPayRecords> _espRecords;
    private DateTime _dateLoaded;
    private string _epFileName;
    private DateTime _startDate;
    private DateTime _endDate;
    private ObservableCollection<string> _mrulist;
    private string _folder;
    private IEnumerable<EasypayFile> _selectedfiles;
    private ObservableCollection<EasypayFile> _mylist;
    private EasyPayFile EPFile;

    public RelayCommand ListCommand { get; private set; }

    public RelayCommand<string> SelectedItemCommand { get; private set; }

    public RelayCommand FolderBowserCommand { get; private set; }

    public RelayCommand WriteToDatabaseCommand { get; private set; }

    public RelayCommand FromDBCommand { get; private set; }

    public RelayCommand SavetoCSVCommand { get; private set; }

    public bool InProgress
    {
      get
      {
        return _inProgress;
      }
      set
      {
        if (_inProgress == value)
          return;
        _inProgress = value;
        RaisePropertyChanged("InProgress");
      }
    }

    public ObservableCollection<Vw_EasyPayRecords> EasyPayRecords
    {
      get
      {
        return _espRecords;
      }
      set
      {
        if (_espRecords == value)
          return;
        _espRecords = value;
        RaisePropertyChanged("EasyPayRecords");
      }
    }

    public DateTime DateLoaded
    {
      get
      {
        return _dateLoaded;
      }
      set
      {
        if (_dateLoaded == value)
          return;
        _dateLoaded = value;
        RaisePropertyChanged("DateLoaded");
      }
    }

    public string EPFileName
    {
      get
      {
        return _epFileName;
      }
      set
      {
        if (_epFileName == value)
          return;
        _epFileName = value;
        RaisePropertyChanged("EPFileName");
      }
    }

    public DateTime StartDate
    {
      get
      {
        return _startDate;
      }
      set
      {
        if (_startDate == value)
          return;
        _startDate = value;
        RaisePropertyChanged("StartDate");
      }
    }

    public DateTime EndDate
    {
      get
      {
        return _endDate;
      }
      set
      {
        if (_endDate == value)
          return;
        _endDate = value;
        RaisePropertyChanged("EndDate");
      }
    }

    public ObservableCollection<string> mruList
    {
      get
      {
        return _mrulist;
      }
      set
      {
        if (_mrulist == value)
          return;
        _mrulist = value;
        RaisePropertyChanged("mruList");
      }
    }

    public string Folder
    {
      get
      {
        return _folder;
      }
      set
      {
        if (_folder == value)
          return;
        _folder = value;
        RaisePropertyChanged("Folder");
      }
    }

    public IEnumerable<EasypayFile> SelectedFiles
    {
      get
      {
        if (DirList != null)
          return DirList.Where(x => x.IsSelected);
        return _selectedfiles;
      }
      set
      {
        if (_selectedfiles == value)
          return;
        _selectedfiles = value;
        RaisePropertyChanged("SelectedFiles");
      }
    }

    public ObservableCollection<EasypayFile> DirList
    {
      get
      {
        return _mylist;
      }
      set
      {
        if (_mylist == value)
          return;
        _mylist = value;
        RaisePropertyChanged("DirList");
      }
    }

    public EasyPayViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
      FtpDirectoryList();
    }

    private void InitializeModels()
    {
      mruList = new ObservableCollection<string>(EasyPayViewModel._mruManager.List);
      StartDate = DateTime.Now;
      EndDate = DateTime.Now;
    }

    private void RegisterCommands()
    {
      ListCommand = new RelayCommand(() => GetFiles());
      FolderBowserCommand = new RelayCommand(OpenNewFolder);
      WriteToDatabaseCommand = new RelayCommand(() => WriteToDB());
      FromDBCommand = new RelayCommand(() => RecordsFromDB());
      SavetoCSVCommand = new RelayCommand(() => SaveCSVFile());
    }

    private void SaveCSVFile()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files(*.*)|*.*|Text Files (*.txt)|*.txt";
      saveFileDialog.FilterIndex = 1;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      new CsvContext().Write(EasyPayRecords.ToList(), saveFileDialog.FileName, new CsvFileDescription()
      {
        SeparatorChar = ',',
        FirstLineHasColumnNames = true
      });
    }

    private async void RecordsFromDB()
    {
      InProgress = true;
      EasyPayRecords = await _service.GetEasyPayRecords(_startDate, _endDate);
      InProgress = false;
    }

    private  void FtpDirectoryList()
    {
      
          EPFile = _service.ReadLastFile();
          _epFileName = EPFile.FileName;
          _dateLoaded = DateTime.ParseExact(EPFile.DateWritten, "yyyy/MM/dd",CultureInfo.InvariantCulture);
          //DateLoaded = Convert.ToDateTime(this.EPFile.DateWritten);
          DirList =  _service.ListFTPFiles();
    }

    private async void GetFiles()
    {
      SelectedFiles = DirList.Where<EasypayFile>((Func<EasypayFile, bool>) (x => x.IsSelected));
      foreach (EasypayFile selectedFile in SelectedFiles)
      {
        string Aname = selectedFile.FileName.ToString();
        string b = Folder + "/" + Aname;
        await _service.DownloadfileAsync(Aname, b);
      }
    }

    private void OpenNewFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = "D:\\ftpeasypay\\";
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      string selectedPath = folderBrowserDialog.SelectedPath;
      EasyPayViewModel._mruManager.Add(selectedPath);
      mruList.Add(selectedPath);
      Folder = selectedPath;
    }

    private async void WriteToDB()
    {
      await _service.WriteFilesToDBAsync(Folder);
    }
  }
}
