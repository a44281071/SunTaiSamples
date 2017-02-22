using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileDownload
{
  public class SuperWebClient : WebClient
  {
    /// <summary>
    /// long stream read write time out for DOWNLOAD
    /// </summary>
    /// <param name="timeoutSecond">default 2 hours</param>
    public SuperWebClient(int timeoutSecond = 10)
    {
      _Timeout = timeoutSecond * 1000;
    }

    readonly int _Timeout;

    protected override WebRequest GetWebRequest(Uri address)
    {
      var request = WebRequest.Create(address) as HttpWebRequest;
      request.ReadWriteTimeout = _Timeout;

      return request;
    }
  }
}
