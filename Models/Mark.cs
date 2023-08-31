using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class Mark
    {
        public int Id { get; set; }

        public int? OriginId { get; set; }
        public string Lang { get; set; }
        

        public string Title { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Type { get; set; } = null!;

        public int Status { get; set; }
       
        public int? Position { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public IList<ProductToMark>  ProductToMark  { get; set; }

    }
}