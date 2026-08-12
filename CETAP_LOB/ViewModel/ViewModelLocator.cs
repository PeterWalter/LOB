/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CETAP_LOB"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/
using System.Data.Entity.Spatial;
using CETAP_LOB.Design;
using CETAP_LOB.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using CETAP_LOB.ViewModel.easypay;
using CETAP_LOB.ViewModel.processing;
using CETAP_LOB.ViewModel.scoring;
using CETAP_LOB.ViewModel.writers;
using CETAP_LOB.ViewModel.Introduction;
using CETAP_LOB.ViewModel.composite;
using CETAP_LOB.ViewModel.Utilities;
using Microsoft.SqlServer.Types;
using CETAP_LOB.ViewModel;

namespace CETAP_LOB.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EasyPayViewModel>();
            SimpleIoc.Default.Register<VenuesViewModel>();
            SimpleIoc.Default.Register<VenueViewModel>();
            SimpleIoc.Default.Register<EditingViewModel>();
            SimpleIoc.Default.Register<QAViewModel>();
            SimpleIoc.Default.Register<BatchViewModel>();
            SimpleIoc.Default.Register<SettingsAppearanceViewModel>();
            SimpleIoc.Default.Register<ScanSettingsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<ScoringViewModel>();
            SimpleIoc.Default.Register<LoadWritersViewModel>();
            SimpleIoc.Default.Register<AnswerSheetBioViewModel>();
            SimpleIoc.Default.Register<ProcessViewModel>();
            SimpleIoc.Default.Register<MatViewModel>();
            SimpleIoc.Default.Register<AQLViewModel>();
            SimpleIoc.Default.Register<ResponseMatrixViewModel>();
            SimpleIoc.Default.Register<TestsViewModel>();
            SimpleIoc.Default.Register<TestAllocationViewModel>();
            SimpleIoc.Default.Register<TestProfileViewModel>();
            SimpleIoc.Default.Register<NewVenueViewModel>();
            SimpleIoc.Default.Register<VenueMapViewModel>();
            SimpleIoc.Default.Register<ModerationViewModel>();
            SimpleIoc.Default.Register<IntroductionViewModel>();
            SimpleIoc.Default.Register<CompositeViewModel>();
            SimpleIoc.Default.Register<RemotesViewModel>();
            SimpleIoc.Default.Register<ScanTrackerViewModel>();
            SimpleIoc.Default.Register<FakeEasyPayViewModel>();
            SimpleIoc.Default.Register<EditCompositeViewModel>();
            SimpleIoc.Default.Register<ErrorViewModel>();
            SimpleIoc.Default.Register<OnlineViewModel>();
            SimpleIoc.Default.Register<SubdomainsViewModel>();
            SimpleIoc.Default.Register<BarcodesViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }


        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        /// 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
       "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public ErrorViewModel Errors
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ErrorViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
       "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public EditCompositeViewModel CompositEdit
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditCompositeViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
       "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public FakeEasyPayViewModel FakeEasyPay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FakeEasyPayViewModel>();
            }
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public ScanTrackerViewModel Tracks
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ScanTrackerViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public CompositeViewModel Composite
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CompositeViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public RemotesViewModel Remotes
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RemotesViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public IntroductionViewModel Introduction
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IntroductionViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public ModerationViewModel Moderation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ModerationViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public QAViewModel QA
        {
            get
            {
                return ServiceLocator.Current.GetInstance<QAViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public BatchViewModel Batch
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BatchViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public ScanSettingsViewModel ScanSettings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ScanSettingsViewModel>();
            }
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public EditingViewModel Editing
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditingViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public VenueMapViewModel VenueMap
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VenueMapViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public NewVenueViewModel NewVenue
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewVenueViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public TestProfileViewModel TestProfile
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TestProfileViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public TestAllocationViewModel TestAllocation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TestAllocationViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public TestsViewModel Tests
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TestsViewModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AQLViewModel AQLScores
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AQLViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public MatViewModel MatScores
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MatViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public AnswerSheetBioViewModel AnswerSheetBio
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnswerSheetBioViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public EasyPayViewModel EasyPay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EasyPayViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public ResponseMatrixViewModel ResponseMatrix
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ResponseMatrixViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public ScoringViewModel Scoring
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ScoringViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public ProcessViewModel Process
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProcessViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public LoadWritersViewModel LoadWriters
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoadWritersViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public VenuesViewModel Venues
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VenuesViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public VenueViewModel Venue
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VenueViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsAppearanceViewModel SettingsAppearance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsAppearanceViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
          "CA1822:MarkMembersAsStatic",
          Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public OnlineViewModel Online
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OnlineViewModel>();
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public SubdomainsViewModel Subdomain
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SubdomainsViewModel>();
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1822:MarkMembersAsStatic",
         Justification = "This non-static member is needed for data binding purposes.")]
        public BarcodesViewModel Barcode
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BarcodesViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}