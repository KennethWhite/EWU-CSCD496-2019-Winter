using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }

        public GiftsController(IGiftService giftService, IMapper mapper)
        {
            GiftService = giftService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
            var gift = await GiftService.GetGift(id);

            if (gift == null)
            {
                Log.Logger.Debug($"{nameof(gift)} null after call to {nameof(GiftService.GetGift)}. NotFound Returned.");
                return NotFound();
            }

            return Ok(Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateGift(int id, GiftInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Log.Logger.Debug($"{nameof(viewModel)} null on call to {nameof(UpdateGift)}. BadRequest Returned.");
                return BadRequest();
            }
            var fetchedGift = await GiftService.GetGift(id);
            if (fetchedGift == null)
            {
                return NotFound();
            }

            Mapper.Map(viewModel, fetchedGift);
            await GiftService.UpdateGift(fetchedGift);
            return NoContent();
        }


        // DELETE api/Gift/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGift(int id)
        {
            if (id <= 0)
            {
                Log.Logger.Debug($"{nameof(id)} invalid on call to {nameof(DeleteGift)}. BadRequest Returned.", id);
                return BadRequest("A Gift id must be specified");
            }

            await GiftService.RemoveGift(id);
            Log.Logger.Information($"Gift with id = {id} was removed.");
            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
            var createdGift = await GiftService.AddGift(Mapper.Map<Gift>(viewModel));
            Log.Logger.Information($"Gift was created.", viewModel);
            return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                Log.Logger.Debug($"{nameof(userId)} invalid on call to {nameof(GiftService.GetGiftsForUser)}. NotFound Returned.", userId);
                return NotFound();
            }
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
