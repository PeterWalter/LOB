
using FeserWard.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using CETAP_LOB.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel.processing
{
  public class BatchViewModel : ViewModelBase
  {
    private string _mystatus = "";
    public const string PeriodsPropertyName = "Periods";
    public const string IntakeRecordPropertyName = "IntakeRecord";
    public const string TestsPropertyName = "Tests";
    public const string AQLEPropertyName = "AQLE";
    public const string AQLAPropertyName = "AQLA";
    public const string MATAPropertyName = "MATA";
    public const string MATEPropertyName = "MATE";
    public const string WrittenTestPropertyName = "WrittenTest";
    public const string BatchMakersPropertyName = "BatchMakers";
    public const string SelectedBatcherPropertyName = "SelectedBatcher";
    public const string BatchNamePropertyName = "BatchName";
    public const string StampNoPropertyName = "StampNo";
    public const string ClientTypePropertyName = "ClientType";
    public const string SelectedProfilePropertyName = "SelectedProfile";
    public const string ProfilesAllocationsPropertyName = "ProfilesAllocations";
    public const string TestDatePropertyName = "TestDate";
    public const string VenuePropertyName = "Venue";
    public const string SelectedVenuePropertyName = "SelectedVenue";
    public const string NoInBatchPropertyName = "NoInBatch";
    public const string StatusPropertyName = "Status";
    public const string BatchPropertyName = "Batch";
    public const string SelectedBatchPropertyName = "SelectedBatch";
    public const string BatchesPropertyName = "Batches";
    public const string BatchesByPersonPropertyName = "BatchesByPerson";
    private bool isVenue;
    private bool isTestComb;
    private List<IntakeYearsBDO> _myPeriods;
    private IntakeYearsBDO _intakerecord;
    private List<TestBDO> _mytests;
    private TestBDO _myAQLE;
    private TestBDO _myAQLA;
    private TestBDO _myMATA;
    private TestBDO _myMATE;
    private string _myWrittenTest;
    private List<UserBDO> _myUsers;
    private UserBDO _mybatcher;
    private string _mybatch;
    private string _myStamp;
    private string _clientType;
    private ProfileAllocationBDO _myProfile;
    private ObservableCollection<ProfileAllocationBDO> _myProfAllocs;
    private DateTime _myTestDate;
    private VenueBDO _myVenue;
    private VenueBDO _selectedVenue;
    private int _BatchCount;
    private BatchBDO _batch;
    private BatchBDO _selectedBatch;
    private ObservableCollection<BatchBDO> _batches;
    private ObservableCollection<BatchBDO> _mypersonBatches;
    private IDataService _service;

    public RelayCommand SaveBatchCommand { get; private set; }

    public RelayCommand DeleteBatchCommand { get; private set; }

    public RelayCommand UpdateBatchCommand { get; private set; }

    public RelayCommand RestoreCommand { get; private set; }
     public RelayCommand LoaduserbatchesCommand { get; private set; }   

    public IIntelliboxResultsProvider BatchProvider { get; private set; }

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
        RaisePropertyChanged("IntakeRecord");
      }
    }

    public List<TestBDO> Tests
    {
      get
      {
        return _mytests;
      }
      set
      {
        if (_mytests == value)
          return;
        _mytests = value;
        RaisePropertyChanged("Tests");
      }
    }

    public TestBDO AQLE
    {
      get
      {
        return _myAQLE;
      }
      set
      {
        if (_myAQLE == value)
          return;
        _myAQLE = value;
        RaisePropertyChanged("AQLE");
      }
    }

    public TestBDO AQLA
    {
      get
      {
        return _myAQLA;
      }
      set
      {
        if (_myAQLA == value)
          return;
        _myAQLA = value;
        RaisePropertyChanged("AQLA");
      }
    }

    public TestBDO MATA
    {
      get
      {
        return _myMATA;
      }
      set
      {
        if (_myMATA == value)
          return;
        _myMATA = value;
        RaisePropertyChanged("MATA");
      }
    }

    public TestBDO MATE
    {
      get
      {
        return _myMATE;
      }
      set
      {
        if (_myMATE == value)
          return;
        _myMATE = value;
        RaisePropertyChanged("MATE");
      }
    }

    public string WrittenTest
    {
      get
      {
        return _myWrittenTest;
      }
      set
      {
        if (_myWrittenTest == value)
          return;
        _myWrittenTest = value;
        RaisePropertyChanged("WrittenTest");
      }
    }

    public List<UserBDO> BatchMakers
    {
      get
      {
        return _myUsers;
      }
      set
      {
        if (_myUsers == value)
          return;
        _myUsers = value;
        RaisePropertyChanged("BatchMakers");
      }
    }

    public UserBDO SelectedBatcher
    {
      get
      {
        return _mybatcher;
      }
      set
      {
        if (_mybatcher == value)
          return;
        _mybatcher = value;
        if (_mybatcher != null)
        {
          if (Batch != null)
            Batch.BatchedBy = _mybatcher.Name.Trim();
                    RefreshData();
        }
             //    getBatchesbyPerson();
                RaisePropertyChanged("SelectedBatcher");
       // SaveBatchCommand.RaiseCanExecuteChanged();
      }
    }

    public string BatchName
    {
      get
      {
        return _mybatch;
      }
      set
      {
        if (_mybatch == value)
          return;
        _mybatch = value;
        if (!string.IsNullOrWhiteSpace(_mybatch) && _mybatch.Length == 22)
          GetBatchProperties();
        RaisePropertyChanged("BatchName");
      }
    }

    public string StampNo
    {
      get
      {
        return _myStamp;
      }
      set
      {
        if (_myStamp == value)
          return;
        _myStamp = value;
        RaisePropertyChanged("StampNo");
      }
    }

    public string ClientType
    {
      get
      {
        return _clientType;
      }
      set
      {
        if (_clientType == value)
          return;
        _clientType = value;
        RaisePropertyChanged("ClientType");
      }
    }

    public ProfileAllocationBDO SelectedProfile
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
        RaisePropertyChanged("SelectedProfile");
      }
    }

    public ObservableCollection<ProfileAllocationBDO> ProfilesAllocations
    {
      get
      {
        return _myProfAllocs;
      }
      set
      {
        if (_myProfAllocs == value)
          return;
        _myProfAllocs = value;
        RaisePropertyChanged("ProfilesAllocations");
      }
    }

    public DateTime TestDate
    {
      get
      {
        return _myTestDate;
      }
      set
      {
        if (_myTestDate == value)
          return;
        _myTestDate = value;
        GetProfileAllocations();
        if (_myTestDate < DateTime.Today)
          SaveBatchCommand.RaiseCanExecuteChanged();
        RaisePropertyChanged("TestDate");
      }
    }

    public VenueBDO Venue
    {
      get
      {
        return _myVenue;
      }
      set
      {
        if (_myVenue == value)
          return;
        _myVenue = value;
        RaisePropertyChanged("Venue");
        SaveBatchCommand.RaiseCanExecuteChanged();
      }
    }

    public VenueBDO SelectedVenue
    {
      get
      {
        return _selectedVenue;
      }
      set
      {
        if (_selectedVenue == value)
          return;
        _selectedVenue = value;
        RaisePropertyChanged("SelectedVenue");
      }
    }

    public int NoInBatch
    {
      get
      {
        return _BatchCount;
      }
      set
      {
        if (_BatchCount == value)
          return;
        _BatchCount = value;
        RaisePropertyChanged("NoInBatch");
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
      }
    }

    public BatchBDO Batch
    {
      get
      {
        return _batch;
      }
      set
      {
        if (_batch == value)
          return;
        _batch = value;
        RaisePropertyChanged("Batch");
      }
    }

    public BatchBDO SelectedBatch
    {
      get
      {
        return _selectedBatch;
      }
      set
      {
        if (_selectedBatch == value)
          return;
        _selectedBatch = value;
        RaisePropertyChanged("SelectedBatch");
        RestoreCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<BatchBDO> Batches
    {
      get
      {
        return _batches;
      }
      set
      {
        if (_batches == value)
          return;
        _batches = value;
        RaisePropertyChanged("Batches");
      }
    }

    public ObservableCollection<BatchBDO> BatchesByPerson
    {
      get
      {
        return _mypersonBatches;
      }
      set
      {
        if (_mypersonBatches == value)
          return;
        _mypersonBatches = value;
        RaisePropertyChanged("BatchesByPerson");
      }
    }

    public BatchViewModel(IDataService Service)
    {
      _service = Service;
      TestDate = new DateTime();
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      BatchProvider = (IIntelliboxResultsProvider) new BatchResultsProvider(_service);
      List<UserBDO> userBdoList = new List<UserBDO>();

      BatchMakers = _service.GetAllUsers().Where(a => a.Areas.StartsWith("1")).OrderBy(c => c.Name).Select(a => a).ToList();

      _myPeriods = _service.GetAllIntakeYears();

      _intakerecord = _myPeriods.Where(x => x.Year == ApplicationSettings.Default.IntakeYear).FirstOrDefault();
      TestDate = DateTime.Now;
            SelectedBatcher = new UserBDO()
            {
                Name = ApplicationSettings.Default.LOBUser
            };
         
      RefreshData();
    }

      private void RegisterCommands()
    {
          SaveBatchCommand = new RelayCommand(() => SaveBatch(), () => canSaveBatch());
          UpdateBatchCommand = new RelayCommand(() => UpdateBatch());
          DeleteBatchCommand = new RelayCommand(() => DeleteBatch());
            LoaduserbatchesCommand = new RelayCommand(() => getBatchesbyPerson());
          RestoreCommand = new RelayCommand(() => RestoreFile(), () => canRestore());
    }

      private void RestoreFile()
    {
          string batchName = SelectedBatch.BatchName;
          ScannedFileBDO scannedFileBdo = new ScannedFileBDO();
          ScannedFileBDO scannedFile = _service.GetScannedFile(batchName);
          if (scannedFile != null)
          {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = ApplicationSettings.Default.ScoreFolder;
                DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                string path2 = batchName + ".dat";
                if (dialogResult == DialogResult.OK)
                  File.WriteAllBytes(Path.Combine(folderBrowserDialog.SelectedPath, path2), scannedFile.FileData);
                int num = (int) ModernDialog.ShowMessage(path2 + " has been downloaded", "Scanned Files", MessageBoxButton.OK, (Window) null);
          }
          else
          {
               ModernDialog.ShowMessage("No scanned file with name " + batchName + " has been Scanned", "Scanned Files", MessageBoxButton.OK, (Window) null);
          }
    }

      private bool canRestore()
        {
          bool flag = false;
          if (SelectedBatch != null)
            flag = true;
          return flag;
        }

      private void DeleteBatch()
        {
          string message = "";
          if (ModernDialog.ShowMessage("Are you sure !!! \n \n You want to delete batch " + SelectedBatch.BatchName.ToString(), "Delete Record", MessageBoxButton.YesNo, (Window) null) != MessageBoxResult.Yes)
            return;
          _service.deleteBatch(SelectedBatch, ref message);
          ModernDialog.ShowMessage(message, "Delete Record", MessageBoxButton.OK, (Window) null);
        }

      private void getBatchesbyPerson()
        {
          if (SelectedBatcher == null)
            return;
          BatchesByPerson = new ObservableCollection<BatchBDO>(Batches.Where(b => b.BatchedBy.Trim() == SelectedBatcher.Name.Trim()).OrderByDescending(x => x.BatchDate).Select(x => x).ToList());
        }

      private void UpdateBatch()
        {
          string message = "";
          string text = "Record " + SelectedBatch.BatchName.ToString() + " has been updated";
          if (_service.updatebatch(SelectedBatch, ref message))
          {
            ModernDialog.ShowMessage(text, "Update Record", MessageBoxButton.OK, (Window) null);
            RefreshData();
            getBatchesbyPerson();
          }
          else
          {
             ModernDialog.ShowMessage(message, "Update Record", MessageBoxButton.OK, (Window) null);
          }
        }

      private void SaveBatch()
        {
          string message = "";
          Batch.TestDate = TestDate;
          Batch.BatchedBy = SelectedBatcher.Name.Trim();
          Batch.BatchID = -1;
          if (_service.addBatch(Batch, ref message))
          {
            ModernDialog.ShowMessage(message, "Save Batch", MessageBoxButton.OK, (Window) null);
            RefreshData();
            getBatchesbyPerson();
          }
          else
          {
             ModernDialog.ShowMessage(message, "Save Batch", MessageBoxButton.OK, (Window) null);
          }
        }

      private bool canSaveBatch()
        {
          bool flag = false;
          if (Batch != null && SelectedBatcher != null && (TestDate < DateTime.Today && isVenue) && isTestComb)
            flag = true;
          return flag;
        }

      private void GetProfileAllocations()
        {
          ProfilesAllocations = new ObservableCollection<ProfileAllocationBDO>(_service.GetProfileAllocationsByDate(TestDate));
        }

      private bool canCreateBatch()
        {
          if (SelectedVenue != null)
            return NoInBatch != 0;
          return false;
        }

      private bool canEditBatch()
        {
          return SelectedBatch != null;
        }

      private void RefreshData()
        {
              Batches = new ObservableCollection<BatchBDO>(_service.GetAllbatches().Where(x => x.TestDate > _intakerecord.yearStart && x.TestDate < _intakerecord.yearEnd)
                                                                                   .OrderByDescending(b => b.BatchID).Select(x => x).ToList());
              BatchName = "";
              NoInBatch = 0;
              StampNo = "";
              WrittenTest = "";
            //  SelectedBatcher = null;
              
              SelectedBatch = null;
              Tests = null;
              Venue = null;
              getBatchesbyPerson();
            
        }

      private void GetBatchProperties()
        {
          if (ApplicationSettings.Default.DBAvailable)
          {
            datFileAttributes datFileAttributes = new datFileAttributes();
            datFileAttributes.SName = BatchName;
            Batch = new BatchBDO();
            NoInBatch = datFileAttributes.RecordCount;
            ClientType = datFileAttributes.Client;
            StampNo = datFileAttributes.RandNumber.ToString("D5");
            BatchViewModel.datFileAttributesToBatch(datFileAttributes, Batch);
            Batch.TestDate = TestDate;
            Tests = new List<TestBDO>();
            Tests = _service.GetTestFromDatFile(datFileAttributes, _intakerecord);
            WrittenTest = " ";
            if (datFileAttributes.Client != "Walk-in Bio")
            {
              string text = "";
              isTestComb = true;
              foreach (TestBDO test in Tests)
              {
                string str1 = test.TestName.Substring(0, 4);
                switch (datFileAttributes.TestCode)
                {
                  case "0105":
                    if (str1 == "AQLE")
                    {
                      AQLE = test;
                      WrittenTest = AQLE.TestName;
                      continue;
                    }
                    continue;
                  case "0115":
                    if (str1 == "AQLA")
                    {
                      AQLA = test;
                      WrittenTest = AQLA.TestName;
                      continue;
                    }
                    continue;
                  case "0106":
                    if (str1 == "MATE")
                      MATE = test;
                    WrittenTest = MATE.TestName;
                    continue;
                  case "0116":
                    if (str1 == "MATA")
                    {
                      MATA = test;
                      WrittenTest = MATA.TestName;
                      continue;
                    }
                    continue;
                  case "0107":
                    if (str1 == "AQLE")
                    {
                      AQLE = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + AQLE.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                    }
                    if (str1 == "MATE")
                    {
                      MATE = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + MATE.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                      continue;
                    }
                    continue;
                  case "0117":
                    if (str1 == "AQLA")
                    {
                      AQLA = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + AQLA.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                    }
                    if (str1 == "MATA")
                    {
                      MATA = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + MATA.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                      continue;
                    }
                    continue;
                  case "0127":
                    if (str1 == "AQLE")
                    {
                      AQLE = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + AQLE.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                    }
                    if (str1 == "MATA")
                    {
                      MATA = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + MATA.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                      continue;
                    }
                    continue;
                  case "0137":
                    if (str1 == "AQLA")
                    {
                      AQLA = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + AQLA.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                    }
                    if (str1 == "MATE")
                    {
                      MATE = test;
                      BatchViewModel batchViewModel = this;
                      string str2 = batchViewModel.WrittenTest + MATE.TestName + "    ";
                      batchViewModel.WrittenTest = str2;
                      continue;
                    }
                    continue;
                  default:
                    text = "No such test Combination!!!";
                    isTestComb = false;
                    continue;
                }
              }
              if (!string.IsNullOrWhiteSpace(text))
              {
                int num = (int) ModernDialog.ShowMessage(text, "Batch Name", MessageBoxButton.OK, (Window) null);
              }
            }
            isVenue = true;
            Venue = _service.GetTestVenue(datFileAttributes.VenueCode);
            if (Venue != null)
              return;
            isVenue = false;
            int num1 = (int) ModernDialog.ShowMessage("Cannot find Venue!!!", "Venue Name", MessageBoxButton.OK, (Window) null);
          }
          else
          {
            int num2 = (int) ModernDialog.ShowMessage("Cannot connect to Server for Database!!!", "Batching", MessageBoxButton.OK, (Window) null);
          }
        }

      private static void datFileAttributesToBatch(datFileAttributes datfileAttrib, BatchBDO batch)
            {
                  batch.Count = datfileAttrib.RecordCount;
                  batch.BatchName = datfileAttrib.SName;
                  batch.TestProfileID = datfileAttrib.Profile;
                  batch.TestCombination = datfileAttrib.TestCode;
                  batch.TestVenueID = datfileAttrib.VenueCode;
                  batch.RandomTestNumber = datfileAttrib.RandNumber;
            }
  }
}
