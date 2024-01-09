﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Configuration;
using Models;
using Models.DTO;

using Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionsController : Controller
    {
        loginUserSessionDto _usr = null;

        IAttractionService _service = null;
        ILogger<AttractionsController> _logger = null;

        //GET: api/friends/read
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "1000", string hasComment = "true")
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

            bool _hasComment = true;
            if (!bool.TryParse(hasComment, out _hasComment))
            {
                return BadRequest("bool format error");
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

            var items = await _service.ReadAttractionsAsync(_usr, _seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize, _hasComment);
            return Ok(items);
        }

        //GET: api/friends/read
        [HttpGet()]
        [ActionName("ReadAttractionsWithComment")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true",
            string filter = null, string pageNr = "0", string pageSize = "1000", string hasComment = "true")
        {
            //Convert and check parameters
            bool _seeded = true;
            if (!bool.TryParse(seeded, out _seeded))
            {
                return BadRequest("seeded format error");
            }

            bool _hasComment = true;
            if (!bool.TryParse(hasComment, out _hasComment))
            {
                return BadRequest("bool format error");
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

            var items = await _service.ReadAttractionsWithCommentsAsync(_usr, _seeded, filter?.Trim()?.ToLower(), _pageNr, _pageSize, _hasComment);
            return Ok(items);
        }

        //GET: api/friends/readitem
        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
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

            var item = await _service.ReadAttractionAsync(_usr, _id, _flat);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }
            return Ok(item);
        }

        //DELETE: api/friends/deleteitem/id
        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
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
                var _item = await _service.DeleteAttractionAsync(_usr, _id);
                _logger.LogInformation($"item {_id} deleted");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete. Error {ex.Message}");
            }
        }

        //GET: api/friends/readitemdto
        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
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

            var item = await _service.ReadAttractionAsync(_usr, _id, false);
            if (item == null)
            {
                return NotFound($"item {_id} not found");
            }

            var dto = new csAttractionCUdto(item);
            return Ok(dto);
        }

        //PUT: api/friends/updateitem/id
        //Body: Friend in Json
        [HttpPut("{id}")]
        [ActionName("UpdateItem")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csAttractionCUdto item)
        {
            //Convert and check parameters
            Guid _id = Guid.Empty;
            if (!Guid.TryParse(id, out _id))
            {
                return BadRequest("Guid format error");
            }
            if (_id != item.AttractionID)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var _item = await _service.UpdateAttractionAsync(_usr, item);
                _logger.LogInformation($"item {_id} updated");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        //POST: api/friends/createitem
        //Body: Friend in Json
        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csAttractionCUdto item)
        {
            try
            {
                var _item = await _service.CreateAttractionAsync(_usr, item);
                _logger.LogInformation($"item {_item.AttractionID} created");

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
            base.OnActionExecuting(context);
        }

        #region constructors

        public AttractionsController(IAttractionService service, ILogger<AttractionsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}

