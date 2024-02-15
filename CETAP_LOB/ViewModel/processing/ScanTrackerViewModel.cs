

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using ClosedXML.Excel;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace CETAP_LOB.ViewModel.processing
{
    public class ScanTrackerViewModel : ViewModelBase
  {
    public const string PeriodsPropertyName = "Periods";
    public const string IntakeRecordPropertyName = "IntakeRecord";
    public const string TrackersPropertyName = "Trackers";
    private IDataService _service;
    private List<IntakeYearsBDO> _myPeriods;
    private IntakeYearsBDO _intakerecord;
    private ObservableCollection<ScanTrackerBDO> _mytrackers;

    public RelayCommand SavetoExcelCommand { get; private set; }

    public RelayCommand FolderBowserCommand { get; private set; }
    //public RelayCommand SelectVenue { get; private set; }
    public RelayCommand  RefreshDataCommand { get; private set; }
    public RelayCommand LoadDataCommand { get; private set; }

        private const string SelectedVenuePropertyName = "SelectedVenue";
        private VenueBDO _selectedVenue;
        public VenueBDO SelectedVenue
        {
            get
            {
                return _selectedVenue;
            }
            set
            {
                if (_selectedVenue == value) return;
                _selectedVenue = value;
                RaisePropertyChanged("SelectedVenue");
            }
        }

        public List<VenueBDO> Venues { get; private set; }
       
        private const string SelectedDate = "SelectedProcessDate";

        private DateTime _selectedProcessDate;
        public DateTime SelectedProcessDate
        {
            get
            {
                return _selectedProcessDate;
            }
            set
            {
                if (_selectedProcessDate == value) return;
                _selectedProcessDate = value;
                RaisePropertyChanged("SelectedProcessDate");
            }
        }
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

    public ObservableCollection<ScanTrackerBDO> Trackers
    {
      get
      {
        return _mytrackers;
      }
      set
      {
        if (_mytrackers == value)
          return;
        _mytrackers = value;
        RaisePropertyChanged("Trackers");
      }
    }

    public ScanTrackerViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
    }

    private void InitializeModels()
    {
            _selectedProcessDate = DateTime.Now;
        List<ScanTrackerBDO> allTracks = _service.GetAllTracks();
        _myPeriods = _service.GetAllIntakeYears();
        _intakerecord = _myPeriods.Where(m => m.Year == ApplicationSettings.Default.IntakeYear).FirstOrDefault();
            Trackers = new ObservableCollection<ScanTrackerBDO>(allTracks
                       .Where(x => !x.FileName.Contains("BIO") && x.DateBatched >= _intakerecord.yearStart && x.DateBatched <= _intakerecord.yearEnd)
                       .OrderByDescending(q => q.DateBatched));
            var Venues1 = _service.GetAllvenues();
            Venues = Venues1.OrderBy(x => x.VenueName).Select(x => x).ToList();

    }

    private void RegisterCommands()
    {
        SavetoExcelCommand = new RelayCommand(SaveToExcelFile);
            RefreshDataCommand = new RelayCommand(() => RefreshData());
            LoadDataCommand = new RelayCommand(() => LoadData());
    }

     private void LoadData()
        {
            var myDate = _selectedProcessDate;
            var myVenue = _selectedVenue;
            string venueCode = myVenue.VenueCode.ToString("00000");
            var MyList = Trackers
                            .Where(x => x.TestDate == myDate && x.FileName.Substring(7, 5) == venueCode);
            Trackers = new ObservableCollection<ScanTrackerBDO>(MyList);
        }

    private void RefreshData()
        {
            InitializeModels();
        }
    private void SaveToExcelFile()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.DefaultExt = ".xlsx";
      saveFileDialog.Filter = "Excel FilesinFolder (*.xlsx)|*.xlsx|CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
      bool? nullable = saveFileDialog.ShowDialog();
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        return;
      GenerateExcelFile(saveFileDialog.FileName);
    }

    private void GenerateExcelFile(string filename)
    {
      var wb = new XLWorkbook();
      var ws = wb.Worksheets.Add("Scan Tracker");
      var list = Trackers.OrderByDescending(x => x.DateBatched).ThenBy(v => v.BatchedBy).Select(x => x).ToList();

      var row1 = ws.Row(1);
      row1.Style.Font.Bold = true;
      row1.Style.Font.FontSize = 12.0;
      ws.Cell(1, 1).Value =  "Batch ID";
      ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
      ws.Cell(1, 2).Value =  "Batch Name";
      ws.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      ws.Cell(1, 3).Value =  "Batched By";
      ws.Cell(1, 4).Value =  "Date Batched";
      ws.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      ws.Cell(1, 5).Value =  "Counted Answer Sheets";
      ws.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      ws.Cell(1, 6).Value =  "Scan Date";
      ws.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      ws.Cell(1, 7).Value =  "Records Scanned";
      ws.Cell(1, 8).Value =  "Date Edited";
      ws.Cell(1, 9).Value =  "Edited Records";
      ws.Cell(1, 10).Value =  "Date QA'ed";
      ws.Cell(1, 11).Value =  "Records QA";
      ws.Cell(1, 12).Value =  "Date sent for Scoring";
      ws.Cell(1, 13).Value =  "Amount Scored";
      ws.Cell(1, 14).Value =  "Date Scores compiled";
      ws.Cell(1, 15).Value =  "Count Compiled";
      ws.Cell(1, 16).Value =  "Test Date";
      ws.Cell(1, 17).Value =  "Date File Modified";
      ws.Cell(1, 18).Value =  "Row Version";
      ws.Cell(2, 1).InsertData((IEnumerable) list);
      DateTime.Now.ToShortDateString();
      wb.SaveAs(filename);
      int num = (int) ModernDialog.ShowMessage("Excel File has been saved to folder", "Save File!!", MessageBoxButton.OK, (Window) null);
    }
  }
}
