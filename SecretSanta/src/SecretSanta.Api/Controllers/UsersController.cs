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
    public class UsersController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }
        private ILogger Logger { get; }

        public UsersController(IUserService userService, IMapper mapper, ILogger logger)
        {
            UserService = userService;
            Mapper = mapper;
            Logger = logger;
        }

        // GET api/User
        [HttpGet]
        public async Task<ActionResult<ICollection<UserViewModel>>> GetAllUsers()
        {
            var users = await UserService.FetchAll();
            return Ok(users.Select(x => Mapper.Map<UserViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var fetchedUser = await UserService.GetById(id);
            if (fetchedUser == null)
            {
                Logger.LogDebug($"{nameof(fetchedUser)} not found on call to {nameof(GetUser)} with {nameof(id)} = {id}. BadRequest Returned.");
                return NotFound();
            }

            return Ok(Mapper.Map<UserViewModel>(fetchedUser));
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogDebug($"{nameof(viewModel)} null on call to {nameof(CreateUser)}. BadRequest Returned.");
                return BadRequest();
            }

            var createdUser = await UserService.AddUser(Mapper.Map<User>(viewModel));

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, Mapper.Map<UserViewModel>(createdUser));
        }

        // PUT api/User/5
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogDebug($"{nameof(viewModel)} null on call to {nameof(CreateUser)}. BadRequest Returned.");
                return BadRequest();
            }
            var fetchedUser = await UserService.GetById(id);
            if (fetchedUser == null)
            {
                Logger.LogDebug($"{nameof(fetchedUser)} null after call to {nameof(UserService.GetById)}. NotFound Returned.", id);
                return NotFound();
            }

            Mapper.Map(viewModel, fetchedUser);
            await UserService.UpdateUser(fetchedUser);
            return NoContent();
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                Logger.LogDebug($"{nameof(id)} invalid on call to {nameof(DeleteUser)}. BadRequest Returned.", id);
                return BadRequest("A User id must be specified");
            }

            if (await UserService.DeleteUser(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
