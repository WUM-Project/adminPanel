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
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BrandService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CancellationToken cancellationToken = default
        public async Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var result = await _context.Brands.Include(d => d.UploadedFiles).ToListAsync(cancellationToken);
            var resultOut = _mapper.Map<IEnumerable<Brand>>(result);

            return resultOut;
        }

        public async Task<Brand> GetByIdAsync(int id, CancellationToken cancellationToken = default)

        {
            var brand = await _context.Brands.Include(d => d.UploadedFiles).FirstOrDefaultAsync(e => e.Id == id);


            return brand;
        }

        public async Task<Brand> Create(Brand brand)
        {

            brand.Status = 3;
            _context.Add(brand);
            await _context.SaveChangesAsync();
            return brand;

        }

        public async Task Update(Brand brand)
        {
            var existingBrand = await _context.Brands
            .FirstOrDefaultAsync(m => m.Id == brand.Id);

            if (existingBrand != null)
            {


                brand.Lang = existingBrand.Lang;

                var originId = brand.OriginId;
                if (originId.HasValue && originId != 0)
                {
                    var originBrand = await _context.Brands
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originBrand != null)
                    {

                        originBrand.Status = brand.Status;
                        originBrand.ImageId = brand.ImageId;

                        _context.Update(originBrand);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originBrand = await _context.Brands
                      .FirstOrDefaultAsync(m => m.OriginId == brand.Id);

                    if (originBrand != null)
                    {
                        originBrand.OriginId = brand.Id;

                        originBrand.Status = brand.Status;
                        originBrand.ImageId = brand.ImageId;

                        _context.Update(originBrand);
                        await _context.SaveChangesAsync();
                    }
                }


                existingBrand.UpdatedAt = DateTime.Now;

                // Update the properties of the existingBrand object
                _context.Entry(existingBrand).CurrentValues.SetValues(brand);
                _context.Update(existingBrand);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {

            var brand = await _context.Brands
            .FirstOrDefaultAsync(m => m.Id == id);

            if (brand != null)
            {
                var originId = brand.OriginId;

                if (originId.HasValue && originId != 0)
                {
                    var originBrand = await _context.Brands.Include(p => p.UploadedFiles)
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originBrand != null)
                    {
                        if (originBrand.UploadedFiles != null)
                        {
                            _context.UploadedFile.Remove(brand.UploadedFiles);
                        }
                        _context.Brands.Remove(originBrand);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originBrand = await _context.Brands.Include(p => p.UploadedFiles)
                      .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (originBrand != null)
                    {   if (originBrand.UploadedFiles != null)
                        {
                            _context.UploadedFile.Remove(brand.UploadedFiles);
                        }
                        _context.Brands.Remove(originBrand);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();

            }
        }
    }
}