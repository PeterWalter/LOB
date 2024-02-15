//using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
    public class ReadEasyPayFile
    {
        private string _filename;

        private easyPayRec _record = new easyPayRec();

        private easyPayStartRec _startRecord = new easyPayStartRec();

        private easyPayEndRec _endRecord = new easyPayEndRec();
        private List<easyPayRec> _myRecords = new List<easyPayRec>();
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

        public easyPayStartRec SOF
        {
            get
            {
                return _startRecord;
            }
            set
            {
            }
        }

        public easyPayEndRec EOF
        {
            get
            {
                return _endRecord;
            }
            set
            {
            }
        }

        public easyPayRec AccountRecord
        {
            get
            {
                return _record;
            }
            set
            {
                _record = value;
            }
        }

        public List<easyPayRec> Records => _myRecords;
    
        public ReadEasyPayFile(string Fname)
        {
            _filename = Fname;
            easyPayRec easyPayRecord = new easyPayRec();
            StreamReader streamReader = new StreamReader(_filename);
            try
            {
                int num = 0;
                int num2 = 0;
                string text;
                while ((text = streamReader.ReadLine()) != null)
                {
                    string[] array = text.Split(',');
                    int num3 = 0;
                    if (array[0] == "X")
                    {
                        if (num > 0)
                        {
                            _myRecords.Add(easyPayRecord);
                        }

                        num++;
                        easyPayRecord = new easyPayRec();
                        num3 = 1;
                        num2 = 0;
                    }

                    switch (array[0])
                    {
                        case "SOF":
                            {
                                StringDate stringDate = new StringDate(array[3]);
                                _startRecord.FileDate = DateTime.Parse(stringDate.TrueDate);
                                string format = "HHmmss";
                                _startRecord.CreationTime = DateTime.ParseExact(array[4].ToString(), format, CultureInfo.InvariantCulture);
                                _startRecord.FileVersion = Convert.ToInt32(array[1]);
                                _startRecord.FileGenerationNumber = Convert.ToInt32(array[5]);
                                _startRecord.Identifier = array[0];
                                _startRecord.ReceiverIdentifier = Convert.ToInt64(array[2]);
                                break;
                            }
                        case "X":
                            {
                                string format2 = "HHmmss";
                                StringDate stringDate2 = new StringDate(array[2]);
                                easyPayRecord.Transaction.Identifier = array[0];
                                easyPayRecord.Transaction.CollectorID = Convert.ToInt64(array[1]);
                                easyPayRecord.Transaction.PayDay = DateTime.Parse(stringDate2.TrueDate);
                                easyPayRecord.Transaction.PayTime = DateTime.ParseExact(array[3].ToString(), format2, CultureInfo.InvariantCulture);
                                easyPayRecord.Transaction.PointOfService = array[4];
                                easyPayRecord.Transaction.TracePayee = array[5];
                                break;
                            }
                        case "P":
                            easyPayRecord.Payment = new Payment();
                            easyPayRecord.Payment.Identifier = array[0];
                            easyPayRecord.Payment.Amount = Convert.ToDecimal(array[1]);
                            easyPayRecord.Payment.Fee = Convert.ToDecimal(array[2]);
                            easyPayRecord.Payment.PayTag = Convert.ToInt64(array[3]);
                            break;
                        case "T":
                            easyPayRecord.Tender = new Tender();
                            if (num2 == 0)
                            {
                                easyPayRecord.Tenders = new List<Tender>();
                            }

                            num2++;
                            num3++;
                            easyPayRecord.Tender.Identifier = array[0];
                            easyPayRecord.Tender.Amount = Convert.ToDecimal(array[1]);
                            easyPayRecord.Tender.BankCost = Convert.ToDecimal(array[2]);
                            easyPayRecord.Tender.TenderType = array[3];
                          //  Debug.WriteLine(easyPayRecord.Tender.AccountNumber);
                            if (array.Length > 4)
                            {
                                easyPayRecord.Tender.AccountNumber = array[4];
                            }

                            easyPayRecord.Tenders.Add(easyPayRecord.Tender);
                            break;
                        default:
                            if (array.Length > 3)
                            {
                               // _myRecords.Add(easyPayRecord);
                                _endRecord.NumberOfPayments = Convert.ToInt64(array[0]);
                                _endRecord.TotalPayments = Convert.ToDecimal(array[1]);
                                _endRecord.Fees = Convert.ToDecimal(array[2]);
                                _endRecord.NumberOfTenders = Convert.ToInt64(array[3]);
                                _endRecord.TotalTenders = Convert.ToDecimal(array[4]);
                                _endRecord.BankCosts = Convert.ToDecimal(array[5]);
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                streamReader?.Close();
            }
        }
    }
}
