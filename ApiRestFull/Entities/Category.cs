using System.ComponentModel.DataAnnotations;

namespace ApiRestFull.Entities;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreationDate { get; set; }
    public List<MovieCategories> MovieCategories{ get; set; }
}