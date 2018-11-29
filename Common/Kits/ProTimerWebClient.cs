using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Common
{
    /// <summary>
    /// WebClient 重写
    /// </summary>
    public class ProTimerWebClient : WebClient
    {
        private int timeOut;

        public ProTimerWebClient(int timeOut)
        {
            this.timeOut = timeOut;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            return request;
        }
    }
}
