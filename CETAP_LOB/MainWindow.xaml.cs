using CETAP_LOB.Model;
using CETAP_LOB.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using FirstFloor.ModernUI.Windows.Controls;
using System.CodeDom.Compiler;
using System;

namespace CETAP_LOB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private IDataService _service { get; set; }
       // private bool _contentLoaded;

        public MainWindow()
        {
            
            Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
            IDataService Service = new DataService();
            _service = Service;

            string message = "";
            bool isDB = _service.CheckForDatabase(ref message);

            SettingsAppearanceViewModel settings = new SettingsAppearanceViewModel();
            settings.SetThemeAndColor(ApplicationSettings.Default.SelectedThemeDisplayName, ApplicationSettings.Default.SelectedThemeSource, ApplicationSettings.Default.SelectedAccentColor, ApplicationSettings.Default.SelectedFontSize);

            ScanSettingsViewModel scanset = new ScanSettingsViewModel();
            scanset.SetScanSettings(ApplicationSettings.Default.ScanningFolder, ApplicationSettings.Default.EditingFolder, ApplicationSettings.Default.QAFolder, ApplicationSettings.Default.IntakeYear);


            ScanSettingsViewModel ScoringSet = new ScanSettingsViewModel();
            ScoringSet.SetScoringSettings(ApplicationSettings.Default.ScoreFolder, ApplicationSettings.Default.ScoreModerationFolder, ApplicationSettings.Default.FilesForScoring, ApplicationSettings.Default.ModerationFilesForScoring);
            if (!isDB)
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBox.Show(message, "Database Availability", btn);
            }
            ApplicationSettings.Default.DBAvailable = isDB;
            ApplicationSettings.Default.Save();

          //  Closing += (s, e) => ViewModelLocator.Cleanup();
        }

     
    }
}
