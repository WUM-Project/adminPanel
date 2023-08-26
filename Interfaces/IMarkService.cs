using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface IMarkService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Mark>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Mark> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<Mark> Create(Mark attribute);
        Task Update(Mark attribute);
        Task Delete(int id);
    }
}