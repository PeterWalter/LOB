

namespace CETAP_LOB.Model.scoring
{
  public class MAT_Score
  {
    private long _id;
    private int? mat;
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
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
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
        public int? M1 { get; set; }
        public int? M2 { get; set; }

        public int? M3 { get; set; }

        public int? M4 { get; set; }

        public int? M5 { get; set; }
    }
}
