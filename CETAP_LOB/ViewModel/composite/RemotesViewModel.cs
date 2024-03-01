
using FeserWard.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CETAP_LOB.ViewModel.composite
{
    public class RemotesViewModel : ViewModelBase
    {
        public const string TotalRecPropertyName = "TotalRec";
        public const string IntakeYearPropertyName = "IntakeYear";
        public const string PeriodsPropertyName = "Periods";
        //public const string PagesPropertyName = "Pages";
        //public const string PagePropertyName = "Page";
        public const string SelectedWritersPropertyName = "SelectedWriters";
        public const string ScoreFolderPropertyName = "ScoreFolder";
        public const string SelectedWriterPropertyName = "SelectedWriter";
        public const string WritersCompositPropertyName = "WritersComposit";
        public const string Composit1PropertyName = "Composit1";
        public const string NBTScoresPropertyName = "NBTScores";

        //     public string RemotesFolder;
        //private const int RecordsLength = 5000;
        //private int recPage;
        private int _totalRec;
        private IntakeYearsBDO _IntakeYear;
        private List<IntakeYearsBDO> _periods;
        //private int _pages;
        //private int _page;
        private ObservableCollection<CompositBDO> _mySelectedWriters;
        private string _remotesFolder;
        private CompositBDO _mywriter;
        private ObservableCollection<CompositBDO> _myWriters;
        private ObservableCollection<CompositBDO> _mycomposit;
        private ObservableCollection<CompositBDO> _myscores;
        private IDataService _service;

        public RelayCommand LoadNBTCommand { get; private set; }

        //public RelayCommand LoadUpdatesCommand { get; private set; }

        public RelayCommand GetIDCommand { get; private set; }

        public RelayCommand GetDOBCommand { get; private set; }

        //public RelayCommand AutoCleanCommand { get; private set; }

        //public RelayCommand PreviousCommand { get; private set; }

        public RelayCommand GenerateFileCompositeCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand SaveDatFileCommand { get; private set; }

        //public RelayCommand NextCommand { get; private set; }

        public RelayCommand IndividualReportCommand { get; private set; }

        public RelayCommand<ObservableCollection<CompositBDO>> SelectionChangedCommand { get; private set; }

        public IIntelliboxResultsProvider ResultsProvider { get; private set; }

        public int TotalRec
        {
            get
            {
                return _totalRec;
            }
            set
            {
                if (_totalRec == value)
                    return;
                _totalRec = value;
                RaisePropertyChanged("TotalRec");
            }
        }

        public IntakeYearsBDO IntakeYear
        {
            get
            {
                return _IntakeYear;
            }
            set
            {
                if (_IntakeYear == value)
                    return;
                _IntakeYear = value;
                RaisePropertyChanged("IntakeYear");
            }
        }


        public string RemotesFolder
        {
            get
            {
                return _remotesFolder;
            }
            set
            {
                if (_remotesFolder == value)
                    return;
                _remotesFolder = value;
                RaisePropertyChanged("ScoreFolder");
            }
        }

        public CompositBDO SelectedWriter
        {
            get
            {
                return _mywriter;
            }
            set
            {
                if (_mywriter == value)
                    return;
                _mywriter = value;
                if (_mywriter != null)
                    RaisePropertyChanged("SelectedWriter");
            }
        }

        public ObservableCollection<CompositBDO> Composit1
        {
            get
            {
                return _mycomposit;
            }
            set
            {
                if (_mycomposit == value)
                    return;
                _mycomposit = value;
                RaisePropertyChanged("Composit1");
            }
        }

        public ObservableCollection<CompositBDO> NBTScores
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
                RaisePropertyChanged("NBTScores");
            }
        }

        public RemotesViewModel(IDataService Service)
        {
            _service = Service;
            InitializeModels();
            RegisterCommands();
        }

        private async void InitializeModels()
        {
            IntakeYear = _service.GetIntakeRecord(ApplicationSettings.Default.IntakeYear);

            //  TotalRec = _service.GetCompositCount(IntakeYear);
            // ResultsProvider = (IIntelliboxResultsProvider) new CompositResultsProvider(_service);
            //    TotalRec = _service.GetCompositCount(IntakeYear);
            // int pp = TotalRec;
            Composit1 = new ObservableCollection<CompositBDO>();
            RemotesFolder = ApplicationSettings.Default.RemotesReportsFolder;
            Composit1 = _service.GetAllRemoteScoresByIntakeYear(IntakeYear);
        }

        private void RegisterCommands()
        {
            IndividualReportCommand = new RelayCommand((Action)(() => GenerateIndividualReport()), (Func<bool>)(() => canGenerate()));
            SelectionChangedCommand = new RelayCommand<ObservableCollection<CompositBDO>>((Action<ObservableCollection<CompositBDO>>)(SelectedWriters =>
            {
                if (SelectedWriters == null)
                    return;
                Composit1 = SelectedWriters;
            }));
        }

        //private void GenerateCompositeSelectedRecords()
        //{
        //  bool flag = false;
        //  string folder = "";
        //  FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        //  if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        //    folder = folderBrowserDialog.SelectedPath;
        //  if (!string.IsNullOrWhiteSpace(folder))
        //    flag = _service.GenerateSelectedComposite(SelectedWriters, folder);
        //  if (!flag)
        //    return;
        //  ModernDialog.ShowMessage("Composite of Selected Records created", " Selected Records!!!", MessageBoxButton.OK);
        //}


        private void GenerateIndividualReport()
        {
            if (canGenerate())
            {
                _service.GenerateIndividualReport(SelectedWriter);
                SelectedWriter = null;
                ModernDialog.ShowMessage("Word Document report has been generated", " Selected Record!!!", MessageBoxButton.OK);
            }
        }

        private bool canGenerate()
        {
            bool flag = false;
            if (SelectedWriter != null)
                flag = true;
            return flag;
        }


    }
}