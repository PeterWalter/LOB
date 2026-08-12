

using ClosedXML.Excel;
using System;
using System.Collections.Generic;

namespace CETAP_LOB.Model.scoring
{
  public class ReadExcel
  {
    private string _filename = "";
    private List<AQL_Score> aql = new List<AQL_Score>();
    private List<MAT_Score> mat = new List<MAT_Score>();
    private List<AL_Score> al = new List<AL_Score>();
    private List<QL_Score> ql = new List<QL_Score>();

    private string _type;

    public string Filename
    {
      get
      {
        return _filename;
      }
      set
      {
        _filename = value;
      }
    }

        public List<AL_Score> ALScores 
        { 
            get 
            { 
                return al; 
            } 
            set 
            { 
                al = value; 
            } 
        }
        public List<QL_Score> QLScores 
        { 
            get 
            {
                return ql; 
            } 
            set
            {
                ql=value; 
            } 
        }
        public List<AQL_Score> AQLScores
    {
      get
      {
        return aql;
      }
      set
      {
        aql = value;
      }
    }

    public List<MAT_Score> MATScores
    {
      get
      {
        return mat;
      }
      set
      {
        mat = value;
      }
    }

    public ReadExcel(string File, string type)
    {
      _filename = File;
      _type = type;
      ReadExcelFile();
    }
  

        private void ReadExcelFile()
    {
      IXLWorksheet Worksheet = new XLWorkbook(_filename).Worksheet(1);
      foreach (IXLTableRow row in (IEnumerable<IXLTableRow>) Worksheet.Range(Worksheet.FirstCellUsed().Address, Worksheet.LastCellUsed().Address).AsTable().DataRange.Rows((Func<IXLTableRow, bool>) null))
      {
        if (!row.Field("ID").IsEmpty())
        {
          switch (_type)
          {
            case "AQL":
              AQL_Score aqlScore = new AQL_Score();
              string str1 = row.Field("ID").GetString();
              string valueCached1 = row.Field("AL_Score").ValueCached;
              string valueCached2 = row.Field("QL_Score").ValueCached;
              aqlScore.ID = Convert.ToInt64(str1);
              aqlScore.AL = new int?(Convert.ToInt32(valueCached1));
              aqlScore.QL = new int?(Convert.ToInt32(valueCached2));
              aql.Add(aqlScore);
              continue;
            case "MAT":
              MAT_Score matScore = new MAT_Score();
              string str2 = row.Field("ID").GetString();
              string valueCached3 = row.Field("MATScore").ValueCached;
              matScore.ID = Convert.ToInt64(str2);
              matScore.MAT = new int?(Convert.ToInt32(valueCached3));

              //string valueCached15 = row.Field("M1").ValueCached;
              //string valueCached16 = row.Field("M2").ValueCached;
              //string valueCached17 = row.Field("M3").ValueCached;
              //string valueCached18 = row.Field("M4").ValueCached;
              //string valueCached19 = row.Field("M5").ValueCached;

              //matScore.M1 = new int?(Convert.ToInt32(valueCached15));
              //matScore.M2 = new int?(Convert.ToInt32(valueCached16));
              //matScore.M3 = new int?(Convert.ToInt32(valueCached17));
              //matScore.M4 = new int?(Convert.ToInt32(valueCached18));
              //matScore.M5 = new int?(Convert.ToInt32(valueCached19));
              mat.Add(matScore);
              continue;
            case "AL":
             AL_Score al_Score = new AL_Score();
             string str3 = row.Field("ID").GetString();
             string valueCached4 = row.Field("ALScore").ValueCached;
             //string valueCached5 = row.Field("AL1").ValueCached;
             //string valueCached6 = row.Field("AL2").ValueCached;
             //string valueCached7 = row.Field("AL3").ValueCached;
             //string valueCached8 = row.Field("AL4").ValueCached;
             //string valueCached9 = row.Field("AL5").ValueCached;
             //string valueCached10 = row.Field("AL6").ValueCached;
             //string valueCached11 = row.Field("AL7").ValueCached;
             //string valueCached12 = row.Field("AL8").ValueCached;
             //string valueCached13 = row.Field("AL9").ValueCached;

             al_Score.ID = Convert.ToInt64(str3);
             al_Score.AL = new int?(Convert.ToInt32(valueCached4));
             //al_Score.AL1 = new int?(Convert.ToInt32(valueCached5));
             //al_Score.AL2 = new int?(Convert.ToInt32(valueCached6));
             //al_Score.AL3 = new int?(Convert.ToInt32(valueCached7));
             //al_Score.AL4 = new int?(Convert.ToInt32(valueCached8));
             //al_Score.AL5 = new int?(Convert.ToInt32(valueCached9));
             //al_Score.AL6 = new int?(Convert.ToInt32(valueCached10));
             //al_Score.AL7 = new int?(Convert.ToInt32(valueCached11));
             //al_Score.AL8 = new int?(Convert.ToInt32(valueCached12));
             //al_Score.AL9 = new int?(Convert.ToInt32(valueCached13));
             al.Add(al_Score);
              continue;
            case "QL":
              QL_Score ql_Score = new QL_Score();
              string str4 = row.Field("ID").GetString();
              string valueCached24 = row.Field("QLScore").ValueCached;
              //string valueCached25 = row.Field("S").ValueCached;
              //string valueCached26 = row.Field("C").ValueCached;
              //string valueCached27 = row.Field("R").ValueCached;
              //string valueCached28 = row.Field("P").ValueCached;
              //string valueCached29 = row.Field("Q").ValueCached;
              //string valueCached30 = row.Field("D").ValueCached;

               ql_Score.ID = Convert.ToInt64(str4);
               ql_Score.QL = new int?(Convert.ToInt32(valueCached24));
              // ql_Score.S = new int?(Convert.ToInt32(valueCached25));
              // ql_Score.C = new int?(Convert.ToInt32(valueCached26));
              // ql_Score.R = new int?(Convert.ToInt32(valueCached27));
              // ql_Score.P = new int?(Convert.ToInt32(valueCached28));
              // ql_Score.Q = new int?(Convert.ToInt32(valueCached29));
              //ql_Score.D = new int?(Convert.ToInt32(valueCached29));
              ql.Add(ql_Score);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }
}
