using System;
using System.Threading;
using System.Windows;

namespace SingleInstance
{
  public partial class App : Application
  {
    private const string UNQUE_KEY = "{124D19DC-50AC-4134-84A7-8432E2CA6E8E}";
    static SimpleSingleInstance _SimpleSingleInstance;

    public App()
    {
      EnsureSingleInstance();
      InitializeComponent();
    }

    private void EnsureSingleInstance()
    {
      _SimpleSingleInstance 
        = new SimpleSingleInstance(UNQUE_KEY, OnEventWaitHandleWaitOne);
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