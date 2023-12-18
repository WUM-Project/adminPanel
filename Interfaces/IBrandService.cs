using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface IBrandService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Brand> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        // Task<Category> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<Brand> Create(Brand brand);
        Task Update(Brand brand);
        Task Delete(int id);
    }
}