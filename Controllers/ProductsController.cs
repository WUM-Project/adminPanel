using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async  Task<IActionResult> Create()
        {   string lang ="uk";
              var result = await _serviceManager.CategoryService.GetAllAsync();
              var marks = await _serviceManager.MarkService.GetAllAsync();
              var attributes = await _serviceManager.AttributeServices.GetAllAsync();
         if (!String.IsNullOrEmpty(lang))
            {
           
              result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
              marks = marks?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
              attributes = attributes?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            var groupedCategories = result
    .GroupBy(category => category.ParentId)
    .ToList();

            Console.WriteLine(groupedCategories);

              List<SelectListItem> mylist = new List<SelectListItem>();
            foreach (var price in result)
            {
                mylist.Add(new SelectListItem { Text = price.Title, Value = price.Id.ToString() });

            }
              List<SelectListItem> markslist = new List<SelectListItem>();
            foreach (var item in marks)
            {
                markslist.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

            }
              List<SelectListItem> attributesList = new List<SelectListItem>();
            foreach (var item in attributes)
            {
                attributesList.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

            }
        
            ViewBag.Categories = groupedCategories;
            // ViewBag.Categories = mylist;
            ViewBag.Marks = markslist;
          
            ViewBag.Attributes = attributesList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product Product,  string SelectedMarks,string SelectedCategories,string Attributes)
        {     
            List<string> languages = new List<string>() { "uk", "ru" };
            Product result = null;
            // string[] selectedCategoryIds;
            // string[] selectedMarkIds;
            // Console.WriteLine(Product);
            // Console.WriteLine(Categories);
            // Console.WriteLine(Marks);
            // Console.WriteLine(SelectedMarks);
            // Console.WriteLine(SelectedCategories);
            int? originProductId = null;
            //get SelectedMarks
        //      if (!string.IsNullOrEmpty(SelectedMarks))
        //     {
        //        selectedMarkIds = SelectedMarks.Split(',');
        //      }
        //      if (!string.IsNullOrEmpty(SelectedCategories))
        //     {
        //  selectedCategoryIds = SelectedCategories.Split(',');
        //      }
            if (ModelState.IsValid)
            {
                foreach (var lang in languages)
                {
                    if (Product?.Id != null) Product.Id = 0;
                    Product.Lang = lang;
                    Product.OriginId = originProductId ?? 0;
                    result = await _serviceManager.ProductService.Create(Product, SelectedCategories, SelectedMarks,Attributes);
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