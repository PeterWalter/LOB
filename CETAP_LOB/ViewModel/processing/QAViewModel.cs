// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.QAViewModel
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClosedXML.Excel;
using System.Reflection.Emit;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Dumpify;

namespace CETAP_LOB.ViewModel.processing
{

    public class QAViewModel : ViewModelBase
    {
        private string Duplicatefile = "";
        public const string BatchesInQueuePropertyName = "BatchesInQueue";
        public const string BatchRecordsPropertyName = "BatchRecords";
        public const string FileCombinationPropertyName = "FileCombination";
        public const string DateofTestPropertyName = "DateofTest";
        public const string ProfilePropertyName = "Profile";
        public const string VenueCodePropertyName = "VenueCode";
        public const string SelectedQuestionPropertyName = "SelectedQuestion";
        public const string StatusPropertyName = "Status";
        public const string SelectedQARecordPropertyName = "SelectedQARecord";
        public const string QARecordsPropertyName = "QARecords";
        public const string SelectedFilePropertyName = "SelectedFile";
        public const string DirListPropertyName = "DirList";
        public const string FolderPropertyName = "Folder";
        public const string TestNamePropertyName = "TestName";

        private IDataService _service;
        private bool HasDuplicates;
        private bool HasDuplicatesQueue;
        private bool HasDuplicatesInDB;
        private ObservableCollection<ForDuplicatesBarcodesBDO> _myInQueue;
        private ObservableCollection<ForDuplicatesBarcodesBDO> _batchedRecords;
        private ObservableCollection<TestBDO> _testName;
        private string _myCombination;
        private DateTime _myDOT;
        private int _myProfile;
        private int _myVenueCode;
        private char _mySelectedQuestion;
        private string _myStatus;
        private IntakeYearsBDO _intake_year;
        private QADatRecord _myQARecord;
        private ObservableCollection<QADatRecord> _myQARecords;
        /// <summary>Guards file-level field propagation against re-entrancy.</summary>
        private bool _propagatingFileLevelFields;

        /// <summary>
        /// Barcode to the set of QA files it appears in. Collected while the QA folder
        /// is read (see Selectfolder), so a barcode repeated across files can be spotted
        /// without re-reading every file each time one is opened.
        /// </summary>
        private readonly Dictionary<long, HashSet<string>> _folderBarcodes = new Dictionary<long, HashSet<string>>();

        /// <summary>
        /// True while Selectfolder is collecting the folder barcode index. The index is
        /// incomplete during that pass, so duplicate marking is deferred until every
        /// file has been read.
        /// </summary>
        private bool _buildingFolderBarcodeIndex;
        private datFileAttributes _myQAFile;
        private ObservableCollection<datFileAttributes> _myQAFiles;

        /// <summary>
        /// Serialises QA file reads. The service keeps per-call state while it parses,
        /// so a background read and a read triggered by the user must never overlap.
        /// </summary>
        private readonly SemaphoreSlim _qaLoadGate = new SemaphoreSlim(1, 1);

        /// <summary>True while the QA folder is being read; the module stays usable.</summary>
        private bool _loading;

        /// <summary>Suppresses the automatic file load while a caller reads records itself.</summary>
        private bool _suppressAutoLoad;
        private string _myFolder;
        private string testName1, testName2;

        public RelayCommand GetNBTCommand { get; private set; }
        public RelayCommand DuplicatesCommand { get; private set; }
        public RelayCommand GetNamesCommand { get; private set; }
        public RelayCommand GetIDCommand { get; private set; }
        public RelayCommand GetDOBCommand { get; private set; }
        public RelayCommand AutoCleanCommand { get; private set; }
        public RelayCommand AddSurnameCommand { get; private set; }
        public RelayCommand AddNameCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand SaveDatFileCommand { get; private set; }
        public RelayCommand UpdateTrackerCommand { get; private set; }
        public RelayCommand ProcessRawScoresCommand { get; private set; }
        public RelayCommand ProcessSummaryCommand { get; private set; }
        public RelayCommand FindDuplicatesCommand {  get; private set; }

        public ObservableCollection<TestBDO> TestName
        {
            get
            {
                return _testName;
            }
            set
            {
                if (_testName != value)
                    return;
                _testName = value;
                RaisePropertyChanged("TestName");
            }
        }

        public ObservableCollection<ForDuplicatesBarcodesBDO> BatchesInQueue
    {
      get
      {
        return _myInQueue;
      }
      set
      {
        if (_myInQueue == value)
          return;
        _myInQueue = value;
        RaisePropertyChanged("BatchesInQueue");
      }
    }

    public ObservableCollection<ForDuplicatesBarcodesBDO> BatchRecords
    {
      get
      {
        return _batchedRecords;
      }
      set
      {
        if (_batchedRecords == value)
          return;
        _batchedRecords = value;
        RaisePropertyChanged("BatchRecords");
      }
    }

