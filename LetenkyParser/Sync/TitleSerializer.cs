using LetenkyParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LetenkyParser.Sync
{
    public class TitleSerializer
    {
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/";
        private static string titlesPath = basePath + "titles.xml";

        private int monthsToKeepTitles = 2;
        private XmlSerializer serializer;

        private TitleList titles;


        public TitleSerializer()
        {
            titles = new TitleList();
            serializer = new XmlSerializer(typeof(TitleList));
            LoadExistingTitles();
        }

        public void AddNewTitlesAndSerialize(TitleList newTitles)
        {
            LoadExistingTitles();
            AddNewTitlesToOldTitles(newTitles);
            RemoveDuplicatesFroTitles();
            Serialize();
        }

        public void RemoveDuplicatesFroTitles()
        {
            titles.Titles = titles.Titles.Distinct().ToList();
        }

        public List<Title> GetTitles()
        {
            return titles.Titles;
            
        }

        private void LoadExistingTitles()
        {
            using (var fileStream = new FileStream(titlesPath, FileMode.OpenOrCreate))
            {
                titles = (TitleList)serializer.Deserialize(fileStream);
            }
        }


        private void AddNewTitlesToOldTitles(TitleList newTitles)
        {
            DateTime latestTitleDate = GetLatestDateTimeFromOldTitles();
            List<Title> titlesToBeAdded = GetTitlesNewerThanDate(newTitles, latestTitleDate);
            titles.Titles.AddRange(titlesToBeAdded);
        }

        private List<Title> GetTitlesNewerThanDate(TitleList newTitles, DateTime latestTitleDate)
        {
            var items = newTitles.Titles.Where(title => title.Date > latestTitleDate);
            var titles = items.ToList<Title>();
            return titles;
        }

        public TitleList RemoveOldTitles(TitleList titles)
        {
            var items = titles.Titles.Where(title => title.Date > DateTime.Now.AddMonths(-monthsToKeepTitles));
            var titleList = new List<Title>();
            titleList = items.ToList();
            titleList = titleList.OrderByDescending(title => title.Date).ToList<Title>();
            titles.Titles = titleList;
            return titles;
        }


        private DateTime GetLatestDateTimeFromOldTitles()
        {
            try
            {
                DateTime latestTitleDate = titles.Titles.Max(r => r.Date);
                return latestTitleDate;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }


        private void Serialize()
        {
            using (var fileStream = new FileStream(titlesPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, titles);
            }
        }
    }
}
