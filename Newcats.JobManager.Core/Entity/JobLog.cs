using System;
using Newcats.JobManager.Core.Repository;

namespace Newcats.JobManager.Core.Entity
{
    [TableName("JobLog")]
    public class JobLog : IEntity
    {
        /// <summary>
        /// LogId
        /// </summary>				
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        /// <summary>
        /// JobID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>				
        public DateTime? ExecutionTime { get; set; }

        /// <summary>
        /// 执行持续时长
        /// </summary>				
        public double? ExecutionDuration { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>				
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>				
        public string RunLog { get; set; }
    }
}