using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Admin_Panel.Models;
namespace Admin_Panel.ViewModels
{

    public class AttributeViewModel
    {
        public int AttributeId { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }
        // Other properties related to Attribute
    }

}