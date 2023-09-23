using System.ComponentModel.DataAnnotations.Schema;
using ApiRestFull.DTO;
using Microsoft.EntityFrameworkCore;

namespace ApiRestFull.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public enum ClassificationType { Seven, Thirteen, Sixteen,  Eighteen  }
    public ClassificationType Type { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<MovieCategories> MovieCategories{ get; set; }
    public List<MoviesActors> MoviesActors { get; set; }
}