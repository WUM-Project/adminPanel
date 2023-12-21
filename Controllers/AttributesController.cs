using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
namespace Admin_Panel.Controllers
{
    public class AttributesController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public AttributesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search, int page = 1, int middleVal = 10,
            int cntBetween = 5, int limit = 10)
        {

            var result = await _serviceManager.AttributeServices.GetAllAsync();
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            if (!String.IsNullOrEmpty(search))
            {

                result = result?.Where(x => x.Title?.ToLower().Contains(search.ToLower()) ?? false)?.ToList();
            }
            var test = Paggination<Models.Attribute>.GetData(currentPage: page, limit: limit, itemsData: result,
                middleVal: middleVal, cntBetween: cntBetween);

            return View(Paggination<Models.Attribute>.GetData(currentPage: page, limit: limit, itemsData: result,
                middleVal: middleVal, cntBetween: cntBetween));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value, cancellationToken);

            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Attribute attribute)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            Models.Attribute result = null;
            int? originAttributeId = null;

            if (ModelState.IsValid)
            {

                foreach (var lang in languages)
                {
                    if (attribute?.Id != null)
                    {
                        attribute.Id = 0;
                    }
                    attribute.Lang = lang;
                    attribute.OriginId = originAttributeId ?? 0;
                    result = await _serviceManager.AttributeServices.Create(attribute);
                    //for storage multiple data
                    if (originAttributeId == null) originAttributeId = result.Id;

                }

                result = attribute;
                // await _serviceManager.AttributeServices.Create(attribute);
                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value);
            if (attribute == null)
            {
                return NotFound();
            }
            return View(attribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Attribute attribute)
        {
            if (id != attribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _serviceManager.AttributeServices.Update(attribute);
                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.AttributeServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}