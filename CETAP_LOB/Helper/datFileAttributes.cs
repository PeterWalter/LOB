// Decompiled with JetBrains decompiler
// Type: LOB.Helper.datFileAttributes
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using CETAP_LOB.Model;
using CETAP_LOB.Model.QA;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CETAP_LOB.Helper
{
  public class datFileAttributes : ModelBase
  {
    private string _mat_Lang = "";
    public const string SNamePropertyName = "SName";
    private int _profile;
    private int _venuecode;
    private string _filecomb;
    private string _filepath;
    private string _firstRecord;
    private int _randNumber;
    private int _recCount;
    private FileInfo _info;
    private string _clientType;
    private string _client;
    private string _testCode;
    private string _aql_Lang;
    private int _csx;
    private string _nameWithoutext;
    private string _ncs_ModifyFlag;
    private string _ncs_Errorflag;
    private bool _edited;

    public string SName
    {
      get
      {
        return _nameWithoutext;
      }
      set
      {
        if (_nameWithoutext == value)
          return;
        _nameWithoutext = value;
        if (_nameWithoutext.Length != 22)
          AddError("SName", "Wrong File Name");
        else
          RemoveError("SName");
        if (_nameWithoutext.Length == 22)
          SplitFileparts(_nameWithoutext);
        RaisePropertyChanged("SName");
      }
    }

    public int NoOfErrors { get; set; }

    public int RecordCount
    {
      get
      {
        return _recCount;
      }
      set
      {
        _recCount = value;
      }
    }

    public string AQL_Language
    {
      get
      {
        return _aql_Lang;
      }
      set
      {
        _aql_Lang = value;
      }
    }

    public bool Edited
    {
      get
      {
        return _edited;
      }
    }

    public string MAT_Language
    {
      get
      {
        return _mat_Lang;
      }
      set
      {
        _mat_Lang = value;
      }
    }

    public int RandNumber
    {
      get
      {
        return _randNumber;
      }
      set
      {
        value = _randNumber;
      }
    }

    public int VenueCode
    {
      get
      {
        return _venuecode;
      }
      set
      {
        _venuecode = value;
      }
    }

    public int Profile
    {
      get
      {
        return _profile;
      }
      set
      {
        _profile = value;
      }
    }

    public string FileCombination
    {
      get
      {
        return _filecomb;
      }
      set
      {
      }
    }

    public string FilePath
    {
      get
      {
        return _filepath;
      }
      set
      {
        _filepath = value;
      }
    }

    public FileInfo info
    {
      get
      {
        return _info;
      }
      set
      {
        _info = value;
      }
    }

    public string Client
    {
      get
      {
        return _client;
      }
      set
      {
        _client = value;
      }
    }

    public string FirstRecord
    {
      get
      {
        return _firstRecord;
      }
    }

    public int CSX
    {
      get
      {
        return _csx;
      }
      set
      {
        _csx = value;
      }
    }

    public string TestCode
    {
      get
      {
        return _testCode;
      }
    }

    public datFileAttributes()
    {
    }

    public datFileAttributes(string filepath)
    {
      _filepath = filepath;
      countRecords();
      readLineAsync();
      _nameWithoutext = Path.GetFileNameWithoutExtension(_filepath).ToUpper();
      if (_nameWithoutext.Length != 22)
        return;
      SplitFileparts(_nameWithoutext);
    }

        private void countRecords()
        {
            try
            {
                using (FileStream fileStream = File.Open(_filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)fileStream))
                    {
                        var lines = File.ReadAllLines(_filepath);
                        _recCount = lines.Length - 1;
                        streamReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    public async Task readLineAsync()
    {
      try
      {
        using (FileStream fileStream = File.Open(_filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          {
            string str = streamReader.ReadLine();
            _firstRecord = str;
       //                 _record = QADatRecord(str);
            _ncs_ModifyFlag = str.Substring(21, 1);
            _ncs_Errorflag = str.Substring(23, 1);
            if (_ncs_ModifyFlag == "Y" && _ncs_Errorflag == " ")
              _edited = true;
            _csx = Convert.ToInt32(str.Substring(0, 3));
            streamReader.Close();
          }
        }
      }
      catch (Exception ex)
      {
        throw ;
      }
    }

    public void SplitFileparts(string namefile)
    {
      _recCount = Convert.ToInt32(namefile.Substring(19, 3));
      _profile = Convert.ToInt32(namefile.Substring(4, 2));
      _testCode = namefile.Substring(0, 4);
      switch (_testCode)
      {
        case "0105":
          _filecomb = "AQLE";
          _aql_Lang = "E";
          break;
        case "0115":
          _filecomb = "AQLA";
          _aql_Lang = "A";
          break;
        case "0106":
          _filecomb = "MATE";
          _mat_Lang = "E";
          break;
        case "0116":
          _filecomb = "MATA";
          _mat_Lang = "A";
          break;
        case "0107":
          _filecomb = "AQLE_MATE";
          _aql_Lang = "E";
          _mat_Lang = "E";
          break;
        case "0117":
          _filecomb = "AQLA_MATA";
          _aql_Lang = "A";
          _mat_Lang = "A";
          break;
        case "0127":
          _filecomb = "AQLE_MATA";
          _aql_Lang = "E";
          _mat_Lang = "A";
          break;
        case "0137":
          _filecomb = "AQLA_MATE";
          _aql_Lang = "A";
          _mat_Lang = "E";
          break;
        default:
          AddError("SName", "No Such Test Code");
          break;
      }
      _clientType = namefile.Substring(18, 1);
      if (_clientType != "O")
      {
        _randNumber = Convert.ToInt32(namefile.Substring(13, 5));
        _venuecode = Convert.ToInt32(namefile.Substring(7, 5));
      }
      else
      {
        _randNumber = Convert.ToInt32(namefile.Substring(11, 5));
        _venuecode = Convert.ToInt32(namefile.Substring(6, 5));
      }
      switch (_clientType)
      {
        case "R":
          _client = "National";
          break;
        case "X":
          _client = "Re-score";
          break;
        case "M":
          _client = "Moderated";
          break;
        case "W":
          _client = "Remote";
          break;
        case "S":
          _client = "Special";
          break;
        case "O":
          _client = "Walk-in Bio";
          break;
        case "D":
          _client = "Disability";
          break;
        case "B":
          _client = "Braille";
          break;
        case "L":
          _client = "Large Print";
          break;
        default:
          _client = "Unknown Client";
          AddError("SName", "No Such Client");
          break;
      }
    }

    public override string ToString()
    {
      return string.Format("{0}{1:00}C{2:00000}B{3:00000}{4}{5:000}", (object) _testCode, (object) _profile, (object) _venuecode, (object) _randNumber, (object) _clientType, (object) _recCount);
    }
  }
}
