// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.easypay.FakeEasyPayViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe
using GalaSoft.MvvmLight.CommandWpf;
using LINQtoCSV;
using CETAP_LOB.Database;
using CETAP_LOB.Helper;
using CETAP_LOB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using GalaSoft.MvvmLight;
using System.Data.SqlClient;
using System.IO;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;

namespace CETAP_LOB.ViewModel.easypay
{
  public class FakeEasyPayViewModel : ViewModelBase
  {
        private string _folder;
        private string _folder1;
        private string _filename;
        private string txt_filename;
        string connectionString = "Data Source=PRDSQLCLUSTER2\\PRD002;Initial Catalog=NBT_Production;Integrated Security=SSPI"; 
        //string connectionString = "Data Source=DEVSQLCLUSTER3\\DEV002;Initial Catalog=NBT_Production;Integrated Security=SSPI";
        //  private IDataService _service;
        public RelayCommand FolderBrowserCommand { get; private set; }
        public RelayCommand SaveFolderCommand { get; private set; }
        public RelayCommand SaveFileCommand { get; private set; }
        public string Folder
        {
            get
            {
                return _folder;
            }
            set
            {
                if (_folder == value)
                    return;
                _folder = value;
                RaisePropertyChanged("Folder");
            }
        }
        public string Folder1
        {
            get
            {
                return _folder1;
            }
            set
            {
                if (_folder1 == value)
                    return;
                _folder1 = value;
                RaisePropertyChanged("Folder1");
            }
        }
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                if (_filename == value)
                    return;
                _filename = value;
                RaisePropertyChanged("FileName");
            }
        }
        public string txt_FileName
        {
            get
            {
                return txt_filename;
            }
            set
            {
                if (txt_filename == value)
                    return;
                txt_filename = value;
                RaisePropertyChanged("txt_FileName");
            }
        }
        public FakeEasyPayViewModel()
        {
         //   _service = Service;
          //  InitializeModels();
            RegisterCommands();
         //   FtpDirectoryList();
        }
        private void RegisterCommands()
        {
           // ListCommand = new RelayCommand(() => GetFiles());
            FolderBrowserCommand = new RelayCommand(OpenNewFolder);
            SaveFolderCommand = new RelayCommand(OpenSaveFolder);
            SaveFileCommand = new RelayCommand(SaveFile);
            //WriteToDatabaseCommand = new RelayCommand(() => WriteToDB());
            //FromDBCommand = new RelayCommand(() => RecordsFromDB());
            //SavetoCSVCommand = new RelayCommand(() => SaveCSVFile());
        }

        private void SaveFile()
        {
            if(_filename is null || _filename == "")
            {
                int num = (int)ModernDialog.ShowMessage("Enter file name to create \n Name of file should start with date", "Existing File", MessageBoxButton.OK, (Window)null);
                return;
            }
            int count = 0;
            double TotalAmtPaid = 0;
            DateTime current = DateTime.Parse(_filename.Substring(0, 4) + "-" + _filename.Substring(4, 2) + "-" + _filename.Substring(6, 2));
            StreamWriter file = new StreamWriter(_folder1 + "\\" + _filename + "_easypay.3100.999");
            file.Write("SOF,3100," + _filename + ",101110,99999\r\n");
            string[] array = File.ReadAllLines(_folder);
            foreach (string line in array)
            {
                if (!(line.Substring(0, 1) == "r"))
                {
                    string[] array2 = line.Split(';', ',');
                    long Ref_No = long.Parse(array2[0]);
                    double Amt_Paid = double.Parse(array2[2]);
                    SQLInsert(Ref_No, Amt_Paid, current);
                    count = ++count;
                    TotalAmtPaid += Amt_Paid;
                    file.Write("X,2630099999," + _filename + ",101110,0263,\r\n");
                    file.Write("P,  " + Amt_Paid + ",   0.00," + Ref_No + "\r\n");
                    file.Write("T,  " + Amt_Paid + ",   0.00,Cash\r\n");
                }
            }
            file.Write(count + "," + TotalAmtPaid + ",0.00," + TotalAmtPaid + ", 0.00");
            file.Close();
            ModernDialog.ShowMessage("You have successfully imported '" + count + "' Records to the Database.\n Please find the CSV file in this location: '" + Folder1 + "'. ", "CSV Generated!", MessageBoxButton.OK, (Window)null);

        }
        private void OpenSaveFolder()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = "C:\\";
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            string selectedPath = folderBrowserDialog1.SelectedPath;
            // EasyPayViewModel._mruManager.Add(selectedPath);
            // mruList.Add(selectedPath);
            Folder1 = selectedPath;
        }
        private void OpenNewFolder()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "C:\\";

                ofd.Filter = "csv files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*,*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Title = "Open bank easypay file";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Folder = ofd.FileName;

                }
            }
        }
        private void SQLInsert(long a, double b, DateTime c)
        {
            string cmdText = "INSERT INTO [dbo].['Bank Payments$']([Ref No#], [Amt pd], [Upload Date])VALUES( '" + a + "', '" + b + "', '" + c.ToString() + "')";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            new SqlCommand(cmdText, con).ExecuteNonQuery();
            con.Close();
        }
    }
}
