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

    /// <summary>
    /// download progress percent.
    /// </summary>
    public int Progress
    {
      get { return _Progress; }
      set
      {
        if (_Progress == value) return;
        _Progress = value;
        NotifyOfPropertyChange("Progress");
      }
    }

    private long _DownloadSpeed;
    /// <summary>
    /// speed per second.
    /// </summary>
    public long DownloadSpeed
    {
      get { return _DownloadSpeed; }
      set
      {
        _DownloadSpeed = value;
        RaisePropertyChanged("DownloadSpeed");
      }
    }

    #endregion 绑定数据

    /// <summary>
    /// start download file async.
    /// </summary>
    public async Task DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
      // build progress handler.
      var progress = new Progress<DownloadArgs>(dd =>
      {
        DownloadSpeed = dd.SpeedLength;
        Progress = dd.DownloadPersent;
      });

      // start download async.
      var sDownloader = new SuperDownloader();
      await sDownloader.DownloadFileAsync(Url, filePath, progress, cancellationToken);
    }
  }
}