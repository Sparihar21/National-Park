using Microsoft.AspNetCore.Mvc;
using WebAPIFrontEnd.Models;
using WebAPIFrontEnd.Repository.IRepository;

namespace WebAPIFrontEnd.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region Api's
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Json(new {data= await _nationalParkRepository.GetAllAsync(SD.NationalParkUrl)});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var nationparkindb = await _nationalParkRepository.DeleteAsync(SD.NationalParkUrl,id);
            if (nationparkindb)
                return Json(new { success = true, message = "Deleted Successfully" });
            return Json(new { success = false, message = "Something Went Wrong" });
        }
        #endregion

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark=await _nationalParkRepository.GetAsync(SD.NationalParkUrl,id.GetValueOrDefault());
            if (nationalPark == null) return NotFound();
            return View(nationalPark);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count() > 0)
                {
                    byte[] b1 = null;
                    using (var fs1 = file[0].OpenReadStream())
                    {
                        using (var ns1 = new MemoryStream())
                        {
                            fs1.CopyTo(ns1);
                            b1= ns1.ToArray();
                        }
                    }
                    nationalPark.Picture = b1;
                }
                else
                {
                    var nationalParkinDb = await _nationalParkRepository.GetAsync(SD.NationalParkUrl , nationalPark.Id);
                    nationalPark.Picture=nationalParkinDb.Picture;
                }
                if (nationalPark.Id == 0)
                    await _nationalParkRepository.CreateAsync(SD.NationalParkUrl, nationalPark);
                else
                    await _nationalParkRepository.UpdateAsync(SD.NationalParkUrl, nationalPark);

                return RedirectToAction(nameof(Index));
            }
            return View(nationalPark);
        }
    }
}
