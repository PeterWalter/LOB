// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.scoring.MatViewModel
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
  public class MatViewModel : ViewModelBase
  {
    public const string MATStatsPropertyName = "MATStats";
    public const string MATPropertyName = "MAT";
    private ObservableCollection<ScoreStats> _mymatstats;
    private ObservableCollection<MAT_Score> _myMAT;
    private IDataService _service;

    public ObservableCollection<ScoreStats> MATStats
    {
      get
      {
        return _mymatstats;
      }
      set
      {
        if (_mymatstats == value)
          return;
        _mymatstats = value;
        RaisePropertyChanged("MATStats");
      }
    }

    public ObservableCollection<MAT_Score> MAT
    {
      get
      {
        return _myMAT;
      }
      set
      {
        if (_myMAT == value)
          return;
        _myMAT = value;
        RaisePropertyChanged("MAT");
      }
    }

    public MatViewModel(IDataService Service)
    {
      _service = Service;
      Refresh();
    }

    private void Refresh()
    {
      MAT = _service.LoadMATScores();
      MATStats = _service.GetMATStats();
    }
  }
}
