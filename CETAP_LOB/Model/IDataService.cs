using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Model.QA;
using CETAP_LOB.Model.scoring;
using CETAP_LOB.Model.venueprep;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model
{
    public interface IDataService
    {
        // void RemoveChar();
        // Database connection checks
        bool CheckForDatabase(ref string message);
        List<UserBDO> GetAllUsers();

        //writer prep
        ObservableCollection<WebWriters> GetData(string data);
        ObservableCollection<WebWriters> Processdata();
        void generateFile(string Filename);
        bool DeleteWriterfromList(WebWriters writer);
        List<WritersBDO> GetAllwriters();
        Task<bool> addwriterToDBAsync();
        ObservableCollection<WebWriters> GetDuplicates();
        Task<ObservableCollection<WebWriters>> GetDuplicatesfromDBAsync();
        bool GetNewNBT(WebWriters writer, ref string NewNBT);
        void CleanFunnyChars();

        //Venues
        VenueBDO GetTestVenue(int venuecode);
        bool updateTestVenue(VenueBDO testVenue, ref string message);
        bool deleteTestVenue(int venueCode, ref string message);
        bool addTestVenue(VenueBDO venueBDO, ref string message);
        VenueBDO GetTestVenueByWebSiteName(string websiteName);
        List<VenueBDO> GetAllvenues();
        string GetWebSiteNameByVenueCode(int venueCode);

        // test Books
        List<TestBDO> GetAllTests();
        string getTestName(int TestID);
        TestBDO getTestByName(string name);
        TestBDO getTestByID(int testID);
        bool updateTest(TestBDO testBDO, ref string message);
        bool deleteTest(TestBDO test, ref string message);
        bool addTest(TestBDO testBDO, ref string message);
        List<TestBDO> GetTestFromDatFile(datFileAttributes batchfile, IntakeYearsBDO intake);

        //Test Allocation
        List<TestAllocationBDO> GetAllTestAllocations();
        List<TestAllocationBDO> getTestAllocationByClient(string name);
        List<TestAllocationBDO> getTestAllocationByTestID(int testID);
        TestAllocationBDO getTestAllocationByID(int ID);
        bool AllocationsToExcel(string filename);
        bool updateTestAllocation(TestAllocationBDO testAllocationBDO, ref string message);
        bool deleteTestAllocation(TestAllocationBDO testAllocationBDO, ref string message);
        bool addTestAllocation(TestAllocationBDO testAllocationBDO, ref string message);

        //TestProfile
        void SaveProfileAllocationsToExcel(string filename);
        List<ProfileAllocationBDO> GetAllProfileAllocations();
        List<ProfileAllocationBDO> GetProfileAllocationsByDate(DateTime date);
        List<TestProfileBDO> GetAllTestProfiles();
        List<TestProfileBDO> getTestprofileByAllocationID(int allocationID);
        List<ProfileAllocationBDO> GetProfileAllocationByProfile(int Profile);
        List<TestProfileBDO> getTestprofileByProfile(int Profile);
        TestProfileBDO getTestProfileByProfileID(int profileID);
        bool updateTestProfile(TestProfileBDO profileBDO, ref string message);
        bool deleteTestProfile(TestProfileBDO profileBDO, ref string message);
        bool addTestProfile(TestProfileBDO profileBDO, ref string message);

        List<TestBDO> GetTestfromTestProfile(int profile);
        //List<TestBDO> GetTestFromDatFile(datFileAttributes batchfile);

        //Batch file
        List<BatchBDO> GetAllbatches();
        List<BatchBDO> getbatchesByVenue(int venueID);
        List<BatchBDO> getbatchesByDate(DateTime date);
        bool addBatch(BatchBDO batchBDO, ref string message);
        bool updatebatch(BatchBDO batchBDO, ref string message);
        bool deleteBatch(BatchBDO batch, ref string message);
        string getBatchRandomNumber();

        //ScanTracker
        bool SaveFileToDB(ScannedFileBDO scannedFileBDO, ref string message); // for saving scanned files to database
        void ReadScanfromDB(string Filename, string fileFolder);
        ScannedFileBDO GetScannedFile(string Filename);
        List<ScanTrackerBDO> GetAllTracks();

        //Provinces
        List<ProvinceBDO> getAllProvinces();
        ProvinceBDO getProvinceByID(int code);
        ProvinceBDO getProvinceByName(string name);

        //Scoring
        ObservableCollection<string> ListScoreFiles(string folder);
        ObservableCollection<AnswerSheetBio> LoadAnswerSheet();
        ObservableCollection<AQL_Score> LoadAQLScores();
        ObservableCollection<MAT_Score> LoadMATScores();
        ObservableCollection<AnswerSheetBio> GetBIO();
        ObservableCollection<AQL_Score> GetAQL();
        ObservableCollection<MAT_Score> GetMat();
        ObservableCollection<CompositBDO> MatchScores();
        ObservableCollection<ScoreStats> GetAQLStats();
        ObservableCollection<ScoreStats> GetMATStats();
        bool GenerateComposite();
        bool RecordsTrackedScores(string Batch, int amount);

        // Composit
        Task<List<CompositBDO>> GetAllNBTScoresAsync(int page, int size, IntakeYearsBDO yr);
        int GetCompositCount(IntakeYearsBDO yr);
        CompositBDO getResultsByName(string name);
        CompositBDO getResultsBySurName(string surname);
        CompositBDO getResultsByID(Int64 SAID);
        CompositBDO getResultsByFID(string FID);
        CompositBDO getResultsByDOB(DateTime DOB);
        CompositBDO getResultsByNBT(Int64 NBT);
        bool updateComposit(CompositBDO results, ref string message);
        bool deleteComposit(CompositBDO results, ref string message);
        Task<bool> addComposit(CompositBDO results);
        //bool GenerateCompositFromDB(string path);
        //Task<bool> LogisticCompositeFromDB(string folder);
        void GetNoMatchFile(string filename);
        bool GenerateSelectedComposite(ObservableCollection<CompositBDO> mySelection, string folder);
        bool RemoveRecordsInQueue(List<long> ScoredRecords);

        //easypay
        ObservableCollection<EasypayFile> ListFTPFiles();
        Task DownloadfileAsync(string filename, string localfile);
        Task WriteFilesToDBAsync(string folder);
        bool LoadEasyPayToDB(string filename, ref string message);
        EasyPayFile ReadLastFile();
        Task<ObservableCollection<Vw_EasyPayRecords>> GetEasyPayRecords(DateTime startDate, DateTime endDate);

        //NewNBTNumbers
        NewNBTNumberBDO GetNewNBTNumberFromDB();
        bool UpdateNBTNumbers(NewNBTNumberBDO myNBT, ref string message);

        // QA Data
        void LoadTestDate(DateTime TheTestDate);
        void ReadEndofDatFile();
        ObservableCollection<QADatRecord> GetQADataFromFile(datFileAttributes filename);
        QADatRecord GetNBTNumberFromDBbySAID(QADatRecord SelectedRecord);
        QADatRecord GetNBTNumberFromDBbyFID(QADatRecord SelectedRecord);
        bool SaveQADatFile(datFileAttributes datfile, ref string message);
        bool AutoClean();
        QADatRecord GetSAIDbyNBT(QADatRecord SelectedRecord);
        QADatRecord GetFIDbyNBT(QADatRecord SelectedRecord);
        QADatRecord GetNamebyNBT(QADatRecord SelectedRecord);
        QADatRecord GetSurnamebyNBT(QADatRecord SelectedRecord);
        QADatRecord GetDOBfromDB(QADatRecord SelectedRecord);
        ObservableCollection<QADatRecord> GetQARecords();

        void SaveRawCSXData();
        bool updateQAtoTracker(string batch, int Count);
        bool AddSurnameToList(string surname);
        bool AddNameToList(string name);

        //Moderation data
        ObservableCollection<CompositBDO> GetAllScores(string path);
        ObservableCollection<CompositBDO> GetAllModeratedScores(string path);
        ObservableCollection<Section7> getSec7DatFile(datFileAttributes datfile);


        // Other utilities
        ObservableCollection<IntakeYear> GetIntakes();
        int GetCurrentYear();
        IntakeYearsBDO GetIntakeRecord(int year);
        List<IntakeYearsBDO> GetAllIntakeYears();
        ObservableCollection<Log> GetAllErrors();
        bool WriteToBatchQueue(ObservableCollection<ForDuplicatesBarcodesBDO> BarcodesBDO);
        ObservableCollection<ForDuplicatesBarcodesBDO> GetBatchesInQueue();
        bool DuplicateReportGeneration(List<ForDuplicatesBarcodesBDO> Duplicates, string filename);
        List<ForDuplicatesBarcodesBDO> FindDuplicatesFromDB(ObservableCollection<ForDuplicatesBarcodesBDO> BatchRecords);
    }
}
