

using ClosedXML.Excel;
using CETAP_LOB.BDO;
using CETAP_LOB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CETAP_LOB.Model.scoring
{
  public class ReadExcelBioFile
  {
    private string _filename = "";
    private List<AnswerSheetBio> bio = new List<AnswerSheetBio>();
    private List<VenueBDO> _venues = new List<VenueBDO>();
    public IDataService _service;

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

    public List<AnswerSheetBio> BioDetails
    {
      get
      {
        return bio;
      }
      set
      {
        bio = value;
      }
    }

    public ReadExcelBioFile(string File, List<VenueBDO> venues)
    {
      _filename = File;
      _venues = venues;
      ReadExcelFile();
    }

    private async void ReadExcelFile()
    {
      IXLWorksheet xlWorksheet = new XLWorkbook(_filename).Worksheet(1);
      xlWorksheet.FirstRowUsed();
      foreach (IXLTableRow row in (IEnumerable<IXLTableRow>) xlWorksheet.Range(xlWorksheet.FirstCellUsed().Address, xlWorksheet.LastCellUsed().Address).AsTable().DataRange.Rows((Func<IXLTableRow, bool>) null))
      {
        AnswerSheetBio myBio = new AnswerSheetBio();
        myBio.NBT = Convert.ToInt64(row.Field("RefNo").GetString());
        if(!row.Field("Barcode").IsEmpty())
        myBio.Barcode = Convert.ToInt64(row.Field("Barcode").GetString());
        myBio.Surname = row.Field("SURNAME").GetString();
        if (!row.Field("FIRST_NAME").IsEmpty())
         myBio.Name = row.Field("FIRST_NAME").GetString();
        if (!row.Field("INITALS").IsEmpty())
          myBio.Initials = row.Field("INITALS").GetString();
        if (!row.Field("ID_NUMBER").IsEmpty())
          myBio.SAID = row.Field("ID_NUMBER").GetString();
        if (!row.Field("ID_Foreign").IsEmpty())
          myBio.ForeignID = row.Field("ID_Foreign").GetString();
        if (!row.Field("ID_Type").IsEmpty())
          myBio.ID_Type = row.Field("ID_Type").GetString().Trim();
        if (!row.Field("Gender").IsEmpty())
          myBio.Gender = row.Field("Gender").GetString().Trim();
        if (!row.Field("Citizenship").IsEmpty())
          myBio.Citizenship = new int?(Convert.ToInt32(row.Field("Citizenship").GetString().Trim()));
        if (!row.Field("Home_Lang").IsEmpty())
          myBio.HomeLanguage = new int?(Convert.ToInt32(row.Field("Home_Lang").GetString().Trim()));
        if (!row.Field("GR12_Language").IsEmpty())
          myBio.Grade12_Language = row.Field("GR12_Language").GetString().Trim();
        if (!row.Field("Date_of_Birth").IsEmpty())
          myBio.DOB = HelperUtils.BioDate(row.Field("Date_of_Birth").GetString().Trim());
        if (!row.Field("DATE").IsEmpty())
          myBio.DOT = HelperUtils.BioDate(row.Field("DATE").GetString().Trim());
        if (!row.Field("Classification").IsEmpty())
          myBio.Classification = row.Field("Classification").GetString().Trim();
        if (!row.Field("HEALTH_SCI_APP").IsEmpty())
          myBio.Faculty1 = row.Field("HEALTH_SCI_APP").GetString().Trim();
        if (!row.Field("Faculty2").IsEmpty())
          myBio.Faculty2 = row.Field("Faculty2").GetString().Trim();
        if (!row.Field("Faculty3").IsEmpty())
          myBio.faculty3 = row.Field("Faculty3").GetString().Trim();
        if (!row.Field("AQL_CODE").IsEmpty())
          myBio.AQLCode = Convert.ToInt32(row.Field("AQL_CODE").GetString());
        if (!row.Field("MAT_CODE").IsEmpty())
          myBio.MatCode = new int?(Convert.ToInt32(row.Field("MAT_CODE").GetString()));
        if (!row.Field("AQL_LANG").IsEmpty())
          myBio.AQL_Language = row.Field("AQL_LANG").GetString().Trim();
        if (!row.Field("MAT_LANG").IsEmpty())
          myBio.Mat_Language = row.Field("MAT_LANG").GetString().Trim();
        //myBio.BatchFile = row.Field("BatchFile").GetString();
        myBio.VenueCode = Convert.ToInt32(row.Field("Test_Cen_Code").GetString().Trim());
        myBio.VenueName = _venues.Where<VenueBDO>((Func<VenueBDO, bool>) (x => x.VenueCode == myBio.VenueCode)).Select<VenueBDO, string>((Func<VenueBDO, string>) (x => x.ShortName)).FirstOrDefault<string>();
                if (!row.Field("AQL_TestNo").IsEmpty())
                    myBio.aqltestname = row.Field("AQL_TestNo").GetString();
                if (!row.Field("MAT_TestNo").IsEmpty())
                    myBio.matTestname = row.Field("MAT_TestNo").GetString();
        bio.Add(myBio);
      }
    }
  }
}
