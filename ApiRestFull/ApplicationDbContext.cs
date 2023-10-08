using ApiRestFull.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ApiRestFull;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<MovieCategories> MovieCategories { get; set; }

    public DbSet<MoviesActors> MoviesActors { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region CategoryEntity
            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);
            
            modelBuilder.Entity<Category>()
                .Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
        #endregion

        #region MovieEntity

        modelBuilder.Entity<Movie>()
            .HasKey(x => x.Id);
                
            modelBuilder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<Movie>()
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(300);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Duration)
                .IsRequired();
            
            
        #endregion

        #region MovieCategories

            modelBuilder.Entity<MovieCategories>().HasKey(x => new { x.CategoryId, x.MovieId });

            modelBuilder.Entity<MovieCategories>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<MovieCategories>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MovieCategories)
                .HasForeignKey(mc => mc.CategoryId);

        #endregion

        #region MoviesActors
            modelBuilder.Entity<MoviesActors>().HasKey(x => new { x.ActorId, x.MovieId });

            modelBuilder.Entity<MoviesActors>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MoviesActors)
                .HasForeignKey(x => x.MovieId);

            modelBuilder.Entity<MoviesActors>()
                .HasOne(x => x.Actor)
                .WithMany(x => x.MoviesActors)
                .HasForeignKey(x => x.ActorId);
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    }
}