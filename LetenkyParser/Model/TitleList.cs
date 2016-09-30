using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LetenkyParser.Model
{
    [Serializable]
    [XmlRoot("Titles")]
    public class TitleList
    {
        private List<Title> titles;
        private Semaphore semaphore = new Semaphore(1,1);

        [XmlArray("TitleList"), XmlArrayItem(typeof(Title), ElementName = "Title")]
        public List<Title> Titles
        {
            get
            {
                return titles;
            }
            set
            {
                semaphore.WaitOne();
                titles = value;
                semaphore.Release();
            }
        }

        

        public TitleList()
        {
            Titles = new List<Title>();
        }
    }
}
