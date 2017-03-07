using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FileDownload
{
  public class SimpleDownloader
  {
    public SimpleDownloader(string directoryPath)
    {
      _DirectoryPath = directoryPath;
    }

    private readonly string _DirectoryPath;

    /// <summary>
    /// default http client download
    /// </summary>
    public async Task<string> Download(string fileName, string address, IProgress<int> progress)
    {
      string file = Path.Combine(_DirectoryPath, fileName);
      string url = address;

      using (WebClient client = new SuperWebClient())
      {
        client.DownloadProgressChanged += (ss, ee) =>
        {
          progress.Report(ee.ProgressPercentage);
        };

        try
        {
          await client.DownloadFileTaskAsync(url, file);
        }
        catch
        {
          File.Delete(file);
          throw;
        }
      }

      return file;
    }    
  }
}