using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Admin_Panel.Models;
namespace Admin_Panel.ViewModels
{

    public  class BrandCreateViewModels
    {
        public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }
        public IFormFile Photo { get; set; }
        public int? ImageId { get; set; }
      
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public IList<Product> Products { get; } = new List<Product>();
         public UploadedFiles UploadedFiles;
        
        
        // public virtual ICollection<Product> Product { get; set; }= new List<Product>();

    }
}