// Decompiled with JetBrains decompiler
// Type: LOB.Helper.MRUManager`1
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace CETAP_LOB.Helper
{
  public class MRUManager<T>
  {
    private string _listName;
    private int _limit;
    private ObservableCollection<T> _list;

    public string ListName
    {
      get
      {
        return _listName;
      }
    }

    public int Limit
    {
      get
      {
        return _limit;
      }
    }

    public ReadOnlyCollection<T> List
    {
      get
      {
        return new ReadOnlyCollection<T>((IList<T>) _list);
      }
    }

    public event EventHandler ListChanged;

    public MRUManager(string listName, int limit)
    {
      if (string.IsNullOrEmpty(listName))
        throw new ArgumentOutOfRangeException("listName", "name cannot be null or empty");
      if (limit <= 0)
        throw new ArgumentOutOfRangeException("limit", "limit must be greater than zero.");
      _listName = listName;
      _limit = limit;
      LoadFromDisk();
    }

    public void Add(T item)
    {
      if (_list.Contains(item))
        _list.Remove(item);
      _list.Insert(0, item);
      RemoveExtraItems();
      SaveToDisk();
    }

    public void Clear()
    {
      _list.Clear();
    }

    private void SaveToDisk()
    {
      try
      {
        using (IsolatedStorageFile storeForAssembly = IsolatedStorageFile.GetUserStoreForAssembly())
        {
          string str = "mruList_" + ListName;
          IsolatedStorageFileStream storageFileStream = storeForAssembly.GetFileNames(str).Length <= 0 ? new IsolatedStorageFileStream(str, FileMode.Create, FileAccess.Write, storeForAssembly) : new IsolatedStorageFileStream(str, FileMode.Truncate, FileAccess.Write, storeForAssembly);
          using (storageFileStream)
            new XmlSerializer(typeof (ObservableCollection<T>)).Serialize((Stream) storageFileStream, (object) _list);
        }
      }
      catch (Exception ex)
      {
                throw ex;
      }
    }

    private void LoadFromDisk()
    {
      ObservableCollection<T> observableCollection = (ObservableCollection<T>) null;
      try
      {
        using (IsolatedStorageFile storeForAssembly = IsolatedStorageFile.GetUserStoreForAssembly())
        {
          string str = "mruList_" + ListName;
          if (storeForAssembly.GetFileNames(str).Length > 0)
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(str, FileMode.Open, FileAccess.Read, storeForAssembly))
              observableCollection = new XmlSerializer(typeof (ObservableCollection<T>)).Deserialize((Stream) storageFileStream) as ObservableCollection<T>;
          }
        }
      }
      catch (Exception ex)
      {
      }
      if (observableCollection == null)
        observableCollection = new ObservableCollection<T>();
      _list = observableCollection;
      _list.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChangedHandler);
      RemoveExtraItems();
    }

    private void RemoveExtraItems()
    {
      if (_list.Count <= Limit)
        return;
      for (int limit = Limit; limit < _list.Count; ++limit)
        _list.RemoveAt(limit);
    }

    private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
      SaveToDisk();
      if (ListChanged == null)
        return;
      ListChanged((object) this, EventArgs.Empty);
    }
  }
}
