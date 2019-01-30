using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _GroupService;

        public GroupController(IGroupService groupService)
        {
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        
        [HttpPost]
        public ActionResult<DTO.Group> AddGroup(DTO.Group group)
        {
            if (group == null) return BadRequest();
            
            return new DTO.Group(_GroupService.AddGroup(DTO.Group.ToDomain(group)));
        }

        [HttpPut]//todo multiple httpPuts on the same route
        public ActionResult<DTO.Group> UpdateGroup(DTO.Group group)
        {
            if (group == null) return BadRequest();
            return new DTO.Group(_GroupService.UpdateGroup(DTO.Group.ToDomain(group)));
        }
        
        [HttpPut]
        public ActionResult RemoveGroup(DTO.Group group)
        {
            if (group == null) return BadRequest();
            _GroupService.RemoveGroup(DTO.Group.ToDomain(group));
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<DTO.Group>> GetAllGroups()
        {
            List<Group> databaseGroups = _GroupService.FetchAllGroups();

            return databaseGroups.Select(x => new DTO.Group(x)).ToList();
        }
    }
}