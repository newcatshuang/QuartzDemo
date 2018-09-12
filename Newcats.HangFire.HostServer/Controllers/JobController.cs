using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newcats.HangFire.HostServer.Models;
using Newcats.HangFire.HostServer.Services;

namespace Newcats.HangFire.HostServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("{add}")]
        public ActionResult<BaseResult> Add(string spName, string cronString)
        {
            RecurringJob.AddOrUpdate(() => _jobService.ExecuteStoredProcedureJobAsync(spName, null, null), Cron.Minutely);
            return new BaseResult(0, "success");
        }
    }
}