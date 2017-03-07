﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FileDownload
{
  internal class SuperDownloader
  {
    private static readonly int BufferSize = 32768;  // 32KB
    private readonly int _HttpTimeout = 30 * 1000;
    private readonly int _SteamTimeout = 30 * 1000;

    private Stopwatch _Stopwatch = new Stopwatch();

    public async Task DownloadFileAsync(string url
      , string filePath
      , IProgress<DownloadArgs> progress
      , CancellationToken cancellationToken = default(CancellationToken))
    {
      FileMode fm = FileMode.Create;
      long lastSpeedDownloadedLength = 0;
      long downloadedLength = 0;
      long contentLength = await GetContentLengthAsync(url);

      Uri installerUrl = new Uri(url);
      byte[] buffer = new byte[BufferSize];
      int currentDataLength;

      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      httpWebRequest.Timeout = _HttpTimeout;
      httpWebRequest.ReadWriteTimeout = _SteamTimeout;
      httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
      cancellationToken.ThrowIfCancellationRequested();

      if (File.Exists(filePath))
      {
        FileInfo fn = new FileInfo(filePath);
        httpWebRequest.AddRange(fn.Length);
        downloadedLength = fn.Length;
        fm = FileMode.Append;
      }

      using (var response = (HttpWebResponse)(await httpWebRequest.GetResponseAsync()))
      using (var netStream = response.GetResponseStream())
      using (var fileStream = new FileStream(filePath, fm))
      {
        cancellationToken.ThrowIfCancellationRequested();
        _Stopwatch.Restart();

        while ((currentDataLength = await netStream.ReadAsync(buffer, 0, BufferSize)) > 0)
        {
          cancellationToken.ThrowIfCancellationRequested();

          await fileStream.WriteAsync(buffer, 0, currentDataLength);
          downloadedLength += currentDataLength;

          #region ProgressReport

          if (_Stopwatch.ElapsedMilliseconds > 500)
          {
            double speedSecond = (downloadedLength - lastSpeedDownloadedLength) / ((double)_Stopwatch.ElapsedMilliseconds) * 1000d;
            progress.Report(new DownloadArgs(downloadedLength, contentLength, (long)speedSecond));
            lastSpeedDownloadedLength = downloadedLength;
            _Stopwatch.Restart();
          }

          #endregion ProgressReport
        }
        progress.Report(new DownloadArgs(contentLength, contentLength, 0));
      }
    }

    private async Task<long> GetContentLengthAsync(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      httpWebRequest.Timeout = _HttpTimeout;
      httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
      using (var req = (HttpWebResponse)(await httpWebRequest.GetResponseAsync()))
      {
        return req.ContentLength;
      }
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