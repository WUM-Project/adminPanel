using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using System.Net.Http;
using Newtonsoft.Json;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.ViewModels;
using Admin_Panel.Pagginations;
using System.Net.Http.Headers;
using System.IO;
namespace Admin_Panel.Controllers
{


    public class BrandsController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public BrandsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search, int page = 1, int middleVal = 10,
            int cntBetween = 5, int limit = 10)
        {

            var result = await _serviceManager.BrandService.GetAllAsync();
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            if (!String.IsNullOrEmpty(search))
            {

                result = result?.Where(x => x.Title?.ToLower().Contains(search.ToLower()) ?? false)?.ToList();
            }


            return View(Paggination<Brand>.GetData(currentPage: page, limit: limit, itemsData: result,
                middleVal: middleVal, cntBetween: cntBetween));
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {

            List<string> languages = new List<string>() { "uk", "ru" };
            Brand result = null;


            int? originBrandId = null;



            if (ModelState.IsValid)
            {
                foreach (var lang in languages)
                {
                    if (brand?.Id != null)
                    {
                        brand.Id = 0;
                    }


                    brand.Lang = lang;
                    brand.OriginId = originBrandId ?? 0;





                    result = await _serviceManager.BrandService.Create(brand);
                    if (originBrandId == null)
                    {

                        originBrandId = result.Id;

                    }

                }

                result = brand;


                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _serviceManager.BrandService.GetByIdAsync(id.Value);


            var brandViewModel = new BrandCreateViewModels
            {
                Id = brand.Id,
                OriginId = brand.OriginId,
                Lang = brand.Lang,

                Title = brand.Title,
                Status = brand.Status,
                ImageId = brand.ImageId,
                UploadedFiles = brand.UploadedFiles,




            };
            if (brand == null)
            {
                return NotFound();
            }
            return View(brandViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand brand, IFormFile Photo, IFormFile PhotoIcon)
        {

            if (id != brand.Id)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {

                await _serviceManager.BrandService.Update(brand);
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _serviceManager.BrandService.GetByIdAsync(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.BrandService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}