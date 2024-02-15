// Decompiled with JetBrains decompiler
// Type: LOB.Converters.ProvinceToNumber
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace CETAP_LOB.Converters
{
  public class ProvinceToNumber : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        value = (object) "0";
      switch (value.ToString())
      {
        case "1":
          return (object) "Eastern Cape";
        case "2":
          return (object) "Free State";
        case "3":
          return (object) "Gauteng";
        case "4":
          return (object) "KwaZulu Natal";
        case "5":
          return (object) "Limpopo";
        case "6":
          return (object) "Mpumalanga";
        case "7":
          return (object) "North West";
        case "8":
          return (object) "Northern Cape";
        case "9":
          return (object) "Western Cape";
        case "10":
          return (object) "Bursary";
        case "11":
          return (object) "Remote";
        case "12":
          return (object) "International";
        default:
          return Binding.DoNothing;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch (value.ToString())
      {
        case "Eastern Cape":
          return (object) 1;
        case "Free State":
          return (object) 2;
        case "Bursary":
          return (object) 0;
        default:
          return Binding.DoNothing;
      }
    }
  }
}
