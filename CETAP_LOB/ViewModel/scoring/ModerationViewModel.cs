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
using ClosedXML.Excel;
using CETAP_LOB.Helper;
using VenueData = CETAP_LOB.Model.scoring.VenueData;
using ModerationFormData = CETAP_LOB.Helper.ModerationFormData;

namespace CETAP_LOB.ViewModel.scoring
{
  public class ModerationViewModel : ViewModelBase
  {
   public const string ModerationFindingsPropertyName = "ModerationFindings";
   public const string DiffScoresPropertyName = "DiffScores";
    public const string ModerationDatePropertyName = "ModerationDate";
   public const string CheckedDatePropertyName = "CheckedDate";
   public const string DataAdminPropertyName = "DataAdmin";
    public const string TestTypePropertyName = "TestType";
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
    private string _dataAdmin;
    private string _testType;
    private string _modfind;  
    private DateTime _moderationDate = DateTime.Today;
    private DateTime _checkedDate = DateTime.Today;

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

        public string ModerationFindings
        {
            get
            {
                return _modfind;
            }
            set
            {
                if (_modfind == value)
                    return;
                _modfind = value;
                RaisePropertyChanged("ModerationFindings");
            }
        }

        public DateTime CheckedDate
        {
            get
            {
                return _checkedDate;
            }
            set
            {
                if (_checkedDate == value)
                    return;
                _checkedDate = value;
                RaisePropertyChanged("CheckedDate");
            }
        }
        public DateTime ModerationDate
        {
            get
            {
                return _moderationDate;
            }
            set
            {
                if (_moderationDate == value)
                    return;
                _moderationDate = value;
                RaisePropertyChanged("ModerationDate");
            }
        }
        public string TestType
        {
            get
            {
                return _testType;
            }
            set
            {
                if (_testType == value)
                    return;
                _testType = value;
                RaisePropertyChanged("TestType");
            }
        }
        public string DataAdmin
        {
            get
            {
                return _dataAdmin;
            }
            set
            {
                if (_dataAdmin == value)
                    return;
                _dataAdmin = value;
                RaisePropertyChanged("DataAdmin");
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

        public List<VenueData> myVenuedata { get; set; } = new List<VenueData>();
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
            ModerationTemplateGenerator genpdf = new ModerationTemplateGenerator();
            ModerationFormData FormData = new ModerationFormData
            {
                TestDate = Scores.FirstOrDefault()?.DOT.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy"),
                ModerationDate = ModerationDate.ToString("dd/MM/yyyy"),
                TestType = TestType,
                TotalTestWriters = Scores.Count.ToString(),
                TotalModerationRecords = ModeratedScores.Count.ToString(),
                CheckedInBy = DataAdmin,
                ModerationFindings = ModerationFindings,
                CheckedDate = CheckedDate.ToString("dd/MM/yyyy")
            };
            
            string dateString = DateTime.Now.ToString("yyyyMMdd");
            string AuditFile = "ModerationComparisonAudit_"+ Scores.FirstOrDefault()?.DOT.ToString("dd MMM yyyy") + " " + TestType + "_" + dateString + ".pdf";
            
            genpdf.GeneratePDF(Path.Combine(ScoreFolder, AuditFile), myVenuedata, "nbt.png", "CEA.png", FormData);
        }

    private void GetScores()
    {
      Scores = _service.GetAllScores(ScoreFolder);
      ModeratedScores = _service.GetAllModeratedScores(ScoreModerationFolder);
    }

        private void CompareScores()
        {
            List<CompositBDO> scorelist = Scores.ToList();
            var testDate = scorelist.FirstOrDefault()?.DOT ?? DateTime.Now;
            var SummaryComp = scorelist
                               .GroupBy(x => x.VenueCode)
                               .Select(g => new
                               {
                                   VenueCode = g.Key,
                                   VenueName = g.FirstOrDefault().VenueName,
                                   Amount = g.Count()
                               }).ToList();
            List<CompositBDO> modlist = ModeratedScores.ToList();
            var SummaryMod = modlist
                               .GroupBy(x => x.VenueCode)
                               .Select(g => new
                               {
                                   VenueCode = g.Key,
                                   VenueName = g.FirstOrDefault().VenueName,
                                   Amount = g.Count()
                               }).ToList();

            myVenuedata = (from s in SummaryComp
                              join m in SummaryMod on s.VenueCode equals m.VenueCode
                              select new VenueData
                              {
                                  VenueCode = s.VenueCode,
                                  VenueName = s.VenueName,
                                  FullCount = s.Amount,
                                  ModerationCount = m.Amount,


                                  Percentage = s.Amount != 0
                                                 ? $"{Math.Round(100.0 * ((double)m.Amount / s.Amount))}%"
                                                 : "0%"

                              }).ToList();
          //  List<VenueData> mydata = new List<VenueData>();

            var result = (from m in modlist
                          join s in scorelist on m.Barcode equals s.Barcode
                          select new
                          {
                              Barcode = s.Barcode,
                              //Surname = s.Surname,
                              //Name = s.Name,
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
                //x.Surname = a.Surname;
                //x.Name = a.Name;
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
                //x.Surname = a.Surname;
                //x.Name = a.Name;
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
                //x.Surname = a.Surname;
                //x.Name = a.Name;
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
            DiffScores = new ObservableCollection<ScoreDifference>(mydiff.GroupBy(x => x.Barcode)
                .Select(g => g.FirstOrDefault()));
            // DiffScores.Di
            if (mydiff.Count > 0) HasScoreErrors = true;


            // Create Excel file
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Comparison Summary");
                worksheet.Cell(1, 1).Value = "VenueCode";
                worksheet.Cell(1, 2).Value = "VenueName";
                worksheet.Cell(1, 3).Value = "FullCount";
                worksheet.Cell(1, 4).Value = "ModerationCount";
                worksheet.Cell(1, 5).Value = "Percentage";

                int row = 2;
                foreach (var item in myVenuedata)
                {
                    worksheet.Cell(row, 1).Value = item.VenueCode;
                    worksheet.Cell(row, 2).Value = item.VenueName;
                    worksheet.Cell(row, 3).Value = item.FullCount;
                    worksheet.Cell(row, 4).Value = item.ModerationCount;
                    worksheet.Cell(row, 5).Value = item.Percentage;
                    row++;
                }

                var worksheet2 = workbook.Worksheets.Add("Comparison Results");
                worksheet2.Cell(1, 1).Value = "Barcode";
                worksheet2.Cell(1, 2).Value = "ALScore";
                worksheet2.Cell(1, 3).Value = "QLScore";
                worksheet2.Cell(1, 4).Value = "MatScore";
                worksheet2.Cell(1, 5).Value = "Mod ALSCore";
                worksheet2.Cell(1, 6).Value = "Mod QLSCore";
                worksheet2.Cell(1, 7).Value = "Mod MatSCore";
                worksheet2.Cell(1, 8).Value = "Diff ALSCore";
                worksheet2.Cell(1, 9).Value = "Diff QLSCore";
                worksheet2.Cell(1, 10).Value = "Diff MatSCore";
                worksheet2.Cell(1, 11).Value = "Batch";
                worksheet2.Cell(1, 12).Value = "Moderation Batch";

                row = 2;
                foreach (var item in result)
                {
                    worksheet2.Cell(row, 1).Value = item.Barcode;
                    worksheet2.Cell(row, 2).Value = item.ALScore;
                    worksheet2.Cell(row, 3).Value = item.QLScore;
                    worksheet2.Cell(row, 4).Value = item.MatScore;
                    worksheet2.Cell(row, 5).Value = item.M_ALScore;
                    worksheet2.Cell(row, 6).Value = item.M_QLScore;
                    worksheet2.Cell(row, 7).Value = item.M_MatScore;
                    worksheet2.Cell(row, 8).Value = item.diff_AL;
                    worksheet2.Cell(row, 9).Value = item.diff_QL;
                    worksheet2.Cell(row, 10).Value = item.diff_MAT;
                    worksheet2.Cell(row, 11).Value = item.Batch;
                    worksheet2.Cell(row, 12).Value = item.MBatch;
                    row++;
                }

                var worksheet1 = workbook.Worksheets.Add("Score Difference");
                worksheet1.Cell(1, 1).Value = "Barcode";
                worksheet1.Cell(1, 2).Value = "ALScore";
                worksheet1.Cell(1, 3).Value = "QLScore";
                worksheet1.Cell(1, 4).Value = "MatScore";
                worksheet1.Cell(1, 5).Value = "Mod ALSCore";
                worksheet1.Cell(1, 6).Value = "Mod QLSCore";
                worksheet1.Cell(1, 7).Value = "Mod MatSCore";
                worksheet1.Cell(1, 8).Value = "Diff ALSCore";
                worksheet1.Cell(1, 9).Value = "Diff QLSCore";
                worksheet1.Cell(1, 10).Value = "Diff MatSCore";
                worksheet1.Cell(1, 11).Value = "Accepted";
                worksheet1.Cell(1, 12).Value = "Batch";
                worksheet1.Cell(1, 13).Value = "Moderation Batch";

                row = 2;
                foreach (var item in mydiff)
                {
                    worksheet1.Cell(row, 1).Value = item.Barcode;
                    worksheet1.Cell(row, 2).Value = item.ALScore;
                    worksheet1.Cell(row, 3).Value = item.QLScore;
                    worksheet1.Cell(row, 4).Value = item.MATScore;
                    worksheet1.Cell(row, 5).Value = item.M_ALScore;
                    worksheet1.Cell(row, 6).Value = item.M_QLScore;
                    worksheet1.Cell(row, 7).Value = item.M_MATScore;
                    worksheet1.Cell(row, 8).Value = item.Diff_ALScore;
                    worksheet1.Cell(row, 9).Value = item.Diff_QLScore;
                    worksheet1.Cell(row, 10).Value = item.Diff_MATScore;
                    worksheet1.Cell(row, 11).Value = 1;
                    worksheet1.Cell(row, 12).Value = item.Batch;
                    worksheet1.Cell(row, 13).Value = item.M_Batch;
                    row++;
                }



                string mypath = ApplicationSettings.Default.ScoreFolder;
                var filename = $"ModerationSummary_{testDate:yyyyMMdd}.xlsx";
                string comparisonfile = Path.Combine(mypath, filename);
                workbook.SaveAs(comparisonfile);
            }


        }
  }
}
