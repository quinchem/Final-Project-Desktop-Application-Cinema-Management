using System;
using System.IO;
using System.Net;
using System.Text;

namespace SharedData.MoMo
{
    public class PaymentRequest
    {
        public PaymentRequest() { }

        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                byte[] data = Encoding.UTF8.GetBytes(postJsonString);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;

                using (var stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)httpWReq.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {

                if (e.Response != null)
                {
                    using (var resp = (HttpWebResponse)e.Response)
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        string body = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(body))
                        {
                            return body;
                        }
                    }
                }
                return e.Message;
            }
        }
    }
}
