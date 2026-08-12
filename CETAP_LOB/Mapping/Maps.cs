// Decompiled with JetBrains decompiler
// Type: LOB.Mapping.Maps
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Linq.Expressions;
using AutoMapper;
using CETAP_LOB.BDO;
using CETAP_LOB.Database;

namespace CETAP_LOB.Mapping
{
    public static class Maps
  {
        [Obsolete]
        public static void Initialize()
    {
      Mapper.CreateMap<ScanTrackerBDO, ScanTracker>().ReverseMap();
     // Mapper.CreateMap<ScanTracker, ScanTrackerBDO>();
      Mapper.CreateMap<CompositBDO, Composit>().ReverseMap();
    //  Mapper.CreateMap<Composit, CompositBDO>();
      Mapper.CreateMap<TestProfileBDO, TestProfile>();
      Mapper.CreateMap<TestProfile, TestProfileBDO>();
      Mapper.CreateMap<TestName, TestBDO>().ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.TestName1));
      Mapper.CreateMap<TestBDO, TestName>().ForMember(dest => dest.TestName1, opt => opt.MapFrom(src => src.TestName));
      Mapper.CreateMap<Batch, BatchBDO>().ForMember(dest => dest.RandomTestNumber, opt => opt.MapFrom(src => src.RandTestNumber));
      Mapper.CreateMap<BatchBDO, Batch>().ForMember(dest => dest.RandTestNumber, opt => opt.MapFrom(src => src.RandomTestNumber));
    }

        [Obsolete]
        public static TestProfile TestProfileBDOToTestProfile(TestProfileBDO testProfileBDO)
    {
      return Mapper.Map<TestProfile>((object) testProfileBDO);
    }

        [Obsolete]
        public static TestProfileBDO TestProfileToTestProfileBDO(TestProfile testProfile)
    {
      return Mapper.Map<TestProfileBDO>((object) testProfile);
    }

        [Obsolete]
        public static TestBDO TestDALToTestBDO(TestName test)
    {
      return Mapper.Map<TestBDO>((object) test);
    }

        [Obsolete]
        public static TestName TestBDOToTestDAL(TestBDO testBDO)
    {
      return Mapper.Map<TestName>((object) testBDO);
    }

        [Obsolete]
        public static BatchBDO BatchDALToBatchBDO(Batch batch)
    {
      return Mapper.Map<BatchBDO>((object) batch);
    }

        [Obsolete]
        public static Batch BatchBDOToBatchDAL(BatchBDO batchBDO)
    {
      return Mapper.Map<Batch>((object) batchBDO);
    }




        [Obsolete]
        public static ScanTracker ScanTrackerBDOToScanTrackerDAL(ScanTrackerBDO scantrackerBDO)
    {
            return Mapper.Map<ScanTracker>((object)scantrackerBDO);
    }

        [Obsolete]
        public static ScanTrackerBDO ScantrackerDALToScantrackerBDO(ScanTracker scantracker)
    {
      return Mapper.Map<ScanTrackerBDO>((object) scantracker);
    }
  }
}
