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
            CreateMap<CategoryCreationDTo, Category>()
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());
                
            CreateMap<MovieCreationDTo, Movie>()
                .ForMember(dest => dest.MoviesActors, opt => opt.MapFrom(MapActorMovies))
                .ForMember(dest => dest.MovieCategories, opt => opt.MapFrom(MapMovieCategories))
                .ReverseMap();
            CreateMap<ActorCreationDTo, Actor>()
                .ReverseMap();
            
            CreateMap<MoviePatchDTo, Movie>()
                .ForMember(dest => dest.MoviesActors, opt => opt.MapFrom(MapMoviesActorPatch))
                .ForMember(dest => dest.MovieCategories, opt => opt.MapFrom(MapMovieCategoriesPatch))
                .ReverseMap();
            
            CreateMap<Movie, MoviePatchDTo>()
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.MoviesActors.Select(ma => ma.ActorId)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.MovieCategories.Select(mc => mc.CategoryId)));
            
            CreateMap<ActorPatchDTo, Actor>().ReverseMap();
            CreateMap<CategoryPatchDTo, Category>().ReverseMap();

            #endregion
    }

    private static List<MovieCategories> MapMovieCategories(MovieCreationDTo movieCreationDTo, Movie movie)
    {
        var categoriesList = new List<MovieCategories>();
        if (movieCreationDTo.Categories is null) return categoriesList;
        categoriesList.AddRange(movieCreationDTo.Categories.Select(item => new MovieCategories { CategoryId = item }));
        return categoriesList;
    }
    
    private static List<MovieCategories> MapMovieCategoriesPatch(MoviePatchDTo moviePatch, Movie movie)
    {
        var categoriesList = new List<MovieCategories>();
        if (moviePatch.Categories is null) return categoriesList;
        categoriesList.AddRange(moviePatch.Categories.Select(categoryId => new MovieCategories() 
        { 
            CategoryId = categoryId 
        }));
        return categoriesList;
    }
    
    private static List<MoviesActors> MapMoviesActorPatch(MoviePatchDTo moviePatch, Movie movie)
    {
        var actorsList = new List<MoviesActors>();
        if (moviePatch.Actors is null) return actorsList;
        actorsList.AddRange(moviePatch.Actors.Select(actorId => new MoviesActors() 
        { 
            ActorId = actorId 
        }));
        return actorsList;
    }

    private static List<MoviesActors> MapActorMovies(MovieCreationDTo movieCreationDTo, Movie movie)
    {
        var actorMoviesResult = new List<MoviesActors>();
        if (movieCreationDTo.Actors is null) return actorMoviesResult;
        actorMoviesResult.AddRange(movieCreationDTo.Actors.Select(item => new MoviesActors() { ActorId = item }));
        return actorMoviesResult;
    }
}