﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Configuration;
using Models;
using Models.DTO;

using Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuestController : Controller
    {
        IAttractionService _attractionService = null;
        ILoginService _loginService = null;
        ILogger<GuestController> _logger = null;

        //GET: api/admin/info
        [HttpGet()]
        [ActionName("Info")]
        [ProducesResponseType(200, Type = typeof(gstusrInfoDbDto))]
        public async Task<IActionResult> Info()
        {
            var info = await _attractionService.InfoAsync;
            return Ok(info);
        }


        //POST: api/Login/LoginUser
        [HttpPost]
        [ActionName("LoginUser")]
        [ProducesResponseType(200, Type = typeof(loginUserSessionDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> LoginUser([FromBody] loginCredentialsDto userCreds)
        {
            _logger.LogInformation("LoginUser initiated");

            try
            {
                var _usr = await _loginService.LoginUserAsync(userCreds);
                _logger.LogInformation($"{_usr.UserName} logged in");
                return Ok(_usr);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Login Error: {ex.Message}");
                return BadRequest($"Login Error: {ex.Message}");
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        /*
        public GuestController(IFriendsService friendService, ILogger<GuestController> logger)
        {
            _friendService = friendService;
            _logger = logger;
        }*/
        public GuestController(IAttractionService attractionService, ILoginService loginService, ILogger<GuestController> logger)
        {
            _attractionService = attractionService;
            _loginService = loginService;

            _logger = logger;
        }
        
        #endregion
    }
}

