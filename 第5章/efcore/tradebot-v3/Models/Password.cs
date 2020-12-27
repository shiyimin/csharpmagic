using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Password", Schema = "Person")]
public class Password
{
    [Key]
    // [ForeignKey("Person")]
    public int BusinessEntityID { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set;} = null!;

    // public Person Person { get; set; } = null!;
}