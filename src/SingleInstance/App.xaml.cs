using System;
using System.Threading;
using System.Windows;

namespace SingleInstance
{
  public partial class App : Application
  {
    private const string UnqueEventKey = "{124D19DC-50AC-4134-84A7-8432E2CA6E8E}";
    private const string UniqueMutexKey = "{FBFB8A02-B8BD-44E6-92CB-AF9680480600}";
    private static EventWaitHandle _EventWaitHandle;
    private static Mutex _Mutex;

    private static void SingleInstance()
    {
      bool isOwned;
      _Mutex = new Mutex(true, UniqueMutexKey, out isOwned);
      _EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UnqueEventKey);

      // So, R# would not give a warning that this variable is not used.
      GC.KeepAlive(_Mutex);

      if (isOwned)
      {
        // Spawn a thread which will be waiting for our event
        var thread = new Thread(() =>
        {
          while (_EventWaitHandle.WaitOne())
          {
            Current.Dispatcher.BeginInvoke((Action)(() =>
            {
              Current.MainWindow.BringToForeground();
            }));
          }
        });

        // It is important mark it as background otherwise it will prevent app from exiting.
        thread.IsBackground = true;

        thread.Start();
      }

      else
      {
        // Notify other instance so it could bring itself to foreground.
        _EventWaitHandle.Set();

        // Terminate this instance.
        Current.Shutdown();
      }
    }
    
    internal static void Restart()
    {
      _Mutex.Dispose();
      System.Diagnostics.Process.Start(ResourceAssembly.Location);
      Current.Shutdown();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      SingleInstance();
      base.OnStartup(e);
    }
  }
}