    public string FileCombination
    {
      get
      {
        return _myCombination;
      }
      set
      {
        if (_myCombination == value)
          return;
        _myCombination = value;
        RaisePropertyChanged("FileCombination");
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
        LoadTestDate(_myDOT);
        RaisePropertyChanged("DateofTest");
      }
    }
public IntakeYearsBDO Intake_Year
        {
            get
            {
                return _intake_year;
            }
            set
            {
                if (_intake_year == value)
                    return;
            _intake_year = value;
                RaisePropertyChanged("Intake_Year");
            }
        }
    public int Profile
    {
      get
      {
        return _myProfile;
      }
      set
      {
        if (_myProfile == value)
          return;
        _myProfile = value;
        RaisePropertyChanged("Profile");
      }
    }

    public int VenueCode
    {
      get
      {
        return _myVenueCode;
      }
      set
      {
        if (_myVenueCode == value)
          return;
        _myVenueCode = value;
        RaisePropertyChanged("VenueCode");
      }
    }

    public char SelectedQuestion
    {
      get
      {
        return _mySelectedQuestion;
      }
      set
      {
        if ((int) _mySelectedQuestion == (int) value)
          return;
        _mySelectedQuestion = value;
        RaisePropertyChanged("SelectedQuestion");
      }
    }

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

    public QADatRecord SelectedQARecord
    {
      get
      {
        return _myQARecord;
      }
      set
      {
        if (_myQARecord == value)
          return;
        _myQARecord = value;
        RaisePropertyChanged("SelectedQARecord");
      }
    }

    /// <summary>
    /// The records of the selected QA file. Replacing the collection subscribes the
    /// file-level propagation handler to the new records and unsubscribes it from the
    /// previous ones, so an edit never leaks into a file that is no longer loaded.
    /// </summary>
    public ObservableCollection<QADatRecord> QARecords
    {
      get
      {
        return _myQARecords;
      }
      set
      {
        if (_myQARecords == value)
          return;
        DetachFileLevelFieldHandlers(_myQARecords);
        _myQARecords = value;
        AttachFileLevelFieldHandlers(_myQARecords);
        RaisePropertyChanged("QARecords");
      }
    }

    /// <summary>
    /// Venue, test codes, test date and test languages are constants for a whole
    /// QA file, so a change on one record is written to every other record. That
    /// keeps the file consistent and correcting one record corrects them all.
    /// </summary>
    private void QARecord_FileLevelFieldChanged(object sender, PropertyChangedEventArgs e)
    {
      if (_propagatingFileLevelFields)
        return;
      if (!QADatRecord.IsFileLevelField(e.PropertyName))
        return;
      QADatRecord source = sender as QADatRecord;
      if (source == null)
        return;
      _propagatingFileLevelFields = true;
      try
      {
        QADatRecord.PropagateFileLevelField(_myQARecords, source, e.PropertyName);
      }
      finally
      {
        _propagatingFileLevelFields = false;
      }
    }

    /// <summary>Subscribes to the property changes of the records being loaded.</summary>
    private void AttachFileLevelFieldHandlers(IEnumerable<QADatRecord> records)
    {
      if (records == null)
        return;
      foreach (QADatRecord record in records)
      {
        if (record != null)
          record.PropertyChanged += QARecord_FileLevelFieldChanged;
      }
    }

