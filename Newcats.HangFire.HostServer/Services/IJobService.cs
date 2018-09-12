using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Newcats.HangFire.HostServer.Services
{
    public interface IJobService
    {
        Task<int> ExecuteStoredProcedureJobAsync(string storedProcedureName, DynamicParameters pars, int? commandTimeout = null);
    }
}
