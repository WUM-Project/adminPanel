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
    public class RolesService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RolesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

// CancellationToken cancellationToken = default
        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        {   
           
           var result = await _context.Roles.ToListAsync(cancellationToken);
           var  resultOut = _mapper.Map<IEnumerable<Role>>(result);
           
            return resultOut;
        }

        public async Task<Role> GetByIdAsync(int id,CancellationToken cancellationToken = default)

        { 
            var role = await _context.Roles.FirstOrDefaultAsync(e => e.Id == id);
            
           
            return role;
        }

        public async Task<Role> Create(Role role)
        {
            //          var res = await _context.Roles
            //      .Where(result => result.Lang == category.Lang)
            //     .OrderByDescending(result => result.Position)
            //.Select(result => new { result.Id, result.Position })
            //.FirstOrDefaultAsync();
            //        category.Position = (res != null && res.Position.HasValue) ? res.Position.Value + 1 : 1;
            //        category.Status= 3;
            //        _context.Add(category);
            //        await _context.SaveChangesAsync();
            //        return category; 

            return null;
            
        }

        public async Task Update(Role role)
        {
    //          var existingCategory = await _context.Categories
    //          .FirstOrDefaultAsync(m => m.Id == category.Id);
            
    //        if (existingCategory != null)
    //        {   
    //            if(category.ParentId !=null){
    //            var originParentCategoryId = await _context.Categories
    //.Where(cat => cat.Id == category.ParentId || cat.OriginId == category.ParentId)
    //.Select(cat => cat.OriginId ?? cat.Id)
    //.FirstOrDefaultAsync();

    
    //}

    //            category.Lang = existingCategory.Lang;

    //            var originId = category.OriginId;
    //            if (originId.HasValue && originId != 0)
    //            {
    //                var originCategory = await _context.Categories
    //                    .FirstOrDefaultAsync(m => m.Id == originId.Value);

    //                if (originCategory != null)
    //                {
                        
    //                    originCategory.Status = category.Status;
    //                    originCategory.ImageId = category.ImageId;
    //                    _context.Update(originCategory);
    //                    await _context.SaveChangesAsync();
    //                }
    //            }
    //            else
    //            {

    //                var originCategory = await _context.Categories
    //                  .FirstOrDefaultAsync(m => m.OriginId == category.Id);

    //                if (originCategory != null)
    //                {
    //                    originCategory.OriginId = category.Id;
                        
    //                    originCategory.Status = category.Status;
    //                    originCategory.ImageId = category.ImageId;
    //                    _context.Update(originCategory);
    //                    await _context.SaveChangesAsync();
    //                }
    //            }


    //            existingCategory.UpdatedAt = DateTime.Now;

    //            // Update the properties of the existingCategory object
    //            _context.Entry(existingCategory).CurrentValues.SetValues(category);
    //            _context.Update(existingCategory);
    //            await _context.SaveChangesAsync();
    //        }
        }

        public async Task Delete(int id)
        {
            // var category = await _context.Categories.FindAsync(id);
            // _context.Categories.Remove(category);
            // await _context.SaveChangesAsync();




            //   var category = await _context.Categories
            //   .FirstOrDefaultAsync(m => m.Id == id);

            //if (category != null)
            //{
            //    var originId = category.OriginId;
                  
            //    if (originId.HasValue && originId != 0)
            //    {
            //        var origincategory = await _context.Categories
            //            .FirstOrDefaultAsync(m => m.Id == originId.Value);

            //        if (origincategory != null)
            //        {
            //            _context.Categories.Remove(origincategory);

            //            await _context.SaveChangesAsync();
            //        }
            //    }
            //    else{
                    
            //          var origincategory = await _context.Categories
            //            .FirstOrDefaultAsync(m => m.OriginId == id);

            //        if (origincategory != null)
            //        {
            //            _context.Categories.Remove(origincategory);

            //            await _context.SaveChangesAsync();
            //        }
            //    }

            //    _context.Categories.Remove(category);
            //    await _context.SaveChangesAsync();
 
           // }
        }
    }
}