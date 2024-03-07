using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAPIFrontEnd.Models;
using WebAPIFrontEnd.Models.View_Model;
using WebAPIFrontEnd.Repository.IRepository;

namespace WebAPIFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepository,INationalParkRepository nationalParkRepository)
        {
            _logger = logger;
            _trailRepository = trailRepository;
            _nationalParkRepository= nationalParkRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new IndexVM()
            {
                nationalParkList= await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl),
                trailList = await _trailRepository.GetAllAsync(SD.TrailUrl)
            };
            return View(indexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}