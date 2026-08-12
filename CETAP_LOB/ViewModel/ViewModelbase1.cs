

using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CETAP_LOB.ViewModel
{
  internal class ViewModelbase1 : ViewModelBase, INotifyDataErrorInfo
  {
    public Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

    public bool HasErrors
    {
      get
      {
        return _errors.Count > 0;
      }
    }

    public bool IsValid
    {
      get
      {
        return !HasErrors;
      }
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
        return (IEnumerable) null;
      return (IEnumerable) _errors[propertyName];
    }

    public void AddError(string propertyName, string error)
    {
      _errors[propertyName] = new List<string>()
      {
        error
      };
      NotifyErrorsChanged(propertyName);
    }

    public void RemoveError(string propertyName)
    {
      if (_errors.ContainsKey(propertyName))
        _errors.Remove(propertyName);
      NotifyErrorsChanged(propertyName);
    }

    private void NotifyErrorsChanged(string propertyName)
    {
      if (ErrorsChanged == null)
        return;
      ErrorsChanged((object) this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
}
