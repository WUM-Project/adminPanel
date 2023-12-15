using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Admin_Panel.Data;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using AutoMapper;
namespace Admin_Panel.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CancellationToken cancellationToken = default
        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var result = await _context.Products.Include(d => d.UploadedFiles).ToListAsync(cancellationToken);
            var resultOut = _mapper.Map<IEnumerable<Product>>(result);

            return resultOut;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)

        {
            // var product = await _context.Products.Include(d => d.UploadedFiles).Include(y => y.ProductToUploadedFile).FirstOrDefaultAsync(e => e.Id == id);
            // if (product.ProductToUploadedFile != null)
            // {
            //     foreach (var mark in product.ProductToUploadedFile)
            //     {
            //         if (mark.UploadId != null)
            //         {

            //             // Замініть цей блок коду залежно від вашої логіки створення об'єкта mark за його ідентифікатором markId
            //             mark.UploadedFile = await _context.UploadedFile
            //                 .Where(m => m.Id == mark.UploadId)
            //                 .Select(m => new UploadedFiles
            //                 {
            //                     // Додайте поля, які вам потрібні
            //                     Id = m.Id,
            //                     FilePath = m.FilePath,
            //                     Name = m.Name,
            //                     // і так далі
            //                 })
            //                 .FirstOrDefaultAsync(cancellationToken);
            //         }
            //     }
            // }

            // return product;
            var product = await _context.Products
       .Include(d => d.UploadedFiles)
       .FirstOrDefaultAsync(e => e.Id == id);

            if (product != null)
            {


                await _context.Entry(product)
                    .Collection(p => p.Attributes)
                    .LoadAsync(cancellationToken);
                await _context.Entry(product)
                         .Collection(d => d.Categories)
                         .LoadAsync(cancellationToken);

                await _context.Entry(product)
                    .Collection(d => d.Marks)
                    .LoadAsync(cancellationToken);

                await _context.Entry(product)
                           .Collection(p => p.ProductToUploadedFile)
                           .LoadAsync(cancellationToken);


                //Витяжка аттрибутів
                if (product.Attributes != null)
                {
                    foreach (var item in product.Attributes)
                    {
                        if (item != null)
                        {
                            item.Product = null;

                            item.Attribute = await _context.Attributes
                                .Where(m => m.Id == item.AttributeId)
                                .Select(m => new Admin_Panel.Models.Attribute
                                {

                                    Id = m.Id,
                                    Title = m.Title,
                                    ShortTitle = m.ShortTitle,
                                    UnitOfMeasurement = m.UnitOfMeasurement,
                                    GroupAttr = m.GroupAttr,

                                })
                                .FirstOrDefaultAsync(cancellationToken);
                        }
                    }
                }
                if (product.Marks != null)
                {
                    foreach (var mark in product.Marks)
                    {
                        if (mark != null)
                        {
                            mark.Product = null;

                            mark.Mark = await _context.Marks
                                .Where(m => m.Id == mark.MarkId)
                                .Select(m => new Mark
                                {

                                    Id = m.Id,
                                    Title = m.Title,
                                    Color = m.Color,

                                })
                                .FirstOrDefaultAsync(cancellationToken);
                        }
                    }
                }
                if (product.Categories != null)
                {
                    foreach (var category in product.Categories)
                    {
                        if (category != null)
                        {


                            category.Category = await _context.Categories
                                .Where(m => m.Id == category.CategoryId)
                                .Select(m => new Category
                                {

                                    Id = m.Id,
                                    Title = m.Title,
                                    Lang = m.Lang,
                                    ParentId = m.ParentId,


                                })
                                .FirstOrDefaultAsync(cancellationToken);
                        }
                    }
                }
                // Add other related collections as needed
                if (product.ProductToUploadedFile != null)
                {
                    foreach (var file in product.ProductToUploadedFile)
                    {
                        if (file.UploadId != null)
                        {


                            file.UploadedFile = await _context.UploadedFile
                                .Where(m => m.Id == file.UploadId)
                                .Select(m => new UploadedFiles
                                {

                                    Id = m.Id,
                                    FilePath = m.FilePath,
                                    Name = m.Name,

                                })
                                .FirstOrDefaultAsync(cancellationToken);
                        }
                    }
                }

            }


            return product;
        }
        public async Task<Product> CreateTest(Product product)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var res = await _context.Products
              .Where(result => result.Lang == product.Lang)
             .OrderByDescending(result => result.Position)
        .Select(result => new { result.Id, result.Position }).FirstOrDefaultAsync();



                //  }
                product.Position = (res != null && res.Position.HasValue) ? res.Position.Value + 1 : 1;
                product.Status = 3;
                // Guid id = Guid.Empty;

                product.Categories.Clear();
                product.Marks.Clear();
                product.ProductToUploadedFile.Clear();

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();








                // _context.Add(product);
                // await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return product;
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }



        }

        public async Task<Product> Create(Product product, string Categories, string Marks, string Attributes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {


                List<string> languages = new List<string>() { "uk", "ru" };
                int indexLang = languages.FindIndex(el => el == product.Lang);
                indexLang = indexLang >= 0 ? indexLang : 0;
                Console.WriteLine(product);
                string[] selectedCategoryIds;
                string[] selectedMarkIds;
                string[] selectedAttributeIds;
                var res = await _context.Products
              .Where(result => result.Lang == product.Lang)
             .OrderByDescending(result => result.Position)
        .Select(result => new { result.Id, result.Position }).FirstOrDefaultAsync();
                // Console.WriteLine(res);
                //  if(product.OriginId > 0){

                // product.Id = (int)(product.OriginId +1);


                //  }
                product.Position = (res != null && res.Position.HasValue) ? res.Position.Value + 1 : 1;
                product.Status = 3;
                // Guid id = Guid.Empty;

                product.Categories.Clear();
                product.Marks.Clear();

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();


                //         if (!string.IsNullOrEmpty(Attributes))
                //     {
                //        selectedAttributeIds = Marks.Split(',');
                //        foreach (var markId in selectedAttributeIds)
                // {
                //     // Create a new ProductToAttribute entity and associate it with the product.

                //     var attribute = new ProductToAttribute()
                //     {

                //         ProductId = product.Id, 
                //         AttributeId = int.Parse(markId)
                //     };

                //  await _context.ProductToAttributes.AddAsync(attribute);

                // }
                //       await _context.SaveChangesAsync(); 
                //      }

                if (!string.IsNullOrEmpty(Marks))
                {
                    selectedMarkIds = Marks.Split(',');
                    foreach (var markId in selectedMarkIds)
                    {
                        // Create a new ProductToMarks entity and associate it with the product.
                        Console.WriteLine(product);
                        var mark = new ProductToMark()
                        {

                            ProductId = product.Id,
                            MarkId = int.Parse(markId)
                        };

                        await _context.ProductToMarks.AddAsync(mark);

                    }
                    await _context.SaveChangesAsync();
                }

                if (!string.IsNullOrEmpty(Categories))
                {
                    selectedCategoryIds = Categories.Split(',');
                    selectedCategoryIds = Categories.Split(',');
                    // await AddProductToCategoryAsync(product.Id, selectedCategoryIds);
                    foreach (var categoryId in selectedCategoryIds)
                    {
                        int catid;


                        if (indexLang < 1) catid = int.Parse(categoryId);
                        else catid = int.Parse(categoryId) + 1;
                        // Create a new ProductToCategory entity and associate it with the product.
                        var productToCategory = new ProductToCategory
                        {
                            ProductId = product.Id, // Set the ProductId

                            CategoryId = catid



                        };
                        // ProductToCategories
                        await _context.ProductToCategories.AddAsync(productToCategory);

                    }
                    await _context.SaveChangesAsync();


                }

                // _context.Add(product);
                // await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return product;
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProductToCategoryAsync(int productId)
        {
            // Delete existing ProductToCategory entities for the given productId
            var existingProductToCategories = _context.ProductToCategories.Where(ptc => ptc.ProductId == productId);
            _context.ProductToCategories.RemoveRange(existingProductToCategories);
            await _context.SaveChangesAsync();
        }
        public async Task AddProductToCategoryAsync(int productId, string selectedCategories, string Lang)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            int indexLang = languages.FindIndex(el => el == Lang);
            indexLang = indexLang >= 0 ? indexLang : 0;
            if (!string.IsNullOrEmpty(selectedCategories))
            {
                foreach (var categoryId in selectedCategories.Split(','))
                {

                    int catId = int.Parse(categoryId) + (indexLang == 1 ? 1 : 0);
                    // Create a new ProductToCategory entity and associate it with the product.
                    var productToCategory = new ProductToCategory
                    {
                        ProductId = productId, // Set the ProductId
                        CategoryId = catId
                    };
                    // ProductToCategories
                    _context.ProductToCategories.Add(productToCategory);

                }
            }


            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductToUploadedFileAsync(int productId)
        {
            // Delete existing ProductToCategory entities for the given productId
            var existingProductToCategories = _context.ProductToUploadedFiles.Where(ptc => ptc.ProductId == productId);
            _context.ProductToUploadedFiles.RemoveRange(existingProductToCategories);
            await _context.SaveChangesAsync();
        }
        public async Task AddProductToUploadedFileAsync(int productId, List<int> galleryImageIds, string Lang)
        {

            if (galleryImageIds.Count > 0)
            {
                foreach (var uploadId in galleryImageIds)
                {

                    var productToUploadedFile = new ProductToUploadedFiles
                    {
                        ProductId = productId, // Set the ProductId
                        UploadId = uploadId
                    };
                    // ProductToUploadedFiles
                    _context.ProductToUploadedFiles.Add(productToUploadedFile);

                }
            }


            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductToMarkAsync(int productId)
        {
            // Delete existing ProductToMark entities for the given productId
            var existingProductToMarks = _context.ProductToMarks.Where(ptc => ptc.ProductId == productId);
            _context.ProductToMarks.RemoveRange(existingProductToMarks);
            await _context.SaveChangesAsync();
        }
        public async Task AddProductToMarkAsync(int productId, string selectedMarks, string Lang)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            int indexLang = languages.FindIndex(el => el == Lang);
            indexLang = indexLang >= 0 ? indexLang : 0;
            if (!string.IsNullOrEmpty(selectedMarks))
            {
                foreach (var markId in selectedMarks.Split(','))
                {

                    int catId = int.Parse(markId) + (indexLang == 1 ? 1 : 0);
                    // Create a new ProductToMarks entity and associate it with the product.
                    var productToMark = new ProductToMark
                    {
                        ProductId = productId, // Set the ProductId
                        MarkId = catId
                    };
                    // ProductToMarks
                    _context.ProductToMarks.Add(productToMark);

                }
            }


            await _context.SaveChangesAsync();
        }
        public async Task Update(Product product, int originProdId,string NewImageIds,string SelectedMarks,string SelectedCategories,string SelectedAttributes)
        {

              List<string> languages = new List<string>() { "uk", "ru" };
               int indexLang = languages.FindIndex(el => el == product.Lang);
            indexLang = indexLang >= 0 ? indexLang : 0;
            var existingProduct = await _context.Products
              .FirstOrDefaultAsync(m => m.Id == product.Id);

               var allProductIds = await _context.Products
    .Where(p => p.Id == originProdId || p.OriginId == originProdId)
    .ToListAsync();

List<int> prodIdsToDelete = new List<int>();
if (allProductIds != null && allProductIds.Any())
{
    foreach (var el in allProductIds)
    {
       
            prodIdsToDelete.Add(el.Id); // Змінено з el.id.Value на el.Id.Value
        
    }
}
            if (existingProduct != null)
            {
                product.Lang = existingProduct.Lang;

                var originId = product.OriginId;
                if (originId.HasValue && originId != 0)
                {
                    var originproduct = await _context.Products
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originproduct != null)
                    {  
                  
                        originproduct.Sku = product.Sku;
                        originproduct.Quantity = product.Quantity;
                        originproduct.ImageId = product.ImageId;
                        originproduct.Price = product.Price;
                        originproduct.DiscountedPrice = product.DiscountedPrice;
                        originproduct.Status = product.Status;
                        _context.Update(originproduct);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originproduct = await _context.Products
                      .FirstOrDefaultAsync(m => m.OriginId == product.Id);

                    if (originproduct != null)
                    {
                  
                        originproduct.OriginId = product.Id;
                        originproduct.Sku = product.Sku;
                        originproduct.Quantity = product.Quantity;
                        originproduct.ImageId = product.ImageId;
                        originproduct.Price = product.Price;
                        originproduct.DiscountedPrice = product.DiscountedPrice;
                        originproduct.Status = product.Status;
                        _context.Update(originproduct);
                        await _context.SaveChangesAsync();
                    }
                }
 foreach (var item in allProductIds)
    {
          _context.ProductToUploadedFiles.RemoveRange(_context.ProductToUploadedFiles.Where(p => p.ProductId == item.Id));
           if (!string.IsNullOrEmpty(NewImageIds))

                        {

                            int[] newImageArray = NewImageIds.Split(',').Select(int.Parse).ToArray();
                            if (newImageArray.Length > 0)
                            {
        foreach (var galleryItem in newImageArray)
        {
            
                var productToUploadedFile = new ProductToUploadedFiles
                {
                    ProductId = item.Id,
                    UploadId =galleryItem
                };

                _context.ProductToUploadedFiles.Add(productToUploadedFile);
            }
         
        }
        }
    }

    //Доробити редагування щоб одночасно редагувало на всіх мовах 
    // foreach (var item in allProductIds)
    // {
          if (!string.IsNullOrEmpty(SelectedAttributes))
            {
                var existingProductToAttributes = _context.ProductToAttributes.Where(ptc => ptc.ProductId == product.Id);
            _context.ProductToAttributes.RemoveRange(existingProductToAttributes);
                foreach (var attributePair in SelectedAttributes.Split(','))
                {
                    var keyValue = attributePair.Split(':');
                    // var attributeId = int.Parse(keyValue[0]) + (indexLang == 1 ? 1 : 0);
                    var attributeId = int.Parse(keyValue[0]);
                    var attributeValue = keyValue[1];

                    // Create a new ProductToAttributes entity and associate it with the product.
                    var productToAttribute = new ProductToAttribute
                    {
                        ProductId = product.Id, 
                        AttributeId = attributeId,
                        Value = attributeValue 
                    };

                    // ProductToAttributes
                    _context.ProductToAttributes.Add(productToAttribute);
                }
            }else{
                  var existingProductToAttributes = _context.ProductToAttributes.Where(ptc => ptc.ProductId == product.Id);
            _context.ProductToAttributes.RemoveRange(existingProductToAttributes);
            }
            
      if (!string.IsNullOrEmpty(SelectedMarks))
            {  
                var existingProductToMarks = _context.ProductToMarks.Where(ptc => ptc.ProductId == product.Id);
            _context.ProductToMarks.RemoveRange(existingProductToMarks);
                foreach (var markId in SelectedMarks.Split(','))
                {

                    int catId = int.Parse(markId);
                    // Create a new ProductToMarks entity and associate it with the product.
                    var productToMark = new ProductToMark
                    {
                        ProductId = product.Id, // Set the ProductId
                        MarkId = catId
                    };
                    // ProductToMarks
                    _context.ProductToMarks.Add(productToMark);

                }
            }
      if (!string.IsNullOrEmpty(SelectedCategories))
            {  
          
                var existingProductToCategories = _context.ProductToCategories.Where(ptc => ptc.ProductId == product.Id);
            _context.ProductToCategories.RemoveRange(existingProductToCategories);
                foreach (var cat in SelectedCategories.Split(','))
                {
                          
                    int catId = int.Parse(cat);
                    // Create a new ProductToMarks entity and associate it with the product.
                    var productToCat = new ProductToCategory
                    {
                        ProductId = product.Id, // Set the ProductId
                        CategoryId = catId
                    };
                    // ProductToMarks
                    _context.ProductToCategories.Add(productToCat);

                }
            }
            // }


                existingProduct.UpdatedAt = DateTime.Now;

                // Update the properties of the existingProduct object
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
            }



            // product.UpdatedAt = DateTime.Now;
            // _context.Update(product);
            // await _context.SaveChangesAsync();
        }


        public async Task AddProductToAttributeAsync(int productId, string selectedAttributes, string Lang)
        {
            List<string> languages = new List<string>() { "uk", "ru" };
            int indexLang = languages.FindIndex(el => el == Lang);
            indexLang = indexLang >= 0 ? indexLang : 0;

            if (!string.IsNullOrEmpty(selectedAttributes))
            {
                foreach (var attributePair in selectedAttributes.Split(','))
                {
                    var keyValue = attributePair.Split(':');
                    var attributeId = int.Parse(keyValue[0]) + (indexLang == 1 ? 1 : 0);
                    var attributeValue = keyValue[1];

                    // Create a new ProductToMarks entity and associate it with the product.
                    var productToAttribute = new ProductToAttribute
                    {
                        ProductId = productId, // Set the ProductId
                        AttributeId = attributeId,
                        Value = attributeValue // Set the MarkValue
                    };

                    // ProductToMarks
                    _context.ProductToAttributes.Add(productToAttribute);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductToAttributeAsync(int productId)
        {
            // Delete existing ProductToMark entities for the given productId
            var existingProductToAttributes = _context.ProductToAttributes.Where(ptm => ptm.ProductId == productId);
            _context.ProductToAttributes.RemoveRange(existingProductToAttributes);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {



            var product = await _context.Products
               .FirstOrDefaultAsync(m => m.Id == id);

            if (product != null)
            {
                var originId = product.OriginId;

                if (originId.HasValue && originId != 0)
                {
                    var originproduct = await _context.Products.Include(p => p.UploadedFiles)
        .Include(p => p.ProductToUploadedFile)
        .Include(p => p.Marks)
        .Include(p => p.Attributes)
        .Include(p => p.Categories)
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originproduct != null)
                    {
                        // Видалення зв'язаних файлів
        if (originproduct.UploadedFiles != null)
        {
            _context.UploadedFile.Remove(product.UploadedFiles);
        }

        // Видалення зв'язаних файлів
        if (originproduct.ProductToUploadedFile != null && originproduct.ProductToUploadedFile.Any())
        {
            _context.ProductToUploadedFiles.RemoveRange(product.ProductToUploadedFile);
        }

        // Видалення зв'язаних міток
        if (originproduct.Marks != null && originproduct.Marks.Any())
        {
            _context.ProductToMarks.RemoveRange(product.Marks);
        }

        // Видалення зв'язаних атрибутів
        if (originproduct.Attributes != null && originproduct.Attributes.Any())
        {
            _context.ProductToAttributes.RemoveRange(originproduct.Attributes);
        }

        // Видалення зв'язаних категорій
        if (originproduct.Categories != null && originproduct.Categories.Any())
        {
            _context.ProductToCategories.RemoveRange(originproduct.Categories);
        }
                        _context.Products.Remove(originproduct);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originproduct = await _context.Products.Include(p => p.UploadedFiles)
        .Include(p => p.ProductToUploadedFile)
        .Include(p => p.Marks)
        .Include(p => p.Attributes)
        .Include(p => p.Categories)
                      .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (originproduct != null)
                    {
                         if (originproduct.UploadedFiles != null)
        {
            _context.UploadedFile.Remove(product.UploadedFiles);
        }

        // Видалення зв'язаних файлів
        if (originproduct.ProductToUploadedFile != null && originproduct.ProductToUploadedFile.Any())
        {
            _context.ProductToUploadedFiles.RemoveRange(product.ProductToUploadedFile);
        }

        // Видалення зв'язаних міток
        if (originproduct.Marks != null && originproduct.Marks.Any())
        {
            _context.ProductToMarks.RemoveRange(product.Marks);
        }

        // Видалення зв'язаних атрибутів
        if (originproduct.Attributes != null && originproduct.Attributes.Any())
        {
            _context.ProductToAttributes.RemoveRange(originproduct.Attributes);
        }

        // Видалення зв'язаних категорій
        if (originproduct.Categories != null && originproduct.Categories.Any())
        {
            _context.ProductToCategories.RemoveRange(originproduct.Categories);
        }
                        _context.Products.Remove(originproduct);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            }
        }
        public async Task UpdateProductToUploadedFileAsync(Product product, string OldImagesIds, string NewImageIds)
        {
            var existingProduct = await _context.Products
           .FirstOrDefaultAsync(m => m.Id == product.Id);

            if (!string.IsNullOrEmpty(NewImageIds) && !string.IsNullOrEmpty(OldImagesIds))

            {
                int[] oldImageArray = OldImagesIds.Split(',').Select(int.Parse).ToArray();
                int[] newImageArray = NewImageIds.Split(',').Select(int.Parse).ToArray();

                int[] removedNumbers = oldImageArray.Except(newImageArray).ToArray();

                if (removedNumbers.Length > 0)
                {



                    var originId = product.OriginId;
                    if (originId.HasValue && originId != 0)
                    {
                        var originproduct = await _context.Products
                            .FirstOrDefaultAsync(m => m.Id == originId.Value);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in removedNumbers)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }

                        }
                    }
                    else
                    {

                        var originproduct = await _context.Products
                          .FirstOrDefaultAsync(m => m.OriginId == product.Id);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in removedNumbers)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }
                        }
                    }

                }

            }
            if (!string.IsNullOrEmpty(NewImageIds) && string.IsNullOrEmpty(OldImagesIds))

            {

                int[] newImageArray = NewImageIds.Split(',').Select(int.Parse).ToArray();


                if (newImageArray.Length > 0)
                {



                    var originId = product.OriginId;
                    if (originId.HasValue && originId != 0)
                    {
                        var originproduct = await _context.Products
                            .FirstOrDefaultAsync(m => m.Id == originId.Value);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in newImageArray)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }

                        }
                    }
                    else
                    {

                        var originproduct = await _context.Products
                          .FirstOrDefaultAsync(m => m.OriginId == product.Id);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in newImageArray)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }
                        }
                    }

                }

            }




            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductToUploadedsFileAsync(Product product, string NewImageIds)
        {
            var existingProduct = await _context.Products
           .FirstOrDefaultAsync(m => m.Id == product.Id);

            if (!string.IsNullOrEmpty(NewImageIds))

            {

                int[] newImageArray = NewImageIds.Split(',').Select(int.Parse).ToArray();



                if (newImageArray.Length > 0)
                {



                    var originId = product.OriginId;
                    if (originId.HasValue && originId != 0)
                    {
                        var originproduct = await _context.Products
                            .FirstOrDefaultAsync(m => m.Id == originId.Value);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in newImageArray)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }

                        }
                    }
                    else
                    {

                        var originproduct = await _context.Products
                          .FirstOrDefaultAsync(m => m.OriginId == product.Id);

                        if (originproduct != null)
                        {
                            foreach (var uploadId in newImageArray)
                            {

                                var productToUploadedFile = new ProductToUploadedFiles
                                {
                                    ProductId = originproduct.Id, // Set the ProductId
                                    UploadId = uploadId
                                };
                                // ProductToUploadedFiles
                                _context.ProductToUploadedFiles.Add(productToUploadedFile);

                            }
                        }
                    }

                }

            }





            await _context.SaveChangesAsync();
        }
    }
}