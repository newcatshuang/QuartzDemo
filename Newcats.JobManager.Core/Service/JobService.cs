using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
            List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>
            {
                new DbUpdate<JobInfo>(j => j.JobType, jobInfo.JobType),
                new DbUpdate<JobInfo>(j => j.Name, jobInfo.Name),
                new DbUpdate<JobInfo>(j => j.AssemblyName, jobInfo.AssemblyName),
                new DbUpdate<JobInfo>(j => j.ClassName, jobInfo.ClassName),
                new DbUpdate<JobInfo>(j => j.Description, jobInfo.Description),
                new DbUpdate<JobInfo>(j => j.JobArgs, jobInfo.JobArgs),
                new DbUpdate<JobInfo>(j => j.CronExpression, jobInfo.CronExpression),
                new DbUpdate<JobInfo>(j => j.CronExpressionDescription, jobInfo.CronExpressionDescription),
                new DbUpdate<JobInfo>(j => j.State, jobInfo.State),
                new DbUpdate<JobInfo>(j => j.UpdateId, jobInfo.UpdateId),
                new DbUpdate<JobInfo>(j => j.UpdateName, jobInfo.UpdateName),
                new DbUpdate<JobInfo>(j => j.UpdateTime, jobInfo.UpdateTime)
            };
            return _jobRepository.Update(jobInfo.Id, dbUpdates) > 0;
        }

        public static bool RemoveJob(long jobId)
        {
            List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>
            {
                new DbUpdate<JobInfo>(j => j.IsDelete, true),
                new DbUpdate<JobInfo>(j => j.UpdateTime, DateTime.Now)
            };
            return _jobRepository.Update(jobId, dbUpdates) > 0;
        }

        public static int RemoveJob(IEnumerable<long> jobIds)
        {
            int i = 0;
            if (jobIds != null && jobIds.Any())
            {
                List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>
                {
                    new DbUpdate<JobInfo>(j => j.IsDelete, true),
                    new DbUpdate<JobInfo>(j => j.UpdateTime, DateTime.Now)
                };

                List<DbWhere<JobInfo>> dbWheres = new List<DbWhere<JobInfo>>
                {
                    new DbWhere<JobInfo>(j => j.State, JobState.Stop),
                    new DbWhere<JobInfo>(j => j.Id, jobIds, OperateType.In)
                };
                i = _jobRepository.Update(dbWheres, dbUpdates);
            }
            return i;
        }

        public static JobInfo GetJob(long jobId)
        {
            return _jobRepository.Get(jobId);
        }

        public static IEnumerable<JobInfo> GetJobs(int pageIndex, int pageSize, ref int totalCount, IEnumerable<DbWhere<JobInfo>> dbWheres = null, int? commandTimeout = null, params DbOrderBy<JobInfo>[] dbOrderBy)
        {
            return _jobRepository.GetPage(pageIndex, pageSize, ref totalCount, dbWheres, commandTimeout, dbOrderBy);
        }

        public static IEnumerable<JobInfo> GetAllowScheduleJobs()
        {
            List<DbWhere<JobInfo>> dbWheres = new List<DbWhere<JobInfo>>
            {
                new DbWhere<JobInfo>(j => j.IsDelete, false),
                new DbWhere<JobInfo>(j => j.State, JobState.Stop, OperateType.NotEqual)
            };
            return _jobRepository.GetAll(dbWheres, null, new DbOrderBy<JobInfo>(j => j.CreateTime, SortType.DESC));
        }

        public static bool UpdateJobState(long jobId, JobState jobState)
        {
            return _jobRepository.Update(jobId, new List<DbUpdate<JobInfo>> { new DbUpdate<JobInfo>(j => j.State, jobState) }) > 0;
        }

        public static bool UpdateJobStatus(long jobId, DateTime lastRunTime, DateTime NextRunTime)
        {
            JobInfo job = _jobRepository.Get(jobId);
            List<DbUpdate<JobInfo>> dbUpdates = new List<DbUpdate<JobInfo>>
            {
                new DbUpdate<JobInfo>(j => j.LastRunTime, lastRunTime),
                new DbUpdate<JobInfo>(j => j.NextRunTime, NextRunTime),
                new DbUpdate<JobInfo>(j => j.RunCount, job.RunCount + 1)
            };
            return _jobRepository.Update(jobId, dbUpdates) > 0;
        }

        public static bool UpdateJobStatus(long jobId, string jobName, DateTime lastRunTime, DateTime nextRunTime, double executionDuration, string runLog)
        {
            bool isSuccess = false;
            using (TransactionScope trans = new TransactionScope())
            {
                isSuccess = UpdateJobStatus(jobId, lastRunTime, nextRunTime);
                JobLog log = new JobLog
                {
                    JobId = jobId,
                    CreateTime = DateTime.Now,
                    ExecutionDuration = executionDuration,
                    ExecutionTime = lastRunTime,
                    RunLog = runLog
                };
                isSuccess = AddLog(log);
                if (isSuccess)
                    trans.Complete();
            }
            return isSuccess;
        }

        public static bool AddLog(long jobId, DateTime executionTime, string runLog)
        {
            return AddLog(new JobLog
            {
                JobId = jobId,
                CreateTime = DateTime.Now,
                ExecutionTime = executionTime,
                ExecutionDuration = 0,
                RunLog = runLog
            });
        }

        public static bool AddLog(JobLog jobLog)
        {
            return _logRepository.Insert(jobLog) > 0;
        }

        public static JobLog GetLog(long logId)
        {
            return _logRepository.Get(logId);
        }

        public static IEnumerable<JobLog> GetLogs(int pageIndex, int pageSize, ref int totalCount, IEnumerable<DbWhere<JobLog>> dbWheres = null, int? commandTimeout = null, params DbOrderBy<JobLog>[] dbOrderBy)
        {
            return _logRepository.GetPage(pageIndex, pageSize, ref totalCount, dbWheres, commandTimeout, dbOrderBy);
        }

        public static bool DeleteLog(long logId)
        {
            return _logRepository.Delete(logId) > 0;
        }

        public static int DeleteLogs(IEnumerable<long> logIds)
        {
            if (logIds != null && logIds.Any())
            {
                return _logRepository.Delete(new List<DbWhere<JobLog>> { new DbWhere<JobLog>(l => l.Id, logIds, OperateType.In) });
            }
            return 0;
        }
    }
}