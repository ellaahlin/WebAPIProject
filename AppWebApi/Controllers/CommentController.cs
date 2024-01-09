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
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentsController : Controller
    {
        loginUserSessionDto _usr = null;

        IAttractionService _service = null;
        ILogger<CommentsController> _logger = null;

        //GET: api/addresses/read
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

            var items = await _service.ReadCommentsAsync(_usr, _seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
            return Ok(items);
        }

        //GET: api/addresses/readitem
        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IComment))]
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
            
            var item = await _service.ReadCommentAsync(_usr, _id, _flat);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }
            return Ok(item);
        }

        //GET: api/addresses/readitem
        [HttpGet()]
        [ActionName("ReadCommentByUser")]
        [ProducesResponseType(200, Type = typeof(IComment))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemByUser(string id = null, string flat = "false")
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
            
            var item = await _service.ReadCommentByUserAsync(_usr, _id, _flat);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }
            return Ok(item);
        }

        //DELETE: api/addresses/deleteitem/id
        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
            => BadRequest("Not implemented");

        //GET: api/friends/readitemdto
        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
            => BadRequest("Not implemented");

        //PUT: api/addresses/updateitem/id
        //Body: Friend in Json
        [HttpPut("{id}")]
        [ActionName("UpdateItem")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csCommentCUdto item)
            => BadRequest("Not implemented");

        //POST: api/addresses/createitem
        //Body: Friend in Json
        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csCommentCUdto item)
            => BadRequest("Not implemented");

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        
        public CommentsController(IAttractionService attractionService, ILogger<CommentsController> logger)
        {
            _service = attractionService;
            _logger = logger;
        }
        
        #endregion
    }
}

