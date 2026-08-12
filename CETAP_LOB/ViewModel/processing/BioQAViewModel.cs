// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.BioQAViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using CETAP_LOB.Model.QA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace CETAP_LOB.ViewModel.processing
{
  public class BioQAViewModel : ViewModelBase
  {
    public const string BioQARecordsPropertyName = "BioQARecords";
    public const string FolderPropertyName = "Folder";
    public const string DirListPropertyName = "DirList";
    public const string DateofTestPropertyName = "DateofTest";
    public const string SelectedFilePropertyName = "SelectedFile";
    private ObservableCollection<BioQADatRecord> _myBioRec;
    private string _myFolder;
    private ObservableCollection<datFileAttributes> _myQAFiles;
    private DateTime _myDOT;
    private datFileAttributes _myQAFile;
    private IDataService _service;

    public RelayCommand GetNBTCommand { get; private set; }

    public RelayCommand GetNamesCommand { get; private set; }

    public RelayCommand GetIDCommand { get; private set; }

    public RelayCommand GetDOBCommand { get; private set; }

    public RelayCommand AutoCleanCommand { get; private set; }

    public RelayCommand AddSurnameCommand { get; private set; }

    public RelayCommand AddNameCommand { get; private set; }

    public RelayCommand RefreshCommand { get; private set; }

    public RelayCommand SaveDatFileCommand { get; private set; }

    public RelayCommand UpdateTrackerCommand { get; private set; }

    public ObservableCollection<BioQADatRecord> BioQARecords
    {
      get
      {
        return _myBioRec;
      }
      set
      {
        if (_myBioRec == value)
          return;
        _myBioRec = value;
        RaisePropertyChanged("BioQARecords");
      }
    }

    public string Folder
    {
      get
      {
        return _myFolder;
      }
      set
      {
        if (_myFolder == value)
          return;
        _myFolder = value;
        RaisePropertyChanged("Folder");
      }
    }

    public ObservableCollection<datFileAttributes> DirList
    {
      get
      {
        return _myQAFiles;
      }
      set
      {
        if (_myQAFiles == value)
          return;
        _myQAFiles = value;
        RaisePropertyChanged("DirList");
      }
    }

    public DateTime DateofTest
    {
      get
      {
        return _myDOT;
      }
      set
      {
        if (_myDOT == value)
          return;
        _myDOT = value;
        RaisePropertyChanged("DateofTest");
      }
    }

    public datFileAttributes SelectedFile
    {
      get
      {
        return _myQAFile;
      }
      set
      {
        if (_myQAFile == value)
          return;
        _myQAFile = value;
        if (_myQAFile == null)
          return;
        RaisePropertyChanged("SelectedFile");
      }
    }

    public BioQAViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      Folder = ApplicationSettings.Default.QAFolder;
      DateTime.Now.Year.ToString();
      _service.ReadEndofDatFile();
      _myDOT = DateTime.Now;
      Selectfolder();
    }

    private void RegisterCommands()
    {
      UpdateTrackerCommand = new RelayCommand(new Action(updateTracker));
      AddSurnameCommand = new RelayCommand((Action) (() => AddSurname()));
      AddNameCommand = new RelayCommand((Action) (() => AddName()));
    }

    private void Selectfolder()
    {
      List<datFileAttributes> datFileAttributesList = new List<datFileAttributes>();
      try
      {
        foreach (FileSystemInfo file in new DirectoryInfo(Folder).GetFiles("*.dat"))
        {
          datFileAttributes datFileAttributes = new datFileAttributes(file.FullName);
          SelectedFile = datFileAttributes;
          GetBioQAData();
          datFileAttributes.NoOfErrors = BioQARecords.Sum<BioQADatRecord>((Func<BioQADatRecord, int>) (x => x.ErrorCount));
          datFileAttributesList.Add(datFileAttributes);
          SelectedFile = (datFileAttributes) null;
        }
      }
      catch (Exception ex)
      {
        int num = (int) ModernDialog.ShowMessage(ex.ToString(), "Update", MessageBoxButton.OK, (Window) null);
      }
    }

    private void GetBioQAData()
    {
    }

    private void AddSurname()
    {
    }

    private void AddName()
    {
    }

    private void updateTracker()
    {
      foreach (datFileAttributes dir in (Collection<datFileAttributes>) DirList)
      {
        bool flag = false;
        File.ReadAllLines(dir.FilePath);
        string sname = dir.SName;
        if (!flag)
        {
          int num = (int) ModernDialog.ShowMessage("Batch was not recorded on tracker!!!", sname, MessageBoxButton.OK, (Window) null);
        }
      }
    }
  }
}
