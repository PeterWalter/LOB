

namespace CETAP_LOB.Model.scoring
{
  public class AQL_Score
  {
    private long _id;
    private int? al;
    private int? ql;

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
  }
}
