
// Decompiled with JetBrains decompiler
// Type: LOB.Model.CompositeService
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using ClosedXML.Excel;
using FirstFloor.ModernUI.Windows.Controls;
using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Mapping;
using CETAP_LOB.Model.scoring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CETAP_LOB.Model;

namespace CETAP_LOB.Model
{
  public class CompositeService : ICompositeService
  {
    private ObservableCollection<FullComposite> FComposite = new ObservableCollection<FullComposite>();
    private ObservableCollection<LogisticsComposite> LComposite = new ObservableCollection<LogisticsComposite>();
    private ObservableCollection<CompositBDO> AllScores;
    private ObservableCollection<CompositBDO> IntakeScores;
    private ObservableCollection<VenueBDO> AllVenues;
   // private ObservableCollection<CompositBDO> Composite;
    //private Dir_categories myDir;

        public Task<bool> addComposit(CompositBDO results)
        {
            throw new NotImplementedException();
        }

        public bool deleteComposit(CompositBDO results, ref string message)
        {
            throw new NotImplementedException();
        }

        public bool GenerateCompositFromDB(string path)
        {
            throw new NotImplementedException();
        }

        public bool GenerateSelectedComposite(ObservableCollection<CompositBDO> mySelection, string folder)
        {
            throw new NotImplementedException();
        }

       

        public void GetNoMatchFile(string filename)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByDOB(DateTime DOB)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByFID(string FID)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByID(long SAID)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByName(string name)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsByNBT(long NBT)
        {
            throw new NotImplementedException();
        }

        public CompositBDO getResultsBySurName(string surname)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogisticCompositeFromDB(string folder)
        {
            throw new NotImplementedException();
        }

        public bool UpdateComposit(CompositBDO results, ref string message)
        {
            throw new NotImplementedException();
        }

        private void CompositToFullComposite()
        {
          if (AllScores == null)
            return;
          foreach (var data in AllScores.AsParallel<CompositBDO>().Select(mydata => new
          {
            RefNo = mydata.RefNo.ToString(),
            Barcode = mydata.Barcode.ToString(),
            LastName = mydata.Surname,
            FName = mydata.Name,
            Initials = mydata.Initials,
            SAID = HelperUtils.ToSAID(mydata.SAID),
            FID = mydata.ForeignID,
            DOB = string.Format("{0:yyyyMMdd}", (object) mydata.DOB),
            IDType = mydata.ID_Type,
            Citizenship = mydata.Citizenship.HasValue ? mydata.Citizenship : new int?(),
            Classification = mydata.Classification,
            Gender = mydata.Gender,
            faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
            Testdate = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
            VenueCode = mydata.VenueCode.ToString("D5"),
            VenueName = mydata.VenueName,
            Hlanguage = mydata.HomeLanguage,
            G12L = mydata.GR12Language,
            AQLLang = mydata.AQLLanguage,
            AQLCode = mydata.AQLCode,
            MatLang = mydata.MatLanguage,
            MatCode = mydata.MatCode,
            Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
            Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
            SessionID = mydata.Barcode.ToString(),
            NBTNumber = mydata.RefNo.ToString(),
            Surname = mydata.Surname,
            Name = mydata.Name,
            Initial = mydata.Initials,
            SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
            Passport = mydata.ForeignID,
            Birth = mydata.SAID.HasValue ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) : string.Format("{0:dd/MM/yyyy}", (object) mydata.DOB),
            W_AL = mydata.WroteAL,
            W_QL = mydata.WroteQL,
            W_Mat = mydata.WroteMat,
            StNo = "",
            Faculty = "",
            Programme = "",
            DateTest = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
            Venue = mydata.VenueName,
            Sex = mydata.Gender == "1" ? "M" : (mydata.Gender == "2" ? "F" : mydata.Gender),
            street1 = "",
            street2 = "",
            Suburb = "",
            City = "",
            Province = "",
            Postal = "",
            Email = "",
            Landline = "",
            Mobile = "",
            ALScore = mydata.ALScore,
            ALLevel = mydata.ALLevel,
            QLScore = mydata.QLScore,
            QLLevel = mydata.QLLevel,
            MatScore = mydata.MATScore,
            MatLevel = mydata.MATLevel,
            AQL_Lang = mydata.AQLLanguage,
            Mat_Lang = mydata.MatLanguage
          }))
            FComposite.Add(new FullComposite()
            {
              RefNo = data.RefNo,
              Barcode = data.Barcode,
              LastName = data.LastName,
              FirstName = data.FName,
              Initials = data.Initials,
              SAID = data.SAID,
              FID = data.FID,
              DOB = data.DOB,
              IDType = data.IDType,
              Citizenship = data.Citizenship.ToString(),
              Classification = data.Classification,
              Gender = data.Gender,
              Faculty1 = data.faculty1,
              DOT = data.Testdate,
              VenueCode = data.VenueCode,
              VenueName = data.VenueName,
              HomeLanguage = data.Hlanguage.ToString(),
              SchLanguage = data.G12L,
              AQLLang = data.AQLLang,
              AQLCode = data.AQLCode.ToString(),
              MATLang = data.MatLang,
              MATCode = data.MatCode.ToString(),
              Faculty2 = data.Faculty2,
              Faculty3 = data.Faculty3,
              SessionId = data.SessionID,
              NBT = data.NBTNumber,
              Surname = data.Surname,
              Name = data.Name,
              MiddleInitials = data.Initial,
              SouthAfricanID = data.SouthAfricanID,
              Passport = data.Passport,
              Birth = data.Birth,
              WroteAL = data.W_AL,
              WroteQL = data.W_QL,
              WroteMat = data.W_Mat,
              StudentNo = data.StNo,
              Faculty = data.Faculty,
              Programme = data.Programme,
              TestDate = data.DateTest,
              Venue = data.Venue,
              Sex = data.Sex,
              Street1 = data.street1,
              Street2 = data.street2,
              Suburb = data.Suburb,
              City = data.City,
              Province = data.Province,
              Postal = data.Postal,
              Email = data.Email,
              Landline = data.Landline,
              Mobile = data.Mobile,
              ALScore = data.ALScore.ToString(),
              ALLevel = data.ALLevel,
              QLScore = data.QLScore.ToString(),
              QLLevel = data.QLLevel,
              MatScore = data.MatScore.ToString(),
              MatLevel = data.MatLevel,
              AQLLanguage = data.AQL_Lang,
              MatLanguage = data.Mat_Lang
            });
        }

