using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using LetenkyParser;
using System.IO;

namespace AkcniLetenkyApi.Background_Tasks
{
    public class UpdateTitlesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            TitleDownloader.Instance.UpdateTitles();
        }
        
    }   
}