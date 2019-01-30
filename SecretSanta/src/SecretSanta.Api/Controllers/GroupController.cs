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
        public ActionResult<DTO.Group> AddGroup(DTO.Group @group)
        {
            if (group == null) return BadRequest();
            
            return new DTO.Group(_GroupService.AddGroup(DTO.Group.ToDomain(group)));
        }
        
        [HttpGet]
        public ActionResult<List<DTO.Group>> GetAllGroups()
        {
            List<Group> databaseGroups = _GroupService.FetchAllGroups();

            return databaseGroups.Select(x => new DTO.Group(x)).ToList();
        }
    }
}