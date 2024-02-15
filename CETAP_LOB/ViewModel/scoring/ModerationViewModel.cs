// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.scoring.ModerationViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CsvHelper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Model.scoring;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CETAP_LOB.ViewModel.scoring
{
  public class ModerationViewModel : ViewModelBase
  {
    public const string DiffScoresPropertyName = "DiffScores";
    public const string ModeratedScoresPropertyName = "ModeratedScores";
    public const string ScoresPropertyName = "Scores";
    public const string SelectedScoreRecordPropertyName = "SelectedScoreRecord";
    public const string ScoreFolderPropertyName = "ScoreFolder";
    public const string ScoreModerationFolderPropertyName = "ScoreModerationFolder";
    public const string FilesForScoringPropertyName = "FilesForScoring";
    public const string ModeratedFilesForScoringPropertyName = "ModeratedFilesForScoring";
    private bool HasScoreErrors;
    private IDataService _service;
    private ObservableCollection<ScoreDifference> _mydiffScores;
    private ObservableCollection<CompositBDO> _moderatedScores;
    private ObservableCollection<CompositBDO> _myscores;
    private CompositBDO _mySelectedScore;
    private string _scoreFolder;
    private string _SMF;
    private string _myFFS;
    private string _myMFFS;

    public RelayCommand SaveModCommand { get; private set; }

    public RelayCommand CorrectRecordCommand { get; private set; }

    public RelayCommand ProcessScoresCommand { get; private set; }

    public RelayCommand GenerateFinalCompositeCommand { get; private set; }

    public ObservableCollection<ScoreDifference> DiffScores
    {
      get
      {
        return _mydiffScores;
      }
      set
      {
        if (_mydiffScores == value)  return;

        _mydiffScores = value;
        RaisePropertyChanged("DiffScores");
      }
    }

    public ObservableCollection<CompositBDO> ModeratedScores
    {
      get
      {
        return _moderatedScores;
      }
      set
      {
        if (_moderatedScores == value)
          return;
        _moderatedScores = value;
        RaisePropertyChanged("ModeratedScores");
      }
    }

    public ObservableCollection<CompositBDO> Scores
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
        RaisePropertyChanged("Scores");
      }
    }

    public CompositBDO SelectedScoreRecord
    {
      get
      {
        return _mySelectedScore;
      }
      set
      {
        if (_mySelectedScore == value)
          return;
        _mySelectedScore = value;
        RaisePropertyChanged("SelectedScoreRecord");
      }
    }

    public string ScoreFolder
    {
      get
      {
        return _scoreFolder;
      }
      set
      {
        if (_scoreFolder == value)
          return;
        _scoreFolder = value;
        RaisePropertyChanged("ScoreFolder");
      }
    }

    public string ScoreModerationFolder
    {
      get
      {
        return _SMF;
      }
      set
      {
        if (_SMF == value)
          return;
        _SMF = value;
        RaisePropertyChanged("ScoreModerationFolder");
      }
    }

    public string FilesForScoring
    {
      get
      {
        return _myFFS;
      }
      set
      {
        if (_myFFS == value)
          return;
        _myFFS = value;
        RaisePropertyChanged("FilesForScoring");
      }
    }

    public string ModeratedFilesForScoring
    {
      get
      {
        return _myMFFS;
      }
      set
      {
        if (_myMFFS == value)
          return;
        _myMFFS = value;
        RaisePropertyChanged("ModeratedFilesForScoring");
      }
    }

    public ModerationViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      ScoreFolder = ApplicationSettings.Default.ScoreFolder;
      ScoreModerationFolder = ApplicationSettings.Default.ScoreModerationFolder;
      FilesForScoring = ApplicationSettings.Default.FilesForScoring;
      ModeratedFilesForScoring = ApplicationSettings.Default.ModerationFilesForScoring;
      Scores = new ObservableCollection<CompositBDO>();
      ModeratedScores = new ObservableCollection<CompositBDO>();
      GetScores();
      CompareScores();
      if (HasScoreErrors) GetAllRawScores();
    }

    private void RegisterCommands()
    {
      SaveModCommand = new RelayCommand(SaveModerationRecords);
      CorrectRecordCommand = new RelayCommand(UpdateScoreRecords);
      GenerateFinalCompositeCommand = new RelayCommand(() => GenerateComposite());
    }

    private void GenerateComposite()
    {
    }

    private void UpdateScoreRecords()
    {
    }

    private void GetAllRawScores()
    {
      foreach (ScoreDifference diffScore in (Collection<ScoreDifference>) DiffScores)
      {
        string path2_1 = diffScore.Batch + ".dat";
        string path2_2 = diffScore.M_Batch + ".dat";
        Path.Combine(FilesForScoring, path2_1);
        Path.Combine(ModeratedFilesForScoring, path2_2);
      }
    }

    private void SaveModerationRecords()
    {
      using (StreamWriter streamWriter = new StreamWriter(Path.Combine(ScoreModerationFolder, "Comparison.csv")))
      {
        using (CsvWriter csvWriter = new CsvWriter((TextWriter) streamWriter))
        {
          csvWriter.Configuration.HasHeaderRecord = true;
          IEnumerable<ScoreDifference> list = (IEnumerable<ScoreDifference>) DiffScores.ToList<ScoreDifference>();
          csvWriter.WriteRecords((IEnumerable) list);
        }
      }
    }

    private void GetScores()
    {
      Scores = _service.GetAllScores(ScoreFolder);
      ModeratedScores = _service.GetAllModeratedScores(ScoreModerationFolder);
    }

    private void CompareScores()
    {
            List<CompositBDO> scorelist = Scores.ToList();
            List<CompositBDO> modlist = ModeratedScores.ToList();
            var result = (from m in modlist
                          join s in scorelist on m.Barcode equals s.Barcode
                          select new
                          {
                              Barcode = s.Barcode,
                              Surname = s.Surname,
                              Name = s.Name,
                              ALScore = s.ALScore,
                              QLScore = s.QLScore,
                              MatScore = s.MATScore,
                              M_Barcode = m.Barcode,
                              M_ALScore = m.ALScore,
                              M_QLScore = m.QLScore,
                              M_MatScore = m.MATScore,
                              diff_AL = s.ALScore == null ? (int?)null :
                                                        m.ALScore == null ? (int?)null :
                                                        s.ALScore - m.ALScore,
                              diff_QL = s.QLScore == null ? (int?)null :
                                                        m.QLScore == null ? (int?)null :
                                                        s.QLScore - m.QLScore,
                              diff_MAT = s.ALScore == null ? (int?)null :
                                                        m.MATScore == null ? (int?)null :
                                                        s.MATScore - m.MATScore,
                              Batch = s.Batch,
                              MBatch = m.Batch

                          }).ToList();

            var ALErrors = (from a in result
                            where (a.diff_AL != 0 && a.diff_AL != null)
                            select a).ToList();
            var QLErrors = (from a in result
                            where (a.diff_QL != 0 && a.diff_QL != null)
                            select a).ToList();
            var MatErrors = (from a in result
                             where (a.diff_MAT != 0 && a.diff_MAT != null)
                             select a).ToList();

            List<ScoreDifference> mydiff = new List<ScoreDifference>();
            foreach (var a in ALErrors)
            {
                ScoreDifference x = new ScoreDifference();
                x.Barcode = a.Barcode;
                x.Surname = a.Surname;
                x.Name = a.Name;
                x.ALScore = a.ALScore;
                x.Batch = a.Batch;
                x.Diff_ALScore = a.diff_AL;
                x.Diff_MATScore = a.diff_MAT;
                x.Diff_QLScore = a.diff_QL;
                x.M_ALScore = a.M_ALScore;
                x.M_MATScore = a.M_MatScore;
                x.M_QLScore = a.M_QLScore;
                x.MATScore = a.MatScore;
                x.QLScore = a.QLScore;
                x.M_Batch = a.MBatch;
                mydiff.Add(x);
            }

            foreach (var a in QLErrors)
            {
                ScoreDifference x = new ScoreDifference();
                x.Barcode = a.Barcode;
                x.Surname = a.Surname;
                x.Name = a.Name;
                x.ALScore = a.ALScore;
                x.Batch = a.Batch;
                x.Diff_ALScore = a.diff_AL;
                x.Diff_MATScore = a.diff_MAT;
                x.Diff_QLScore = a.diff_QL;
                x.M_ALScore = a.M_ALScore;
                x.M_MATScore = a.M_MatScore;
                x.M_QLScore = a.M_QLScore;
                x.MATScore = a.MatScore;
                x.QLScore = a.QLScore;
                x.M_Batch = a.MBatch;
                mydiff.Add(x);
            }

            foreach (var a in MatErrors)
            {
                ScoreDifference x = new ScoreDifference();
                x.Barcode = a.Barcode;
                x.Surname = a.Surname;
                x.Name = a.Name;
                x.ALScore = a.ALScore;
                x.Batch = a.Batch;
                x.Diff_ALScore = a.diff_AL;
                x.Diff_MATScore = a.diff_MAT;
                x.Diff_QLScore = a.diff_QL;
                x.M_ALScore = a.M_ALScore;
                x.M_MATScore = a.M_MatScore;
                x.M_QLScore = a.M_QLScore;
                x.MATScore = a.MatScore;
                x.QLScore = a.QLScore;
                x.M_Batch = a.MBatch;
                mydiff.Add(x);
            }
            //  var Adiff = mydiff.GroupBy(a => a).Select(m => m.First()); // remove duplicates
            DiffScores = new ObservableCollection<ScoreDifference>(mydiff.Distinct());
            // DiffScores.Di
            if (mydiff.Count > 0) HasScoreErrors = true;
        }
  }
}
