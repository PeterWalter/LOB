

using FirstFloor.ModernUI.Presentation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace CETAP_LOB.ViewModel
{
  public class SettingsAppearanceViewModel : ViewModelBase
  {
    public const string SelectedPalettePropertyName = "SelectedPalette";
    public const string PalettesPropertyName = "Palettes";
    public const string SelectedMetroAccentColorPropertyName = "SelectedMetroAccentColor";
    public const string AccentColorsPropertyName = "AccentColors";
    public const string metroAccentColorsPropertyName = "metroAccentColors";
    public const string wpaccentColorsPropertyName = "wpAccentColors";
    public const string SelectedAccentColorPropertyName = "SelectedAccentColor";
    public const string ThemesPropertyName = "Themes";
    public const string SelectedThemePropertyName = "SelectedTheme";
    public const string SelectedFontSizePropertyName = "SelectedFontSize";
    private const string FontSmall = "small";
    private const string FontLarge = "large";
    private bool _colorLoadedYet;
    private string _mySelectedPalette;
    private ObservableCollection<string> _mypalettes;
    private Color _myMetroColor;
    private ObservableCollection<Color> _accentColors;
    private ObservableCollection<Color> _metroColor;
    private ObservableCollection<Color> _myProperty;
    private Color _selectedAccentColor;
    private LinkCollection themes;
    private Link _selectedTheme;
    private string selectedFontSize;

    public string SelectedPalette
    {
      get
      {
        return _mySelectedPalette;
      }
      set
      {
        if (_mySelectedPalette == value)
          return;
        _mySelectedPalette = value;
        RaisePropertyChanged("AccentColors");
        SelectedAccentColor = AccentColors.FirstOrDefault<Color>();
        RaisePropertyChanged("SelectedPalette");
      }
    }

    public ObservableCollection<string> Palettes
    {
      get
      {
        return _mypalettes;
      }
      set
      {
        if (_mypalettes == value)
          return;
        _mypalettes = value;
        RaisePropertyChanged("Palettes");
      }
    }

    public Color SelectedMetroAccentColor
    {
      get
      {
        return _myMetroColor;
      }
      set
      {
        if (_myMetroColor == value)
          return;
        _myMetroColor = value;
        RaisePropertyChanged("SelectedMetroAccentColor");
      }
    }

    public ObservableCollection<Color> AccentColors
    {
      get
      {
        if (!(SelectedPalette == "metro"))
          return wpAccentColors;
        return metroAccentColors;
      }
      set
      {
        if (_accentColors == value)
          return;
        _accentColors = value;
        RaisePropertyChanged("AccentColors");
      }
    }

    public ObservableCollection<Color> metroAccentColors
    {
      get
      {
        return _metroColor;
      }
      set
      {
        if (_metroColor == value)
          return;
        _metroColor = value;
        RaisePropertyChanged("metroAccentColors");
      }
    }

    public ObservableCollection<Color> wpAccentColors
    {
      get
      {
        return _myProperty;
      }
      set
      {
        if (_myProperty == value)
          return;
        _myProperty = value;
        RaisePropertyChanged("wpAccentColors");
      }
    }

    public Color SelectedAccentColor
    {
      get
      {
        return _selectedAccentColor;
      }
      set
      {
        if (_selectedAccentColor == value)
          return;
        _selectedAccentColor = value;
        AppearanceManager.Current.AccentColor = value;
        RaisePropertyChanged("SelectedAccentColor");
      }
    }

    public LinkCollection Themes
    {
      get
      {
        return themes;
      }
      set
      {
        if (themes == value)
          return;
        themes = value;
        RaisePropertyChanged("Themes");
      }
    }

    public Link SelectedTheme
    {
      get
      {
        return _selectedTheme;
      }
      set
      {
        if (_selectedTheme == value)
          return;
        _selectedTheme = value;
        RaisePropertyChanged("SelectedTheme");
                AppearanceManager.Current.ThemeSource = value.Source; // update the actual theme
            }
    }

    public string SelectedFontSize
    {
      get
      {
        return selectedFontSize;
      }
      set
      {
        if (selectedFontSize == value)
          return;
        selectedFontSize = value;
        RaisePropertyChanged("SelectedFontSize");
        AppearanceManager.Current.FontSize = value == "large" ? FontSize.Large : FontSize.Small;
        if (!_colorLoadedYet)
          return;
        ApplicationSettings.Default.SelectedFontSize = selectedFontSize;
        ApplicationSettings.Default.Save();
      }
    }

    public string[] FontSizes
    {
      get
      {
        return new string[2]{ "small", "large" };
      }
    }

    public SettingsAppearanceViewModel()
    {
      ObservableCollection<string> observableCollection1 = new ObservableCollection<string>();
      observableCollection1.Add("metro");
      observableCollection1.Add("windows phone");
      _mypalettes = observableCollection1;
      ObservableCollection<Color> observableCollection2 = new ObservableCollection<Color>();
      observableCollection2.Add(Color.FromRgb((byte) 51, (byte) 153, byte.MaxValue));
      observableCollection2.Add(Color.FromRgb((byte) 0, (byte) 171, (byte) 169));
      observableCollection2.Add(Color.FromRgb((byte) 51, (byte) 153, (byte) 51));
      observableCollection2.Add(Color.FromRgb((byte) 140, (byte) 191, (byte) 38));
      observableCollection2.Add(Color.FromRgb((byte) 240, (byte) 150, (byte) 9));
      observableCollection2.Add(Color.FromRgb(byte.MaxValue, (byte) 69, (byte) 0));
      observableCollection2.Add(Color.FromRgb((byte) 229, (byte) 20, (byte) 0));
      observableCollection2.Add(Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 151));
      observableCollection2.Add(Color.FromRgb((byte) 162, (byte) 0, byte.MaxValue));
      _metroColor = observableCollection2;
      ObservableCollection<Color> observableCollection3 = new ObservableCollection<Color>();
      observableCollection3.Add(Color.FromRgb((byte) 164, (byte) 196, (byte) 0));
      observableCollection3.Add(Color.FromRgb((byte) 96, (byte) 169, (byte) 23));
      observableCollection3.Add(Color.FromRgb((byte) 0, (byte) 138, (byte) 0));
      observableCollection3.Add(Color.FromRgb((byte) 0, (byte) 171, (byte) 169));
      observableCollection3.Add(Color.FromRgb((byte) 27, (byte) 161, (byte) 226));
      observableCollection3.Add(Color.FromRgb((byte) 0, (byte) 80, (byte) 239));
      observableCollection3.Add(Color.FromRgb((byte) 106, (byte) 0, byte.MaxValue));
      observableCollection3.Add(Color.FromRgb((byte) 170, (byte) 0, byte.MaxValue));
      observableCollection3.Add(Color.FromRgb((byte) 244, (byte) 114, (byte) 208));
      observableCollection3.Add(Color.FromRgb((byte) 216, (byte) 0, (byte) 115));
      observableCollection3.Add(Color.FromRgb((byte) 162, (byte) 0, (byte) 37));
      observableCollection3.Add(Color.FromRgb((byte) 229, (byte) 20, (byte) 0));
      observableCollection3.Add(Color.FromRgb((byte) 250, (byte) 104, (byte) 0));
      observableCollection3.Add(Color.FromRgb((byte) 240, (byte) 163, (byte) 10));
      observableCollection3.Add(Color.FromRgb((byte) 227, (byte) 200, (byte) 0));
      observableCollection3.Add(Color.FromRgb((byte) 130, (byte) 90, (byte) 44));
      observableCollection3.Add(Color.FromRgb((byte) 109, (byte) 135, (byte) 100));
      observableCollection3.Add(Color.FromRgb((byte) 100, (byte) 118, (byte) 135));
      observableCollection3.Add(Color.FromRgb((byte) 118, (byte) 96, (byte) 138));
      observableCollection3.Add(Color.FromRgb((byte) 135, (byte) 121, (byte) 78));
      _myProperty = observableCollection3;
      themes = new LinkCollection();
      selectedFontSize = string.Empty;

      LinkCollection themes1 = themes;
      Link link1 = new Link();
      link1.DisplayName = "dark";
      link1.Source = AppearanceManager.DarkThemeSource;
      Link link2 = link1;
      themes1.Add(link2);
      LinkCollection themes2 = themes;
      Link link3 = new Link();
      link3.DisplayName = "light";
      link3.Source = AppearanceManager.LightThemeSource;
      Link link4 = link3;
      themes2.Add(link4);
      LinkCollection themes3 = themes;
      Link link5 = new Link();
      link5.DisplayName = "snowflake";
      link5.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.Snowflakes.xaml", UriKind.Relative);
      Link link6 = link5;
      themes3.Add(link6);
      LinkCollection themes4 = themes;
      Link link7 = new Link();
      link7.DisplayName = "bing image";
      link7.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.BingImage.xaml", UriKind.Relative);
      Link link8 = link7;
      themes4.Add(link8);
      LinkCollection themes5 = themes;
      Link link9 = new Link();
      link9.DisplayName = "cetap";
      link9.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.Cetap.xaml", UriKind.Relative);
      Link link10 = link9;
      themes5.Add(link10);
      LinkCollection themes6 = themes;
      Link link11 = new Link();
      link11.DisplayName = "rose";
      link11.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.Rose.xaml", UriKind.Relative);
      Link link12 = link11;
      themes6.Add(link12);
      LinkCollection themes7 = themes;
      Link link13 = new Link();
      link13.DisplayName = "bubbles";
      link13.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.Bubbles.xaml", UriKind.Relative);
      Link link14 = link13;
      themes7.Add(link14);
      LinkCollection themes8 = themes;
      Link link15 = new Link();
      link15.DisplayName = "water drop";
      link15.Source = new Uri("/CETAP_LOB;component/Assets/ModernUI.WaterDrop.xaml", UriKind.Relative);
      Link link16 = link15;
      themes8.Add(link16);
      SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? "large" : "small";
      SyncThemeAndColor();
      AppearanceManager.Current.PropertyChanged += new PropertyChangedEventHandler(OnAppearanceManagerPropertyChanged);
    }

    private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "ThemeSource") && !(e.PropertyName == "AccentColor"))
        return;
      SyncThemeAndColor();
    }

    private void SyncThemeAndColor()
    {
      SelectedTheme = themes.FirstOrDefault<Link>((Func<Link, bool>) (l => l.Source.Equals((object) AppearanceManager.Current.ThemeSource)));
      SelectedAccentColor = AppearanceManager.Current.AccentColor;
      if (!_colorLoadedYet)
        return;
      ApplicationSettings.Default.SelectedThemeDisplayName = SelectedTheme.DisplayName;
      ApplicationSettings.Default.SelectedThemeSource = SelectedTheme.Source;
      ApplicationSettings.Default.SelectedAccentColor = SelectedAccentColor;
      ApplicationSettings.Default.SelectedFontSize = SelectedFontSize;
      ApplicationSettings.Default.Save();
    }

    public void SetThemeAndColor(string themeSourceDisplayName, Uri themeSourceUri, Color accentColor, string fontSize)
    {
      Link link = new Link();
      link.DisplayName = themeSourceDisplayName;
      link.Source = themeSourceUri;
      SelectedTheme = link;
      SelectedAccentColor = accentColor;
      SelectedFontSize = fontSize;
      _colorLoadedYet = true;
    }
  }
}
