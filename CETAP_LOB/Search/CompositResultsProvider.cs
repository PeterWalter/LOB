// Decompiled with JetBrains decompiler
// Type: LOB.Search.CompositResultsProvider
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
  public class CompositResultsProvider : IIntelliboxResultsProvider
  {
    private List<CompositBDO> _results;
    private IDataService _service;

    public CompositResultsProvider(IDataService Service)
    {
      _service = Service;
    }

    public IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo)
    {
      _results = new List<CompositBDO>();
      if (ApplicationSettings.Default.DBAvailable)
      {
        using (CETAPEntities cetapEntities = new CETAPEntities())
        {
          if (HelperUtils.IsNumeric(searchTerm))
          {
            foreach (Composit composit in cetapEntities.Composits.ToList<Composit>().Where<Composit>((Func<Composit, bool>) (p =>
            {
              if (!p.SAID.ToString().StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                return p.RefNo.ToString().StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase);
              return true;
            })).Cast<object>())
            {
              CompositBDO compositBdo = new CompositBDO();
              _results.Add(Maps.CompositDALToCompositBDO(composit));
            }
          }
          else
          {
            foreach (Composit composit in cetapEntities.Composits.ToList<Composit>().Where<Composit>((Func<Composit, bool>) (p =>
            {
              if (!p.Surname.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                return p.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase);
              return true;
            })).Cast<object>())
            {
              CompositBDO compositBdo = new CompositBDO();
              _results.Add(Maps.CompositDALToCompositBDO(composit));
            }
          }
        }
      }
      return (IEnumerable) _results;
    }
  }
}
