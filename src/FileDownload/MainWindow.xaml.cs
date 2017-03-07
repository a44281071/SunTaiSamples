using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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

    CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

    public MainViewModel ViewModel { get; } = new MainViewModel();

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
        Downloader _Downloader = new Downloader(App.WORK_DIR);
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
        string fileName = Path.Combine(App.WORK_DIR, "textabc.exe");      
        await ViewModel.DownloadFileAsync( fileName, _CancellationTokenSource.Token);

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
      proc.StartInfo.Arguments = $"/select,{fileFullName}";   
      proc.Start();
    }

    private async void btnDownload_Click(object sender, RoutedEventArgs e)
    {
      _CancellationTokenSource = new CancellationTokenSource();

      //await DownloadFileAsync();
      await SuperDownloadFileAsync();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _CancellationTokenSource.Cancel();
    }
  }
}