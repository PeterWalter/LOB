

namespace CETAP_LOB.Model.scoring
{
  public class AQL_Score
  {
    private long _id;
    private int? al;
    private int? ql;
    private string _language;
    private int? _testcode;
        public long ID
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
      }
    }

    public int? AL
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
        public int? TestCode
        {
            get
            {
                return _testcode;
            }
            set
            {
                _testcode = value;
            }
        }
        public int? QL
    {
      get
      {
        return ql;
      }
      set
      {
        ql = value;
      }
    }
        public string Language 
        { get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }
    }
}
