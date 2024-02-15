// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.ScanSettingsViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel
{
  public class ScanSettingsViewModel : ViewModelBase
  {
    public const string PeriodsPropertyName = "Periods";
    public const string IntakeRecordPropertyName = "IntakeRecord";
    public const string IntakeYearPropertyName = "IntakeYear";
    public const string ScoreModerationFolderPropertyName = "ScoreModerationFolder";
    public const string ScoreFolderPropertyName = "ScoreFolder";
    public const string FilesForScoringFolderPropertyName = "FilesForScoringFolder";
    public const string FilesForScoringModerationFolderPropertyName = "FilesForScoringModerationFolder";
    public const string QAFolderPropertyName = "QAFolder";
    public const string EditFolderPropertyName = "EditFolder";
    public const string ScanFolderPropertyName = "ScanFolder";
    private bool IsSet;
    private List<IntakeYearsBDO> _myPeriods;
    private IntakeYearsBDO _intakerecord;
    private int _intakeYear;
    private string _myScoreModFolder;
    private string _myScoreFolder;
    private string _myFFSF;
    private string _myFFSMF;
    private string _myQAFolder;
    private string _editFolder;
    private string _scanfolder;
    private IDataService _service;

    public RelayCommand ScanFolderBowserCommand { get; private set; }

    public RelayCommand EditFolderBowserCommand { get; private set; }

    public RelayCommand QAFolderBowserCommand { get; private set; }

    public RelayCommand FilesForScoringFolderModerationCommand { get; private set; }

    public RelayCommand FilesForScoringFolderCommand { get; private set; }

    public RelayCommand ScoreFolderCommand { get; private set; }

    public RelayCommand ScoreModerationFolderCommand { get; private set; }

    public List<IntakeYearsBDO> Periods
    {
      get
      {
        return _myPeriods;
      }
      set
      {
        if (_myPeriods == value)
          return;
        _myPeriods = value;
        RaisePropertyChanged("Periods");
      }
    }

    public IntakeYearsBDO IntakeRecord
    {
      get
      {
        return _intakerecord;
      }
      set
      {
        if (_intakerecord == value)
          return;
        _intakerecord = value;
        _intakeYear = _intakerecord.Year;
        ApplicationSettings.Default.IntakeYear = _intakeYear;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("IntakeYear");
      }
    }

    public int IntakeYear
    {
      get
      {
        return _intakeYear;
      }
      set
      {
        if (_intakeYear == value)
          return;
        _intakeYear = value;
        ApplicationSettings.Default.IntakeYear = _intakeYear;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("IntakeYear");
      }
    }

    public string ScoreModerationFolder
    {
      get
      {
        return _myScoreModFolder;
      }
      set
      {
        if (_myScoreModFolder == value)
          return;
        _myScoreModFolder = value;
        ApplicationSettings.Default.ScoreModerationFolder = _myScoreModFolder;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("ScoreModerationFolder");
      }
    }

    public string ScoreFolder
    {
      get
      {
        return _myScoreFolder;
      }
      set
      {
        if (_myScoreFolder == value)
          return;
        _myScoreFolder = value;
        ApplicationSettings.Default.ScoreFolder = _myScoreFolder;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("ScoreFolder");
      }
    }

    public string FilesForScoringFolder
    {
      get
      {
        return _myFFSF;
      }
      set
      {
        if (_myFFSF == value)
          return;
        _myFFSF = value;
        ApplicationSettings.Default.FilesForScoring = _myFFSF;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("FilesForScoringFolder");
      }
    }

    public string FilesForScoringModerationFolder
    {
      get
      {
        return _myFFSMF;
      }
      set
      {
        if (_myFFSMF == value)
          return;
        _myFFSMF = value;
        ApplicationSettings.Default.ModerationFilesForScoring = _myFFSMF;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("FilesForScoringModerationFolder");
      }
    }

    public string QAFolder
    {
      get
      {
        return _myQAFolder;
      }
      set
      {
        if (_myQAFolder == value)
          return;
        _myQAFolder = value;
        ApplicationSettings.Default.QAFolder = _myQAFolder;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("QAFolder");
      }
    }

    public string EditFolder
    {
      get
      {
        return _editFolder;
      }
      set
      {
        if (_editFolder == value)
          return;
        _editFolder = value;
        ApplicationSettings.Default.EditingFolder = _editFolder;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("EditFolder");
      }
    }

    public string ScanFolder
    {
      get
      {
        return _scanfolder;
      }
      set
      {
        if (_scanfolder == value)
          return;
        _scanfolder = value;
        ApplicationSettings.Default.ScanningFolder = _scanfolder;
        ApplicationSettings.Default.Save();
        RaisePropertyChanged("ScanFolder");
      }
    }

    public ScanSettingsViewModel()
    {
      _service = (IDataService) new DataService();
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      ScanFolder = ApplicationSettings.Default.ScanningFolder;
      EditFolder = ApplicationSettings.Default.EditingFolder;
      QAFolder = ApplicationSettings.Default.QAFolder;
      FilesForScoringFolder = ApplicationSettings.Default.FilesForScoring;
      FilesForScoringModerationFolder = ApplicationSettings.Default.ModerationFilesForScoring;
      ScoreModerationFolder = ApplicationSettings.Default.ScoreModerationFolder;
      ScoreFolder = ApplicationSettings.Default.ScoreFolder;
      _intakeYear = ApplicationSettings.Default.IntakeYear;
    }

    private void RegisterCommands()
    {
      EditFolderBowserCommand = new RelayCommand(new Action(SelectEditFolder));
      ScanFolderBowserCommand = new RelayCommand(new Action(SelectScanFolder));
      QAFolderBowserCommand = new RelayCommand(new Action(SelectQAFolder));
      ScoreFolderCommand = new RelayCommand(new Action(SelectScoreFolder));
      FilesForScoringFolderCommand = new RelayCommand(new Action(SelectFilesForScoringFolder));
      FilesForScoringFolderModerationCommand = new RelayCommand(new Action(SelectFilesForScoringModerationFolder));
      ScoreModerationFolderCommand = new RelayCommand(new Action(SelectScoreModerationFolder));
      _myPeriods = new List<IntakeYearsBDO>();
      _myPeriods = _service.GetAllIntakeYears();
      _intakerecord = _myPeriods.Where<IntakeYearsBDO>((Func<IntakeYearsBDO, bool>) (x => x.Year == _intakeYear)).FirstOrDefault<IntakeYearsBDO>();
    }

    private void SelectScoreFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      ScanFolder = ApplicationSettings.Default.ScoreFolder;
      folderBrowserDialog.SelectedPath = ScoreFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      ScoreFolder = folderBrowserDialog.SelectedPath;
      ApplicationSettings.Default.ScoreFolder = ScoreFolder;
      ApplicationSettings.Default.Save();
    }

    private void SelectScoreModerationFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = ScoreModerationFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      ScoreModerationFolder = folderBrowserDialog.SelectedPath;
      ApplicationSettings.Default.ScoreModerationFolder = ScoreModerationFolder;
      ApplicationSettings.Default.Save();
    }

    private void SelectFilesForScoringFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = FilesForScoringFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      FilesForScoringFolder = folderBrowserDialog.SelectedPath;
      ApplicationSettings.Default.FilesForScoring = FilesForScoringFolder;
      ApplicationSettings.Default.Save();
    }

    private void SelectFilesForScoringModerationFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = FilesForScoringModerationFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      FilesForScoringModerationFolder = folderBrowserDialog.SelectedPath;
      ApplicationSettings.Default.ModerationFilesForScoring = FilesForScoringModerationFolder;
      ApplicationSettings.Default.Save();
    }

    private void SelectQAFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = QAFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      QAFolder = folderBrowserDialog.SelectedPath;
      ApplicationSettings.Default.QAFolder = QAFolder;
      ApplicationSettings.Default.Save();
    }

    private void SelectScanFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = ScanFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      ScanFolder = folderBrowserDialog.SelectedPath;
    }

    private void SelectEditFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = EditFolder;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      EditFolder = folderBrowserDialog.SelectedPath;
    }

    private void StoreSettings()
    {
      if (!IsSet)
        return;
      ApplicationSettings.Default.ScanningFolder = ScanFolder;
      ApplicationSettings.Default.EditingFolder = EditFolder;
      ApplicationSettings.Default.ScoreFolder = ScoreFolder;
      ApplicationSettings.Default.ScoreModerationFolder = ScoreModerationFolder;
      ApplicationSettings.Default.ModerationFilesForScoring = FilesForScoringModerationFolder;
      ApplicationSettings.Default.FilesForScoring = FilesForScoringFolder;
      ApplicationSettings.Default.IntakeYear = IntakeYear;
      ApplicationSettings.Default.Save();
    }

    public void SetScanSettings(string scanfolder, string editfolder, string Qafolder, int intakeYear)
    {
      ScanFolder = scanfolder;
      EditFolder = editfolder;
      QAFolder = Qafolder;
      IntakeYear = intakeYear;
      IsSet = true;
    }

    public void SetScoringSettings(string Scorefolder, string ScoreModFolder, string FFScoring, string MFFScoring)
    {
      ScoreFolder = Scorefolder;
      ScoreModerationFolder = ScoreModFolder;
      FilesForScoringFolder = FFScoring;
      FilesForScoringModerationFolder = MFFScoring;
      IsSet = true;
    }
  }
}
