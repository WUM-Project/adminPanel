using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
namespace Admin_Panel.Controllers
{
   

    public class CategoriesController : Controller
    {
       private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search,int page=1,int middleVal = 10, 
            int cntBetween = 5, int limit = 10)
        {  
          
            var result = await _serviceManager.CategoryService.GetAllAsync();
         if (!String.IsNullOrEmpty(lang))
            {
           
              result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
         if (!String.IsNullOrEmpty(search))
            {
           
              result = result?.Where(x => x.Title?.ToLower().Contains(search.ToLower()) ?? false)?.ToList();
            }
          
           
            return View( Paggination<Category>.GetData(currentPage: page, limit: limit, itemsData: result, 
                middleVal: middleVal, cntBetween: cntBetween));
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
           
          List<string> languages = new List<string>() { "uk", "ru" };
            Category result = null;
            
            int? parent_id = null; // Assuming parent_id is needed
            
            int? originCategoryId = null;
          
       
            if (ModelState.IsValid)
            { 
            foreach (var lang in languages)
            {    
               if(category?.Id != null){
                category.Id = 0;
            } 
                category.Lang = lang;
                category.OriginId = originCategoryId ?? 0;
                category.ParentId = category.ParentId ?? 0;
                category.ImageId = category.ImageId ?? 0;
                 if(parent_id.HasValue && category.ParentId < parent_id){
                    category.ParentId = parent_id;
                 }
                 
            result = await _serviceManager.CategoryService.Create(category);
                //for storage multiple data
                if (originCategoryId == null){ 
               
                    originCategoryId = result.Id; 
                    parent_id = result.ParentId++;
                } 
                
                if (parent_id.HasValue)  parent_id++;
            }

            result = category; 
               
                // await _serviceManager.CategoryService.Create(category);
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