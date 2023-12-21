using System.Collections.Generic;
using System.Threading.Tasks;
using Admin_Panel.Data;
using Admin_Panel.Models;
namespace Admin_Panel.Interfaces
{
    public interface IAttributeService
    {
        // CancellationToken cancellationToken
        Task<IEnumerable<Models.Attribute>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Models.Attribute> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Models.Attribute> Create(Models.Attribute attribute);
        Task Update(Models.Attribute attribute);
        Task Delete(int id);
    }
}