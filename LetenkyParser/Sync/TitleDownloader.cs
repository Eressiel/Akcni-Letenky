using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Threading;
using LetenkyParser.Sync;
using LetenkyParser.Model;
using System.IO;

namespace LetenkyParser
{
    public class TitleDownloader
    {
        // Singleton
        private static object syncObj = new Object();
        private static volatile TitleDownloader instance;
        private SyndicationFeed articles;
        private XmlReader xmlReader;
        private string[] sourceWebsiteUrls;


        private TitleSerializer serializer;


        public static TitleDownloader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                        {
                            instance = new TitleDownloader();
                        }
                    }
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        private TitleDownloader()
        {
            var websites = new WebsiteUrls();
            sourceWebsiteUrls = websites.Websites;
            serializer = new TitleSerializer();
        }

        ~TitleDownloader()
        {
        }

        public void UpdateTitles()
        {
            //Log updating event
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/updates.txt", DateTime.Now.ToString() + Environment.NewLine);

            var titles = new TitleList();
            foreach (var sourceWebsiteUrl in sourceWebsiteUrls)
            {
                titles.Titles.AddRange(LoadAndReturnTitles(sourceWebsiteUrl));
            }
            titles.Titles = RemoveDuplicateTitlesFromList(titles.Titles);
            serializer.AddNewTitlesAndSerialize(titles);
        }

        public void LoadTitlesFromWebsite(string feedUrl)
        {
            try
            {   
                xmlReader = XmlReader.Create(feedUrl);  
                articles = SyndicationFeed.Load(xmlReader);
                xmlReader.Close();
            }
            catch(Exception e)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/downloadErrors.txt", e.Message +"," + DateTime.Now + Environment.NewLine);
            }
        }

        public List<Title> GetTitlesFromArticles()
        {
            List<Title> titles = new List<Title>();
            foreach (var articleItem in articles.Items)
            {
                titles.Add(CreateTitle(
                    articleItem.Title.Text,
                    articleItem.Links.First().Uri.OriginalString,
                    articleItem.PublishDate.DateTime));
            }
            return titles;
        }

        private List<Title> RemoveDuplicateTitlesFromList(List<Title> titles)
        {
            return titles.GroupBy(x => x.ArticleTitle).Select(x => x.First()).ToList();
        }

        private bool IsItemInList(List<Title> titleList, string ArticleTitle)
        {
            bool found = false;
            foreach (var item in titleList)
            {
                if (item.ArticleTitle == ArticleTitle)
                    found = true;
            }
            return found;
        }

        public List<Title> LoadAndReturnTitles(string feedUrl)
        {
            LoadTitlesFromWebsite(feedUrl);
            return GetTitlesFromArticles();
        }


        private Title CreateTitle(string articleTitle, string url, DateTime date)
        {
            return new Title(articleTitle, url, date);
        }

        public List<Title> GetTitles()
        {
            return serializer.GetTitles();
        }

    }
}
