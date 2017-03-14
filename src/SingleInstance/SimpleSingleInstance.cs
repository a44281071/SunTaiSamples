using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingleInstance
{
  /// <summary>
  /// a single instance WPF application
  /// </summary>
  public class SimpleSingleInstance : IDisposable
  {
    /// <summary>
    /// simple singleton app instance helper.
    /// </summary>
    /// <param name="uniqueKey">the key of app instance.</param>
    /// <param name="activeHandleAction">handle event when other instance notify try excute.</param>
    public SimpleSingleInstance(string uniqueKey, Action activeHandleAction)
    {
      _UniqueKey = uniqueKey;
      _ActiveHandleAction = activeHandleAction;

      string uniqueEventKey = $"Event_{uniqueKey}";
      string uniqueMutexKey = $"Mutex_{uniqueKey}";
      _EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, uniqueEventKey);
      _Mutex = new Mutex(true, uniqueMutexKey, out _IsOwned);
      if (_IsOwned)
      {
        StartEventWaitHandleThread();
      }
      else
      {
        _EventWaitHandle.Set();
      }
    }

    readonly Action _ActiveHandleAction;
    readonly string _UniqueKey;

    private readonly EventWaitHandle _EventWaitHandle;
    private readonly Mutex _Mutex;
    private Thread _EventWaitHandleThread;

    bool _IsOwned;

    /// <summary>
    /// get is first time run app.
    /// </summary>
    public bool IsOwned { get { return _IsOwned; } }

    /// <summary>
    /// Spawn a thread which will be waiting for our event
    /// </summary>
    private void StartEventWaitHandleThread()
    {
      _EventWaitHandleThread = new Thread(() =>
      {
        while (_EventWaitHandle.WaitOne())
        {
          _ActiveHandleAction?.Invoke();
        }
      });

      // It is important mark it as background otherwise it will prevent app from exiting.
      _EventWaitHandleThread.IsBackground = true;
      _EventWaitHandleThread.Start();
    }

    public void Dispose()
    {
      _Mutex.Dispose();
      _EventWaitHandle.Dispose();
      if (null != _EventWaitHandleThread && _EventWaitHandleThread.IsAlive)
      {
        _EventWaitHandleThread.Abort();
      }
      _EventWaitHandleThread = null;
    }
  }
}
