using Newtonsoft.Json;

namespace ApiRestFull.Entities;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<MoviesActors> MoviesActors { get; set; }
}