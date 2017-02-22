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
      //await DownloadFileAsync();

      await SuperDownloadFileAsync();
    }

    private async Task DownloadFileAsync()
    {
      try
      {
        Stopwatch sWatch = new Stopwatch();
        IProgress<int> progress = new Progress<int>(dd =>
        {
          pgbProgress.Value = dd;
          txtProgress.Text = $"{sWatch.Elapsed.TotalSeconds}s：{dd}%";
        });
        string fileName = Path.GetRandomFileName();
        string url = txtUrl.Text;

        // start download file.
        sWatch.Restart();
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

    private async Task SuperDownloadFileAsync()
    {
      try
      {
        Stopwatch sWatch = new Stopwatch();
        IProgress<int> progress = new Progress<int>(dd =>
        {
          Dispatcher.Invoke(() =>
          {
            pgbProgress.Value = dd;
            txtProgress.Text = $"{sWatch.Elapsed.TotalSeconds}s：{dd}%";
          });
        });
        string fileName = Path.Combine(App.WORK_DIR, "textabc.exe");
        string url = txtUrl.Text;

        // start download file.
        sWatch.Restart();
        var sDownloader = new DownloadManager();
        await sDownloader.DownloadFileAsync(url, fileName, progress);

        // open dir explorer and select file.        
        ShowWorkDir(fileName);
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