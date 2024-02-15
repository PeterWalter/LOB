// Decompiled with JetBrains decompiler
// Type: LOB.Helper.HelperUtils
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model.QA;
using CETAP_LOB.Model.venueprep;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CETAP_LOB.Helper
{
  public static class HelperUtils
  {
    public static bool IsFileLocked(FileInfo file)
    {
      FileStream fileStream = (FileStream) null;
      try
      {
        fileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      }
      catch (IOException ex)
      {
        throw ex;
      }
      finally
      {
        if (fileStream != null)
          fileStream.Close();
      }
      return false;
    }

    public static string FirstCharToUpper(string input)
    {
      if (string.IsNullOrEmpty(input))
        return (string) null;
      input.Trim();
      string[] separator = new string[1]{ " " };
      string[] strArray = input.Split(separator, StringSplitOptions.None);
      int num = ((IEnumerable<string>) strArray).Count<string>();
      for (int index = 0; index < num; ++index)
        strArray[index] = strArray[index].First<char>().ToString().ToUpper() + string.Join<char>("", strArray[index].Skip<char>(1));
      string str = strArray[0];
      for (int index = 1; index < num; ++index)
      {
        if (num == 1)
        {
          str = strArray[0];
          break;
        }
        str = str + " " + strArray[index];
      }
      return str;
    }

    public static bool StringHasANumber(string input)
    {
      return Regex.IsMatch(input, "\\d");
    }

    public static int ComputeChecksum(string input)
    {
      return input.Where<char>((Func<char, bool>) (c => char.IsDigit(c))).Reverse<char>().SelectMany<char, char>((Func<char, int, IEnumerable<char>>) ((c, i) => (IEnumerable<char>) ((int) c - 48 << (i & 1)).ToString())).Sum<char>((Func<char, int>) (c => (int) c - 48)) % 10;
    }

    public static bool IsValidChecksum(string value)
    {
      return HelperUtils.ComputeChecksum(value) == 0;
    }

    public static bool GenerateWriterList(ProcessList list)
    {
      try
      {
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static string YourChange(int amount)
    {
      string str = "";
      if (amount < 50)
      {
        if (amount == 0)
          return str;
        int num = amount / 10;
        if (amount % 10 > 0)
          ++num;
        return num.ToString() + "(10)";
      }
      int num1 = amount / 50;
      int num2 = amount % 50;
      int num3 = num2 / 10;
      if (num2 % 10 > 0)
        ++num3;
      if (num3 == 0)
        return num1.ToString() + "(50)";
      return num1.ToString() + "(50) " + num3.ToString() + "(10)";
    }

    public static int RoundAmount(int amount)
    {
      return (int) Math.Round((double) amount / 10.0, MidpointRounding.AwayFromZero) * 10;
    }

    public static int RoundUp(int number)
    {
      return (int) (Math.Ceiling((double) number / 10.0) * 10.0);
    }

    public static int RoundUpWalkIn(int number)
    {
      return (int) (Math.Ceiling((double) number / 250.0) * 10.0);
    }

    public static DateTime UnixTimeToDateTime(long unixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) unixTimeStamp).ToLocalTime();
    }

    public static DateTime WebPrepDateTime(string date)
    {
      DateTime dateTime = new DateTime();
      if (string.IsNullOrWhiteSpace(date))
        return dateTime = DateTime.ParseExact("1900/01/01", "yyyy/MM/dd", (IFormatProvider) null);
      string[] strArray = date.Split(new char[2]{ '-', '/' });
      return strArray[2].Trim().Length <= 13 ? (strArray[2].Trim().Length <= 8 ? DateTime.ParseExact(date, "yyyy/MM/dd HH:mm", (IFormatProvider) null) : DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss", (IFormatProvider) null)) : DateTime.ParseExact(date, "yyyy/MM/dd hh:mm:ss tt", (IFormatProvider) null);
    }
        public static DateTime weblistDateTime(string DatdateString)
        {
            string dateString = "2022/08/09 1:45:29 PM";
            DateTime dateTime = DateTime.ParseExact(dateString, "yyyy/MM/dd h:mm:ss tt", CultureInfo.InvariantCulture);
            return dateTime;
        }
        public static DateTime WebDateTime(string date)
    {
     // DateTime dateTime = new DateTime();
      string[] strArray = date.Split(new char[2]{ '-', '/' });
      return strArray[0].Trim().Length != 4 ? (strArray[2].Length >= 13 ? DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", (IFormatProvider) null) : DateTime.ParseExact(date, "dd-MM-yyyy HH:mm", (IFormatProvider) null)) : DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss", (IFormatProvider) null);
    }

    public static DateTime BIDate(string date)
    {
      DateTime dateTime = new DateTime();
      if (string.IsNullOrWhiteSpace(date))
        return DateTime.ParseExact("1900/01/01", "yyyy/MM/dd", (IFormatProvider) null);
      string[] strArray = date.Split(' ');
      if (strArray[0].Split(new char[2]{ '-', '/' })[0].Trim().Length == 4)
        dateTime = DateTime.ParseExact(strArray[0], "yyyy-MM-dd", (IFormatProvider) null);
      return dateTime;
    }

    public static DateTime WebDate(string date)
    {
      DateTime dateTime = new DateTime();
      if (string.IsNullOrWhiteSpace(date))
        return dateTime = DateTime.ParseExact("1900/01/01", "yyyy/MM/dd", (IFormatProvider) null);
      string[] strArray = date.Split(' ');
      return strArray[0].Split(new char[2]{ '-', '/' })[0].Trim().Length != 4 ? DateTime.ParseExact(date, "dd-MM-yyyy", (IFormatProvider) null) : DateTime.ParseExact(strArray[0], "yyyy/MM/dd", (IFormatProvider) null);
    }

    public static DateTime BioDate(string date)
    {
      if (date.Length < 8 || !Regex.IsMatch(date, "^[0-9]+$"))
        date = "19000101";
      int int32_1 = Convert.ToInt32(date.Substring(4, 2));
      int int32_2 = Convert.ToInt32(date.Substring(6, 2));
      if (int32_2 > 30 && int32_1 == 6)
        date = "19000101";
      if (DateTime.IsLeapYear(Convert.ToInt32(date.Substring(0, 4))))
      {
        if (int32_2 > 29 && int32_1 == 2)
          date = "19000101";
      }
      else if (int32_2 > 28 && int32_1 == 2)
        date = "19000101";
      if (int32_2 > 31)
        date = "19000101";
      if (date.Substring(6, 2) == "00" || date.Substring(4, 2) == "00" || int32_1 > 12)
        date = "19000101";
    //  DateTime dateTime = new DateTime();
      return DateTime.ParseExact(date, "yyyyMMdd", (IFormatProvider) null);
    }

    public static string DOBfromSAID(string SAID)
    {
      if (SAID.Length < 10)
        return "";
      SAID = Convert.ToInt64(SAID).ToString("D13");
      if (!HelperUtils.IsNumeric(SAID.Substring(0, 6)))
        return (string) null;
      string str1 = SAID.Substring(4, 2);
      string str2 = SAID.Substring(2, 2);
      string str3 = SAID.Substring(0, 2);
      string str4 = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) <= Convert.ToInt32(str3) ? "19" + str3 : "20" + str3;
      return str1 + "/" + str2 + "/" + str4;
    }

    public static string DOBfromSAID1(string SAID)
    {
      string str1 = SAID.Substring(4, 2);
      string str2 = SAID.Substring(2, 2);
      string str3 = SAID.Substring(0, 2);
      return (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) <= Convert.ToInt32(str3) ? "19" + str3 : "20" + str3) + str2 + str1;
    }

    public static string ToSAID(long? SAID)
    {
      string str = "";
      if (SAID.HasValue)
        str = SAID.Value.ToString("D13");
      return str;
    }

    public static string GetFacultyName(string faculty)
    {
      if (!string.IsNullOrWhiteSpace(faculty))
        faculty = faculty.Trim();
      string str = "";
      switch (faculty)
      {
        case "A":
          str = "Allied Healthcare/Nursing";
          break;
        case "C":
          str = "Business/Commerce/Management";
          break;
        case "B":
          str = "Art/Design";
          break;
        case "D":
          str = "Education";
          break;
        case "E":
          str = "Engineering/Built Environment";
          break;
        case "G":
          str = "Hospitality/Tourism";
          break;
        case "H":
          str = "Humanities";
          break;
        case "I":
          str = "Information & Communication Technology";
          break;
        case "J":
          str = "Law";
          break;
        case "K":
          str = "Science/Mathematics";
          break;
        case "L":
          str = "Other";
          break;
        case "Y":
          str = "Health Science";
          break;
      }
      return str;
    }

    public static string GenderSymbol(int number)
    {
      string str = "";
      if (number == 1)
        str = "M";
      if (number == 1)
        str = "F";
      return str;
    }

    public static bool SurnameIsAvailable(string lastname)
    {
      return false;
    }

    public static string RemoveWorksheetChars(string worksheetName)
    {
      string str = "";
      char[] separator = new char[5]
      {
        '\\',
        '[',
        ':',
        ']',
        '/'
      };
      string[] strArray = worksheetName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      for (int index = 0; index < ((IEnumerable<string>) strArray).Count<string>(); ++index)
        str += strArray[index];
      return str;
    }

    public static bool IsValidEmail(string emailaddress)
    {
      try
      {
        MailAddress mailAddress = new MailAddress(emailaddress);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }

    public static List<DatAnswer> GetAnswerList(string section)
    {
      char[] charArray = section.ToCharArray();
      List<DatAnswer> datAnswerList = new List<DatAnswer>();
      foreach (char ch in charArray)
        datAnswerList.Add(new DatAnswer() { Value = ch });
      return datAnswerList;
    }

    public static string CollectionToString(ObservableCollection<DatAnswer> answers)
    {
      DatAnswer[] array = new List<DatAnswer>((IEnumerable<DatAnswer>) answers).ToArray();
      int length = ((IEnumerable<DatAnswer>) array).Count<DatAnswer>();
      char[] chArray = new char[length];
      for (int index = 0; index < length; ++index)
        chArray[index] = array[index].Value;
      return string.Join<char>("", (IEnumerable<char>) chArray);
    }

    public static string GetRandomNumber()
    {
      return new Random(DateTime.Now.Millisecond).Next(0, 99999).ToString("D5");
    }

    public static bool IsNumeric(string s)
    {
      return s.All<char>(new Func<char, bool>(char.IsDigit));
    }

    public static double LowerQuartile(this IOrderedEnumerable<int> list)
    {
      return HelperUtils.GetQuartile(list, 0.25);
    }

    public static double UpperQuartile(this IOrderedEnumerable<int> list)
    {
      return HelperUtils.GetQuartile(list, 0.75);
    }

    public static double MiddleQuartile(this IOrderedEnumerable<int> list)
    {
      return HelperUtils.GetQuartile(list, 0.5);
    }

    public static double InterQuartileRange(this IOrderedEnumerable<int> list)
    {
      return list.UpperQuartile() - list.LowerQuartile();
    }

    private static double GetQuartile(IOrderedEnumerable<int> list, double quartile)
    {
      double d = quartile * (double) (list.Count<int>() + 1);
      double remainder = d % 1.0;
      double num1 = Math.Floor(d) - 1.0;
      double num2;
      if (remainder.Equals(0.0))
      {
        num2 = (double) list.ElementAt<int>((int) num1);
      }
      else
      {
        double a = (double) list.ElementAt<int>((int) num1);
        double num3 = a.Interpolate((double) list.ElementAt<int>((int) (num1 + 1.0)), remainder);
        num2 = a + num3;
      }
      return num2;
    }

    private static double Interpolate(this double a, double b, double remainder)
    {
      return (b - a) * remainder;
    }
  }
}
