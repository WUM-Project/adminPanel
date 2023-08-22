using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class ProductToCategory
    {
        public int ProductId { get; set; }
     
        public int CategoryId { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }

       


    }
}