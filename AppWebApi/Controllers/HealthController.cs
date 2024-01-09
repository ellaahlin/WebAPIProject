using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HealthController : Controller
    {
        IAttractionService _service = null;
        ILogger<AttractionsController> _logger = null;

        // GET: health/hello
        [HttpGet()]
        [ActionName("Hello")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Hello()
        {
            //to verify the layers are accessible
            string sRet = $"\nLayer access:\n{csAppConfig.Hello}" +
                $"\n{csAttraction.Hello}" +
                $"\n{csLoginService.Hello}" +
                $"\n{csJWTService.Hello}" +
                $"\n{csAttractionsServiceModel.Hello}";


            //to verify connection strings can be read from appsettings.json
            sRet += $"\n\nDbConnections:\nDbLocation: {csAppConfig.DbSetActive.DbLocation}" +
                $"\nDbServer: {csAppConfig.DbSetActive.DbServer}";

            sRet += "\nDbUserLogins in DbSet:";
            foreach (var item in csAppConfig.DbSetActive.DbLogins)
            {
                sRet += $"\n   DbUserLogin: {item.DbUserLogin}" +
                    $"\n   DbConnection: {item.DbConnection}\n   ConString: <secret>";
            }

            return Ok(sRet);
        }

        //GET: health/log
        [HttpGet()]
        [ActionName("Log")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<csLogMessage>))]
        public async Task<IActionResult> Log([FromServices] ILoggerProvider _loggerProvider)
        {
            //Note the way to get the LoggerProvider, not the logger from Services via DI
            if (_loggerProvider is csInMemoryLoggerProvider cl)
            {
                return Ok(await cl.MessagesAsync);
            }
            return Ok("No messages in log");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        public HealthController(IAttractionService service, ILogger<AttractionsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}

