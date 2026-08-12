

namespace CETAP_LOB.Model.scoring
{
  public class ResponseMatrix : ModelBase
  {
    public const string BarcodePropertyName = "Barcode";
    public const string ResponsesPropertyName = "Responses";
    private long _barcode;
    private string[] _response;

    public long Barcode
    {
      get
      {
        return _barcode;
      }
      set
      {
        if (_barcode == value)
          return;
        _barcode = value;
        RaisePropertyChanged("Barcode");
      }
    }

    public string[] Responses
    {
      get
      {
        return _response;
      }
      set
      {
        if (_response == value)
          return;
        _response = value;
        RaisePropertyChanged("Responses");
      }
    }
  }
}
