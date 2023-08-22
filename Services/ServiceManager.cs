using System;

using Microsoft.Extensions.Options;


using Microsoft.Extensions.Configuration;
using Admin_Panel.Interfaces;

using Admin_Panel.Data;
namespace Admin_Panel.Services
{
    public sealed class ServiceManager : IServiceManager
    {   
        
          private readonly AppDbContext _context;
        private readonly Lazy<IAttributeService> _lazyAttiruteService;
        private readonly Lazy<ICategoryService> _lazyCategoryService;
        private readonly Lazy<IMarkService> __lazyMarkService;

        public ServiceManager( AppDbContext context)
        {
            _context = context;
            _lazyAttiruteService = new Lazy<IAttributeService>(() => new AttributeServices(_context));
            _lazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(_context));
            __lazyMarkService = new Lazy<IMarkService>(() => new MarkService(_context));
            // reportGrpcService, examGrpcService,
        }

    
        public IAttributeService AttributeServices => _lazyAttiruteService.Value;
        public ICategoryService CategoryService => _lazyCategoryService.Value;
        public IMarkService MarkService => __lazyMarkService.Value;

     
    }

}