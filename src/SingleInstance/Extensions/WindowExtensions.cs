namespace System.Windows
{
  internal static class WindowExtensions
  {
    internal static void BringToForeground(this Window window)
    {
      if (window.WindowState == WindowState.Minimized || window.Visibility == Visibility.Hidden)
      {
        window.Show();
        window.WindowState = WindowState.Normal;
      }

      // According to some sources these steps gurantee that an app will be brought to foreground.
      window.Activate();
      window.Topmost = true;
      window.Topmost = false;
      window.Focus();
    }
  }
}