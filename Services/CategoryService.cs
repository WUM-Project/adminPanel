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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           

           
            return await _context.Categories.ToListAsync(cancellationToken);
        }

        public async Task<Category> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        { 
            var category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id);
            
           
            return category;
        }

        public async Task Create(Category category)
        {    category.Status= 3;
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {category.UpdatedAt = DateTime.Now;
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}