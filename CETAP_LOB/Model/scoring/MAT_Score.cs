

namespace CETAP_LOB.Model.scoring
{
  public class MAT_Score
  {
    private long _id;
    private int? mat;

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

    public int? MAT
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
  }
}
