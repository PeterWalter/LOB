// Decompiled with JetBrains decompiler
// Type: LOB.ViewModel.processing.EditingViewModel
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CETAP_LOB.BDO;
using CETAP_LOB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace CETAP_LOB.ViewModel.processing
{
  public class EditingViewModel : ViewModelBase
  {
    public const string DirListPropertyName = "DirList";
    public const string SelectedFilePropertyName = "SelectedFile";
    public const string FolderPropertyName = "Folder";
    public const string SelectedFilesPropertyName = "SelectedFiles";
    private IDataService _service;
    private ObservableCollection<ScannedFileBDO> _myfiles;
    private ScannedFileBDO _myfile;
    private string _folder;
    private IEnumerable<ScannedFileBDO> _selectedfiles;

    public RelayCommand SaveScannedFileCommand { get; private set; }

    public RelayCommand FolderBowserCommand { get; private set; }

    public ObservableCollection<ScannedFileBDO> DirList
    {
      get
      {
        return _myfiles;
      }
      set
      {
        if (_myfiles == value)
          return;
        _myfiles = value;
        RaisePropertyChanged("DirList");
      }
    }

    public ScannedFileBDO SelectedFile
    {
      get
      {
        return _myfile;
      }
      set
      {
        if (_myfile == value)
          return;
        _myfile = value;
        RaisePropertyChanged("SelectedFile");
      }
    }

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

    public IEnumerable<ScannedFileBDO> SelectedFiles
    {
      get
      {
        return DirList.Where<ScannedFileBDO>((Func<ScannedFileBDO, bool>) (x => x.IsSelected));
      }
      set
      {
        if (_selectedfiles == value)
          return;
        _selectedfiles = value;
        RaisePropertyChanged("SelectedFiles");
      }
    }

    public EditingViewModel(IDataService Service)
    {
      _service = Service;
      InitializeModels();
      RegisterCommands();
      Refresh();
    }

    private void InitializeModels()
    {
    }

    private void RegisterCommands()
    {
      SaveScannedFileCommand = new RelayCommand(new Action(SaveDatFile));
    }

    private void LoadData(string filename)
    {
    }

    private void Selectfolder()
    {
      List<ScannedFileBDO> list = new List<ScannedFileBDO>();
      foreach (FileInfo file in new DirectoryInfo(Folder).GetFiles("*.dat"))
        list.Add(new ScannedFileBDO()
        {
          Filepath = file.FullName,
          Filename = file.Name.ToUpper(),
          DateScanned = file.CreationTime
        });
      DirList = new ObservableCollection<ScannedFileBDO>(list);
    }

    private void Refresh()
    {
      Folder = ApplicationSettings.Default.ScanningFolder;
      Selectfolder();
    }

    private void SaveDatFile()
    {
      foreach (ScannedFileBDO selectedFile in SelectedFiles)
      {
        string message = "";
        if (!_service.SaveFileToDB(selectedFile, ref message))
        {
          int num = (int) ModernDialog.ShowMessage(message, "Existing File", MessageBoxButton.OK, (Window) null);
        }
        else
        {
          string destFileName = Path.Combine(ApplicationSettings.Default.EditingFolder, selectedFile.Filename);
          File.Move(selectedFile.Filepath, destFileName);
        }
      }
      Refresh();
    }
  }
}
