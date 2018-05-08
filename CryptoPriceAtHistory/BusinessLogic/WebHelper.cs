﻿using System.IO;
using System.Net;

namespace CryptoPriceAtHistory.BusinessLogic
{
    public static class WebHelper
    {
        public static string GetJsonFromUrl(string fullUrl)
        {
            var webReq = (HttpWebRequest)WebRequest.Create(string.Format(fullUrl));
            webReq.Method = "GET";

            var WebResp = (HttpWebResponse)webReq.GetResponse();

            string jsonString;
            using (var stream = WebResp.GetResponseStream())
            {
                var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
    }
}
