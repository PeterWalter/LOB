// Decompiled with JetBrains decompiler
// Type: LOB.Converters.NumberToColorConverter
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using CETAP_LOB.Helper;

namespace CETAP_LOB.Converters
{
  /// <summary>
  /// Colours a QA file-list entry by the file's total error count. The shades come from
  /// <see cref="QAHighlightColors"/> because black - the colour of a file with no errors -
  /// and the reds cannot be read on the dark theme, so each band has a lighter shade
  /// there. QAView keeps the page alive only while it is open, so the list is converted
  /// again, with the theme of the moment, whenever the module is opened.
  /// </summary>
  [ValueConversion(typeof (int), typeof (Brush))]
  public class NumberToColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) QAHighlightColors.FileListBrush(value is int ? (int) value : 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
