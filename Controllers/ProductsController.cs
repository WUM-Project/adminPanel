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
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                marks = marks?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                attributes = attributes?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
            }
            var groupedCategories = result
    .GroupBy(category => category.ParentId)
    .ToList();



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
        public async Task<IActionResult> Create(Product Product, string SelectedMarks, string SelectedCategories, string SelectedAttributes, IFormFile Photo, List<IFormFile> Gallery)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            Product result = null;
            UploadedFiles downloadedimage = null;
            int? originProductId = null;
            int? imageId = null;
            string originLang = null;
            List<UploadedFiles> uploadedFilesList = new List<UploadedFiles>();
            List<int> galleryImageIds = new List<int>();
            using (var client = new HttpClient())
            {
                if (Photo != null)
                {
                    // Create a new MultipartFormDataContent
                    var formData = new MultipartFormDataContent();

                    // Open the file you want to send
                    using (var fileStream = Photo.OpenReadStream())
                    {
                        // Create a StreamContent for the file
                        var fileContent = new StreamContent(fileStream);

                        // Add the StreamContent to the MultipartFormDataContent
                        formData.Add(fileContent, "file", Photo.FileName);

                        // You can also add other form data fields if needed
                        formData.Add(new StringContent("Some additional data"), "field1");

                        // Send the HTTP POST request with the MultipartFormDataContent
                        var response = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=productviews", formData);

                        if (response.IsSuccessStatusCode)

                        {

                            Console.WriteLine("File sent successfully.");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            downloadedimage = JsonConvert.DeserializeObject<UploadedFiles>(responseContent);
                            imageId = downloadedimage.Id;

                        }
                        else
                        {
                            Console.WriteLine("Error sending file: " + response.ReasonPhrase);
                        }
                    }
                }

                // Handle the gallery images
                if (Gallery != null && Gallery.Count > 0)
                {
                    foreach (var galleryImage in Gallery)
                    {
                        var galleryFormData = new MultipartFormDataContent();
                        using (var galleryFileStream = galleryImage.OpenReadStream())
                        {
                            var galleryFileContent = new StreamContent(galleryFileStream);
                            galleryFormData.Add(galleryFileContent, "file", galleryImage.FileName);
                            galleryFormData.Add(new StringContent("Some additional data"), "field1");

                            var galleryResponse = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=productGallery", galleryFormData);

                            if (galleryResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Gallery File sent successfully.");
                                var galleryResponseContent = await galleryResponse.Content.ReadAsStringAsync();
                                var galleryDownloadedimage = JsonConvert.DeserializeObject<UploadedFiles>(galleryResponseContent);
                                uploadedFilesList.Add(galleryDownloadedimage);
                                galleryImageIds.Add(galleryDownloadedimage.Id);
                                // Process the gallery image response as needed

                            }
                            else
                            {
                                Console.WriteLine("Error sending gallery file: " + galleryResponse.ReasonPhrase);
                            }
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {


                foreach (var lang in languages)
                {
                    if (Product?.Id != null) Product.Id = 0;
                    Product.Lang = lang;
                    if (imageId != null) Product.ImageId = imageId;

                    Product.OriginId = originProductId ?? 0;
                    result = await _serviceManager.ProductService.CreateTest(Product);

                    if (originProductId == null)
                    {
                        originProductId = result.Id;
                        originLang = result.Lang;
                    }
                    //Add Gallery to product
                    await _serviceManager.ProductService.AddProductToUploadedFileAsync(result.Id, galleryImageIds, result.Lang);
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
                await _serviceManager.ProductService.AddProductToUploadedFileAsync(originProductId.Value, galleryImageIds, originLang);
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
            if (!String.IsNullOrEmpty(lang))
            {

                result = result?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                marks = marks?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
                attributes = attributes?.Where(x => x.Lang?.Contains(lang.ToLower()) ?? false)?.ToList();
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
                ProductToUploadedFile = Product.ProductToUploadedFile,
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
               Console.WriteLine(markslist);
            // ViewBag.Categories = mylist;
            ViewBag.Marks = markslist;

            ViewBag.Attributes = attributesList;
            Console.WriteLine(ViewBag.Categories);
            return View(ProductViewModel);
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