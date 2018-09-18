using System;
using System.Collections.Generic;
using Newcats.JobManager.Core.Entity;
using Newcats.JobManager.Core.Repository;

namespace Newcats.JobManager.Core.Service
{
    public class JobService
    {
        private static readonly Repository<JobInfo, long> _jobRepository;

        private static readonly Repository<JobLog, long> _logRepository;

        static JobService()
        {
            _jobRepository = new Repository<JobInfo, long>();
            _logRepository = new Repository<JobLog, long>();
        }

        public static bool AddJob(JobInfo jobInfo)
        {
            jobInfo.CreateTime = DateTime.Now;
            jobInfo.UpdateTime = DateTime.Now;
            return _jobRepository.Insert(jobInfo) > 0;
        }

        public static bool ModifyJob(JobInfo jobInfo)
        {
            List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>();
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.JobType, jobInfo.JobType));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.Name, jobInfo.Name));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.AssemblyName, jobInfo.AssemblyName));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.ClassName, jobInfo.ClassName));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.Description, jobInfo.Description));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.JobArgs, jobInfo.JobArgs));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.CronExpression, jobInfo.CronExpression));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.CronExpressionDescription, jobInfo.CronExpressionDescription));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.State, jobInfo.State));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.UpdateId, jobInfo.UpdateId));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.UpdateName, jobInfo.UpdateName));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.UpdateTime, jobInfo.UpdateTime));
            return _jobRepository.Update(jobInfo.Id, dbUpdates) > 0;
        }

        public static bool RemoveJob(long jobId)
        {
            List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>();
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.IsDelete, true));
            dbUpdates.Add(new DbUpdate<JobInfo>(j => j.UpdateTime, DateTime.Now));
            return _jobRepository.Update(jobId, dbUpdates) > 0;
        }
    }
}