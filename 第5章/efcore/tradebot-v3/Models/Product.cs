using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Product", Schema = "Production")]
public class Product
{
    public int ProductID { get; set; }

    [MaxLength(50)]
    public string Name { get; set;} = null!;

    [MaxLength(25)]
    public string ProductNumber { get; set; } = null!;

    public bool MakeFlag { get; set; }

    public List<ProductProductPhoto> ProductProductPhotos { get; set; } = null!;
}