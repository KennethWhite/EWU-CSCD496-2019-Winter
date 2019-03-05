using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get; }
        private ILogger Logger { get; }

        public GroupsController(IGroupService groupService, IMapper mapper, ILogger logger)
        {
            GroupService = groupService;
            Mapper = mapper;
        }

        // GET api/group
        [HttpGet]
        public async Task<ActionResult<ICollection<GroupViewModel>>> GetGroups()
        {
            var groups = await GroupService.FetchAll();
            return Ok(groups.Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupViewModel>> GetGroup(int id)
        {
            var group = await GroupService.GetById(id);
            if (group == null)
            {
                Logger.LogDebug($"{nameof(group)} null after call to {nameof(GroupService.GetById)}. NotFound Returned.");
                return NotFound();
            }

            return Ok(Mapper.Map<GroupViewModel>(group));
        }

        // POST api/group
        [HttpPost]
        public async Task<ActionResult<GroupViewModel>> CreateGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogDebug($"{nameof(viewModel)} null on call to {nameof(CreateGroup)}. BadRequest Returned.");
                return BadRequest();
            }
            var createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel));
            return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id}, Mapper.Map<GroupViewModel>(createdGroup));
        }

        // PUT api/group/5
        [HttpPut]
        public async Task<ActionResult> UpdateGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogDebug($"{nameof(viewModel)} null on call to {nameof(UpdateGroup)}. BadRequest Returned.");
                return BadRequest();
            }
            var group = await GroupService.GetById(id);
            if (group == null)
            {
                Logger.LogDebug($"{nameof(group)} null after call to {nameof(GroupService.GetById)}. BadRequest Returned.");
                return NotFound();
            }

            Mapper.Map(viewModel, group);
            await GroupService.UpdateGroup(group);

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            if (id <= 0)
            {
                Logger.LogDebug($"{nameof(id)} invalid on call to {nameof(GroupService.GetById)}. BadRequest Returned.", id);
                return BadRequest("A group id must be specified");
            }

            if (await GroupService.DeleteGroup(id))
            {
                return Ok();
            }
            Logger.LogDebug($"No group found for the specified parameeter {nameof(id)}, NotFound returned", id);
            return NotFound();
        }
    }
}
