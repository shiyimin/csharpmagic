using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SalesOrderDetail", Schema = "Sales")]
public class SalesOrderDetail
{
    public int SalesOrderID { get; set; }

    public int SalesOrderDetailID { get; set; }

    public int ProductID { get; set; }

    public short OrderQty { get; set; }

    public Product Product { get; set; } = null!;

    [MaxLength(25)]
    public string? CarrierTrackingNumber { get; set; }
}