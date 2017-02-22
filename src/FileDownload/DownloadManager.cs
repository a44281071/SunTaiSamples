﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FileDownload
{
  internal class DownloadManager
  {
    private Stopwatch _downloadStopWatch = new Stopwatch();
    private static readonly int BufferSize = 32768;

    public async Task DownloadFileAsync(string url, string filePath, IProgress<int> progress, CancellationToken cancellationToken = default(CancellationToken))
    {
      //bool resumeDownload = IsResume(url, filePath);
      string tempFileName = filePath + ".temp";
      FileMode fm = FileMode.Create;
      this._downloadStopWatch.Start();
      try
      {
        Uri installerUrl = new Uri(url);
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

        double contentLength = await GetContentLengthAsync(url);
        byte[] buffer = new byte[BufferSize];
        long downloadedLength = 0;
        int currentDataLength;

        cancellationToken.ThrowIfCancellationRequested();

        if (File.Exists(tempFileName))
        {
          FileInfo fn = new FileInfo(tempFileName);
          httpWebRequest.AddRange(fn.Length);
          downloadedLength = fn.Length;
          fm = FileMode.Append;
        }

        using (var response = (HttpWebResponse)(await httpWebRequest.GetResponseAsync()))
        using (var netStream = response.GetResponseStream())
        using (var fileStream = new FileStream(tempFileName, fm))
        {
          cancellationToken.ThrowIfCancellationRequested();

          while ((currentDataLength = await netStream.ReadAsync(buffer, 0, BufferSize)) > 0)
          {
            cancellationToken.ThrowIfCancellationRequested();

            await fileStream.WriteAsync(buffer, 0, currentDataLength);
            downloadedLength += currentDataLength;

            if (this._downloadStopWatch.ElapsedMilliseconds > 1000)
            {
              this._downloadStopWatch.Reset();
              this._downloadStopWatch.Start();

              double doubleDownloadPersent = 0.0;
              if (contentLength > 0.0)
              {
                doubleDownloadPersent = downloadedLength / contentLength;
                if (doubleDownloadPersent > 1.0)
                {
                  doubleDownloadPersent = 1.0;
                }
              }

              int intDownloadPersent = (int)(doubleDownloadPersent * 100);
              progress.Report(intDownloadPersent);
            }
          }
          progress.Report(100);
        }
      }
      finally
      {
        this._downloadStopWatch.Stop();
      }
    }

    private async Task<long> GetContentLengthAsync(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
      var res = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());

      return res.ContentLength;
    }

    private static bool GetAcceptRanges(WebResponse res)
    {
      if (res.Headers["Accept-Ranges"] != null)
      {
        string s = res.Headers["Accept-Ranges"];
        if (s == "none")
        {
          return false;
        }
      }
      return true;
    }
  }
}