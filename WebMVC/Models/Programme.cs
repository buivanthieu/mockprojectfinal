using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC.Models;

public class Programme
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [ForeignKey("Contact")]
    public int ContactId { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("ContactId")]
    public Contact? Contact { get; set; }
}