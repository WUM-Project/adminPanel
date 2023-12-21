using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class Category
    {
        public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }


        public int? ImageId { get; set; }

        public UploadedFiles UploadedFiles;



        public int? IconId { get; set; }

        public UploadedFiles UploadedFileIcon;

        public int? Position { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public IList<ProductToCategory> Products { get; set; }



    }
}