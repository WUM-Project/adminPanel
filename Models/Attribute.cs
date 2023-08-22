using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Admin_Panel.Models
{

public partial class Attribute
{
    public int Id { get; set; }

    public int? OriginId { get; set; }

    public string Lang { get; set; }

    public string Value { get; set; }

    public string UnitOfMeasurement { get; set; }

    public string Title { get; set; } = null!;

    public string ShortTitle { get; set; }

    public int? GroupAttr { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; } =DateTime.Now;

    public DateTime? UpdatedAt { get; set; } =DateTime.Now;

    public int? Position { get; set; }
}
}