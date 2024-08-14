using System;
using System.IO;
using System.Net;
using System.Text;

namespace MiniServer
{


class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Listening for requests on http://localhost:8080/ ...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                // Log the request method (GET/POST)
                Console.WriteLine("Received {0} request for: {1}", request.HttpMethod, request.Url);

                // Log the headers
                Console.WriteLine("Headers:");
                foreach (string key in request.Headers.AllKeys)
                {
                    Console.WriteLine("{0}: {1}", key, request.Headers[key]);
                }

                // Log the body if it's a POST request
                if (request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string body = reader.ReadToEnd();
                        Console.WriteLine("Body:");
                        Console.WriteLine(body);
                    }
                }

                // Respond with a simple HTML page
                HttpListenerResponse response = context.Response;
                string responseString = "<html><body><h1>Request Received</h1></body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }


}
