using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
namespace Admin_Panel.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search,int page=1,int middleVal = 10, 
            int cntBetween = 5, int limit = 10)
        {
               var result = await _serviceManager.ProductService.GetAllAsync();
   if (!String.IsNullOrEmpty(lang))
            {
           
              result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
         if (!String.IsNullOrEmpty(search))
            {
           
              result = result?.Where(x => x.Name?.ToLower().Contains(search.ToLower()) ?? false)?.ToList();
            }
             var test = Paggination<Product>.GetData(currentPage: page, limit: limit, itemsData: result, 
                middleVal: middleVal, cntBetween: cntBetween);
               

             return View( Paggination<Product>.GetData(currentPage: page, limit: limit, itemsData: result, 
                middleVal: middleVal, cntBetween: cntBetween));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _serviceManager.ProductService.GetByIdAsync(id.Value, cancellationToken);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        public  IActionResult Create()
        {  
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product Product)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            Product result = null;
            int? originProductId = null;
            if (ModelState.IsValid)
            {
                foreach (var lang in languages)
                {
                    if (Product?.Id != null) Product.Id = 0;
                    Product.Lang = lang;
                    Product.OriginId = originProductId ?? 0;
                    result = await _serviceManager.ProductService.Create(Product);
                    //for storage multiple data
                    if (originProductId == null) originProductId = result.Id;
                }
                result = Product;
                // await _serviceManager.ProductService.Create(Product);
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _serviceManager.ProductService.GetByIdAsync(id.Value);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product Product)
        {
            if (id != Product.Id)
            {
                return NotFound();
            }
     
            if (ModelState.IsValid)
            {
                await _serviceManager.ProductService.Update(Product);
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _serviceManager.ProductService.GetByIdAsync(id.Value);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.ProductService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}