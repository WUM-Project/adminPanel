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
        Task Update(Product product);
        Task Delete(int id);
    }
}