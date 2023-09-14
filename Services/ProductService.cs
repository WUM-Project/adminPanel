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
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CancellationToken cancellationToken = default
        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var result = await _context.Products.ToListAsync(cancellationToken);
            var resultOut = _mapper.Map<IEnumerable<Product>>(result);

            return resultOut;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)

        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id);


            return product;
        }

        public async Task<Product> Create(Product product)
        {
            var res = await _context.Products
          .Where(result => result.Lang == product.Lang)
         .OrderByDescending(result => result.Position)
    .Select(result => new { result.Id, result.Position })
    .FirstOrDefaultAsync();
            product.Position = (res != null && res.Position.HasValue) ? res.Position.Value + 1 : 1;
            product.Status = 3;
            _context.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Update(Product product)
        {
            var existingProduct = await _context.Products
              .FirstOrDefaultAsync(m => m.Id == product.Id);

            if (existingProduct != null)
            {
                product.Lang = existingProduct.Lang;

                var originId = product.OriginId;
                if (originId.HasValue && originId != 0)
                {
                    var originproduct = await _context.Products
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originproduct != null)
                    {
                       
                        originproduct.Sku = product.Sku;
                        originproduct.Quantity = product.Quantity;
                        originproduct.Price = product.Price;
                        originproduct.DiscountedPrice = product.DiscountedPrice;
                        originproduct.Status = product.Status;
                        _context.Update(originproduct);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originproduct = await _context.Products
                      .FirstOrDefaultAsync(m => m.OriginId == product.Id);

                    if (originproduct != null)
                    {
                        originproduct.OriginId = product.Id;
                         originproduct.Sku = product.Sku;
                        originproduct.Quantity = product.Quantity;
                        originproduct.Price = product.Price;
                        originproduct.DiscountedPrice = product.DiscountedPrice;
                        originproduct.Status = product.Status;
                        _context.Update(originproduct);
                        await _context.SaveChangesAsync();
                    }
                }


                existingProduct.UpdatedAt = DateTime.Now;

                // Update the properties of the existingProduct object
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
            }



            // product.UpdatedAt = DateTime.Now;
            // _context.Update(product);
            // await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {



            var product = await _context.Products
               .FirstOrDefaultAsync(m => m.Id == id);

            if (product != null)
            {
                var originId = product.OriginId;

                if (originId.HasValue && originId != 0)
                {
                    var originproduct = await _context.Products
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originproduct != null)
                    {
                        _context.Products.Remove(originproduct);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originproduct = await _context.Products
                      .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (originproduct != null)
                    {
                        _context.Products.Remove(originproduct);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            }
        }
    }
}