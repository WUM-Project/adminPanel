using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin_Panel.Models
{

 
        public partial class ProductToMark
    {
        [Key]
        [Column(Order = 0)]
        // [ForeignKey("Product")]
        public int ProductId { get; set; }

         [Key]
        [Column(Order = 1)]
        // [ForeignKey("Mark")]     
        public int MarkId { get; set; }


        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
         [ForeignKey("MarkId")]
        public Mark Mark { get; set; }

       


    }
}