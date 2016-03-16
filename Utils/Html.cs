using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Text;

// ReSharper disable CheckNamespace
namespace Bergfall.Utils
// ReSharper restore CheckNamespace
{
    public class Html
    {
        public static string MobstaWebRequest(string url, string method, string postData, bool useISO8859encoding)
        {
            string htmlResponse = "";

            var uri = new Uri(url);

            // Create HttpWebRequest
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            // Set Accepted content-type (???)
            webRequest.Accept =
                "text/html, application/xml;q=0.9, application/xhtml+xml, image/png, image/webp, image/jpeg, image/gif, image/x-xbitmap, */*;q=0.1";
            webRequest.UserAgent = "Opera/9.80 (Windows NT 6.1; U; en) Presto/2.9.168 Version/11.50";
            webRequest.Headers[HttpRequestHeader.AcceptLanguage] = "en,nb-NO;q=0.9,nb;q=0.8,no-NO;q=0.7,no;q=0.6";
            webRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            webRequest.Headers[HttpRequestHeader.CacheControl] = "no-cache";
            webRequest.KeepAlive = true;

            // CookieContainer
            var cookieContainer = new CookieContainer();
            webRequest.CookieContainer = cookieContainer;

            // Request Methode (GET or POST)
            webRequest.Method = method;

            if (!String.IsNullOrEmpty(postData))
            {
                // Postdata is converted to byte[] and added to request
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                webRequest.ContentLength = byteArray.Length;

                Stream stream = webRequest.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();
            }

            // Need to set encoding in case of æøå
            Encoding encoding = useISO8859encoding ? Encoding.GetEncoding("iso-8859-1") : Encoding.Default;
            // Get WebResponse from Request and read the entire response
            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();

                if (webResponse.ContentEncoding.ToLower().Contains("gzip"))
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                else if (webResponse.ContentEncoding.ToLower().Contains("deflate"))
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                StreamReader streamReader = new StreamReader(responseStream, true);
                //if(webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Found)

                htmlResponse = streamReader.ReadToEnd();
                webResponse.Close();
                streamReader.Close();
            }

            catch (Exception exp)
            {
                htmlResponse = "Error: " + exp.Message;
            }

            return htmlResponse;
        }

        public static string MobstaWebRequest(string url)
        {
            return MobstaWebRequest(url, "GET", "", false);
        }
        ///// <summary>
        ///// Add cookies to the request object
        ///// </summary>
        //private void AddCookiesTo(HttpWebRequest request)
        //{
        //    if (Cookies != null && Cookies.Count > 0)
        //    {
        //        request.CookieContainer.Add(Cookies);
        //    }
        //}

        ///// <summary>
        ///// Saves cookies from the response object to the local CookieCollection object
        ///// </summary>
        //private void SaveCookiesFrom(HttpWebResponse response)
        //{
        //    if (response.Cookies.Count > 0)
        //    {
        //        if (Cookies == null) Cookies = new CookieCollection();
        //        Cookies.Add(response.Cookies);
        //    }
        //}
        //public List<string> GetEntireSite(string domain)
        //{
        //    var URLvisited = new List<string>();
        //    var URLnotvisited = new List<string>();

        //    MobstaWebRequest(domain);

        //}
        public bool SendGmail(string recipient, string subject, string body)
        {
            bool success = false;
            var mail = new MailMessage("mortensaus@gmail.com", recipient, subject, body);
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("mortensaus@gmail.com", "kingfish");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
            }
            return success;
        }
    }
}