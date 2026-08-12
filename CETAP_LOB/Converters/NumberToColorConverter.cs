// Decompiled with JetBrains decompiler
// Type: LOB.Converters.NumberToColorConverter
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CETAP_LOB.Converters
{
  [ValueConversion(typeof (int), typeof (Brush))]
  public class NumberToColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int num = 0;
      if (value == null)
      {
        value = (object) 0;
        return (object) Brushes.Black;
      }
      if ((int) value >= 0)
        num = (int) value;
      if (num == 0)
        return (object) Brushes.Black;
      if (num > 0 && num <= 5)
        return (object) Brushes.Chocolate;
      if (num > 5 && num <= 10)
        return (object) Brushes.Violet;
      if (num > 10 && num <= 20)
        return (object) Brushes.Orange;
      if (num > 20 && num <= 30)
        return (object) Brushes.OrangeRed;
      return (object) Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
