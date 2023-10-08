using System.ComponentModel.DataAnnotations;
using ApiRestFull.Entities;
using Newtonsoft.Json;

namespace ApiRestFull.DTO;

public class MovieCreationDTo
{
    [Required]
    [MaxLength(100, ErrorMessage = "The name is too long, please provide us a name shorter")]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Duration { get; set; }
    public ClassificationType Type { get; set; }
    
    public List<int> Categories { get; set; }
    
    public List<int> Actors { get; set; }
}