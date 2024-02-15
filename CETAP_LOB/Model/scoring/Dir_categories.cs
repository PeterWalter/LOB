

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CETAP_LOB.Model.scoring
{
  public class Dir_categories
  {
    private string _folder = "";
    private List<string> _filesinFolder = new List<string>();
    private List<string> _ResponseMatrix = new List<string>();
    private List<string> _AQLScorefiles = new List<string>();
    private List<string> _MATScorefiles = new List<string>();
    private string _AnswersheetBio = "";

    public string AnswerSheetBio
    {
      get
      {
        return _AnswersheetBio;
      }
      set
      {
        _AnswersheetBio = value;
      }
    }

    public List<string> AQLScorefiles
    {
      get
      {
        return _AQLScorefiles;
      }
      set
      {
        _AQLScorefiles = value;
      }
    }

    public List<string> MATScoresfiles
    {
      get
      {
        return _MATScorefiles;
      }
      set
      {
        _MATScorefiles = value;
      }
    }

    public string Dir
    {
      get
      {
        return _folder;
      }
      set
      {
        _folder = value;
      }
    }

    public List<string> ResponseMatrix
    {
      get
      {
        return _ResponseMatrix;
      }
      set
      {
        _ResponseMatrix = value;
      }
    }

    public List<string> FilesinFolder
    {
      get
      {
        return _filesinFolder;
      }
      set
      {
        _filesinFolder = value;
      }
    }

    public Dir_categories(string path)
    {
      _folder = path;
      SortFiles(((IEnumerable<string>) Directory.GetFiles(_folder)).ToList<string>());
    }

    private void SortFiles(List<string> thefiles)
    {
      foreach (string thefile in thefiles)
      {
        string fileName = Path.GetFileName(thefile);
        _filesinFolder.Add(fileName);
        if (Path.GetExtension(thefile) == ".xlsx")
        {
          string str = fileName.Split(' ')[0];
          switch (str.Substring(0, 3))
          {
            case "NBT":
              if (str.Substring(0, 7) == "NBT_Ans")
              {
                _AnswersheetBio = thefile;
                break;
              }
              break;
            case "AQL":
              _AQLScorefiles.Add(thefile);
              break;
            case "MAT":
              _MATScorefiles.Add(thefile);
              break;
          }
        }
        if (fileName.Contains("Response"))
          _ResponseMatrix.Add(thefile);
      }
    }
  }
}
