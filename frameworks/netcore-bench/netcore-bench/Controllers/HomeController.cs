using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreBench.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("plaintext")]
        public IActionResult Plaintext()
        {
            return new PlainTextActionResult();
        }

        [HttpGet("json")]
        [Produces("application/json")]
        public object Json()
        {
            return new {message = "Hello, World!"};
        }

        private class PlainTextActionResult : IActionResult
        {
            private static readonly byte[] HelloWorldPayload = Encoding.UTF8.GetBytes("Hello, World!");

            public Task ExecuteResultAsync(ActionContext context)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                context.HttpContext.Response.ContentType = "text/plain";
                context.HttpContext.Response.ContentLength = HelloWorldPayload.Length;
                return context.HttpContext.Response.Body.WriteAsync(HelloWorldPayload, 0, HelloWorldPayload.Length);
            }
        }
    }
}