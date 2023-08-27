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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           
           var result = await _context.Categories.ToListAsync(cancellationToken);
           var  resultOut = _mapper.Map<IEnumerable<Category>>(result);
           
            return resultOut;
        }

        public async Task<Category> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        { 
            var category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id);
            
           
            return category;
        }

        public async Task<Category> Create(Category category)
        {    category.Status= 3;
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category; 
            
        }

        public async Task Update(Category category)
        {category.UpdatedAt = DateTime.Now;
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            // var category = await _context.Categories.FindAsync(id);
            // _context.Categories.Remove(category);
            // await _context.SaveChangesAsync();
               var category = await _context.Categories
               .FirstOrDefaultAsync(m => m.Id == id);

            if (category != null)
            {
                var originId = category.OriginId;
                  
                if (originId.HasValue && originId != 0)
                {
                    var origincategory = await _context.Categories
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (origincategory != null)
                    {
                        _context.Categories.Remove(origincategory);

                        await _context.SaveChangesAsync();
                    }
                }
                else{
                    
                      var origincategory = await _context.Categories
                        .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (origincategory != null)
                    {
                        _context.Categories.Remove(origincategory);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
 
        }
        }
    }
}