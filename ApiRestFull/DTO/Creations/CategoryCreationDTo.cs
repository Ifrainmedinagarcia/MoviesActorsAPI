using System.ComponentModel.DataAnnotations;

namespace ApiRestFull.DTO;

public class CategoryCreationDTo
{
    [Required]
    [MaxLength(100, ErrorMessage = "The name is too long, please provide us a name shorter")]
    public string CategoryName { get; set; }
}