    private void CompositToLogisticsComposite()
    {
      if (AllScores == null)
        return;
      foreach (var data in AllScores.AsParallel<CompositBDO>().Select(mydata => new
      {
        SessionID = mydata.Barcode.ToString(),
        NBTNumber = mydata.RefNo.ToString(),
        Surname = mydata.Surname,
        Name = mydata.Name,
        Initial = mydata.Initials,
        SouthAfricanID = mydata.SAID.ToString(),
        Passport = mydata.ForeignID,
        Birth = mydata.SAID.HasValue ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) : string.Format("{0:dd/MM/yyyy}", (object) mydata.DOB),
        W_AL = mydata.WroteAL,
        W_QL = mydata.WroteQL,
        W_Mat = mydata.WroteMat,
        StNo = "",
        Faculty = "",
        Programme = "",
        DateTest = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        Venue = mydata.VenueName,
        Sex = mydata.Gender == "1" ? "M" : (mydata.Gender == "2" ? "F" : mydata.Gender),
        ALScore = mydata.ALScore,
        ALLevel = mydata.ALLevel,
        QLScore = mydata.QLScore,
        QLLevel = mydata.QLLevel,
        MatScore = mydata.MATScore,
        MatLevel = mydata.MATLevel,
        AQL_Lang = mydata.AQLLanguage,
        Mat_Lang = mydata.MatLanguage,
        mm = mydata.Batch,
        TestType = MytestType(mydata.Batch),
        province = getProvince(mydata.VenueCode)
      }))
        LComposite.Add(new LogisticsComposite()
        {
          SessionId = data.SessionID,
          NBT = data.NBTNumber,
          Surname = data.Surname,
          Name = data.Name,
          MiddleInitials = data.Initial,
          SouthAfricanID = data.SouthAfricanID,
          Passport = data.Passport,
          Birth = data.Birth,
          WroteAL = data.W_AL,
          WroteQL = data.W_QL,
          WroteMat = data.W_Mat,
          StudentNo = data.StNo,
          Faculty = data.Faculty,
          Programme = data.Programme,
          TestDate = data.DateTest,
          Venue = data.Venue,
          Sex = data.Sex,
          ALScore = data.ALScore.ToString(),
          ALLevel = data.ALLevel,
          QLScore = data.QLScore.ToString(),
          QLLevel = data.QLLevel,
          MatScore = data.MatScore.ToString(),
          MatLevel = data.MatLevel,
          AQLLanguage = data.AQL_Lang,
          MatLanguage = data.Mat_Lang,
          TestType = data.TestType,
          VenueProvince = data.province
        });
    }

        //public bool GenerateSelectedComposite(ObservableCollection<CompositBDO> mySelection, string folder)
        //{
        //    Composite = mySelection;
        //    myDir = new Dir_categories(folder);
        //    return GenerateExcelComposite();
        //}

        public async Task<List<CompositBDO>> GetYearScoresAsync()
        {
            List<CompositBDO> NBTScores = new List<CompositBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (CETAPEntities cetapEntities = new CETAPEntities())
                {

                }
             }
            return NBTScores;
        }
        public async Task<List<CompositBDO>> GetAllNBTScoresAsync(int page, int size)
        {
            List<CompositBDO> NBTScores = new List<CompositBDO>();
            if (ApplicationSettings.Default.DBAvailable)
            {
                using (CETAPEntities cetapEntities = new CETAPEntities())
                {
                    try
                    {
                        List<Composit> scores = await cetapEntities.Composits.OrderBy(x => x.DOT).Skip(page * size).Take(size).ToListAsync();
                        foreach (Composit composit in scores)
                        {
                            CompositBDO compositBdo = new CompositBDO();
                            NBTScores.Add(Maps.CompositDALToCompositBDO(composit));
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            AllScores = new ObservableCollection<CompositBDO>(NBTScores);
            return NBTScores;
        }

        private string getProvince(int venuecode)
        {
            return getProvinceByID(AllVenues.Where(x => x.VenueCode == venuecode).FirstOrDefault().ProvinceID).Name;
        }

        private string MytestType(string datfile)
        {
            return new datFileAttributes() { SName = datfile }.Client;
        }

        //private void GenerateCompFromDB(string folder)
        //{
        //  IEnumerable<\u003C\u003Ef__AnonymousType0<string, string, string, string, string, string, string, string, string, int?, string, string, string, string, string, string, int?, string, string, int?, string, int?, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int?, string, int?, string, int?, string, string, string>> source = AllScores.Select(mydata => new
        //  {
        //    RefNo = mydata.RefNo.ToString(),
        //    Barcode = mydata.Barcode.ToString(),
        //    LastName = mydata.Surname,
        //    FName = mydata.Name,
        //    Initials = mydata.Initials,
        //    SAID = HelperUtils.ToSAID(mydata.SAID),
        //    FID = mydata.ForeignID,
        //    DOB = string.Format("{0:yyyyMMdd}", (object) mydata.DOB),
        //    IDType = mydata.ID_Type,
        //    Citizenship = mydata.Citizenship,
        //    Classification = mydata.Classification,
        //    Gender = mydata.Gender,
        //    faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
        //    Testdate = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        //    VenueCode = mydata.VenueCode.ToString("D5"),
        //    VenueName = mydata.VenueName,
        //    Hlanguage = mydata.HomeLanguage,
        //    G12L = mydata.GR12Language,
        //    AQLLang = mydata.AQLLanguage,
        //    AQLCode = mydata.AQLCode,
        //    MatLang = mydata.MatLanguage,
        //    MatCode = mydata.MatCode,
        //    Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
        //    Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
        //    SessionID = mydata.Barcode.ToString(),
        //    NBTNumber = mydata.RefNo.ToString(),
        //    Surname = mydata.Surname,
        //    Name = mydata.Name,
        //    Initial = mydata.Initials,
        //    SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
        //    Passport = mydata.ForeignID,
        //    Birth = mydata.SAID.HasValue ? HelperUtils.DOBfromSAID(mydata.SAID.ToString()) : string.Format("{0:dd/MM/yyyy}", (object) mydata.DOB),
        //    W_AL = mydata.WroteAL,
        //    W_QL = mydata.WroteQL,
        //    W_Mat = mydata.WroteMat,
        //    StNo = "",
        //    Faculty = "",
        //    Programme = "",
        //    DateTest = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        //    Venue = mydata.VenueName,
        //    Sex = mydata.Gender == "1" ? "M" : (mydata.Gender == "2" ? "F" : mydata.Gender),
        //    street1 = "",
        //    street2 = "",
        //    Suburb = "",
        //    City = "",
        //    Province = "",
        //    Postal = "",
        //    Email = "",
        //    Landline = "",
        //    Mobile = "",
        //    ALScore = mydata.ALScore,
        //    ALLevel = mydata.ALLevel,
        //    QLScore = mydata.QLScore,
        //    QLLevel = mydata.QLLevel,
        //    MatScore = mydata.MATScore,
        //    MatLevel = mydata.MATLevel,
        //    AQL_Lang = mydata.AQLLanguage,
        //    Mat_Lang = mydata.MatLanguage
        //  });
        //  XLWorkbook xlWorkbook = new XLWorkbook();
        //  IXLWorksheet xlWorksheet = xlWorkbook.Worksheets.Add("Composite").SetTabColor(XLColor.Almond);
        //  xlWorksheet.Columns("A1:BF1").Width = 13.0;
        //  xlWorksheet.Range("A1:X1").Style.Fill.BackgroundColor = XLColor.TealBlue;
        //  xlWorksheet.Range("Y1:Y1").Style.Fill.BackgroundColor = XLColor.Orange;
        //  xlWorksheet.Range("Z1:AI1").Style.Fill.BackgroundColor = XLColor.Yellow;
        //  xlWorksheet.Range("AJ1:AL1").Style.Fill.BackgroundColor = XLColor.Magenta;
        //  xlWorksheet.Range("AM1:AX1").Style.Fill.BackgroundColor = XLColor.GreenYellow;
        //  xlWorksheet.Range("AY1:BD1").Style.Fill.BackgroundColor = XLColor.LightGray;
        //  xlWorksheet.Range("BE1:BF1").Style.Fill.BackgroundColor = XLColor.Yellow;
        //  IXLRow xlRow = xlWorksheet.Row(1);
        //  xlRow.Height = 30.0;
        //  xlRow.Style.Font.Bold = true;
        //  xlRow.Style.Font.FontSize = 9.0;
        //  xlRow.Style.Alignment.WrapText = true;
        //  xlRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //  xlRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //  xlWorksheet.Cell(1, 1).Value = (object) "Ref No";
        //  xlWorksheet.Cell(1, 2).Value = (object) "Barcode";
        //  xlWorksheet.Cell(1, 3).Value = (object) "Last Name";
        //  xlWorksheet.Cell(1, 4).Value = (object) "First_Name";
        //  xlWorksheet.Cell(1, 5).Value = (object) "INITIALS";
        //  xlWorksheet.Cell(1, 6).Value = (object) "ID NUMBER";
        //  xlWorksheet.Cell(1, 7).Value = (object) "ID_Foreign";
        //  xlWorksheet.Cell(1, 8).Value = (object) "Date of Birth";
        //  xlWorksheet.Cell(1, 9).Value = (object) "ID Type";
        //  xlWorksheet.Cell(1, 10).Value = (object) "Citizenship";
        //  xlWorksheet.Cell(1, 11).Value = (object) "Classification";
        //  xlWorksheet.Cell(1, 12).Value = (object) "Gender 1";
        //  xlWorksheet.Cell(1, 13).Value = (object) "Faculty 1";
        //  xlWorksheet.Cell(1, 14).Value = (object) "DATE";
        //  xlWorksheet.Cell(1, 15).Value = (object) "Test Centre Code";
        //  xlWorksheet.Cell(1, 16).Value = (object) "Venue Name";
        //  xlWorksheet.Cell(1, 17).Value = (object) "Home Lang";
        //  xlWorksheet.Cell(1, 18).Value = (object) "GR12 Language";
        //  xlWorksheet.Cell(1, 19).Value = (object) "AQL LANG";
        //  xlWorksheet.Cell(1, 20).Value = (object) "AQL CODE";
        //  xlWorksheet.Cell(1, 21).Value = (object) "MAT LANG";
        //  xlWorksheet.Cell(1, 22).Value = (object) "MAT CODE";
        //  xlWorksheet.Cell(1, 23).Value = (object) "Faculty 2";
        //  xlWorksheet.Cell(1, 24).Value = (object) "Faculty 3";
        //  xlWorksheet.Cell(1, 25).Value = (object) "Test Session ID";
        //  xlWorksheet.Cell(1, 26).Value = (object) "NBT Reference";
        //  xlWorksheet.Cell(1, 27).Value = (object) "Surname";
        //  xlWorksheet.Cell(1, 28).Value = (object) "First Name";
        //  xlWorksheet.Cell(1, 29).Value = (object) "Middle Initials";
        //  xlWorksheet.Cell(1, 30).Value = (object) "South African ID";
        //  xlWorksheet.Cell(1, 31).Value = (object) "Foreign ID";
        //  xlWorksheet.Cell(1, 32).Value = (object) "Date of Birth";
        //  xlWorksheet.Cell(1, 33).Value = (object) "Wrote AL";
        //  xlWorksheet.Cell(1, 34).Value = (object) "Wrote QL";
        //  xlWorksheet.Cell(1, 35).Value = (object) "Wrote Maths";
        //  xlWorksheet.Cell(1, 36).Value = (object) "Student Number";
        //  xlWorksheet.Cell(1, 37).Value = (object) "Faculty";
        //  xlWorksheet.Cell(1, 38).Value = (object) "Programme";
        //  xlWorksheet.Cell(1, 39).Value = (object) "Date_of_Test";
        //  xlWorksheet.Cell(1, 40).Value = (object) "Venue";
        //  xlWorksheet.Cell(1, 41).Value = (object) "Gender";
        //  xlWorksheet.Cell(1, 42).Value = (object) "Street and Number";
        //  xlWorksheet.Cell(1, 43).Value = (object) "Street Name";
        //  xlWorksheet.Cell(1, 44).Value = (object) "Suburb";
        //  xlWorksheet.Cell(1, 45).Value = (object) "City/Town";
        //  xlWorksheet.Cell(1, 46).Value = (object) "Province/Region";
        //  xlWorksheet.Cell(1, 47).Value = (object) "Postal Code";
        //  xlWorksheet.Cell(1, 48).Value = (object) "e-mail Address";
        //  xlWorksheet.Cell(1, 49).Value = (object) "Landline Number";
        //  xlWorksheet.Cell(1, 50).Value = (object) "Mobile Number";
        //  xlWorksheet.Cell(1, 51).Value = (object) "AL Score";
        //  xlWorksheet.Cell(1, 52).Value = (object) "AL Performance";
        //  xlWorksheet.Cell(1, 53).Value = (object) "QL Score";
        //  xlWorksheet.Cell(1, 54).Value = (object) "QL Performance";
        //  xlWorksheet.Cell(1, 55).Value = (object) "Maths Score";
        //  xlWorksheet.Cell(1, 56).Value = (object) "Maths Performance";
        //  xlWorksheet.Cell(1, 57).Value = (object) "AQL TEST LANGUAGE";
        //  xlWorksheet.Cell(1, 58).Value = (object) "MATHS TEST LANGUAGE";
        //  xlWorksheet.Cell(2, 1).Value = (object) source.AsEnumerable();
        //  DateTime dateTime = new DateTime();
        //  DateTime now = DateTime.Now;
        //  string str = now.Year.ToString() + now.Month.ToString("00") + now.Day.ToString("00") + "_" + now.Hour.ToString("00") + now.Minute.ToString("00");
        //  string file = folder + "_" + str + "n=" + (object) source.Count() + ".xlsx";
        //  xlWorkbook.SaveAs(file);
        //}

        //public CompositBDO getResultsByName(string name)
        //{
        //  throw new NotImplementedException();
        //}

        //public CompositBDO getResultsBySurName(string surname)
        //{
        //  throw new NotImplementedException();
        //}

        //public CompositBDO getResultsByID(long SAID)
        //{
        //  throw new NotImplementedException();
        //}

        //public CompositBDO getResultsByFID(string FID)
        //{
        //  throw new NotImplementedException();
        //}

        //public CompositBDO getResultsByDOB(DateTime DOB)
        //{
        //  throw new NotImplementedException();
        //}

        //public CompositBDO getResultsByNBT(long NBT)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool UpdateComposit(CompositBDO results, ref string message)
        //{
        //    bool flag = false;
        //    message = "Scores successfully updated";
        //    using (CETAPEntities cetapEntities = new CETAPEntities())
        //    {
        //        long ID = results.Barcode;
        //        Composit composit = cetapEntities.Composits.Where<Composit>((System.Linq.Expressions.Expression<Func<Composit, bool>>)(x => x.Barcode == ID)).FirstOrDefault<Composit>();
        //        if (composit != null)
        //        {
        //            flag = true;
        //            cetapEntities.Composits.Remove(composit);
        //            CompositBDOToCompositDAL(results, composit);
        //            composit.DateModified = DateTime.Now;
        //            cetapEntities.Composits.Attach(composit);
        //            cetapEntities.Entry<Composit>(composit).State = EntityState.Modified;
        //            cetapEntities.SaveChanges();
        //            results.RowVersion = composit.RowVersion;
        //        }
        //        else
        //            message = "No Writer with ID " + (object)results.Barcode;
        //    }
        //    return flag;
        //}

        public ProvinceBDO getProvinceByID(int code)
        {
            ProvinceBDO provinceBDO = null;
            int ProvID = code;
            using (var context = new CETAPEntities())
            {
                Province prov = context.Provinces.Where(x => x.Code == ProvID).FirstOrDefault();

                if (prov != null)
                {
                    provinceBDO = new ProvinceBDO();
                    ProvinceToProvinceBDO(provinceBDO, prov);

                }

            }
            return provinceBDO;
        }
      
        public string GetWebSiteNameByVenueCode(int venueCode)
        {
            string webname = "";

            using (var context = new CETAPEntities())
            {
                TestVenue venue = context.TestVenues.Where(x => x.VenueCode == venueCode).FirstOrDefault();

                if (venue != null)
                {

                    webname = venue.WebsiteName;
                }
            }
            return webname;
        }

        #region Translate
        static void CompositBDOToCompositDAL(CompositBDO compositBDO, Composit composit)
        {
            composit.ALScore = compositBDO.ALScore;
            composit.AQLCode = compositBDO.AQLCode;
            composit.AQLLanguage = compositBDO.AQLLanguage;
            composit.Barcode = compositBDO.Barcode;
            composit.Citizenship = compositBDO.Citizenship;
            composit.Classification = compositBDO.Classification;
            composit.DOB = compositBDO.DOB;
            composit.DOT = compositBDO.DOT;
            composit.ALLevel = compositBDO.ALLevel;
            composit.Faculty = compositBDO.Faculty;
            composit.Faculty2 = compositBDO.Faculty2;
            composit.Faculty3 = compositBDO.Faculty3;
            composit.ForeignID = compositBDO.ForeignID;
            composit.Gender = compositBDO.Gender;
            composit.GR12Language = compositBDO.GR12Language;
            composit.HomeLanguage = compositBDO.HomeLanguage.ToString();
            composit.ID_Type = compositBDO.ID_Type;
            composit.Initials = compositBDO.Initials;
            composit.MatCode = compositBDO.MatCode;
            composit.MatLanguage = compositBDO.MatLanguage;
            composit.MATLevel = compositBDO.MATLevel;
            composit.MATScore = compositBDO.MATScore;
            composit.Name = compositBDO.Name;
            composit.QLLevel = compositBDO.QLLevel;
            composit.QLScore = compositBDO.QLScore;
            composit.RefNo = compositBDO.RefNo;
            composit.RowGuid = compositBDO.RowGuid;
            composit.RowVersion = compositBDO.RowVersion;
            composit.SAID = compositBDO.SAID;
            composit.Surname = compositBDO.Surname;
            composit.VenueName = compositBDO.VenueName;
            composit.VenueCode = compositBDO.VenueCode;
            composit.WroteAL = compositBDO.WroteAL;
            composit.WroteQL = compositBDO.WroteQL;
            composit.WroteMat = compositBDO.WroteMat;
        }
        static void CompositDALToCompositBDO(CompositBDO compositBDO, Composit composit)
        {
            compositBDO.ALScore = composit.ALScore;
            compositBDO.AQLCode = composit.AQLCode;
            compositBDO.AQLLanguage = composit.AQLLanguage;
            compositBDO.Barcode = composit.Barcode;
            compositBDO.Citizenship = composit.Citizenship;
            compositBDO.Classification = composit.Classification;
            compositBDO.DOB = composit.DOB;
            compositBDO.DOT = composit.DOT;
            compositBDO.ALLevel = composit.ALLevel;
            compositBDO.Faculty = composit.Faculty;
            compositBDO.Faculty2 = composit.Faculty2;
            compositBDO.Faculty3 = composit.Faculty3;
            compositBDO.ForeignID = composit.ForeignID;
            compositBDO.Gender = composit.Gender;
            compositBDO.GR12Language = composit.GR12Language;
            compositBDO.HomeLanguage = (int?)Convert.ToInt32(composit.HomeLanguage);
            compositBDO.ID_Type = composit.ID_Type;
            compositBDO.Initials = composit.Initials;
            compositBDO.MatCode = composit.MatCode;
            compositBDO.MatLanguage = composit.MatLanguage;
            compositBDO.MATLevel = composit.MATLevel;
            compositBDO.MATScore = composit.MATScore;
            compositBDO.Name = composit.Name;
            compositBDO.QLLevel = composit.QLLevel;
            compositBDO.QLScore = composit.QLScore;
            compositBDO.RefNo = composit.RefNo;
            compositBDO.RowGuid = composit.RowGuid;
            compositBDO.RowVersion = composit.RowVersion;
            compositBDO.SAID = composit.SAID;
            compositBDO.Surname = composit.Surname;
            compositBDO.VenueName = composit.VenueName;
            compositBDO.VenueCode = composit.VenueCode;
            compositBDO.WroteAL = composit.WroteAL;
            compositBDO.WroteQL = composit.WroteQL;
            compositBDO.WroteMat = composit.WroteMat;
            compositBDO.DateModified = composit.DateModified;
            compositBDO.Batch = compositBDO.Batch;

        }
        static void ProvinceDALToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        {
            provinceBDO.Code = province.Code;
            provinceBDO.Id = province.Id;
            provinceBDO.Name = province.Name;
            provinceBDO.RowVersion = province.RowVersion;
        }
        static void provinceBDOtoprovinceDAL(ProvinceBDO provinceBDO, Province province)
        {
            province.Code = provinceBDO.Code;
            province.Id = provinceBDO.Id;
            province.Name = provinceBDO.Name;
            province.RowVersion = provinceBDO.RowVersion;

        }
        static void ProvinceToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        {
            provinceBDO.Code = province.Code;
            provinceBDO.Id = province.Id;
            provinceBDO.Name = province.Name;
            provinceBDO.RowVersion = province.RowVersion;
            //provinceBDO.Code = province.Code;
            //provinceBDO.Code = province.Code;
        }
        #endregion
        //public bool deleteComposit(CompositBDO results, ref string message)
        //{
        //  throw new NotImplementedException();
        //}

        //public async Task<bool> addComposit(CompositBDO results)
        //{
        //  bool ret = false;
        //  string message = results.RefNo.ToString() + " Not saved :";
        //  using (CETAPEntities cetapEntities = new CETAPEntities())
        //  {
        //    try
        //    {
        //      cetapEntities.Composits.Add(new Composit()
        //      {
        //        RefNo = results.RefNo,
        //        Surname = results.Surname,
        //        Name = results.Name,
        //        Initials = results.Initials,
        //        SAID = !results.SAID.HasValue ? new long?() : new long?(Convert.ToInt64((object) results.SAID)),
        //        ForeignID = results.ForeignID,
        //        Gender = results.Gender,
        //        DOB = results.DOB,
        //        Classification = results.Classification,
        //        AQLLanguage = results.AQLLanguage,
        //        MatLanguage = results.MatLanguage,
        //        HomeLanguage = results.HomeLanguage.ToString(),
        //        GR12Language = results.GR12Language,
        //        ID_Type = results.ID_Type,
        //        Barcode = results.Barcode,
        //        DOT = results.DOT,
        //        Citizenship = results.Citizenship,
        //        Faculty = results.Faculty,
        //        VenueCode = results.VenueCode,
        //        VenueName = results.VenueName,
        //        AQLCode = results.AQLCode,
        //        MatCode = results.MatCode,
        //        ALScore = results.ALScore,
        //        QLScore = results.QLScore,
        //        MATScore = results.MATScore,
        //        MATLevel = results.MATLevel,
        //        ALLevel = results.ALLevel,
        //        QLLevel = results.QLLevel,
        //        WroteAL = results.WroteAL,
        //        WroteQL = results.WroteQL,
        //        WroteMat = results.WroteMat,
        //        Batch = results.Batch,
        //        Faculty2 = results.Faculty2,
        //        Faculty3 = results.Faculty3,
        //        RowGuid = Guid.NewGuid(),
        //        DateModified = DateTime.Now
        //      });
        //      int num = await cetapEntities.SaveChangesAsync();
        //      ret = true;
        //    }
        //    catch (Exception ex)
        //    {
        //      ret = false;
        //      // ISSUE: explicit reference operation
        //      // ISSUE: reference to a compiler-generated field
        //      (^this).\u003Cmessage\u003E5__13 += ex.InnerException.ToString();
        //      int num = (int) ModernDialog.ShowMessage(message, "Record not saved", MessageBoxButton.OK, (Window) null);
        //    }
        //  }
        //  return ret;
        //}

        //public void GetNoMatchFile(string filename)
        //{
        //  string directoryName = Path.GetDirectoryName(filename);
        //  string str1 = "UCT_matched";
        //  ReadBI readBi = new ReadBI(filename);
        //  List<BI> biList = new List<BI>();
        //  List<BI> biDetails = readBi.BiDetails;
        //  List<Composit> outer = new List<Composit>();
        //  using (CETAPEntities cetapEntities = new CETAPEntities())
        //    outer = cetapEntities.Composits.ToList<Composit>();
        //  IEnumerable<\u003C\u003Ef__AnonymousType3<Composit, string, string, DateTime, string, long>> datas1 = outer.Join((IEnumerable<BI>) biDetails, comp => new
        //  {
        //    NBT = comp.RefNo,
        //    SAID = comp.SAID.ToString()
        //  }, match => new{ NBT = match.NBT, SAID = match.SAID }, (comp, match) => new
        //  {
        //    composit = comp,
        //    Surname = match.Surname.ToUpper().Trim(),
        //    Name = match.Name.ToUpper().Trim(),
        //    DOB = match.DOB,
        //    SAID = match.SAID,
        //    NBT = match.NBT
        //  }).ToList().Distinct();
        //  datas1.Count();
        //  IEnumerable<\u003C\u003Ef__AnonymousType3<Composit, string, string, DateTime, string, long>> datas2 = outer.Join((IEnumerable<BI>) biDetails, comp => new
        //  {
        //    Name = comp.Name.ToUpper().Trim(),
        //    DOB = comp.DOB,
        //    Surname = comp.Surname.ToUpper().Trim()
        //  }, match => new
        //  {
        //    Name = match.Name.ToUpper().Trim(),
        //    DOB = match.DOB,
        //    Surname = match.Surname.ToUpper().Trim()
        //  }, (comp, match) => new
        //  {
        //    composit = comp,
        //    Surname = match.Surname.ToUpper().Trim(),
        //    Name = match.Name.ToUpper().Trim(),
        //    DOB = match.DOB,
        //    SAID = match.SAID,
        //    NBT = match.NBT
        //  }).ToList().Distinct();
        //  datas2.Count();
        //  IEnumerable<\u003C\u003Ef__AnonymousType3<Composit, string, string, DateTime, string, long>> datas3 = outer.Join((IEnumerable<BI>) biDetails, comp => new
        //  {
        //    NBT = comp.RefNo,
        //    DOB = comp.DOB,
        //    Name = comp.Name.ToUpper().Trim()
        //  }, match => new
        //  {
        //    NBT = match.NBT,
        //    DOB = match.DOB,
        //    Name = match.Name.ToUpper().Trim()
        //  }, (comp, match) => new
        //  {
        //    composit = comp,
        //    Surname = match.Surname.ToUpper().Trim(),
        //    Name = match.Name.ToUpper().Trim(),
        //    DOB = match.DOB,
        //    SAID = match.SAID,
        //    NBT = match.NBT
        //  }).ToList().Distinct();
        //  datas3.Count();
        //  IEnumerable<\u003C\u003Ef__AnonymousType3<Composit, string, string, DateTime, string, long>> datas4 = outer.Join((IEnumerable<BI>) biDetails, comp => new
        //  {
        //    NBT = comp.RefNo,
        //    DOB = comp.DOB,
        //    Surname = comp.Surname.ToUpper().Trim()
        //  }, match => new
        //  {
        //    NBT = match.NBT,
        //    DOB = match.DOB,
        //    Surname = match.Surname.ToUpper().Trim()
        //  }, (comp, match) => new
        //  {
        //    composit = comp,
        //    Surname = match.Surname.ToUpper().Trim(),
        //    Name = match.Name.ToUpper().Trim(),
        //    DOB = match.DOB,
        //    SAID = match.SAID,
        //    NBT = match.NBT
        //  }).ToList().Distinct();
        //  datas4.Count();
        //  IEnumerable<\u003C\u003Ef__AnonymousType3<Composit, string, string, DateTime, string, long>> source1 = datas4.Union(datas3.Union(datas2.Union(datas1)));
        //  source1.Count();
        //  outer.Clear();
        //  foreach (var data in source1)
        //  {
        //    if (data.composit.Name != data.Name)
        //      data.composit.Name = data.Name;
        //    if (data.composit.Surname != data.Surname)
        //      data.composit.Surname = data.Surname;
        //  }
        //  if (source1.Count() <= 0)
        //    return;
        //  IEnumerable<\u003C\u003Ef__AnonymousType7<string, string, string, string, string, string, string, string, int, int?, string, int?, string, int?, string>> source2 = source1.Select(b => b.composit).Select(C => new
        //  {
        //    NBT = C.RefNo.ToString(),
        //    Surname = C.Surname,
        //    FName = C.Name,
        //    Initials = C.Initials,
        //    SaID = HelperUtils.ToSAID(C.SAID),
        //    Passport = C.ForeignID,
        //    DOB = string.Format("{0:dd/MM/yyyy}", (object) C.DOB),
        //    DOT = string.Format("{0:yyyyMMdd}", (object) C.DOT),
        //    Venue = C.VenueCode,
        //    ALScore = C.ALScore,
        //    ALLevel = C.ALLevel == "Proficient Upper" ? "Proficient" : (C.ALLevel == "Proficient Lower" ? "Proficient" : (C.ALLevel == "Intermediate Upper" ? "Intermediate" : (C.ALLevel == "Intermediate Lower" ? "Intermediate" : (C.ALLevel == "Basic Upper" ? "Basic" : (C.ALLevel == "Basic Lower" ? "Basic" : ""))))),
        //    QLScore = C.QLScore,
        //    QLLevel = C.QLLevel == "Proficient Upper" ? "Proficient" : (C.QLLevel == "Proficient Lower" ? "Proficient" : (C.QLLevel == "Intermediate Upper" ? "Intermediate" : (C.QLLevel == "Intermediate Lower" ? "Intermediate" : (C.QLLevel == "Basic Upper" ? "Basic" : (C.QLLevel == "Basic Lower" ? "Basic" : ""))))),
        //    MATScore = C.MATScore,
        //    MATLevel = C.MATLevel == "Proficient Upper" ? "Proficient" : (C.MATLevel == "Proficient Lower" ? "Proficient" : (C.MATLevel == "Intermediate Upper" ? "Intermediate" : (C.MATLevel == "Intermediate Lower" ? "Intermediate" : (C.MATLevel == "Basic Upper" ? "Basic" : (C.MATLevel == "Basic Lower" ? "Basic" : "")))))
        //  });
        //  XLWorkbook xlWorkbook = new XLWorkbook();
        //  IXLWorksheet xlWorksheet = xlWorkbook.Worksheets.Add("UCT Upload").SetTabColor(XLColor.DarkCoral);
        //  xlWorksheet.Cell(1, 1).Value = (object) "NBT_Ref_No";
        //  xlWorksheet.Cell(1, 2).Value = (object) "Surname";
        //  xlWorksheet.Cell(1, 3).Value = (object) "First_Name";
        //  xlWorksheet.Cell(1, 4).Value = (object) "Initials";
        //  xlWorksheet.Cell(1, 5).Value = (object) "South_African_ID_No";
        //  xlWorksheet.Cell(1, 6).Value = (object) "Foreign_Passport";
        //  xlWorksheet.Cell(1, 7).Value = (object) "Date_of_Birth";
        //  xlWorksheet.Cell(1, 8).Value = (object) "Date_of_Test";
        //  xlWorksheet.Cell(1, 9).Value = (object) "Venue";
        //  xlWorksheet.Cell(1, 10).Value = (object) "AL Score";
        //  xlWorksheet.Cell(1, 11).Value = (object) "AL Performance Level";
        //  xlWorksheet.Cell(1, 12).Value = (object) "QL Score";
        //  xlWorksheet.Cell(1, 13).Value = (object) "QL Performance Level";
        //  xlWorksheet.Cell(1, 14).Value = (object) "Maths Score";
        //  xlWorksheet.Cell(1, 15).Value = (object) "Maths Performance Level";
        //  xlWorksheet.Cell(2, 1).Value = (object) source2.AsEnumerable();
        //  DateTime dateTime = new DateTime();
        //  DateTime now = DateTime.Now;
        //  string str2 = now.Year.ToString() + now.Month.ToString("00") + now.Day.ToString("00") + "_" + now.Hour.ToString("00") + now.Minute.ToString("00");
        //  string path2 = str1 + "_" + str2 + ".xlsx";
        //  string file = Path.Combine(directoryName, path2);
        //  xlWorkbook.SaveAs(file);
        //}

        //private bool GenerateExcelComposite()
        //{
        //  bool flag = false;
        //  string str1 = "NBT_Composite";
        //  if (Composite.Count > 0)
        //  {
        //    IEnumerable<\u003C\u003Ef__AnonymousType0<string, string, string, string, string, string, string, string, string, int?, string, string, string, string, string, string, int?, string, string, int?, string, int?, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int?, string, int?, string, int?, string, string, string>> source1 = Composite.Select(mydata => new
        //    {
        //      RefNo = mydata.RefNo.ToString(),
        //      Barcode = mydata.Barcode.ToString(),
        //      LastName = mydata.Surname,
        //      FName = mydata.Name,
        //      Initials = mydata.Initials,
        //      SAID = HelperUtils.ToSAID(mydata.SAID),
        //      FID = mydata.ForeignID,
        //      DOB = string.Format("{0:yyyyMMdd}", (object) mydata.DOB),
        //      IDType = mydata.ID_Type,
        //      Citizenship = mydata.Citizenship,
        //      Classification = mydata.Classification,
        //      Gender = mydata.Gender,
        //      faculty1 = HelperUtils.GetFacultyName(mydata.Faculty),
        //      Testdate = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        //      VenueCode = mydata.VenueCode.ToString("D5"),
        //      VenueName = mydata.VenueName,
        //      Hlanguage = mydata.HomeLanguage,
        //      G12L = mydata.GR12Language,
        //      AQLLang = mydata.AQLLanguage,
        //      AQLCode = mydata.AQLCode,
        //      MatLang = mydata.MatLanguage,
        //      MatCode = mydata.MatCode,
        //      Faculty2 = HelperUtils.GetFacultyName(mydata.Faculty2),
        //      Faculty3 = HelperUtils.GetFacultyName(mydata.Faculty3),
        //      SessionID = mydata.Barcode.ToString(),
        //      NBTNumber = mydata.RefNo.ToString(),
        //      Surname = mydata.Surname,
        //      Name = mydata.Name,
        //      Initial = mydata.Initials,
        //      SouthAfricanID = HelperUtils.ToSAID(mydata.SAID),
        //      Passport = mydata.ForeignID,
        //      Birth = string.Format("{0:dd/MM/yyyy}", (object) mydata.DOB),
        //      W_AL = mydata.WroteAL,
        //      W_QL = mydata.WroteQL,
        //      W_Mat = mydata.WroteMat,
        //      StNo = "",
        //      Faculty = "",
        //      Programme = "",
        //      DateTest = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        //      Venue = mydata.VenueName,
        //      Sex = mydata.Gender == "1" ? "M" : (mydata.Gender == "2" ? "F" : mydata.Gender),
        //      street1 = "",
        //      street2 = "",
        //      Suburb = "",
        //      City = "",
        //      Province = "",
        //      Postal = "",
        //      Email = "",
        //      Landline = "",
        //      Mobile = "",
        //      ALScore = mydata.ALScore,
        //      ALLevel = mydata.ALLevel,
        //      QLScore = mydata.QLScore,
        //      QLLevel = mydata.QLLevel,
        //      MatScore = mydata.MATScore,
        //      MatLevel = mydata.MATLevel,
        //      AQL_Lang = mydata.AQLLanguage,
        //      Mat_Lang = mydata.MatLanguage
        //    });
        //    IEnumerable<\u003C\u003Ef__AnonymousType7<string, string, string, string, string, string, string, string, string, int?, string, int?, string, int?, string>> source2 = source1.Select(mydata => new
        //    {
        //      NBT = mydata.NBTNumber,
        //      Surname = mydata.Surname,
        //      FName = mydata.FName,
        //      Initials = mydata.Initials,
        //      SaID = mydata.SAID,
        //      Passport = mydata.Passport,
        //      DOB = mydata.Birth,
        //      DOT = mydata.DateTest,
        //      Venue = mydata.VenueCode,
        //      ALScore = mydata.ALScore,
        //      ALLevel = mydata.ALLevel,
        //      QLScore = mydata.QLScore,
        //      QLLevel = mydata.QLLevel,
        //      MATScore = mydata.MatScore,
        //      MATLevel = mydata.MatLevel
        //    });
        //    IEnumerable<\u003C\u003Ef__AnonymousType7<string, string, string, string, string, string, string, string, string, int?, string, int?, string, int?, string>> source3 = source1.Select(C => new
        //    {
        //      NBT = C.NBTNumber.ToString(),
        //      Surname = C.Surname,
        //      FName = C.FName,
        //      Initials = C.Initials,
        //      SaID = C.SAID,
        //      Passport = C.Passport,
        //      DOB = C.Birth,
        //      DOT = C.DateTest,
        //      Venue = C.VenueCode,
        //      ALScore = C.ALScore,
        //      ALLevel = C.ALLevel == "Proficient Upper" ? "Proficient" : (C.ALLevel == "Proficient Lower" ? "Proficient" : (C.ALLevel == "Intermediate Upper" ? "Intermediate" : (C.ALLevel == "Intermediate Lower" ? "Intermediate" : (C.ALLevel == "Basic Upper" ? "Basic" : (C.ALLevel == "Basic Lower" ? "Basic" : ""))))),
        //      QLScore = C.QLScore,
        //      QLLevel = C.QLLevel == "Proficient Upper" ? "Proficient" : (C.QLLevel == "Proficient Lower" ? "Proficient" : (C.QLLevel == "Intermediate Upper" ? "Intermediate" : (C.QLLevel == "Intermediate Lower" ? "Intermediate" : (C.QLLevel == "Basic Upper" ? "Basic" : (C.QLLevel == "Basic Lower" ? "Basic" : ""))))),
        //      MATScore = C.MatScore,
        //      MATLevel = C.MatLevel == "Proficient Upper" ? "Proficient" : (C.MatLevel == "Proficient Lower" ? "Proficient" : (C.MatLevel == "Intermediate Upper" ? "Intermediate" : (C.MatLevel == "Intermediate Lower" ? "Intermediate" : (C.MatLevel == "Basic Upper" ? "Basic" : (C.MatLevel == "Basic Lower" ? "Basic" : "")))))
        //    });
        //    IEnumerable<\u003C\u003Ef__AnonymousType8<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int?, string, int?, string, int?, string, string, string>> source4 = Composite.Select(mydata => new
        //    {
        //      Barcode = mydata.Barcode.ToString(),
        //      SessionID = mydata.Barcode.ToString(),
        //      NBTNumber = mydata.RefNo.ToString(),
        //      Surname = mydata.Surname,
        //      Name = mydata.Name,
        //      Initial = mydata.Initials,
        //      SouthAfricanID = mydata.SAID.ToString(),
        //      Passport = mydata.ForeignID,
        //      Birth = string.Format("{0:dd/MM/yyyy}", (object) mydata.DOB),
        //      W_AL = mydata.WroteAL,
        //      W_QL = mydata.WroteQL,
        //      W_Mat = mydata.WroteMat,
        //      StNo = "",
        //      Faculty = "",
        //      Programme = "",
        //      DateTest = string.Format("{0:yyyyMMdd}", (object) mydata.DOT),
        //      Venue = GetWebSiteNameByVenueCode(mydata.VenueCode),
        //      Sex = mydata.Gender,
        //      street1 = "",
        //      street2 = "",
        //      Suburb = "",
        //      City = "",
        //      Province = "",
        //      Postal = "",
        //      Email = "",
        //      Landline = "",
        //      Mobile = "",
        //      ALScore = !mydata.ALScore.HasValue ? new int?(0) : mydata.ALScore,
        //      ALLevel = mydata.ALLevel,
        //      QLScore = !mydata.QLScore.HasValue ? new int?(0) : mydata.QLScore,
        //      QLLevel = mydata.QLLevel,
        //      MatScore = !mydata.MATScore.HasValue ? new int?(0) : mydata.MATScore,
        //      MatLevel = mydata.MATLevel,
        //      AQL_Lang = mydata.AQLLanguage,
        //      Mat_Lang = mydata.MatLanguage
        //    });
        //    XLWorkbook xlWorkbook = new XLWorkbook();
        //    IXLWorksheet xlWorksheet1 = xlWorkbook.Worksheets.Add("Composite").SetTabColor(XLColor.Almond);
        //    xlWorksheet1.Columns("A1:BF1").Width = 13.0;
        //    xlWorksheet1.Range("A1:X1").Style.Fill.BackgroundColor = XLColor.TealBlue;
        //    xlWorksheet1.Range("Y1:Y1").Style.Fill.BackgroundColor = XLColor.Orange;
        //    xlWorksheet1.Range("Z1:AI1").Style.Fill.BackgroundColor = XLColor.Yellow;
        //    xlWorksheet1.Range("AJ1:AL1").Style.Fill.BackgroundColor = XLColor.Magenta;
        //    xlWorksheet1.Range("AM1:AX1").Style.Fill.BackgroundColor = XLColor.GreenYellow;
        //    xlWorksheet1.Range("AY1:BD1").Style.Fill.BackgroundColor = XLColor.LightGray;
        //    xlWorksheet1.Range("BE1:BF1").Style.Fill.BackgroundColor = XLColor.Yellow;
        //    IXLRow xlRow = xlWorksheet1.Row(1);
        //    xlRow.Height = 30.0;
        //    xlRow.Style.Font.Bold = true;
        //    xlRow.Style.Font.FontSize = 9.0;
        //    xlRow.Style.Alignment.WrapText = true;
        //    xlRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //    xlRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //    xlWorksheet1.Cell(1, 1).Value = (object) "Ref No";
        //    xlWorksheet1.Cell(1, 2).Value = (object) "Barcode";
        //    xlWorksheet1.Cell(1, 3).Value = (object) "Last Name";
        //    xlWorksheet1.Cell(1, 4).Value = (object) "First_Name";
        //    xlWorksheet1.Cell(1, 5).Value = (object) "INITIALS";
        //    xlWorksheet1.Cell(1, 6).Value = (object) "ID NUMBER";
        //    xlWorksheet1.Cell(1, 7).Value = (object) "ID_Foreign";
        //    xlWorksheet1.Cell(1, 8).Value = (object) "Date of Birth";
        //    xlWorksheet1.Cell(1, 9).Value = (object) "ID Type";
        //    xlWorksheet1.Cell(1, 10).Value = (object) "Citizenship";
        //    xlWorksheet1.Cell(1, 11).Value = (object) "Classification";
        //    xlWorksheet1.Cell(1, 12).Value = (object) "Gender 1";
        //    xlWorksheet1.Cell(1, 13).Value = (object) "Faculty 1";
        //    xlWorksheet1.Cell(1, 14).Value = (object) "DATE";
        //    xlWorksheet1.Cell(1, 15).Value = (object) "Test Centre Code";
        //    xlWorksheet1.Cell(1, 16).Value = (object) "Venue Name";
        //    xlWorksheet1.Cell(1, 17).Value = (object) "Home Lang";
        //    xlWorksheet1.Cell(1, 18).Value = (object) "GR12 Language";
        //    xlWorksheet1.Cell(1, 19).Value = (object) "AQL LANG";
        //    xlWorksheet1.Cell(1, 20).Value = (object) "AQL CODE";
        //    xlWorksheet1.Cell(1, 21).Value = (object) "MAT LANG";
        //    xlWorksheet1.Cell(1, 22).Value = (object) "MAT CODE";
        //    xlWorksheet1.Cell(1, 23).Value = (object) "Faculty 2";
        //    xlWorksheet1.Cell(1, 24).Value = (object) "Faculty 3";
        //    xlWorksheet1.Cell(1, 25).Value = (object) "Test Session ID";
        //    xlWorksheet1.Cell(1, 26).Value = (object) "NBT Reference";
        //    xlWorksheet1.Cell(1, 27).Value = (object) "Surname";
        //    xlWorksheet1.Cell(1, 28).Value = (object) "First Name";
        //    xlWorksheet1.Cell(1, 29).Value = (object) "Middle Initials";
        //    xlWorksheet1.Cell(1, 30).Value = (object) "South African ID";
        //    xlWorksheet1.Cell(1, 31).Value = (object) "Foreign ID";
        //    xlWorksheet1.Cell(1, 32).Value = (object) "Date of Birth";
        //    xlWorksheet1.Cell(1, 33).Value = (object) "Wrote AL";
        //    xlWorksheet1.Cell(1, 34).Value = (object) "Wrote QL";
        //    xlWorksheet1.Cell(1, 35).Value = (object) "Wrote Maths";
        //    xlWorksheet1.Cell(1, 36).Value = (object) "Student Number";
        //    xlWorksheet1.Cell(1, 37).Value = (object) "Faculty";
        //    xlWorksheet1.Cell(1, 38).Value = (object) "Programme";
        //    xlWorksheet1.Cell(1, 39).Value = (object) "Date_of_Test";
        //    xlWorksheet1.Cell(1, 40).Value = (object) "Venue";
        //    xlWorksheet1.Cell(1, 41).Value = (object) "Gender";
        //    xlWorksheet1.Cell(1, 42).Value = (object) "Street and Number";
        //    xlWorksheet1.Cell(1, 43).Value = (object) "Street Name";
        //    xlWorksheet1.Cell(1, 44).Value = (object) "Suburb";
        //    xlWorksheet1.Cell(1, 45).Value = (object) "City/Town";
        //    xlWorksheet1.Cell(1, 46).Value = (object) "Province/Region";
        //    xlWorksheet1.Cell(1, 47).Value = (object) "Postal Code";
        //    xlWorksheet1.Cell(1, 48).Value = (object) "e-mail Address";
        //    xlWorksheet1.Cell(1, 49).Value = (object) "Landline Number";
        //    xlWorksheet1.Cell(1, 50).Value = (object) "Mobile Number";
        //    xlWorksheet1.Cell(1, 51).Value = (object) "AL Score";
        //    xlWorksheet1.Cell(1, 52).Value = (object) "AL Performance";
        //    xlWorksheet1.Cell(1, 53).Value = (object) "QL Score";
        //    xlWorksheet1.Cell(1, 54).Value = (object) "QL Performance";
        //    xlWorksheet1.Cell(1, 55).Value = (object) "Maths Score";
        //    xlWorksheet1.Cell(1, 56).Value = (object) "Maths Performance";
        //    xlWorksheet1.Cell(1, 57).Value = (object) "AQL TEST LANGUAGE";
        //    xlWorksheet1.Cell(1, 58).Value = (object) "MATHS TEST LANGUAGE";
        //    xlWorksheet1.Cell(2, 1).Value = (object) source1.AsEnumerable();
        //    IXLWorksheet xlWorksheet2 = xlWorkbook.Worksheets.Add("UP Upload").SetTabColor(XLColor.BrightTurquoise);
        //    xlWorksheet2.Cell(1, 1).Value = (object) "NBT_Ref_No";
        //    xlWorksheet2.Cell(1, 2).Value = (object) "Surname";
        //    xlWorksheet2.Cell(1, 3).Value = (object) "First_Name";
        //    xlWorksheet2.Cell(1, 4).Value = (object) "Initials";
        //    xlWorksheet2.Cell(1, 5).Value = (object) "South_African_ID_No";
        //    xlWorksheet2.Cell(1, 6).Value = (object) "Foreign_Passport";
        //    xlWorksheet2.Cell(1, 7).Value = (object) "Date_of_Birth";
        //    xlWorksheet2.Cell(1, 8).Value = (object) "Date_of_Test";
        //    xlWorksheet2.Cell(1, 9).Value = (object) "Venue";
        //    xlWorksheet2.Cell(1, 10).Value = (object) "AL Score";
        //    xlWorksheet2.Cell(1, 11).Value = (object) "AL Performance Level";
        //    xlWorksheet2.Cell(1, 12).Value = (object) "QL Score";
        //    xlWorksheet2.Cell(1, 13).Value = (object) "QL Performance Level";
        //    xlWorksheet2.Cell(1, 14).Value = (object) "Maths Score";
        //    xlWorksheet2.Cell(1, 15).Value = (object) "Maths Performance Level";
        //    xlWorksheet2.Cell(2, 1).Value = (object) source2.AsEnumerable();
        //    IXLWorksheet xlWorksheet3 = xlWorkbook.Worksheets.Add("UCT Upload").SetTabColor(XLColor.DarkCoral);
        //    xlWorksheet3.Cell(1, 1).Value = (object) "NBT_Ref_No";
        //    xlWorksheet3.Cell(1, 2).Value = (object) "Surname";
        //    xlWorksheet3.Cell(1, 3).Value = (object) "First_Name";
        //    xlWorksheet3.Cell(1, 4).Value = (object) "Initials";
        //    xlWorksheet3.Cell(1, 5).Value = (object) "South_African_ID_No";
        //    xlWorksheet3.Cell(1, 6).Value = (object) "Foreign_Passport";
        //    xlWorksheet3.Cell(1, 7).Value = (object) "Date_of_Birth";
        //    xlWorksheet3.Cell(1, 8).Value = (object) "Date_of_Test";
        //    xlWorksheet3.Cell(1, 9).Value = (object) "Venue";
        //    xlWorksheet3.Cell(1, 10).Value = (object) "AL Score";
        //    xlWorksheet3.Cell(1, 11).Value = (object) "AL Performance Level";
        //    xlWorksheet3.Cell(1, 12).Value = (object) "QL Score";
        //    xlWorksheet3.Cell(1, 13).Value = (object) "QL Performance Level";
        //    xlWorksheet3.Cell(1, 14).Value = (object) "Maths Score";
        //    xlWorksheet3.Cell(1, 15).Value = (object) "Maths Performance Level";
        //    xlWorksheet3.Cell(2, 1).Value = (object) source3.AsEnumerable();
        //    IXLWorksheet xlWorksheet4 = xlWorkbook.Worksheets.Add("NBT Website Upload").SetTabColor(XLColor.MintGreen);
        //    xlWorksheet4.Cell(1, 1).Value = (object) "Barcode";
        //    xlWorksheet4.Cell(1, 2).Value = (object) "Test Session ID";
        //    xlWorksheet4.Cell(1, 3).Value = (object) "NBT Reference";
        //    xlWorksheet4.Cell(1, 4).Value = (object) "Surname";
        //    xlWorksheet4.Cell(1, 5).Value = (object) "First Name";
        //    xlWorksheet4.Cell(1, 6).Value = (object) "Middle Initials";
        //    xlWorksheet4.Cell(1, 7).Value = (object) "South African ID";
        //    xlWorksheet4.Cell(1, 8).Value = (object) "Foreign ID";
        //    xlWorksheet4.Cell(1, 9).Value = (object) "Date of Birth";
        //    xlWorksheet4.Cell(1, 10).Value = (object) "Wrote AL";
        //    xlWorksheet4.Cell(1, 11).Value = (object) "Wrote QL";
        //    xlWorksheet4.Cell(1, 12).Value = (object) "Wrote Maths";
        //    xlWorksheet4.Cell(1, 13).Value = (object) "Student Number";
        //    xlWorksheet4.Cell(1, 14).Value = (object) "Faculty";
        //    xlWorksheet4.Cell(1, 15).Value = (object) "Programme";
        //    xlWorksheet4.Cell(1, 16).Value = (object) "Date_of_Test";
        //    xlWorksheet4.Cell(1, 17).Value = (object) "Venue";
        //    xlWorksheet4.Cell(1, 18).Value = (object) "Gender";
        //    xlWorksheet4.Cell(1, 19).Value = (object) "Street and Number";
        //    xlWorksheet4.Cell(1, 20).Value = (object) "Street Name";
        //    xlWorksheet4.Cell(1, 21).Value = (object) "Suburb";
        //    xlWorksheet4.Cell(1, 22).Value = (object) "City/Town";
        //    xlWorksheet4.Cell(1, 23).Value = (object) "Province/Region";
        //    xlWorksheet4.Cell(1, 24).Value = (object) "Postal Code";
        //    xlWorksheet4.Cell(1, 25).Value = (object) "e-mail Address";
        //    xlWorksheet4.Cell(1, 26).Value = (object) "Landline Number";
        //    xlWorksheet4.Cell(1, 27).Value = (object) "Mobile Number";
        //    xlWorksheet4.Cell(1, 28).Value = (object) "AL Score";
        //    xlWorksheet4.Cell(1, 29).Value = (object) "AL Performance";
        //    xlWorksheet4.Cell(1, 30).Value = (object) "QL Score";
        //    xlWorksheet4.Cell(1, 31).Value = (object) "QL Performance";
        //    xlWorksheet4.Cell(1, 32).Value = (object) "Maths Score";
        //    xlWorksheet4.Cell(1, 33).Value = (object) "Maths Performance";
        //    xlWorksheet4.Cell(1, 34).Value = (object) "AQL TEST LANGUAGE";
        //    xlWorksheet4.Cell(1, 35).Value = (object) "MATHS TEST LANGUAGE";
        //    xlWorksheet4.Cell(2, 1).Value = (object) source4.AsEnumerable();
        //    xlWorksheet4.Columns().AdjustToContents();
        //    string dir = myDir.Dir;
        //    DateTime dateTime = new DateTime();
        //    DateTime now = DateTime.Now;
        //    string str2 = now.Year.ToString() + now.Month.ToString("00") + now.Day.ToString("00") + "_" + now.Hour.ToString("00") + now.Minute.ToString("00");
        //    string path2 = str1 + "_" + str2 + ".xlsx";
        //    string file = Path.Combine(dir, path2);
        //    xlWorkbook.SaveAs(file);
        //    flag = true;
        //  }
        //  return flag;
        //}

        //private static void CompositBDOToCompositDAL(CompositBDO compositBDO, Composit composit)
        //{
        //  composit.ALScore = compositBDO.ALScore;
        //  composit.AQLCode = compositBDO.AQLCode;
        //  composit.AQLLanguage = compositBDO.AQLLanguage;
        //  composit.Barcode = compositBDO.Barcode;
        //  composit.Citizenship = compositBDO.Citizenship;
        //  composit.Classification = compositBDO.Classification;
        //  composit.DOB = compositBDO.DOB;
        //  composit.DOT = compositBDO.DOT;
        //  composit.ALLevel = compositBDO.ALLevel;
        //  composit.Faculty = compositBDO.Faculty;
        //  composit.Faculty2 = compositBDO.Faculty2;
        //  composit.Faculty3 = compositBDO.Faculty3;
        //  composit.ForeignID = compositBDO.ForeignID;
        //  composit.Gender = compositBDO.Gender;
        //  composit.GR12Language = compositBDO.GR12Language;
        //  composit.HomeLanguage = compositBDO.HomeLanguage.ToString();
        //  composit.ID_Type = compositBDO.ID_Type;
        //  composit.Initials = compositBDO.Initials;
        //  composit.MatCode = compositBDO.MatCode;
        //  composit.MatLanguage = compositBDO.MatLanguage;
        //  composit.MATLevel = compositBDO.MATLevel;
        //  composit.MATScore = compositBDO.MATScore;
        //  composit.Name = compositBDO.Name;
        //  composit.QLLevel = compositBDO.QLLevel;
        //  composit.QLScore = compositBDO.QLScore;
        //  composit.RefNo = compositBDO.RefNo;
        //  composit.RowGuid = compositBDO.RowGuid;
        //  composit.RowVersion = compositBDO.RowVersion;
        //  composit.SAID = compositBDO.SAID;
        //  composit.Surname = compositBDO.Surname;
        //  composit.VenueName = compositBDO.VenueName;
        //  composit.VenueCode = compositBDO.VenueCode;
        //  composit.WroteAL = compositBDO.WroteAL;
        //  composit.WroteQL = compositBDO.WroteQL;
        //  composit.WroteMat = compositBDO.WroteMat;
        //}

        //private static void CompositDALToCompositBDO(CompositBDO compositBDO, Composit composit)
        //{
        //  compositBDO.ALScore = composit.ALScore;
        //  compositBDO.AQLCode = composit.AQLCode;
        //  compositBDO.AQLLanguage = composit.AQLLanguage;
        //  compositBDO.Barcode = composit.Barcode;
        //  compositBDO.Citizenship = composit.Citizenship;
        //  compositBDO.Classification = composit.Classification;
        //  compositBDO.DOB = composit.DOB;
        //  compositBDO.DOT = composit.DOT;
        //  compositBDO.ALLevel = composit.ALLevel;
        //  compositBDO.Faculty = composit.Faculty;
        //  compositBDO.Faculty2 = composit.Faculty2;
        //  compositBDO.Faculty3 = composit.Faculty3;
        //  compositBDO.ForeignID = composit.ForeignID;
        //  compositBDO.Gender = composit.Gender;
        //  compositBDO.GR12Language = composit.GR12Language;
        //  compositBDO.HomeLanguage = new int?(Convert.ToInt32(composit.HomeLanguage));
        //  compositBDO.ID_Type = composit.ID_Type;
        //  compositBDO.Initials = composit.Initials;
        //  compositBDO.MatCode = composit.MatCode;
        //  compositBDO.MatLanguage = composit.MatLanguage;
        //  compositBDO.MATLevel = composit.MATLevel;
        //  compositBDO.MATScore = composit.MATScore;
        //  compositBDO.Name = composit.Name;
        //  compositBDO.QLLevel = composit.QLLevel;
        //  compositBDO.QLScore = composit.QLScore;
        //  compositBDO.RefNo = composit.RefNo;
        //  compositBDO.RowGuid = composit.RowGuid;
        //  compositBDO.RowVersion = composit.RowVersion;
        //  compositBDO.SAID = composit.SAID;
        //  compositBDO.Surname = composit.Surname;
        //  compositBDO.VenueName = composit.VenueName;
        //  compositBDO.VenueCode = composit.VenueCode;
        //  compositBDO.WroteAL = composit.WroteAL;
        //  compositBDO.WroteQL = composit.WroteQL;
        //  compositBDO.WroteMat = composit.WroteMat;
        //  compositBDO.DateModified = composit.DateModified;
        //  compositBDO.Batch = compositBDO.Batch;
        //}

        //private static void ProvinceDALToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        //{
        //  provinceBDO.Code = province.Code;
        //  provinceBDO.Id = province.Id;
        //  provinceBDO.Name = province.Name;
        //  provinceBDO.RowVersion = province.RowVersion;
        //}

        //private static void provinceBDOtoprovinceDAL(ProvinceBDO provinceBDO, Province province)
        //{
        //  province.Code = provinceBDO.Code;
        //  province.Id = provinceBDO.Id;
        //  province.Name = provinceBDO.Name;
        //  province.RowVersion = provinceBDO.RowVersion;
        //}

        //private static void ProvinceToProvinceBDO(ProvinceBDO provinceBDO, Province province)
        //{
        //  provinceBDO.Code = province.Code;
        //  provinceBDO.Id = province.Id;
        //  provinceBDO.Name = province.Name;
        //  provinceBDO.RowVersion = province.RowVersion;
        //}

        //public ProvinceBDO getProvinceByID(int code)
        //{
        //  ProvinceBDO provinceBDO = (ProvinceBDO) null;
        //  int ProvID = code;
        //  using (CETAPEntities cetapEntities = new CETAPEntities())
        //  {
        //    Province province = cetapEntities.Provinces.Where<Province>((System.Linq.Expressions.Expression<Func<Province, bool>>) (x => x.Code == ProvID)).FirstOrDefault<Province>();
        //    if (province != null)
        //    {
        //      provinceBDO = new ProvinceBDO();
        //      CompositeService.ProvinceToProvinceBDO(provinceBDO, province);
        //    }
        //  }
        //  return provinceBDO;
        //}

        //public string GetWebSiteNameByVenueCode(int venueCode)
        //{
        //  string str = "";
        //  using (CETAPEntities cetapEntities = new CETAPEntities())
        //  {
        //    TestVenue testVenue = cetapEntities.TestVenues.Where<TestVenue>((System.Linq.Expressions.Expression<Func<TestVenue, bool>>) (x => x.VenueCode == venueCode)).FirstOrDefault<TestVenue>();
        //    if (testVenue != null)
        //      str = testVenue.WebsiteName;
        //  }
        //  return str;
        //}

        //public bool GenerateCompositFromDB(string path)
        //{
        //  throw new NotImplementedException();
        //}

        //public Task<bool> LogisticCompositeFromDB(string folder)
        //{
        //  throw new NotImplementedException();
        //}
    }
}
