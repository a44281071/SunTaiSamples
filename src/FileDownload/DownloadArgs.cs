using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownload
{
  /// <summary>
  /// 下载进度通知值
  /// </summary>
  public class DownloadArgs : EventArgs
  {
    /// <summary>
    /// 使用当前计数、总量计数，初始化进度值
    /// </summary>
    /// <param name="downloaded">length of downloaded</param>
    /// <param name="total">total length of content</param>
    /// <param name="speed">length per second.</param>
    public DownloadArgs(long downloaded
      , long total
      , long speed)
    {
      DownloadedLength = downloaded;
      ContentLength = total;
      SpeedLength = speed;
    }

    /// <summary>
    /// length of downloaded.
    /// </summary>
    public long DownloadedLength { get; set; }
    /// <summary>
    /// total length of content.
    /// </summary>
    public long ContentLength { get; set; }

    /// <summary>
    /// length per second.
    /// </summary>
    public long SpeedLength { get; set; }

    /// <summary>
    /// 下载进度百分比
    /// </summary>
    public int DownloadPersent
    {
      get
      {
        double doubleDownloadPersent = 0.0;
        if (ContentLength > 0.0)
        {
          doubleDownloadPersent = (double)DownloadedLength / ContentLength;
          if (doubleDownloadPersent > 1.0)
          {
            doubleDownloadPersent = 1.0;
          }
        }

        return (int)(doubleDownloadPersent * 100);
      }
    }
  }
}
