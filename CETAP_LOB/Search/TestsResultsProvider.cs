// Decompiled with JetBrains decompiler
// Type: LOB.Search.TestsResultsProvider
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FeserWard.Controls;
using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Mapping;
using CETAP_LOB.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CETAP_LOB.Search
{
  public class TestsResultsProvider : IIntelliboxResultsProvider
  {
    private List<TestBDO> _results;
    private IDataService _service;

    public TestsResultsProvider(IDataService Service)
    {
      _service = Service;
    }

    public IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo)
    {
      _results = new List<TestBDO>();
      if (ApplicationSettings.Default.DBAvailable)
      {
        using (CETAPEntities cetapEntities = new CETAPEntities())
        {
          if (HelperUtils.IsNumeric(searchTerm))
          {
            foreach (TestName test in cetapEntities.TestNames.ToList<TestName>().Where<TestName>((Func<TestName, bool>) (p => p.TestCode.ToString().StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))).Cast<object>())
            {
              TestBDO testBdo = new TestBDO();
              _results.Add(Maps.TestDALToTestBDO(test));
            }
          }
          else
          {
            foreach (TestName test in cetapEntities.TestNames.ToList<TestName>().Where<TestName>((Func<TestName, bool>) (p => p.TestName1.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))).Cast<object>())
            {
              TestBDO testBdo = new TestBDO();
              _results.Add(Maps.TestDALToTestBDO(test));
            }
          }
        }
      }
      return (IEnumerable) _results;
    }
  }
}
