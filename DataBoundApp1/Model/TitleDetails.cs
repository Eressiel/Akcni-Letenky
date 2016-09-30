using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBoundApp1.Model
{
    public class TitleDetails
    {
        public int Id { get; set; }
        public string ArticleTitle { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }


        public TitleDetails(string ArticleTitle, string Url, DateTime Date, int Id)
        {
            this.Id = Id;
            this.ArticleTitle = ArticleTitle;
            this.Url = Url;
            this.Date = Date;
        }

        public TitleDetails()
        {

        }
    }
}
