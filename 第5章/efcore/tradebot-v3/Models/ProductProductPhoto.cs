using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ProductProductPhoto", Schema = "Production")]
public class ProductProductPhoto
{
    [Key]
    [Column(Order = 0)]
    public int ProductID { get; set; }
    
    [Key]
    [Column(Order = 1)]
    public int ProductPhotoID { get; set; }

    public Product Product { get; set; } = null!;

    public ProductPhoto ProductPhoto { get; set; } = null!;
}