using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin_Panel.Models
{

    public partial class ProductToAttribute
    {
          [Key]
        [Column(Order = 1)]
        [ForeignKey("Attribute")] 
        public int AttributeId { get; set; }
        public string Value { get; set; }
          [Key]
        [Column(Order = 0)]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public Attribute Attribute { get; set; }

       


    }
}