using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class Product
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
        public int? ImageId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

            // public virtual ICollection<Models.Attribute> Attributes { get; set; } = new List<Models.Attribute>();
            // public virtual Category Categories { get; set; }
            // public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
            // public Category category;
            public IList<ProductToMark>  Marks  { get; set; } =  new List<ProductToMark>();
            public IList<ProductToAttribute>  ProductToAttribute  { get; set; }
            public IList<ProductToCategory>  Categories  { get; set; }= new List<ProductToCategory>();
            //   public virtual ICollection<Category> Category { get; set; }= new List<Category>();

    }
}