using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get;  }

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET api/group
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            return new OkObjectResult(GroupService.FetchAll().Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        // POST api/group
        [HttpPost]
        public IActionResult CreateGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            return Created(nameof(CreateGroup), Mapper.Map<GroupViewModel>(GroupService.AddGroup(Mapper.Map<Group>(viewModel))));
        }

        // PUT api/group/5
        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            var fetchedGroup = GroupService.Find(id);
            if (fetchedGroup == null)
            {
                return NotFound();
            }

            fetchedGroup.Name = viewModel.Name;

            return new OkObjectResult(Mapper.Map<GroupViewModel>(GroupService.UpdateGroup(fetchedGroup)));
        }

        [HttpPut("{groupId}/{userid}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                return BadRequest();
            }

            if (userId <= 0)
            {
                return BadRequest();
            }

            if (GroupService.AddUserToGroup(groupId, userId))
            {
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A group id must be specified");
            }

            if (GroupService.DeleteGroup(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
