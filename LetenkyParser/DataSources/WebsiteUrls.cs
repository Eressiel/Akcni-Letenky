using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetenkyParser
{
    public class WebsiteUrls
    {
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/";
        private static string WebsiteUrlsPath = basePath + "WebsiteUrls.txt";

        public string[] Websites { get; private set; }

        public WebsiteUrls()
        {
            if (!File.Exists(WebsiteUrlsPath))
            {
                throw new FileNotFoundException();
            }
            else
            {
                Websites = File.ReadAllLines(WebsiteUrlsPath);
            }
        }
    }
}
