using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiRestFull.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieCategoriesToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories");

            migrationBuilder.RenameTable(
                name: "MoviesActors",
                newName: "Type1");

            migrationBuilder.RenameTable(
                name: "MovieCategories",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesActors_MovieId",
                table: "Type1",
                newName: "IX_Type1_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCategories_MovieId",
                table: "Type",
                newName: "IX_Type_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Type1",
                table: "Type1",
                columns: new[] { "ActorId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Type",
                table: "Type",
                columns: new[] { "CategoryId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Type_Categories_CategoryId",
                table: "Type",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Type_Movies_MovieId",
                table: "Type",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Type1_Actors_ActorId",
                table: "Type1",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Type1_Movies_MovieId",
                table: "Type1",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Type_Categories_CategoryId",
                table: "Type");

            migrationBuilder.DropForeignKey(
                name: "FK_Type_Movies_MovieId",
                table: "Type");

            migrationBuilder.DropForeignKey(
                name: "FK_Type1_Actors_ActorId",
                table: "Type1");

            migrationBuilder.DropForeignKey(
                name: "FK_Type1_Movies_MovieId",
                table: "Type1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Type1",
                table: "Type1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Type",
                table: "Type");

            migrationBuilder.RenameTable(
                name: "Type1",
                newName: "MoviesActors");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "MovieCategories");

            migrationBuilder.RenameIndex(
                name: "IX_Type1_MovieId",
                table: "MoviesActors",
                newName: "IX_MoviesActors_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Type_MovieId",
                table: "MovieCategories",
                newName: "IX_MovieCategories_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "ActorId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories",
                columns: new[] { "CategoryId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
