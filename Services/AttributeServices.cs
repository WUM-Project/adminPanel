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
    public class AttributeServices : IAttributeService
    {
        private readonly AppDbContext _context;

        public AttributeServices(AppDbContext context)
        {
            _context = context;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Models.Attribute>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           

           
            return await _context.Attributes.ToListAsync(cancellationToken);
        }

        public async Task<Models.Attribute> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        {    Console.WriteLine(id);
            var attribute = await _context.Attributes.FirstOrDefaultAsync(e => e.Id == id);
            
            Console.WriteLine(attribute);
            Console.WriteLine("First");
            return attribute;
        }

        public async Task Create(Models.Attribute attribute)
        {    attribute.Status= 3;
            _context.Add(attribute);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Models.Attribute attribute)
        {attribute.UpdatedAt = DateTime.Now;
            _context.Update(attribute);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var attribute = await _context.Attributes.FindAsync(id);
            _context.Attributes.Remove(attribute);
            await _context.SaveChangesAsync();
        }
    }
}