﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfCore
{
  /// <summary>
  /// Simple relay command for internal use only.
  /// </summary>
  public class RelayCommand : ICommand
  {
    #region Fields

    readonly Action _execute;
    readonly Func<bool> _canExecute;

    #endregion // Fields

    #region Constructors

    public RelayCommand(Action execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");

      _execute = execute;
      _canExecute = canExecute;
    }
    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute();
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      _execute();
    }

    #endregion // ICommand Members
  }

  /// <summary>
  /// Simple relay command for internal use only.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  class RelayCommand<T> : ICommand where T : class
  {
    #region Fields

    readonly Action<T> _execute;
    readonly Predicate<T> _canExecute;

    #endregion // Fields

    #region Constructors

    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");

      _execute = execute;
      _canExecute = canExecute;
    }
    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute(parameter as T);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      _execute(parameter as T);
    }

    #endregion // ICommand Members
  }
}
