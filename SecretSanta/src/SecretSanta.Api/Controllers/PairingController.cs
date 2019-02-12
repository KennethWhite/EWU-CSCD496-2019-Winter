using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using Microsoft.AspNetCore.Http;
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

        // GET api/Pairing/5
        [HttpGet("{groupId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GenerateUserPairings(int groupId)
        {
            if (groupId <= 0) return NotFound();
            
            List<Pairing> pairingsGenerated = await PairingService.GenerateUserPairings(groupId);
           
            return CreatedAtAction(nameof(GenerateUserPairings), new { id = groupId}, pairingsGenerated);
        }
    }
}
