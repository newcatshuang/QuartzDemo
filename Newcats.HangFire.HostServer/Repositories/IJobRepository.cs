using System.Threading.Tasks;
using Dapper;

namespace Newcats.HangFire.HostServer.Repositories
{
    public interface IJobRepository
    {
        Task<int> ExecuteStoredProcedureAsync(string storedProcedureName, DynamicParameters pars, int? commandTimeout = null);
    }
}
