using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileDownload
{
  public class MainViewModel : PropertyChangedBase
  {
    #region 绑定数据

    private string _Url;

    public string Url
    {
      get { return _Url; }
      set
      {
        _Url = value;
        RaisePropertyChanged("Url");
      }
    }

    private int _Progress;

    public int Progress
    {
      get { return _Progress; }
      set
      {
        _Progress = value;
        NotifyOfPropertyChange("Progress");
      }
    }

    #endregion 绑定数据

    /// <summary>
    /// start download file
    /// </summary>
    public async Task DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
      var sDownloader = new DownloadManager();
      var progress = new Progress<int>(dd =>
      {
        Progress = dd;
      });
      await sDownloader.DownloadFileAsync(Url, filePath, progress, cancellationToken);
    }
  }
}