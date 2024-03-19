using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Mapping;
using CETAP_LOB.Model.easypay;
using CETAP_LOB.Model.QA;
using CETAP_LOB.Model.scoring;
using CETAP_LOB.Model.venueprep;
using ClosedXML.Excel;
//using CommonLibrary;
using CsvHelper;
using FileHelpers;
using FirstFloor.ModernUI.Windows.Controls;
using LinqStatistics;
using LINQtoCSV;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Transactions;
using CETAP_LOB.Model.Composite;
using Syncfusion.Licensing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Grid;
using DocumentFormat.OpenXml.Packaging;
using Syncfusion.Pdf.Security;
//using Syncfusion.Pdf.Grid;
//using Syncfusion.Licensing;
//using Syncfusion.Pdf.Security;
//using Syncfusion.Drawing;
//using Syncfusion.Drawing;

//using System.Windows.Input;
//using DocumentFormat.OpenXml.Bibliography;
//using CETAP_LOB.View.Composite;
//using DocumentFormat.OpenXml.Office.CustomXsn;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
//using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
// using System.Windows.Forms;

namespace CETAP_LOB.Model
{

    public class DataService : IDataService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Site = "ftp.transwitch.co.za";
        string user = "easy3100";
        string password = "xe6edRe8";
        Boolean canAddFile = false;
        string EOFDatFile;
        static DateTime TestDate;
        ObservableCollection<IntakeYear> Intake;
        ObservableCollection<CompositBDO> AllScores;
        ObservableCollection<VenueBDO> AllVenues;
        //static List<SurnameBDO> QA_Surnames;
        IntakeYear CurrentIntake;
        int CurrentYear;
        Stopwatch Timer = new Stopwatch();
        private ObservableCollection<FullComposite> FComposite = new ObservableCollection<FullComposite>();
        private ObservableCollection<LogisticsComposite> LComposite = new ObservableCollection<LogisticsComposite>();
        public ObservableCollection<WebWriters> WritersList1;
        private bool hasrecords;
        private Dir_categories myDir;
        private ObservableCollection<AnswerSheetBio> BIO;
        private ObservableCollection<AQL_Score> AQL;
        private ObservableCollection<ScoreStats> MatStatistics;
        private ObservableCollection<ScoreStats> AQLStatistics;
        private ObservableCollection<ScoreStats> AnswerSheetBioStatistics;
        private ObservableCollection<MAT_Score> MAT;
        private ObservableCollection<CompositBDO> Composite;
        private ObservableCollection<CompositBDO> Scores;
        private ObservableCollection<CompositBDO> ModeratedScores;
        private ObservableCollection<QADatRecord> QaData;
        public ObservableCollection<BenchMarkLevelsBDO> benchmarkLevels;

        public bool CheckForDatabase(ref string message)
        {
            bool ret = false;
            using (var context = new CETAPEntities())
                try
                {
                    context.Database.Connection.Open();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log.Warn("Database not available", ex);
                    message = "Database is not available!!  ";
                }
            ApplicationSettings.Default.DBAvailable = ret;
            ApplicationSettings.Default.Save();

            return ret;
        }

        public List<IntakeYearsBDO> GetAllIntakeYears()
        {
            var periods = new List<IntakeYearsBDO>();
            using (var context = new CETAPEntities())
            {
                var period = context.IntakeYears.ToList();
                foreach (var yr in period)
                {
                    var yrBDO = new IntakeYearsBDO();
                    IntakeYearDalToIntakeYearBDO(yr, yrBDO);
                    periods.Add(yrBDO);
                }
            }
            return periods;
        }

        public ObservableCollection<IntakeYear> GetIntakes()
        {

            using (var context = new CETAPEntities())
            {
                var period = context.IntakeYears.ToList();
                Intake = new ObservableCollection<IntakeYear>(period);
                var today = DateTime.Now.Date;
                CurrentIntake = period.Where(x => x.yearStart < today && x.yearEnd > today).Select(m => m).FirstOrDefault();

                if (CurrentIntake != null) 
                { 
                    CurrentYear = CurrentIntake.Year; 
                }
                else
                {
                    CurrentYear = ApplicationSettings.Default.IntakeYear; 
                }

            }
            return Intake;
        }
        public int GetCurrentYear()
        {
            if (CurrentYear == null) return DateTime.Now.Year;
            return CurrentYear;
        }

        public IntakeYearsBDO GetIntakeRecord(int year)
        {
            var myYear = new IntakeYearsBDO();
            using (var context = new CETAPEntities())
            {
                var maYear = context.IntakeYears.Where(x => x.Year == year).Select(m => m).FirstOrDefault();
                IntakeYearDalToIntakeYearBDO(maYear, myYear);
            }
            return myYear;
        }

        public void GetIntakeBenchmarks()
        {
            int year = ApplicationSettings.Default.IntakeYear;

          //  List<BenchMarkLevelsBDO> mybenchmarks = new List<BenchMarkLevelsBDO>();
            benchmarkLevels = new ObservableCollection<BenchMarkLevelsBDO>();
            using (var context = new CETAPEntities())
            {
               var Dbbenchmarks = context.BenchmarkLevels.Where(x => x.StartIntakeYear <= year && x.EndIntakeYear >= year).Select(x => x).ToList();

                foreach(BenchmarkLevel mark in Dbbenchmarks)
                {
                    BenchMarkLevelsBDO mybmark = new BenchMarkLevelsBDO();  
                    benchmarklevelDALToBenchmarkLevelBDO(mark, mybmark);
                    benchmarkLevels.Add(mybmark);
                }
            }
          //  return benchmarkLevels;
        }

        private void benchmarklevelDALToBenchmarkLevelBDO(BenchmarkLevel mark, BenchMarkLevelsBDO mybmark)
        {
           mybmark.YearSet = mark.YearSet;
            mybmark.StartIntakeYear = mark.StartIntakeYear;
            mybmark.YearSet = mark.YearSet;
            mybmark.EndIntakeYear = mark.EndIntakeYear;
            mybmark.yearID = mark.yearID;
            mybmark.QL_BU = mark.QL_BU;
            mybmark.QL_PL = mark.QL_PL;
            mybmark.QL_BL = mark.QL_BL; 
            mybmark.QL_PU = mark.QL_PU; 
            mybmark.QL_IL = mark.QL_IL;
            mybmark.QL_IU = mark.QL_IU;
            mybmark.MAT_IL = mark.MAT_IL;
            mybmark.MAT_BU = mark.MAT_BU;
            mybmark.MAT_PU = mark.MAT_PU;   
            mybmark.MAT_IU = mark.MAT_IU;
            mybmark.MAT_PL = mark.MAT_PL;   
            mybmark.MAT_BL = mark.MAT_BL;
            mybmark.AL_BL = mark.AL_BL;
            mybmark.AL_BU = mark.AL_BU;
            mybmark.AL_IL = mark.AL_IL;
            mybmark.AL_IU = mark.AL_IU;
            mybmark.AL_PL = mark.AL_PL;
            mybmark.AL_PU = mark.AL_PU;
            mybmark.Type = mark.Type;
        }

