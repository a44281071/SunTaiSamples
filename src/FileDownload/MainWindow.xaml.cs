using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FileDownload
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private Downloader _Downloader = new Downloader(App.WORK_DIR);

    private async void btnDownload_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        IProgress<int> progress = new Progress<int>(dd =>
        {
          pgbProgress.Value = dd;
          // display delay.
          Task.Delay(300);
        });
        string fileName = Path.GetRandomFileName();
        string url = txtUrl.Text;

        // start download file.
        string fileFullName = await _Downloader.Download(fileName, url, progress);

        // open dir explorer and select file.
        ShowWorkDir(fileFullName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
      }
      finally
      {
        // reset progress display.
        pgbProgress.Value = 0;
      }
    }

    private void ShowWorkDir(string fileFullName)
    {
      Process proc = new Process();
      proc.StartInfo.FileName = "explorer";
      //打开资源管理器
      proc.StartInfo.Arguments = $"/select,{fileFullName}";
      //选中"notepad.exe"这个程序,即记事本
      proc.Start();
    }
  }
}