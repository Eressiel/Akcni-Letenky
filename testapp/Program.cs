using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetenkyParser;
using LetenkyParser.Model;
using System.IO;
using LetenkyParser.Sync;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            var websites = new WebsiteUrls();
            var flightOffers = new List<Title>();
            string[] rssFeedsUrls = websites.Websites;


            var titleDownloader = TitleDownloader.Instance;
            flightOffers = titleDownloader.Titles;
            var titles = new TitleList();
            titles.Titles = flightOffers;

            var titleSerializer = new TitleSerializer();
            var testitems = titleSerializer.RemoveOldTitles(titles);
            titleSerializer.AddNewTitlesAndSerialize(titles);

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
