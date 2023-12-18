using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface IProductService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<Product> Create(Product product, string  Categories,string  Marks,string Attribute);
        Task<Product> CreateTest(Product product);
        Task Update(Product product,int originProdId, string NewImageIds,string SelectedMarks,string SelectedCategories,string SelectedAttributes);
        Task Delete(int id);    
        Task AddProductToUploadedFileAsync(int productId,  string NewImageIds ,string Lang);
              Task DeleteProductToUploadedFileAsync(int productId);
        Task AddProductToCategoryAsync(int productId, string selectedCategories,string Lang);
    
        Task DeleteProductToCategoryAsync(int productId);
        Task AddProductToMarkAsync(int productId, string SelectedMarks,string Lang);
        Task DeleteProductToMarkAsync(int productId);
        Task AddProductToAttributeAsync(int productId,string selectedAttributes, string Lang);
        Task DeleteProductToAttributeAsync(int productId);
      
        // Task UpdateProductToCategoryAsync(int newProductId, int oldProductId, string selectedCategories);
    }
}