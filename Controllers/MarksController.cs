using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
namespace Admin_Panel.Controllers
{
    public class MarksController : Controller
    {
       private readonly IServiceManager _serviceManager;

        public MarksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {  
          
        
        
            
            return View(await _serviceManager.MarkService.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id,CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var mark = await _serviceManager.MarkService.GetByIdAsync(id.Value,cancellationToken);
         
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
            if (ModelState.IsValid)
            {
                await _serviceManager.MarkService.Create(mark);
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