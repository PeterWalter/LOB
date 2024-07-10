using GalaSoft.MvvmLight;
using FirstFloor.ModernUI.Presentation;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using System;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;

namespace CETAP_LOB.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
    public const string TitleLinkPropertyName = "TitleLink";
    public const string MenuLinkGroupsPropertyName = "MenuLinkGroups";
    public const string LinksPropertyName = "Links";
    public const string UserPropertyName = "User";
    private LinkCollection _mytitleLinks;
    private LinkGroupCollection _mymenulinks;
    private LinkCollection _mylinks;
    private UserBDO _myuser;
    private IDataService _service;
    private string myconnect;

        public LinkCollection TitleLink
    {
        get
        {
            return _mytitleLinks;
        }
        set
        {
            if (_mytitleLinks == value)
                return;
            _mytitleLinks = value;
            RaisePropertyChanged("TitleLink");
        }
    }

    public LinkGroupCollection MenuLinkGroups
    {
        get
        {
            return _mymenulinks;
        }
        set
        {
            if (_mymenulinks == value)
                return;
            _mymenulinks = value;
            RaisePropertyChanged("MenuLinkGroups");
        }
    }

    public LinkCollection Links
    {
        get
        {
            return _mylinks;
        }
        set
        {
            if (_mylinks == value)
                return;
            _mylinks = value;
            RaisePropertyChanged("Links");
        }
    }

    public UserBDO User
    {
        get
        {
            return _myuser;
        }
        set
        {
            if (_myuser == value)
                return;
            _myuser = value;
            RaisePropertyChanged("User");
        }
    }

    public MainViewModel(IDataService Service)
    {
        if (IsInDesignMode)
            return;
        _service = Service;
        string person = WindowsIdentity.GetCurrent().Name;
        User = new UserBDO();
            bool mytest = _service.CheckForDatabase(ref myconnect);
            List<UserBDO> userBdoList = new List<UserBDO>();
            if (mytest) 
            {
                User = _service.GetAllUsers().Where(m => m.StaffID == person.Substring(3)).Select(v => v).FirstOrDefault();
            }
            
         
        ApplicationSettings.Default.LOBUser = User.Name.Trim();
            ApplicationSettings.Default.Save();
        InitializeModels();
    }

    private void InitializeModels()
    {
        MenuLinkGroups = new LinkGroupCollection();
        TitleLink = new LinkCollection();
        CreateTitles();
        CreateMenu();
     
        _service.GetIntakes();
    }

    private void CreateTitles()
    {
        TitleLink.Clear();
        string str = "Logged in as: " + User.ToString();
        Link link1 = new Link();
        link1.DisplayName = str;
        link1.Source = new Uri("/View/introduction/IntroView.xaml", UriKind.Relative);
        TitleLink.Add(link1);
        Link link2 = new Link();
        link2.DisplayName = "settings";
        link2.Source = new Uri("/View/SettingsView.xaml", UriKind.Relative);
        TitleLink.Add(link2);
    }

    private void CreateMenu()
    {
        MenuLinkGroups.Clear();
        LinkGroup linkGroup1 = new LinkGroup();
        linkGroup1.DisplayName = "Welcome";
        LinkGroup linkGroup2 = linkGroup1;
        Link link1 = new Link();
        link1.DisplayName = "Introduction";
        link1.Source = new Uri("/View/introduction/IntroView.xaml", UriKind.Relative);
        Link link2 = link1;
        linkGroup2.Links.Add(link2);
        Link link3 = new Link();
        link3.DisplayName = "Instructions";
        link3.Source = new Uri("/View/introduction/InstructView.xaml", UriKind.Relative);
        Link link4 = link3;
        Link link5 = new Link();
        link5.DisplayName = "Map";
        link5.Source = new Uri("/View/writers/VenueMapView.xaml", UriKind.Relative);
        Link link6 = link5;
        linkGroup2.Links.Add(link4);
        linkGroup2.Links.Add(link6);
        MenuLinkGroups.Add(linkGroup2);
        string[] strArray = User.Areas.Split('-');
        foreach (char ch1 in strArray[0].ToCharArray())
        {
            switch (ch1)
            {
                case '0':
                    LinkGroup linkGroup3 = new LinkGroup();
                    linkGroup3.DisplayName = "preparation";
                    LinkGroup linkGroup4 = linkGroup3;
                    foreach (char ch2 in strArray[1].ToCharArray())
                    {
                        switch (ch2)
                        {
                            case '1':
                                Link link7 = new Link();
                                link7.DisplayName = "The Tests";
                                link7.Source = new Uri("/View/processing/TestsView.xaml", UriKind.Relative);
                                Link link8 = link7;
                                linkGroup4.Links.Add(link8);
                                break;
                            case '2':
                                Link link9 = new Link();
                                link9.DisplayName = "Venues";
                                link9.Source = new Uri("/View/writers/VenuesView.xaml", UriKind.Relative);
                                Link link10 = link9;
                                linkGroup4.Links.Add(link10);
                                break;
                            case '3':
                                Link link11 = new Link();
                                link11.DisplayName = "Load List";
                                link11.Source = new Uri("/View/writers/LoadFileView.xaml", UriKind.Relative);
                                Link link12 = link11;
                                linkGroup4.Links.Add(link12);
                                break;
                            case '4':
                                Link link13 = new Link();
                                link13.DisplayName = "Writer List";
                                link13.Source = new Uri("/View/writers/ProcessView.xaml", UriKind.Relative);
                                Link link14 = link13;
                                linkGroup4.Links.Add(link14);
                                break;
                        }
                    }
                    MenuLinkGroups.Add(linkGroup4);
                    break;
                case '1':
                    LinkGroup linkGroup5 = new LinkGroup();
                    linkGroup5.DisplayName = "Data Processing";
                    LinkGroup linkGroup6 = linkGroup5;
                    foreach (char ch2 in strArray[2].ToCharArray())
                    {
                        switch (ch2)
                        {
                            case '1':
                                Link link7 = new Link();
                                link7.DisplayName = "Batching";
                                link7.Source = new Uri("/View/processing/BatchView1.xaml", UriKind.Relative);
                                Link link8 = link7;
                                linkGroup6.Links.Add(link8);
                                break;
                            case '2':
                                Link link9 = new Link();
                                link9.DisplayName = "QA";
                                link9.Source = new Uri("/View/processing/QAView.xaml", UriKind.Relative);
                                Link link10 = link9;
                                linkGroup6.Links.Add(link10);
                                break;
                            case '3':
                                Link link11 = new Link();
                                link11.DisplayName = "Tracker";
                                link11.Source = new Uri("/View/processing/ScanTrackerView.xaml", UriKind.Relative);
                                Link link12 = link11;
                                linkGroup6.Links.Add(link12);
                                break;
                            case '4':
                                Link link13 = new Link();
                                link13.DisplayName = "Error Logs";
                                link13.Source = new Uri("/View/processing/ErrorView.xaml", UriKind.Relative);
                                Link link14 = link13;
                                linkGroup6.Links.Add(link14);
                                break;
                            case '5':
                                Link link15 = new Link();
                                link15.DisplayName = "Bio QA";
                                link15.Source = new Uri("/View/processing/BioQAView.xaml", UriKind.Relative);
                                Link link16 = link15;
                                linkGroup6.Links.Add(link16);
                                break;
                        }
                    }
                    MenuLinkGroups.Add(linkGroup6);
                    break;
                case '2':
                    LinkGroup linkGroup7 = new LinkGroup();
                    linkGroup7.DisplayName = "Scoring";
                    LinkGroup linkGroup8 = linkGroup7;
                    foreach (char ch2 in strArray[3].ToCharArray())
                    {
                        switch (ch2)
                        {
                            case '1':
                                Link link7 = new Link();
                                link7.DisplayName = "Process Scores";
                                link7.Source = new Uri("/View/scoring/ScoringView.xaml", UriKind.Relative);
                                Link link8 = link7;
                                linkGroup8.Links.Add(link8);
                                break;
                            case '2':
                                Link link9 = new Link();
                                link9.DisplayName = "Moderation";
                                link9.Source = new Uri("/View/scoring/ModerationView.xaml", UriKind.Relative);
                                Link link10 = link9;
                                linkGroup8.Links.Add(link10);
                                break;
                        }
                    }
                    MenuLinkGroups.Add(linkGroup8);
                    break;
                case '3':
                    LinkGroup linkGroup9 = new LinkGroup();
                    linkGroup9.DisplayName = "Composite";
                    LinkGroup linkGroup10 = linkGroup9;
                    foreach (char ch2 in strArray[4].ToCharArray())
                    {
                        switch (ch2)
                        {
                                case '1':
                                    Link link20 = new Link();
                                    link20.DisplayName = "Remotes";
                                    link20.Source = new Uri("/View/Composite/RemotesView.xaml", UriKind.Relative);
                                    Link link21 = link20;
                                    linkGroup10.Links.Add(link21);
                                    break;
                                case '2':
                                Link link22 = new Link();
                                link22.DisplayName = "Load and Process";
                                link22.Source = new Uri("/View/Composite/CompositeView.xaml", UriKind.Relative);
                                Link link23 = link22;
                                linkGroup10.Links.Add(link23);
                                break;
                            case '3':
                                Link link24 = new Link();
                                link24.DisplayName = "Edit and Reload";
                                link24.Source = new Uri("/View/Composite/EditCompositeView.xaml", UriKind.Relative);
                                Link link25 = link24;
                                linkGroup10.Links.Add(link25);
                                break;
                        }
                    }
                    MenuLinkGroups.Add(linkGroup10);
                    break;
                case '5':
                    LinkGroup linkGroup11 = new LinkGroup();
                    linkGroup11.DisplayName = "EasyPay";
                    LinkGroup linkGroup12 = linkGroup11;
                    foreach (char ch2 in strArray[6].ToCharArray())
                    {
                        switch (ch2)
                        {
                            case '1':
                                Link link7 = new Link();
                                link7.DisplayName = "FTP files";
                                link7.Source = new Uri("/View/easypay/EasyPayView.xaml", UriKind.Relative);
                                Link link8 = link7;
                                linkGroup12.Links.Add(link8);
                                break;
                            case '2':
                                Link link9 = new Link();
                                link9.DisplayName = "Fake Easypay";
                                link9.Source = new Uri("/View/easypay/FakeEasyPayView.xaml", UriKind.Relative);
                                Link link10 = link9;
                                linkGroup12.Links.Add(link10);
                                break;
                        }
                    }
                    MenuLinkGroups.Add(linkGroup12);
                    break;

                case '6':
                        LinkGroup linkGroup13 = new LinkGroup();
                        linkGroup13.DisplayName = "Utilities";
                       // LinkGroup linkGroup14 = linkGroup13;
                        foreach (char ch2 in strArray[6].ToCharArray())
                        {
                            switch (ch2)
                            {
                                case '1':
                                    Link link15 = new Link();
                                    link15.DisplayName = "Online tests";
                                    link15.Source = new Uri("/View/Utilities/OnlineView.xaml", UriKind.Relative);
                                   // Link link16 = link15;
                                    linkGroup13.Links.Add(link15);
                                    break;
                                case '2':
                                    Link link16 = new Link();
                                    link16.DisplayName = "Subdomains";
                                    link16.Source = new Uri("/View/Utilities/SubdomainsView.xaml", UriKind.Relative);
                                   // Link link10 = link9;
                                    linkGroup13.Links.Add(link16);
                                    break;
                                case '3':
                                    Link link17 = new Link();
                                    link17.DisplayName = "Barcode Generation";
                                    link17.Source = new Uri("/View/Utilities/BarcodesView.xaml", UriKind.Relative);
                                    // Link link10 = link9;
                                    linkGroup13.Links.Add(link17);
                                    break;
                            }
                        }
                        MenuLinkGroups.Add(linkGroup13);
                        break;
                }
        }
    }
}
}