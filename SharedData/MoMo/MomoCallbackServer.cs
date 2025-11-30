using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SharedData.MoMo
{
    public class MomoCallbackServer
    {
        public event Action<MomoCallbackData> OnPaymentSuccess;

        public void Start()
        {
            Task.Run(() =>
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:8080/notify/");
                listener.Start();

                while (true)
                {
                    var context = listener.GetContext();

                    using var reader = new StreamReader(context.Request.InputStream);
                    string body = reader.ReadToEnd();

                    var data = JsonConvert.DeserializeObject<MomoCallbackData>(body);

                    // resultCode = 0 → PAYMENT SUCCESS
                    if (data.resultCode == 0)
                    {
                        OnPaymentSuccess?.Invoke(data);
                    }

                    // Response 200 OK
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
            });
        }
    }

    public class MomoCallbackData
    {
        public string orderId { get; set; }
        public string requestId { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
    }
}
