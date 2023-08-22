using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

    public partial class ProductToMark
    {
        public int ProductId { get; set; }
     
        public int MarkId { get; set; }

        public Product Product { get; set; }
        public Mark Mark { get; set; }

       


    }
}