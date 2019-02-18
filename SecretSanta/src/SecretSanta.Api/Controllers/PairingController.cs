using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingController : ControllerBase
    {
        private IPairingService PairingService { get; }
        private IMapper Mapper { get; }

        public PairingController(IPairingService pairingService, IMapper mapper)
        {
            PairingService = pairingService;
            Mapper = mapper;
        }

        // POST api/pairing/5
        [HttpPost("{groupId}")]
        [Produces(typeof(ICollection<PairingViewModel>))]
        public async Task<IActionResult> GenerateUserPairings(int groupId)
        {
            if (groupId <= 0)
            {
                return BadRequest("A group id must be specified");
            }
            

            IActionResult ret;
            try
            {
                var pairings = await PairingService.GenerateUserPairings(groupId);
                ret = new CreatedResult($"/pairing/{groupId}", pairings.Select(p => Mapper.Map<PairingViewModel>(p)).ToList());
            }
            catch (Exception e)
            {
                ret = base.NotFound($"exception: {e.Message}");
            }
            return ret;
        }
    }
}