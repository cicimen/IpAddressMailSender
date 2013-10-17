using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Configuration;
using HtmlAgilityPack;
using System.Web;

namespace IpAddressMailSender
{
    class IP
    {
        public static string GetIP()
        {
            try
            {
                string IP = "";

                WebClient wc = new WebClient();
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                string siteHTML = wc.DownloadString("http://www.whatismyip.com/");
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(siteHTML);

                var nodes = htmlDoc.DocumentNode
               .Descendants("td")
               .Where(td => td.GetAttributeValue("class", "").Contains("ip"))
               .SelectMany(index => index.Descendants())
               .ToArray();

                foreach (var node in nodes)
                {
                    IP = IP + HttpUtility.HtmlDecode(node.InnerText.Trim());
                }

                return IP;
            }
            catch
            {
                return null;
            }

        }


    }
}
