using System.ComponentModel.DataAnnotations;
using ApiRestFull.Entities;

namespace ApiRestFull.DTO;

public class MoviePatchDTo
{
    
    [MaxLength(100, ErrorMessage = "The name is too long, please provide us a name shorter")]
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public ClassificationType Type { get; set; }
    public List<int> Actors { get; set; }

    public List<int> Categories { get; set; }
}