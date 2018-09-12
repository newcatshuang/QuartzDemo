using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newcats.HangFire.HostServer.Repositories;

namespace Newcats.HangFire.HostServer.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<int> ExecuteStoredProcedureJobAsync(string storedProcedureName, DynamicParameters pars, int? commandTimeout = null)
        {
            return await _jobRepository.ExecuteStoredProcedureAsync(storedProcedureName, pars, commandTimeout);
        }
    }
}
