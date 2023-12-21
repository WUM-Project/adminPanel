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


    public class CategoriesController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(string lang, string search, int page = 1, int middleVal = 10,
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


            return View(Paggination<Category>.GetData(currentPage: page, limit: limit, itemsData: result,
                middleVal: middleVal, cntBetween: cntBetween));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _serviceManager.CategoryService.GetByIdAsync(id.Value, cancellationToken);

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
        public async Task<IActionResult> Create(Category category, IFormFile Photo, IFormFile PhotoIcon)
        {

            List<string> languages = new List<string>() { "uk", "ru" };
            Category result = null;
            UploadedFiles downloadedimage = null;
            UploadedFiles downloadedIcon = null;
            int? parent_id = null; // Assuming parent_id is needed
            int? imageId = null;
            int? iconId = null;
            int? originCategoryId = null;

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
                        var response = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=categories", formData);

                        if (response.IsSuccessStatusCode)

                        {

                            Console.WriteLine("File sent successfully.");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            downloadedimage = JsonConvert.DeserializeObject<UploadedFiles>(responseContent);
                            imageId = downloadedimage.Id;
                            Console.WriteLine(responseContent);
                        }
                        else
                        {
                            Console.WriteLine("Error sending file: " + response.ReasonPhrase);
                        }
                    }
                }
                if (PhotoIcon != null)
                {
                    // Create a new MultipartFormDataContent
                    var formData = new MultipartFormDataContent();

                    // Open the file you want to send
                    using (var fileStream = PhotoIcon.OpenReadStream())
                    {
                        // Create a StreamContent for the file
                        var fileContent = new StreamContent(fileStream);

                        // Add the StreamContent to the MultipartFormDataContent
                        formData.Add(fileContent, "file", PhotoIcon.FileName);

                        // You can also add other form data fields if needed
                        formData.Add(new StringContent("Some additional data"), "field1");

                        // Send the HTTP POST request with the MultipartFormDataContent
                        var response = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=categoriesIcon", formData);

                        if (response.IsSuccessStatusCode)

                        {

                            Console.WriteLine("File sent successfully.");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            downloadedIcon = JsonConvert.DeserializeObject<UploadedFiles>(responseContent);
                            iconId = downloadedIcon.Id;
                            Console.WriteLine(responseContent);
                        }
                        else
                        {
                            Console.WriteLine("Error sending file: " + response.ReasonPhrase);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var lang in languages)
                {
                    if (category?.Id != null)
                    {
                        category.Id = 0;
                    }
                    if (imageId != null)
                    {
                        category.ImageId = imageId;
                    }
                    if (iconId != null)
                    {
                        category.IconId = iconId;
                    }
                    category.Lang = lang;
                    category.OriginId = originCategoryId ?? 0;
                    category.ParentId = category.ParentId ?? 0;
                    category.ImageId = downloadedimage.Id;
                    // category.ImageId = downloadedimage.Id;
                    if (parent_id.HasValue && category.ParentId < parent_id)
                    {
                        category.ParentId = parent_id;
                    }



                    result = await _serviceManager.CategoryService.Create(category);
                    //for storage multiple data
                    if (originCategoryId == null)
                    {

                        originCategoryId = result.Id;
                        parent_id = result.ParentId++;
                    }

                    if (parent_id.HasValue) parent_id++;
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

            // Convert the Category model to CategoryEditViewModels
            var categoryViewModel = new CategoryEditViewModels
            {
                Id = category.Id,
                OriginId = category.OriginId,
                Lang = category.Lang,
                ParentId = category.ParentId,
                Title = category.Title,
                Status = category.Status,
                ImageId = category.ImageId,
                UploadedFiles = category.UploadedFiles,
                IconId = category.IconId,
                UploadedFileIcon = category.UploadedFileIcon,


                // Photo = category.UploadedFiles.FilePath
                // Map other properties as needed
            };
            if (category == null)
            {
                return NotFound();
            }
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category Category, IFormFile Photo, IFormFile PhotoIcon)
        {
            UploadedFiles downloadedimage = null;
            int? imageId = null;
            if (id != Category.Id)
            {
                return NotFound();
            }

            if (PhotoIcon != null)
            {
                using (var client = new HttpClient())
                {
                    if (Category.IconId != null)
                    {
                        await client.DeleteAsync("https://localhost:7144/api/Uploads/" + Category.IconId);

                    }
                    // Create a new MultipartFormDataContent
                    var formData = new MultipartFormDataContent();

                    // Open the file you want to send
                    using (var fileStream = PhotoIcon.OpenReadStream())
                    {
                        // Create a StreamContent for the file
                        var fileContent = new StreamContent(fileStream);

                        // Add the StreamContent to the MultipartFormDataContent
                        formData.Add(fileContent, "file", PhotoIcon.FileName);

                        // You can also add other form data fields if needed
                        formData.Add(new StringContent("Some additional data"), "field1");

                        // Send the HTTP POST request with the MultipartFormDataContent
                        var response = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=categories", formData);

                        if (response.IsSuccessStatusCode)

                        {

                            Console.WriteLine("File sent successfully.");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            downloadedimage = JsonConvert.DeserializeObject<UploadedFiles>(responseContent);
                            Category.IconId = downloadedimage.Id;
                            Console.WriteLine(responseContent);
                        }
                        else
                        {
                            Console.WriteLine("Error sending file: " + response.ReasonPhrase);
                        }
                    }
                }

            }
            if (Photo != null)
            {
                using (var client = new HttpClient())
                {
                    if (Category.ImageId != null)
                    {
                        await client.DeleteAsync("https://localhost:7144/api/Uploads/" + Category.ImageId);

                    }
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
                        var response = await client.PostAsync("https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=categories", formData);

                        if (response.IsSuccessStatusCode)

                        {

                            Console.WriteLine("File sent successfully.");
                            var responseContent = await response.Content.ReadAsStringAsync();
                            downloadedimage = JsonConvert.DeserializeObject<UploadedFiles>(responseContent);
                            Category.ImageId = downloadedimage.Id;
                            Console.WriteLine(responseContent);
                        }
                        else
                        {
                            Console.WriteLine("Error sending file: " + response.ReasonPhrase);
                        }
                    }
                }

            }

            if (ModelState.IsValid)
            {
                // var origin_id = Category.OriginId ?? Category.Id;
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