    /// <summary>Unsubscribes the property changes of the records being replaced.</summary>
    private void DetachFileLevelFieldHandlers(IEnumerable<QADatRecord> records)
    {
      if (records == null)
        return;
      foreach (QADatRecord record in records)
      {
        if (record != null)
          record.PropertyChanged -= QARecord_FileLevelFieldChanged;
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
        if (_myQAFile != null && !_suppressAutoLoad)
          _ = LoadSelectedFileAsync(_myQAFile);
        RaisePropertyChanged("SelectedFile");
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

    public QAViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    /// <summary>True while the QA folder is being read; the module stays usable.</summary>
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

    private void InitializeModels()
    {
      Folder = ApplicationSettings.Default.QAFolder;
      _myDOT = DateTime.Now;

      // The list is bound by the view and read by IsDataClean(), so it has to exist from
      // the start rather than only once the background read has begun filling it in.
      DirList = new ObservableCollection<datFileAttributes>();

      // Reading the folder parses and validates every file, which is far too slow to
      // do on the dispatcher - it made the module appear to freeze. Start it in the
      // background and let the file list fill in as each file is read.
      _ = LoadAsync();
    }

    private void RegisterCommands()
    {
      GetNBTCommand = new RelayCommand(new Action(GetNBTNumber));
      GetNamesCommand = new RelayCommand(new Action(GetNamesfromDB));
      GetDOBCommand = new RelayCommand(new Action(GetDOBfromDB));
      GetIDCommand = new RelayCommand(new Action(GetIDfromDB));
      RefreshCommand = new RelayCommand(new Action(Refresh));
      SaveDatFileCommand = new RelayCommand((Action) (() => SaveDatFile()));
      AutoCleanCommand = new RelayCommand(new Action(AutoClean));
      ProcessRawScoresCommand = new RelayCommand(new Action(ProcessRawScores));
      UpdateTrackerCommand = new RelayCommand(new Action(updateTracker));
      AddSurnameCommand = new RelayCommand((Action) (() => AddSurname()));
      AddNameCommand = new RelayCommand((Action) (() => AddName()));
      // Only when nothing is being read, so a folder scan and the duplicate run cannot
      // use the service at the same time.
      DuplicatesCommand = new RelayCommand(() => FindDuplicates(), () => !IsLoading && IsDataClean());
      ProcessSummaryCommand = new RelayCommand(() => GenerateSummary(),() => !IsLoading && IsDataClean());
    }

    private void GenerateSummary()
        {
            List<datFileAttributes> source = new List<datFileAttributes>();
            List<BatchBDO> batches = new List<BatchBDO>();
            try
            {
                foreach (FileSystemInfo afile in new DirectoryInfo(Folder).GetFiles("*.dat"))
                {
                    datFileAttributes datFileAttributes = new datFileAttributes(afile.FullName);
                    SelectedFile = datFileAttributes;
                    BatchBDO mybatch = _service.GetBatchByName(datFileAttributes.SName.ToString());
                    var tests = _service.GetTestFromDatFile(datFileAttributes, _intake_year);
                    //  GetQAData();
                    //   _service.WriteToQaTable(QARecords, mybatch.BatchID);
                    _service.WriteQAdataToDB(datFileAttributes);
                    source.Add(datFileAttributes);
                    batches.Add(mybatch);
                    //   SelectedFile = (datFileAttributes)null;
                }
                var summary = source.GroupBy(g => new { g.SName, g.FileCombination, g.TestCode, g.Profile, g.Client, g.AQL_Language, g.MAT_Language, g.VenueCode })
                    .Join(batches, g => g.Key.SName, b => b.BatchName,
                      (g, b) => new
                      {
                          batchID = b.BatchID,
                          Batchfile = g.Key.SName,
                          Combination = g.Key.FileCombination,
                          Test_code = g.Key.TestCode,
                          TestClient = g.Key.Client,
                          TestProfileBDO = g.Key.Profile,
                          AQL = g.Key.AQL_Language,
                          Math = g.Key.MAT_Language,
                          Totalrecords = g.Sum(w => w.RecordCount),
                          VenueCode = g.Key.VenueCode

                      });

                var SummaryVenue = summary
                                              .GroupBy(x => x.VenueCode)
                                              .Select(g => new
                                              {
                                                  VenueCode = g.Key,
                                                  VenueName = _service.GetTestVenue(g.Key).VenueName.ToString(),
                                                  Amount = g.Sum(x => x.Totalrecords)
                                              }).ToList();

                // create an excel summary sheet
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Batches");
                var Row1 = ws.Row(1);

                Row1.Style.Font.Bold = true;
                Row1.Style.Font.FontSize = 12.0;
                ws.Cell(1, 1).Value = "Batch ID";
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(1, 2).Value = "BatchName";
                ws.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(1, 3).Value = "FileCombination";
                ws.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 4).Value = "TestCode";
                ws.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 5).Value = "CLient";
                ws.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 6).Value = "Profile";
                ws.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 7).Value = "AQL_Lang";
                ws.Cell(1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 8).Value = "MAT_Lang";
                ws.Cell(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 9).Value = "Record_count";
                ws.Cell(1, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 10).Value = "VenueCode";
                ws.Cell(1, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A2").InsertData(summary);

                // new spreadsheet with summary data by venue
                var ws1 = wb.Worksheets.Add("Venues");
                var Row2 = ws.Row(1);
                Row2.Style.Font.Bold = true;
                Row2.Style.Font.FontSize = 12.0;
                ws1.Cell(1, 1).Value = "Venue Code";
                ws1.Cell(1, 2).Value = "Venue Name";
                ws1.Cell(1, 3).Value = "Total Count";
                ws1.Cell("A2").InsertData(SummaryVenue);


                // ws.Cell(1, 6).Value = "AQL";
                // ws.Cell(1, 7).Value = "MAT";
                //  ws.Cell(1, 8).Value = "Venue";
                string file = Path.Combine(Folder, "SummaryForScoring.xlsx");
                wb.SaveAs(file);
                //DirList = new ObservableCollection<datFileAttributes>(source.OrderByDescending(m => m.NoOfErrors));
            }
            catch (Exception ex)
            {
                int num = (int)ModernDialog.ShowMessage(ex.ToString(), "Summary Data", MessageBoxButton.OK, (Window)null);
            }

