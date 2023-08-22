using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
namespace Admin_Panel.Controllers
{
    public class CategoriesController : Controller
    {
       private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {  
          
        
        
            
            return View(await _serviceManager.CategoryService.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id,CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var Category = await _serviceManager.CategoryService.GetByIdAsync(id.Value,cancellationToken);
         
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {      
           
           
           
          
            if (ModelState.IsValid)
            {
                await _serviceManager.CategoryService.Create(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _serviceManager.CategoryService.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category Category)
        {
            if (id != Category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _serviceManager.CategoryService.Update(Category);
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _serviceManager.CategoryService.GetByIdAsync(id.Value);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.CategoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}