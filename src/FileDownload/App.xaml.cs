using System;
using System.IO;
using System.Windows;

namespace FileDownload
{
  /// <summary>
  /// App.xaml 的交互逻辑
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      Directory.CreateDirectory(WORK_DIR);
    }

    internal static readonly string WORK_DIR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
  }
}