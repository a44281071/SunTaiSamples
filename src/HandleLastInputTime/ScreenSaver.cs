using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace HandleLastInputTime
{
  public class ScreenSaver
  {
    /// <summary>
    /// Invoke after 5 seconds
    /// </summary>
    public ScreenSaver() : this(5)
    {
    }

    /// <summary>
    /// Invoke after the specified seconds time
    /// </summary>
    /// <param name="notifySecondTime">seconds</param>
    public ScreenSaver(int notifySecondTime)
    {
      _NotifySecondTime = notifySecondTime * 1000;
      timer.Elapsed += Timer_Elapsed;
    }

    /// <summary>
    /// specified seconds time
    /// </summary>
    private readonly int _NotifySecondTime;

    /// <summary>
    /// Invoke every 100 milliseconds
    /// </summary>
    private readonly Timer timer = new Timer() { Interval = 100 };

    private LASTINPUTINFO lastInPut = new LASTINPUTINFO();
    private double duration = 0;

    public event EventHandler<EventArgs> NotifyNoOperation;

    /// <summary>
    /// Check whether it has timed out.
    /// </summary>
    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      duration += timer.Interval;
      long notTime = GetNoOperationMilliseconds();

      if (duration >= _NotifySecondTime
          && notTime > _NotifySecondTime)
      {
        duration = 0;
        OnNotifyNoOperation();
      }
    }

    protected void OnNotifyNoOperation()
    {
      var handler = NotifyNoOperation;
      handler?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Gets the milliseconds time of the last user input.
    /// </summary>
    public long GetNoOperationMilliseconds()
    {
      lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
      SSNativeMethod.GetLastInputInfo(ref lastInPut);
      return Environment.TickCount - lastInPut.dwTime;
    }

    public void Start()
    {
      duration = 0;
      timer.Start();
    }

    public void Stop()
    {
      duration = 0;
      timer.Stop();
    }
  }

  internal class SSNativeMethod
  {
    /// <summary>
    /// Gets the time of the last user input (in ms since the system started)
    /// </summary>
    /// <param name="plii">receives the time of the last input event</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport("user32.dll")]
    internal static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
  }
  
  [StructLayout(LayoutKind.Sequential)]
  internal struct LASTINPUTINFO
  {
    public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

    [MarshalAs(UnmanagedType.U4)]
    public uint cbSize;

    [MarshalAs(UnmanagedType.U4)]
    public uint dwTime;
  }
}