        public List<UserBDO> GetAllUsers()
        {
            List<UserBDO> myUsers = new List<UserBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                    try
                    {
                        List<User> DbUsers = context.Users.Select(x => x).ToList();
                        foreach (User person in DbUsers)
                        {
                            UserBDO M_User = new UserBDO();
                            UserDALToUserBDO(M_User, person);
                            myUsers.Add(M_User);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
            }
            return myUsers;
        }


        #region testVenue
        public VenueBDO GetTestVenue(int venuecode)
        {
            VenueBDO venueBDO = null;
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    TestVenue venue = context.TestVenues.Where(x => x.VenueCode == venuecode).FirstOrDefault();

                    if (venue != null)
                    {
                        venueBDO = new VenueBDO();
                        TestVenueToVenueBDO(venueBDO, venue);

                    }

                }
            }
            return venueBDO;
        }

        private void LoadNBTVenues() // need to load all venues in memory to make processing of Composites faster
        {
            AllVenues = new ObservableCollection<VenueBDO>(GetAllvenues().OrderBy(x => x.VenueCode));

        }
        public List<VenueBDO> GetAllvenues()
        {
            List<VenueBDO> venuesBDO = new List<VenueBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    List<TestVenue> venues = (from a in context.TestVenues
                                              select a).ToList();
                    foreach (var v in venues)
                    {
                        VenueBDO vbdo = new VenueBDO();
                        TestVenueToVenueBDO(vbdo, v);
                        // vbdo.IsDirty = false;
                        //   vbdo.Province = getProvinceByID(vbdo.ProvinceID);

                        venuesBDO.Add(vbdo);
                    }

                }
            }
            return venuesBDO;
        }
        public bool updateTestVenue(VenueBDO testVenue, ref string message)
        {
            bool ret = false;
            message = "TestVenue successfully updated";

            using (var context = new CETAPEntities())
            {
                context.Database.CommandTimeout = 120;
                try
                {
                    var ID = testVenue.VenueCode;
                    TestVenue venue = context.TestVenues.Where(x => x.VenueCode == ID).FirstOrDefault();

                    if (venue != null)
                    {
                        ret = true;
                        context.TestVenues.Remove(venue);

                        //update testVenue
                        venue.VenueCode = testVenue.VenueCode;
                        venue.ShortName = testVenue.ShortName;
                        venue.RowVersion = testVenue.RowVersion;
                        venue.ProvinceID = testVenue.ProvinceID;
                        venue.Place = testVenue.Place;
                        venue.Comments = testVenue.Description;
                        venue.VenueName = testVenue.VenueName;
                        venue.VenueType = testVenue.VenueType;
                        venue.WebsiteName = testVenue.WebSiteName;
                        venue.Capacity = testVenue.Capacity;
                        venue.Room = testVenue.Room;
                        venue.Available = testVenue.Available;
                        venue.RowGuid = testVenue.RowGuid;
                        venue.DateModified = DateTime.Now;

                        context.TestVenues.Attach(venue);
                        context.Entry(venue).State = System.Data.Entity.EntityState.Modified;
                        int num = context.SaveChanges();

                        testVenue.RowVersion = venue.RowVersion;

                    }
                    else
                    {
                        message = "No Venue with Code " + testVenue.VenueCode;
                    }

                }
                catch (Exception ex)
                {

                    message += " :  " + ex.InnerException.ToString();
                    //  ModernDialog.ShowMessage(message, "Batch not saved", MessageBoxButton.OK);
                }

            }
            return ret;
        }

        public bool deleteTestVenue(int venueCode, ref string message)
        {
            message = "Test Venue successfully deleted";
            bool ret = false;

            using (var context = new CETAPEntities())
            {
                //look for venue
                TestVenue venueInDB = (from p in context.TestVenues
                                       where (p.VenueCode == venueCode)
                                       select p).FirstOrDefault();
                // if venue in database
                if (venueInDB != null)
                {
                    context.TestVenues.Remove(venueInDB);
                    int num = context.SaveChanges();
                    ret = true;
                }
                else
                {
                    message = "No such venue in database";
                    ret = false;
                }

            }
            return ret;
        }

        public bool addTestVenue(VenueBDO venueBDO, ref string message)
        {
            bool ret = false;
            int code = venueBDO.VenueCode;
            using (var context = new CETAPEntities())
            {
                context.Database.CommandTimeout = 120;
                try

                {
                    //look for venue
                    TestVenue venueInDB = context.TestVenues.Where(x => x.VenueCode == code).FirstOrDefault();

                    //Add TestVenue
                    if (venueInDB != null)
                    {
                        ret = false;
                        message = "Venue with code " + code + " is already available";
                    }
                    else
                    {
                        // var VenueInDB = Maps.
                        var VenueInDB = new TestVenue();

                        VenueInDB.VenueCode = venueBDO.VenueCode;
                        VenueInDB.VenueName = venueBDO.VenueName;
                        VenueInDB.ShortName = venueBDO.ShortName;
                        VenueInDB.WebsiteName = venueBDO.WebSiteName;
                        VenueInDB.VenueType = venueBDO.VenueType;
                        VenueInDB.RowGuid = Guid.NewGuid();
                        VenueInDB.Room = venueBDO.Room;
                        VenueInDB.ProvinceID = venueBDO.ProvinceID;
                        VenueInDB.Place = venueBDO.Place;
                        VenueInDB.DateModified = DateTime.Now;
                        VenueInDB.Available = venueBDO.Available;
                        VenueInDB.Capacity = venueBDO.Capacity;
                        VenueInDB.Comments = venueBDO.Description;

                        context.TestVenues.Add(VenueInDB);
                        context.SaveChanges();

                        ret = true;
                        message = "Test Venue added succesfully";
                    }
                }
                catch (Exception ex)
                {
                    ret = false;
                    //message = ex.ToString();
                    message += " :  " + ex.InnerException.ToString();
                    ModernDialog.ShowMessage(message, "Venue not saved", MessageBoxButton.OK); // to use messaging later
                }


            }
            return ret;
        }

        public VenueBDO GetTestVenueByWebSiteName(string websiteName)
        {
            VenueBDO venueBDO = null;
            using (var context = new CETAPEntities())
            {
                TestVenue venue = context.TestVenues.Where(x => x.WebsiteName == websiteName).FirstOrDefault();

                if (venue != null)
                {
                    venueBDO = new VenueBDO();
                    TestVenueToVenueBDO(venueBDO, venue);

                }

            }
            return venueBDO;
        }
        public string GetWebSiteNameByVenueCode(int venueCode)
        {
            string webname = "";

            using (var context = new CETAPEntities())
            {
                TestVenue venue = context.TestVenues.Where(x => x.VenueCode == venueCode).FirstOrDefault();

                if (venue != null)
                {

                    webname = venue.WebsiteName;
                }
            }
            return webname;
        }

        #endregion

        #region Tests
        public void ActualUsedTests()
        {
            var ActualAllocations = new List<ActualAllocationBDO>();
            using (var context = new CETAPEntities())
            {
                var composite = context.Composits
                                       .Select(x => new
                                       {
                                           TestDate = x.DOT,
                                           AQLLang = x.AQLLanguage,
                                           AQLCode = x.AQLCode,
                                           MATLang = x.MatLanguage,
                                           MATCode = x.MatCode,
                                           Batch = x.Batch,
                                           Amount = x.Batch.Substring(19),
                                           client = x.Batch.Substring(18, 1)
                                       }).Distinct();

                int myBatches = composite.Count();
                foreach (var b in composite)
                {

                    if (b.AQLCode.HasValue)
                    {
                        ActualAllocationBDO actAll = new ActualAllocationBDO();
                        actAll.Date = b.TestDate;
                        if (b.AQLLang == "E") actAll.TestType = "AQLE";
                        if (b.AQLLang == "A") actAll.TestType = "AQLA";
                        actAll.Amount = Convert.ToInt32(b.Amount);
                        actAll.Code = b.AQLCode.Value;
                        actAll.Client = b.client;
                        ActualAllocations.Add(actAll);
                    }


                    if (b.MATCode.HasValue)
                    {
                        ActualAllocationBDO actAll = new ActualAllocationBDO();
                        actAll.Date = b.TestDate;
                        if (b.MATLang == "E") actAll.TestType = "MATE";
                        if (b.MATLang == "A") actAll.TestType = "MATA";
                        actAll.Amount = Convert.ToInt32(b.Amount);
                        actAll.Code = b.MATCode.Value;
                        actAll.Client = b.client;
                        ActualAllocations.Add(actAll);
                    }
                }
            }
            var AllocationDays = ActualAllocations.OrderBy(m => m.Date).ThenBy(m => m.TestType).ThenBy(m => m.Code).GroupBy(m => m.Date).ToList();


            foreach (var date in AllocationDays)
            {
                bool newDate = true;
                int Count = 0;
                int TestAmount = 0;
                string tempType = "";
                int tempCode = 0;
                int records = date.Count();
                int myRecs = 0;
                foreach (var rec in date)
                {
                    myRecs++;
                    if (Count == 0 && newDate)
                    {
                        TestAmount = rec.Amount;
                        tempCode = rec.Code;
                        tempType = rec.TestType;

                    }
                    else if (Count == 0 && !newDate)
                    {
                        if (tempType == rec.TestType && tempCode == rec.Code) // first record and second record the same?
                        {
                            TestAmount += rec.Amount;

                        }
                        else
                        {
                            using (var context = new CETAPEntities())
                            {
                                var TestActualAllocation = context.TestAllocations
                                                                  .Where(b => b.TestDate == rec.Date && b.TestName.TestName1.StartsWith(tempType) && b.TestName.TestCode == tempCode)
                                                                  .Select(b => b).FirstOrDefault();
                                if (TestActualAllocation != null)
                                {
                                    TestActualAllocation.ActualUsed = TestAmount;
                                    context.SaveChanges();
                                    Count = -1;
                                }
                            }
                        }


                    }
                    else if (Count > 0)
                    {
                        if (tempType == rec.TestType && tempCode == rec.Code)
                        {
                            TestAmount += rec.Amount;

                        }
                        else
                        {
                            using (var context = new CETAPEntities())
                            {
                                var TestActualAllocation = context.TestAllocations
                                                                  .Where(b => b.TestDate == rec.Date && b.TestName.TestName1.StartsWith(tempType) && b.TestName.TestCode == tempCode)
                                                                  .Select(b => b).FirstOrDefault();
                                if (TestActualAllocation != null)
                                {
                                    TestActualAllocation.ActualUsed = TestAmount;
                                    context.SaveChanges();
                                }
                            }
                            TestAmount = rec.Amount;
                            Count = -1;
                        }

                    }
                    if (myRecs == records)
                    {
                        using (var context = new CETAPEntities())
                        {
                            var TestActualAllocation = context.TestAllocations
                                                              .Where(b => b.TestDate == rec.Date && b.TestName.TestName1.StartsWith(tempType) && b.TestName.TestCode == tempCode)
                                                              .Select(b => b).FirstOrDefault();
                            if (TestActualAllocation != null)
                            {
                                TestActualAllocation.ActualUsed = TestAmount;
                                context.SaveChanges();
                            }
                        }
                    }
                    tempCode = rec.Code;
                    tempType = rec.TestType;
                    newDate = false;
                    Count++;
                }
            }

        }

        public bool AllocationsToExcel(string filename)
        {
            ActualUsedTests();
            bool saved = false;
            using (var context = new CETAPEntities())
            {
                try
                {
                    var AllAlloc = context.TestAllocations
                                      .Select(x => new
                                      {
                                          TestDate = x.TestDate,
                                          Test = x.TestName.TestName1,
                                          Client = x.Client,
                                          ClientType = x.ClientType,
                                          Estimate = x.Estimated,
                                          ActualUsed = x.ActualUsed
                                      }).OrderBy(x => x.TestDate).ToList();

                    // setting up Excel file
                    var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add("Test Allocations").SetTabColor(XLColor.Almond);
                    var AllColumns = ws.Columns("A1:F1");
                    AllColumns.Width = 30;
                    var Row1 = ws.Row(1);
                    Row1.Height = 30;
                    // ws.Columns().Width = 30;
                    Row1.Style.Font.Bold = true;
                    ws.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.TealBlue;
                    ws.Cell(1, 1).Value = "TestDate";
                    ws.Cell(1, 2).Value = "TestName";
                    ws.Cell(1, 3).Value = "Test Client";
                    ws.Cell(1, 4).Value = "Client Type";
                    ws.Cell(1, 5).Value = "Estimated Books";
                    ws.Cell(1, 6).Value = "Actual Books";


                    // Write records
                    ws.Cell(2, 1).Value = AllAlloc.AsEnumerable();
                    // save the records
                    ws.Columns().AdjustToContents();
                    workbook.SaveAs(filename);
                    saved = true;
                }
                catch (Exception ex)
                {
                    log.Error("Cannot save file", ex);
                    throw ex;
                }

            }
            return saved;
        }

        public void WriteAllAllocations(IntakeYearsBDO year)
        {
            using (var context = new CETAPEntities())
            {

            }
        }
        public List<TestBDO> GetAllTests()
        {
            using (var context = new CETAPEntities())
            {
                TestBDO testBDO = null;
                List<TestBDO> alltests = new List<TestBDO>();
                try
                {


                    // get all database	tests

                    List<TestName> tests = (from p in context.TestNames
                                            select p).ToList();

                    //Convert each test to TestBDO
                    foreach (TestName test in tests)
                    {
                        testBDO = new TestBDO();
                        TranslateTestDALToTestBDO(test, testBDO);
                        //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                        alltests.Add(testBDO);
                    }
                }
                catch (Exception ex)
                {

                    string message = ex.InnerException.ToString();
                    ModernDialog.ShowMessage(message, "Test", MessageBoxButton.OK);
                }



                return alltests;
            }
        }

        public string getTestName(int TestID)
        {
            using (var context = new CETAPEntities())
            {
                var name = context.TestNames.Where(x => x.TestID == TestID).Select(x => x.TestName1).FirstOrDefault();
                return name;
            }
        }
        public TestBDO getTestByName(string name)
        {
            TestBDO testBDO = null;
            using (var context = new CETAPEntities())
            {
                TestName test = context.TestNames.Where(x => x.TestName1 == name).FirstOrDefault();

                if (test != null)
                {
                    testBDO = new TestBDO();
                    TestDALToTestBDO(testBDO, test);

                }

            }
            return testBDO;
        }

        public TestBDO getTestByID(int testID)
        {
            TestBDO testBDO = null;
            using (var context = new CETAPEntities())
            {
                TestName test = context.TestNames.Where(x => x.TestID == testID).FirstOrDefault();

                if (test != null)
                {
                    TestDALToTestBDO(testBDO, test);

                }

            }
            return testBDO;
        }

        public bool updateTest(TestBDO testBDO, ref string message)
        {
            bool ret = false;
            message = "Test successfully updated";

            using (var context = new CETAPEntities())
            {
                try
                {
                    var ID = testBDO.TestID;
                    TestName testInDB = context.TestNames.Where(x => x.TestID == ID).FirstOrDefault();

                    if (testInDB != null)
                    {
                        ret = true;
                        context.TestNames.Remove(testInDB);

                        //update testVenue
                        // TestName mytest = new TestName();
                        //testInDB = Maps.TestBDOToTestDAL(testBDO);
                        TestBDOtoTestDAL(testBDO, testInDB);
                        testInDB.DateModified = DateTime.Now;

                        //context.TestNames.Attach(testInDB);
                        context.Entry(testInDB).State = System.Data.Entity.EntityState.Modified;
                        int num = context.SaveChanges();

                        testBDO.RowVersion = testInDB.RowVersion;

                    }
                    else
                    {
                        message = "No Test with ID " + testBDO.TestID;
                    }
                }
                catch (Exception ex)
                {

                    message += ex.InnerException.ToString();
                    ModernDialog.ShowMessage(message, "Test not saved", MessageBoxButton.OK);
                }


            }
            return ret;
        }

        public bool deleteTest(TestBDO test, ref string message)
        {
            message = "Test successfully deleted";
            bool ret = false;
            int TID = test.TestID;
            using (var context = new CETAPEntities())
            {
                //look for venue
                TestName testInDB = (from p in context.TestNames
                                     where (p.TestID == TID)
                                     select p).FirstOrDefault();
                // if venue in database
                if (testInDB != null)
                {
                    context.TestNames.Remove(testInDB);
                    int num = context.SaveChanges();
                    ret = true;
                }
                else
                {
                    message = "No such Test in database";
                    ret = false;
                }

            }
            return ret;
        }

        public bool addTest(TestBDO testBDO, ref string message)
        {
            bool ret = false;
            string tname = testBDO.TestName.ToUpper();
            using (var context = new CETAPEntities())
            {
                try
                {
                    //look for venue
                    TestName testInDB = context.TestNames.Where(x => x.TestName1 == tname).FirstOrDefault();

                    //Add TestVenue
                    if (testInDB != null)
                    {
                        ret = false;
                        message = "Test with name " + tname + " is already available";
                    }
                    else
                    {
                        TestName testInDBa = new TestName();
                        TestBDOtoTestDAL(testBDO, testInDBa);

                        testInDBa.rowguid = Guid.NewGuid();

                        testInDBa.DateModified = DateTime.Now;

                        context.TestNames.Add(testInDBa);
                        context.SaveChanges();

                        ret = true;
                        message = "Test  added succesfully";
                    }
                }
                catch (Exception ex)
                {
                    ret = false;
                    message += ex.InnerException.ToString();
                    ModernDialog.ShowMessage(message, "Test not saved", MessageBoxButton.OK);

                }


            }
            return ret;
        }
        #endregion

        #region TestAllocation
        public List<TestAllocationBDO> GetAllTestAllocations()
        {
            List<TestAllocationBDO> allAllocatedTests = new List<TestAllocationBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    TestAllocationBDO testAllocationBDO = null;

                    try
                    {
                        // get all database	tests

                        List<TestAllocation> allocations = context.TestAllocations
                                                            .OrderBy(x => x.TestDate)
                                                            .ThenBy(x => x.TestName).ToList();


                        //Convert each test to TestBDO
                        //Parallel.ForEach(allocations, allocation =>
                        //{
                        //    testAllocationBDO = new TestAllocationBDO();
                        //    TranslateTestAllocationDALToTestAllocationBDO(allocation, testAllocationBDO);
                        //    //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                        //    allAllocatedTests.Add(testAllocationBDO);
                        //}
                        // );

                        foreach (TestAllocation allocation in allocations)
                        {
                            testAllocationBDO = new TestAllocationBDO();
                            TranslateTestAllocationDALToTestAllocationBDO(allocation, testAllocationBDO);
                            //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                            allAllocatedTests.Add(testAllocationBDO);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }




                }
            }

            return allAllocatedTests;
        }

        public List<TestAllocationBDO> getTestAllocationByClient(string name)
        {
            throw new NotImplementedException();
        }

        public List<TestAllocationBDO> getTestAllocationByTestID(int testID)
        {
            List<TestAllocationBDO> testAllocationBDOs = new List<TestAllocationBDO>();
            using (var context = new CETAPEntities())
            {
                List<TestAllocation> testAllocation = context.TestAllocations.Where(x => x.TestID == testID).ToList();

                if (testAllocation != null)
                {
                    foreach (TestAllocation testa in testAllocation)
                    {
                        TestAllocationBDO testAllocationBDO = new TestAllocationBDO();
                        TranslateTestAllocationDALToTestAllocationBDO(testa, testAllocationBDO);
                        testAllocationBDOs.Add(testAllocationBDO);
                    }

                }

            }
            return testAllocationBDOs;
        }

        public TestAllocationBDO getTestAllocationByID(int ID)
        {
            throw new NotImplementedException();
        }

        public bool updateTestAllocation(TestAllocationBDO testalloc, ref string message)
        {
            bool ret = false;
            message = "Test Allocation successfully updated";

            using (var context = new CETAPEntities())
            {
                var ID = testalloc.ID;
                TestAllocation allocInDB = context.TestAllocations.Where(x => x.ID == ID).FirstOrDefault();

                if (allocInDB != null)
                {
                    ret = true;
                    context.TestAllocations.Remove(allocInDB);

                    //update testVenue
                    // TestName mytest = new TestName();
                    TranslateTestAllocationBDOToTestAllocationDAL(testalloc, allocInDB);
                    allocInDB.DateModified = DateTime.Now;

                    context.TestAllocations.Attach(allocInDB);
                    context.Entry(allocInDB).State = System.Data.Entity.EntityState.Modified;
                    int num = context.SaveChanges();

                    testalloc.RowVersion = allocInDB.RowVersion;

                }
                else
                {
                    message = "No Test allocation with ID " + testalloc.ID;
                }

            }
            return ret;
        }

        public bool deleteTestAllocation(TestAllocationBDO testAlloc, ref string message)
        {
            message = "Test Allocation successfully deleted";
            bool ret = false;
            int TAID = testAlloc.ID;
            using (var context = new CETAPEntities())
            {
                //look for venue
                TestAllocation allocationInDB = (from p in context.TestAllocations
                                                 where (p.ID == TAID)
                                                 select p).FirstOrDefault();
                // if venue in database
                if (allocationInDB != null)
                {
                    context.TestAllocations.Remove(allocationInDB);
                    int num = context.SaveChanges();
                    ret = true;
                }
                else
                {
                    message = "No such Test Allocation in database";
                    ret = false;
                }

            }
            return ret;
        }

        public bool addTestAllocation(TestAllocationBDO testAllocationBDO, ref string message)
        {
            bool ret = false;
            int code = testAllocationBDO.TestID;
            DateTime testDate = testAllocationBDO.TestDate;
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    try
                    {
                        //look for venue
                        TestAllocation testAllocInDB = context.TestAllocations.Where(x => x.TestID == code && x.TestDate == testDate && x.Client == testAllocationBDO.Client).FirstOrDefault();

                        //Add TestVenue
                        if (testAllocInDB != null)
                        {
                            ret = false;
                            message = "Test Allocation with Test code " + code + " is already available";
                        }
                        else
                        {
                            var testAllocationInDB = new TestAllocation();
                            TranslateTestAllocationBDOToTestAllocationDAL(testAllocationBDO, testAllocationInDB);
                            testAllocationInDB.RowGuid = Guid.NewGuid();

                            testAllocationInDB.DateModified = DateTime.Now;

                            context.TestAllocations.Add(testAllocationInDB);
                            context.SaveChanges();

                            ret = true;
                            message = "Test Allocation added succesfully";
                        }
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        message = ex.ToString();

                    }


                }
            }

            return ret;
        }
        #endregion

        #region Test Profile
        public List<ProfileAllocationBDO> GetProfileAllocationsByDate(DateTime date)
        {
            List<ProfileAllocationBDO> PABDO = new List<ProfileAllocationBDO>();
            //var m = GetAllTests();
            var Allocs = GetAllTestAllocations();
            var profiles = GetAllTestProfiles();
            var ProfilesWithTestNames = (from a in profiles
                                         join c in Allocs on a.AllocationID equals c.ID
                                         where (c.TestDate == date)
                                         orderby a.Profile ascending
                                         select new
                                         {
                                             a.ProfileID,
                                             a.Profile,
                                             c.TestID,
                                             c.TestName,
                                             c.Client,
                                             c.TestDate
                                         }).ToList();

            var myProfAllocs = ProfilesWithTestNames.GroupBy(x => x.Profile)
                                                   .Select(g => new
                                                   {
                                                       Profile = g.Key,
                                                       Data = g
                                                   });


            foreach (var a in myProfAllocs)
            {
                ProfileAllocationBDO xx = new ProfileAllocationBDO();
                xx.Profile = a.Profile;

                foreach (var mm in a.Data)
                {
                    xx.TestDate = mm.TestDate;
                    xx.Client = mm.Client;
                    if (mm.TestName.StartsWith("AQLE")) xx.AQLE = mm.TestName;
                    if (mm.TestName.StartsWith("MATE")) xx.MATE = mm.TestName;
                    if (mm.TestName.StartsWith("AQLA")) xx.AQLA = mm.TestName;
                    if (mm.TestName.StartsWith("MATA")) xx.MATA = mm.TestName;

                }

                PABDO.Add(xx);
            }
            return PABDO;
        }

        public List<ProfileAllocationBDO> GetProfileAllocationByProfile(int Profile)
        {
            List<ProfileAllocationBDO> PABDO = new List<ProfileAllocationBDO>();
            var Allocs = GetAllTestAllocations();
            var profiles = GetAllTestProfiles();
            var ProfilesWithTestNames = (from a in profiles
                                         join c in Allocs on a.AllocationID equals c.ID
                                         where (a.Profile == Profile)
                                         orderby a.Profile ascending
                                         select new
                                         {
                                             a.ProfileID,
                                             a.Profile,
                                             c.TestID,
                                             c.TestName,
                                             c.Client,
                                             c.TestDate
                                         }).ToList();

            var myProfAllocs = ProfilesWithTestNames.GroupBy(x => x.Profile)
                                                   .Select(g => new
                                                   {
                                                       Profile = g.Key,
                                                       Data = g
                                                   });


            foreach (var a in myProfAllocs)
            {
                ProfileAllocationBDO xx = new ProfileAllocationBDO();
                xx.Profile = a.Profile;

                foreach (var mm in a.Data)
                {
                    xx.TestDate = mm.TestDate;
                    xx.Client = mm.Client;
                    if (mm.TestName.StartsWith("AQLE")) xx.AQLE = mm.TestName;
                    if (mm.TestName.StartsWith("MATE")) xx.MATE = mm.TestName;
                    if (mm.TestName.StartsWith("AQLA")) xx.AQLA = mm.TestName;
                    if (mm.TestName.StartsWith("MATA")) xx.MATA = mm.TestName;

                }

                PABDO.Add(xx);
            }
            return PABDO;
        }

        public List<ProfileAllocationBDO> GetAllProfileAllocations()
        {
            List<ProfileAllocationBDO> PABDO = new List<ProfileAllocationBDO>();
            var m = GetAllTests();
            var Allocs = GetAllTestAllocations();
            var profiles = GetAllTestProfiles();
            var ProfilesWithTestNames = (from a in profiles
                                         join c in Allocs on a.AllocationID equals c.ID
                                         orderby a.Profile ascending
                                         select new
                                         {
                                             a.ProfileID,
                                             a.Profile,
                                             c.TestID,
                                             c.TestName,
                                             c.Client,
                                             c.TestDate
                                         }).ToList();

            var myProfAllocs = ProfilesWithTestNames.GroupBy(x => new { x.Profile, x.Client, x.TestDate })
                                                   .Select(g => new
                                                   {
                                                       Profiler = g.Key,
                                                       Data = g
                                                   });


            foreach (var a in myProfAllocs)
            {
                ProfileAllocationBDO xx = new ProfileAllocationBDO();
                xx.Profile = a.Profiler.Profile;

                foreach (var mm in a.Data)
                {
                    xx.TestDate = mm.TestDate;
                    xx.Client = mm.Client;
                    if (mm.TestName.StartsWith("AQLE")) xx.AQLE = mm.TestName;
                    if (mm.TestName.StartsWith("MATE")) xx.MATE = mm.TestName;
                    if (mm.TestName.StartsWith("AQLA")) xx.AQLA = mm.TestName;
                    if (mm.TestName.StartsWith("MATA")) xx.MATA = mm.TestName;

                    //xx.AQLE = mm.TestName.StartsWith("AQLE").ToString();
                    //xx.MATE = mm.TestName.StartsWith("MATE").ToString();
                    //xx.AQLA = mm.TestName.StartsWith("AQLA").ToString();
                    //xx.MATA = mm.TestName.StartsWith("MATA").ToString();
                }

                PABDO.Add(xx);
            }
            return PABDO;
        }
        public void SaveProfileAllocationsToExcel(string filename)
        {
            List<ProfileAllocationBDO> myAllocations = new List<ProfileAllocationBDO>();
            myAllocations = GetAllProfileAllocations();

            // setting up Excel file
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Test Allocation").SetTabColor(XLColor.Almond);
            var AllColumns = ws.Columns("A1:G1");
            AllColumns.Width = 30;
            var Row1 = ws.Row(1);
            Row1.Height = 30;
            // ws.Columns().Width = 30;
            Row1.Style.Font.Bold = true;
            ws.Range("A1:G1").Style.Fill.BackgroundColor = XLColor.TealBlue;
            ws.Cell(1, 1).Value = "Date";
            ws.Cell(1, 2).Value = "Profile";
            ws.Cell(1, 3).Value = "Test Type";
            ws.Cell(1, 4).Value = "AQLE";
            ws.Cell(1, 5).Value = "MATE";
            ws.Cell(1, 6).Value = "AQLA";
            ws.Cell(1, 7).Value = "MATA";

            // Write records
            ws.Cell(2, 1).Value = myAllocations.AsEnumerable();
            // save the records
            ws.Columns().AdjustToContents();
            workbook.SaveAs(filename);

        }
        public List<TestProfileBDO> GetAllTestProfiles()
        {
            List<TestProfileBDO> allProfilesBDO = new List<TestProfileBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    TestProfileBDO testProfileBDO = null;

                    try
                    {


                        // get all database	tests

                        List<TestProfile> profilesInDB = (from p in context.TestProfiles
                                                          select p).ToList();

                        //Convert each test to TestBDO
                        foreach (TestProfile profile in profilesInDB)
                        {
                            testProfileBDO = new TestProfileBDO();
                            ProfileDALToProfileBDO(testProfileBDO, profile);
                            //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                            allProfilesBDO.Add(testProfileBDO);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }




                }
            }
            return allProfilesBDO;

        }
        public List<TestBDO> GetTestFromDatFile(datFileAttributes datafile, IntakeYearsBDO intake)
        {
            List<TestBDO> myTest = new List<TestBDO>();
            string client = "Special";
            using (var context = new CETAPEntities())
            {
                if (datafile.Client != "Special")
                {
                    var di = context.TestProfiles
                             .Where(a => a.Profile == datafile.Profile && a.TestAllocation.ClientType == "National" && a.Intake == intake.Year)
                             .Select(v => v.TestAllocation.TestName).ToList();
                    if (di != null)
                    {
                        foreach (TestName test in di)
                        {

                            var myT = Maps.TestDALToTestBDO(test);
                            myTest.Add(myT);
                        }

                    }
                }

                else
                {
                    var di = context.TestProfiles
                             .Where(a => a.Profile == datafile.Profile && a.TestAllocation.ClientType == client && a.Intake == intake.Year)
                             .Select(v => v.TestAllocation.TestName).ToList();
                    if (di != null)
                    {
                        foreach (TestName test in di)
                        {

                            var myT = Maps.TestDALToTestBDO(test);
                            myTest.Add(myT);
                        }

                    }
                }

            }
            return myTest;
        }

        public bool updateTestProfile(TestProfileBDO profileBDO, ref string message)
        {
            bool ret = false;
            message = "Profile successfully updated";

            using (var context = new CETAPEntities())
            {
                var ID = profileBDO.ProfileID;
                TestProfile profileInDB = context.TestProfiles.Where(x => x.ProfileID == ID).FirstOrDefault();

                if (profileInDB != null)
                {
                    ret = true;
                    context.TestProfiles.Remove(profileInDB);

                    //update testVenue
                    // TestName mytest = new TestName();
                    ProfileBDOToProfileDAL(profileBDO, profileInDB);
                    profileInDB.ModifiedDate = DateTime.Now;

                    context.TestProfiles.Attach(profileInDB);
                    context.Entry(profileInDB).State = System.Data.Entity.EntityState.Modified;
                    int num = context.SaveChanges();

                    profileBDO.RowVersion = profileInDB.RowVersion;

                }
                else
                {
                    message = "No Profile with ID " + profileBDO.ProfileID;
                }

            }
            return ret;
        }

        public bool deleteTestProfile(TestProfileBDO profileBDO, ref string message)
        {
            message = "Profile successfully deleted";
            bool ret = false;
            int TID = profileBDO.ProfileID;
            using (var context = new CETAPEntities())
            {
                //look for venue
                TestProfile profileInDB = (from p in context.TestProfiles
                                           where (p.ProfileID == TID)
                                           select p).FirstOrDefault();
                // if venue in database
                if (profileInDB != null)
                {
                    context.TestProfiles.Remove(profileInDB);
                    int num = context.SaveChanges();
                    ret = true;
                }
                else
                {
                    message = "No such Profile in database";
                    ret = false;
                }

            }
            return ret;
        }

        public bool addTestProfile(TestProfileBDO profileBDO, ref string message)
        {
            bool ret = false;
            int TAID = profileBDO.AllocationID;
            int prof = profileBDO.Profile;
            using (var context = new CETAPEntities())
            {
                try
                {
                    //look for profile
                    TestProfile profileInDB = context.TestProfiles.Where(x => x.AllocationID == TAID && x.Profile == prof).FirstOrDefault();

                    //Add TestVenue
                    if (profileInDB != null)
                    {
                        ret = false;
                        message = "Test Profile with profile " + prof + " is already available";
                    }
                    else
                    {
                        profileInDB = new TestProfile();
                        profileInDB = Maps.TestProfileBDOToTestProfile(profileBDO);
                        //ProfileBDOToProfileDAL(profileBDO, profileInDB);

                        profileInDB.RowGuid = Guid.NewGuid();

                        profileInDB.ModifiedDate = DateTime.Now;

                        context.TestProfiles.Add(profileInDB);
                        context.SaveChanges();

                        ret = true;
                        message = "Test Profile added succesfully";
                    }
                }
                catch (Exception ex)
                {
                    ret = false;
                    message = ex.ToString();

                }


            }
            return ret;
        }

        public List<TestProfileBDO> getTestprofileByAllocationID(int allocationID)
        {
            List<TestProfileBDO> testProfileBDOs = new List<TestProfileBDO>();
            using (var context = new CETAPEntities())
            {
                List<TestProfile> testProfiles = context.TestProfiles.Where(x => x.AllocationID == allocationID).ToList();

                if (testProfiles != null)
                {
                    foreach (TestProfile profile in testProfiles)
                    {
                        TestProfileBDO testProfileBDO = new TestProfileBDO();
                        ProfileDALToProfileBDO(testProfileBDO, profile);
                        testProfileBDOs.Add(testProfileBDO);
                    }

                }

            }
            return testProfileBDOs;
        }

        public List<TestBDO> GetTestfromTestProfile(int profile)
        {
            List<TestBDO> myTest = new List<TestBDO>();
            using (var context = new CETAPEntities())
            {
                var di = context.TestProfiles.Where(a => a.Profile == profile).Select(v => v.TestAllocation.TestName).ToList();
                if (di != null)
                {
                    foreach (TestName test in di)
                    {

                        var myT = Maps.TestDALToTestBDO(test);
                        myTest.Add(myT);
                    }

                }
            }
            return myTest;
        }
        public TestProfileBDO getTestProfileByProfileID(int profileID)
        {
            TestProfileBDO profileBDO = null;
            using (var context = new CETAPEntities())
            {
                TestProfile profile = context.TestProfiles.Where(x => x.ProfileID == profileID).FirstOrDefault();

                if (profile != null)
                {
                    ProfileDALToProfileBDO(profileBDO, profile);

                }

            }
            return profileBDO;
        }
        public List<TestProfileBDO> getTestprofileByProfile(int prof)
        {
            List<TestProfileBDO> testProfileBDOs = new List<TestProfileBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    List<TestProfile> testProfiles = context.TestProfiles.Where(x => x.Profile == prof).ToList();

                    if (testProfiles != null)
                    {
                        foreach (TestProfile profile in testProfiles)
                        {

                            var testPBDO = Maps.TestProfileToTestProfileBDO(profile);

                            //TestProfileBDO testProfileBDO = new TestProfileBDO();
                            //ProfileDALToProfileBDO(testProfileBDO, profile);
                            testProfileBDOs.Add(testPBDO);
                        }

                    }

                }
            }
            return testProfileBDOs;
        }
        #endregion

        #region Tracker
        public List<ScanTrackerBDO> GetAllTracks()
        {
            List<ScanTrackerBDO> tracksBDO = new List<ScanTrackerBDO>();
            //IsDB = ApplicationSettings.Default.DBAvailable;
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    List<ScanTracker> tracks = (from a in context.ScanTrackers
                                                select a).ToList();
                    foreach (var track in tracks)
                    {
                        ScanTrackerBDO trackbdo = new ScanTrackerBDO();
                        trackbdo = Maps.ScantrackerDALToScantrackerBDO(track);
                        // BatchDALToBatchBDO(batchbdo, batch);
                        // vbdo.IsDirty = false;
                        //vbdo.Province = getProvinceByID(vbdo.ProvinceID);
                        tracksBDO.Add(trackbdo);
                    }

                }
            }
            return tracksBDO;
        }


        #endregion

        #region Batches
        public string getBatchRandomNumber()
        {
            string number = HelperUtils.GetRandomNumber();
            return number;
        }
        public List<BatchBDO> GetAllbatches()
        {
            List<BatchBDO> batchesBDO = new List<BatchBDO>();
            //IsDB = ApplicationSettings.Default.DBAvailable;
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (var context = new CETAPEntities())
                {
                    List<Batch> batches = (from a in context.Batches
                                           select a
                                           ).ToList();
                    foreach (var batch in batches)
                    {
                        BatchBDO batchbdo = new BatchBDO();
                        batchbdo = CETAP_LOB.Mapping.Maps.BatchDALToBatchBDO(batch);
                        // BatchDALToBatchBDO(batchbdo, batch);
                        // vbdo.IsDirty = false;
                        //vbdo.Province = getProvinceByID(vbdo.ProvinceID);
                        batchesBDO.Add(batchbdo);
                    }

                }
            }
            return batchesBDO;
        }

        public BatchBDO GetBatchByName(string BName)
        {
            BatchBDO myBatch = new BatchBDO();
            using (var context = new CETAPEntities())
            {
                Batch dbBatch = context.Batches.Where(a => a.BatchName == BName).Select(b => b).FirstOrDefault();
                if (dbBatch != null)
                {
                    myBatch = Maps.BatchDALToBatchBDO(dbBatch);
                    //  BatchDALToBatchBDO(myBatch, dbBatch);
                }
            }
            return myBatch;
        }
        public List<BatchBDO> getbatchesByVenue(int venueID)
        {
            List<BatchBDO> batchesBDO = new List<BatchBDO>();
            using (var context = new CETAPEntities())
            {
                List<Batch> batches = context.Batches.Where(x => x.TestVenueID == venueID).ToList();

                if (batches != null)
                {
                    foreach (var batch in batches)
                    {
                        BatchBDO batchbdo = new BatchBDO();
                        batchbdo = Maps.BatchDALToBatchBDO(batch);
                        //BatchDALToBatchBDO(batchbdo, batch);
                        batchesBDO.Add(batchbdo);
                    }

                }

            }
            return batchesBDO;
        }

        public List<BatchBDO> getbatchesByDate(DateTime date)
        {
            List<BatchBDO> batchesBDO = new List<BatchBDO>();
            using (var context = new CETAPEntities())
            {
                List<Batch> batches = context.Batches.Where(x => x.BatchDate == date).ToList();

                if (batches != null)
                {
                    foreach (var batch in batches)
                    {
                        BatchBDO batchbdo = new BatchBDO();
                        batchbdo = Maps.BatchDALToBatchBDO(batch);
                        //BatchDALToBatchBDO(batchbdo, batch);
                        batchesBDO.Add(batchbdo);
                    }

                }

            }
            return batchesBDO;
        }

        public bool addBatch(BatchBDO batchBDO, ref string message)
        {
            bool ret = false;
            string codeName = batchBDO.BatchName;
            using (var context = new CETAPEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //look for venue
                        Batch batchInDB = context.Batches.Where(x => x.BatchName == codeName).FirstOrDefault();
                        ScanTracker tracker = context.ScanTrackers.Where(s => s.FileName == batchBDO.BatchName).FirstOrDefault();
                        //Add TestVenue
                        if (batchInDB != null)
                        {
                            ret = false;
                            message = "Batch Named " + codeName + " is already available";
                        }
                        else
                        {
                            batchInDB = new Batch();
                            batchInDB = Maps.BatchBDOToBatchDAL(batchBDO);
                            // BatchBDOtoBatchDAL(batchBDO, batchInDB);
                            //batchInDB. = Guid.NewGuid();
                            batchInDB.DateModified = DateTime.Now;
                            batchInDB.BatchDate = DateTime.Now;
                            context.Batches.Add(batchInDB);
                            //context.SaveChanges();

                            ret = true;
                            message = "Batch added succesfully";
                            if (tracker != null)
                            {
                                // batchBDO already in tracker, probably missed in the entry of batches
                                context.ScanTrackers.Remove(tracker);
                                tracker.DateBatched = batchInDB.BatchDate;
                                tracker.DateModified = DateTime.Now;
                                tracker.Records = batchInDB.Count;
                                tracker.BatchedBy = batchInDB.BatchedBy;
                                tracker.TestDate = batchInDB.TestDate;
                                context.ScanTrackers.Attach(tracker);
                                context.Entry(tracker).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                ScanTracker scant = new ScanTracker();
                                scant.BatchedBy = batchInDB.BatchedBy;
                                scant.DateBatched = batchInDB.BatchDate;
                                scant.Records = batchInDB.Count;
                                scant.DateModified = DateTime.Now;
                                scant.FileName = batchInDB.BatchName;
                                context.ScanTrackers.Add(scant);
                            }
                            context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        message = ex.ToString();
                        dbContextTransaction.Rollback();
                    }
                }


            }
            return ret;
        }

        public bool deleteBatch(BatchBDO batch, ref string message)
        {
            message = "Test Venue successfully deleted";
            bool ret = false;
            int BID = batch.BatchID;
            using (var context = new CETAPEntities())
            {
                //look for venue
                Batch batchInDB = (from p in context.Batches
                                   where (p.BatchID == BID)
                                   select p).FirstOrDefault();
                // if venue in database
                if (batchInDB != null)
                {
                    ScanTracker scanT = context.ScanTrackers.Where(s => s.FileName == batch.BatchName).FirstOrDefault();
                    ScannedFile scannedFile = context.ScannedFiles.Where(m => m.Filename == batch.BatchName).FirstOrDefault();
                    context.Batches.Remove(batchInDB);
                    if (scanT != null)
                    {
                        context.ScanTrackers.Remove(scanT);
                    }
                    if (scannedFile != null)
                    {
                        context.ScannedFiles.Remove(scannedFile);
                    }
                    int num = context.SaveChanges();
                    ret = true;
                }
                else
                {
                    message = "No such Batch in database";
                    ret = false;
                }

            }
            return ret;
        }
        public bool updatebatch(BatchBDO batchBDO, ref string message)
        {
            bool ret = false;
            message = "Batch successfully updated";

            using (var context = new CETAPEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = batchBDO.BatchID;
                        Batch batch = context.Batches.Where(x => x.BatchID == ID).FirstOrDefault();

                        if (batch != null)
                        {

                            ret = true;
                            // do we need to remove when we are just changing the file name?
                            //context.Batches.Remove(batch);
                            batch.BatchName = batchBDO.BatchName;
                            batch.TestDate = batchBDO.TestDate;
                            batch.TestCombination = batchBDO.TestCombination;
                            batch.TestVenueID = batchBDO.TestVenueID;
                            batch.TestProfileID = batchBDO.TestProfileID;
                            batch.BatchedBy = batchBDO.BatchedBy;
                            batch.Count = batchBDO.Count;
                            batch.RandTestNumber = batchBDO.RandomTestNumber;
                            batch.Description = batchBDO.Description;
                            batch.DateModified = DateTime.Now;

                            //context.Batches.Attach(batch);
                            //context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                            ScanTracker scanT = context.ScanTrackers.Where(s => s.FileName == batch.BatchName).FirstOrDefault();
                            if (scanT != null)
                            {
                                //context.ScanTrackers.Remove(scanT);
                                //scanT.DateBatched = batch.BatchDate;
                                scanT.DateModified = DateTime.Now;
                                //scanT.Records = batch.Count;
                                scanT.BatchedBy = batch.BatchedBy;
                                scanT.FileName = batch.BatchName;
                                scanT.Records = batch.Count;
                                scanT.TestDate = batch.TestDate;
                                //context.ScanTrackers.Attach(scanT);
                                //context.Entry(scanT).State = System.Data.Entity.EntityState.Modified;
                            }
                            context.SaveChanges();
                            dbContextTransaction.Commit();

                            batchBDO.RowVersion = batch.RowVersion;

                        }
                        else
                        {
                            message = "No Batch with ID " + batchBDO.BatchID;
                        }
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        dbContextTransaction.Rollback();
                        message = batchBDO.BatchName + " :  " + ex.InnerException.ToString();

                        // ModernDialog.ShowMessage(message, "Batch not saved", MessageBoxButton.OK);
                    }
                }


            }
            return ret;
        }

        #endregion

        #region Scanned files

        public bool SaveFileToDB(ScannedFileBDO scannedFileBDO, ref string message)
        {

            bool ret = false;
            string fname = scannedFileBDO.Filename;

            using (CETAPEntities context = new CETAPEntities())
            {
                context.Database.CommandTimeout = 180;
                try
                {
                    //look for venue
                    ScannedFile fileInDB = context.ScannedFiles.Where(x => x.Filename == fname).FirstOrDefault();
                    if (fileInDB != null)
                    {
                        ret = false;
                        message = "File " + fname + " is already available in Database!!";
                    }
                    else
                    {
                        ScannedFile scannedfile = new ScannedFile();
                        ScannedFileBDOToScannedFileDAL(scannedFileBDO, scannedfile);

                        scannedfile.DateModified = DateTime.Now;

                        context.ScannedFiles.Add(scannedfile);
                        context.SaveChanges();

                        ret = true;
                        message = "Scanned file  added succesfully";

                    }
                }

                catch (DbEntityValidationException dbEx)
                {
                    ret = false;
                    message = dbEx.ToString();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                }
            }
            return ret;
        }

        public void ReadScanfromDB(string Filename, string fileFolder)
        {
            throw new NotImplementedException();
        }
        public ScannedFileBDO GetScannedFile(string Filename)
        {
            ScannedFileBDO myScannedfile = new ScannedFileBDO();
            string filename = Filename;
            using (var context = new CETAPEntities())
            {
                var ScannedFile = context.ScannedFiles.Where(x => x.Filename == filename).Select(x => x).FirstOrDefault();
                if (ScannedFile != null)
                {
                    ScannedFileDALToScannedFileBDO(myScannedfile, ScannedFile);
                }
            }
            return myScannedfile;
        }

        #endregion

        #region Provinces
        public List<ProvinceBDO> getAllProvinces()
        {
            List<ProvinceBDO> provincesBDO = new List<ProvinceBDO>();

            if (ApplicationSettings.Default.DBAvailable) // check if connected to database
            {
                using (var context = new CETAPEntities())
                {
                    List<Province> provinces = (from a in context.Provinces
                                                select a).ToList();
                    foreach (var prov in provinces)
                    {
                        ProvinceBDO provinceBDO = new ProvinceBDO();
                        ProvinceDALToProvinceBDO(provinceBDO, prov);

                        provincesBDO.Add(provinceBDO);
                    }

                }
            }
            return provincesBDO;
        }

        public ProvinceBDO getProvinceByID(int code)
        {
            ProvinceBDO provinceBDO = null;
            int ProvID = code;
            using (var context = new CETAPEntities())
            {
                Province prov = context.Provinces.Where(x => x.Code == ProvID).FirstOrDefault();

                if (prov != null)
                {
                    provinceBDO = new ProvinceBDO();
                    ProvinceToProvinceBDO(provinceBDO, prov);

                }

            }
            return provinceBDO;
        }

        public ProvinceBDO getProvinceByName(string name)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Translate
        private static void InqueueToForDuplicateBarcodesBDO(RecordsInQueue inqueue, ForDuplicatesBarcodesBDO duplicates)
        {
            duplicates.Barcode = inqueue.Barcode;
            duplicates.RefNo = inqueue.NBT;
            duplicates.SAID = inqueue.SAID;
            duplicates.FID = inqueue.FID;
            duplicates.DateModified = inqueue.DateModified;
            duplicates.Reason = inqueue.Reason;
            duplicates.RecID = new long?(inqueue.RecID);
        }

        private static void ForDuplicateBarcodesBDOToInqueue(RecordsInQueue inqueue, ForDuplicatesBarcodesBDO duplicates)
        {
            inqueue.Barcode = duplicates.Barcode;
            inqueue.NBT = duplicates.RefNo;
            inqueue.SAID = duplicates.SAID;
            inqueue.FID = duplicates.FID;
            inqueue.DateModified = duplicates.DateModified;
            inqueue.Reason = duplicates.Reason;
            inqueue.RecID = duplicates.RecID ?? -1L;
        }

        static void SurnameToSurnameBDO(Surname lastname, SurnameBDO surname)
        {
            surname.SurnameID = lastname.SurnameID;
            surname.LastName = lastname.Surname1;
            surname.DateModified = lastname.DateModified;

        }
        static void SurnameBDOToSurname(SurnameBDO lastname, Surname surname)
        {
            surname.SurnameID = lastname.SurnameID;
            surname.Surname1 = lastname.LastName;
            surname.DateModified = lastname.DateModified;

        }
        static void IntakeYearDalToIntakeYearBDO(IntakeYear intakeDAL, IntakeYearsBDO intakeBDO)
        {
            intakeBDO.Year = intakeDAL.Year;
            intakeBDO.yearID = intakeDAL.yearID;
            intakeBDO.yearStart = intakeDAL.yearStart;
            intakeBDO.yearEnd = intakeDAL.yearEnd;
            intakeBDO.DateModified = intakeDAL.DateModified;
        }

        static void BatchBDOToBatchDAL(BatchBDO batchBDO, Batch batch)
        {
            batch.BatchDate = batchBDO.BatchDate;
            batch.BatchedBy = batchBDO.BatchedBy;
            batch.BatchID = batchBDO.BatchID;
            batch.BatchName = batchBDO.BatchName;
            batch.Count = batchBDO.Count;
            batch.DateModified = batchBDO.DateModified;
            batch.Description = batchBDO.Description;
            batch.RandTestNumber = batchBDO.RandomTestNumber;
            batch.RowVersion = batchBDO.RowVersion;
            batch.TestCombination = batchBDO.TestCombination;
            batch.TestDate = batchBDO.TestDate;
            batch.TestProfileID = batchBDO.TestProfileID;
            batch.TestVenueID = batchBDO.TestVenueID;

        }

        static void BatchDALToBatchBDO(BatchBDO batchBDO, Batch batch)
        {
            batchBDO.BatchID = batch.BatchID;
            batchBDO.BatchedBy = batch.BatchedBy;
            batchBDO.BatchDate = batch.BatchDate;
            batchBDO.BatchName = batch.BatchName;
            batchBDO.Count = batch.Count;
            batchBDO.Description = batch.Description;
            batchBDO.RandomTestNumber = batch.RandTestNumber;
            batchBDO.TestCombination = batch.TestCombination;
            batchBDO.TestDate = batch.TestDate;
            batchBDO.TestProfileID = batch.TestProfileID;
            batchBDO.TestVenueID = batch.TestVenueID;
            batchBDO.RowVersion = batch.RowVersion;
            batchBDO.DateModified = batch.DateModified;
        }
        static void CompositBDOToCompositDAL(CompositBDO compositBDO, Composit composit)
        {
            composit.ALScore = compositBDO.ALScore;
            composit.AQLCode = compositBDO.AQLCode;
            composit.AQLLanguage = compositBDO.AQLLanguage;
            composit.Barcode = compositBDO.Barcode;
            composit.Citizenship = compositBDO.Citizenship;
            composit.Classification = compositBDO.Classification;
            composit.DOB = compositBDO.DOB;
            composit.DOT = compositBDO.DOT;
            composit.ALLevel = compositBDO.ALLevel;
            composit.Faculty = compositBDO.Faculty;
            composit.Faculty2 = compositBDO.Faculty2;
            composit.Faculty3 = compositBDO.Faculty3;
            composit.ForeignID = compositBDO.ForeignID;
            composit.Gender = compositBDO.Gender;
            composit.GR12Language = compositBDO.GR12Language;
            composit.HomeLanguage = compositBDO.HomeLanguage.ToString();
            composit.ID_Type = compositBDO.ID_Type;
            composit.Initials = compositBDO.Initials;
            composit.MatCode = compositBDO.MatCode;
            composit.MatLanguage = compositBDO.MatLanguage;
            composit.MATLevel = compositBDO.MATLevel;
            composit.MATScore = compositBDO.MATScore;
            composit.Name = compositBDO.Name;
            composit.QLLevel = compositBDO.QLLevel;
            composit.QLScore = compositBDO.QLScore;
            composit.RefNo = compositBDO.RefNo;
            composit.RowGuid = compositBDO.RowGuid;
            composit.RowVersion = compositBDO.RowVersion;
            composit.SAID = compositBDO.SAID;
            composit.Surname = compositBDO.Surname;
            composit.VenueName = compositBDO.VenueName;
            composit.VenueCode = compositBDO.VenueCode;
            composit.WroteAL = compositBDO.WroteAL;
            composit.WroteQL = compositBDO.WroteQL;
            composit.WroteMat = compositBDO.WroteMat;
        }
        static void CompositDALToCompositBDO(CompositBDO compositBDO, Composit composit)
        {
            compositBDO.ALScore = composit.ALScore;
            compositBDO.AQLCode = composit.AQLCode;
            compositBDO.AQLLanguage = composit.AQLLanguage;
            compositBDO.Barcode = composit.Barcode;
            compositBDO.Citizenship = composit.Citizenship;
            compositBDO.Classification = composit.Classification;
            compositBDO.DOB = composit.DOB;
            compositBDO.DOT = composit.DOT;
            compositBDO.ALLevel = composit.ALLevel;
            compositBDO.Faculty = composit.Faculty;
            compositBDO.Faculty2 = composit.Faculty2;
            compositBDO.Faculty3 = composit.Faculty3;
            compositBDO.ForeignID = composit.ForeignID;
            compositBDO.Gender = composit.Gender;
            compositBDO.GR12Language = composit.GR12Language;
            compositBDO.HomeLanguage = (int?)Convert.ToInt32(composit.HomeLanguage);
            compositBDO.ID_Type = composit.ID_Type;
            compositBDO.Initials = composit.Initials;
            compositBDO.MatCode = composit.MatCode;
            compositBDO.MatLanguage = composit.MatLanguage;
            compositBDO.MATLevel = composit.MATLevel;
            compositBDO.MATScore = composit.MATScore;
            compositBDO.Name = composit.Name;
            compositBDO.QLLevel = composit.QLLevel;
            compositBDO.QLScore = composit.QLScore;
            compositBDO.RefNo = composit.RefNo;
            compositBDO.RowGuid = composit.RowGuid;
            compositBDO.RowVersion = composit.RowVersion;
            compositBDO.SAID = composit.SAID;
            compositBDO.Surname = composit.Surname;
            compositBDO.VenueName = composit.VenueName;
            compositBDO.VenueCode = composit.VenueCode;
            compositBDO.WroteAL = composit.WroteAL;
            compositBDO.WroteQL = composit.WroteQL;
            compositBDO.WroteMat = composit.WroteMat;
            compositBDO.DateModified = composit.DateModified;
            compositBDO.Batch = compositBDO.Batch;

        }


        static void ProfileBDOToProfileDAL(TestProfileBDO testProfileBDO, TestProfile testProfile)
        {


            testProfile.Profile = testProfileBDO.Profile;
            testProfile.ProfileID = testProfileBDO.ProfileID;
            testProfile.RowGuid = testProfileBDO.RowGuid;
            testProfile.RowVersion = testProfileBDO.RowVersion;
            testProfile.AllocationID = testProfileBDO.AllocationID;
            testProfile.ModifiedDate = testProfileBDO.ModifiedDate;

        }

        static void ProfileDALToProfileBDO(TestProfileBDO testProfileBDO, TestProfile testProfile)
        {
            testProfileBDO.Profile = testProfile.Profile;
            testProfileBDO.ProfileID = testProfile.ProfileID;
            testProfileBDO.RowGuid = testProfile.RowGuid;
            testProfileBDO.RowVersion = testProfile.RowVersion;
            testProfileBDO.AllocationID = testProfile.AllocationID;
        }

        static void ScannedFileBDOToScannedFileDAL(ScannedFileBDO scannedFileBDO, ScannedFile scannedfile)
        {
            scannedfile.FileID = scannedFileBDO.FileID;
            scannedfile.Filename = scannedFileBDO.Filename.ToUpper();
            scannedfile.FileData = File.ReadAllBytes(scannedFileBDO.Filepath);
            scannedfile.DateModified = DateTime.Now;
            scannedfile.Datescanned = File.GetCreationTime(scannedFileBDO.Filepath);
            scannedfile.BatchID = scannedFileBDO.BatchID;
        }

        static void ScannedFileDALToScannedFileBDO(ScannedFileBDO scannedBDO, ScannedFile scannedDAL)
        {
            scannedBDO.BatchID = scannedDAL.BatchID;
            scannedBDO.Filename = scannedDAL.Filename;
            scannedBDO.FileData = scannedDAL.FileData;
            scannedBDO.DateScanned = scannedDAL.Datescanned;
            scannedBDO.FileID = scannedDAL.FileID;

        }
        static void ProvinceDALToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        {
            provinceBDO.Code = province.Code;
            provinceBDO.Id = province.Id;
            provinceBDO.Name = province.Name;
            provinceBDO.RowVersion = province.RowVersion;
        }
        static void provinceBDOtoprovinceDAL(ProvinceBDO provinceBDO, Province province)
        {
            province.Code = provinceBDO.Code;
            province.Id = provinceBDO.Id;
            province.Name = provinceBDO.Name;
            province.RowVersion = provinceBDO.RowVersion;

        }

        static void TestDALToTestBDO(TestBDO testBDO, TestName test)
        {
            testBDO.TestCode = test.TestCode;
            testBDO.DateModified = test.DateModified;
            testBDO.Description = test.Description;
            testBDO.rowguid = test.rowguid;
            testBDO.RowVersion = test.RowVersion;
            testBDO.Section7 = test.Section7;
            testBDO.TestID = test.TestID;
            testBDO.TestName = test.TestName1;


        }
        static void TestBDOtoTestDAL(TestBDO testBDO, TestName test)
        {
            test.DateModified = testBDO.DateModified;
            test.Description = testBDO.Description;
            test.rowguid = testBDO.rowguid;
            // test.RowVersion = testBDO.RowVersion;
            test.Section7 = testBDO.Section7;
            test.TestCode = testBDO.TestCode;
            test.TestID = testBDO.TestID;
            test.TestName1 = testBDO.TestName.ToUpper();
            test.HasErrors = testBDO.HasErrors;

        }



        static void UserDALToUserBDO(UserBDO userBDO, User user)
        {
            userBDO.Areas = user.Areas;
            userBDO.Name = user.Name;
            userBDO.StaffID = user.StaffID;
        }
        static void UserBDOToUserDAL(UserBDO userBDO, User user)
        {
            user.StaffID = userBDO.StaffID;
            user.Name = userBDO.Name;
            user.Areas = userBDO.Areas;
        }

        static void TestVenueToVenueBDO(VenueBDO venueBDO, TestVenue testVenue)
        {
            venueBDO.Available = testVenue.Available;
            venueBDO.Capacity = testVenue.Capacity;
            venueBDO.Description = testVenue.Comments;
            venueBDO.Place = testVenue.Place;
            venueBDO.ProvinceID = testVenue.ProvinceID;
            venueBDO.Room = testVenue.Room;
            venueBDO.ShortName = testVenue.ShortName;
            venueBDO.VenueCode = testVenue.VenueCode;
            venueBDO.VenueName = testVenue.VenueName;
            venueBDO.VenueType = testVenue.VenueType;
            venueBDO.RowGuid = testVenue.RowGuid;
            venueBDO.WebSiteName = testVenue.WebsiteName;
            //ProvinceBDO m = new ProvinceBDO();
            //venueBDO.Province = m;
        }
        static void VenueBDOtoTestVenue(VenueBDO venueBDO, TestVenue venue)
        {
            venue.Available = venueBDO.Available;
            venue.Capacity = venueBDO.Capacity;
            venue.Comments = venueBDO.Description;
            venue.Place = venueBDO.Place;
            venue.ProvinceID = venueBDO.ProvinceID;
            venue.Room = venueBDO.Room;
            venue.ShortName = venueBDO.ShortName;
            venue.VenueCode = venueBDO.VenueCode;
            venue.VenueName = venueBDO.VenueName;
            venue.VenueType = venueBDO.VenueType;
            venue.WebsiteName = venueBDO.WebSiteName;
            venue.RowGuid = venueBDO.RowGuid;

        }

        static void WriterListToWriterlistBDO(WritersBDO writersBDO, WriterList writerlist)
        {
            writersBDO.AccountCreation = writerlist.AccountCreation;
            writersBDO.Classification = writerlist.Classification;
            writersBDO.DateModified = writerlist.DateModified;
            writersBDO.DOB = writerlist.DOB;
            writersBDO.DOT = writerlist.DOT;
            writersBDO.EMail = writerlist.EMail;
            writersBDO.ForeignID = writerlist.ForeignID;
            writersBDO.Gender = writerlist.Gender;
            writersBDO.HomeTelephone = writerlist.HomeTelephone;
            writersBDO.Id = writerlist.Id;
            writersBDO.Initials = writerlist.Initials;
            writersBDO.Mobile = writerlist.Mobile;
            writersBDO.Name = writerlist.Name;
            writersBDO.NBT = writerlist.NBT;
            writersBDO.RegistrationDate = writerlist.RegistrationDate;
            writersBDO.RowGuid = writerlist.RowGuid;
            writersBDO.RowVersion = writerlist.RowVersion;
            writersBDO.SAID = writerlist.SAID;
            writersBDO.Surname = writerlist.Surname;
            writersBDO.TestLanguage = writerlist.TestLanguage;
            writersBDO.TestType = writerlist.TestType;
            writersBDO.VenueID = writerlist.VenueID;
            writersBDO.Wrote = writerlist.Wrote;
            writersBDO.Paid = writerlist.Paid;
            writersBDO.RowGuid = writerlist.RowGuid;
        }

        static void WriterListBDOtoWriterList(WritersBDO writerBDO, WriterList writer)
        {
            writer.AccountCreation = writerBDO.AccountCreation;
            writer.Classification = writerBDO.Classification;
            writer.DateModified = writerBDO.DateModified;
            writer.DOB = writerBDO.DOB;
            writer.DOT = writerBDO.DOT;
            writer.EMail = writerBDO.EMail;
            writer.ForeignID = writerBDO.ForeignID;
            writer.Gender = writerBDO.Gender;
            writer.HomeTelephone = writerBDO.HomeTelephone;
            writer.Id = writerBDO.Id;
            writer.Initials = writerBDO.Initials;
            writer.Mobile = writerBDO.Mobile;
            writer.Name = writerBDO.Name;
            writer.NBT = writerBDO.NBT;
            writer.RegistrationDate = writerBDO.RegistrationDate;
            writer.RowGuid = writerBDO.RowGuid;
            writer.RowVersion = writerBDO.RowVersion;
            writer.SAID = writerBDO.SAID;
            writer.Surname = writerBDO.Surname;
            writer.TestLanguage = writerBDO.TestLanguage;
            writer.TestType = writerBDO.TestType;
            writer.VenueID = writerBDO.VenueID;
            writer.Wrote = writerBDO.Wrote;
            writer.Paid = writerBDO.Paid;

        }

        static void ProvinceToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        {
            provinceBDO.Code = province.Code;
            provinceBDO.Id = province.Id;
            provinceBDO.Name = province.Name;
            provinceBDO.RowVersion = province.RowVersion;
            //provinceBDO.Code = province.Code;
            //provinceBDO.Code = province.Code;
        }

        private void TranslateTestDALToTestBDO(TestName test, TestBDO testBDO)
        {
            testBDO.TestCode = test.TestCode;
            testBDO.DateModified = test.DateModified;
            testBDO.Description = test.Description;
            //		testBDO.Language = test.Language;
            testBDO.HasErrors = test.HasErrors;
            testBDO.Section7 = test.Section7;
            testBDO.TestID = test.TestID;
            testBDO.TestName = test.TestName1;
            testBDO.RowVersion = test.RowVersion;
            testBDO.rowguid = test.rowguid;
            //testBDO.Code = test.Code;
            //testBDO.Code = test.Code;
        }

        private void TranslateTestDBOToTestDAL(TestBDO testBDO, TestName test)
        {
            test.TestCode = testBDO.TestCode;
            test.DateModified = testBDO.DateModified;
            test.Description = testBDO.Description;
            //	test.Language = test.Language;
            test.HasErrors = testBDO.HasErrors;
            test.Section7 = testBDO.Section7;
            test.TestID = testBDO.TestID;
            test.TestName1 = testBDO.TestName;
            test.RowVersion = testBDO.RowVersion;
            test.rowguid = testBDO.rowguid;
            //testBDO.Code = test.Code;
            //testBDO.Code = test.Code;
        }

        private void TranslateTestAllocationDALToTestAllocationBDO(TestAllocation allocation, TestAllocationBDO testAllocationBDO)
        {
            testAllocationBDO.Client = allocation.Client;
            testAllocationBDO.ActualUsed = allocation.ActualUsed;
            testAllocationBDO.ClientType = allocation.ClientType;
            testAllocationBDO.DateModified = allocation.DateModified;
            testAllocationBDO.Estimated = allocation.Estimated;
            testAllocationBDO.ID = allocation.ID;
            testAllocationBDO.RowGuid = allocation.RowGuid;
            testAllocationBDO.RowVersion = allocation.RowVersion;
            testAllocationBDO.TestDate = allocation.TestDate;
            testAllocationBDO.TestID = allocation.TestID;

            testAllocationBDO.TestName = getTestName(allocation.TestID);

        }

        private void TranslateTestAllocationBDOToTestAllocationDAL(TestAllocationBDO testAllocationBDO, TestAllocation allocation)
        {
            allocation.Client = testAllocationBDO.Client;
            allocation.ActualUsed = testAllocationBDO.ActualUsed;
            allocation.ClientType = testAllocationBDO.ClientType;
            allocation.DateModified = testAllocationBDO.DateModified;
            allocation.Estimated = testAllocationBDO.Estimated;
            allocation.ID = testAllocationBDO.ID;
            allocation.RowGuid = testAllocationBDO.RowGuid;
            allocation.RowVersion = testAllocationBDO.RowVersion;
            allocation.TestDate = testAllocationBDO.TestDate;
            allocation.TestID = testAllocationBDO.TestID;

        }

        private void TranslatewritersDALToWritersBDO(WriterList writer, WritersBDO writersBDO)
        {
            writersBDO.AccountCreation = writer.AccountCreation;
            writersBDO.Classification = writer.Classification;
            writersBDO.DateModified = writer.DateModified;
            writersBDO.DOB = writer.DOB;
            writersBDO.DOT = writer.DOT;
            writersBDO.EMail = writer.EMail;
            writersBDO.ForeignID = writer.ForeignID;
            writersBDO.Gender = writer.Gender;
            writersBDO.HomeTelephone = writer.HomeTelephone;
            writersBDO.Id = writer.Id;
            writersBDO.Initials = writer.Initials;
            writersBDO.Mobile = writer.Mobile;
            writersBDO.Name = writer.Name;
            writersBDO.NBT = writer.NBT;
            writersBDO.Paid = writer.Paid;
            writersBDO.RegistrationDate = writer.RegistrationDate;
            writersBDO.RowGuid = writer.RowGuid;
            writersBDO.RowVersion = writer.RowVersion;
            writersBDO.SAID = writer.SAID;
            writersBDO.Surname = writer.Surname;
            writersBDO.TestLanguage = writer.TestLanguage;
            writersBDO.TestType = writer.TestType;
            writersBDO.VenueID = writer.VenueID;
            writersBDO.Wrote = writer.Wrote;

        }
        private void TranslateWebwritersToWritersBDO(WebWriters writer, WritersBDO writersBDO)
        {
            writersBDO.AccountCreation = writer.CreationDate;
            writersBDO.Classification = writer.Classification;
            writersBDO.DOB = writer.DOB;
            writersBDO.DOT = writer.DOT;
            writersBDO.EMail = writer.Email;
            writersBDO.ForeignID = writer.ForeignID;
            writersBDO.Gender = writer.Gender;
            writersBDO.HomeTelephone = writer.HTelephone;
            //writersBDO.Id = writer.
            writersBDO.Initials = writer.initials;
            writersBDO.Mobile = writer.Mobile;
            writersBDO.Name = writer.FirstName;
            writersBDO.NBT = Convert.ToInt64(writer.Reference);
            writersBDO.Paid = Convert.ToDecimal(writer.Paid);
            writersBDO.RegistrationDate = writer.RegDate;
            //writersBDO.RowGuid = writer.
            //writersBDO.RowVersion = writer.RowVersion;
            if (!string.IsNullOrWhiteSpace(writer.SAID)) writersBDO.SAID = Convert.ToInt64(writer.SAID);
            writersBDO.Surname = writer.Surname;
            writersBDO.TestLanguage = writer.Language;
            writersBDO.TestType = writer.Tests;
            VenueBDO VID = GetTestVenueByWebSiteName(writer.Venue);

            if (VID == null)
            {
                string message = "Database has no venue like " + writer.Venue;
                ModernDialog.ShowMessage(message, "Record not saved", MessageBoxButton.OK);
               
            }
            writersBDO.VenueID = VID.VenueCode;
            //writersBDO.Wrote = writer.

        }
        private void TranslatewritersBDOToWritersDAL(WritersBDO writersBDO, WriterList writer)
        {
            writer.AccountCreation = writersBDO.AccountCreation;
            writer.Classification = writersBDO.Classification;
            writer.DateModified = writersBDO.DateModified;
            writer.DOB = writersBDO.DOB;
            writer.DOT = writersBDO.DOT;
            writer.EMail = writersBDO.EMail;
            writer.ForeignID = writersBDO.ForeignID;
            writer.Gender = writersBDO.Gender;
            writer.HomeTelephone = writersBDO.HomeTelephone;
            //writer.Id = writersBDO.Id;
            writer.Initials = writersBDO.Initials;
            writer.Mobile = writersBDO.Mobile;
            writer.Name = writersBDO.Name;
            writer.NBT = writersBDO.NBT;
            writer.Paid = writersBDO.Paid;
            writer.RegistrationDate = writersBDO.RegistrationDate;
            writer.RowGuid = writersBDO.RowGuid;
            writer.RowVersion = writersBDO.RowVersion;
            writer.SAID = writersBDO.SAID;
            writer.Surname = writersBDO.Surname;
            writer.TestLanguage = writersBDO.TestLanguage;
            writer.TestType = writersBDO.TestType;
            writer.VenueID = writersBDO.VenueID;
            writer.Wrote = writersBDO.Wrote;

        }

        static void NewNBTDALToNewNBTBDO(NewNBTNumberBDO nbtBDO, NewNBTNumber nbtNumber)
        {
            nbtBDO.NewNBT = nbtNumber.NewNBT;
            nbtBDO.DateModified = nbtNumber.DateModified;
            nbtBDO.OriginalNBT = nbtNumber.OriginalNBT;
            nbtBDO.Used = nbtNumber.Used;
            nbtBDO.WriterListID = nbtNumber.WriterListID;
        }

        static void NewNBTBDOToNewNBTDAL(NewNBTNumber nbtNumber, NewNBTNumberBDO nbtBDO)
        {
            nbtNumber.NewNBT = nbtBDO.NewNBT;
            nbtNumber.DateModified = nbtBDO.DateModified;
            nbtNumber.OriginalNBT = nbtBDO.OriginalNBT;
            nbtNumber.Used = nbtBDO.Used;
            nbtNumber.WriterListID = nbtBDO.WriterListID;
        }

        static void CSX667ToQADatRecord(ASC667 record, QADatRecord QAData)
        {
            QAData.Barcode = record.SessionID;
            QAData.Reference = record.NBT;
            QAData.SAID = record.SAID.Trim();
            QAData.ForeignID = record.ForeignID;
            QAData.Surname = record.Surname;
            QAData.FirstName = record.Name;
            QAData.initials = record.Initials;
            QAData.DOB = HelperUtils.BioDate(record.DOB);
            QAData.Citizenship = record.Citizenship;
            QAData.DOT = HelperUtils.BioDate(record.DOT);
            QAData.IDType = record.IDType;

            QAData.Classification = record.Classification;
            QAData.Edited = record.CSX.Substring(18, 2);
            QAData.Gender = record.Gender;
            QAData.HomeLanguage = record.HomeLanguage;
            QAData.SchoolLanguage = record.SchoolLanguage;
            QAData.AQL_Language = record.AQL_Language;
            QAData.AQL_Code = record.AQL_Code;
            QAData.VenueCode = record.VenueCode;
            // Char[] sec1 = record.AQL_Section1.ToCharArray();
            QAData.Section1 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section1));
            QAData.Section2 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section2));
            QAData.Section3 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section3));
            QAData.Section4 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_section4));
            QAData.Section5 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section5));
            QAData.Section6 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section6));
            QAData.Section7 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section7));
            QAData.Mat_Language = record.Maths_Language;
            QAData.MatCode = record.Maths_Code;
            QAData.MathSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.Maths_Answers));
            QAData.Faculty1 = record.Faculty1;
            QAData.Faculty2 = record.Faculty2;
            QAData.Faculty3 = record.Faculty3;
            QAData.EOL = record.EndofLine;
            QAData.CSX_Number = record.CSX_Number;
            QAData.CSX_Part = record.CSX;
        }
        static void CSX909ToQADatRecord(ASC909 record, QADatRecord QAData)
        {
            QAData.Barcode = record.SessionID;
            QAData.Reference = record.NBT;
            QAData.SAID = record.SAID.Trim();
            QAData.ForeignID = record.ForeignID;
            QAData.Surname = record.Surname;
            QAData.FirstName = record.Name;
            QAData.initials = record.Initials;
            QAData.DOB = HelperUtils.BioDate(record.DOB);
            QAData.Citizenship = record.Citizenship;
            QAData.DOT = HelperUtils.BioDate(record.DOT);
            QAData.IDType = record.IDType;
            QAData.Classification = record.Classification;
            // string x = record.CSX.Substring(3, 3);
            QAData.ScanNo = Convert.ToInt32(record.CSX.Substring(3, 3));
            QAData.Gender = record.Gender;
            QAData.HomeLanguage = record.HomeLanguage;
            QAData.Edited = record.CSX.Substring(18, 2);
            QAData.SchoolLanguage = record.SchoolLanguage;
            QAData.AQL_Language = record.AQL_Language;
            QAData.AQL_Code = record.AQL_Code;
            QAData.VenueCode = record.VenueCode;
            // Char[] sec1 = record.AQL_Section1.ToCharArray();
            QAData.Section1 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section1));
            QAData.Section2 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section2));
            QAData.Section3 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section3));
            QAData.Section4 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_section4));
            QAData.Section5 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section5));
            QAData.Section6 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section6));
            QAData.Section7 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section7));
            QAData.Mat_Language = record.Maths_Language;
            QAData.MatCode = record.Maths_Code;
            QAData.MathSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.Maths_Answers));
            QAData.Faculty1 = record.Faculty1;
            QAData.Faculty2 = record.Faculty2;
            QAData.Faculty3 = record.Faculty3;
            QAData.EOL = record.EndofLine;
            QAData.CSX_Number = record.CSX_Number;
            QAData.CSX_Part = record.CSX;
        }
        private string QADataRecordToCSX909(QADatRecord QAData)
        {
            string CSX909Record = "";

            string CSX = string.Format("{0,-37}", QAData.CSX_Part);
            string Barcode = string.Format("{0,-12}", QAData.Barcode);
            string Reference = string.Format("{0,-14}", QAData.Reference);

            CSX909Record = QAData.CSX_Number + CSX + Reference + Barcode;

            string ID = string.Format("{0,-13}", QAData.SAID);
            string FID = string.Format("{0,-15}", QAData.ForeignID.Trim());
            string Surname = string.Format("{0,-20}", QAData.Surname);
            string Name = string.Format("{0,-18}", QAData.FirstName);
            string Initials = string.Format("{0,-3}", QAData.initials);
            string IDType = string.Format("{0,-1}", QAData.IDType);
            string Gender = string.Format("{0,-1}", QAData.Gender);

            string VenueCode = string.Format("{0,-5}", QAData.VenueCode);
            string DOT = QAData.DOT.ToString("yyyyMMdd");
            string DOB = QAData.DOB.ToString("yyyyMMdd");
            string AQL_Lang = string.Format("{0,-1}", QAData.AQL_Language);
            string AQL_Code = string.Format("{0,-3}", QAData.AQL_Code);
            string Mat_Lang = string.Format("{0,-1}", QAData.Mat_Language);
            string Mat_Code = string.Format("{0,-3}", QAData.MatCode);
            string Faculty = string.Format("{0,-1}", QAData.Faculty1);
            string Faculty2 = string.Format("{0,-1}", QAData.Faculty2);
            string Faculty3 = string.Format("{0,-1}", QAData.Faculty3);
            string Citizenship = string.Format("{0,-1}", QAData.Citizenship);
            string HLanguage = string.Format("{0,-2}", QAData.HomeLanguage);
            string SLanguage = string.Format("{0,-1}", QAData.SchoolLanguage);
            string Classification = string.Format("{0,-1}", QAData.Classification);

            CSX909Record += ID + FID + IDType + Gender + Citizenship + Faculty + DOB + Surname + Name + Initials;
            string Section1 = HelperUtils.CollectionToString(QAData.Section1);
            string Section2 = HelperUtils.CollectionToString(QAData.Section2);
            string Section3 = HelperUtils.CollectionToString(QAData.Section3);
            string Section4 = HelperUtils.CollectionToString(QAData.Section4);
            string Section5 = HelperUtils.CollectionToString(QAData.Section5);
            string Section6 = HelperUtils.CollectionToString(QAData.Section6);
            string Section7 = HelperUtils.CollectionToString(QAData.Section7);
            string MatSection = HelperUtils.CollectionToString(QAData.MathSection);

            CSX909Record += VenueCode + DOT + HLanguage + SLanguage + Classification + AQL_Lang + AQL_Code;
            CSX909Record += Section1 + Section2 + Section3 + Section4 + Section5 + Section6 + Section7 + Mat_Lang + Mat_Code + MatSection;
            CSX909Record += Faculty + Faculty2 + Faculty3 + QAData.EOL;
            return CSX909Record;
        }
        static void CSX886ToQADatRecord(ASC886 record, QADatRecord QAData)
        {
            QAData.Barcode = record.SessionID;
            QAData.Reference = record.NBT;
            QAData.SAID = record.SAID.Trim();
            QAData.ForeignID = record.ForeignID;
            QAData.Surname = record.Surname;
            QAData.FirstName = record.Name;
            QAData.initials = record.Initials;
            QAData.DOB = HelperUtils.BioDate(record.DOB);
            QAData.Citizenship = record.Citizenship;
            QAData.DOT = HelperUtils.BioDate(record.DOT);
            QAData.IDType = record.IDType;
            QAData.Classification = record.Classification;
            // string x = record.CSX.Substring(3, 3);
            QAData.ScanNo = Convert.ToInt32(record.CSX.Substring(3, 3));
            QAData.Gender = record.Gender;
            QAData.HomeLanguage = record.HomeLanguage;
            QAData.Edited = record.CSX.Substring(18, 2);
            QAData.SchoolLanguage = record.SchoolLanguage;
            QAData.AQL_Language = record.AQL_Language;
            QAData.AQL_Code = record.AQL_Code;
            QAData.VenueCode = record.VenueCode;
            // Char[] sec1 = record.AQL_Section1.ToCharArray();
            QAData.Section1 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section1));
            QAData.Section2 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section2));
            QAData.Section3 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section3));
            QAData.Section4 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_section4));
            QAData.Section5 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section5));
            QAData.Section6 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section6));
            QAData.Section7 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section7));
            QAData.Mat_Language = record.Maths_Language;
            QAData.MatCode = record.Maths_Code;
            QAData.MathSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.Maths_Answers));
            QAData.Faculty1 = record.Faculty1;
            QAData.Faculty2 = record.Faculty2;
            QAData.Faculty3 = record.Faculty3;
            QAData.EOL = record.EndofLine;
            QAData.CSX_Number = record.CSX_Number;
            QAData.CSX_Part = record.CSX;
        }
        private string QADataRecordToCSX886 (QADatRecord QAData)
        {
            string CSX886Record = "";

            string CSX = string.Format("{0,-37}", QAData.CSX_Part);
            string Barcode = string.Format("{0,-12}", QAData.Barcode);
            string Reference = string.Format("{0,-14}", QAData.Reference);

            CSX886Record = QAData.CSX_Number + CSX + Reference + Barcode;

            string ID = string.Format("{0,-13}", QAData.SAID);
            string FID = string.Format("{0,-15}", QAData.ForeignID.Trim());
            string Surname = string.Format("{0,-20}", QAData.Surname);
            string Name = string.Format("{0,-18}", QAData.FirstName);
            string Initials = string.Format("{0,-3}", QAData.initials);
            string IDType = string.Format("{0,-1}", QAData.IDType);
            string Gender = string.Format("{0,-1}", QAData.Gender);

            string VenueCode = string.Format("{0,-5}", QAData.VenueCode);
            string DOT = QAData.DOT.ToString("yyyyMMdd");
            string DOB = QAData.DOB.ToString("yyyyMMdd");
            string AQL_Lang = string.Format("{0,-1}", QAData.AQL_Language);
            string AQL_Code = string.Format("{0,-3}", QAData.AQL_Code);
            string Mat_Lang = string.Format("{0,-1}", QAData.Mat_Language);
            string Mat_Code = string.Format("{0,-3}", QAData.MatCode);
            string Faculty = string.Format("{0,-1}", QAData.Faculty1);
            string Faculty2 = string.Format("{0,-1}", QAData.Faculty2);
            string Faculty3 = string.Format("{0,-1}", QAData.Faculty3);
            string Citizenship = string.Format("{0,-1}", QAData.Citizenship);
            string HLanguage = string.Format("{0,-2}", QAData.HomeLanguage);
            string SLanguage = string.Format("{0,-1}", QAData.SchoolLanguage);
            string Classification = string.Format("{0,-1}", QAData.Classification);

            CSX886Record += ID + FID + IDType + Gender + Citizenship + Faculty + DOB + Surname + Name + Initials;
            string Section1 = HelperUtils.CollectionToString(QAData.Section1);
            string Section2 = HelperUtils.CollectionToString(QAData.Section2);
            string Section3 = HelperUtils.CollectionToString(QAData.Section3);
            string Section4 = HelperUtils.CollectionToString(QAData.Section4);
            string Section5 = HelperUtils.CollectionToString(QAData.Section5);
            string Section6 = HelperUtils.CollectionToString(QAData.Section6);
            string Section7 = HelperUtils.CollectionToString(QAData.Section7);
            string MatSection = HelperUtils.CollectionToString(QAData.MathSection);

            CSX886Record += VenueCode + DOT + HLanguage + SLanguage + Classification + AQL_Lang + AQL_Code;
            CSX886Record += Section1 + Section2 + Section3 + Section4 + Section5 + Section6 + Section7 + Mat_Lang + Mat_Code + MatSection;
            CSX886Record += Faculty + Faculty2 + Faculty3 + QAData.EOL;
            return CSX886Record;
        }

        static void CSX761ToQADatRecord(ASC761 record, QADatRecord QAData)
        {
            QAData.Barcode = record.SessionID;
            QAData.Reference = record.NBT;
            QAData.SAID = record.SAID.Trim();
            QAData.ForeignID = record.ForeignID;
            QAData.Surname = record.Surname;
            QAData.FirstName = record.Name;
            QAData.initials = record.Initials;
            QAData.DOB = HelperUtils.BioDate(record.DOB);
            QAData.Citizenship = record.Citizenship;
            QAData.DOT = HelperUtils.BioDate(record.DOT);
            QAData.IDType = record.IDType;
            QAData.Classification = record.Classification;
            // string x = record.CSX.Substring(3, 3);
            QAData.ScanNo = Convert.ToInt32(record.CSX.Substring(3, 3));
            QAData.Gender = record.Gender;
            QAData.HomeLanguage = record.HomeLanguage;
            QAData.Edited = record.CSX.Substring(18, 2);
            QAData.SchoolLanguage = record.SchoolLanguage;
            QAData.AQL_Language = record.AQL_Language;
            QAData.AQL_Code = record.AQL_Code;
            QAData.VenueCode = record.VenueCode;
            // Char[] sec1 = record.AQL_Section1.ToCharArray();
            QAData.Section1 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section1));
            QAData.Section2 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section2));
            QAData.Section3 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section3));
            QAData.Section4 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_section4));
            QAData.Section5 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section5));
            QAData.Section6 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section6));
            QAData.Section7 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section7));
            QAData.Mat_Language = record.Maths_Language;
            QAData.MatCode = record.Maths_Code;
            QAData.MathSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.Maths_Answers));
            QAData.Faculty1 = record.Faculty1;
            QAData.Faculty2 = record.Faculty2;
            QAData.Faculty3 = record.Faculty3;
            QAData.EOL = record.EndofLine;
            QAData.CSX_Number = record.CSX_Number;
            QAData.CSX_Part = record.CSX;
        }

        private string QADataRecordToCSX761(QADatRecord QAData)
        {
            string CSX761Record = "";

            string CSX = string.Format("{0,-37}", QAData.CSX_Part);
            string Barcode = string.Format("{0,-12}", QAData.Barcode);
            string Reference = string.Format("{0,-14}", QAData.Reference);

            CSX761Record = QAData.CSX_Number + CSX + Reference + Barcode;

            string ID = string.Format("{0,-13}", QAData.SAID);
            string FID = string.Format("{0,-15}", QAData.ForeignID.Trim());
            string Surname = string.Format("{0,-20}", QAData.Surname);
            string Name = string.Format("{0,-18}", QAData.FirstName);
            string Initials = string.Format("{0,-3}", QAData.initials);
            string IDType = string.Format("{0,-1}", QAData.IDType);
            string Gender = string.Format("{0,-1}", QAData.Gender);

            string VenueCode = string.Format("{0,-5}", QAData.VenueCode);
            string DOT = QAData.DOT.ToString("yyyyMMdd");
            string DOB = QAData.DOB.ToString("yyyyMMdd");
            string AQL_Lang = string.Format("{0,-1}", QAData.AQL_Language);
            string AQL_Code = string.Format("{0,-3}", QAData.AQL_Code);
            string Mat_Lang = string.Format("{0,-1}", QAData.Mat_Language);
            string Mat_Code = string.Format("{0,-3}", QAData.MatCode);
            string Faculty = string.Format("{0,-1}", QAData.Faculty1);
            string Faculty2 = string.Format("{0,-1}", QAData.Faculty2);
            string Faculty3 = string.Format("{0,-1}", QAData.Faculty3);
            string Citizenship = string.Format("{0,-1}", QAData.Citizenship);
            string HLanguage = string.Format("{0,-2}", QAData.HomeLanguage);
            string SLanguage = string.Format("{0,-1}", QAData.SchoolLanguage);
            string Classification = string.Format("{0,-1}", QAData.Classification);

            CSX761Record += ID + FID + IDType + Gender + Citizenship + Faculty + DOB + Surname + Name + Initials;
            string Section1 = HelperUtils.CollectionToString(QAData.Section1);
            string Section2 = HelperUtils.CollectionToString(QAData.Section2);
            string Section3 = HelperUtils.CollectionToString(QAData.Section3);
            string Section4 = HelperUtils.CollectionToString(QAData.Section4);
            string Section5 = HelperUtils.CollectionToString(QAData.Section5);
            string Section6 = HelperUtils.CollectionToString(QAData.Section6);
            string Section7 = HelperUtils.CollectionToString(QAData.Section7);
            string MatSection = HelperUtils.CollectionToString(QAData.MathSection);

            CSX761Record += VenueCode + DOT + HLanguage + SLanguage + Classification + AQL_Lang + AQL_Code;
            CSX761Record += Section1 + Section2 + Section3 + Section4 + Section5 + Section6 + Section7 + Mat_Lang + Mat_Code + MatSection;
            CSX761Record += Faculty + Faculty2 + Faculty3 + QAData.EOL;
            return CSX761Record;
        }

        private string QADataRecordToCSX667(QADatRecord QAData)
        {
            string CSX667Record = "";

            string CSX = string.Format("{0,-37}", QAData.CSX_Part);
            string Barcode = string.Format("{0,-12}", QAData.Barcode);
            string Reference = string.Format("{0,-14}", QAData.Reference);

            CSX667Record = QAData.CSX_Number + CSX + Reference + Barcode;

            string ID = string.Format("{0,-13}", QAData.SAID);
            string FID = string.Format("{0,-15}", QAData.ForeignID.Trim());
            string Surname = string.Format("{0,-20}", QAData.Surname);
            string Name = string.Format("{0,-18}", QAData.FirstName);
            string Initials = string.Format("{0,-3}", QAData.initials);
            string IDType = string.Format("{0,-1}", QAData.IDType);
            string Gender = string.Format("{0,-1}", QAData.Gender);
            string VenueCode = string.Format("{0,-5}", QAData.VenueCode);
            string DOT = QAData.DOT.ToString("yyyyMMdd");
            string DOB = QAData.DOB.ToString("yyyyMMdd");
            string AQL_Lang = string.Format("{0,-1}", QAData.AQL_Language);
            string AQL_Code = string.Format("{0,-2}", QAData.AQL_Code);
            string Mat_Lang = string.Format("{0,-1}", QAData.Mat_Language);
            string Mat_Code = string.Format("{0,-2}", QAData.MatCode);
            string Faculty = string.Format("{0,-1}", QAData.Faculty1);
            string Faculty2 = string.Format("{0,-1}", QAData.Faculty2);
            string Faculty3 = string.Format("{0,-1}", QAData.Faculty3);
            string Citizenship = string.Format("{0,-1}", QAData.Citizenship);
            string HLanguage = string.Format("{0,-2}", QAData.HomeLanguage);
            string SLanguage = string.Format("{0,-1}", QAData.SchoolLanguage);
            string Classification = string.Format("{0,-1}", QAData.Classification);

            CSX667Record += ID + FID + IDType + Gender + Citizenship + Faculty + DOB + Surname + Name + Initials;
            string Section1 = HelperUtils.CollectionToString(QAData.Section1);
            string Section2 = HelperUtils.CollectionToString(QAData.Section2);
            string Section3 = HelperUtils.CollectionToString(QAData.Section3);
            string Section4 = HelperUtils.CollectionToString(QAData.Section4);
            string Section5 = HelperUtils.CollectionToString(QAData.Section5);
            string Section6 = HelperUtils.CollectionToString(QAData.Section6);
            string Section7 = HelperUtils.CollectionToString(QAData.Section7);
            string MatSection = HelperUtils.CollectionToString(QAData.MathSection);
            string Freespace = "  ";

            CSX667Record += VenueCode + DOT + HLanguage + SLanguage + Classification + AQL_Lang + AQL_Code;
            CSX667Record += Section1 + Section2 + Section3 + Section4 + Section5 + Section6 + Section7 + Mat_Lang + Mat_Code + MatSection;
            CSX667Record += Freespace + Faculty + Faculty2 + Faculty3 + QAData.EOL;
            return CSX667Record;
        }

        #endregion

        #region Writer prep

        public ObservableCollection<WebWriters> GetData(string filename)
        {
            FileInfo infofile = new FileInfo(filename);
            if (!HelperUtils.IsFileLocked(infofile))
            {
                //int count = 0;
                try
                {

                    hasrecords = false;
                    StreamReader textreader = new StreamReader(filename);
                    var csv = new CsvReader(textreader);
                    csv.Configuration.Encoding = Encoding.Unicode;
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.CultureInfo = CultureInfo.CurrentCulture;
                    csv.Configuration.HasHeaderRecord = true;
                    //csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.WillThrowOnMissingField = false;
                    csv.Configuration.Quote = '"';
                    // csv.Configuration.RegisterClassMap<WebWriterMap>();

                    WritersList1 = new ObservableCollection<WebWriters>();

                    while (csv.Read())
                    {
                        //count++;
                        WebWriters record = new WebWriters();
                        record.Reference = csv.GetField<string>(0);
                        string sn = csv.GetField<string>(1);
                        record.Surname = sn.Trim();
                        string fn = csv.GetField<string>(2);
                        record.FirstName = fn.Trim();
                        record.initials = csv.GetField<string>(3);
                        record.SAID = csv.GetField<string>(4);
                        record.ForeignID = csv.GetField<string>(5);

                        string m = csv.GetField<string>(6);
                        record.DOB = HelperUtils.WebDate(m);
                        record.Gender = csv.GetField<string>(7);
                        record.Classification = csv.GetField<string>(8);
                        record.Tests = csv.GetField<string>(9);
                        record.Language = csv.GetField<string>(10);
                        record.Venue = csv.GetField<string>(11);

                        string dot = csv.GetField<string>(12);
                        record.DOT = HelperUtils.WebDate(dot);
                        record.Mobile = csv.GetField<string>(13);
                        record.HTelephone = csv.GetField<string>(14);
                        record.Email = csv.GetField<string>(15);

                        string dor = csv.GetField<string>(16);
                        record.RegDate = HelperUtils.weblistDateTime(dor);

                        string payed = csv.GetField<string>(17);
                        if (string.IsNullOrWhiteSpace(payed))
                        {
                            record.Paid = 0.0;
                        }
                        else
                        {
                            record.Paid = csv.GetField<double>(17);
                        }
                        string doc = csv.GetField<string>(18);
                        record.CreationDate = HelperUtils.weblistDateTime(doc);
                        // check dates up here
                        WritersList1.Add(record);
                    }

                    hasrecords = true;
                    WritersList1.OrderByDescending(x => x.Reference);

                }
                catch (Exception ex)
                {
                    //int a = count;
                    log.Error("File has corrupt columns", ex);
                    MessageBox.Show(ex.ToString());
                    throw ex;
                }
            }
            else
            {
                log.Error(infofile + " is being used somewher else");
            }
            return WritersList1;
        }

        public void generateFile(string Filename)
        {
            if (hasrecords)
            {
                try
                {
                    StreamWriter TextWriter = new StreamWriter(Filename);
                    var csv = new CsvWriter(TextWriter);
                    csv.Configuration.Encoding = Encoding.Unicode;
                    csv.Configuration.HasHeaderRecord = true;
                    List<WebWriters> writers = new List<WebWriters>(WritersList1);
                    List<WebWriters1> games = new List<WebWriters1>();

                    foreach (var rec in writers)
                    {
                        WebWriters1 x = new WebWriters1();
                        x = ConvertTorecord(rec, x);

                        games.Add(x);

                    }
                    csv.WriteRecords(games);
                    TextWriter.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Unable to generate file", ex);
                    throw ex;
                }

            }

        }
        private WebWriters1 ConvertTorecord(WebWriters rec, WebWriters1 x)
        {
            x.Reference = rec.Reference;
            x.Surname = rec.Surname.Trim();
            x.FirstName = rec.FirstName.Trim();
            x.initials = rec.initials;
            x.SAID = rec.SAID;
            x.ForeignID = rec.ForeignID;
            x.DOB = Convert.ToDateTime(rec.DOB);
            x.Gender = rec.Gender;
            x.Classification = rec.Classification;
            x.DOT = Convert.ToDateTime(rec.DOT);
            x.Tests = rec.Tests;
            x.Language = rec.Language;
            x.Venue = rec.Venue;
            x.Mobile = rec.Mobile;
            x.HTelephone = rec.HTelephone;
            x.Email = rec.Email;
            //x.RegDate = HelperUtils.UnixTimeToDateTime(Convert.ToInt64(rec.RegDate));
            x.RegDate = Convert.ToDateTime(rec.RegDate);
            x.Paid = rec.Paid;
            // x.CreationDate = HelperUtils.UnixTimeToDateTime(Convert.ToInt64(rec.CreationDate));
            x.CreationDate = Convert.ToDateTime(rec.CreationDate);
            x.Last = "";
            return x;
        }

        public ObservableCollection<WebWriters> Processdata()
        {
            if (!hasrecords)
            {
                WritersList1 = new ObservableCollection<WebWriters>();
            }
            return WritersList1;
        }
        public bool DeleteWriterfromList(WebWriters writer)
        {
            bool ret = false;
            ret = WritersList1.Remove(writer);
            return ret;
        }
        /// <summary>
        /// ********************************************************************************************************
        /// </summary>
        public void CleanFunnyChars()
        {
            if (WritersList1 == null) return;
            var ApplicantErrors = WritersList1.Where(x => x.HasErrors == true).Select(m => m).ToList();

            foreach (var a in ApplicantErrors)
            {
                var myerrors = a._errors.Keys.ToList();
                int count = myerrors.Count;
                for (int i = 0; i < count; i++)
                {
                    string area = myerrors[i];
                    switch (area)
                    {
                        case "Surname":
                            StringBuilder b = new StringBuilder(a.Surname);

                            if (a.Surname.Contains("&#039;"))
                            {
                                b.Replace("&#039;", string.Empty);
                                a.Surname = b.ToString();

                            }


                            break;

                        case "FirstName":
                            StringBuilder c = new StringBuilder(a.FirstName);
                            if (a.FirstName.Contains("&#039;"))
                            {
                                c.Replace("&#039;", string.Empty);
                                a.FirstName = c.ToString();
                            }
                            if (a.FirstName.Length > 18)
                            {
                                string myName = "";
                                string[] mynames = a.FirstName.Split(' ');
                                int count1 = mynames.Count();
                                if (count1 > 1)
                                {
                                    for (int u = 0; u < count1 - 1; u++)
                                    {
                                        if (u != 0) myName += " ";
                                        myName += mynames[u];
                                    }
                                    a.FirstName = myName.Trim();
                                }
                                else
                                {
                                    a.FirstName = mynames[0];
                                }
                            }
                            break;

                        case "SAID":       // proper cleaning of  South Arican ID
                            if (string.IsNullOrWhiteSpace(a.ForeignID))
                            {
                                a.ForeignID = a.SAID;
                                a.SAID = null;
                            }
                            else
                            {
                                a.SAID = null;
                            }
                            break;
                        case "DOB": // Cleaning of DateTime of Birth
                            if (!string.IsNullOrWhiteSpace(a.SAID))
                            {
                                string myDOB = HelperUtils.DOBfromSAID(a.SAID);
                                a.DOB = DateTime.ParseExact(myDOB, "dd/MM/yyyy", null);

                            }
                            else
                            {
                                string myDOB = "1960/01/01";
                                a.DOB = DateTime.ParseExact(myDOB, "yyyy/MM/dd", null);
                            }
                            break;

                    }
                }
            }
        }
        /// <summary>
        /// ***********************************************************************************************************
        /// </summary>

        public bool GetNewNBT(WebWriters writer, ref string NewNBT)
        {
            bool ret = false;
            NewNBT = "";
            NewNBTNumberBDO myNBT = new NewNBTNumberBDO();
            Int64 OldNBT = Convert.ToInt64(writer.Reference);

            // call NewNBTNumbers from Database                 
            myNBT = GetNewNBTNumberFromDB();


            if (myNBT != null)
            {
                writer.Reference = myNBT.NewNBT.ToString();
                myNBT.OriginalNBT = OldNBT;
                // writer old NBT number to database
                string msg = "";
                ret = UpdateNBTNumbers(myNBT, ref msg);

                NewNBT = "Changed from: " + OldNBT.ToString() + "To: " + writer.Reference + msg;
                ret = true;
            }
            else
            {
                NewNBT = "No New NBt numbers available to allocate";
            }

            return ret;
        }
        #endregion

        #region Writerlist to Database

        public ObservableCollection<WebWriters> GetDuplicates()
        {
            //1. Check for duplicates in File

            List<WebWriters> writers = new List<WebWriters>(WritersList1);

            var duplicates = writers.GroupBy(x => x.Reference)
                                    .Where(g => g.Count() > 1)
                                    .SelectMany(m => m).ToList();
            //.SelectMany(grp => grp.Skip(1)).ToList();

            ObservableCollection<WebWriters> duplicate = new ObservableCollection<WebWriters>(duplicates);
            return duplicate;

        }
        public async Task<ObservableCollection<WebWriters>> GetDuplicatesfromDBAsync()
        {
            //1. Check for duplicates in Database
            List<WebWriters> writers = new List<WebWriters>(WritersList1);
            //ObservableCollection<WebWriters> duplic = null;
            var duplic = new ObservableCollection<WebWriters>();
            using (var context = new CETAPEntities())
            {
                //Parallel.ForEach(writers, (m) =>
                //    {
                //        long myNBT = Convert.ToInt64(m.Reference);
                //        var found = context.WriterLists.Where(x => x.NBT == myNBT && x.Surname != m.Surname).Select(x => x).FirstOrDefault();
                //        if (found != null) duplic.Add(m);
                //    });
                foreach (var m in writers)
                {
                    long myNBT = Convert.ToInt64(m.Reference);
                    var found = await context.WriterLists.Where(x => x.NBT == myNBT && x.SAID.ToString() != m.SAID && x.ForeignID != m.ForeignID).Select(x => x).FirstOrDefaultAsync();
                    if (found != null) duplic.Add(m);
                }
                //var ids = context.WriterLists.Select(x => x.NBT);
                //var wr = (from a in writers
                //          join b in ids
                //              on a.Reference equals Convert.ToInt64(b)
                //          into xx
                //          select new { a });
                //duplic = new ObservableCollection<WebWriters>(wr);
            }


            return duplic;
        }
        public List<WritersBDO> GetAllwriters()
        {
            using (var context = new CETAPEntities())
            {
                WritersBDO writersBDO = null;
                List<WritersBDO> allwriters = new List<WritersBDO>();
                try
                {


                    // get all database	tests

                    List<WriterList> writers = (from p in context.WriterLists
                                                select p).ToList();

                    //Convert each test to TestBDO
                    foreach (WriterList writer in writers)
                    {
                        writersBDO = new WritersBDO();
                        TranslatewritersDALToWritersBDO(writer, writersBDO);
                        //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                        allwriters.Add(writersBDO);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Cannot get writers from Db", ex);
                    throw ex;
                }



                return allwriters;
            }
        }

        public async Task<bool> addwriterToDBAsync()
        {
            var task = false;
            string logUser = "";

            var DbWriters = new List<WriterList>();
            var ApplicantsBDO = new ConcurrentBag<WritersBDO>();

            //Parallel.ForEach(WritersList1, (applic) =>
            //{
            //    WritersBDO writer = new WritersBDO();
            //    TranslateWebwritersToWritersBDO(applic, writer);
            //    ApplicantsBDO.Add(writer);
            //});

            foreach (var applic in WritersList1)
            {
                WritersBDO writer = new WritersBDO();
                TranslateWebwritersToWritersBDO(applic, writer);
                ApplicantsBDO.Add(writer);
            }

            using (var context = new CETAPEntities())
            {
                using (var transScope = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
                {

                    try
                    {
                        foreach (var applicant in ApplicantsBDO)
                        {

                            logUser = "Writer not saved to Database : " + applicant.Name + " " + applicant.Surname + " " + applicant.NBT;

                            var writerInDB = new WriterList();
                            TranslatewritersBDOToWritersDAL(applicant, writerInDB);
                            writerInDB.RowGuid = Guid.NewGuid();
                            writerInDB.DateModified = DateTime.Now;

                            DbWriters.Add(writerInDB);

                        }

                        context.WriterLists.AddRange(DbWriters);

                        await context.SaveChangesAsync();
                        transScope.Complete();
                        task = true;
                    }
                    catch (Exception ex)
                    {
                        task = false;

                        log.Error(logUser, ex);
                        throw ex;
                    }

                }
            }
            return task;
        }

        #endregion

        #region NewNBTNumbers
        public NewNBTNumberBDO GetNewNBTNumberFromDB()
        {
            //NewNBTNumberBDO newNBT = new NewNBTNumberBDO();
            var task = new NewNBTNumberBDO();
            using (var context = new CETAPEntities())
            {
                NewNBTNumber name = context.NewNBTNumbers.Where(x => x.OriginalNBT == null).Select(x => x).FirstOrDefault();
                NewNBTDALToNewNBTBDO(task, name);
            }
            return task;
        }
        public bool UpdateNBTNumbers(NewNBTNumberBDO myNBT, ref string message)
        {
            bool ret = false;
            message = "NBT updated!!";

            using (var context = new CETAPEntities())
            {
                var ID = myNBT.NewNBT;
                NewNBTNumber nbtInDB = context.NewNBTNumbers.Where(x => x.NewNBT == ID).FirstOrDefault();

                if (nbtInDB != null)
                {
                    ret = true;
                    context.NewNBTNumbers.Remove(nbtInDB);

                    //update testVenue
                    // TestName mytest = new TestName();
                    NewNBTBDOToNewNBTDAL(nbtInDB, myNBT);
                    nbtInDB.DateModified = DateTime.Now;
                    nbtInDB.Used = true;
                    context.NewNBTNumbers.Attach(nbtInDB);
                    context.Entry(nbtInDB).State = System.Data.Entity.EntityState.Modified;
                    int num = context.SaveChanges();


                }
                else
                {
                    log.Warn("Something wrong with NBt number allocation");
                    message = "No such NBT Number " + myNBT.NewNBT;
                }
            }
            return ret;
        }
        #endregion

        #region Easypay



        public bool FTPconnect(string site, string name, string password)
        {
            bool Isconnection = true;
            ftp easypay = new ftp(site, name, password);
            return Isconnection;
        }

        public ObservableCollection<EasypayFile> ListFTPFiles()
        {
            string[] myfiles;
            ftp easypay = new ftp(Site, user, password);
            myfiles = easypay.directoryListDetailed("");
            ObservableCollection<EasypayFile> data = new ObservableCollection<EasypayFile>();
            foreach (var a in myfiles)
            {
                if (a.Length < 20) continue;
                EasypayFile pay = new EasypayFile();
                pay.FileDetails = a;
                //pay.Size = easypay.getFileSize(pay.FileName);
                data.Add(pay);
            }
            return data;
        }

        public async Task DownloadfileAsync(string filename, string Localfile)
        {
            string myfile = Localfile;
            ftp easypay = new ftp(Site, user, password);
            await easypay.downloadAsync(filename, myfile);
        }

        public bool LoadEasyPayToDB(string filename, ref string message)
        {
            throw new NotImplementedException();
        }

        public async Task WriteFilesToDBAsync(string folder)
        {
            var myfiles = Directory.GetFiles(folder);
            //  myfiles = myfiles.Where(x => x.StartsWith("easy3100")).ToArray();
            foreach (string filename in myfiles)
            {
                await ReadFolderFilesAsync(filename);
            }
        }

        private async Task ReadFolderFilesAsync(string theFile)
        {

            string Filename;
            string nameOfFile;
            Decimal TotalAmount = 0;
            Decimal EasyPayFees = 0;
            Decimal BankFees = 0;

            // openFileDialog1.ShowDialog();
            Filename = theFile;
            //listBox1.Items.Add(DateTime.Now);
            ReadEasyPayFile Readfile = new ReadEasyPayFile(Filename);

            //Open the database proxy


            EasyPayFile DbFile = new EasyPayFile();
            //  EasyPayRecord DbEPRecord = new EasyPayRecord();

            string myS;
            myS = Readfile.Filename;
            // Filename to be written to database
            nameOfFile = Path.GetFileName(myS);

            //check first record
            int axx = Readfile.SOF.FileGenerationNumber;
            await CheckAddFileAsync(axx);
            if (canAddFile)
            {
                //write to database
                using (CETAPEntities Manager = new CETAPEntities())
                {
                    DbFile.FileGenerationNumber = axx;
                    DbFile.FileName = nameOfFile;
                    DbFile.DateWritten = Readfile.SOF.FileDate.ToShortDateString();
                    DbFile.Time = Readfile.SOF.CreationTime.ToShortTimeString();

                    Manager.EasyPayFiles.Add(DbFile);
                    await Manager.SaveChangesAsync();
                }
                /// <summary>
                using (CETAPEntities Manager = new CETAPEntities())
                {
                    if (Readfile.EOF.TotalTenders > 0)
                    {

                        List<easyPayRec> myData = new List<easyPayRec>();
                        myData = Readfile.Records;
                        foreach (easyPayRec var in myData)
                        {
                            //Add all fees and money paid
                            TotalAmount += var.Payment.Amount;
                            EasyPayFees += var.Payment.Fee;
                            Decimal xTender = 0;
                            //if (var.Transactions.Count > 1)
                            //{
                            for (int i = 0; i < var.Tenders.Count; i++)
                            {
                                xTender += var.Tenders[i].BankCost;

                            }
                            BankFees += xTender;


                            string a;
                            a = var.Payment.PayTag + "  " + var.Transaction.CollectorID + "  " + var.Payment.Fee + "     " + var.Transaction.PayDay.ToShortDateString() + "    " + "  " + var.Payment.Amount + "  " + var.Transaction.PayTime.ToShortTimeString() + "  " + var.Transaction.PointOfService;
                            a += "  " + var.Tender.AccountNumber + "  " + var.Tender.TenderType + "  " + var.Tender.Amount + "  " + var.Transaction.TracePayee + "    " + Readfile.SOF.FileGenerationNumber;

                            CETAP_LOB.Database.EasyPayRecord DbEPRecord = new CETAP_LOB.Database.EasyPayRecord();

                            DbEPRecord.FileGenerationNumber = axx;
                            DbEPRecord.BankCost = xTender;
                            DbEPRecord.Fee = var.Payment.Fee;
                            DbEPRecord.PointOfService = var.Transaction.PointOfService;
                            DbEPRecord.CollectorID = var.Transaction.CollectorID;
                            DbEPRecord.PayTag = var.Payment.PayTag;
                            DbEPRecord.NumOfTrans = Convert.ToInt16(var.Tenders.Count);
                            DbEPRecord.AmountPaid = var.Payment.Amount;
                            DbEPRecord.Time = var.Transaction.PayTime.ToShortTimeString();
                            DbEPRecord.Trace = var.Transaction.TracePayee;
                            DbEPRecord.date = var.Transaction.PayDay.ToShortDateString();
                            DbEPRecord.TenderType_PAN = var.Tender.TenderType;

                            Manager.EasyPayRecords.Add(DbEPRecord);

                            // listBox1.Items.Add(a);

                        }

                    }

                    var result = await Manager.EasyPayFiles.SingleOrDefaultAsync(b => b.FileGenerationNumber == DbFile.FileGenerationNumber);
                    result.CalculatedAmountCollected = TotalAmount;
                    result.TotalBankFees = BankFees;
                    result.TotalFees = EasyPayFees;
                    result.NumberOfTenders = Convert.ToInt32(Readfile.EOF.NumberOfTenders);
                    result.NumberOfPayments = Convert.ToInt32(Readfile.EOF.TotalPayments);
                    result.TotalPayment = Readfile.EOF.TotalPayments;
                    // Manager.EasyPayFiles.Add(DbFile);
                    await Manager.SaveChangesAsync();

                    //write all records to database
                }
            }
        }

        public async Task CheckAddFileAsync(int filenumber)
        {
            using (CETAPEntities Manager = new CETAPEntities())
            {
                EasyPayFile ep = await Manager.EasyPayFiles.SingleOrDefaultAsync(p => p.FileGenerationNumber == filenumber);
                if (ep != null)
                {
                    canAddFile = false;
                }
                else
                {
                    canAddFile = true;
                }
            }
        }

        public EasyPayFile ReadLastFile()
        {
            var file1 = new EasyPayFile();
            using (CETAPEntities context = new CETAPEntities())
            {
                file1 = context.EasyPayFiles.OrderByDescending(x => x.FileGenerationNumber).Select(x => x).First();

            }
            return file1;
        }
        public async Task<ObservableCollection<Vw_EasyPayRecords>> GetEasyPayRecords(DateTime startDate, DateTime endDate)
        {
            var EPRecs = new ObservableCollection<Vw_EasyPayRecords>();
            string Date1 = startDate.ToString("yyyy/MM/dd");

            // get last Loaded file if  last date is greater;
            var myLastFile = new EasyPayFile();
            myLastFile = ReadLastFile();
            string Date2 = endDate.ToString("yyyy/MM/dd");
            // if(myLastFile.)

            using (CETAPEntities context = new CETAPEntities())
            {
                // get the file numbers
                int StartNum = context.EasyPayFiles.Where(s => s.DateWritten == Date1).Select(s => s.FileGenerationNumber).FirstOrDefault();
                int endNum = context.EasyPayFiles.Where(s => s.DateWritten == Date2).Select(s => s.FileGenerationNumber).FirstOrDefault();
                if ( endNum != 0)
                {
                    var records = await context.Vw_EasyPayRecords.Where(x => x.File_Number >= StartNum && x.File_Number <= endNum).OrderBy(x => x.File_Number).Select(x => x).ToListAsync();
                    EPRecs = new ObservableCollection<Vw_EasyPayRecords>(records);
                }
                else
                {
                    endNum = myLastFile.FileGenerationNumber;
                    var records = await context.Vw_EasyPayRecords.Where(x => x.File_Number >= StartNum && x.File_Number <= endNum).OrderBy(x => x.File_Number).Select(x => x).ToListAsync();
                    EPRecs = new ObservableCollection<Vw_EasyPayRecords>(records);
                }
                //var recs = records.Where(m => Convert.ToDateTime(m.Payment_Date) >= startDate && Convert.ToDateTime(m.Payment_Date) <= endDate).OrderBy(x => x.Payment_Date).ToList();


            }
            return EPRecs;

        }


        #endregion

        #region Composite Scores
        //generate collection of Full Composite

        private void CompositToFullComposite()
        {
            if (AllScores != null)
            {
                var Compo = from mydata in AllScores.AsParallel()
                            select new
                            {
                                RefNo = mydata.RefNo.ToString(),
                                Barcode = mydata.Barcode.ToString(),
                                LastName = mydata.Surname,
                                FName = mydata.Name,
                                Initials = mydata.Initials,
                                SAID = HelperUtils.ToSAID(mydata.SAID),
                                FID = mydata.ForeignID,
                                DOB = String.Format("{0:yyyyMMdd}", mydata.DOB),
                                IDType = mydata.ID_Type,
                                Citizenship = (mydata.Citizenship != null ? mydata.Citizenship : null),
                                Classification = mydata.Classification,
                                Gender = mydata.Gender,
                                faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
                                Testdate = String.Format("{0:yyyyMMdd}", mydata.DOT),
                                VenueCode = mydata.VenueCode.ToString("D5"),
                                VenueName = mydata.VenueName,
                                Hlanguage = mydata.HomeLanguage,
                                G12L = mydata.GR12Language,
                                AQLLang = mydata.AQLLanguage,
                                AQLCode = mydata.AQLCode,
                                MatLang = mydata.MatLanguage,
                                MatCode = mydata.MatCode,
                                Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
                                Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
                                SessionID = mydata.Barcode.ToString(),
                                NBTNumber = mydata.RefNo.ToString(),
                                Surname = mydata.Surname,
                                Name = mydata.Name,
                                Initial = mydata.Initials,
                                SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
                                Passport = mydata.ForeignID,
                                Birth = (mydata.SAID != null ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) :
                                String.Format("{0:dd/MM/yyyy}", mydata.DOB)),

                                W_AL = mydata.WroteAL,
                                W_QL = mydata.WroteQL,
                                W_Mat = mydata.WroteMat,
                                StNo = "",
                                Faculty = "",
                                Programme = "",
                                DateTest = String.Format("{0:yyyyMMdd}", mydata.DOT),
                                Venue = mydata.VenueName,
                                Sex = (mydata.Gender == "1" ? "M" :
                                       mydata.Gender == "2" ? "F" : mydata.Gender),
                                street1 = "",
                                street2 = "",
                                Suburb = "",
                                City = "",
                                Province = "",
                                Postal = "",
                                Email = "",
                                Landline = "",
                                Mobile = "",
                                ALScore = mydata.ALScore,
                                ALLevel = mydata.ALLevel,
                                QLScore = mydata.QLScore,
                                QLLevel = mydata.QLLevel,
                                MatScore = mydata.MATScore,
                                MatLevel = mydata.MATLevel,
                                AQL_Lang = mydata.AQLLanguage,
                                Mat_Lang = mydata.MatLanguage

                            };
                foreach (var a in Compo)
                {
                    FullComposite FC = new FullComposite();

                    FC.RefNo = a.RefNo;
                    FC.Barcode = a.Barcode;
                    FC.LastName = a.LastName;
                    FC.FirstName = a.FName;
                    FC.Initials = a.Initials;
                    FC.SAID = a.SAID;
                    FC.FID = a.FID;
                    FC.DOB = a.DOB;
                    FC.IDType = a.IDType;
                    FC.Citizenship = a.Citizenship.ToString();
                    FC.Classification = a.Classification;
                    FC.Gender = a.Gender;
                    FC.Faculty1 = a.faculty1;
                    FC.DOT = a.Testdate;
                    FC.VenueCode = a.VenueCode;
                    FC.VenueName = a.VenueName;
                    FC.HomeLanguage = a.Hlanguage.ToString();
                    FC.SchLanguage = a.G12L;
                    FC.AQLLang = a.AQLLang;
                    FC.AQLCode = a.AQLCode.ToString();
                    FC.MATLang = a.MatLang;
                    FC.MATCode = a.MatCode.ToString();
                    FC.Faculty2 = a.Faculty2;
                    FC.Faculty3 = a.Faculty3;
                    FC.SessionId = a.SessionID;
                    FC.NBT = a.NBTNumber;
                    FC.Surname = a.Surname;
                    FC.Name = a.Name;
                    FC.MiddleInitials = a.Initial;
                    FC.SouthAfricanID = a.SouthAfricanID;
                    FC.Passport = a.Passport;
                    FC.Birth = a.Birth;
                    FC.WroteAL = a.W_AL;
                    FC.WroteQL = a.W_QL;
                    FC.WroteMat = a.W_Mat;
                    FC.StudentNo = a.StNo;
                    FC.Faculty = a.Faculty;
                    FC.Programme = a.Programme;
                    FC.TestDate = a.DateTest;
                    FC.Venue = a.Venue;
                    FC.Sex = a.Sex;
                    FC.Street1 = a.street1;
                    FC.Street2 = a.street2;
                    FC.Suburb = a.Suburb;
                    FC.City = a.City;
                    FC.Province = a.Province;
                    FC.Postal = a.Postal;
                    FC.Email = a.Email;
                    FC.Landline = a.Landline;
                    FC.Mobile = a.Mobile;
                    FC.ALScore = a.ALScore.ToString();
                    FC.ALLevel = a.ALLevel;
                    FC.QLScore = a.QLScore.ToString();
                    FC.QLLevel = a.QLLevel;
                    FC.MatScore = a.MatScore.ToString();
                    FC.MatLevel = a.MatLevel;
                    FC.AQLLanguage = a.AQL_Lang;
                    FC.MatLanguage = a.Mat_Lang;
                    //FC.RefNo = a.RefNo;
                    //FC.RefNo = a.RefNo;
                    //FC.RefNo = a.RefNo;


                    FComposite.Add(FC);
                }
            }
        }
        // generate collection of LOgistics data

        private void CompositToLogisticsComposite()
        {
            if (AllScores != null)
            {
                var Compo = (from mydata in AllScores.AsParallel()
                             select new
                             {
                                 SessionID = mydata.Barcode.ToString(),
                                 NBTNumber = mydata.RefNo.ToString(),
                                 Surname = mydata.Surname,
                                 Name = mydata.Name,
                                 Initial = mydata.Initials,
                                 SouthAfricanID = mydata.SAID.ToString(),
                                 Passport = mydata.ForeignID,
                                 Birth = (mydata.SAID != null ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) :
                                 String.Format("{0:dd/MM/yyyy}", mydata.DOB)),

                                 W_AL = mydata.WroteAL,
                                 W_QL = mydata.WroteQL,
                                 W_Mat = mydata.WroteMat,
                                 StNo = "",
                                 Faculty = "",
                                 Programme = "",
                                 DateTest = String.Format("{0:yyyyMMdd}", mydata.DOT),
                                 Venue = mydata.VenueName,
                                 Sex = (mydata.Gender == "1" ? "M" :
                                        mydata.Gender == "2" ? "F" : mydata.Gender),

                                 ALScore = mydata.ALScore,
                                 ALLevel = mydata.ALLevel,
                                 QLScore = mydata.QLScore,
                                 QLLevel = mydata.QLLevel,
                                 MatScore = mydata.MATScore,
                                 MatLevel = mydata.MATLevel,
                                 AQL_Lang = mydata.AQLLanguage,
                                 Mat_Lang = mydata.MatLanguage,
                                 mm = mydata.Batch,
                                 TestType = MytestType(mydata.Batch),
                                 province = getProvince(mydata.VenueCode)

                             });

                // create the collection
                foreach (var a in Compo)
                {
                    LogisticsComposite FC = new LogisticsComposite();


                    FC.SessionId = a.SessionID;
                    FC.NBT = a.NBTNumber;
                    FC.Surname = a.Surname;
                    FC.Name = a.Name;
                    FC.MiddleInitials = a.Initial;
                    FC.SouthAfricanID = a.SouthAfricanID;
                    FC.Passport = a.Passport;
                    FC.Birth = a.Birth;
                    FC.WroteAL = a.W_AL;
                    FC.WroteQL = a.W_QL;
                    FC.WroteMat = a.W_Mat;
                    FC.StudentNo = a.StNo;
                    FC.Faculty = a.Faculty;
                    FC.Programme = a.Programme;
                    FC.TestDate = a.DateTest;
                    FC.Venue = a.Venue;
                    FC.Sex = a.Sex;
                    FC.ALScore = a.ALScore.ToString();
                    FC.ALLevel = a.ALLevel;
                    FC.QLScore = a.QLScore.ToString();
                    FC.QLLevel = a.QLLevel;
                    FC.MatScore = a.MatScore.ToString();
                    FC.MatLevel = a.MatLevel;
                    FC.AQLLanguage = a.AQL_Lang;
                    FC.MatLanguage = a.Mat_Lang;
                    FC.TestType = a.TestType;
                    FC.VenueProvince = a.province;
                    //FC.RefNo = a.RefNo;


                    LComposite.Add(FC);
                }
            }
        }

        // generate composite file from selected list
        public bool GenerateSelectedComposite(ObservableCollection<CompositBDO> mySelection, string folder)
        {
            bool generated = false;
            //Composite.Clear();
            Composite = mySelection;
            myDir = new Dir_categories(folder);
            //myDir.Dir = Path.GetDirectoryName(folder);
            generated = GenerateExcelComposite();
            return generated;
        }
        public int GetCompositCount(IntakeYearsBDO yr)
        {
            int Total = 0;
            using (var context = new CETAPEntities())
            {
                var AllRecs = context.Composits
                                     .Where(x => x.DOT >= yr.yearStart && x.DOT <= yr.yearEnd)
                                     .Count();
                Total = AllRecs;
            }
            return Total;
        }
        public async Task<List<CompositBDO>> GetAllNBTScoresAsync(int page, int size, IntakeYearsBDO yr)
        {
            var NBTScores = new List<CompositBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                // get all database	Scores
                using (var context = new CETAPEntities())
                    try
                    {
                        var scores = await context.Composits
                                                  .Where(x => x.DOT >= yr.yearStart && x.DOT <= yr.yearEnd)
                                                  .OrderBy(x => x.DOT)
                                                  .Skip(page * size)
                                                  .Take(size)
                                                  .ToListAsync();


                        //Convert each score to CompositBDO
                        foreach (Composit score in scores)
                        {
                            CompositBDO comp = new CompositBDO();
                            comp = Maps.CompositDALToCompositBDO(score);
                            //CompositDALToCompositBDO(comp, score);
                            //	testBDO.AllocatedTests = GetAllocatedTestsByTestID(testBDO.TestID);
                            NBTScores.Add(comp);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

            }
            AllScores = new ObservableCollection<CompositBDO>(NBTScores);
            return NBTScores;

        }

        private string getProvince(int venuecode)
        {
            VenueBDO venue = AllVenues.Where(x => x.VenueCode == venuecode).FirstOrDefault();


            ProvinceBDO prov = getProvinceByID(venue.ProvinceID);
            return prov.Name;
        }
        private string MytestType(string datfile)
        {
            datFileAttributes mydat = new datFileAttributes();
            mydat.SName = datfile;
            return mydat.Client;
        }
        private void GenerateCompFromDB(string folder)
        {
            var Compo = from mydata in AllScores
                        select new
                        {
                            RefNo = mydata.RefNo.ToString(),
                            Barcode = mydata.Barcode.ToString(),
                            LastName = mydata.Surname,
                            FName = mydata.Name,
                            Initials = mydata.Initials,
                            SAID = HelperUtils.ToSAID(mydata.SAID),
                            FID = mydata.ForeignID,
                            DOB = String.Format("{0:yyyyMMdd}", mydata.DOB),
                            IDType = mydata.ID_Type,
                            Citizenship = mydata.Citizenship,
                            Classification = mydata.Classification,
                            Gender = mydata.Gender,
                            faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
                            Testdate = String.Format("{0:yyyyMMdd}", mydata.DOT),
                            VenueCode = mydata.VenueCode.ToString("D5"),
                            VenueName = mydata.VenueName,
                            Hlanguage = mydata.HomeLanguage,
                            G12L = mydata.GR12Language,
                            AQLLang = mydata.AQLLanguage,
                            AQLCode = mydata.AQLCode,
                            MatLang = mydata.MatLanguage,
                            MatCode = mydata.MatCode,
                            Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
                            Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
                            SessionID = mydata.Barcode.ToString(),
                            NBTNumber = mydata.RefNo.ToString(),
                            Surname = mydata.Surname,
                            Name = mydata.Name,
                            Initial = mydata.Initials,
                            SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
                            Passport = mydata.ForeignID,
                            Birth = (mydata.SAID != null ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) :
                            String.Format("{0:dd/MM/yyyy}", mydata.DOB)),

                            W_AL = mydata.WroteAL,
                            W_QL = mydata.WroteQL,
                            W_Mat = mydata.WroteMat,
                            StNo = "",
                            Faculty = "",
                            Programme = "",
                            DateTest = String.Format("{0:yyyyMMdd}", mydata.DOT),
                            Venue = mydata.VenueName,
                            Sex = (mydata.Gender == "1" ? "M" :
                                   mydata.Gender == "2" ? "F" : mydata.Gender),
                            street1 = "",
                            street2 = "",
                            Suburb = "",
                            City = "",
                            Province = "",
                            Postal = "",
                            Email = "",
                            Landline = "",
                            Mobile = "",
                            ALScore = mydata.ALScore,
                            ALLevel = mydata.ALLevel,
                            QLScore = mydata.QLScore,
                            QLLevel = mydata.QLLevel,
                            MatScore = mydata.MATScore,
                            MatLevel = mydata.MATLevel,
                            AQL_Lang = mydata.AQLLanguage,
                            Mat_Lang = mydata.MatLanguage

                        };
            var workbook = new XLWorkbook();


            var ws = workbook.Worksheets.Add("Composite").SetTabColor(XLColor.Almond);
            var AllColumns = ws.Columns("A1:BF1");
            AllColumns.Width = 13;
            ws.Range("A1:X1").Style.Fill.BackgroundColor = XLColor.TealBlue;
            ws.Range("Y1:Y1").Style.Fill.BackgroundColor = XLColor.Orange;
            ws.Range("Z1:AI1").Style.Fill.BackgroundColor = XLColor.Yellow;
            ws.Range("AJ1:AL1").Style.Fill.BackgroundColor = XLColor.Magenta;
            ws.Range("AM1:AX1").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            ws.Range("AY1:BD1").Style.Fill.BackgroundColor = XLColor.LightGray;
            ws.Range("BE1:BF1").Style.Fill.BackgroundColor = XLColor.Yellow;
            var Row1 = ws.Row(1);

            Row1.Height = 30;
            Row1.Style.Font.Bold = true;
            Row1.Style.Font.FontSize = 9;
            Row1.Style.Alignment.WrapText = true;
            Row1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            Row1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell(1, 1).Value = "Ref No";
            ws.Cell(1, 2).Value = "Barcode";
            ws.Cell(1, 3).Value = "Last Name";
            ws.Cell(1, 4).Value = "First_Name";
            ws.Cell(1, 5).Value = "INITIALS";
            ws.Cell(1, 6).Value = "ID NUMBER";
            ws.Cell(1, 7).Value = "ID_Foreign";
            ws.Cell(1, 8).Value = "Date of Birth";
            ws.Cell(1, 9).Value = "ID Type";
            ws.Cell(1, 10).Value = "Citizenship";
            ws.Cell(1, 11).Value = "Classification";
            ws.Cell(1, 12).Value = "Gender 1";
            ws.Cell(1, 13).Value = "Faculty 1";
            ws.Cell(1, 14).Value = "DATE";
            ws.Cell(1, 15).Value = "Test Centre Code";
            ws.Cell(1, 16).Value = "Venue Name";
            ws.Cell(1, 17).Value = "Home Lang";
            ws.Cell(1, 18).Value = "GR12 Language";
            ws.Cell(1, 19).Value = "AQL LANG";
            ws.Cell(1, 20).Value = "AQL CODE";
            ws.Cell(1, 21).Value = "MAT LANG";
            ws.Cell(1, 22).Value = "MAT CODE";
            ws.Cell(1, 23).Value = "Faculty 2";
            ws.Cell(1, 24).Value = "Faculty 3";
            ws.Cell(1, 25).Value = "Test Session ID";
            ws.Cell(1, 26).Value = "NBT Reference";
            ws.Cell(1, 27).Value = "Surname";
            ws.Cell(1, 28).Value = "First Name";
            ws.Cell(1, 29).Value = "Middle Initials";
            ws.Cell(1, 30).Value = "South African ID";
            ws.Cell(1, 31).Value = "Foreign ID";
            ws.Cell(1, 32).Value = "Date of Birth";
            ws.Cell(1, 33).Value = "Wrote AL";
            ws.Cell(1, 34).Value = "Wrote QL";
            ws.Cell(1, 35).Value = "Wrote Maths";
            ws.Cell(1, 36).Value = "Student Number";
            ws.Cell(1, 37).Value = "Faculty";
            ws.Cell(1, 38).Value = "Programme";
            ws.Cell(1, 39).Value = "Date_of_Test";
            ws.Cell(1, 40).Value = "Venue";
            ws.Cell(1, 41).Value = "Gender";
            ws.Cell(1, 42).Value = "Street and Number";
            ws.Cell(1, 43).Value = "Street Name";
            ws.Cell(1, 44).Value = "Suburb";
            ws.Cell(1, 45).Value = "City/Town";
            ws.Cell(1, 46).Value = "Province/Region";
            ws.Cell(1, 47).Value = "Postal Code";
            ws.Cell(1, 48).Value = "e-mail Address";
            ws.Cell(1, 49).Value = "Landline Number";
            ws.Cell(1, 50).Value = "Mobile Number";
            ws.Cell(1, 51).Value = "AL Score";
            ws.Cell(1, 52).Value = "AL Performance";
            ws.Cell(1, 53).Value = "QL Score";
            ws.Cell(1, 54).Value = "QL Performance";
            ws.Cell(1, 55).Value = "Maths Score";
            ws.Cell(1, 56).Value = "Maths Performance";
            ws.Cell(1, 57).Value = "AQL TEST LANGUAGE";
            ws.Cell(1, 58).Value = "MATHS TEST LANGUAGE";


            ws.Cell(2, 1).Value = Compo.AsEnumerable();

            //ws.Columns().AdjustToContents();


            // Get the date today
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            string myDay = dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00") + "_" + dt.Hour.ToString("00") + dt.Minute.ToString("00");

            string ExcelFileName = folder + "_" + myDay + "n=" + Compo.Count() + ".xlsx";

            //  string Combination = Path.Combine(path1, ExcelFileName);

            workbook.SaveAs(ExcelFileName);
        }
        public CompositBDO getResultsByName(string name)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsBySurName(string surname)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByID(long SAID)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByFID(string FID)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByDOB(DateTime DOB)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByNBT(long NBT)
        {
            throw new NotImplementedException();
        }

        public bool updateComposit(CompositBDO results, ref string message)
        {
            bool ret = false;
            message = "Scores successfully updated";

            using (var context = new CETAPEntities())
            {
                var ID = results.Barcode;
                Composit writerDB = context.Composits.Where(x => x.Barcode == ID).FirstOrDefault();

                if (writerDB != null)
                {
                    ret = true;
                    context.Composits.Remove(writerDB);

                    CompositBDOToCompositDAL(results, writerDB);

                    //writerDB = Maps.CompositBDOToCompositDAL(results);

                    writerDB.DateModified = DateTime.Now;

                    context.Composits.Attach(writerDB);
                    context.Entry(writerDB).State = EntityState.Modified;
                    int num = context.SaveChanges();

                    results.RowVersion = writerDB.RowVersion;

                }
                else
                {
                    message = "No Writer with ID " + results.Barcode;
                }

            }
            return ret;
        }


        public bool deleteComposit(CompositBDO results, ref string message)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> addComposit(CompositBDO results)
        {
            bool ret = false;
            string message = results.RefNo.ToString() + " Not saved :";

            using (var context = new CETAPEntities())
            {
                try
                {
                    //Add TestVenue
                    var applicantInDB = new Composit();
                    applicantInDB.RefNo = results.RefNo;
                    applicantInDB.Surname = results.Surname;
                    applicantInDB.Name = results.Name;
                    applicantInDB.Initials = results.Initials;
                    applicantInDB.SAID = (results.SAID == null ? (long?)null : Convert.ToInt64(results.SAID));
                    applicantInDB.ForeignID = results.ForeignID;
                    applicantInDB.Gender = results.Gender;
                    applicantInDB.DOB = results.DOB;
                    applicantInDB.Classification = results.Classification;
                    applicantInDB.AQLLanguage = results.AQLLanguage;
                    applicantInDB.MatLanguage = results.MatLanguage;
                    applicantInDB.HomeLanguage = results.HomeLanguage.ToString();
                    applicantInDB.GR12Language = results.GR12Language;
                    applicantInDB.ID_Type = results.ID_Type;
                    applicantInDB.Barcode = results.Barcode;
                    applicantInDB.DOT = results.DOT;
                    applicantInDB.Citizenship = results.Citizenship;
                    applicantInDB.Faculty = results.Faculty;
                    applicantInDB.VenueCode = results.VenueCode;
                    applicantInDB.VenueName = results.VenueName;
                    applicantInDB.AQLCode = results.AQLCode;
                    applicantInDB.MatCode = results.MatCode;
                    applicantInDB.ALScore = results.ALScore;
                    applicantInDB.QLScore = results.QLScore;
                    applicantInDB.MATScore = results.MATScore;
                    applicantInDB.MATLevel = results.MATLevel;
                    applicantInDB.ALLevel = results.ALLevel;
                    applicantInDB.QLLevel = results.QLLevel;
                    applicantInDB.WroteAL = results.WroteAL;
                    applicantInDB.WroteQL = results.WroteQL;
                    applicantInDB.WroteMat = results.WroteMat;
                    applicantInDB.Batch = results.Batch;
                    applicantInDB.Faculty2 = results.Faculty2;
                    applicantInDB.Faculty3 = results.Faculty3;
                    //	writerInDB.NBT = writer.NBT;
                    applicantInDB.RowGuid = Guid.NewGuid();

                    applicantInDB.DateModified = DateTime.Now;


                    context.Composits.Add(applicantInDB);
                    await context.SaveChangesAsync();

                    ret = true;
                    //message = "Writer added succesfully";
                }
                catch (Exception ex)
                {
                    ret = false;
                    message += ex.InnerException.ToString();
                    ModernDialog.ShowMessage(message, "Record not saved", MessageBoxButton.OK);

                }
            }
            return ret;
        }

        public bool RemoveRecordsInQueue(List<long> ScoredRecords)
        {
            
            bool flag = false;
            using (CETAPEntities cetapEntities = new CETAPEntities())
            {
                if (cetapEntities.RecordsInQueues.Count<RecordsInQueue>() > 0)
                {
                    var recordsInQueues = cetapEntities.RecordsInQueues;

                    List<RecordsInQueue> list = recordsInQueues.Where(x => ScoredRecords.Any(m => m == x.Barcode)).ToList();
                                                               
                    cetapEntities.RecordsInQueues.RemoveRange((IEnumerable<RecordsInQueue>)list);
                    cetapEntities.SaveChanges();
                    flag = true;
                }
            }
            return flag;
        }


        public void GetNoMatchFile(string filename)
        {
            string DIR = Path.GetDirectoryName(filename);

            string ExcelFileName = "UCT_matched";
            ReadBI MyRead = new ReadBI(filename);
            List<BI> unmatched = new List<BI>();
            unmatched = MyRead.BiDetails;

            List<Composit> DB = new List<Composit>();
            using (var context = new CETAPEntities())
            {
                DB = context.Composits.ToList();
            }


            // query by NBT and ID
            var query =
                    (from comp in DB
                     join match in unmatched
                     on new { NBT = comp.RefNo, SAID = comp.SAID.ToString() } equals new { NBT = match.NBT, SAID = match.SAID }
                     select new
                     {
                         composit = comp,
                         Surname = match.Surname.ToUpper().Trim(),
                         Name = match.Name.ToUpper().Trim(),
                         match.DOB,
                         match.SAID,
                         match.NBT
                     }
                     ).ToList().Distinct();
            int m = query.Count();
            // query by name, surname and DOB
            var query1 =
                        (from comp in DB
                         join match in unmatched
                         on new { Name = comp.Name.ToUpper().Trim(), DOB = comp.DOB, Surname = comp.Surname.ToUpper().Trim() } equals new { Name = match.Name.ToUpper().Trim(), DOB = match.DOB, Surname = match.Surname.ToUpper().Trim() }
                         select new
                         {
                             composit = comp,
                             Surname = match.Surname.ToUpper().Trim(),
                             Name = match.Name.ToUpper().Trim(),
                             match.DOB,
                             match.SAID,
                             match.NBT
                         }
                         ).ToList().Distinct();

            int y = query1.Count();
            // query by surname, NBT, DOB
            var query2 =
                        (from comp in DB
                         join match in unmatched
                         on new { NBT = comp.RefNo, DOB = comp.DOB, Name = comp.Name.ToUpper().Trim() } equals new { NBT = match.NBT, DOB = match.DOB, Name = match.Name.ToUpper().Trim() }
                         select new
                         {
                             composit = comp,
                             Surname = match.Surname.ToUpper().Trim(),
                             Name = match.Name.ToUpper().Trim(),
                             match.DOB,
                             match.SAID,
                             match.NBT
                         }
                         ).ToList().Distinct();
            int x = query2.Count();

            // query by surname, NBT, DOB
            var query3 =
                        (from comp in DB
                         join match in unmatched
                         on new { NBT = comp.RefNo, DOB = comp.DOB, Surname = comp.Surname.ToUpper().Trim() } equals new { NBT = match.NBT, DOB = match.DOB, Surname = match.Surname.ToUpper().Trim() }
                         select new
                         {
                             composit = comp,
                             Surname = match.Surname.ToUpper().Trim(),
                             Name = match.Name.ToUpper().Trim(),
                             match.DOB,
                             match.SAID,
                             match.NBT
                         }
                         ).ToList().Distinct();
            int z = query3.Count();

            var queryUnion = query3.Union(query2.Union(query1.Union(query)));

            int ab = queryUnion.Count();
            DB.Clear(); // free up memory


            foreach (var a in queryUnion)
            {
                if (a.composit.Name != a.Name) a.composit.Name = a.Name;
                if (a.composit.Surname != a.Surname) a.composit.Surname = a.Surname;
                //if (a.composit.RefNo != a.NBT) a.composit.RefNo = a.NBT;
                //if ((a.composit.SAID.ToString() != a.SAID) && (a.composit.SAID.ToString().Length > 1) && (a.SAID.Length > 1)) a.composit.SAID = Convert.ToInt64(a.SAID);
            }

            // query by NBT and DOB

            //  var queryUnion = Enumerable.Union(query, query2, query1);

            if (queryUnion.Count() > 0)
            {
                var xq = from b in queryUnion select b.composit;
                var UCT = from C in xq
                          select new
                          {
                              NBT = C.RefNo.ToString(),
                              Surname = C.Surname,
                              FName = C.Name,
                              Initials = C.Initials,
                              SaID = HelperUtils.ToSAID(C.SAID),
                              Passport = C.ForeignID,
                              DOB = String.Format("{0:dd/MM/yyyy}", C.DOB),
                              DOT = String.Format("{0:yyyyMMdd}", C.DOT),
                              Venue = C.VenueCode,
                              ALScore = C.ALScore,
                              ALLevel = (C.ALLevel == "Proficient Upper" ? "Proficient" :
                                            C.ALLevel == "Proficient Lower" ? "Proficient" :
                                            C.ALLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.ALLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.ALLevel == "Basic Upper" ? "Basic" :
                                            C.ALLevel == "Basic Lower" ? "Basic" : ""),
                              QLScore = C.QLScore,
                              QLLevel = (C.QLLevel == "Proficient Upper" ? "Proficient" :
                                            C.QLLevel == "Proficient Lower" ? "Proficient" :
                                            C.QLLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.QLLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.QLLevel == "Basic Upper" ? "Basic" :
                                            C.QLLevel == "Basic Lower" ? "Basic" : ""),
                              MATScore = C.MATScore,
                              MATLevel = (C.MATLevel == "Proficient Upper" ? "Proficient" :
                                            C.MATLevel == "Proficient Lower" ? "Proficient" :
                                            C.MATLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.MATLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.MATLevel == "Basic Upper" ? "Basic" :
                                            C.MATLevel == "Basic Lower" ? "Basic" : "")
                          };

                var workbook = new XLWorkbook();



                var ws_UCT = workbook.Worksheets.Add("UCT Upload").SetTabColor(XLColor.DarkCoral);
                ws_UCT.Cell(1, 1).Value = "NBT_Ref_No";
                ws_UCT.Cell(1, 2).Value = "Surname";
                ws_UCT.Cell(1, 3).Value = "First_Name";
                ws_UCT.Cell(1, 4).Value = "Initials";
                ws_UCT.Cell(1, 5).Value = "South_African_ID_No";
                ws_UCT.Cell(1, 6).Value = "Foreign_Passport";
                ws_UCT.Cell(1, 7).Value = "Date_of_Birth";
                ws_UCT.Cell(1, 8).Value = "Date_of_Test";
                ws_UCT.Cell(1, 9).Value = "Venue";
                ws_UCT.Cell(1, 10).Value = "AL Score";
                ws_UCT.Cell(1, 11).Value = "AL Performance Level";
                ws_UCT.Cell(1, 12).Value = "QL Score";
                ws_UCT.Cell(1, 13).Value = "QL Performance Level";
                ws_UCT.Cell(1, 14).Value = "Maths Score";
                ws_UCT.Cell(1, 15).Value = "Maths Performance Level";
                ws_UCT.Cell(2, 1).Value = UCT.AsEnumerable();

                //string path1 = myDir.Dir;

                // Get the date today
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string myDay = dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00");
                myDay += "_" + dt.Hour.ToString("00") + dt.Minute.ToString("00");

                ExcelFileName += "_" + myDay + ".xlsx";

                string Combination = Path.Combine(DIR, ExcelFileName);

                workbook.SaveAs(Combination);
            }





        }
        #endregion

        #region Scoring

        public ObservableCollection<string> ListScoreFiles(string folder)
        {
            myDir = new Dir_categories(folder);
            //			string[] myfiles = Directory.GetFiles(folder);
            BIO = null;
            AQL = null;
            MAT = null;
            ObservableCollection<string> ListScoreFiles = new ObservableCollection<string>(myDir.FilesinFolder);
            return ListScoreFiles;
        }

        //Reading Answer sheet bio file
        public ObservableCollection<AnswerSheetBio> LoadAnswerSheet()
        {
            if (myDir != null)
            {

                // Load venues
                List<VenueBDO> venues = new List<VenueBDO>();
                List<ScoreStats> MATS = new List<ScoreStats>();
                venues = GetAllvenues();
                // read the excel file.
                ReadExcelBioFile x = new ReadExcelBioFile(myDir.AnswerSheetBio, venues);

                // Requires Statistics of Excel sheet here
                //---------------------------------------
                ScoreStats MAT = new ScoreStats();
                MAT.Records = x.BioDetails.Count();
                MAT.Mean = x.BioDetails.Count() / 2;
                MATS.Add(MAT);
                AnswerSheetBioStatistics = new ObservableCollection<ScoreStats>(MATS);

                //-----------------------------------------
                BIO = new ObservableCollection<AnswerSheetBio>(x.BioDetails);
                return BIO;
            }
            else return null;
        }

        //reading AQL scores
        public ObservableCollection<AQL_Score> LoadAQLScores()
        {
            if (myDir != null)
            {
                string atype = "AQL";
                List<AQL_Score> AQLs = new List<AQL_Score>();

                List<ScoreStats> AQLStats = new List<ScoreStats>();
                foreach (var a in myDir.AQLScorefiles)
                {
                    ReadExcel readfile = new ReadExcel(a, atype);
                    ScoreStats Alstat = new ScoreStats();
                    ScoreStats Qlstat = new ScoreStats();
                    // Requires Statistics of Excel sheet here
                    //---------------------------------------
                    var AQLread = readfile.AQLScores;
                    var AQLreads = AQLread.GroupBy(e => e.ID).Select(gp => gp.First()).ToList();
                    List<int> myAL = new List<int>();
                    List<int> myQL = new List<int>();
                    foreach (var x in AQLreads)
                    {
                        myAL.Add((int)x.AL);
                        myQL.Add((int)x.QL);
                    }
                    IOrderedEnumerable<int> orderedAL = myAL.OrderBy(i => i);
                    IOrderedEnumerable<int> orderedQL = myQL.OrderBy(i => i);

                    ////IEnumerable<int> ALs = new IEnumerable<int>(mm);
                    Alstat.Filename = Path.GetFileName(a);
                    Alstat.type = "AL";
                    if (AQLreads.Count > 2)
                    {
                        Alstat.FirstQuantile = HelperUtils.LowerQuartile(orderedAL);
                        Alstat.ThirdQuantile = HelperUtils.UpperQuartile(orderedAL);
                        Alstat.stdDev = orderedAL.StandardDeviation();
                    }
                    Alstat.Min = orderedAL.Min();
                    Alstat.Max = orderedAL.Max();
                    Alstat.Records = orderedAL.Count();
                    Alstat.Mean = orderedAL.Average();
                    Alstat.Median = orderedAL.Median();


                    //IEnumerable<int> ALs = new IEnumerable<int>(mm);
                    Qlstat.Filename = Path.GetFileName(a);
                    Qlstat.type = "QL";
                    if (AQLreads.Count > 2)
                    {
                        Qlstat.FirstQuantile = HelperUtils.LowerQuartile(orderedQL);
                        Qlstat.ThirdQuantile = HelperUtils.UpperQuartile(orderedQL);
                        Qlstat.stdDev = orderedQL.StandardDeviation();
                    }
                    Qlstat.Min = orderedQL.Min();
                    Qlstat.Max = orderedQL.Max();
                    Qlstat.Records = orderedQL.Count();
                    Qlstat.Mean = orderedQL.Average();
                    Qlstat.Median = orderedQL.Median();

                    //-----------------------------------------
                    AQLs.AddRange(AQLreads);
                    AQLStats.Add(Alstat);
                    AQLStats.Add(Qlstat);

                }
                // AQLStats = AQLStats.GroupBy(x => x.Filename).ToList();
                //List<ScoreStats> mydata = AQLStats.GroupBy(x => x.Filename).ToList();
                AQLStatistics = new ObservableCollection<ScoreStats>(AQLStats);
                AQL = new ObservableCollection<AQL_Score>(AQLs);
                return AQL;
            }
            else return null;
        }

        public ObservableCollection<ScoreStats> GetAQLStats()
        {
            return AQLStatistics;
        }

        public ObservableCollection<ScoreStats> GetMATStats()
        {
            return MatStatistics;
        }
        //  generate composite
        public ObservableCollection<CompositBDO> MatchScores()
        {
             BenchMarkLevelsBDO degbenchmark = new BenchMarkLevelsBDO();
            GetIntakeBenchmarks();

            degbenchmark = benchmarkLevels.Where(rec => rec.Type == "Degree    ").Select(x => x).FirstOrDefault();

            Composite = new ObservableCollection<CompositBDO>();
            var matched = (from b in BIO
                           join a in AQL on b.Barcode equals a.ID into ab_j
                           join m in MAT on b.Barcode equals m.ID into bm_j
                           from ab in ab_j.DefaultIfEmpty(new AQL_Score())
                           from bm in bm_j.DefaultIfEmpty(new MAT_Score())
                           select new
                           {
                               Barcode = b.Barcode,
                               NBT = b.NBT,
                               Surname = b.Surname,
                               Name = b.Name,
                               Initials = b.Initials,
                               SAID = b.SAID,
                               ForeignID = b.ForeignID,
                               ID_Type = b.ID_Type,
                               //DOB = b.DOB != null ? b.DOB: HelperUtils.BioDate("19000101"),
                               DOB = b.DOB,
                               Gender = b.Gender,
                               Classification = b.Classification,
                               Citizenship = b.Citizenship,
                               GR12Language = b.Grade12_Language,
                               HomeLanguage = b.HomeLanguage,

                               DOT = b.DOT,
                               VenueCode = b.VenueCode,
                               VenueName = b.VenueName,

                               MATCode = b.MatCode,
                               MAT_Language = b.Mat_Language,
                               AQLCode = b.AQLCode,
                               AQL_Language = b.AQL_Language,

                               Faculty1 = b.Faculty1,
                               Faculty2 = b.Faculty2,
                               faculty3 = b.faculty3,
                               AL = ab.AL == 0 ? (int?)null : ab.AL,
                               wroteAL = ab.AL == null ? "NO" :
                                          ab.AL == 0 ? "NO" : "YES",
                               AL_Level = ab.AL >= degbenchmark.AL_PU ? "Proficient Upper" :
                                          ab.AL >= degbenchmark.AL_PL ? "Proficient Lower" :
                                          ab.AL >= degbenchmark.AL_IU ? "Intermediate Upper" :
                                          ab.AL >= degbenchmark.AL_IL ? "Intermediate Lower" :
                                          ab.AL >= degbenchmark.AL_BU ? "Basic Upper" :
                                          ab.AL > degbenchmark.AL_BL ? "Basic Lower" : "",

                               QL = ab.QL == 0 ? (int?)null : ab.QL,
                               wroteQL = ab.QL == null ? "NO" :
                                          ab.QL == 0 ? "NO" : "YES",
                               QL_Level = ab.QL >= degbenchmark.QL_PU ? "Proficient Upper" :
                                          ab.QL >= degbenchmark.QL_PL ? "Proficient Lower" :
                                          ab.QL >= degbenchmark.QL_IU ? "Intermediate Upper" :
                                          ab.QL >= degbenchmark.QL_IL ? "Intermediate Lower" :
                                          ab.QL >= degbenchmark.QL_BU ? "Basic Upper" :
                                          ab.QL > degbenchmark.QL_BL ? "Basic Lower" : "",
                               MAT = bm.MAT != null ? (int?)bm.MAT : null,
                               //MAT = bm.MAT,
                               wroteMat = bm.MAT != null ? "YES" : "NO",
                               mat_level = bm.MAT >= degbenchmark.MAT_PU ? "Proficient Upper" :
                                          bm.MAT >= degbenchmark.MAT_PL ? "Proficient Lower" :
                                          bm.MAT >= degbenchmark.MAT_IU ? "Intermediate Upper" :
                                          bm.MAT >= degbenchmark.MAT_IL ? "Intermediate Lower" :
                                          bm.MAT >= degbenchmark.MAT_BU ? "Basic Upper" :
                                          bm.MAT > degbenchmark.MAT_BL ? "Basic Lower" : "",
                               Batch = b.BatchFile
                           }).Distinct().ToList();

            //Parallel.ForEach( matched, x =>
            //    {
            //        CompositBDO record = new CompositBDO();
            //        record.Barcode = x.Barcode;
            //        record.RefNo = x.NBT;
            //        record.Surname = x.Surname;
            //        record.Name = x.Name;
            //        record.Initials = x.Initials;
            //        record.SAID = (x.SAID == null || x.SAID == "") ? (long?)null : Convert.ToInt64(x.SAID);
            //        record.ForeignID = x.ForeignID;
            //        record.ID_Type = x.ID_Type.ToString();
            //        record.Gender = x.Gender.ToString();
            //        record.DOB = x.DOB;

            //        record.HomeLanguage = x.HomeLanguage;
            //        record.GR12Language = x.GR12Language.ToString();
            //        record.Citizenship = x.Citizenship;
            //        record.Classification = x.Classification.ToString();

            //        record.ALScore = x.AL;
            //        record.QLScore = x.QL;
            //        record.MATScore = x.MAT;
            //        record.ALLevel = x.AL_Level;
            //        record.QLLevel = x.QL_Level;
            //        record.MATLevel = x.mat_level;
            //        record.WroteAL = x.wroteAL;
            //        record.WroteQL = x.wroteQL;
            //        record.WroteMat = x.wroteMat;
            //        record.DOT = x.DOT;
            //        record.VenueCode = x.VenueCode;
            //        record.VenueName = x.VenueName;
            //        record.AQLCode = x.AQLCode;
            //        record.MatCode = x.MATCode;
            //        record.AQLLanguage = x.AQL_Language;
            //        record.MatLanguage = x.MAT_Language;
            //        record.Faculty = x.Faculty1;
            //        record.Faculty2 = x.Faculty2;
            //        record.Faculty3 = x.faculty3;
            //        record.Batch = x.Batch;
            //        record.RowGuid = Guid.NewGuid();
            //        record.DateModified = DateTime.Now;

            //        Composite.Add(record);
            //    }
            //    );

            foreach (var x in matched)
            {
                CompositBDO record = new CompositBDO();
                record.Barcode = x.Barcode;
                record.RefNo = x.NBT;
                record.Surname = x.Surname;
                record.Name = x.Name;
                record.Initials = x.Initials;
                record.SAID = (x.SAID == null || x.SAID == "") ? (long?)null : Convert.ToInt64(x.SAID);
                record.ForeignID = x.ForeignID;
                record.ID_Type = x.ID_Type.ToString();
                record.Gender = x.Gender.ToString();
                record.DOB = x.DOB;

                record.HomeLanguage = x.HomeLanguage;
                record.GR12Language = x.GR12Language.ToString();
                record.Citizenship = x.Citizenship;
                record.Classification = x.Classification.ToString();

                record.ALScore = x.AL;
                record.QLScore = x.QL;
                record.MATScore = x.MAT;
                record.ALLevel = x.AL_Level;
                record.QLLevel = x.QL_Level;
                record.MATLevel = x.mat_level;
                record.WroteAL = x.wroteAL;
                record.WroteQL = x.wroteQL;
                record.WroteMat = x.wroteMat;
                record.DOT = x.DOT;
                record.VenueCode = x.VenueCode;
                record.VenueName = x.VenueName;
                record.AQLCode = x.AQLCode;
                record.MatCode = x.MATCode;
                record.AQLLanguage = x.AQL_Language;
                record.MatLanguage = x.MAT_Language;
                record.Faculty = x.Faculty1;
                record.Faculty2 = x.Faculty2;
                record.Faculty3 = x.faculty3;
                record.Batch = x.Batch;
                record.RowGuid = Guid.NewGuid();
                record.DateModified = DateTime.Now;

                Composite.Add(record);
                //if (Composite.Count() % 315 == 0)
                //{
                //    MessageBox.Show("Hi there!!");
                //}
            }
            return Composite;
        }

        public bool GenerateComposite()

        {
          //  string resultpath = Path.Combine(ApplicationSettings.Default.ScoreFolder, "Composit.csv") ;
            string filepath = Path.Combine(myDir.Dir, "Composit.csv");
            bool gen = false;

            using (var streamWriter = new StreamWriter(filepath))
            {
                using (var writer = new CsvWriter(streamWriter))
                {
                    writer.Configuration.HasHeaderRecord = true;
                    IEnumerable<CompositBDO> records = Composite.ToList();
                    writer.WriteRecords(records);
                    
                }
            }
         //   File.Copy(filepath, resultpath);
            gen = GenerateExcelComposite();
            return gen;
        }
        private bool GenerateExcelComposite()
        {
            bool done = false;
            string ExcelFileName = "NBT_Composite";
            if (Composite.Count > 0)
            {
                var Compo = from mydata in Composite
                            select new
                            {
                                RefNo = mydata.RefNo.ToString(),
                                Barcode = mydata.Barcode.ToString(),
                                LastName = mydata.Surname,
                                FName = mydata.Name,
                                Initials = mydata.Initials,
                                SAID = HelperUtils.ToSAID(mydata.SAID),
                                FID = mydata.ForeignID,
                                DOB = String.Format("{0:yyyyMMdd}", mydata.DOB),
                                IDType = mydata.ID_Type,
                                Citizenship = mydata.Citizenship,
                                Classification = mydata.Classification,
                                Gender = mydata.Gender,
                                faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
                                Testdate = String.Format("{0:yyyyMMdd}", mydata.DOT),
                                VenueCode = mydata.VenueCode.ToString("D5"),
                                VenueName = mydata.VenueName,
                                Hlanguage = mydata.HomeLanguage,
                                G12L = mydata.GR12Language,
                                AQLLang = mydata.AQLLanguage,
                                AQLCode = mydata.AQLCode,
                                MatLang = mydata.MatLanguage,
                                MatCode = mydata.MatCode,
                                Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
                                Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
                                SessionID = mydata.Barcode.ToString(),
                                NBTNumber = mydata.RefNo.ToString(),
                                Surname = mydata.Surname,
                                Name = mydata.Name,
                                Initial = mydata.Initials,
                                SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
                                Passport = mydata.ForeignID,
                                Birth = string.Format("{0:dd/MM/yyyy}", mydata.DOB),

                                W_AL = mydata.WroteAL,
                                W_QL = mydata.WroteQL,
                                W_Mat = mydata.WroteMat,
                                StNo = "",
                                Faculty = "",
                                Programme = "",
                                DateTest = String.Format("{0:yyyyMMdd}", mydata.DOT),
                                Venue = mydata.VenueName,
                                Sex = (mydata.Gender == "1" ? "M" :
                                       mydata.Gender == "2" ? "F" : mydata.Gender),
                                street1 = "",
                                street2 = "",
                                Suburb = "",
                                City = "",
                                Province = "",
                                Postal = "",
                                Email = "",
                                Landline = "",
                                Mobile = "",
                                ALScore = mydata.ALScore,
                                ALLevel = mydata.ALLevel,
                                QLScore = mydata.QLScore,
                                QLLevel = mydata.QLLevel,
                                MatScore = mydata.MATScore,
                                MatLevel = mydata.MATLevel,
                                AQL_Lang = mydata.AQLLanguage,
                                Mat_Lang = mydata.MatLanguage

                            };


                var UP = from mydata in Compo
                         select new
                         {
                             NBT = mydata.NBTNumber,
                             Surname = mydata.Surname,
                             FName = mydata.FName,
                             Initials = mydata.Initials,
                             SaID = mydata.SAID,
                             Passport = mydata.Passport,
                             DOB = mydata.Birth,
                             DOT = mydata.DateTest,
                             Venue = mydata.VenueCode,
                             ALScore = mydata.ALScore,
                             ALLevel = mydata.ALLevel,
                             QLScore = mydata.QLScore,
                             QLLevel = mydata.QLLevel,
                             MATScore = mydata.MatScore,
                             MATLevel = mydata.MatLevel

                         };

                var UCT = from C in Compo
                          select new
                          {
                              NBT_Ref_No = C.NBTNumber.ToString(),
                              Surname = C.Surname,
                              First_Name = C.FName,
                              Initials = C.Initials,
                              South_African_ID_No = C.SAID,
                              Foreign_Passport = C.Passport,
                              Date_of_Birth = C.Birth,
                              Date_of_Test = C.DateTest,
                              Venue = C.VenueCode,
                              ALScore = C.ALScore,
                              ALPerformanceLevel = (C.ALLevel == "Proficient Upper" ? "Proficient" :
                                            C.ALLevel == "Proficient Lower" ? "Proficient" :
                                            C.ALLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.ALLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.ALLevel == "Basic Upper" ? "Basic" :
                                            C.ALLevel == "Basic Lower" ? "Basic" : ""),
                              QLScore = C.QLScore,
                              QLPerformanceLevel = (C.QLLevel == "Proficient Upper" ? "Proficient" :
                                            C.QLLevel == "Proficient Lower" ? "Proficient" :
                                            C.QLLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.QLLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.QLLevel == "Basic Upper" ? "Basic" :
                                            C.QLLevel == "Basic Lower" ? "Basic" : ""),
                              MathsScore = C.MatScore,
                              MathsPerformanceLevel = (C.MatLevel == "Proficient Upper" ? "Proficient" :
                                            C.MatLevel == "Proficient Lower" ? "Proficient" :
                                            C.MatLevel == "Intermediate Upper" ? "Intermediate" :
                                            C.MatLevel == "Intermediate Lower" ? "Intermediate" :
                                            C.MatLevel == "Basic Upper" ? "Basic" :
                                            C.MatLevel == "Basic Lower" ? "Basic" : ""),

                              AQLLanguage = C.AQL_Lang,
                              MathLanguage = C.Mat_Lang
                          };
                List<UCT_Upload> UCTLoads = new List<UCT_Upload>();
                foreach (var rec in UCT)
                {
                    UCT_Upload UctRec = new UCT_Upload();
                    UctRec.ALLevel = rec.ALPerformanceLevel;
                    UctRec.ALScore = rec.ALScore;
                    UctRec.DOB = rec.Date_of_Birth;
                    UctRec.DOT = rec.Date_of_Test;
                    UctRec.ForeignID = rec.Foreign_Passport;
                    UctRec.Initials = rec.Initials;
                    UctRec.MATLevel = rec.MathsPerformanceLevel;
                    UctRec.MATScore = rec.MathsScore;
                    UctRec.Name = rec.First_Name;
                    UctRec.NBT = rec.NBT_Ref_No;
                    UctRec.QLLevel = rec.QLPerformanceLevel;
                    UctRec.QLScore = rec.QLScore;
                    UctRec.SAID = rec.South_African_ID_No;
                    UctRec.Surname = rec.Surname;
                    UctRec.Venue = rec.Venue;
                    UctRec.AQLLang = rec.AQLLanguage;
                    UctRec.MATLANG = rec.MathLanguage;



                    UCTLoads.Add(UctRec);
                }

                var NBTWeb = from Adata in Composite

                             select new
                             {
                                 Barcode = Adata.Barcode.ToString(),
                                 TestSessionID = Adata.Barcode.ToString(),
                                 NBT = Adata.RefNo.ToString(),
                                 Surname = Adata.Surname,
                                 Name = Adata.Name,
                                 Initial = Adata.Initials,
                                 SAID = Adata.SAID.ToString(),
                                 Passport = Adata.ForeignID,
                                 DOB = String.Format("{0:dd/MM/yyyy}", Adata.DOB),
                                 W_AL = Adata.WroteAL,
                                 W_QL = Adata.WroteQL,
                                 W_Maths = Adata.WroteMat,
                                 StudentNumber = "",
                                 Faculty = "",
                                 Programme = "",
                                 Date_of_Test = String.Format("{0:yyyyMMdd}", Adata.DOT),
                                 Venue = GetWebSiteNameByVenueCode(Adata.VenueCode),
                                 Gender = Adata.Gender,
                                 Street1 = "",
                                 Street2 = "",
                                 Suburb = "",
                                 City = "",
                                 Province = "",
                                 Postal = "",
                                 Email = "",
                                 Landline = "",
                                 Mobile = "",
                                 ALScore = (Adata.ALScore == null ? 0 : Adata.ALScore),
                                 ALLevel = Adata.ALLevel,
                                 QLScore = (Adata.QLScore == null ? 0 : Adata.QLScore),
                                 QLLevel = Adata.QLLevel,
                                 MatScore = (Adata.MATScore == null ? 0 : Adata.MATScore),
                                 MatLevel = Adata.MATLevel,
                                 AQL_Lang = Adata.AQLLanguage,
                                 Mat_Lang = Adata.MatLanguage
                             };
                List<NBTWebUpload> WebUploads = new List<NBTWebUpload>();
                foreach (var web in NBTWeb)
                {
                    NBTWebUpload WebRec = new NBTWebUpload();
                    WebRec.ALLevel = web.ALLevel;
                    WebRec.ALScore = web.ALScore;
                    WebRec.AQLLang = web.AQL_Lang;
                    WebRec.Barcode = web.Barcode;
                    WebRec.Celephone = web.Mobile;
                    WebRec.City = web.City;
                    WebRec.DOB = web.DOB;
                    WebRec.DOT = web.Date_of_Test;
                    WebRec.EMail = web.Email;
                    WebRec.Faculty = web.Faculty;
                    WebRec.ForeignID = web.Passport;
                    WebRec.Gender = web.Gender;
                    WebRec.Initials = web.Initial;
                    WebRec.MATLANG = web.Mat_Lang;
                    WebRec.MATLevel = web.MatLevel;
                    WebRec.MATScore = web.MatScore;
                    WebRec.Name = web.Name;
                    WebRec.NBT = web.NBT;
                    WebRec.PostCode = web.Postal;
                    WebRec.Programme = web.Programme;
                    WebRec.Province = web.Province;
                    WebRec.QLLevel = web.QLLevel;
                    WebRec.QLScore = web.QLScore;
                    WebRec.SAID = web.SAID;
                    WebRec.SessionID = web.TestSessionID;
                    WebRec.StreetName = web.Street2;
                    WebRec.StreetNo = web.Street1;
                    WebRec.StudentID = web.StudentNumber;
                    WebRec.Suburb = web.Suburb;
                    WebRec.Surname = web.Surname;
                    WebRec.Telephone = web.Mobile;
                    WebRec.Venue = web.Venue;
                    WebRec.Wrote_AL = web.W_AL;
                    WebRec.Wrote_Mat = web.W_Maths;
                    WebRec.Wrote_QL = web.W_QL;

                    WebUploads.Add(WebRec);
                }
                // writing to excel files

                // Get the date today
                DateTime dt = new DateTime();
                string n = " n=" + Compo.Count().ToString() + ".xlsx";

                dt = DateTime.Now;
                string myDay = dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00") + "_" + dt.Hour.ToString("00") + dt.Minute.ToString("00");

                #region ForExcel
                var workbook = new XLWorkbook();


                var ws = workbook.Worksheets.Add("Composite").SetTabColor(XLColor.Almond);
                var AllColumns = ws.Columns("A1:BF1");
                AllColumns.Width = 13;
                ws.Range("A1:X1").Style.Fill.BackgroundColor = XLColor.TealBlue;
                ws.Range("Y1:Y1").Style.Fill.BackgroundColor = XLColor.Orange;
                ws.Range("Z1:AI1").Style.Fill.BackgroundColor = XLColor.Yellow;
                ws.Range("AJ1:AL1").Style.Fill.BackgroundColor = XLColor.Magenta;
                ws.Range("AM1:AX1").Style.Fill.BackgroundColor = XLColor.GreenYellow;
                ws.Range("AY1:BD1").Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Range("BE1:BF1").Style.Fill.BackgroundColor = XLColor.Yellow;
                var Row1 = ws.Row(1);

                Row1.Height = 30;
                Row1.Style.Font.Bold = true;
                Row1.Style.Font.FontSize = 9;
                Row1.Style.Alignment.WrapText = true;
                Row1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                Row1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(1, 1).Value = "Ref No";
                ws.Cell(1, 2).Value = "Barcode";
                ws.Cell(1, 3).Value = "Last Name";
                ws.Cell(1, 4).Value = "First_Name";
                ws.Cell(1, 5).Value = "INITIALS";
                ws.Cell(1, 6).Value = "ID NUMBER";
                ws.Cell(1, 7).Value = "ID_Foreign";
                ws.Cell(1, 8).Value = "Date of Birth";
                ws.Cell(1, 9).Value = "ID Type";
                ws.Cell(1, 10).Value = "Citizenship";
                ws.Cell(1, 11).Value = "Classification";
                ws.Cell(1, 12).Value = "Gender 1";
                ws.Cell(1, 13).Value = "Faculty 1";
                ws.Cell(1, 14).Value = "DATE";
                ws.Cell(1, 15).Value = "Test Centre Code";
                ws.Cell(1, 16).Value = "Venue Name";
                ws.Cell(1, 17).Value = "Home Lang";
                ws.Cell(1, 18).Value = "GR12 Language";
                ws.Cell(1, 19).Value = "AQL LANG";
                ws.Cell(1, 20).Value = "AQL CODE";
                ws.Cell(1, 21).Value = "MAT LANG";
                ws.Cell(1, 22).Value = "MAT CODE";
                ws.Cell(1, 23).Value = "Faculty 2";
                ws.Cell(1, 24).Value = "Faculty 3";
                ws.Cell(1, 25).Value = "Test Session ID";
                ws.Cell(1, 26).Value = "NBT Reference";
                ws.Cell(1, 27).Value = "Surname";
                ws.Cell(1, 28).Value = "First Name";
                ws.Cell(1, 29).Value = "Middle Initials";
                ws.Cell(1, 30).Value = "South African ID";
                ws.Cell(1, 31).Value = "Foreign ID";
                ws.Cell(1, 32).Value = "Date of Birth";
                ws.Cell(1, 33).Value = "Wrote AL";
                ws.Cell(1, 34).Value = "Wrote QL";
                ws.Cell(1, 35).Value = "Wrote Maths";
                ws.Cell(1, 36).Value = "Student Number";
                ws.Cell(1, 37).Value = "Faculty";
                ws.Cell(1, 38).Value = "Programme";
                ws.Cell(1, 39).Value = "Date_of_Test";
                ws.Cell(1, 40).Value = "Venue";
                ws.Cell(1, 41).Value = "Gender";
                ws.Cell(1, 42).Value = "Street and Number";
                ws.Cell(1, 43).Value = "Street Name";
                ws.Cell(1, 44).Value = "Suburb";
                ws.Cell(1, 45).Value = "City/Town";
                ws.Cell(1, 46).Value = "Province/Region";
                ws.Cell(1, 47).Value = "Postal Code";
                ws.Cell(1, 48).Value = "e-mail Address";
                ws.Cell(1, 49).Value = "Landline Number";
                ws.Cell(1, 50).Value = "Mobile Number";
                ws.Cell(1, 51).Value = "AL Score";
                ws.Cell(1, 52).Value = "AL Performance";
                ws.Cell(1, 53).Value = "QL Score";
                ws.Cell(1, 54).Value = "QL Performance";
                ws.Cell(1, 55).Value = "Maths Score";
                ws.Cell(1, 56).Value = "Maths Performance";
                ws.Cell(1, 57).Value = "AQL TEST LANGUAGE";
                ws.Cell(1, 58).Value = "MATHS TEST LANGUAGE";


                ws.Cell(2, 1).Value = Compo.AsEnumerable();

                // write the UP excel file
                var wb = new XLWorkbook();
                var ws1 = wb.Worksheets.Add("Sheet 1");
                var ws_UP = wb.Worksheets.Add("Sheet2").SetTabColor(XLColor.BrightTurquoise);
                ws_UP.Cell(1, 1).Value = "NBT_Ref_No";
                ws_UP.Cell(1, 2).Value = "Surname";
                ws_UP.Cell(1, 3).Value = "First_Name";
                ws_UP.Cell(1, 4).Value = "Initials";
                ws_UP.Cell(1, 5).Value = "South_African_ID_No";
                ws_UP.Cell(1, 6).Value = "Foreign_Passport";
                ws_UP.Cell(1, 7).Value = "Date_of_Birth";
                ws_UP.Cell(1, 8).Value = "Date_of_Test";
                ws_UP.Cell(1, 9).Value = "Venue";
                ws_UP.Cell(1, 10).Value = "AL Score";
                ws_UP.Cell(1, 11).Value = "AL Performance Level";
                ws_UP.Cell(1, 12).Value = "QL Score";
                ws_UP.Cell(1, 13).Value = "QL Performance Level";
                ws_UP.Cell(1, 14).Value = "Maths Score";
                ws_UP.Cell(1, 15).Value = "Maths Performance Level";
                ws_UP.Cell(2, 1).Value = UP.AsEnumerable();




                //var ws_UCT = workbook.Worksheets.Add("UCT Upload").SetTabColor(XLColor.DarkCoral);
                //ws_UCT.Cell(1, 1).Value = "NBT_Ref_No";
                //ws_UCT.Cell(1, 2).Value = "Surname";
                //ws_UCT.Cell(1, 3).Value = "First_Name";
                //ws_UCT.Cell(1, 4).Value = "Initials";
                //ws_UCT.Cell(1, 5).Value = "South_African_ID_No";
                //ws_UCT.Cell(1, 6).Value = "Foreign_Passport";
                //ws_UCT.Cell(1, 7).Value = "Date_of_Birth";
                //ws_UCT.Cell(1, 8).Value = "Date_of_Test";
                //ws_UCT.Cell(1, 9).Value = "Venue";
                //ws_UCT.Cell(1, 10).Value = "AL Score";
                //ws_UCT.Cell(1, 11).Value = "AL Performance Level";
                //ws_UCT.Cell(1, 12).Value = "QL Score";
                //ws_UCT.Cell(1, 13).Value = "QL Performance Level";
                //ws_UCT.Cell(1, 14).Value = "Maths Score";
                //ws_UCT.Cell(1, 15).Value = "Maths Performance Level";
                //ws_UCT.Cell(2, 1).Value = UCT.AsEnumerable();

                //var ws_NBT = workbook.Worksheets.Add("NBT Website Upload").SetTabColor(XLColor.MintGreen);
                //ws_NBT.Cell(1, 1).Value = "Barcode";
                //ws_NBT.Cell(1, 2).Value = "Test Session ID";
                //ws_NBT.Cell(1, 3).Value = "NBT Reference";
                //ws_NBT.Cell(1, 4).Value = "Surname";
                //ws_NBT.Cell(1, 5).Value = "First Name";
                //ws_NBT.Cell(1, 6).Value = "Middle Initials";
                //ws_NBT.Cell(1, 7).Value = "South African ID";
                //ws_NBT.Cell(1, 8).Value = "Foreign ID";
                //ws_NBT.Cell(1, 9).Value = "Date of Birth";
                //ws_NBT.Cell(1, 10).Value = "Wrote AL";
                //ws_NBT.Cell(1, 11).Value = "Wrote QL";
                //ws_NBT.Cell(1, 12).Value = "Wrote Maths";
                //ws_NBT.Cell(1, 13).Value = "Student Number";
                //ws_NBT.Cell(1, 14).Value = "Faculty";
                //ws_NBT.Cell(1, 15).Value = "Programme";
                //ws_NBT.Cell(1, 16).Value = "Date_of_Test";
                //ws_NBT.Cell(1, 17).Value = "Venue";
                //ws_NBT.Cell(1, 18).Value = "Gender";
                //ws_NBT.Cell(1, 19).Value = "Street and Number";
                //ws_NBT.Cell(1, 20).Value = "Street Name";
                //ws_NBT.Cell(1, 21).Value = "Suburb";
                //ws_NBT.Cell(1, 22).Value = "City/Town";
                //ws_NBT.Cell(1, 23).Value = "Province/Region";
                //ws_NBT.Cell(1, 24).Value = "Postal Code";
                //ws_NBT.Cell(1, 25).Value = "e-mail Address";
                //ws_NBT.Cell(1, 26).Value = "Landline Number";
                //ws_NBT.Cell(1, 27).Value = "Mobile Number";
                //ws_NBT.Cell(1, 28).Value = "AL Score";
                //ws_NBT.Cell(1, 29).Value = "AL Performance";
                //ws_NBT.Cell(1, 30).Value = "QL Score";
                //ws_NBT.Cell(1, 31).Value = "QL Performance";
                //ws_NBT.Cell(1, 32).Value = "Maths Score";
                //ws_NBT.Cell(1, 33).Value = "Maths Performance";
                //ws_NBT.Cell(1, 34).Value = "AQL TEST LANGUAGE";
                //ws_NBT.Cell(1, 35).Value = "MATHS TEST LANGUAGE";


                //ws_NBT.Cell(2, 1).Value = NBT_Web.AsEnumerable();
                //ws_NBT.Columns().AdjustToContents();

                #endregion
                string path1 = myDir.Dir;

                ExcelFileName += "_" + myDay + n;
                string Combination = Path.Combine(path1, ExcelFileName);
                workbook.SaveAs(Combination);

                string UpFileName = "UP_Upload ";
                UpFileName += myDay + n;
                string Combination1 = Path.Combine(path1, UpFileName);
                wb.SaveAs(Combination1);

                //   writing to Csv files for UCT and website
                CsvFileDescription outputDesc = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true

                };
                string n1 = " n=" + Compo.Count().ToString() + ".csv";

                CsvContext cc = new CsvContext();
                string UCTFilename = "UCT_Upload " + myDay + n1;
                string Combination2 = Path.Combine(path1, UCTFilename);
                cc.Write(UCTLoads, Combination2, outputDesc);

                //	CsvContext ccw = new CsvContext();
                string WebFilename = "WEB_Upload " + myDay + n1;
                string Combination3 = Path.Combine(path1, WebFilename);
                cc.Write(WebUploads, Combination3, outputDesc);

                done = true;

            }

            return done;
        }
        //reading Mats scores
        public ObservableCollection<MAT_Score> LoadMATScores()
        {
            if (myDir != null && myDir.MATScoresfiles != null)
            {

                string atype = "MAT";
                List<MAT_Score> mats = new List<MAT_Score>();

                List<ScoreStats> matstats = new List<ScoreStats>();
                foreach (var a in myDir.MATScoresfiles)
                {
                    ReadExcel readfile = new ReadExcel(a, atype);
                    var Matreads = readfile.MATScores;
                    var Matread1 = Matreads.GroupBy(e => e.ID).Select(gr => gr.First()).ToList();
                    ScoreStats MATstat = new ScoreStats();
                    List<int> myMat = new List<int>();
                    foreach (var x in Matread1)
                    {
                        myMat.Add((int)x.MAT);

                    }
                    IOrderedEnumerable<int> orderedMAT = myMat.OrderBy(i => i);
                    // Requires Statistics of Excel sheet here
                    //---------------------------------------

                    //IEnumerable<int> ALs = new IEnumerable<int>(mm);
                    if (Matread1.Count() > 2)
                    {
                        MATstat.FirstQuantile = HelperUtils.LowerQuartile(orderedMAT);
                        MATstat.ThirdQuantile = HelperUtils.UpperQuartile(orderedMAT);
                        MATstat.stdDev = orderedMAT.StandardDeviation();
                    }
                    MATstat.Min = orderedMAT.Min();
                    MATstat.Max = orderedMAT.Max();
                    MATstat.Records = orderedMAT.Count();
                    MATstat.Mean = orderedMAT.Average();
                    MATstat.Median = orderedMAT.Median();

                    MATstat.Filename = Path.GetFileName(a);
                    MATstat.type = "MAT";
                    //-----------------------------------------
                    mats.AddRange(Matread1);
                    matstats.Add(MATstat);
                }
                MAT = new ObservableCollection<MAT_Score>(mats);
                MatStatistics = new ObservableCollection<ScoreStats>(matstats);
                return MAT;
            }
            else
                return null;
        }
        public ObservableCollection<AnswerSheetBio> GetBIO()
        {
            LoadAnswerSheet();
            return BIO;
        }
        public ObservableCollection<AQL_Score> GetAQL()
        {
            LoadAQLScores();
            return AQL;
        }
        public ObservableCollection<MAT_Score> GetMat()
        {
            LoadMATScores();
            return MAT;
        }

        // write score record numbers to Scantracker
        public bool RecordsTrackedScores(string Batch, int amount)
        {
            bool istracked = false;
            int Count = amount;
            string mybatch = Batch;
            using (var context = new CETAPEntities())
            {
                try
                {
                    ScanTracker tracker = context.ScanTrackers.Where(s => s.FileName == mybatch).FirstOrDefault();
                    if (tracker != null)
                    {
                        // batchBDO already in tracker, probably missed in the entry of batches
                        context.ScanTrackers.Remove(tracker);
                        tracker.DateScored = DateTime.Now;
                        tracker.DateModified = DateTime.Now;
                        tracker.ScoredRecords = Count;
                        // tracker.BatchedBy = batchInDB.BatchedBy;
                        context.ScanTrackers.Attach(tracker);
                        context.Entry(tracker).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        ScanTracker scant = new ScanTracker();
                        //   scant.BatchedBy = batchInDB.BatchedBy;
                        scant.DateScored = DateTime.Now;
                        scant.ScoredRecords = Count;
                        scant.DateModified = DateTime.Now;
                        scant.FileName = mybatch;
                        context.ScanTrackers.Add(scant);
                    }
                    context.SaveChanges();


                    istracked = true;
                }
                catch (Exception ex)
                {

                    throw ex.InnerException;
                }
            }

            return istracked;
        }
        #endregion

        #region  Moderation


        public ObservableCollection<CompositBDO> GetAllScores(string path)
        {
            string Filename = Path.Combine(path, "Composit.csv");
            if (File.Exists(Filename))
            {
                using (var sr = new StreamReader(Filename))
                {

                    var reader = new CsvReader(sr);
                    reader.Configuration.HasHeaderRecord = true;
                    IEnumerable<CompositBDO> records = reader.GetRecords<CompositBDO>().ToList();
                    Scores = new ObservableCollection<CompositBDO>(records);
                }
            }
            else
            {
                Scores = new ObservableCollection<CompositBDO>();
            }
            return Scores;
        }
        public ObservableCollection<CompositBDO> GetAllRemoteScores()
        {
            ObservableCollection<CompositBDO> remoteScores = new ObservableCollection<CompositBDO>();

            using (CETAPEntities cetapEntities = new CETAPEntities())
            {
                //   List<TestVenue> venues = cetapEntities.TestVenues.Where(x => x.VenueType == "Remote").ToList();

                //  var Scores = cetapEntities.Composits.Where(x => x.VenueCode >= myYear.yearStart && x.DOT <= myYear.yearEnd).ToList();
                var Scores = cetapEntities.Composits
               .Join(cetapEntities.TestVenues,
                   c => c.VenueCode,
                   v => v.VenueCode,
                   (c, v) => new { Composit = c, v.VenueType })
               .Where(x => x.VenueType == "Remote")
               .Select(x => x.Composit)
                .ToList();

                foreach (Composit score in Scores)
                {
                    CompositBDO comp = new CompositBDO();
                    comp = Maps.CompositDALToCompositBDO(score);
                    remoteScores.Add(comp);
                }
               // remoteScores = new ObservableCollection<CompositBDO>(Scores);

            }
                return remoteScores;
        }
        public ObservableCollection<CompositBDO> GetAllModeratedScores(string path)
        {
            string Filename = Path.Combine(path, "Composit.csv");
            if (File.Exists(Filename))
            {
                using (var sr = new StreamReader(Filename))
                {
                    var reader = new CsvReader(sr);
                    IEnumerable<CompositBDO> records = reader.GetRecords<CompositBDO>().ToList();
                    ModeratedScores = new ObservableCollection<CompositBDO>(records);
                }
            }
            else
            {
                ModeratedScores = new ObservableCollection<CompositBDO>();
            }
            return ModeratedScores;

        }
        #endregion

        #region QA Data

        public void LoadTestDate(DateTime TheTestDate)
        {
            TestDate = TheTestDate;
        }
        public void SaveRawCSXData()
        {
            string Folder = ApplicationSettings.Default.QAFolder;
            // List<datFileAttributes> myfiles = new List<datFileAttributes>();

            DirectoryInfo dir = new DirectoryInfo(Folder);
            FileInfo[] info = dir.GetFiles("*.dat");


            List<QADatRecord> mylistData = new List<QADatRecord>();
            foreach (FileInfo file in info)
            {
                datFileAttributes qafBDO = new datFileAttributes(file.FullName);

                // myRawData = GetQADataFromFile(qafBDO);

                // myfiles.Add(qafBDO);
                FileHelperAsyncEngine engine;
                engine = new FileHelperAsyncEngine(typeof(ASC667));
                engine.BeginReadFile(qafBDO.FilePath);

                List<QADatRecord> records = new List<QADatRecord>();
                foreach (ASC667 record in engine)
                {
                    QADatRecord mdata = new QADatRecord(qafBDO.FilePath);
                    // mdata.TestDate = bb.BatchID < 1 ? TestDate : bb.TestDate;
                    CSX667ToQADatRecord(record, mdata);

                    records.Add(mdata);
                }

                mylistData.AddRange(records);
            }

            ObservableCollection<QADatRecord> myRawData = new ObservableCollection<QADatRecord>(mylistData);


            //string message = "File not saved";
            DateTime Now = DateTime.Now;
            string myDate = Now.ToString("yyyyMMdd");
            string DirectoryPath = Folder;
            DirectoryPath = Path.Combine(DirectoryPath, myDate);
            string SName = "Raw2015Intake_20150706";
            string QAFolderpath = Path.Combine(DirectoryPath, SName);
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                //write file into directory
                using (StreamWriter file = new StreamWriter(QAFolderpath))
                {
                    foreach (QADatRecord QA in myRawData)
                    {
                        // string line = "";


                        string line = QADataRecordToCSX667_raw(QA);
                        file.WriteLine(line);

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void ReadEndofDatFile()
        {

            try
            {
                using (StreamReader reader = new StreamReader("CetapDat.dat"))
                {
                    string readtext = reader.ReadLine();
                    EOFDatFile = readtext;
                    reader.Close();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
      
        
        public ObservableCollection<QADatRecord> GetQADataFromFile(datFileAttributes filename)
        {

            FileHelperAsyncEngine engine;

            BatchBDO bb = new BatchBDO();
            List<TestBDO> Tests = new List<TestBDO>();
            string AQLCode = "";
            string MATCode = "";
            string surname = "";
            if (ApplicationSettings.Default.DBAvailable)
            {
                //----------------------Magic Area ------------------------------------------------------------------------------------//
                //1. get the Batch file from Batches and date of test

                bb = GetBatchByName(filename.SName);

                //2. get the Tests from the profile

                Tests = GetTestfromTestProfile(filename.Profile); // now we know the tests which were used for this file

                foreach (var prof in Tests)
                {
                    string testT = prof.TestName.Substring(0, 4);
                    switch (bb.TestCombination)
                    {
                        case "0105":
                            if (testT == "AQLE") AQLCode = prof.TestCode.ToString("000");

                            break;

                        case "0115":
                            if (testT == "AQLA") AQLCode = prof.TestCode.ToString("000");

                            break;

                        case "0106":
                            if (testT == "MATE") MATCode = prof.TestCode.ToString("000");

                            break;

                        case "0116":
                            if (testT == "MATA") MATCode = prof.TestCode.ToString("000");

                            break;

                        case "0107":
                            if (testT == "AQLE")
                            {
                                AQLCode = prof.TestCode.ToString("000");

                            }
                            if (testT == "MATE")
                            {
                                MATCode = prof.TestCode.ToString("000");
                            }
                            break;

                        case "0117":
                            if (testT == "AQLA")
                            {
                                AQLCode = prof.TestCode.ToString("000");
                            }
                            if (testT == "MATA")
                            {
                                MATCode = prof.TestCode.ToString("000");
                            }

                            break;

                        case "0127":
                            if (testT == "AQLE")
                            {
                                AQLCode = prof.TestCode.ToString("000");
                            }

                            if (testT == "MATA")
                            {
                                MATCode = prof.TestCode.ToString("000");
                            }
                            break;

                        case "0137":
                            if (testT == "AQLA")
                            {
                                AQLCode = prof.TestCode.ToString("000");
                            }
                            if (testT == "MATE")
                            {
                                MATCode = prof.TestCode.ToString("000");
                            }
                            break;

                        default:
                            break;
                    }

                } // get test codes

            }
            //-------------------- End Magic Area --------------------------------------------------------------------------------//
            try
            {
                switch (filename.CSX)
                {
                  
                    case 667:
                        // FileHelperAsyncEngine engine;
                        engine = new FileHelperAsyncEngine(typeof(ASC667));
                        engine.BeginReadFile(filename.FilePath);
                        List<QADatRecord> records = new List<QADatRecord>();
                        foreach (ASC667 record in engine)
                        {
                            QADatRecord mdata = new QADatRecord(filename.FilePath);
                            mdata.TestDate = bb.BatchID < 1 ? TestDate : bb.TestDate;
                            mdata.AQLCOD = AQLCode;
                            mdata.MATCOD = MATCode;
                            CSX667ToQADatRecord(record, mdata);

                            records.Add(mdata);
                        }

                        // arrange records by size of errors
                        // records = records.OrderByDescending(a => a.errorCount).ToList();
                        QaData = new ObservableCollection<QADatRecord>(records);
                        break;

                    case 669:
                        break;

                    case 761:
                        try
                        {
                            //   FileHelperAsyncEngine engine;
                            engine = new FileHelperAsyncEngine(typeof(ASC761));

                            engine.BeginReadFile(filename.FilePath);
                            List<QADatRecord> records1 = new List<QADatRecord>();
                            foreach (ASC761 record1 in engine)
                            {
                                QADatRecord xdata = new QADatRecord(filename.FilePath);
                                xdata.TestDate = bb.BatchID < 1 ? TestDate : bb.TestDate;
                                xdata.AQLCOD = AQLCode;
                                xdata.MATCOD = MATCode;
                                CSX761ToQADatRecord(record1, xdata);
                                surname = xdata.Surname;
                                records1.Add(xdata);
                            }
                            QaData = new ObservableCollection<QADatRecord>(records1);



                        }
                        catch (FileHelpers.BadUsageException ex)
                        {

                            MessageBox.Show(ex.Message, "File: " + filename.SName, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                        break;

                    case 886:
                        try
                        {
                            //   FileHelperAsyncEngine engine;
                            engine = new FileHelperAsyncEngine(typeof(ASC886));

                            engine.BeginReadFile(filename.FilePath);
                            List<QADatRecord> records8 = new List<QADatRecord>();
                            foreach (ASC886 record8 in engine)
                            {
                                QADatRecord xdata = new QADatRecord(filename.FilePath);
                                xdata.TestDate = bb.BatchID < 1 ? TestDate : bb.TestDate;
                                xdata.AQLCOD = AQLCode;
                                xdata.MATCOD = MATCode;
                                CSX886ToQADatRecord(record8, xdata);
                                surname = xdata.Surname;
                                records8.Add(xdata);
                            }
                            QaData = new ObservableCollection<QADatRecord>(records8);



                        }
                        catch (FileHelpers.BadUsageException ex)
                        {

                            MessageBox.Show(ex.Message, "File: " + filename.SName, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                        break;

                    case 909:
                        try
                        {
                            //   FileHelperAsyncEngine engine;
                            engine = new FileHelperAsyncEngine(typeof(ASC909));

                            engine.BeginReadFile(filename.FilePath);
                            List<QADatRecord> records9 = new List<QADatRecord>();
                            foreach (ASC909 record9 in engine)
                            {
                                QADatRecord xdata = new QADatRecord(filename.FilePath);
                                xdata.TestDate = bb.BatchID < 1 ? TestDate : bb.TestDate;
                                xdata.AQLCOD = AQLCode;
                                xdata.MATCOD = MATCode;
                                CSX909ToQADatRecord(record9, xdata);
                                surname = xdata.Surname;
                                records9.Add(xdata);
                            }
                            QaData = new ObservableCollection<QADatRecord>(records9);



                        }
                        catch (FileHelpers.BadUsageException ex)
                        {

                            MessageBox.Show(ex.Message, "File: " + filename.SName, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                string wrong = surname;
                string a = ex.ToString();
                // throw;
            }


            return QaData;
        }

        public ObservableCollection<QADatRecord> GetQARecords()
        {
            return QaData;
        }

        public QADatRecord GetNBTNumberFromDBbySAID(QADatRecord SelectedRecord)
        {

            long mySAID = Convert.ToInt64(SelectedRecord.SAID);
            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.SAID == mySAID).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.Reference = WriterInDB.NBT.ToString();
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetNBTNumberFromDBbyFID(QADatRecord SelectedRecord)
        {

            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.ForeignID == SelectedRecord.ForeignID).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.Reference = WriterInDB.NBT.ToString();
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetSAIDbyNBT(QADatRecord SelectedRecord)
        {

            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.NBT.ToString() == SelectedRecord.Reference).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.SAID = WriterInDB.SAID.ToString();
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetNamebyNBT(QADatRecord SelectedRecord)
        {
            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.NBT.ToString() == SelectedRecord.Reference).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.FirstName = WriterInDB.Name.ToUpper();
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetSurnamebyNBT(QADatRecord SelectedRecord)
        {
            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.NBT.ToString() == SelectedRecord.Reference).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.Surname = WriterInDB.Surname.ToUpper();
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetDOBfromDB(QADatRecord SelectedRecord)
        {
            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.NBT.ToString() == SelectedRecord.Reference).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.DOB = WriterInDB.DOB;
                }
            }
            return SelectedRecord;
        }
        public QADatRecord GetFIDbyNBT(QADatRecord SelectedRecord)
        {

            using (var context = new CETAPEntities())
            {
                WriterList WriterInDB = context.WriterLists.Where(x => x.NBT.ToString() == SelectedRecord.Reference).FirstOrDefault();
                if (WriterInDB != null)
                {
                    SelectedRecord.ForeignID = WriterInDB.ForeignID.ToString();
                }
            }
            return SelectedRecord;
        }

        public bool SaveQADatFile(datFileAttributes datfile, ref string message)
        {
            bool ret = false;

            DateTime Now = DateTime.Now;
            string myDate = Now.ToString("yyyyMMdd");
            string DirectoryPath = Path.GetDirectoryName(datfile.FilePath);
            DirectoryPath = Path.Combine(DirectoryPath, myDate);
            string SName = Path.GetFileName(datfile.FilePath);
            string QAFolderpath = Path.Combine(DirectoryPath, SName);
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                //write file into directory
                using (StreamWriter file = new StreamWriter(QAFolderpath))
                {
                    foreach (QADatRecord QA in QaData)
                    {
                        // string line = "";

                        switch (datfile.CSX)
                        {
                            case 667:
                                string line = QADataRecordToCSX667(QA);
                                file.WriteLine(line);
                                //line += "\r";
                                //file.Write(line); SPSS gives error if file does not have CR\LF
                                break;
                            case 761:
                                string line1 = QADataRecordToCSX761(QA);
                                //line1 += "\r";
                                //file.Write(line1); // spss errors if line does not have windows CR\LF
                                file.WriteLine(line1);
                                break;

                            case 669:
                                break;

                            case 886:
                                string line8 = QADataRecordToCSX886(QA);
                                file.WriteLine(line8);
                                break;

                            case 909:
                                string line9 = QADataRecordToCSX886(QA);
                                file.WriteLine(line9);
                                break;

                            default:
                                break;
                        }



                    }
                    file.Write(EOFDatFile); // temporary for checking to see if spss is affected by last line.
                    file.Close();
                    message = "File moved to subfolder \n";
                    if (ApplicationSettings.Default.DBAvailable)
                    {
                        int QaRecords = File.ReadAllLines(QAFolderpath).Length - 1;
                        ScanTrackerBDO scantrackerBDO = new ScanTrackerBDO();
                        scantrackerBDO.QARecords = QaRecords;
                        scantrackerBDO.DateQA = DateTime.Now;
                        scantrackerBDO.FileName = datfile.SName;


                        using (var context = new CETAPEntities())
                        {
                            context.Database.CommandTimeout = 120;
                            try
                            {
                                ScanTracker scantracker = context.ScanTrackers.Where(m => m.FileName == datfile.SName).Select(v => v).FirstOrDefault();
                                if (scantracker != null)
                                {
                                    context.ScanTrackers.Remove(scantracker);

                                    scantracker.DateModified = DateTime.Now;
                                    scantracker.QARecords = QaRecords;
                                    scantracker.DateQA = DateTime.Now;
                                    context.ScanTrackers.Attach(scantracker);
                                    context.Entry(scantracker).State = System.Data.Entity.EntityState.Modified;

                                }
                                else // creater new tracker
                                {
                                    var tracker = Maps.ScanTrackerBDOToScanTrackerDAL(scantrackerBDO);
                                    tracker.DateModified = DateTime.Now;
                                    context.ScanTrackers.Add(tracker);
                                }
                                context.SaveChanges();
                                message += "Recorded in Database \n";
                            }
                            catch (Exception ex)
                            {

                                message += "Error moving file or \n Error saving file to Database \n" + ex.InnerException.ToString();
                            }
                        }
                    }
                    ret = true;
                }

                if (File.Exists(QAFolderpath)) File.Delete(datfile.FilePath);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ret;
        }

        public bool AutoClean()
        {
            bool ret = false;
            bool InDB = false;
            BatchBDO bb = new BatchBDO();

            bool IsDB = ApplicationSettings.Default.DBAvailable;
            if (QaData.Count() > 0)
            {
                ret = true;
                //int Profile = SelectedFile.Profile;
                var QAerrors = (from a in QaData
                                where a.HasErrors == true
                                select a).ToList();

                foreach (QADatRecord toCleanRecord in QAerrors)
                {
                    string Walkin = toCleanRecord.Reference.Substring(7, 1);
                    if (Walkin != "9") InDB = true;

                    QADatRecord QDat = new QADatRecord();
                    bb = GetBatchByName(toCleanRecord.DatFile.SName);
                    var myerrors = toCleanRecord._errors.Keys.ToList();
                    int count = myerrors.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string area = myerrors[i];
                        switch (area)
                        {
                            case "Reference":
                                if (IsDB && (toCleanRecord.SAID != null || toCleanRecord.SAID.Trim() != "") && InDB)
                                {
                                    QDat = GetNBTNumberFromDBbySAID(toCleanRecord);
                                    toCleanRecord.Reference = QDat.Reference;
                                }
                                if (IsDB && (toCleanRecord.ForeignID != null || toCleanRecord.ForeignID.Trim() != "") && InDB)
                                {
                                    QDat = GetNBTNumberFromDBbyFID(toCleanRecord);
                                    toCleanRecord.Reference = QDat.Reference;
                                }

                                break;

                            case "Barcode":
                                break;

                            case "Surname":
                                if (IsDB && InDB)
                                {
                                    QDat = GetSurnamebyNBT(toCleanRecord);
                                    toCleanRecord.Surname = QDat.Surname;
                                }
                                break;

                            case "FirstName":
                                if (IsDB && InDB)
                                {
                                    QDat = GetNamebyNBT(toCleanRecord);
                                    toCleanRecord.FirstName = QDat.FirstName;
                                }
                                break;

                            case "initials":
                                break;

                            case "SAID":
                                if (IsDB && InDB)
                                {
                                    QDat = GetSAIDbyNBT(toCleanRecord);
                                }
                                break;

                            case "ForeignID":
                                if (IsDB && InDB)
                                {
                                    QDat = GetFIDbyNBT(toCleanRecord);
                                    toCleanRecord.ForeignID = QDat.ForeignID;
                                }
                                break;

                            case "DOB":
                                if (IsDB && InDB)
                                {
                                    QDat = GetDOBfromDB(toCleanRecord);
                                    toCleanRecord.DOB = QDat.DOB;
                                }
                                break;

                            case "IDType":
                                if (toCleanRecord.IDType == "1")
                                {
                                    toCleanRecord.IDType = "2";
                                }
                                else
                                {
                                    toCleanRecord.IDType = "1";
                                }
                                break;

                            case "Gender":
                                break;

                            case "Citizenship":
                                break;

                            case "Classification":
                                break;

                            case "VenueCode":
                                toCleanRecord.VenueCode = toCleanRecord.DatFile.VenueCode.ToString("D5");
                                break;

                            case "DOT":
                                toCleanRecord.DOT = bb.TestDate;
                                break;

                            case "Homelanguage":
                                break;

                            case "SchoolLanguage":
                                break;

                            case "AQL_Language":
                                toCleanRecord.AQL_Language = toCleanRecord.DatFile.AQL_Language;
                                break;

                            case "AQL_Code":
                                toCleanRecord.AQL_Code = toCleanRecord.AQLCOD;
                                break;

                            case "Mat_Language":
                                toCleanRecord.Mat_Language = toCleanRecord.DatFile.MAT_Language;
                                break;

                            case "MatCode":
                                toCleanRecord.MatCode = toCleanRecord.MATCOD;
                                break;

                            case "Faculty1":
                                if (toCleanRecord.Faculty1 == " " || toCleanRecord.Faculty1 == "" || toCleanRecord.Faculty1 == "*") toCleanRecord.Faculty1 = "N";
                                break;

                            case "Faculty2":
                                if (toCleanRecord.Faculty2 == "*") toCleanRecord.Faculty2 = "N";
                                break;

                            case "Faculty3":
                                if (toCleanRecord.Faculty3 == "*") toCleanRecord.Faculty3 = "N";
                                break;

                            case "Section1":
                                foreach (DatAnswer a in toCleanRecord.Section1)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section2":
                                foreach (DatAnswer a in toCleanRecord.Section2)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section3":
                                foreach (DatAnswer a in toCleanRecord.Section3)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section4":
                                foreach (DatAnswer a in toCleanRecord.Section4)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section5":
                                foreach (DatAnswer a in toCleanRecord.Section5)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section6":
                                foreach (DatAnswer a in toCleanRecord.Section6)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "Section7":
                                foreach (DatAnswer a in toCleanRecord.Section7)
                                {
                                    if (a.Value == '*') a.Value = 'N';
                                    string n = a.Value.ToString().ToUpper();
                                    a.Value = Convert.ToChar(n);
                                }
                                break;

                            case "MathSection":
                                if (toCleanRecord.DatFile.TestCode == "0105" || toCleanRecord.DatFile.TestCode == "0115")
                                {
                                    toCleanRecord.MatCode = "";
                                    toCleanRecord.Mat_Language = "";
                                    toCleanRecord.MathSection.Clear();
                                }
                                else
                                {
                                    foreach (DatAnswer a in toCleanRecord.MathSection)
                                    {
                                        if (a.Value == '*') a.Value = 'N';
                                        string n = a.Value.ToString().ToUpper();
                                        a.Value = Convert.ToChar(n);
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }


                }
            }

            return ret;
        }

        public bool updateQAtoTracker(string batch, int Count)
        {
            bool isSaved = false;
            using (var context = new CETAPEntities())
            {
                try
                {
                    ScanTracker tracker = context.ScanTrackers.Where(s => s.FileName == batch).FirstOrDefault();
                    if (tracker != null)
                    {
                        // batchBDO already in tracker, probably missed in the entry of batches
                        context.ScanTrackers.Remove(tracker);
                        tracker.DateSentForScoring = DateTime.Now;
                        tracker.DateModified = DateTime.Now;
                        tracker.SentCount = Count;
                        // tracker.BatchedBy = batchInDB.BatchedBy;
                        context.ScanTrackers.Attach(tracker);
                        context.Entry(tracker).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        ScanTracker scant = new ScanTracker();
                        //   scant.BatchedBy = batchInDB.BatchedBy;
                        scant.DateSentForScoring = DateTime.Now;
                        scant.SentCount = Count;
                        scant.DateModified = DateTime.Now;
                        scant.FileName = batch;
                        context.ScanTrackers.Add(scant);
                    }
                    context.SaveChanges();


                    isSaved = true;
                }
                catch (Exception ex)
                {

                    throw ex.InnerException;
                }
            }

            return isSaved;
        }
        public bool AddNameToList(string name)
        {
            bool added = false;
            name = name.Trim();
            using (var context = new CETAPEntities())
            {
                var myname = new FirstName()
                {
                    Name = name,
                    DateModified = DateTime.Now
                };
                context.FirstNames.Add(myname);
                context.SaveChanges();
                added = true;
            }
            return added;
        }
        public bool AddSurnameToList(string surname)
        {
            bool Added = false;
            surname = surname.Trim();
            using (var context = new CETAPEntities())
            {
                Surname surn = new Surname()
                {
                    Surname1 = surname,
                    DateModified = DateTime.Now
                };
                context.Surnames.Add(surn);
                context.SaveChanges();
                Added = true;
            }

            ListSurnames.AddSurname(surname);
            return Added;
        }

        #endregion

        #region 2015 Raw Scoring Data
        private string QADataRecordToCSX667_raw(QADatRecord QAData)
        {
            string CSX667Raw = "";

            string CSX = string.Format("{0,-37}", QAData.CSX_Part);
            string Barcode = string.Format("{0,-12}", QAData.Barcode);
            string Reference = string.Format("{0,-14}", QAData.Reference);

            CSX667Raw = Barcode;

            string ID = string.Format("{0,-13}", QAData.SAID);
            string FID = string.Format("{0,-15}", QAData.ForeignID);
            string Surname = string.Format("{0,-20}", QAData.Surname);
            string Name = string.Format("{0,-18}", QAData.FirstName);
            string Initials = string.Format("{0,-3}", QAData.initials);
            string IDType = string.Format("{0,-1}", QAData.IDType);
            string Gender = string.Format("{0,-1}", QAData.Gender);
            string VenueCode = string.Format("{0,-5}", QAData.VenueCode);
            string DOT = QAData.DOT.ToString("yyyyMMdd");
            string DOB = QAData.DOB.ToString("yyyyMMdd");
            string AQL_Lang = string.Format("{0,-1}", QAData.AQL_Language);
            string AQL_Code = string.Format("{0,-2}", QAData.AQL_Code);
            string Mat_Lang = string.Format("{0,-1}", QAData.Mat_Language);
            string Mat_Code = string.Format("{0,-2}", QAData.MatCode);
            string Faculty = string.Format("{0,-1}", QAData.Faculty1);
            string Faculty2 = string.Format("{0,-1}", QAData.Faculty2);
            string Faculty3 = string.Format("{0,-1}", QAData.Faculty3);
            string Citizenship = string.Format("{0,-1}", QAData.Citizenship);
            string HLanguage = string.Format("{0,-2}", QAData.HomeLanguage);
            string SLanguage = string.Format("{0,-1}", QAData.SchoolLanguage);
            string Classification = string.Format("{0,-1}", QAData.Classification);

            //CSX667Raw += ID + FID + IDType + Gender + Citizenship + Faculty + DOB + Surname + Name + Initials;
            string Section1 = HelperUtils.CollectionToString(QAData.Section1);
            string Section2 = HelperUtils.CollectionToString(QAData.Section2);
            string Section3 = HelperUtils.CollectionToString(QAData.Section3);
            string Section4 = HelperUtils.CollectionToString(QAData.Section4);
            string Section5 = HelperUtils.CollectionToString(QAData.Section5);
            string Section6 = HelperUtils.CollectionToString(QAData.Section6);
            string Section7 = HelperUtils.CollectionToString(QAData.Section7);
            string MatSection = HelperUtils.CollectionToString(QAData.MathSection);
            string Batch = QAData.DatFile.SName;
            //string Freespace = "  ";

            //CSX667Raw += VenueCode + DOT + HLanguage + SLanguage + Classification + AQL_Lang + AQL_Code;
            CSX667Raw += Section1 + Section2 + Section3 + Section4 + Section5 + Section6 + Section7 + MatSection;
            CSX667Raw += "," + Reference + "," + Barcode + "," + Batch;
            return CSX667Raw;
        }
        private string ReplaceN(string record)
        {
            int length = record.Length;
            char[] array = record.ToCharArray();
            // first replace space with N
            int i;
            for (i = 0; i < record.Length; i++)
            {
                if (array[i] == ' ') array[i] = 'N';
            }

            // replace end with X
            while ((array[i] == 'N') && (i > 2))
            {
                array[i] = 'X';
                i -= 1;
            }

            string rec = new string(array);
            return rec;

        }

        // 4. Translate Answersheet to required AQL and Maths files
        //private AQLdat TranslateAStoAQL(AS667 record)
        //{
        //    AQLdat recAQL = new AQLdat();
        //    recAQL.AQL_Code = record.AQL_Code;
        //    recAQL.AQL_Language = record.AQL_Language;
        //    recAQL.SessionID = record.SessionID;
        //    recAQL.Citizenship = record.Citizenship;
        //    recAQL.Classification = record.Classification;
        //    recAQL.Gender = record.Gender;
        //    recAQL.IDType = record.IDType;
        //    recAQL.HomeLanguage = record.HomeLanguage;
        //    recAQL.SchoolLanguage = record.SchoolLanguage;
        //    recAQL.AQL_Section1 = ReplaceN(record.AQL_Section1.Substring(0, 17));
        //    recAQL.AQL_Section2 = ReplaceN(record.AQL_Section2.Substring(0, 17));
        //    recAQL.AQL_Section3 = ReplaceN(record.AQL_Section3.Substring(0, 25));
        //    recAQL.AQL_section4 = ReplaceN(record.AQL_section4.Substring(0, 25));
        //    recAQL.AQL_Section5 = ReplaceN(record.AQL_Section5.Substring(0, 17));
        //    recAQL.AQL_Section6 = ReplaceN(record.AQL_Section6.Substring(0, 24));
        //    recAQL.AQL_Section7 = ReplaceN(record.AQL_Section7.Substring(0, 25));

        //    return recAQL;
        //}


        #endregion

        #region Utilities
        public long GenerateNBTNumber(string intakeYr, int number)
        {
            long NBT;
            string year = intakeYr.Substring(2, 2);
            string anum = number.ToString("D6");
            string myNumber = "3100" + year + number;

            int checkD = HelperUtils.ComputeChecksum(myNumber);
            string myNBT = "9" + myNumber + checkD.ToString();
            if (myNBT.Length != 14) return 0;
            NBT = Convert.ToInt64(myNBT);
            return NBT;
        }
        #endregion

        #region Section7_Read
        public ObservableCollection<Section7> getSec7DatFile(datFileAttributes datfile)
        {
            ObservableCollection<Section7> TrialData = new ObservableCollection<Section7>();


            FileHelperAsyncEngine engine;

            try
            {
                switch (datfile.CSX)
                {
                    case 667:
                        // FileHelperAsyncEngine engine;
                        engine = new FileHelperAsyncEngine(typeof(ASC667));
                        engine.BeginReadFile(datfile.FilePath);
                        List<Section7> records = new List<Section7>();
                        foreach (ASC667 record in engine)
                        {
                            Section7 mdata = new Section7();

                            mdata.Barcode = record.SessionID;
                            string msize = string.Format("{0,-25}", record.AQL_Section7);
                            mdata.TrialSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(msize));
                            //  QAData.Section1 = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(record.AQL_Section1));
                            records.Add(mdata);
                        }

                        // arrange records by size of errors
                        // records = records.OrderByDescending(a => a.errorCount).ToList();
                        TrialData = new ObservableCollection<Section7>(records);

                        break;

                    case 669:
                        break;

                    case 761:
                        //   FileHelperAsyncEngine engine;
                        engine = new FileHelperAsyncEngine(typeof(ASC761));

                        engine.BeginReadFile(datfile.FilePath);
                        List<Section7> records1 = new List<Section7>();
                        foreach (ASC761 record1 in engine)
                        {
                            Section7 xdata = new Section7();

                            xdata.Barcode = record1.SessionID;

                            string msize = string.Format("{0,-25}", record1.AQL_Section7);
                            xdata.TrialSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(msize));
                            records1.Add(xdata);
                        }
                        TrialData = new ObservableCollection<Section7>(records1);


                        break;
                    case 886:
                        //   FileHelperAsyncEngine engine;
                        engine = new FileHelperAsyncEngine(typeof(ASC886));

                        engine.BeginReadFile(datfile.FilePath);
                        List<Section7> records8 = new List<Section7>();
                        foreach (ASC886 record8 in engine)
                        {
                            Section7 xdata = new Section7();

                            xdata.Barcode = record8.SessionID;

                            string msize = string.Format("{0,-25}", record8.AQL_Section7);
                            xdata.TrialSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(msize));
                            records8.Add(xdata);
                        }
                        TrialData = new ObservableCollection<Section7>(records8);
                        break;

                    case 909:
                        //   FileHelperAsyncEngine engine;
                        engine = new FileHelperAsyncEngine(typeof(ASC909));

                        engine.BeginReadFile(datfile.FilePath);
                        List<Section7> records9 = new List<Section7>();
                        foreach (ASC909 record9 in engine)
                        {
                            Section7 xdata = new Section7();

                            xdata.Barcode = record9.SessionID;

                            string msize = string.Format("{0,-25}", record9.AQL_Section7);
                            xdata.TrialSection = new ObservableCollection<DatAnswer>(HelperUtils.GetAnswerList(msize));
                            records9.Add(xdata);
                        }
                        TrialData = new ObservableCollection<Section7>(records9);
                        break;

                    default:
                        break;

                }
            }
            catch (Exception ex)
            {

                string a = ex.ToString();
                // throw;
            }
            return TrialData;
        }
        #endregion

        public List<ForDuplicatesBarcodesBDO> FindDuplicatesFromDB(ObservableCollection<ForDuplicatesBarcodesBDO> BatchRecords)
        {

            IntakeYearsBDO myYear = new IntakeYearsBDO();
            List<ForDuplicatesBarcodesBDO> duplicatesBarcodesBdoList = new List<ForDuplicatesBarcodesBDO>();            

            var myBatch = new List<Composit>();
            myYear = GetIntakeRecord(ApplicationSettings.Default.IntakeYear);

            using (CETAPEntities cetapEntities = new CETAPEntities())
            {

                var Scores = cetapEntities.Composits.Where(x => x.DOT >= myYear.yearStart && x.DOT <= myYear.yearEnd).ToList();

                // find duplicates in Database
                var resu = BatchRecords.Where(x => Scores.Any(m => m.Barcode.Equals(x.Barcode))).ToList();

                if (resu != null)
                {
                    foreach (ForDuplicatesBarcodesBDO duplicatesBarcodesBdo in resu)
                        duplicatesBarcodesBdoList.Add(new ForDuplicatesBarcodesBDO()
                        {

                            Reason = "Duplicate Barcode in Database",
                            FID = duplicatesBarcodesBdo.FID,
                            Barcode = duplicatesBarcodesBdo.Barcode,
                            RefNo = duplicatesBarcodesBdo.RefNo,
                            SAID = duplicatesBarcodesBdo.SAID,
                            Batch = duplicatesBarcodesBdo.Batch,
                            DateModified = DateTime.Now
                        });

                }


                //Check for duplicate NBT numbers

                var NBT_Duplicates = Scores.GroupBy(x => x.RefNo)
                                             .Where(g => g.Skip(1).Any())
                                             .SelectMany(g => g);

                var res1 = BatchRecords.Where(x => NBT_Duplicates.Any(m => m.RefNo.Equals(x.RefNo)));
                if (res1 != null)
                {
                    foreach (var duplicatesBarcodesBdo in res1)
                        duplicatesBarcodesBdoList.Add(new ForDuplicatesBarcodesBDO()
                        {
                            Reason = "NBT appears more than once in Database, Check if it should be scored for the third time",
                            FID = duplicatesBarcodesBdo.FID,
                            Barcode = duplicatesBarcodesBdo.Barcode,
                            RefNo = duplicatesBarcodesBdo.RefNo,
                            SAID = duplicatesBarcodesBdo.SAID,
                            Batch = duplicatesBarcodesBdo.Batch,
                            DateModified = DateTime.Now
                        });

                }

                // Check duplicate SAID
                var SAID_Duplicates = Scores.GroupBy(x => x.SAID)
                                             .Where(g => g.Skip(1).Any() && g.Key.HasValue)
                                             .SelectMany(g => g);
                var res2 = BatchRecords.Where(y => SAID_Duplicates.Any(m => m.SAID.Equals(y.SAID)));
                if (res2 != null)
                {
                    foreach (var duplicatesBarcodesBdo in res2)
                        duplicatesBarcodesBdoList.Add(new ForDuplicatesBarcodesBDO()
                        {
                            Reason = "SAID appears more than once in Database, Check if it should be scored for the third time",
                            FID = duplicatesBarcodesBdo.FID,
                            Barcode = duplicatesBarcodesBdo.Barcode,
                            RefNo = duplicatesBarcodesBdo.RefNo,
                            SAID = duplicatesBarcodesBdo.SAID,
                            Batch = duplicatesBarcodesBdo.Batch,
                            DateModified = DateTime.Now
                        });

                }


                // Check duplicate ForeignID
                var FID_Duplicates = Scores.GroupBy(x => x.ForeignID)
                                            .Where(g => g.Skip(1).Any() && g.Key != "")
                                            .SelectMany(g => g);
                var res3 = BatchRecords.Where(xy => FID_Duplicates.Any(m => m.ForeignID.Equals(xy.FID)));
                if (res3 != null)
                {
                    foreach (var duplicatesBarcodesBdo in res3)
                        duplicatesBarcodesBdoList.Add(new ForDuplicatesBarcodesBDO()
                        {
                            Reason = "Foreign ID appears more than once in Database, Check if it should be scored for the third time",
                            FID = duplicatesBarcodesBdo.FID,
                            Barcode = duplicatesBarcodesBdo.Barcode,
                            RefNo = duplicatesBarcodesBdo.RefNo,
                            SAID = duplicatesBarcodesBdo.SAID,
                            Batch = duplicatesBarcodesBdo.Batch,
                            DateModified = DateTime.Now
                        });

                }

                return duplicatesBarcodesBdoList;
            }
        }
            
        public bool DuplicateReportGeneration(List<ForDuplicatesBarcodesBDO> Duplicates, string filename)
        {
            XLWorkbook xlWorkbook = new XLWorkbook();
            IXLWorksheet xlWorksheet = xlWorkbook.Worksheets.Add("Duplicate Records");
            xlWorksheet.Range("A1:H1").Style.Fill.BackgroundColor = XLColor.TealBlue;
            xlWorksheet.Cell(1, 1).Value = (object)"Barcode";
            xlWorksheet.Cell(1, 2).Value = (object)"NBT Number";
            xlWorksheet.Cell(1, 3).Value = (object)"Batch Name";
            xlWorksheet.Cell(1, 4).Value = (object)"SA ID";
            xlWorksheet.Cell(1, 5).Value = (object)"Passport";
            xlWorksheet.Cell(1, 6).Value = (object)"Reason";
            xlWorksheet.Cell(1, 7).Value = (object)"RecNo";
            xlWorksheet.Cell(1, 8).Value = (object)"Date Modified";
            xlWorksheet.Cell(2, 1).Value = (object)Duplicates.AsEnumerable<ForDuplicatesBarcodesBDO>();
          //  DateTime dateTime = new DateTime();
            DateTime now = DateTime.Now;
            string str = now.Year.ToString() + now.Month.ToString("00") + now.Day.ToString("00") + "_" + now.Hour.ToString("00") + now.Minute.ToString("00");
            filename = filename + str + ".xlsx";
            xlWorkbook.SaveAs(filename);
            return true;
        }

        public ObservableCollection<ForDuplicatesBarcodesBDO> GetBatchesInQueue()
        {
            ObservableCollection<ForDuplicatesBarcodesBDO> observableCollection = new ObservableCollection<ForDuplicatesBarcodesBDO>();
            using (CETAPEntities cetapEntities = new CETAPEntities())
            {
                foreach (RecordsInQueue inqueue in cetapEntities.RecordsInQueues.ToList<RecordsInQueue>())
                {
                    ForDuplicatesBarcodesBDO duplicates = new ForDuplicatesBarcodesBDO();
                    InqueueToForDuplicateBarcodesBDO(inqueue, duplicates);
                    observableCollection.Add(duplicates);
                }
            }
            return observableCollection;
        }

        public bool WriteToBatchQueue(ObservableCollection<ForDuplicatesBarcodesBDO> BarcodesBDO)
        {
            List<RecordsInQueue> recordsInQueueList = new List<RecordsInQueue>();
            foreach (ForDuplicatesBarcodesBDO duplicates in (Collection<ForDuplicatesBarcodesBDO>)BarcodesBDO)
            {
                RecordsInQueue inqueue = new RecordsInQueue();
                ForDuplicateBarcodesBDOToInqueue(inqueue, duplicates);
                recordsInQueueList.Add(inqueue);
            }
            using (CETAPEntities cetapEntities = new CETAPEntities())
            {
                cetapEntities.RecordsInQueues.AddRange((IEnumerable<RecordsInQueue>)recordsInQueueList);
                cetapEntities.SaveChanges();
                return true;
            }
        }


        public ObservableCollection<Log> GetAllErrors()
        {
            ObservableCollection<Log> LogCollect = new ObservableCollection<Log>();
            using (var context = new CETAPEntities())
            {
                var LC = context.Logs.OrderByDescending(x => x.Date).Select(x => x);
                foreach (Log mylog in LC) LogCollect.Add(mylog);
            }

            return LogCollect;
        }

        public ObservableCollection<CompositBDO> GetAllRemoteScoresByIntakeYear(IntakeYearsBDO intakeYear)
        {
            ObservableCollection<CompositBDO> remoteScores = new ObservableCollection<CompositBDO>();

            using (CETAPEntities cetapEntities = new CETAPEntities())
            {
                //   List<TestVenue> venues = cetapEntities.TestVenues.Where(x => x.VenueType == "Remote").ToList();

                //  var Scores = cetapEntities.Composits.Where(x => x.VenueCode >= myYear.yearStart && x.DOT <= myYear.yearEnd).ToList();
                var Scores = cetapEntities.Composits
               .Join(cetapEntities.TestVenues,
                   c => c.VenueCode,
                   v => v.VenueCode,
                   (c, v) => new { Composit = c, v.VenueType })
               .Where(x => (x.VenueType == "Remote" || x.Composit.RefNo.ToString().Substring(7,1) == "9") && x.Composit.VenueCode != 99999 && x.Composit.DOT > intakeYear.yearStart && x.Composit.DOT < intakeYear.yearEnd)
               .Select(x => x.Composit)
                .ToList();

                foreach (Composit score in Scores)
                {
                    CompositBDO comp = new CompositBDO();
                    comp = Maps.CompositDALToCompositBDO(score);
                    remoteScores.Add(comp);
                }
                // remoteScores = new ObservableCollection<CompositBDO>(Scores);

            }
            return remoteScores;
        }

        public void GenerateIndividualReport(CompositBDO selectedWriter)
        {
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxfdHVWR2FZUENwXkc=");

          
            //  string appRootPath = AppDomain.CurrentDomain.BaseDirectory;
           // string sourceFilePath = "template.docx";
            //  string filePath = Path.Combine(appRootPath, "template.docx");
            string mypassword;

            if (selectedWriter.SAID is null)
            {
                mypassword = selectedWriter.ForeignID;
            }
            else
            {
               long password = selectedWriter.SAID ?? 0;
                mypassword = password.ToString("D13");

            }

            var RemotesFolder = ApplicationSettings.Default.RemotesReportsFolder;

            string destinationFileName = selectedWriter.Name.Trim() + "_" + selectedWriter.Surname.Trim() + ".pdf";

            string destinationFilePath = Path.Combine(RemotesFolder, destinationFileName.ToLower());

            if (File.Exists(destinationFilePath))
            {
                // File exists, do something
                File.Delete(destinationFilePath);
            }


           

            string toWhom = "Dear " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(selectedWriter.Name);
            string txtSurname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(selectedWriter.Surname);
            string txtName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(selectedWriter.Name);
            string txtDate = selectedWriter.DOT.ToString("yyyy/MM/dd");
            string txtVenue = selectedWriter.VenueName;
            string txtNbt = selectedWriter.RefNo.ToString();
            string txtSaid = mypassword;

            int? al = selectedWriter.ALScore;
            int? ql = selectedWriter.QLScore;
            int? mat = selectedWriter.MATScore;


            using (PdfDocument doc = new PdfDocument())
            {
                // add  a page
                PdfPage page = doc.Pages.Add();
                PdfGraphics graphics = page.Graphics;
                PointF iconLocation = new PointF(1, 1);
 
                // add text
                PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 16, PdfFontStyle.Bold);
                FileStream imageStream = new FileStream("nbt.png", FileMode.Open, FileAccess.Read);
                PdfBitmap con = new PdfBitmap(imageStream);
                graphics.DrawImage(con, iconLocation.X,iconLocation.Y);

                graphics.DrawString("NBT RESULTS", font,PdfBrushes.Black,iconLocation.X + 200, iconLocation.Y + 130);

                font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                graphics.DrawString(toWhom, font,PdfBrushes.Black,iconLocation.X, iconLocation.Y + 180);


                font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                graphics.DrawString("Herewith Confirmation of your NBT results", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 220); ;

                //create a grid 
                PdfGrid parentGrid = new PdfGrid();
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                PdfGridCellStyle cellStyle1 = new PdfGridCellStyle();
                PdfGridCellStyle cellStyle2 = new PdfGridCellStyle();
                cellStyle.BackgroundBrush = PdfBrushes.Gray;

                //cellStyle.TextBrush = PdfBrushes.White;
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                cellStyle1.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
                cellStyle2.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);

                parentGrid.Columns.Add(4);
                parentGrid.Columns[0].Width = 200f;
                parentGrid.Columns[1].Width = 100f;
                parentGrid.Columns[2].Width = 100f;
                parentGrid.Columns[3].Width = 100f;

                cellStyle1.TextBrush = PdfBrushes.Black;
                cellStyle2.TextBrush = PdfBrushes.Black;
                cellStyle1.BackgroundBrush = PdfBrushes.White;
               // parentGrid.Style = cellStyle;
               // parentGrid.Rows[0].Cells[0].Style = cellStyle;

                parentGrid.Headers.Add(1);
                PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                PdfGridRow header = parentGrid.Headers[0];
                header.Height = 50f;
                header.ApplyStyle(cellStyle);
                // add columns
                PdfGridRow row1 = parentGrid.Rows.Add();
                PdfGridRow row2 = parentGrid.Rows.Add();
                parentGrid.Rows[1].Height = 180f;
                //parentGrid.Rows[1].Height = 25f;
                header.Cells[0].Value = "Details";
                header.Cells[0].StringFormat = stringFormat;
                header.Cells[1].Value = "Academic Literacy (AL Score)";
                header.Cells[1].StringFormat = stringFormat;
                header.Cells[2].Value = "Quantitative Literacy (QL Score)";
                header.Cells[2].StringFormat = stringFormat;
                header.Cells[3].Value = "Mathematics";
                header.Cells[3].StringFormat = stringFormat;

                row2.Cells[0].Style = cellStyle2;
                row2.Cells[1].Value = al.ToString();
                row2.Cells[1].StringFormat = stringFormat;
                row2.Cells[1].Style = cellStyle1;
                //row2.ApplyStyle(cellStyle);
                row2.Cells[2].Value = ql.ToString();
                row2.Cells[2].StringFormat = stringFormat;
                row2.Cells[2].Style = cellStyle1;
                row2.Cells[3].Value = mat != null ? mat.ToString() : "N/A";
                row2.Cells[3].StringFormat = stringFormat;
                row2.Cells[3].Style = cellStyle1;

                //add child grid
                PdfGrid childGrid = new PdfGrid();
                childGrid.Style.BorderOverlapStyle = parentGrid.Style.BorderOverlapStyle;
                //childGrid.Style.BackgroundBrush.Equals(parentGrid.Style.BackgroundBrush);
                childGrid.Columns.Add(1);
                childGrid.Columns.Add(2);
                childGrid.Columns[0].Width = 100f;
                childGrid.Columns[1].Width = 100f;

                // add child rows
                PdfGridRow childRow1 = childGrid.Rows.Add();
                childRow1.Height = 30;
                PdfGridRow childRow2 = childGrid.Rows.Add();
                childRow2.Height = 30;
                PdfGridRow childRow3 = childGrid.Rows.Add();
                childRow3.Height = 30;
                PdfGridRow childRow4 = childGrid.Rows.Add();
                childRow4.Height = 30;
                PdfGridRow childRow5 = childGrid.Rows.Add();
                childRow5.Height = 30;
                PdfGridRow childRow6 = childGrid.Rows.Add();
                childRow6.Height = 30;

                childRow1.Cells[0].Value = "NBT Reference number:";
                childRow1.Cells[1].Value = txtNbt;
                childRow2.Cells[0].Value = "ID/Passport number:";
                childRow2.Cells[1].Value = txtSaid;
                childRow3.Cells[0].Value = "Surname:";
                childRow3.Cells[1].Value = txtSurname;
                childRow4.Cells[0].Value = "First Name:";
                childRow4.Cells[1].Value = txtName;
                childRow5.Cells[0].Value = "Test Date:";
                childRow5.Cells[1].Value = txtDate;
                childRow6.Cells[0].Value = "Venue:";
                childRow6.Cells[1].Value = txtVenue;

                //PDF grid cell
                PdfGridCell cell = childGrid.Rows[0].Cells[0];
                childGrid.Rows[0].Cells[1].StringFormat = stringFormat;
                childGrid.Rows[1].Cells[0].StringFormat = stringFormat;
                childGrid.Rows[1].Cells[1].StringFormat = stringFormat;
                childGrid.Rows[2].Cells[0].StringFormat = stringFormat;
                childGrid.Rows[2].Cells[1].StringFormat = stringFormat;
                childGrid.Rows[3].Cells[0].StringFormat = stringFormat;
                childGrid.Rows[3].Cells[1].StringFormat = stringFormat;
                childGrid.Rows[4].Cells[0].StringFormat = stringFormat;
                childGrid.Rows[4].Cells[1].StringFormat = stringFormat;
                childGrid.Rows[5].Cells[0].StringFormat = stringFormat;
                childGrid.Rows[5].Cells[1].StringFormat = stringFormat;

                //Set cell style.
                //cell.Style.BackgroundBrush = new PdfSolidBrush(Color.SkyBlue);

                cell.StringFormat = stringFormat;
                //childGrid.Rows[0].Cells[0].Style = cellStyle;

                parentGrid.Rows[1].Cells[0].Value = childGrid;
                parentGrid.Rows[1].Cells[0].StringFormat = stringFormat;
                //parentGrid.Rows[1].Cells[2].Value = al.ToString();
                parentGrid.Rows[1].Cells[0].Style.CellPadding = new PdfPaddings(0, 0, 0, 0);
                //parentGrid.Rows[1].Cells[0].Style.Borders = new PdfBorders(PdfBorderType.None);


                parentGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable5DarkAccent5);
                PdfGridStyle gridStyle = new PdfGridStyle();
                parentGrid.Style = gridStyle;

                childGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable2Accent4);
                PdfGridStyle gridStyle1 = new PdfGridStyle();
                childGrid.Style = gridStyle1;

                //draw parent grid
                parentGrid.Draw(page,iconLocation.X,iconLocation.Y + 240);

                font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                graphics.DrawString("Centre for Educational Accessments (CEA) ", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 550);
                graphics.DrawString("Level 4, Hoerikwaggo building, ", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 565);
                graphics.DrawString("North Lane, Upper Campus ", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 580);
                graphics.DrawString("University of Cape Town, Private Bag ", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 595);
                graphics.DrawString("Rondebosch 7701, South Africa ", font, PdfBrushes.Black, iconLocation.X, iconLocation.Y + 610);


                font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
                graphics.DrawString("Helpdesk contact number is (021) 650-3523 and email is nbt@uct.ac.za", font,PdfBrushes.Black, iconLocation.X, iconLocation.Y + 660);
                

                FileStream imageStream1 = new FileStream("cea.png", FileMode.Open, FileAccess.Read);
                PdfBitmap con1 = new PdfBitmap(imageStream1);
                graphics.DrawImage(con1, iconLocation.X, iconLocation.Y + 680);


                //Document security.
                PdfSecurity security = doc.Security;
                //Specifies key size and encryption algorithm.
                security.KeySize = PdfEncryptionKeySize.Key128Bit;
                security.Algorithm = PdfEncryptionAlgorithm.RC4;
                security.OwnerPassword = "nbt";
                //It allows printing and accessibility copy content.
                security.Permissions = PdfPermissionsFlags.Print | PdfPermissionsFlags.AccessibilityCopyContent;
                security.UserPassword = mypassword;


                // save document
                MemoryStream stream = new MemoryStream();
                doc.Save(stream);
                doc.Close(true);

                stream.Position = 0;
                File.WriteAllBytes(destinationFilePath, stream.ToArray());
            }



            // SizeF clientSize = page.GetClientSize();

            //             var y = clientSize.Height;
            //            var x = clientSize.Width;

            //SizeF iconSize = new SizeF(con.Width, con.Height);
            //            PdfImage icon = new PdfBitmap(imageStream);
            //            SizeF iconSize = new SizeF(icon.Width, icon.Height);

            // draw the image in the page
            //PdfGraphics graphics = page.Graphics;
            //PointF iconLocation = new PointF(1, 1);
            //PointF textLocation = new PointF(10, 10);
            //graphics.DrawImage(icon, 1,1);
            ////icon.Draw(page, new PointF(10, 10));






            ////text.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            ////result = text.Draw(page, new PointF(clientSize.Width - 25, iconLocation.Y + 10));
            ////create a grid 
            //PdfGrid parentGrid = new PdfGrid();
            //PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            //PdfGridCellStyle cellStyle1 = new PdfGridCellStyle();
            //cellStyle.BackgroundBrush = PdfBrushes.Gray;
            ////cellStyle.TextBrush = PdfBrushes.White;
            //cellStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10,PdfFontStyle.Bold);
            //cellStyle1.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
 



           

            

































            //FileStream imageStream1 = new FileStream("cea.png", FileMode.Open, FileAccess.Read);
            //PdfImage icon1 = new PdfBitmap(imageStream1);
            //SizeF iconSize1 = new SizeF(icon1.Width, icon1.Height);

            //// draw the image in the page
            //PdfGraphics graphics1 = page.Graphics;
            //PointF iconLocation1 = new PointF(iconLocation.X, iconLocation.Y + 460);
            //graphics1.DrawImage(icon1, iconLocation1);

            ////Document security.
            //PdfSecurity security = doc.Security;
            ////Specifies key size and encryption algorithm.
            //security.KeySize = PdfEncryptionKeySize.Key128Bit;
            //security.Algorithm = PdfEncryptionAlgorithm.RC4;
            //security.OwnerPassword = "nbt";
            ////It allows printing and accessibility copy content.
            //security.Permissions = PdfPermissionsFlags.Print | PdfPermissionsFlags.AccessibilityCopyContent;
            //security.UserPassword = password;



            //        hasvalues = true;
            //        stream.Position = 0;
            //        File.WriteAllBytes(destinationFilePath, stream.ToArray()); 


            //            //File.Copy(sourceFilePath, destinationFilePath, true);

            //            //using (WordprocessingDocument sourceDoc = WordprocessingDocument.Open(sourceFilePath, true))
            //            //{
            //            //    using (WordprocessingDocument copiedDoc = WordprocessingDocument.Open(destinationFilePath, true))
            //            //    {
            //            //        // Replace text in the main document part
            //            //        ReplaceText(copiedDoc, "Bianca", selectedWriter.Name); // use name from database

            //            //        // Access the first table in the document (you may need to adjust this based on your document structure)
            //            //        Table table = copiedDoc.MainDocumentPart.Document.Body.Elements<Table>().FirstOrDefault();

            //            //        if (table != null)
            //            //        {
            //            //            // Replace values in certain cells of the table
            //            //            ReplaceTableCellText(table, "93100", selectedWriter.RefNo.ToString(), 1, 1);//NBT number
            //            //            ReplaceTableCellText(table, "ILNE", selectedWriter.Name, 4, 1); // First Name
            //            //            ReplaceTableCellText(table, "DU TOIT", selectedWriter.Surname, 3, 1); // Surname
            //            //            ReplaceTableCellText(table, "ST PATRICKS COLLEGE", selectedWriter.VenueName, 6, 1); // Venue
            //            //            ReplaceTableCellText(table, "DOT", selectedWriter.DOT.ToString("yyyy/MM/dd"), 5, 1); // "DateTime.Now.ToString("yyyy/MM/dd"), 5,1); // Date of Test
            //            //            ReplaceTableCellText(table, "9902190245082", mypassword, 2, 1); // National Id or Passport Number
            //            //            ReplaceTableCellText(table, "75", Convert.ToString(selectedWriter.ALScore), 1, 2); // AL
            //            //            ReplaceTableCellText(table, "70", Convert.ToString(selectedWriter.QLScore), 1, 3); // QL  
            //            //            ReplaceTableCellText(table, "61", Convert.ToString(selectedWriter.MATScore), 1, 4); // Math
            //            //                                                                                                // Add more ReplaceTableCellText calls as needed


            //            //            // Save the modified document
            //            //            copiedDoc.Save();


            //            //        }
            //            //    }
            //            //    //// Initialize Word Application
            //            //    //System.Windows.Application wordApp = new Application();

            //            //    //// Open the Word document
            //            //    //Document doc = wordApp.Documents.Open(@"C:\Path\To\Your\WordDocument.docx");

            //            //    //// Specify the output PDF file path
            //            //    //object outputFileName = @"C:\Path\To\Your\Output\PDF\Document.pdf";
            //            //    //object fileFormat = WdSaveFormat.wdFormatPDF;

            //            //    //// Save the document as PDF
            //            //    //doc.SaveAs(ref outputFileName, ref fileFormat);

            //            //    //// Close the Word document
            //            //    //doc.Close();

            //            //    //Document forPdf = new Document();

            //            //    //string fname = Path.GetFileNameWithoutExtension(destinationFilePath);
            //            //    ////Load a sample Word document

            //            //    //string fnamepdf = fname + ".pdf";

            //            //    //forPdf.LoadFromFile(destinationFilePath);

            //            //    //string pdfFolder = Path.Combine(RemotesFolder, fnamepdf);
            //            //    //if (File.Exists(pdfFolder))
            //            //    //{
            //            //    //    // File exists, do something
            //            //    //    File.Delete(pdfFolder);
            //            //    //}

            //            //    ////Create a ToPdfParameterList instance
            //            //    //ToPdfParameterList parameters = new ToPdfParameterList();

            //            //    ////Set open password and permission password for PDF
            //            //    //string openPsd = "CEA-nbt";
            //            //    //string permissionPsd = mypassword;
            //            //    //parameters.PdfSecurity.Encrypt(openPsd, permissionPsd, PdfPermissionsFlags.Default, PdfEncryptionKeySize.Key128Bit);

            //            //    ////Save the Word document to PDF with password

            //            //    //forPdf.SaveToFile(pdfFolder, parameters);
            //            //}

            //           // return hasvalues;
        }
        //        // Replace text in the document
        //        //private static void ReplaceText(WordprocessingDocument doc, string oldValue, string newValue)
        //        //{
        //        //    foreach (var textElement in doc.MainDocumentPart.Document.Descendants<Text>())
        //        //    {
        //        //        if (textElement.Text.Contains(oldValue))
        //        //        {
        //        //            textElement.Text = textElement.Text.Replace(oldValue, newValue);
        //        //        }
        //        //    }
        //        //}

        //        // Replace text in a specific cell of the table
        //        //private static void ReplaceTableCellText(Table table, string oldValue, string newValue, int rowIndex, int colIndex)
        //        //{
        //        //    var cell = table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAtOrDefault(rowIndex)?.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAtOrDefault(colIndex);

        //        //    if (cell != null)
        //        //    {
        //        //        foreach (var textElement in cell.Descendants<Text>())
        //        //        {
        //        //            if (textElement.Text.Contains(oldValue))
        //        //            {
        //        //                textElement.Text = textElement.Text.Replace(oldValue, newValue);
        //        //            }
        //        //        }
        //        //    }
        //        //}

    }
}
