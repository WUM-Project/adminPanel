using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Newtonsoft.Json;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using Admin_Panel.Pagginations;
using Microsoft.AspNetCore.Mvc.Rendering;

using Admin_Panel.ViewModels;
namespace Admin_Panel.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search, int page = 1, int middleVal = 10,
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


            return View(Paggination<Product>.GetData(currentPage: page, limit: limit, itemsData: result,
               middleVal: middleVal, cntBetween: cntBetween));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _serviceManager.ProductService.GetByIdAsync(id.Value, cancellationToken);
            Console.WriteLine(Product);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        public async Task<IActionResult> Create()
        {
            string lang = "uk";
            var result = await _serviceManager.CategoryService.GetAllAsync();
            var marks = await _serviceManager.MarkService.GetAllAsync();
            var attributes = await _serviceManager.AttributeServices.GetAllAsync();
            var brands = await _serviceManager.BrandService.GetAllAsync();
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                marks = marks?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                attributes = attributes?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                brands = brands?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            var groupedCategories = result
    .GroupBy(category => category.ParentId)
    .ToList();



            List<SelectListItem> mylist = new List<SelectListItem>();
            foreach (var price in result)
            {
                mylist.Add(new SelectListItem { Text = price.Title, Value = price.Id.ToString() });

            }
            List<SelectListItem> brandslist = new List<SelectListItem>();
            foreach (var item in brands)
            {
                brandslist.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

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
            ViewBag.Brands = brandslist;

            ViewBag.Attributes = attributesList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product Product, string SelectedMarks, string SelectedCategories, string SelectedAttributes, string UploadedImageIds)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            Product result = null;

            int? originProductId = null;

            string originLang = null;


            if (ModelState.IsValid)
            {


                foreach (var lang in languages)
                {
                    int indexLang = languages.FindIndex(el => el == lang);
                    indexLang = indexLang >= 0 ? indexLang : 0;
                    if (Product?.Id != null) Product.Id = 0;
                    Product.Lang = lang;
                    Product.BrandId = Product.BrandId + (indexLang == 1 ? 1 : 0);
                    // if (imageId != null) Product.ImageId = imageId;

                    Product.OriginId = originProductId ?? 0;
                    result = await _serviceManager.ProductService.CreateTest(Product);

                    if (originProductId == null)
                    {
                        originProductId = result.Id;
                        originLang = result.Lang;
                    }
                    //Add Gallery to product
                    await _serviceManager.ProductService.AddProductToUploadedFileAsync(result.Id, UploadedImageIds, result.Lang);
                    //Add Category to product
                    await _serviceManager.ProductService.AddProductToCategoryAsync(result.Id, SelectedCategories, result.Lang);


                    //Add Attributes to product
                    await _serviceManager.ProductService.AddProductToAttributeAsync(result.Id, SelectedAttributes, result.Lang);

                    // Delete existing ProductToMarks entities for the old product
                    await _serviceManager.ProductService.DeleteProductToAttributeAsync(originProductId.Value);
                    //Add Marks to product
                    await _serviceManager.ProductService.AddProductToMarkAsync(result.Id, SelectedMarks, result.Lang);

                    // Delete existing ProductToMarks entities for the old product
                    await _serviceManager.ProductService.DeleteProductToMarkAsync(originProductId.Value);

                    // Delete existing ProductToCategory entities for the old product
                    await _serviceManager.ProductService.DeleteProductToCategoryAsync(originProductId.Value);

                    // Delete existing ProductToUploadedFiles entities for the old product
                    await _serviceManager.ProductService.DeleteProductToUploadedFileAsync(originProductId.Value);
                }
                result = Product;
                //Add ProductToUploadedFiles origin
                await _serviceManager.ProductService.AddProductToUploadedFileAsync(originProductId.Value, UploadedImageIds, originLang);
                //Add ProductToCategory origin
                await _serviceManager.ProductService.AddProductToCategoryAsync(originProductId.Value, SelectedCategories, originLang);

                //Add ProductToMarks origin
                await _serviceManager.ProductService.AddProductToMarkAsync(originProductId.Value, SelectedMarks, originLang);

                await _serviceManager.ProductService.AddProductToAttributeAsync(originProductId.Value, SelectedAttributes, originLang);


                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            string lang = "uk";
            var result = await _serviceManager.CategoryService.GetAllAsync();
            var marks = await _serviceManager.MarkService.GetAllAsync();
            var attributes = await _serviceManager.AttributeServices.GetAllAsync();
            var brands = await _serviceManager.BrandService.GetAllAsync();
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                marks = marks?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                attributes = attributes?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                brands = brands?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            var groupedCategories = result
    .GroupBy(category => category.ParentId)
    .ToList();

            if (id == null)
            {
                return NotFound();
            }
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
            List<SelectListItem> brandslist = new List<SelectListItem>();
            foreach (var item in brands)
            {
                brandslist.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

            }
            List<SelectListItem> attributesList = new List<SelectListItem>();
            foreach (var item in attributes)
            {
                attributesList.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });

            }
            var Product = await _serviceManager.ProductService.GetByIdAsync(id.Value);
            var ProductViewModel = new ProductEditViewModel
            {
                Id = Product.Id,
                OriginId = Product.OriginId,
                Lang = Product.Lang,
                Status = Product.Status,
                Description = Product.Description,
                ShortDescription = Product.ShortDescription,
                Sku = Product.Sku,
                Price = Product.Price,
                DiscountedPrice = Product.DiscountedPrice,
                Quantity = Product.Quantity,
                Name = Product.Name,
                ShortName = Product.ShortName,
                Position = Product.Position,
                Availability = Product.Availability,
                ImageId = Product.ImageId,
                UploadedFiles = Product.UploadedFiles,
                Brands = Product.Brands,
                ProductGallery = Product.ProductToUploadedFile.Select(m => new GalleryViewModel
                {
                    FilePath = m.UploadedFile.FilePath,
                    ImageId = m.UploadId,

                    // Map other properties if needed
                }).ToList(),
                // ProductToUploadedFile = Product.ProductToUploadedFile,
                // SelectedMarks = Product.Marks,
                // SelectedAttributes = Product.Attributes,
                // SelectedCategories = Product.Categories
                SelectedMarks = Product.Marks.Select(m => new MarkViewModel
                {
                    MarkId = m.Mark.Id,
                    Title = m.Mark.Title,
                    // Map other properties if needed
                }).ToList(),

                SelectedAttributes = Product.Attributes.Select(a => new AttributeViewModel
                {
                    AttributeId = a.Attribute.Id,
                    Value = a.Value,
                    Title = a.Attribute.Title
                    // Map other properties if needed
                }).ToList(),

                SelectedCategories = Product.Categories.Select(c => new CategoryViewModel
                {
                    CategoryId = c.Category.Id,
                    Title = c.Category.Title,
                    // Map other properties if needed
                }).ToList()

            };
            if (Product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = groupedCategories;

            // ViewBag.Categories = mylist;
            ViewBag.Marks = markslist;
            ViewBag.Brands = brandslist;

            ViewBag.Attributes = attributesList;
            // Console.WriteLine(ViewBag.Categories);
            return View(ProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product Product, string UploadedImageIds, string SelectedMarks, string SelectedCategories, string SelectedAttributes)
        {
            if (id != Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                int originProdId = Product.OriginId.GetValueOrDefault() == 0 ? Product.Id : Product.OriginId.GetValueOrDefault();


                await _serviceManager.ProductService.Update(Product, originProdId, UploadedImageIds, SelectedMarks, SelectedCategories, SelectedAttributes);

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