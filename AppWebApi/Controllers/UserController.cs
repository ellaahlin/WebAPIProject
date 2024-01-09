using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Configuration;
using Models;
using Models.DTO;

using Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        loginUserSessionDto _usr = null;

        IAttractionService _service = null;
        ILogger<UserController> _logger = null;

        //GET: api/pets/read
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "1000")
        {
            //Convert and check parameters
            bool _seeded = true;
            if (!bool.TryParse(seeded, out _seeded))
            {
                return BadRequest("seeded format error");
            }

            bool _flat = true;
            if (!bool.TryParse(flat, out _flat))
            {
                return BadRequest("flat format error");
            }

            int _pageNr = 0;
            if (!int.TryParse(pageNr, out _pageNr))
            {
                return BadRequest("pageNr format error");
            }

            int _pageSize = 0;
            if (!int.TryParse(pageSize, out _pageSize))
            {
                return BadRequest("pageSize format error");
            }

            var items = await _service.ReadUsersAsync(_usr, _seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
            return Ok(items);
        }

        //GET: api/pets/readitem
        [HttpGet()]
        [ActionName("Readitem")]
        [ProducesResponseType(200, Type = typeof(IUser))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "false")
        {
            //Convert and check parameters
            Guid _id = Guid.Empty;
            if (!Guid.TryParse(id, out _id))
            {
                return BadRequest("Guid format error");
            }

            bool _flat = false;
            if (!bool.TryParse(flat, out _flat))
            {
                return BadRequest("flat format error");
            }

            var item = await _service.ReadUserAsync(_usr, _id, _flat);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }
            return Ok(item);
        }

        //DELETE: api/pets/deleteitem/id
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            //Convert and check parameters
            Guid _id = Guid.Empty;
            if (!Guid.TryParse(id, out _id))
            {
                return BadRequest("Guid format error");
            }

            try
            {
                var _item = await _service.DeleteUserAsync(_usr, _id);
                _logger.LogInformation($"item {_id} deleted");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete. Error {ex.Message}");
            }
        }

        //GET: api/friends/readitemdto
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "usr, supusr")]
        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            //Convert and check parameters
            Guid _id = Guid.Empty;
            if (!Guid.TryParse(id, out _id))
            {
                return BadRequest("Guid format error");
            }

            var item = await _service.ReadUserAsync(_usr, _id, false);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }

            var dto = new csUserCUdto(item);
            return Ok(dto);
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
          Policy = null, Roles = "usr, supusr")]
        [HttpPut("{id}")]
        [ActionName("UpdateItem")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csUserCUdto item)
        {
            //Convert and check parameters
            Guid _id = Guid.Empty;
            if (!Guid.TryParse(id, out _id))
            {
                return BadRequest("Guid format error");
            }
            if (_id != item.UserID)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var _item = await _service.UpdateUserAsync(_usr, item);
                _logger.LogInformation($"item {_id} updated");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "usr, supusr")]
        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csUserCUdto item)
        {
            try
            {
                var _item = await _service.CreateUserAsync(_usr, item);
                _logger.LogInformation($"item {_item.UserId} created");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Remember async programming. .Result waits for the Task to complete
            //var _token = HttpContext.GetTokenAsync("access_token").Result;
            //_usr = csJWTService.DecodeToken(_token);
            base.OnActionExecuting(context);
        }

        #region constructors
        public UserController(IAttractionService attractionService, ILogger<UserController> logger)
        {
            _service = attractionService;
            _logger = logger;
        }
        #endregion
    }
}
