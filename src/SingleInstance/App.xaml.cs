using System;
using System.Threading;
using System.Windows;

namespace SingleInstance
{
  public partial class App : Application
  {
    private const string UnqueKey = "{124D19DC-50AC-4134-84A7-8432E2CA6E8E}";
    static SimpleSingleInstance _SimpleSingleInstance;

    public App()
    {
      EnsureSingleInstance();
      InitializeComponent();
    }

    private void EnsureSingleInstance()
    {
      _SimpleSingleInstance 
        = new SimpleSingleInstance(UnqueKey, OnEventWaitHandleWaitOne);
      if (!_SimpleSingleInstance.IsOwned)
      {
        Shutdown();
      }
    }

    private void OnEventWaitHandleWaitOne()
    {
      Dispatcher.BeginInvoke((Action)(() =>
      {
        MainWindow.BringToForeground();
      }));
    }
    
    internal static void Restart()
    {
      _SimpleSingleInstance?.Dispose();
      System.Diagnostics.Process.Start(ResourceAssembly.Location);
      Current.Shutdown();
    }

  }
}