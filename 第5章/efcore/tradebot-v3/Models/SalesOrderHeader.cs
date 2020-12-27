using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SalesOrderHeader", Schema = "Sales")]
public class SalesOrderHeader
{
    public int SalesOrderID { get; set; }

    public byte RevisionNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DueDate { get; set; }

    public byte Status { get; set;}

    public List<SalesOrderDetail> SalesOrderDetails { get; set; } = null!;
}