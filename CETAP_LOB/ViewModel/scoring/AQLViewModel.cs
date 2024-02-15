// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.scoring.AQLViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using CETAP_LOB.Model.scoring;
using System.Collections.ObjectModel;

namespace CETAP_LOB.ViewModel.scoring
{
  public class AQLViewModel : ViewModelBase
  {
    public const string AQLStatsPropertyName = "AQLStats";
    public const string AQLPropertyName = "AQL";
    private ObservableCollection<ScoreStats> _mystats;
    private ObservableCollection<AQL_Score> _myAQL;
    private IDataService _service;

    public ObservableCollection<ScoreStats> AQLStats
    {
      get
      {
        return _mystats;
      }
      set
      {
        if (_mystats == value)  return;
        _mystats = value;
        RaisePropertyChanged("AQLStats");
      }
    }

    public ObservableCollection<AQL_Score> AQL
    {
      get
      {
        return _myAQL;
      }
      set
      {
        if (_myAQL == value)
          return;
        _myAQL = value;
        RaisePropertyChanged("AQL");
      }
    }

    public AQLViewModel(IDataService Service)
    {
      _service = Service;
      Refresh();
    }

    private void Refresh()
    {
      AQL = _service.LoadAQLScores();
      AQLStats = _service.GetAQLStats();
    }
  }
}
