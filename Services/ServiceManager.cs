using System;

using Microsoft.Extensions.Options;


using Microsoft.Extensions.Configuration;
using Admin_Panel.Interfaces;
using AutoMapper;
using Admin_Panel.Data;
namespace Admin_Panel.Services
{
    public sealed class ServiceManager : IServiceManager
    {   
        
          private readonly AppDbContext _context;
        private readonly Lazy<IAttributeService> _lazyAttiruteService;
        private readonly Lazy<ICategoryService> _lazyCategoryService;
        private readonly Lazy<IBrandService> _lazyBrandService;
        private readonly Lazy<IMarkService> __lazyMarkService;
        private readonly Lazy<IProductService> __lazyProductService;

        public ServiceManager( AppDbContext context,IMapper mapper)
        {
            _context = context;
            _lazyAttiruteService = new Lazy<IAttributeService>(() => new AttributeServices(_context, mapper));
            _lazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(_context, mapper));
            _lazyBrandService = new Lazy<IBrandService>(() => new BrandService(_context, mapper));
            __lazyMarkService = new Lazy<IMarkService>(() => new MarkService(_context,mapper));
            __lazyProductService = new Lazy<IProductService>(() => new ProductService(_context,mapper));
            // reportGrpcService, examGrpcService,
        }

    
        public IAttributeService AttributeServices => _lazyAttiruteService.Value;
        public ICategoryService CategoryService => _lazyCategoryService.Value;
        public IBrandService BrandService => _lazyBrandService.Value;
        public IMarkService MarkService => __lazyMarkService.Value;
        public IProductService ProductService => __lazyProductService.Value;

     
    }

}