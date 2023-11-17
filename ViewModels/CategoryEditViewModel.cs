using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Admin_Panel.Models;
namespace Admin_Panel.ViewModels
{

    public  class CategoryEditViewModels
    {
        public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }
        public IFormFile Photo { get; set; }
        public int? ImageId { get; set; }
        public IFormFile PhotoIcon { get; set; }
        public int? IconId { get; set; }
        public int? Position { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public IList<ProductToCategory>  Products  { get; set; }
         public UploadedFiles UploadedFiles;
             public UploadedFiles UploadedFileIcon;
        
        // public virtual ICollection<Product> Product { get; set; }= new List<Product>();

    }
}