        //    The_Message();

        }
    private void FindDuplicates()
    {
          BatchRecords = new ObservableCollection<ForDuplicatesBarcodesBDO>();
          Duplicatefile = Path.Combine(Folder, "Duplicate QA records");
          ReadQARecs();
          DuplicatesWithinAQBatches();
          if (HasDuplicates)  return;
          BatchesInQueue = _service.GetBatchesInQueue();
          if (BatchesInQueue.Count<ForDuplicatesBarcodesBDO>() > 0)
            FindDuplicatesInQueue();
          if (HasDuplicatesQueue)
            return;
          FindDuplicatesInDB();
          if (HasDuplicatesInDB)
            return;
          if (!_service.WriteToBatchQueue(BatchRecords))
            return;
          int num = (int) MessageBox.Show("Barcodes now in Score Queue");
    }

    private void FindDuplicatesInDB()
    {
      List<ForDuplicatesBarcodesBDO> duplicatesBarcodesBdoList = new List<ForDuplicatesBarcodesBDO>();
      List<ForDuplicatesBarcodesBDO> duplicatesFromDb = _service.FindDuplicatesFromDB(BatchRecords);
      if (duplicatesFromDb.Count<ForDuplicatesBarcodesBDO>() <= 0)
        return;
      HasDuplicatesInDB = true;
      GenerateDuplicatesReport(duplicatesFromDb);
    }

