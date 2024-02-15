// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.composite.EditCompositeViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using CETAP_LOB.Model.QA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel.composite
{
  public class EditCompositeViewModel : ViewModelBase
  {
    public const string RecInListPropertyName = "RecInList";
    public const string RecordsPropertyName = "Records";
    public const string QADataPropertyName = "QAData";
    public const string TrialSectPropertyName = "TrialSect";
    public const string NoOfFilesPropertyName = "NoOfFiles";
    public const string DatFilesPropertyName = "DatFiles";
    public const string FileNamePropertyName = "FileName";
    public const string CompositPropertyName = "Composit";
    private int _mylist;
    private int _myRecords;
    private ObservableCollection<QADatRecord> _myQAData;
    private ObservableCollection<Section7> _myTrials;
    private int _myCount;
    private ObservableCollection<string> _myfiles;
    private string _myfile;
    private ObservableCollection<CompositBDO> _composit;
    private IDataService _service;

    public RelayCommand NoMatchCommand { get; private set; }

    public RelayCommand GenerateCompositeCommand { get; private set; }

    public RelayCommand ReadFilesCommand { get; private set; }

    public RelayCommand SavetoCSVCommand { get; private set; }

    public int RecInList
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
        RaisePropertyChanged("RecInList");
      }
    }

    public int Records
    {
      get
      {
        return _myRecords;
      }
      set
      {
        if (_myRecords == value)
          return;
        _myRecords = value;
        RaisePropertyChanged("Records");
      }
    }

    public ObservableCollection<QADatRecord> QAData
    {
      get
      {
        return _myQAData;
      }
      set
      {
        if (_myQAData == value)
          return;
        _myQAData = value;
        RaisePropertyChanged("QAData");
      }
    }

    public ObservableCollection<Section7> TrialSect
    {
      get
      {
        return _myTrials;
      }
      set
      {
        if (_myTrials == value)
          return;
        _myTrials = value;
        RaisePropertyChanged("TrialSect");
      }
    }

    public int NoOfFiles
    {
      get
      {
        return _myCount;
      }
      set
      {
        if (_myCount == value)
          return;
        _myCount = value;
        RaisePropertyChanged("NoOfFiles");
      }
    }

    public ObservableCollection<string> DatFiles
    {
      get
      {
        return _myfiles;
      }
      set
      {
        if (_myfiles == value)
          return;
        _myfiles = value;
        RaisePropertyChanged("DatFiles");
      }
    }

    public string FileName
    {
      get
      {
        return _myfile;
      }
      set
      {
        if (_myfile == value)
          return;
        _myfile = value;
        RaisePropertyChanged("FileName");
      }
    }

    public ObservableCollection<CompositBDO> Composit
    {
      get
      {
        return _composit;
      }
      set
      {
        if (_composit == value)
          return;
        _composit = value;
        RaisePropertyChanged("Composit");
      }
    }

    public EditCompositeViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
      TrialSect = new ObservableCollection<Section7>();
    }

    private void InitializeModels()
    {
    }

    private void RegisterCommands()
    {
      NoMatchCommand = new RelayCommand(new Action(NoMatchFile));
      GenerateCompositeCommand = new RelayCommand((Action) (() => GenerateComposite()), (Func<bool>) (() => HasSelection()));
      ReadFilesCommand = new RelayCommand(new Action(ReadFiles));
      SavetoCSVCommand = new RelayCommand(new Action(SaveToCSVFile));
    }

    private void SaveToCSVFile()
    {
      Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
      saveFileDialog.DefaultExt = ".xlsx";
      saveFileDialog.Filter = "CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
      bool? nullable = saveFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      GenerateCSVFile(saveFileDialog.FileName);
    }

    private void GenerateCSVFile(string filename)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
      {
        foreach (Section7 section7 in (Collection<Section7>) TrialSect)
          streamWriter.WriteLine((object) section7);
      }
      int num = (int) ModernDialog.ShowMessage("Excel File has been saved to folder", "Save File!!", MessageBoxButton.OK, (Window) null);
    }

    private async void ReadFiles()
    {
      await ReadFilesAsync();
      await WriteRecords();
      RecInList = TrialSect.Count<Section7>();
    }

    private async Task WriteRecords()
    {
      foreach (string datFile in (Collection<string>) DatFiles)
      {
        datFileAttributes thefile = new datFileAttributes(datFile);
        Records += thefile.RecordCount;
        ObservableCollection<Section7> data = new ObservableCollection<Section7>();
        data = _service.getSec7DatFile(thefile);
        await AddAsync(data);
      }
    }

    private async Task ReadFilesAsync()
    {
            // select folder

            string path = "";
            var dialog = new FolderBrowserDialog();
            DatFiles = new ObservableCollection<string>();
            string[] files;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = dialog.SelectedPath;
                files = Directory.GetFiles(path, "01*.dat", SearchOption.AllDirectories);
                DatFiles = new ObservableCollection<string>(files);
                NoOfFiles = DatFiles.Count;


            }
        }

    private async Task AddAsync(ObservableCollection<Section7> Data)
    {
      foreach (Section7 section7 in  Data) TrialSect.Add(section7);
    }

    private void GenerateComposite()
    {
    }

    public bool HasSelection()
    {
            bool isSelected = false;
            if (Composit.Count > 0) isSelected = true;
            return isSelected;
    }

    private void NoMatchFile()
    {
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.DefaultExt = ".csv";
      openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
      bool? nullable = openFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      FileName = openFileDialog.FileName;
      GetFile();
    }

    public void GetFile()
    {
      _service.GetNoMatchFile(FileName);
    }
  }
}
