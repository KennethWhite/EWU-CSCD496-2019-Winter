using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut]
        public ActionResult<DTO.Group> UpdateGroup(DTO.Group group)
        {
            if (group == null) return BadRequest();
            
            return new DTO.Group(_GroupService.UpdateGroup(DTO.Group.ToDomain(group)));
        }
        
        [HttpDelete]
        public ActionResult<DTO.Group> RemoveGroup(DTO.Group group)
        {
            if (group == null) return BadRequest();
            
            return new DTO.Group(_GroupService.RemoveGroup(DTO.Group.ToDomain(group)));
        }

        [HttpPost("{groupId}")]
        public ActionResult<DTO.User> AddUserToGroup(int groupId, DTO.User user)
        {
            if (groupId <= 0) return NotFound();
            if (user == null) return BadRequest();
            
            var userAdded = _GroupService.AddUserToGroup(groupId, DTO.User.ToDomain(user));
            return new DTO.User(userAdded);
        }
        
        [HttpDelete("{groupId}")]
        public ActionResult<DTO.User> RemoveUserFromGroup(int groupId, DTO.User user)
        {
            if (groupId <= 0) return NotFound();
            if (user == null) return BadRequest();
            
            return new DTO.User(_GroupService.RemoveUserFromGroup(groupId, DTO.User.ToDomain(user)));
        }

        [HttpGet("{groupId}")]
        public ActionResult<List<DTO.User>> FetchAllUsersInGroup(int groupId)
        {
            if (groupId <= 0) return NotFound();

            return _GroupService.FetchAllUsersInGroup(groupId).Select(user => new DTO.User(user)).ToList();
        }
        

        [HttpGet]
        public ActionResult<List<DTO.Group>> GetAllGroups()
        {
            List<Domain.Models.Group> databaseGroups = _GroupService.FetchAllGroups();

            return databaseGroups.Select(x => new DTO.Group(x)).ToList();
        }
    }
}