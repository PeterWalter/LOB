

using CETAP_LOB.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CETAP_LOB.Helper
{
  public static class ApplicantNames
  {
    private static List<string> _writernames = new List<string>();
    private static string _name;
    private static bool _found;

    public static bool IsFound
    {
      get
      {
        return _found;
      }
    }

    public static string FirstName
    {
      get
      {
        return _name;
      }
      set
      {
       _name = value;
        IsAvailable();
      }
    }

    static ApplicantNames()
    {
      using (CETAPEntities cetapEntities = new CETAPEntities())
         _writernames = cetapEntities.FirstNames.Select(x => x.Name).ToList();
    }

    public static void AddName(string name)
    {
      _writernames.Add(name.Trim());
    }

    private static void IsAvailable()
    {
      _found = _writernames.Any(a => a == _name);
    }
  }
}
