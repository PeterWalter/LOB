using System;
using System.Windows;
using System.Windows.Media;

namespace CETAP_LOB.Helper
{
  /// <summary>
  /// The colours the QA grid uses to highlight a record, chosen for the theme that is
  /// applied. The purple that marks a field differing from the WriterList (like the blue
  /// used for a walk-in reference and the amber used for a barcode problem) is dark
  /// enough to read on the light theme but all but disappears on the dark one, so lighter,
  /// more saturated shades are used there.
  /// <para>
  /// The values are published as application resources and the grid styles refer to them
  /// with DynamicResource, so a theme change repaints the grid that is already on screen.
  /// </para>
  /// </summary>
  public static class QAHighlightColors
  {
    /// <summary>WriterList difference (purple).</summary>
    public const string MismatchKey = "QAMismatchBrush";
    /// <summary>WriterList difference, on the row held by the selection highlight.</summary>
    public const string MismatchSelectedKey = "QAMismatchSelectedBrush";
    /// <summary>Walk-in reference Composit already holds (blue).</summary>
    public const string WalkInKey = "QAWalkInBrush";
    /// <summary>Walk-in reference, on the selected row.</summary>
    public const string WalkInSelectedKey = "QAWalkInSelectedBrush";
    /// <summary>Barcode that is duplicated or unknown to the database (amber).</summary>
    public const string BarcodeKey = "QABarcodeBrush";
    /// <summary>A field that failed validation (red).</summary>
    public const string ErrorKey = "QAErrorBrush";
    /// <summary>Barcode problem, on the selected row.</summary>
    public const string BarcodeSelectedKey = "QABarcodeSelectedBrush";

    private const string LightMismatch = "#FF7B1FA2";
    private const string LightWalkIn = "#FF1565C0";
    private const string LightBarcode = "#FFA15C00";

    private const string DarkMismatch = "#FFCE93D8";
    private const string DarkWalkIn = "#FF90CAF9";
    private const string DarkBarcode = "#FFFFD54F";

    // The selected row is filled with the accent colour in both themes, so the lighter
    // shades that read on it are kept for both.
    private const string SelectedMismatch = "#FFE1BEE7";
    private const string SelectedWalkIn = "#FFBBDEFB";
    private const string SelectedBarcode = "#FFFFE082";

    /// <summary>The theme dictionaries key the window background colour under this name.</summary>
    private const string WindowBackgroundColorKey = "WindowBackgroundColor";

    /// <summary>
    /// Republishes the highlight colours for the theme that is currently applied. Called
    /// when the theme changes; the light shades are used when the theme cannot be read.
    /// </summary>
    public static void Apply()
    {
      if (Application.Current == null)
        return;

      bool dark = IsDarkTheme();

      Publish(MismatchKey, dark ? DarkMismatch : LightMismatch);
      Publish(MismatchSelectedKey, SelectedMismatch);
      Publish(WalkInKey, dark ? DarkWalkIn : LightWalkIn);
      Publish(WalkInSelectedKey, SelectedWalkIn);
      Publish(BarcodeKey, dark ? DarkBarcode : LightBarcode);
      Publish(BarcodeSelectedKey, SelectedBarcode);

      // Validation errors: red on the light theme, a lighter red on the dark one.
      Publish(ErrorKey, dark ? "#FFFF8A80" : "#FFD32F2F");
    }

    /// <summary>
    /// Colour of a QA file-list entry, which is tinted by the file's total error count so
    /// the worst files stand out. The shades that read on white are too dark on the dark
    /// theme - black most of all - so each band has a shade for each theme.
    /// </summary>
    public static Brush FileListBrush(int errorCount)
    {
      return FileListBrush(errorCount, IsDarkTheme());
    }

    /// <summary>
    /// The file-list shade for one theme. Kept separate so the choice can be checked
    /// without applying a theme.
    /// </summary>
    private static Brush FileListBrush(int errorCount, bool dark)
    {
      if (errorCount <= 0)
        return Brush(dark ? "#FFF0F0F0" : "#FF000000");
      if (errorCount <= 5)
        return Brush(dark ? "#FFF4A460" : "#FFD2691E");
      if (errorCount <= 10)
        return Brush("#FFEE82EE");
      if (errorCount <= 20)
        return Brush("#FFFFA500");
      if (errorCount <= 30)
        return Brush(dark ? "#FFFF7043" : "#FFFF4500");
      return Brush(dark ? "#FFFF6B6B" : "#FFFF0000");
    }

    /// <summary>
    /// True when the window background of the applied theme is dark. Every theme
    /// dictionary defines the window background colour - the custom themes merge either
    /// the light or the dark one - so this follows the theme without listing it.
    /// </summary>
    public static bool IsDarkTheme()
    {
      if (Application.Current == null)
        return false;

      object value = Application.Current.TryFindResource(WindowBackgroundColorKey);
      Color background;
      if (value is Color)
        background = (Color)value;
      else if (value is SolidColorBrush)
        background = ((SolidColorBrush)value).Color;
      else
        return false;

      return Luminance(background) < 0.4;
    }

    /// <summary>Relative luminance, as WCAG defines it.</summary>
    private static double Luminance(Color color)
    {
      return 0.2126 * Channel(color.R) + 0.7152 * Channel(color.G) + 0.0722 * Channel(color.B);
    }

    private static double Channel(byte value)
    {
      double channel = value / 255.0;
      return channel <= 0.03928 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
    }

    private static void Publish(string key, string color)
    {
      Application.Current.Resources[key] = Brush(color);
    }

    private static SolidColorBrush Brush(string color)
    {
      return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
    }
  }
}
