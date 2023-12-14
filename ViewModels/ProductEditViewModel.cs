using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Admin_Panel.Models;
namespace Admin_Panel.ViewModels
{

    public  class ProductEditViewModel
    {
         public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }
        
        public int Status { get; set; }
        public string Description { get; set; } 
        public string ShortDescription { get; set; }
        public string Sku { get; set; } 
        public int Price { get; set; } 
        public int DiscountedPrice { get; set; } 
        public int Quantity { get; set; } 
        public string Name { get; set; } = null!;
        public string ShortName { get; set; }     
        public int? Position { get; set; }
        public int? Availability { get; set; }
       
        public int? Popular { get; set; }
       public IFormFile Photo { get; set; }
        public int? ImageId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
      public  List<IFormFile> Gallery { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        //  public List<IFormFile>  Gallery { get; set; }
          public UploadedFiles UploadedFiles;
            public IList<ProductToUploadedFiles>  ProductToUploadedFile  { get; set; } =  new List<ProductToUploadedFiles>();
          
           public List<MarkViewModel> SelectedMarks { get; set; }
    public List<AttributeViewModel> SelectedAttributes { get; set; }
    public List<CategoryViewModel> SelectedCategories { get; set; }
            // public IList<ProductToMark>  SelectedMarks  { get; set; } =  new List<ProductToMark>();
            // public IList<ProductToAttribute>  SelectedAttributes  { get; set; } = new List<ProductToAttribute>();
            // public IList<ProductToCategory>  SelectedCategories  { get; set; }= new List<ProductToCategory>();

    }
}