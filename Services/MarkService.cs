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
namespace Admin_Panel.Services
{
    public class MarkService : IMarkService
    {
        private readonly AppDbContext _context;

        public MarkService(AppDbContext context)
        {
            _context = context;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Mark>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           

           
            return await _context.Marks.ToListAsync(cancellationToken);
        }

        public async Task<Mark> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        { 
            var mark = await _context.Marks.FirstOrDefaultAsync(e => e.Id == id);
            
           
            return mark;
        }

        public async Task Create(Mark mark)
        {    mark.Status= 3;
            _context.Add(mark);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Mark mark)
        {mark.UpdatedAt = DateTime.Now;
            _context.Update(mark);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var mark = await _context.Marks.FindAsync(id);
            _context.Marks.Remove(mark);
            await _context.SaveChangesAsync();
        }
    }
}