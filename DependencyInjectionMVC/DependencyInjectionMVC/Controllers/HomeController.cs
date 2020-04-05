using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DependencyInjectionMVC.Models;
using Crypto;
using DependencyInjectionMVC.Interface;

namespace DependencyInjectionMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepositoryService postRepositoryService;
        public HomeController(
            ILogger<HomeController> logger,
            IPostRepositoryService postRepositoryService)
        {
            _logger = logger;
            this.postRepositoryService = postRepositoryService;
        }

        [Route("Post")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Post model)
        {
            model.PostDateTime = new DateTime(DateTime.Now.Date.Ticks, DateTimeKind.Utc);
            await this.postRepositoryService.Create(model);
            return View("Privacy");
        }

        [Route("PostList")]
        public IActionResult Privacy()
        {
           
            return View(this.postRepositoryService.GetAll());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
