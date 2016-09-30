using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace AkcniLetenkyApi.Background_Tasks
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<UpdateTitlesJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithCronSchedule("0 0/30 4-17 * * ?")
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}