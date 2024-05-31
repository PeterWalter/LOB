// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.writers.ProcessViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using ClosedXML.Excel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using CETAP_LOB.Model.venueprep;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CETAP_LOB.ViewModel.writers
{
  public class ProcessViewModel : ViewModelBase
  {
    public const string NBTDOTPropertyName = "NBTDOT";
    public const string CollectionPropertyName = "Collection";
    public const string SelectedProfilePropertyName = "SelectedProfile";
    public const string ProfilesAllocationsPropertyName = "ProfilesAllocations";
    public const string SummaryPropertyName = "Summary";
    public const string VenuePropertyName = "Venue";
    private string testDate;
    private DateTime _myNBTdate;
    private ListCollectionView _collection;
    private IDataService _service;
    private int AQLE_Code;
    private int MATE_Code;
    private int AQLA_Code;
    private int MATA_Code;
    private TestBDO UsedTest;
    private ProfileAllocationBDO _selectedProfile;
    private ObservableCollection<ProfileAllocationBDO> _myProfAllocs;
    private ObservableCollection<VenueSummary> _mysummary;
    private ObservableCollection<ProcessList> _venue;

    public RelayCommand SaveToExcelCommand { get; private set; }

    public RelayCommand PackingListToExcelCommand { get; private set; }

    public DateTime NBTDOT
    {
      get
      {
        return _myNBTdate;
      }
      set
      {
        if (_myNBTdate == value)
          return;
        _myNBTdate = value;
        RaisePropertyChanged("NBTDOT");
      }
    }

    public ListCollectionView Collection
    {
      get
      {
        return _collection;
      }
      set
      {
        if (_collection == value)
          return;
        _collection = value;
        RaisePropertyChanged("Collection");
      }
    }

    public ProfileAllocationBDO SelectedProfile
    {
      get
      {
        return _selectedProfile;
      }
      set
      {
        if (_selectedProfile == value)
          return;
        _selectedProfile = value;
        RaisePropertyChanged("SelectedProfile");
        PackingListToExcelCommand.RaiseCanExecuteChanged();
        if (_selectedProfile == null)
          return;
        GetTestCodes();
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

    public ObservableCollection<VenueSummary> Summary
    {
      get
      {
        return _mysummary;
      }
      set
      {
        if (_mysummary == value)
          return;
        _mysummary = value;
        RaisePropertyChanged("Summary");
      }
    }

    public ObservableCollection<ProcessList> Venue
    {
      get
      {
        return _venue;
      }
      set
      {
        if (_venue == value)
          return;
        _venue = value;
        RaisePropertyChanged("Venue");
      }
    }

    public ProcessViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
      LoadData();
    }

    private void InitializeModels()
    {
      Venue = new ObservableCollection<ProcessList>();
      Summary = new ObservableCollection<VenueSummary>();
    }

    private void RegisterCommands()
    {
          SaveToExcelCommand = new RelayCommand(SaveListToExcel);
          PackingListToExcelCommand = new RelayCommand(() => PackingList(), () => CanSave());
    }

    private bool CanSave()
    {
        return SelectedProfile != null;
    }

    private void GetTestCodes()
    {
          UsedTest = _service.getTestByName(_selectedProfile.AQLE);
          AQLE_Code = UsedTest.TestCode;
          UsedTest = _service.getTestByName(_selectedProfile.MATE);
          MATE_Code = UsedTest.TestCode;
            UsedTest = _service.getTestByName(_selectedProfile.AQLA);
            AQLA_Code = UsedTest.TestCode;
            UsedTest = _service.getTestByName(_selectedProfile.MATA);
            MATA_Code = UsedTest.TestCode;
        }

    private void LoadData()
    {
          var DbVenues = _service.GetAllvenues(); // get DB venues

          var processdata = _service.Processdata(); //

          NBTDOT = processdata.Select(x => x.DOT).FirstOrDefault<DateTime>(); // get test dates

          ProfilesAllocations = new ObservableCollection<ProfileAllocationBDO>(_service.GetProfileAllocationsByDate(NBTDOT));

            // List<ProcessList> venues = new List<ProcessList>();
            var venues1 = (from ss in processdata
                           join x in DbVenues on ss.Venue.Trim() equals x.WebSiteName
                           orderby ss.Venue, ss.Language ascending, ss.Tests descending, ss.Surname ascending, ss.FirstName
                           select new
                           {

                               Surname = ss.Surname.ToUpper(),
                               Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ss.FirstName.ToLower()),
                               Regno = 0,
                               NBT = ss.Reference,
                               ID = (ss.SAID.Trim() != "" ? ss.SAID : ss.ForeignID),
                               AQL = (ss.Language == "English" ? "E" : "A"),
                               Maths = (ss.Tests == "AQL" ? "" : ss.Language == "English" ? "E" : "A"),
                               Paid = ss.Paid,
                               Venue = (ss.Venue != "" ? x.ShortName : "Unknown Venue"),
                               Home = ss.HTelephone,
                               Mobile = ss.Mobile,
                               EMail = ss.Email,
                               DOT = ss.DOT,
                               RegDate = ss.RegDate,
                               CreationDate = ss.CreationDate,
                               VenueCode = x.VenueCode

                           }).ToList();
            int count = 1;
            string c = "";

            foreach (var b in venues1)

            {
                ProcessList a = new ProcessList();
                a.AQL = b.AQL;
                a.ID = b.ID;
                a.NBT = b.NBT;
                a.Maths = b.Maths;
                a.Name = b.Name;
                a.Surname = b.Surname;
                a.Venue = b.Venue;
                a.Paid = b.Paid;
                if (c != b.Venue) count = 1;
                a.RegNo = count;
                a.FullName = b.Surname + ", " + b.Name;
                a.Home = b.Home;
                a.Mobile = b.Mobile;
                a.DOT = b.DOT;
                a.EMail = b.EMail;
                a.creationDate = b.CreationDate;
                a.regDate = b.RegDate;
                // add record to collection of records
                Venue.Add(a);
                // provide c with a venue name
                c = b.Venue;
                count++;
            }
            # region summary data for packing list
            var summary = from x in venues1
                          orderby x.Venue ascending
                          group x by x.Venue into venueGroups

                          select new
                          {
                              TestVenue = venueGroups.Key,

                              Writers = venueGroups.Count(),
                              AQL =
                                    from Vg in venueGroups
                                    where (Vg.AQL != "")
                                    group Vg by Vg.AQL into AQLGroups

                                    select new
                                    {
                                        Language = AQLGroups.Key,
                                        AQLCount = AQLGroups.Count(),

                                    },
                              Maths =
                                     from mt in venueGroups
                                     where (mt.Maths != "")
                                     group mt by mt.Maths into groupMaths
                                     select new
                                     {
                                         MathsLanguage = groupMaths.Key,
                                         Numbers = groupMaths.Count()
                                     },
                              VenueCode =
                                    from vc in venueGroups
                                    group vc by vc.VenueCode into codes
                                    select new
                                    {
                                        vcode = codes.Key
                                    },
                              TestDate =
                                    from vc in venueGroups
                                    group vc by vc.DOT into dates
                                    select new
                                    {
                                        DOT = dates.Key
                                    }
                          };

            foreach (var x in summary)
            {
                VenueSummary vensum = new VenueSummary();

                vensum.TotalWriters = x.Writers;
                vensum.Venue = x.TestVenue;

                foreach (var z in x.AQL)
                {
                    if (z.Language == "E") vensum.AQL_E = z.AQLCount;
                    if (z.Language == "A") vensum.AQL_A = z.AQLCount;

                }
                foreach (var m in x.Maths)
                {
                    if (m.MathsLanguage == "E") vensum.Maths_E = m.Numbers;
                    if (m.MathsLanguage == "A") vensum.Maths_A = m.Numbers;
                }
                foreach (var cc in x.VenueCode)
                {
                    vensum.CentreCode = cc.vcode;
                }
                vensum.Walkin_E = HelperUtils.RoundUpWalkIn(vensum.AQL_E);
                vensum.Walkin_A = HelperUtils.RoundUpWalkIn(vensum.AQL_A);
                vensum.AQLE_S = HelperUtils.RoundAmount(vensum.AQL_E) + vensum.Walkin_E;
                vensum.AQLA_S = HelperUtils.RoundAmount(vensum.AQL_A) + vensum.Walkin_A;
                vensum.MATE_S = vensum.Maths_E > 0 ? HelperUtils.RoundAmount(vensum.Maths_E) + 10 : 0;
                vensum.MATA_S = vensum.Maths_A > 0 ? HelperUtils.RoundAmount(vensum.Maths_A) + 10 : 0;
                foreach (var dot in x.TestDate)
                {
                    vensum.TestDate = dot.DOT;

                }

                if (vensum.Maths_E > 0 && vensum.Maths_E < 5)
                {
                    vensum.MATE_S = 10;
                }

                if (vensum.AQL_E == 0)
                {
                    vensum.Walkin_E = 0;
                    vensum.AQLE_S = 0;
                }
                if (vensum.Maths_A > 0 && vensum.Maths_A < 5)
                {
                    vensum.MATA_S = 10;
                }
                if (vensum.AQL_A == 0)
                {
                    vensum.Walkin_A = 0;
                    vensum.AQLA_S = 0;
                }

                if (vensum.AQL_A > 0 && vensum.AQL_A < 5)
                {
                    vensum.AQLA_S = 10;
                }

                if (vensum.AQL_E > 0 && vensum.AQL_E < 5)
                {
                    vensum.AQLE_S = 10;
                }

                vensum.AQLE_Batch = HelperUtils.YourChange(vensum.AQLE_S);
                vensum.AQLA_Batch = HelperUtils.YourChange(vensum.AQLA_S);
                vensum.MATE_Batch = HelperUtils.YourChange(vensum.MATE_S);
                vensum.MATA_Batch = HelperUtils.YourChange(vensum.MATA_S);
                vensum.TotalSent = vensum.AQLE_S + vensum.AQLA_S;
                Summary.Add(vensum);
                // int count1 = Summary.Count();
            }

            //  ProfilesAllocations = _service.GetProfileAllocationsByDate(NBTDOT);
            #endregion

            //  VenueList = new ObservableCollection<ProcessList>(venues);
            Collection = new ListCollectionView(_venue);
            Collection.GroupDescriptions.Add(new PropertyGroupDescription("Venue"));
        }

        private void SaveListToExcel()
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.DefaultExt = ".xlsx";
          saveFileDialog.Filter = "Excel FilesinFolder (*.xlsx)|*.xlsx|CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
          bool? nullable = saveFileDialog.ShowDialog();
          if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
            return;
          GenerateExcelWriters(saveFileDialog.FileName);
    }

    private void GenerateExcelWriters(string filename)
    {
          var wb = new XLWorkbook();

          var processLists = Venue.GroupBy(i => i.Venue).Select(group => group.First());

          int position = 1;
          foreach (var rws in processLists)
          {
            //ProcessList rws = processList;
            try
            {
                  string VenueN = rws.Venue.ToString().Trim();

                  if (VenueN.Length > 30) // names for excel TABS
                        VenueN = VenueN.Substring(0, 30);

                  VenueN = HelperUtils.RemoveWorksheetChars(VenueN);
                    string str = "NBT TEST - " + rws.DOT.Date.ToLongDateString() + ",  "; //Environment.NewLine;
                  testDate = rws.DOT.Date.ToLongDateString();
                  NBTDOT = rws.DOT;
                  string text = str + " CHECK-IN SHEET : REGISTERED WRITERS " + ",  " + VenueN.ToUpper();
                  if (text.Length > 120)
                    text = text.Substring(0, 120);

                  var wb1 = new XLWorkbook();
                  var ws = wb.Worksheets.Add(VenueN);
                  var ws1 = wb1.Worksheets.Add(VenueN);

                    // set headers
                  ws1.PageSetup.Header.Center.Clear(XLHFOccurrence.AllPages);
                  ws1.PageSetup.Header.Center.AddText(text).SetBold();
                  ws1.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                  ws1.PageSetup.AdjustTo(100);
                  ws1.PageSetup.PaperSize = XLPaperSize.A4Paper;
                  ws1.PageSetup.VerticalDpi = 600;
                  ws1.PageSetup.HorizontalDpi = 600;
                  ws1.PageSetup.PrintAreas.Add("A1:F51");
                  ws1.PageSetup.SetRowsToRepeatAtTop(1, 1);
                  ws1.PageSetup.Margins.Top = 0.3;
                  ws1.PageSetup.Margins.Bottom = 0.0;
                  ws1.PageSetup.Margins.Left = 0.2;
                  ws1.PageSetup.Margins.Right = 0.2;
                  ws1.PageSetup.Margins.Footer = 0.0;
                  ws1.PageSetup.Margins.Header = 0.0;
                  ws1.PageSetup.CenterHorizontally = true;
                  ws1.PageSetup.CenterVertically = true;
                  ws1.PageSetup.PageOrder = XLPageOrderValues.DownThenOver;
                  ws1.PageSetup.ShowGridlines = true;

                    // copy writer's template to workbook for  writers
                    ws = wb.Worksheet(position);
                    ws1 = wb1.Worksheet(1);
                    var workdata = Venue.Where(wsd => wsd.Venue == rws.Venue).Select(wsd => new
                          {
                            Surname = wsd.Surname,
                            Name = wsd.Name,
                            Reg = wsd.RegNo.ToString("000"),
                            FullName = wsd.FullName,
                            ID = wsd.ID,
                            AQL = wsd.AQL,
                            Mat = wsd.Maths,
                            Venue = wsd.Venue,
                            NBT = wsd.NBT,
                            DOT = wsd.DOT,
                            Mobile = wsd.Mobile,
                            Home = wsd.Home,
                            email = wsd.EMail,
                            regdate = wsd.regDate,
                            creationdate = wsd.creationDate
                          });

                    int lastRow = workdata.Count() + 1;
                    string rangeAddress = "A2:F" + lastRow;
                    var Row1 = ws.Row(1);

                    Row1.Style.Font.Bold = true;
                    Row1.Style.Font.FontSize = 12.0;
                  ws.Cell(1, 1).Value =  "Surname";
                  ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                  ws.Cell(1, 2).Value =  "First Name";
                  ws.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  ws.Cell(1, 3).Value =  "Reg #";
                  ws.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  ws.Cell(1, 4).Value =  "FULL NAME";
                  ws.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  ws.Cell(1, 5).Value =  "ID";
                  ws.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                  ws.Cell(1, 6).Value =  "AQL";
                  ws.Cell(1, 7).Value =  "MAT";
                  ws.Cell(1, 8).Value =  "Venue";
                  ws.Cell(1, 9).Value =  "Reference";
                  ws.Cell(1, 10).Value =  "Date of Test";
                  ws.Cell(1, 11).Value =  "Mobile";
                  ws.Cell(1, 12).Value =  "Home";
                  ws.Cell(1, 13).Value =  "EMail";
                  ws.Cell(1, 14).Value = "Date of Registration";
                  ws.Cell(1, 15).Value = "Creation Date";
                    //ws.Cell(2, 1).InsertData((IEnumerable) source);
                    var rangeWebWriters = ws.Cell(2, 1).InsertData(workdata);

                    var workdata1 = Venue.Where(wsd => wsd.Venue == rws.Venue).Select(wsd => new
                          {
                            Reg = wsd.RegNo.ToString("000"),
                            FullName = wsd.FullName,
                            ID = wsd.ID,
                            AQL = wsd.AQL,
                            Mat = wsd.Maths,
                            Signature = "         "
                          });
                    var col1 = ws1.Column("A");
                    var col2 = ws1.Column("C");
                    var col3 = ws1.Column("D");
                    var col4 = ws1.Column("E");
                    col1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    col2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    col3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    col4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                  var HeaderRange = ws1.Range("A1:F1");
                  var DataRange = ws1.Range(rangeAddress);
                    HeaderRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    HeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    HeaderRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    HeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    DataRange.Style.Font.FontSize = 10.0;
                    DataRange.Style.Font.FontName = "Calibri";
                    DataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    DataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    DataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                  var RowW = ws1.Row(1);
                  var Row2 = ws1.Row(2);
                  RowW.Style.Font.Bold = false;
                  RowW.Style.Font.FontSize = 9.0;
                  RowW.Style.Fill.BackgroundColor = XLColor.Cyan;
                  RowW.Height = 20;
                  ws1.Rows(2, lastRow).Height = 16.0;
                  ws1.Rows(2, lastRow).Style.Font.FontSize = 10.0;
                  ws1.Column(1).Width = 5.0;
                  ws1.Column(2).Width = 30.0;
                  ws1.Column(3).Width = 16.0;
                  ws1.Column(4).Width = 4.9;
                  ws1.Column(5).Width = 4.9;
                 ws1.Column(6).Width = 30.0;
                  ws1.Cell(1, 1).Value =  "#";
                  ws1.Cell(1, 2).Value =  "Full Name";
                  ws1.Cell(1, 3).Value =  "ID #";
                    ws1.Cell(1, 4).Value =  "AQL";
                    ws1.Cell(1, 5).Value =  "MAT";
                    ws1.Cell(1, 6).Value =  "SIGN";
                    ws1.Cell(2, 1).InsertData(workdata1);
                  string file = Path.Combine(Path.GetDirectoryName(filename), rws.DOT.Date.ToLongDateString() + " " + VenueN + ".xlsx");
                  wb1.SaveAs(file);
                  position++;
            }
            catch (Exception ex)
            {
              throw ex;
            }
          }
          wb.SaveAs(filename);
          int num = (int) MessageBox.Show("All files saved");
    }

    private void PackingList()
    {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.DefaultExt = ".xlsx";
          saveFileDialog.Filter = "Excel FilesinFolder (*.xlsx)|*.xlsx|CSV FilesinFolder (*.csv)|*.csv|Data files (*.dat)|*.dat|Text files (*.txt|*.txt";
          bool? nullable = saveFileDialog.ShowDialog();
          if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
            GeneratePackingList(saveFileDialog.FileName);

          MessageBox.Show("All files saved");
    }

    private void GeneratePackingList(string FileName)
    {
          var wb = new XLWorkbook("PackingList.xlsx");
          var wsSummary = wb.Worksheet(2);
           var summaryData = Summary.Select(ss => new
                  {
                    VenueName = HelperUtils.RemoveWorksheetChars(ss.Venue),
                    Total = ss.TotalWriters,
                    CentreCode = ss.CentreCode.ToString("00000"),
                    walkins = "",
                    Booked = "",
                    regNotOnlist = "",
                    AQLE_Reg = ss.AQL_E,
                    AQLE_Sent = ss.AQLE_S,
                    E_Walkin = ss.Walkin_E,
                    AQLEBatches = ss.AQLE_Batch,
                    AQLA_Reg = ss.AQL_A,
                    AQLa_Send = ss.AQLA_S,
                    A_Walkin = ss.Walkin_A,
                    AQLABatches = ss.AQLA_Batch,
                    Mate_reg = ss.Maths_E,
                    Mate_send = ss.MATE_S,
                    Mate_Batch = ss.MATE_Batch,
                    Mata_reg = ss.Maths_A,
                    Mata_Send = ss.MATA_S,
                    Mata_Batch = ss.MATA_Batch,
                    Pencils = "",
                    Erasers = "",
                    Sharpeners = "",
                    Totalsent = ss.TotalSent,
                    TotalWriters = ss.TotalWriters,
                    total_walkins =  ss.Walkin_A + ss.Walkin_E
                  });

            int count = summaryData.Count();
          wsSummary.Cell("A1").Value =  testDate;
          wsSummary.Cell(4, 1).InsertData((IEnumerable) summaryData);
          int num = 2;
          foreach (var data in summaryData)
          {
                ++num;
                string str = data.VenueName.Trim();
                if (data.VenueName.Length > 30)
                  str = data.VenueName.Substring(0, 30);
                wb.Worksheet(1).CopyTo(str);
                var ws = wb.Worksheet(str);
                ws.Cell("B8").Value =  SelectedProfile.TestDate;
                ws.Cell("E1").Value =  SelectedProfile.Client.ToUpper();
                ws.Cell("C13").Value =  data.AQLEBatches;
                ws.Cell("C14").Value =  data.Mate_Batch;
                ws.Cell("C15").Value =  data.AQLABatches;
                ws.Cell("C16").Value =  data.Mata_Batch;
                ws.Cell("D13").Value =  data.AQLE_Sent;
                ws.Cell("D14").Value =  data.Mate_send;
                ws.Cell("D15").Value =  data.AQLa_Send;
                ws.Cell("D16").Value =  data.Mata_Send;
                ws.Cell("E8").Value =  data.CentreCode;
                ws.Cell("G2").Value =  str;
                ws.Cell("D17").Value =  data.TotalWriters;
                // ws.Cell("D18").Value =  data.E_Walkin;
                //  ws.Cell("D19").Value =  data.A_Walkin;
                ws.Cell("D18").Value = data.total_walkins;
                ws.Cell("O5").Value =  AQLE_Code;
                ws.Cell("O6").Value =  MATE_Code;
                ws.Cell("O7").Value =  AQLA_Code;
                ws.Cell("O8").Value =  MATA_Code;
                ws.Cell("B13").Value =  SelectedProfile.AQLE;
                ws.Cell("B14").Value =  SelectedProfile.MATE;
                ws.Cell("B15").Value =  SelectedProfile.AQLA;
                ws.Cell("B16").Value =  SelectedProfile.MATA;
                ws.Cell("H8").Value =  SelectedProfile.Profile;
          }
          wb.CalculateMode = XLCalculateMode.Auto;
          wb.SaveAs(FileName);
    }
  }
}
