// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.scoring.ScoringViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Model.scoring;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel.scoring
{
  public class ScoringViewModel : ViewModelBase
  {
    public const string StatusPropertyName = "Status";
    public const string CompositPropertyName = "Composit";
    public const string BIOPropertyName = "BIO";
    public const string MATPropertyName = "MAT";
    public const string AQLPropertyName = "AQL";
    public const string FilesInFolderPropertyName = "FilesInFolder";
    private IDataService _service;
    private bool IsmatchScores;
    private string _myStatus;
    private ObservableCollection<CompositBDO> _composit;
    private ObservableCollection<AnswerSheetBio> _bio;
    private ObservableCollection<MAT_Score> _mat;
    private ObservableCollection<AQL_Score> _aql;
    private ObservableCollection<string> _myfiles;
    private ObservableCollection<BenchMarkLevelsBDO> _levels;

    public RelayCommand SelectFolderCommand { get; private set; }

    public RelayCommand ReadFilesCommand { get; private set; }

    public RelayCommand ProcessScoresCommand { get; private set; }

    public RelayCommand GenerateCompositeCommand { get; private set; }

    public RelayCommand TrackScoresCommand { get; private set; }

    public string Status
    {
      get
      {
        return _myStatus;
      }
      set
      {
        if (_myStatus == value)
          return;
        _myStatus = value;
        RaisePropertyChanged("Status");
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

    public ObservableCollection<AnswerSheetBio> BIO
    {
      get
      {
        return _bio;
      }
      set
      {
        if (_bio == value)
          return;
        _bio = value;
        RaisePropertyChanged("BIO");
      }
    }

    public ObservableCollection<MAT_Score> MAT
    {
      get
      {
        return _mat;
      }
      set
      {
        if (_mat == value)
          return;
        _mat = value;
        RaisePropertyChanged("MAT");
      }
    }

    public ObservableCollection<AQL_Score> AQL
    {
      get
      {
        return _aql;
      }
      set
      {
        if (_aql == value)
          return;
        _aql = value;
        RaisePropertyChanged("AQL");
      }
    }

    public ObservableCollection<string> FilesInFolder
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
        RaisePropertyChanged("FilesInFolder");
      }
    }

    public ScoringViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      FilesInFolder = new ObservableCollection<string>();
      Composit = new ObservableCollection<CompositBDO>();
    }

    private void RegisterCommands()
    {
      SelectFolderCommand = new RelayCommand((selectFolder));
      ReadFilesCommand = new RelayCommand((readScoreFiles));
      ProcessScoresCommand = new RelayCommand(() => ProcessScores(), () => CanprocessScores());
      GenerateCompositeCommand = new RelayCommand(() => GenerateComposite());
      TrackScoresCommand = new RelayCommand(() => TrackScores(),() => canTrack());
    }

    private void TrackScores()
    {
            //foreach (var data in Composit.GroupBy((b => b.Batch)).Select(g => new
            //{
            //    Batch = g.Key,
            //    Amount = g.Count<CompositBDO>()
            //}).ToList())
            //{
            //    int the_count = Convert.ToInt32(data.Amount);
            //    _service.RecordsTrackedScores(data.Batch.ToString(), the_count);
            //}
            foreach (var data in Composit.GroupBy(b => b.Batch)
                              .Select(g => new
                              {
                                  Batch = g.Key,
                                  Amount = g.Count()
                              }))
            {
                int theCount = data.Amount; // No need to convert to int, it's already an int
                _service.RecordsTrackedScores(data.Batch.ToString(), theCount);
            }


        }

        private bool canTrack()
    {
      bool flag = false;
      if (Composit.Count > 0)
        flag = true;
      return flag;
    }

    private void GenerateComposite()
    {
      string text;
      if (_service.GenerateComposite())
      {
        text = "Composite files have been generated !!";
        int num = (int) ModernDialog.ShowMessage(text, "Composite", MessageBoxButton.OK, (Window) null);
      }
      else
      {
        text = "Composite not created";
        int num = (int) ModernDialog.ShowMessage(text, "Composite", MessageBoxButton.OK, (Window) null);
      }
      Status = text;
    }

    private bool CanGenerateComposite()
    {
      return Composit.Count > 0;
    }

    private void ProcessScores()
    {
      MatchScores();
    }

    private bool CanprocessScores()
    {
      return false;
    }

    private void LoadData(string filename)
    {
    }

    private void readScoreFiles()
    {
      AQL = _service.GetAQL();
      MAT = _service.GetMat();
      BIO = _service.GetBIO();
      IsmatchScores = true;
      MatchScores();
    }

    private void SendMessageCallback(string message)
    {
      if (!(message == "Clean BIO"))
        return;
      MatchScores();
    }

    private void MatchScores()
    {
      if (!IsmatchScores)
        return;
      Composit = _service.MatchScores();
    }

    private void selectFolder()
    {
      FilesInFolder.Clear();
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = ApplicationSettings.Default.FilesForScoring;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      string selectedPath = folderBrowserDialog.SelectedPath;
      FilesInFolder = _service.ListScoreFiles(selectedPath);
      ApplicationSettings.Default.FilesForScoring = selectedPath;
      ApplicationSettings.Default.Save();
    }
  }
}
