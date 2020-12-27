using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ProductPhoto", Schema = "Production")]
public class ProductPhoto
{
    public int ProductPhotoID { get; set; }

    public byte[]? ThumbNailPhoto { get; set; }

    public string? ThumbnailPhotoFileName { get; set; }

    public List<ProductProductPhoto> ProductProductPhotos { get; set; } = null!;
}