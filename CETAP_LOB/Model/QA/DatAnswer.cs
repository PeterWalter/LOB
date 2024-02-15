

namespace CETAP_LOB.Model.QA
{
  public class DatAnswer : ModelBase
  {
    public const string errorCountPropertyName = "errorCount";
    public const string ValuePropertyName = "Value";
    private int _mycount;
    private char _myValue;

    public int errorCount
    {
      get
      {
        return _mycount;
      }
      set
      {
        if (_mycount == value)
          return;
        _mycount = value;
        RaisePropertyChanged("errorCount");
      }
    }

    public char Value
    {
      get
      {
        return _myValue;
      }
      set
      {
        if ((int) _myValue == (int) value)
          return;
        _myValue = value;
        bool flag = false;
        if ((int) _myValue == 65)
          flag = true;
        if ((int) _myValue == 66)
          flag = true;
        if ((int) _myValue == 67)
          flag = true;
        if ((int) _myValue == 68)
          flag = true;
        if ((int) _myValue == 78)
          flag = true;
        if ((int) _myValue == 88)
          flag = true;
        if ((int) _myValue == 32)
          flag = true;
        if (!flag)
          AddError("Value", "Score value not correct");
        else
          RemoveError("Value");
        checkerrors();
        RaisePropertyChanged("Value");
      }
    }

    private void checkerrors()
    {
      if (HasErrors)
        errorCount = _errors.Count;
      else
        errorCount = 0;
    }
  }
}
