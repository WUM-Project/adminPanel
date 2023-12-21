using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
namespace Admin_Panel.Controllers
{
    public class MarksController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public MarksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search, int page = 1, int middleVal = 10,
            int cntBetween = 5, int limit = 10)
        {
            var result = await _serviceManager.MarkService.GetAllAsync();
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            if (!String.IsNullOrEmpty(search))
            {

                result = result?.Where(x => x.Title?.ToLower().Contains(search.ToLower()) ?? false)?.ToList();
            }
            var test = Paggination<Mark>.GetData(currentPage: page, limit: limit, itemsData: result,
               middleVal: middleVal, cntBetween: cntBetween);


            return View(Paggination<Mark>.GetData(currentPage: page, limit: limit, itemsData: result,
               middleVal: middleVal, cntBetween: cntBetween));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _serviceManager.MarkService.GetByIdAsync(id.Value, cancellationToken);

            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mark mark)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            Mark result = null;
            int? originMarkId = null;
            if (ModelState.IsValid)
            {
                foreach (var lang in languages)
                {
                    if (mark?.Id != null) mark.Id = 0;
                    mark.Lang = lang;
                    mark.OriginId = originMarkId ?? 0;
                    result = await _serviceManager.MarkService.Create(mark);
                    //for storage multiple data
                    if (originMarkId == null) originMarkId = result.Id;
                }
                result = mark;
                // await _serviceManager.MarkService.Create(mark);
                return RedirectToAction(nameof(Index));
            }
            return View(mark);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _serviceManager.MarkService.GetByIdAsync(id.Value);
            if (mark == null)
            {
                return NotFound();
            }
            return View(mark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mark mark)
        {
            if (id != mark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _serviceManager.MarkService.Update(mark);
                return RedirectToAction(nameof(Index));
            }
            return View(mark);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _serviceManager.MarkService.GetByIdAsync(id.Value);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.MarkService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}