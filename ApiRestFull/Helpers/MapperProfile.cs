using ApiRestFull.DTO;
using ApiRestFull.Entities;
using AutoMapper;

namespace ApiRestFull.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region GET
            CreateMap<Category, CategoryDTo>().ReverseMap();
            CreateMap<Movie, MovieDTo>()
                .ForMember(dest => dest.Categories, 
                    opt => opt.MapFrom(src => src.MovieCategories.Select(pc => pc.Category.CategoryName)))
                .ReverseMap();
        #endregion
        
        #region POST
            CreateMap<CategoryCreationDTo, Category>().ReverseMap();
            CreateMap<MovieCreationDTo, Movie>()
                .ForMember(dest => dest.MovieCategories, opt => opt.MapFrom(MapMovieCategories))
                .ReverseMap();
        #endregion
    }

    private static List<MovieCategories> MapMovieCategories(MovieCreationDTo movieCreationDTo, Movie movie)
    {
        var categoriesList = new List<MovieCategories>();
        if (movieCreationDTo.Categories is null) return categoriesList;
        categoriesList.AddRange(movieCreationDTo.Categories.Select(item => new MovieCategories { CategoryId = item }));
        return categoriesList;
    }
    
}