using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class Brand
    {
        public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }


        public string Title { get; set; }

        public int Status { get; set; }
        // public IFormFile ImageId { get; set; }
        // [ForeignKey("UploadedFiles")]


        public int? ImageId { get; set; }

        public UploadedFiles UploadedFiles;
        public IList<Product> Products { get; } = new List<Product>();
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;





    }
}