using LetenkyParser;
using LetenkyParser.Sync;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace AkcniLetenkyApi.Controllers
{
    public class TitlesController : ApiController
    {


        public TitlesController()
        {
        }

        public IEnumerable<Title> GetTitles()
        {
            //TitleDownloader.Instance.UpdateTitles();
            return TitleDownloader.Instance.GetTitles().OrderByDescending(t => t.Date);
        }

        [Route("api/titles/{date}")]
        public IEnumerable<Title> GetTitles(string date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            //DateTime test = DateTime.ParseExact(date, "yyyyMMddHHmmss", provider);
            //List<Title> testlist = new List<Title>();
            //testlist = TitleDownloader.Instance.GetTitles().Where(t => t.Date > DateTime.ParseExact(date, "yyyyMMddHHmmss", provider)).OrderByDescending(t => t.Date).ToList<Title>();
            return TitleDownloader.Instance.GetTitles().Where(t => t.Date > DateTime.ParseExact(date, "yyyyMMddHHmmss", provider)).OrderByDescending(t => t.Date);
        }
    }
}
