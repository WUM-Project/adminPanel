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
    public class AttributeServices : IAttributeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AttributeServices(AppDbContext context, IMapper mapper)
        {
            _context = context;
             _mapper = mapper;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Models.Attribute>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           
           var result = await _context.Attributes.ToListAsync(cancellationToken);
           var  resultOut = _mapper.Map<IEnumerable<Models.Attribute>>(result);
           
            return resultOut;
           
            
        }

        public async Task<Models.Attribute> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        {    Console.WriteLine(id);
            var attribute = await _context.Attributes.FirstOrDefaultAsync(e => e.Id == id);
            
            Console.WriteLine(attribute);
            Console.WriteLine("First");
            return attribute;
        }

        public async Task<Models.Attribute> Create(Models.Attribute attribute)
        {    attribute.Status= 3;
            _context.Add(attribute);
            await _context.SaveChangesAsync();
            return attribute;
        }

        public async Task Update(Models.Attribute attribute)
        {attribute.UpdatedAt = DateTime.Now;
            _context.Update(attribute);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            // var attribute = await _context.Attributes.FindAsync(id);
            // _context.Attributes.Remove(attribute);
            // await _context.SaveChangesAsync();
               var attribute = await _context.Attributes
               .FirstOrDefaultAsync(m => m.Id == id);

            if (attribute != null)
            {
                var originId = attribute.OriginId;
                  
                if (originId.HasValue && originId != 0)
                {
                    var originattribute = await _context.Attributes
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originattribute != null)
                    {
                        _context.Attributes.Remove(originattribute);

                        await _context.SaveChangesAsync();
                    }
                }
                else{
                    
                      var originattribute = await _context.Attributes
                        .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (originattribute != null)
                    {
                        _context.Attributes.Remove(originattribute);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Attributes.Remove(attribute);
                await _context.SaveChangesAsync();
            }
            }
    }
}