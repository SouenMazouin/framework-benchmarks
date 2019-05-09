using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBench.Models;

namespace NetCoreBench.Controllers
{
    [Route("db")]
    public class SingleQueryController : Controller
    {
        [Produces("application/json")]
        public Task<World> Dapper()
        {
            return ExecuteQuery<DapperDb>();
        }

        private Task<World> ExecuteQuery<T>() where T : IDb
        {
            var db = HttpContext.RequestServices.GetRequiredService<T>();
            return db.LoadSingleQueryRow();
        }
    }
}