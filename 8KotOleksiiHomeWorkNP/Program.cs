using System;
using System.IO;
using System.Net;

namespace _8KotOleksiiHomeWorkNP
{
    class Program
    {
        static string requestUri = "https://google.com";
        static string uriPrefix = "http://127.0.0.1:8080/";
        static string path = "index.html";

        static void Main()
        {
            HttpWebRequest request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "GET";

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ua-UK");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var fstream = File.Open(path, FileMode.OpenOrCreate))
            {
                response.GetResponseStream().CopyTo(fstream);
            }

            HttpListener server = new HttpListener();
            server.Prefixes.Add(uriPrefix);
            server.Start();

            Console.WriteLine("Wait for request...");

            var ctx = server.GetContext();
            var res = ctx.Response;

            using (Stream stream = res.OutputStream)
            {
                using (var fstream = File.Open(path, FileMode.Open))
                {
                    fstream.CopyTo(stream);
                    Console.ReadLine();
                }
            }

            server.Stop();
            Console.WriteLine("Server stoped...");
        }
    }
}