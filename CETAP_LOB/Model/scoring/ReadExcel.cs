

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
      IXLWorksheet xlWorksheet = new XLWorkbook(_filename).Worksheet(1);
      foreach (IXLTableRow row in (IEnumerable<IXLTableRow>) xlWorksheet.Range(xlWorksheet.FirstCellUsed().Address, xlWorksheet.LastCellUsed().Address).AsTable().DataRange.Rows((Func<IXLTableRow, bool>) null))
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
              string valueCached3 = row.Field("MAT_Score").ValueCached;
              matScore.ID = Convert.ToInt64(str2);
              matScore.MAT = new int?(Convert.ToInt32(valueCached3));
              mat.Add(matScore);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }
}
