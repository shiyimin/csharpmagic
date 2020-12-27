using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Person", Schema = "Person")]
public class Person
{
    [Key]
    public int BusinessEntityID { get; set; }

    public string PersonType { get; set; } = null!;

    public string? Title { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public Password Password { get; set; } = null!;
}