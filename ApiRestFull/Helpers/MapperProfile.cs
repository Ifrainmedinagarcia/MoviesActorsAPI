using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Migrations;
using AutoMapper;
using Actor = ApiRestFull.Entities.Actor;

namespace ApiRestFull.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region GET
            CreateMap<Category, CategoryDTo>().ReverseMap();
            CreateMap<Movie, MovieDTo>()
                .ForMember(dest => dest.Categories, 
                    opt => opt.MapFrom(src => src.MovieCategories.Select(x => x.Category.CategoryName)))
                .ForMember(dest => dest.Actors, 
                    opt =>opt.MapFrom(src => src.MoviesActors.Select(x =>x.Actor.Name)))
                .ReverseMap();
            CreateMap<Actor, ActorDTo>()
                .ForMember(dest => dest.Movies, 
                    opt => opt.MapFrom(src => src.MoviesActors.Select(x => x.Movie.Title)))
                .ReverseMap();
        #endregion
        
        #region POST
            CreateMap<CategoryCreationDTo, Category>().ReverseMap();
            CreateMap<MovieCreationDTo, Movie>()
                .ForMember(dest => dest.MoviesActors, opt => opt.MapFrom(MapActorMovies))
                .ForMember(dest => dest.MovieCategories, opt => opt.MapFrom(MapMovieCategories))
                .ReverseMap();
            CreateMap<ActorCreationDTo, Actor>()
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

    private static List<MoviesActors> MapActorMovies(MovieCreationDTo movieCreationDTo, Movie movie)
    {
        var actorMoviesResult = new List<MoviesActors>();
        if (movieCreationDTo.Actors is null) return actorMoviesResult;
        actorMoviesResult.AddRange(movieCreationDTo.Actors.Select(item => new MoviesActors() { ActorId = item }));
        return actorMoviesResult;
    }
    
}