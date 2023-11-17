using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface ICategoryService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Category> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        // Task<Category> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<Category> Create(Category attribute);
        Task Update(Category attribute);
        Task Delete(int id);
    }
}