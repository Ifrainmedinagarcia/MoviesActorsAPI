using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiRestFull.Migrations
{
    /// <inheritdoc />
    public partial class ActorBirthday2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Actors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Actors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
