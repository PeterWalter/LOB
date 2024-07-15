
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Model.scoring;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using CETAP_LOB.Database;
using System.Windows.Controls;

namespace CETAP_LOB.ViewModel.Utilities
{
    public class OnlineViewModel : ViewModelBase
    {
        public const string StatusPropertyName = "Status";
        public const string CompositPropertyName = "Composit";
        public const string DBCompositPropertyName = "DBComposit";
        public const string testDatePropertyName = "testDate";
        public const string BIOPropertyName = "BIO";
        public const string MATPropertyName = "MAT";
        public const string AQLPropertyName = "AQL";
        public const string FilesInFolderPropertyName = "FilesInFolder";
        private IDataService _service;
        private bool IsmatchScores;
        private string _myStatus;
        private string _testDate;
        private ObservableCollection<CompositBDO> _composit;
        private ObservableCollection<OnlineCompositBDO> _dbcomposit;
        private ObservableCollection<AnswerSheetBio> _bio;
        private ObservableCollection<MAT_Score> _mat;
        private ObservableCollection<AQL_Score> _aql;
        private ObservableCollection<string> _myfiles;
        private ObservableCollection<BenchMarkLevelsBDO> _levels;

        public RelayCommand SelectFolderCommand { get; private set; }

        public RelayCommand ReadFilesCommand { get; private set; }

        public RelayCommand ProcessScoresCommand { get; private set; }

        public RelayCommand GenerateCompositeCommand { get; private set; }

        public RelayCommand GenerateFromDBCommand { get; private set; }

        public ObservableCollection<MAT_Score> MAT
        {
            get
            {
                return _mat;
            }
            set
            {
                if (_mat == value)
                    return;
                _mat = value;
                RaisePropertyChanged("MAT");
            }
        }

        public ObservableCollection<AQL_Score> AQL
        {
            get
            {
                return _aql;
            }
            set
            {
                if (_aql == value)
                    return;
                _aql = value;
                RaisePropertyChanged("AQL");
            }
        }

        public ObservableCollection<CompositBDO> Composit
        {
            get
            {
                return _composit;
            }
            set
            {
                if (_composit == value)
                    return;
                _composit = value;
                RaisePropertyChanged("Composit");
            }
        }
        public ObservableCollection<AnswerSheetBio> BIO
        {
            get
            {
                return _bio;
            }
            set
            {
                if (_bio == value)
                    return;
                _bio = value;
                RaisePropertyChanged("BIO");
            }
        }

        public ObservableCollection<string> FilesInFolder
        {
            get
            {
                return _myfiles;
            }
            set
            {
                if (_myfiles == value)
                    return;
                _myfiles = value;
                RaisePropertyChanged("FilesInFolder");
            }
        }

        public string testDate 
        {
            get
            {
                return _testDate;
            }
            set
            {
                if (_testDate == value)
                    return;
                _testDate = value;
                RaisePropertyChanged(testDate);
            }
        }
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                RaisePropertyChanged("SelectedDate");
                // Notify property changed if you're using MVVM pattern
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

        public OnlineViewModel(IDataService Service)
        {
            _service = Service;
            InitializeModels();
            RegisterCommands();
        }
        public ObservableCollection<OnlineCompositBDO> DBComposit
        {
            get
            {
                return _dbcomposit;
            }
            set
            {
                if (_dbcomposit == value)
                    return;
                _dbcomposit = value;
                RaisePropertyChanged("Composit");
            }
        }

        private void InitializeModels()
        {
            FilesInFolder = new ObservableCollection<string>();
            DBComposit = new ObservableCollection<OnlineCompositBDO>();
            // Composit = new ObservableCollection<CompositBDO>();
        }
        private void RegisterCommands()
        {
            SelectFolderCommand = new RelayCommand((selectFolder));
            ReadFilesCommand = new RelayCommand((readScoreFiles));
            ProcessScoresCommand = new RelayCommand(() => ProcessScores(), () => CanprocessScores());
            GenerateCompositeCommand = new RelayCommand(() => GenerateComposite());
            GenerateFromDBCommand = new RelayCommand((CompileScores));
            //  TrackScoresCommand = new RelayCommand(() => TrackScores(), () => canTrack());
        }

        private void CompileScores()
        {
            if (_dbcomposit != null)
            {
                _dbcomposit.Clear();
            }
             if(testDate == null)
            {
                string text = "Please enter test Date";
                int num = (int)ModernDialog.ShowMessage(text, "OnlineComposite", MessageBoxButton.OK, (Window)null);
                return;
            }
            string userInput = testDate;//"2024-06-22";
            if (DateTime.TryParse(userInput, out DateTime date))
            {
                SelectedDate = date;
                _dbcomposit = _service.DBOnlineScores(date);
            }
            else
            {
                SelectedDate = null;
            }
            
        }
        private void selectFolder()
        {
            FilesInFolder.Clear();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = ApplicationSettings.Default.FilesForScoring;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            string selectedPath = folderBrowserDialog.SelectedPath;
            FilesInFolder = _service.ListScoreFiles(selectedPath);
            ApplicationSettings.Default.FilesForScoring = selectedPath;
            ApplicationSettings.Default.Save();
        }

        private void readScoreFiles()
        {
            AQL = _service.GetAQL();
            MAT = _service.GetMat();
            BIO = _service.GetBIO();
            IsmatchScores = true;
            MatchScores();
        }
        private void ProcessScores()
        {
            MatchScores();
        }

        private bool CanprocessScores()
        {
            return false;
        }
        private void MatchScores()
        {
            if (!IsmatchScores)
                return;
            Composit = _service.MatchOnlineScores();
        }
        private void GenerateComposite()
        {
            string text;
            if (_service.GenerateCompositeForSession())
            {
                text = "Composite files have been generated !!";
                int num = (int)ModernDialog.ShowMessage(text, "Composite", MessageBoxButton.OK, (Window)null);
            }
            else
            {
                text = "Composite not created";
                int num = (int)ModernDialog.ShowMessage(text, "Composite", MessageBoxButton.OK, (Window)null);
            }
            Status = text;
        }

        private bool CanGenerateComposite()
        {
            return Composit.Count > 0;
        }
    }
}
