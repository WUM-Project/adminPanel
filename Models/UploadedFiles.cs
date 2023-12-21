using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin_Panel.Models
{
    public partial class UploadedFiles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string FilePath { get; set; }
        
        public string Path { get; set; }

        public IList<Product> Products { get; } = new List<Product>();
        public IList<ProductToUploadedFiles> ProductToUploadedFiles { get; set; }
        public IList<Brand> Brands { get; } = new List<Brand>();
        public IList<Category> Categories { get; } = new List<Category>();
        public IList<Category> CategoryIcon { get; } = new List<Category>();
    }
}