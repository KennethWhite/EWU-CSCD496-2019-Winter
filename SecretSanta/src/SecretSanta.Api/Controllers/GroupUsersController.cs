using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    public class GroupUsersController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private ILogger Logger { get; }

        public GroupUsersController(IGroupService groupService, ILogger logger)
        {
            GroupService = groupService;
            Logger = logger;
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> AddUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Logger.LogDebug($"{nameof(groupId)} invalid on call to {nameof(AddUserToGroup)}. BadRequest Returned.");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Logger.LogDebug($"{nameof(userId)} invalid on call to {nameof(AddUserToGroup)}. BadRequest Returned.");
                return BadRequest();
            }

            if (await GroupService.AddUserToGroup(groupId, userId))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{groupId}")]
        public async Task<ActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Logger.LogDebug($"{nameof(groupId)} invalid on call to {nameof(RemoveUserFromGroup)}. BadRequest Returned.");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Logger.LogDebug($"{nameof(userId)} invalid on call to {nameof(RemoveUserFromGroup)}. BadRequest Returned.");
                return BadRequest();
            }

            if (await GroupService.RemoveUserFromGroup(groupId, userId))
            {
                return Ok();
            }
            Logger.LogDebug($"{nameof(groupId)} or {nameof(userId)} not found on call to {nameof(RemoveUserFromGroup)}. NotFound Returned.");
            return NotFound();
        }
    }
}
