using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SecretSanta.Web.ApiModels;

namespace SecretSanta.Web.Controllers
{
    public class GiftsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private IMapper Mapper { get; }
        public GiftsController(IHttpClientFactory clientFactory, IMapper mapper)
        {
            ClientFactory = clientFactory;
            Mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Users = await secretSantaClient.GetAllUsersAsync();
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Add(int userId)
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    ViewBag.UserId = userId;
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Gifts = await secretSantaClient.GetGiftsForUserAsync(userId);
                
                }
                catch (SwaggerException se)
                {
                    ViewBag.ErrorMessage = se.Message;
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GiftInputViewModel viewModel)
        {

            IActionResult result = View();
            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateGiftAsync(viewModel);

                        var routeValues = new RouteValueDictionary(new { userId = viewModel.UserId });
                        result = RedirectToAction(nameof(UserGifts), routeValues);
                    }
                    catch (SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> UserGifts(int userId)
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    ViewBag.UserId = userId;            
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Gifts = await secretSantaClient.GetGiftsForUserAsync(userId);
                }
                catch (SwaggerException se)
                {
                    ViewBag.ErrorMessage = se.Message;
                }
            }
            return View();
        }


        public async Task<IActionResult> Delete(int giftId, int userId)
        {
            IActionResult result = View();
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.DeleteGiftAsync(giftId);
                    var routeValues = new RouteValueDictionary(new { userId });
                    result = RedirectToAction(nameof(UserGifts), routeValues);
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int giftId, int userId)
        {
            GiftViewModel fetchedGift = null;

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    ViewBag.UserId = userId;
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    fetchedGift = await secretSantaClient.GetGiftAsync(giftId);
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }
            return View(fetchedGift);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GiftViewModel gift, int userId)
        {
            IActionResult result = View();
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {

                    var inputGift = new GiftInputViewModel
                    {
                        Title = gift.Title,
                        Description = gift.Description,
                        OrderOfImportance = gift.OrderOfImportance, // this is gross and I don't like it
                        Url = gift.Url,                             // would it be better to add an exception to the mapper
                        UserId = userId                             // that allows GiftViewModel to be mapped to GiftInputViewModel?
                    };
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.UpdateGiftAsync(gift.Id, inputGift);

                    var routeValues = new RouteValueDictionary(new { userId });
                    result = RedirectToAction(nameof(UserGifts), routeValues);
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }

            return result;
        }

    }
}