using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin_Panel.Models
{

    public partial class ProductToCategory
    {
        // [Key]
        // public int Id { get; set; }
         [Key]
        [Column(Order = 0)]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
          [Key]
        [Column(Order = 1)]
        [ForeignKey("Category")] 
        public int CategoryId { get; set; }

        // [ForeignKey(nameof(ProductId))]
        // public Product Product { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
       


    }
}