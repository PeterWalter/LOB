// Decompiled with JetBrains decompiler
// Type: LOB.Helper.AppMessages
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using GalaSoft.MvvmLight.Messaging;
using System;

namespace CETAP_LOB.Helper
{
  public class AppMessages
  {
    public static class WriterIsValid
    {
      public static void Send(bool argument)
      {
        Messenger.Default.Send<bool>(argument);
      }

      public static void Register(object recipient, Action<bool> action)
      {
        Messenger.Default.Register<bool>(recipient, action);
      }
    }
  }
}
