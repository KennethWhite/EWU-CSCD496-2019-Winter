using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Add(int id)
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Gifts = await secretSantaClient.GetGiftsForUserAsync(id);
                    ViewBag.UserId = id;
                    var result = RedirectToAction(nameof(Index));
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

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }

            return result;
        }


    }
}