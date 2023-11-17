using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface IRoleService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Role> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<Role> Create(Role attribute);
        Task Update(Role attribute);
        Task Delete(int id);
    }
}