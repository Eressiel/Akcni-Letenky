﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LetenkyParser
{
    [Serializable]
    [XmlRoot("Titles")]
    public class Title
    {
        public int Id { get; set; }
        public string ArticleTitle { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }

        public Title(string ArticleTitle, string Url, DateTime Date)
        {
            this.ArticleTitle = ArticleTitle;
            this.Url = Url;
            this.Date = Date;
        }

        public Title()
        {

        }
    }
}
