using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Topshelf;

namespace Newcats.JobManager.Host
{
    public class JobServer : ServiceControl
    {
        private readonly ILog _log;
        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public JobServer()
        {
            _log = LogManager.GetLogger(typeof(JobServer));
            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                _schedulerFactory = new StdSchedulerFactory();
                _scheduler = await _schedulerFactory.GetScheduler().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _log.Error($"Server initialization failed: {e.Message}", e);
                throw;
            }
        }

        public void Start()
        {
            try
            {
                _scheduler.Start();
            }
            catch (Exception e)
            {
                _log.Fatal($"Scheduler start failed: {e.Message}", e);
                throw;
            }
            _log.Info("Scheduler started successfully");
        }

        public void Stop()
        {
            try
            {
                _scheduler.Shutdown(true);
            }
            catch (Exception e)
            {
                _log.Error($"Scheduler stop failed: {e.Message}", e);
                throw;
            }

            _log.Info("Scheduler shutdown complete");
        }

        /// <summary>
        /// Pauses all activity in scheduler.
        /// </summary>
	    public virtual void Pause()
        {
            _scheduler.PauseAll();
        }

        /// <summary>
        /// Resumes all activity in server.
        /// </summary>
	    public void Resume()
        {
            _scheduler.ResumeAll();
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Start()"/>.
        /// </summary>
        public bool Start(HostControl hostControl)
        {
            Start();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Stop()"/>.
        /// </summary>
        public bool Stop(HostControl hostControl)
        {
            Stop();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Pause()"/>.
        /// </summary>
        public bool Pause(HostControl hostControl)
        {
            Pause();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Resume()"/>.
        /// </summary>
        public bool Continue(HostControl hostControl)
        {
            Resume();
            return true;
        }
    }
}