// Decompiled with JetBrains decompiler
// Type: LOB.Model.ICompositeService
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.BDO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CETAP_LOB.Model
{
  public interface ICompositeService
  {
    Task<List<CompositBDO>> GetAllNBTScoresAsync(int page, int size);

    CompositBDO getResultsByName(string name);

    CompositBDO getResultsBySurName(string surname);

    CompositBDO getResultsByID(long SAID);

    CompositBDO getResultsByFID(string FID);

    CompositBDO getResultsByDOB(DateTime DOB);

    CompositBDO getResultsByNBT(long NBT);

    bool UpdateComposit(CompositBDO results, ref string message);

    bool deleteComposit(CompositBDO results, ref string message);

    Task<bool> addComposit(CompositBDO results);

    bool GenerateCompositFromDB(string path);

    Task<bool> LogisticCompositeFromDB(string folder);

    void GetNoMatchFile(string filename);

    bool GenerateSelectedComposite(ObservableCollection<CompositBDO> mySelection, string folder);
  }
}
