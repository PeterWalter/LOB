

using System.Collections.Generic;
using System.Linq;
using CETAP_LOB.Database;

namespace CETAP_LOB.Helper
{
    public static class ListSurnames
  {
    private static List<string> _lastnames = new List<string>();
    private static string _surname;
    private static bool _found;

    public static bool IsFound
    {
      get
      {
        return ListSurnames._found;
      }
    }

    public static string surname
    {
      get
      {
        return _surname;
      }
      set
      {
        _surname = value;
        IsAvailable();
      }
    }

    static ListSurnames()
    {
      using (CETAPEntities cetapEntities = new CETAPEntities())
        _lastnames = cetapEntities.Surnames.Select(x => x.Surname1).ToList();
    }

    public static void AddSurname(string name)
    {
      _lastnames.Add(name.Trim());
    }

    private static void IsAvailable()
    {
      _found = _lastnames.Any(a => a == _surname);
    }
  }
}
