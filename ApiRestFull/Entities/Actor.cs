namespace ApiRestFull.Entities;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Birthday { get; set; }
    public List<MoviesActors> MoviesActors { get; set; }
}