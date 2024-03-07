using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WebAPIFrontEnd.Models;
using WebAPIFrontEnd.Models.View_Model;
using WebAPIFrontEnd.Repository.IRepository;

namespace WebAPIFrontEnd.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View();

        }
        #region API's
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data =await _trailRepository.GetAllAsync(SD.TrailUrl) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var nationparkindb = await _trailRepository.DeleteAsync(SD.TrailUrl, id);
            if (nationparkindb)
                return Json(new { success = true, message = "Deleted Successfully" });
            return Json(new { success = false, message = "Something Went Wrong" });
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkInDb = await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl);
            TrailVm trailVm = new TrailVm()
            {
                Trails = new Trails(),
                nationalParkList = nationalParkInDb.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                })
            };
            if (id == null) return View(trailVm);
            trailVm.Trails = await _trailRepository.GetAsync(SD.TrailUrl, id.GetValueOrDefault());
            if (trailVm.Trails == null) return NotFound();
            return View(trailVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVm trailVm)
        {
            if (ModelState.IsValid)
            {
                if (trailVm.Trails.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailUrl, trailVm.Trails);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailUrl, trailVm.Trails);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var nationalParkInDb = await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl);
                 trailVm = new TrailVm()
                {
                    Trails = new Trails(),
                    nationalParkList = nationalParkInDb.Select(np => new SelectListItem()
                    {
                        Text = np.Name,
                        Value = np.Id.ToString()
                    })
                };
                return View(trailVm); 
            }
        }
    }

}