// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.Introduction.IntroductionViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight;
using CETAP_LOB.Model;

namespace CETAP_LOB.ViewModel.Introduction
{
  public class IntroductionViewModel : ViewModelBase
  {
    private IDataService _service;

    public IntroductionViewModel(IDataService Service)
    {
      _service = Service;
    }
  }
}
