using System.ComponentModel.DataAnnotations;
using ApiRestFull.Entities;

namespace ApiRestFull.DTO;

public class MovieDTo
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Duration { get; set; }
    public ClassificationType Type { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Actors { get; set; }
}