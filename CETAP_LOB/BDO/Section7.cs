// Decompiled with JetBrains decompiler
// Type: LOB.BDO.Section7
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using CETAP_LOB.Model.QA;
using System.Collections.ObjectModel;
using System.Linq;

namespace CETAP_LOB.BDO
{
  public class Section7 : ViewModelBase
  {
    public const string TrialSectionPropertyName = "TrialSection";
    private ObservableCollection<DatAnswer> _mySect;

    public string Barcode { get; set; }

    public ObservableCollection<DatAnswer> TrialSection
    {
      get
      {
        return _mySect;
      }
      set
      {
        if (_mySect == value)
          return;
        _mySect = value;
        RaisePropertyChanged("TrialSection");
      }
    }

    public override string ToString()
    {
      return Barcode + ObsToString();
    }

    private string ObsToString()
    {
      string str = "";
      DatAnswer[] array = TrialSection.ToArray<DatAnswer>();
      for (int index = 0; index < 25; ++index)
        str = str + "," + (object) array[index].Value;
      return str;
    }
  }
}