    private void FindDuplicatesInQueue()
    {
          // duplicates in Queue
          List<ForDuplicatesBarcodesBDO> duplicatesBarcodesBdoList = new List<ForDuplicatesBarcodesBDO>();
          foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in BatchRecords.Where(x => BatchesInQueue.Any(v => v.Barcode == x.Barcode)).ToList<ForDuplicatesBarcodesBDO>())
          {
                duplicatesBarcodesBdo.Reason = "Duplicate Barcode in files being scored";
                duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
          }
          foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in BatchRecords.Where<ForDuplicatesBarcodesBDO>((Func<ForDuplicatesBarcodesBDO, bool>) (x => BatchesInQueue.Any<ForDuplicatesBarcodesBDO>((Func<ForDuplicatesBarcodesBDO, bool>) (v =>
          {
                long? said1 = v.SAID;
                long? said2 = x.SAID;
                if ((said1.GetValueOrDefault() != said2.GetValueOrDefault() ? 0 : (said1.HasValue == said2.HasValue ? 1 : 0)) != 0)
                  return v.SAID.HasValue;
                return false;
              })))).ToList<ForDuplicatesBarcodesBDO>())
          {
                duplicatesBarcodesBdo.Reason = "Duplicate SA ID in files being scored";
                duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
          }
          foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in BatchRecords.Where<ForDuplicatesBarcodesBDO>((Func<ForDuplicatesBarcodesBDO, bool>) (x => BatchesInQueue.Any<ForDuplicatesBarcodesBDO>((Func<ForDuplicatesBarcodesBDO, bool>) (v =>
          {
                if (v.FID == x.FID)
                  return v.FID != "";
                return false;
              })))).ToList<ForDuplicatesBarcodesBDO>())
          {
                duplicatesBarcodesBdo.Reason = "Duplicate ForeignID in files being scored";
                duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
          }
          if (duplicatesBarcodesBdoList.Count<ForDuplicatesBarcodesBDO>() <= 0)   return;
          HasDuplicatesQueue = true;
          GenerateDuplicatesReport(duplicatesBarcodesBdoList);
    }

    private void DuplicatesWithinAQBatches()
    {
          var duplicatesBarcodesBdoList = new List<ForDuplicatesBarcodesBDO>();

            var duplicates = BatchRecords.GroupBy(x => x.Barcode)
                                         .Where(g => g.Skip(1).Any())
                                         .SelectMany(g => g);
                                        
          
            foreach (var duplicatesBarcodesBdo in duplicates)
            {
              duplicatesBarcodesBdo.Reason = "Duplicate Barcode in QA Batches";
              duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
            }
          
        var NBT_Duplicates = BatchRecords.GroupBy(x => x.RefNo)
                                         .Where(g => g.Skip(1).Any())
                                         .SelectMany(g => g);
            
            foreach (var duplicatesBarcodesBdo in NBT_Duplicates)
            {
                  duplicatesBarcodesBdo.Reason = "Duplicate NBT numbers in AQ Batches";
                  duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
            }

            var SAID_Duplicates = BatchRecords.GroupBy(x => x.SAID)
                                         .Where(g => g.Skip(1).Any() && g.Key.HasValue)
                                         .SelectMany(g => g);

            foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in SAID_Duplicates)
            {
                  duplicatesBarcodesBdo.Reason = "Duplicate SA IDs in QA Batches";
                  duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
            }

            var FID_Duplicates = BatchRecords.GroupBy(x => x.FID)
                                               .Where(g => g.Skip(1).Any() && g.Key != "")
                                               .SelectMany(g => g);
            foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in FID_Duplicates)
            {
                  duplicatesBarcodesBdo.Reason = "Duplicate Foreign IDs in QA Batches";
                  duplicatesBarcodesBdoList.Add(duplicatesBarcodesBdo);
            }
        
          if (duplicatesBarcodesBdoList.Count<ForDuplicatesBarcodesBDO>() > 0)
          {
                HasDuplicates = true;
                duplicatesBarcodesBdoList.Count<ForDuplicatesBarcodesBDO>();
                GenerateDuplicatesReport(duplicatesBarcodesBdoList);
          }
          else
            HasDuplicates = false;
    }

    private void GenerateDuplicatesReport(List<ForDuplicatesBarcodesBDO> Duplicates)
    {
          string messageBoxText = "There are " + Duplicates.Count().ToString() + " duplicate records in the (batches) QA folder. " + "See generated report in folder " + " Please re-run option after corrections";

          if (!_service.DuplicateReportGeneration(Duplicates, Duplicatefile))
                return;
          if (Duplicates.Count() == 0)
          {
            ModernDialog.ShowMessage("No duplicate records!","No Duplicates",MessageBoxButton.OK);
          }
          else
          {
                ModernDialog.ShowMessage(messageBoxText,"Duplicates",MessageBoxButton.OK);
          }
    }

    private void ReadBarcodes()
    {
      foreach (QADatRecord qaRecord in (Collection<QADatRecord>) QARecords)
        BatchRecords.Add(new ForDuplicatesBarcodesBDO()
        {
          Barcode = Convert.ToInt64(qaRecord.Barcode),
          RefNo = Convert.ToInt64(qaRecord.Reference),
          SAID = !(qaRecord.SAID.Trim() != "") ? new long?() : new long?(Convert.ToInt64(qaRecord.SAID)),
          FID = qaRecord.ForeignID.Trim(),
          Batch = qaRecord.DatFile.SName,
          RecID = new long?(-1L),
          DateModified = DateTime.Now,
          Reason = ""
        });
    }

    private void ReadQARecs()
    {
      // This caller reads the records itself, so the selection setter must not start
      // its own background read for the same file.
      _suppressAutoLoad = true;
      try
      {
        foreach (datFileAttributes afile in (Collection<datFileAttributes>) DirList)
        {
          SelectedFile = afile;
          GetQAData();
          ReadBarcodes();
        }
      }
      finally
      {
        _suppressAutoLoad = false;
      }
    }

    /// <summary>
    /// True when the folder has been read and none of its files still has errors. The file
    /// list is filled in on a background thread, so it is empty - and therefore not clean -
    /// until the first file has been read; the command bindings query this as soon as the
    /// module opens, which is why a null or empty list must never be treated as clean.
    /// </summary>
    private bool IsDataClean()
    {
      if (DirList == null || DirList.Count == 0)
        return false;

      bool flag = true;
      foreach (datFileAttributes dir in DirList)
      {
        if (dir.NoOfErrors > 0)
          flag = false;
      }
      return flag;
    }

    private void AddSurname()
    {
      string surname = SelectedQARecord.Surname;
      if (!_service.AddSurnameToList(surname))
        return;
      ModernDialog.ShowMessage(surname + " Has been Added to Database", "Add Surname", MessageBoxButton.OK);
    }

    private void AddName()
    {
      string firstName = SelectedQARecord.FirstName;
      if (!_service.AddNameToList(firstName))
        return;
      ModernDialog.ShowMessage(firstName + " Has been Added to Database", "Add Name", MessageBoxButton.OK);
    }

    /// <summary>
    /// Accepts a scanned value as correct and writes it into the matching WriterList
    /// row - the reverse of using the WriterList value. A confirmation shows both
    /// values first, and the record's comparison is refreshed once the write succeeds.
    /// Only the identity fields may be pushed; the NBT Reference is never written back.
    /// </summary>
    public void AcceptQAValueAsCorrect(QADatRecord record, string field)
    {
      if (record == null || !record.CanAcceptQAValueForWriter(field))
        return;

      string current = WriterValueFor(record, field);
      string scanned = ScannedValueFor(record, field);

      MessageBoxResult answer = MessageBox.Show(
        "Write the scanned value into the WriterList?" + Environment.NewLine + Environment.NewLine +
        field + Environment.NewLine +
        "WriterList: " + (string.IsNullOrEmpty(current) ? "(blank)" : current) + Environment.NewLine +
        "Scanned: " + (string.IsNullOrEmpty(scanned) ? "(blank)" : scanned),
        "Update WriterList", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (answer != MessageBoxResult.Yes)
        return;

      string message = "";
      if (!_service.AcceptQAValueIntoWriterList(record, field, ref message))
      {
        ModernDialog.ShowMessage(message, "Update WriterList", MessageBoxButton.OK);
        return;
      }

      record.AcceptQAValueForWriter(field);
      ModernDialog.ShowMessage(message, "Update WriterList", MessageBoxButton.OK);
    }

    /// <summary>The scanned value of a field, for the confirmation dialog.</summary>
    private static string ScannedValueFor(QADatRecord record, string field)
    {
      switch (field)
      {
        case "Name":
          return record.FirstName;
        case "Surname":
          return record.Surname;
        case "SAID":
          return record.SAID;
        case "ForeignID":
          return record.ForeignID;
        case "DOB":
          return record.DOB.ToString("yyyy/MM/dd");
        case "Gender":
          return record.Gender;
        default:
          return "";
      }
    }

    /// <summary>The WriterList value of a field, for the confirmation dialog.</summary>
    private static string WriterValueFor(QADatRecord record, string field)
    {
      switch (field)
      {
        case "Name":
          return record.WriterName;
        case "Surname":
          return record.WriterSurname;
        case "SAID":
          return record.WriterSAID;
        case "ForeignID":
          return record.WriterForeignID;
        case "DOB":
          return record.WriterDOB;
        case "Gender":
          return record.WriterGender;
        default:
          return "";
      }
    }

    /// <summary>
    /// Allocates a new walk-in reference for a record whose details do not match
    /// the WriterList. The next unused NewNBTNumbers row supplies the new
    /// reference, and the replaced reference is written back to that same row.
    /// </summary>
    public void AllocateWalkInReference(QADatRecord record)
    {
      if (record == null)
        return;

      string replaced = record.Reference;
      NewNBTNumberBDO allocation = _service.GetNewNBTNumberFromDB();
      if (allocation == null || allocation.NewNBT == 0)
      {
        ModernDialog.ShowMessage("No new NBT numbers are available to allocate.", "Walk-in reference", MessageBoxButton.OK);
        return;
      }

      long original;
      if (long.TryParse((replaced ?? "").Trim(), out original))
        allocation.OriginalNBT = original;

      string message = "";
      if (!_service.UpdateNBTNumbers(allocation, ref message))
      {
        ModernDialog.ShowMessage(message, "Walk-in reference", MessageBoxButton.OK);
        return;
      }

      record.Reference = allocation.NewNBT.ToString();
      ModernDialog.ShowMessage("Reference changed from " + replaced + " to " + record.Reference + ".", "Walk-in reference", MessageBoxButton.OK);
    }

    private void updateTracker()
    {
      if (DirList == null)
        return;

      foreach (datFileAttributes dir in DirList)
      {
        int Count = File.ReadAllLines(dir.FilePath).Length - 1;
        string sname = dir.SName;
        if (!_service.updateQAtoTracker(sname, Count))
        {
          ModernDialog.ShowMessage("Batch was not recorded on tracker!!!", sname, MessageBoxButton.OK);
        }
      }
    }

    private void ProcessRawScores()
    {
      _service.SaveRawCSXData();
    }

    private void GetDOBfromDB()
    {
      SelectedQARecord = _service.GetDOBfromDB(SelectedQARecord);
    }

    private void AutoClean()
    {
      _service.AutoClean();
    }

    private void Refresh()
    {
      Folder = ApplicationSettings.Default.QAFolder;
      _ = LoadAsync();
    }

    private void GetIDfromDB()
    {
      if (!(SelectedQARecord.Reference.Substring(7, 1) != "9"))
        return;
      SelectedQARecord = _service.GetSAIDbyNBT(SelectedQARecord);
      SelectedQARecord = _service.GetFIDbyNBT(SelectedQARecord);
    }

    /// <summary>
    /// Reads the QA folder off the UI thread: lists the .dat files, parses and validates
    /// each one, builds the folder-wide barcode index and fills the file list as the
    /// results arrive. Reading a file means parsing every record plus two database
    /// comparison passes, so doing it on the dispatcher made the module appear to freeze;
    /// here the module appears straight away and the list fills in behind it.
    /// </summary>
    public async Task LoadAsync()
    {
      if (IsLoading)
        return;

      IsLoading = true;
      string lastFile = null;
      try
      {
        _intake_year = await Task.Run(() => _service.GetIntakeRecord(ApplicationSettings.Default.IntakeYear));
        await Task.Run(() => _service.ReadEndofDatFile());

        List<string> paths = await Task.Run(() => Directory.Exists(Folder)
          ? Directory.GetFiles(Folder, "*.dat").ToList()
          : new List<string>());

        _folderBarcodes.Clear();
        _buildingFolderBarcodeIndex = true;
        if (DirList == null)
          DirList = new ObservableCollection<datFileAttributes>();
        else
          DirList.Clear();

        foreach (string path in paths)
        {
          datFileAttributes file = await Task.Run(() => new datFileAttributes(path));
          QARecords = await ReadRecordsAsync(file);
          file.NoOfErrors = QARecords.Sum<QADatRecord>((Func<QADatRecord, int>) (x => x.errorCount));
          AddFolderBarcodes(QARecords, file.SName);
          DirList.Add(file);
          lastFile = file.SName;
        }
      }
      catch (Exception ex)
      {
        int num = (int) ModernDialog.ShowMessage(ex.ToString(), "Update", MessageBoxButton.OK, (Window) null);
      }
      finally
      {
        _buildingFolderBarcodeIndex = false;
        IsLoading = false;

        // IsDataClean() feeds the command CanExecute checks; without this the toolbar stays
        // as it was until the next unrelated UI event makes WPF re-query it.
        System.Windows.Input.CommandManager.InvalidateRequerySuggested();
      }

      // Worst first, once every file has been read.
      if (DirList != null)
        DirList = new ObservableCollection<datFileAttributes>(DirList.OrderByDescending(m => m.NoOfErrors));

      // The records still loaded belong to the last file read; mark them now that the
      // folder wide barcode index is complete.
      MarkBarcodeDuplicates(lastFile);
    }

    /// <summary>
    /// Reads and validates one file on a background thread, worst record first. Reads are
    /// serialised through the gate because the service keeps the parsed records in
    /// per-call state while it works.
    /// </summary>
    private async Task<ObservableCollection<QADatRecord>> ReadRecordsAsync(datFileAttributes file)
    {
      await _qaLoadGate.WaitAsync();
      try
      {
        ObservableCollection<QADatRecord> records = await Task.Run(() => _service.GetQADataFromFile(file));
        return new ObservableCollection<QADatRecord>(records.OrderByDescending(a => a.errorCount));
      }
      finally
      {
        _qaLoadGate.Release();
      }
    }

    /// <summary>
    /// Loads the file the user selected, without blocking the UI thread, then marks its
    /// duplicate barcodes.
    /// </summary>
    private async Task LoadSelectedFileAsync(datFileAttributes file)
    {
      try
      {
        QARecords = await ReadRecordsAsync(file);
      }
      catch (Exception ex)
      {
        int num = (int) ModernDialog.ShowMessage(ex.ToString(), "Update", MessageBoxButton.OK, (Window) null);
        return;
      }

      if (!_buildingFolderBarcodeIndex)
        MarkBarcodeDuplicates(file.SName);
    }

    /// <summary>
    /// Loads the selected file's records, worst first. The service validates each
    /// record on this call and attaches the WriterList and Composit comparisons.
    /// </summary>
    private void GetQAData()
    {
      QARecords = new ObservableCollection<QADatRecord>(_service.GetQADataFromFile(SelectedFile).OrderByDescending(a => a.errorCount));
    }

    /// <summary>Records every barcode of a file in the folder wide barcode index.</summary>
    private void AddFolderBarcodes(IEnumerable<QADatRecord> records, string fileName)
    {
      if (records == null || string.IsNullOrEmpty(fileName))
        return;

      foreach (QADatRecord record in records)
      {
        long? barcode = ConvertBarcode(record == null ? null : record.Barcode);
        if (!barcode.HasValue)
          continue;

        HashSet<string> files;
        if (!_folderBarcodes.TryGetValue(barcode.Value, out files))
        {
          files = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
          _folderBarcodes.Add(barcode.Value, files);
        }
        files.Add(fileName);
      }
    }

    /// <summary>
    /// Marks the loaded records whose barcode is repeated within the file, appears
    /// in another file of the QA folder, or already exists in the Composit table.
    /// </summary>
    private void MarkBarcodeDuplicates(string currentFile)
    {
      if (QARecords == null)
        return;

      List<QADatRecord> records = QARecords.Where(record => record != null).ToList();
      foreach (QADatRecord record in records)
        record.ClearBarcodeDuplicate();
      if (records.Count == 0)
        return;

      // Repeated within this file.
      var withinFile = records
        .GroupBy(record => ConvertBarcode(record.Barcode))
        .Where(group => group.Key.HasValue && group.Count<QADatRecord>() > 1);
      foreach (var group in withinFile)
      {
        string reason = "duplicated " + group.Count<QADatRecord>() + " times in this file";
        foreach (QADatRecord record in group)
          record.MarkBarcodeDuplicate(reason);
      }

      // Repeated in another file of the QA folder.
      foreach (QADatRecord record in records)
      {
        long? barcode = ConvertBarcode(record.Barcode);
        if (!barcode.HasValue)
          continue;

        HashSet<string> files;
        if (!_folderBarcodes.TryGetValue(barcode.Value, out files))
          continue;

        List<string> others = files
          .Where(name => !string.Equals(name, currentFile, StringComparison.OrdinalIgnoreCase))
          .ToList();
        if (others.Count > 0)
          record.MarkBarcodeDuplicate("also in " + string.Join(", ", others.ToArray()));
      }

      // Already held in the Composit table.
      List<long> barcodes = records
        .Select(record => ConvertBarcode(record.Barcode))
        .Where(value => value.HasValue)
        .Select(value => value.Value)
        .Distinct()
        .ToList();
      if (barcodes.Count == 0)
        return;

      List<long> inComposit = _service.FindCompositBarcodes(barcodes);
      if (inComposit == null || inComposit.Count == 0)
        return;

      HashSet<long> compositBarcodes = new HashSet<long>(inComposit);
      foreach (QADatRecord record in records)
      {
        long? barcode = ConvertBarcode(record.Barcode);
        if (barcode.HasValue && compositBarcodes.Contains(barcode.Value))
          record.MarkBarcodeDuplicate("already exists in Composit");
      }
    }

    /// <summary>
    /// Parses a scanned barcode for comparison. Returns null when it is empty or not
    /// numeric, so an unreadable barcode is never treated as a duplicate.
    /// </summary>
    private static long? ConvertBarcode(string barcode)
    {
      long value;
      if (long.TryParse((barcode ?? "").Trim(), out value))
        return value;
      return null;
    }

    private void GetNBTNumber()
    {
      if (string.IsNullOrEmpty(SelectedQARecord.ForeignID.Trim()))
        SelectedQARecord = _service.GetNBTNumberFromDBbySAID(SelectedQARecord);
      else
        SelectedQARecord = _service.GetNBTNumberFromDBbyFID(SelectedQARecord);
    }

    private void GetNamesfromDB()
    {
      SelectedQARecord = _service.GetNamebyNBT(SelectedQARecord);
      SelectedQARecord = _service.GetSurnamebyNBT(SelectedQARecord);
    }

    private void SaveDatFile()
    {
      // Nothing selected yet - the folder is still being read, or the last save cleared the
      // selection - so there is no file to write.
      if (SelectedFile == null)
      {
        ModernDialog.ShowMessage("Select a file in the list before saving.", "Save QA file", MessageBoxButton.OK);
        return;
      }

      string message = "";
      if (_service.SaveQADatFile(SelectedFile, ref message))
      {
        DirList.Remove(SelectedFile);
        message = "File successfully saved";
        SelectedFile = (datFileAttributes) null;
        QARecords.Clear();
      }
      else
      {
         ModernDialog.ShowMessage(message, "Save Error !!", MessageBoxButton.OK);
      }
      Status = message;
    }

    private void LoadTestDate(DateTime mydate)
    {
      _service.LoadTestDate(_myDOT);
    }

        private void The_Message()
        {
            string excelfile = "SummaryForScoring.xlsx";
            string Fullpath = Path.Combine(Folder, excelfile); 
            MimeMessage mimeMessage = new MimeMessage();
            // add the sender address
            mimeMessage.From.Add(new MailboxAddress("QA Data", "nbt@uct.ac.za"));

            // AddName the receiver of email address
            mimeMessage.To.Add(new MailboxAddress("Peter Chifamba", "peter.chifamba@uct.ac.za"));
            mimeMessage.To.Add(new MailboxAddress("Zethu Mthethwa", "zethu.mthethwa@uct.ac.za"));
            mimeMessage.To.Add(new MailboxAddress("Shaunda Swarts", "shaunda.swarts@uct.ac.za"));

            // set the message subject
            mimeMessage.Subject = "QAed data in Excel Attachment";
            // Set the message body (plain text)
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = @"Hi,
                This email contains Batches of Qa data in an attached Excel file.
                
                kind regards,

                 LOB QA Logistics";

            // Attach the Excel file (replace with your actual file path)
            var excelAttachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                Content = new MimeContent(System.IO.File.OpenRead(Fullpath)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = excelfile
            };

            bodyBuilder.Attachments.Add(excelAttachment);
            mimeMessage.Body = bodyBuilder.ToMessageBody();


            //// set the message body
            //mimeMessage.Body = new TextPart("plain")
            //{
            //    Text = @"Hi,
            //    Please find attached file 'summaryforscoring',

            //    kind Regards, 
            //    {ApplicationSettings.Default.LOBUser}"
            //};

            SmtpClient client = new SmtpClient();
            try
            {
               // client.Connect("smtp-mail.outlook.com", 587, false);
                client.Connect("mail.uct.ac.za", 25);
               
                //client.Connect("smtp.live.com", 587, false);
                //client.Authenticate("peter.chifamba@live.com", "1shumba34a");
               // client.AuthenticationMechanisms.Remove("XOAUTH2");
             //   client.Authenticate("peter.chifamba@uct.ac.za", "Gurundoro@34ashumba");
                client.Send(mimeMessage);

              //  client.Send(mimeMessage);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
               //QAViewModel.dump(ex);
                MessageBox.Show(ex.ToString());
            }


        }

    }
}

