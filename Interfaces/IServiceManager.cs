using System;

namespace Admin_Panel.Interfaces
{
    public interface IServiceManager
    {
       
        IAttributeService AttributeServices { get; }
        ICategoryService CategoryService { get; }
        IBrandService BrandService { get; }
        IMarkService MarkService { get; }
        IProductService ProductService { get; }
    }
}