// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.TestsViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Search;
using FeserWard.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace CETAP_LOB.ViewModel.processing
{
    public class TestsViewModel : ViewModelBase
  {
    public const string PeriodsPropertyName = "Periods";
    public const string IntakeRecordPropertyName = "IntakeRecord";
    public const string IntakeYearPropertyName = "IntakeYear";
    public const string GrpProfilesPropertyName = "GrpProfiles";
    public const string ProfilesAllocationsPropertyName = "ProfilesAllocations";
    public const string SelectedProfilePropertyName = "SelectedProfile";
    public const string ProfilesPropertyName = "Profiles";
    public const string SelectedProfNumberPropertyName = "SelectedProfNumber";
    public const string SelectedProfAllocPropertyName = "SelectedProfAlloc";
    public const string AllAllocationsPropertyName = "AllAllocations";
    public const string BStatusPropertyName = "BStatus";
    public const string AStatusPropertyName = "AStatus";
    public const string StatusPropertyName = "Status";
    public const string SelectedAllocationPropertyName = "SelectedAllocation";
    public const string SelectedTestPropertyName = "SelectedTest";
    public const string AllocationsPropertyName = "Allocations";
    public const string TestsPropertyName = "Tests";
    private List<IntakeYearsBDO> _myPeriods;
    private IntakeYearsBDO _intakerecord;
    private int _intakeyear;
    private ObservableCollection<TestProfileBDO> _mygrpProfiles;
    private ObservableCollection<ProfileAllocationBDO> _myProfAllocs;
    private TestProfileBDO _myProfile;
    private ObservableCollection<TestProfileBDO> _myProfiles;
    private int _myProfNumber;
    private TestAllocationBDO _myselectedProfAlloc;
    private ObservableCollection<TestAllocationBDO> _allAlloc;
    private string _myBstatus;
    private string _myAstatus;
    private string _status;
    private TestAllocationBDO _myselectedAllocation;
    private TestBDO _test;
    private ObservableCollection<TestAllocationBDO> _myallocations;
    private ObservableCollection<TestBDO> _tests;
    private IDataService _service;

    public IIntelliboxResultsProvider TestsProvider { get; private set; }

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
        IntakeYear = _intakerecord.Year;
        RefreshAll();
        RaisePropertyChanged("IntakeRecord");
      }
    }

    public int IntakeYear
    {
      get
      {
        return _intakeyear;
      }
      set
      {
        if (_intakeyear == value)
          return;
        _intakeyear = value;
        GetSelectedIntake();
        RaisePropertyChanged("IntakeYear");
      }
    }

    public ObservableCollection<TestProfileBDO> GrpProfiles
    {
      get
      {
        return _mygrpProfiles;
      }
      set
      {
        if (_mygrpProfiles == value)
          return;
        _mygrpProfiles = value;
        RaisePropertyChanged("GrpProfiles");
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

    public TestProfileBDO SelectedProfile
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
        SaveAProfileCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<TestProfileBDO> Profiles
    {
      get
      {
        return _myProfiles;
      }
      set
      {
        if (_myProfiles == value)
          return;
        _myProfiles = value;
        RaisePropertyChanged("Profiles");
      }
    }

    public int SelectedProfNumber
    {
      get
      {
        return _myProfNumber;
      }
      set
      {
        if (_myProfNumber == value)
          return;
        _myProfNumber = value;
        _myProfile.Profile = _myProfNumber;
        RaisePropertyChanged("SelectedProfNumber");
        GetGroupProfiles();
      }
    }

    public TestAllocationBDO SelectedProfAlloc
    {
      get
      {
        return _myselectedProfAlloc;
      }
      set
      {
        if (_myselectedProfAlloc == value)
          return;
        _myselectedProfAlloc = value;
        RaisePropertyChanged("SelectedProfAlloc");
      }
    }

    public ObservableCollection<TestAllocationBDO> AllAllocations
    {
      get
      {
        return _allAlloc;
      }
      set
      {
        if (_allAlloc == value)
          return;
        _allAlloc = value;
        RaisePropertyChanged("AllAllocations");
      }
    }

    public string BStatus
    {
      get
      {
        return _myBstatus;
      }
      set
      {
        if (_myBstatus == value)
          return;
        _myBstatus = value;
        RaisePropertyChanged("BStatus");
      }
    }

    public string AStatus
    {
      get
      {
        return _myAstatus;
      }
      set
      {
        if (_myAstatus == value)
          return;
        _myAstatus = value;
        RaisePropertyChanged("AStatus");
      }
    }

    public string Status
    {
      get
      {
        return _status;
      }
      set
      {
        if (_status == value)
          return;
        _status = value;
        RaisePropertyChanged("Status");
      }
    }

    public TestAllocationBDO SelectedAllocation
    {
      get
      {
        return _myselectedAllocation;
      }
      set
      {
        if (_myselectedAllocation == value)
          return;
        _myselectedAllocation = value;
        RaisePropertyChanged("SelectedAllocation");
      }
    }

    public TestBDO SelectedTest
    {
      get
      {
        return _test;
      }
      set
      {
        if (_test == value)
          return;
        _test = value;
        Status = "";
        if (_test != null)
          getAllocationBytestID(_test.TestID);
        RaisePropertyChanged("SelectedTest");
        DeleteTestCommand.RaiseCanExecuteChanged();
        SaveTestCommand.RaiseCanExecuteChanged();
        UpDateTestCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<TestAllocationBDO> Allocations
    {
      get
      {
        return _myallocations;
      }
      set
      {
        if (_myallocations == value)
          return;
        _myallocations = value;
        RaisePropertyChanged("Allocations");
      }
    }

    public ObservableCollection<TestBDO> Tests
    {
      get
      {
        return _tests;
      }
      set
      {
        if (_tests == value)
          return;
        _tests = value;
        RaisePropertyChanged("Tests");
      }
    }

    public RelayCommand AllocationProfileToExcelCommand { get; private set; }

    public RelayCommand AddTestCommand { get; private set; }

    public RelayCommand UpDateTestCommand { get; private set; }

    public RelayCommand DeleteTestCommand { get; private set; }

    public RelayCommand SaveTestCommand { get; private set; }

    public RelayCommand AllocateTestCommand { get; private set; }

    public RelayCommand SaveAllocationCommand { get; private set; }

    public RelayCommand UpdateAllocationCommand { get; private set; }

    public RelayCommand DeleteAllocationCommand { get; private set; }

    public RelayCommand SaveExcelAllocationsCommand { get; private set; }

    public RelayCommand NewProfileCommand { get; private set; }

    public RelayCommand SaveAProfileCommand { get; private set; }

    public RelayCommand UpdateProfileCommand { get; private set; }

    public RelayCommand DeleteProfileCommand { get; private set; }

    public TestsViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
      Tests = new ObservableCollection<TestBDO>();
      TestsProvider = (IIntelliboxResultsProvider) new TestsResultsProvider(_service);
      AllAllocations = new ObservableCollection<TestAllocationBDO>();
      if (!ApplicationSettings.Default.DBAvailable)
        return;
      RefreshTests();
      _myPeriods = _service.GetAllIntakeYears();
      IntakeYear = _service.GetCurrentYear();
      GetProfiles();
      Refresh_alloc();
      Refresh_ProfAllocs();
    }

    private void RegisterCommands()
    {
      AddTestCommand = new RelayCommand((Action) (() => CreateTest()));
      UpDateTestCommand = new RelayCommand((Action) (() => UpdateTest()), (Func<bool>) (() => canSaveTest()));
      SaveTestCommand = new RelayCommand((Action) (() => SaveTest()), (Func<bool>) (() => canSaveTest()));
      DeleteTestCommand = new RelayCommand((Action) (() => DeleteTest()), (Func<bool>) (() => canDeleteTest()));
      AllocateTestCommand = new RelayCommand((Action) (() => AllocateTest()), (Func<bool>) (() => canAllocateTest()));
      SaveAllocationCommand = new RelayCommand((Action) (() => SaveAllocation()), (Func<bool>) (() => canSaveAllocation()));
      DeleteAllocationCommand = new RelayCommand((Action) (() => DeleteAllocation()), (Func<bool>) (() => canSaveAllocation()));
      UpdateAllocationCommand = new RelayCommand((Action) (() => UpdateAllocation()), (Func<bool>) (() => canSaveAllocation()));
      SaveExcelAllocationsCommand = new RelayCommand((Action) (() => SaveAllocationsToExcel()));
      AllocationProfileToExcelCommand = new RelayCommand((Action) (() => AllocationProfileToExcel()));
      NewProfileCommand = new RelayCommand((Action) (() => CreateProfile()));
      SaveAProfileCommand = new RelayCommand((Action) (() => SaveAProfile()), (Func<bool>) (() => canSaveProfile()));
      DeleteProfileCommand = new RelayCommand((Action) (() => DeletingProfile()));
    }

    private void SaveAllocationsToExcel()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx|Data files (*.dat)|*.dat|All files(*.*)|*.*";
      bool? nullable = saveFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      string title = "Test Allocations";
      if (!_service.AllocationsToExcel(saveFileDialog.FileName))
        return;
      ModernDialog.ShowMessage("Allocation Data saved to excel file ", title, MessageBoxButton.OK);
    }

    private void Refresh_ProfAllocs()
    {
      ProfilesAllocations = new ObservableCollection<ProfileAllocationBDO>(_service.GetAllProfileAllocations()
                                                .Where(x => x.TestDate >= IntakeRecord.yearStart && x.TestDate <= IntakeRecord.yearEnd)
                                                .Select(m => m).ToList());
    }

    private void Refresh_alloc()
    {
      AllAllocations.Clear();
      IEnumerable<TestAllocationBDO> source = _service.GetAllTestAllocations()
                                                      .Where(x => x.TestDate >= IntakeRecord.yearStart && x.TestDate <= IntakeRecord.yearEnd)
                                                      .Select(m => m);
      List<TestProfileBDO> profs = _service.GetAllTestProfiles();
      AllAllocations = new ObservableCollection<TestAllocationBDO>(source.Where(i => !profs.Any(e => e.AllocationID == i.ID))
                                                                         .OrderBy(m => m.TestDate).ToList());
    }

    private void AllocationProfileToExcel()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx|Data files (*.dat)|*.dat|All files(*.*)|*.*";
      bool? nullable = saveFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      _service.SaveProfileAllocationsToExcel(saveFileDialog.FileName);
    }

    private void GetProfiles()
    {
      Profiles = new ObservableCollection<TestProfileBDO>(new ObservableCollection<TestProfileBDO>(_service.GetAllTestProfiles())
                                                                                                           .Where(x => x.Intake == IntakeYear)
                                                                                                           .Select(v => v).ToList()
                                                                                                           .GroupBy(e => e.Profile)
                                                                                                           .Select(gp => gp.First())
                                                                                                           .OrderBy(m => m.Profile)
                                                                                                           .ToList());
    }

    private void GetGroupProfiles()
    {
      GrpProfiles = new ObservableCollection<TestProfileBDO>(_service.getTestprofileByProfile(_myProfNumber));
    }

    private void SaveAProfile()
    {
      string message = "";
      _service.addTestProfile(SelectedProfile, ref message);
      BStatus = message;
      Refresh_alloc();
      Refresh_ProfAllocs();
    }

    private void CreateProfile()
    {
      SelectedProfile = (TestProfileBDO) null;
      _myProfile = new TestProfileBDO()
      {
        AllocationID = SelectedProfAlloc.ID
      };
      _myProfile.Intake = _intakeyear;
      SaveAProfileCommand.RaiseCanExecuteChanged();
    }

    private bool canSaveProfile()
    {
      bool flag = false;
      if (SelectedProfile != null)
        return true;
      return flag;
    }

    private void DeletingProfile()
    {
      string message = "";
      _service.deleteTestProfile(SelectedProfile, ref message);
      Status = message;
    }

    private void SaveAllocation()
    {
      string message = "";
      _service.addTestAllocation(SelectedAllocation, ref message);
      AStatus = message;
      getAllocationBytestID(SelectedTest.TestID);
      Refresh_alloc();
    }

    private bool canSaveAllocation()
    {
      if (SelectedAllocation != null && !string.IsNullOrWhiteSpace(SelectedAllocation.Client))
        return !string.IsNullOrWhiteSpace(SelectedAllocation.ClientType);
      return false;
    }

    private void DeleteAllocation()
    {
      if (SelectedAllocation == null)
        return;
            var result = ModernDialog.ShowMessage("Do you really want to delete this Test Allocation?", "Test Name: " + SelectedTest.TestName.ToString(), MessageBoxButton.YesNo);
      if (result.ToString()== "Yes")
      {
        string message = "";
        _service.deleteTestAllocation(SelectedAllocation, ref message);
        Status = message;
      }
      RefreshAllocations();
      Refresh_alloc();
      RefreshTests();
    }

    private void UpdateAllocation()
    {
      AStatus = "";
      string message = "";
      _service.updateTestAllocation(SelectedAllocation, ref message);
      ModernDialog.ShowMessage(message, "Update", MessageBoxButton.OK);
      AStatus = message;
      getAllocationBytestID(SelectedTest.TestID);
      Refresh_alloc();
    }

    private void RefreshAllocations()
    {
      Allocations.Clear();
      Allocations = new ObservableCollection<TestAllocationBDO>(_service.GetAllTestAllocations());
    }

    private void getAllocationBytestID(int testID)
    {
      Allocations = new ObservableCollection<TestAllocationBDO>(_service.getTestAllocationByTestID(testID)
                                                                .Where(x => x.TestDate >= IntakeRecord.yearStart && x.TestDate <= IntakeRecord.yearEnd)
                                                                .Select(m => m).OrderBy(v => v.TestDate).ToList());
    }

    private void AllocateTest()
    {
      SelectedAllocation = new TestAllocationBDO()
      {
        TestID = SelectedTest.TestID
      };
      SelectedAllocation.TestDate = DateTime.Now;
    }

    private bool canAllocateTest()
    {
      if (SelectedTest != null && !string.IsNullOrWhiteSpace(SelectedTest.TestName))
        return SelectedTest.TestID > 0;
      return false;
    }

    private void DeleteTest()
    {
          if (SelectedTest == null)
            return;
          var result = ModernDialog.ShowMessage("Do you really want to delete this Test?\n Tests affect other parts of the application \n and the NBT_Production Database ", "Test Name: " + SelectedTest.TestName.ToString(), MessageBoxButton.YesNo);
          if (result.ToString() == "Yes")
          {
                string message = "";
                _service.deleteTest(SelectedTest, ref message);
                Status = message;
          }
          RefreshTests();
    }

    private void RefreshTests()
    {
      Tests.Clear();
      Tests = new ObservableCollection<TestBDO>( _service.GetAllTests().OrderBy(t => t.TestName));
    }

    private void UpdateTest()
    {
      Status = "";
      string message = "";
      _service.updateTest(SelectedTest, ref message);
      ModernDialog.ShowMessage(message, "Update", MessageBoxButton.OK);
      Status = message;
      RefreshTests();
    }

    private void CreateTest()
    {
      SelectedTest = (TestBDO) null;
      SelectedTest = new TestBDO();
    }

    private void SaveTest()
    {
      string message = "";
      _service.addTest(SelectedTest, ref message);
      Status = message;
      RefreshTests();
    }

    private bool canSaveTest()
    {
      if (SelectedTest != null)
        return !string.IsNullOrWhiteSpace(SelectedTest.TestName);
      return false;
    }

    private bool canDeleteTest()
    {
      if (SelectedTest != null)
        return !string.IsNullOrWhiteSpace(SelectedTest.TestName);
      return false;
    }

    private void GetSelectedIntake()
    {
      IntakeRecord = _myPeriods.Where(x => x.Year == IntakeYear).Select(x => x).FirstOrDefault();
    }

    private void RefreshAll()
    {
      Refresh_ProfAllocs();
      Refresh_alloc();
    }
  }
}
