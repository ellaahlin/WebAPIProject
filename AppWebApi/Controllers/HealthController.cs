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
        IFriendsService _service = null;
        ILogger<FriendsController> _logger = null;

        // GET: health/hello
        [HttpGet()]
        [ActionName("Hello")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Hello()
        {
            //to verify the layers are accessible
            string sRet = $"\nLayer access:\n{csAppConfig.Hello}" +
                $"\n{csFriend.Hello}" +
                $"\n{csLoginService.Hello}" +
                $"\n{csJWTService.Hello}" +
                $"\n{csFriendsServiceModel.Hello}";


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

        #region constructors
        /*
        public HealthController(IFriendsService service, ILogger<FriendsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        */
        #endregion
    }
}

