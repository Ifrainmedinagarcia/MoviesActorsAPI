namespace ApiRestFull.DTO;

public class ActorDTo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime Birthday { get; set; }
    public List<string> Movies { get; set; }
}