// Decompiled with JetBrains decompiler
// Type: LOB.Search.VenueResultsProvider
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FeserWard.Controls;
using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CETAP_LOB;

namespace CETAP_LOB.Search
{
  public class VenueResultsProvider : IIntelliboxResultsProvider
  {
    private List<VenueBDO> _results;
    private IDataService _service;

    public VenueResultsProvider(IDataService Service)
    {
      _service = Service;
    }

    public IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo)
    {
      _results = new List<VenueBDO>();
      if (ApplicationSettings.Default.DBAvailable)
      {
        using (CETAPEntities cetapEntities = new CETAPEntities())
        {
          if (HelperUtils.IsNumeric(searchTerm))
          {
            foreach (TestVenue testVenue in cetapEntities.TestVenues.ToList<TestVenue>().Where<TestVenue>((Func<TestVenue, bool>) (p => p.VenueCode.ToString().StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))).Cast<object>())
            {
              VenueBDO venueBDO = new VenueBDO();
              TestVenueToVenueBDO(venueBDO, testVenue);
              _results.Add(venueBDO);
            }
          }
          else
          {
            foreach (TestVenue testVenue in cetapEntities.TestVenues.ToList<TestVenue>().Where<TestVenue>((Func<TestVenue, bool>) (p => p.VenueName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))).Cast<object>())
            {
              VenueBDO venueBDO = new VenueBDO();
              VenueResultsProvider.TestVenueToVenueBDO(venueBDO, testVenue);
              _results.Add(venueBDO);
            }
          }
        }
      }
      return (IEnumerable) _results;
    }

    private static void TestVenueToVenueBDO(VenueBDO venueBDO, TestVenue testVenue)
    {
      venueBDO.Available = testVenue.Available;
      venueBDO.Capacity = testVenue.Capacity;
      venueBDO.Description = testVenue.Comments;
      venueBDO.Place = testVenue.Place;
      venueBDO.ProvinceID = testVenue.ProvinceID;
      venueBDO.Room = testVenue.Room;
      venueBDO.ShortName = testVenue.ShortName;
      venueBDO.VenueCode = testVenue.VenueCode;
      venueBDO.VenueName = testVenue.VenueName;
      venueBDO.VenueType = testVenue.VenueType;
      venueBDO.RowGuid = testVenue.RowGuid;
      venueBDO.WebSiteName = testVenue.WebsiteName;
    }
  }
}
