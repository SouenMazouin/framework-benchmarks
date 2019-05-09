using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBench.Models;

namespace NetCoreBench.Controllers
{
    [Route("queries")]
    public class MultipleQueriesController : Controller
    {
        [Produces("application/json")]
        public Task<World[]> Dapper(int queries = 1)
        {
            return ExecuteQuery<DapperDb>(queries);
        }

        private Task<World[]> ExecuteQuery<T>(int queries) where T : IDb
        {
            queries = queries < 1 ? 1 : queries > 500 ? 500 : queries;
            var db = HttpContext.RequestServices.GetRequiredService<T>();
            return db.LoadMultipleQueriesRows(queries);
        }
    }
}