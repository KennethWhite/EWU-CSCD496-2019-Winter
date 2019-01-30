using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using SecretSanta.Api.DTO;


namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService userService)
        {
            _UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        //Post api/User
        [HttpPost("{user}")]
        public ActionResult<DTO.User> CreateUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            Domain.Models.User domUser = DTO.User.ToDomain(user);
            _UserService.AddUser(domUser);
            DTO.User createdUser = new DTO.User(domUser);

            return createdUser;
        }

        [HttpPut("{user}")]
        public ActionResult<DTO.User> UpdateUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            Domain.Models.User domUser = DTO.User.ToDomain(user);
            _UserService.UpdateUser(domUser);
            return new DTO.User(domUser);
        }

        // Put api/User/5
        [HttpDelete("{user}")]
        public ActionResult<DTO.User> DeleteUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            Domain.Models.User domUser = DTO.User.ToDomain(user);
            _UserService.DeleteUser(domUser);
            return new DTO.User(domUser);
        }


    }
}
