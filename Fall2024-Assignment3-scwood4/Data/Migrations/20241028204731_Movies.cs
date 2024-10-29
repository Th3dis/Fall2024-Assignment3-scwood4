using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2024_Assignment3_scwood4.Data.Migrations
{
    /// <inheritdoc />
    public partial class Movies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Actors",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)");

            migrationBuilder.AddColumn<string>(
                name: "MoviesActed",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "ActorMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    actorId = table.Column<int>(type: "int", nullable: false),
                    movieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActorMovies_Actors_actorId",
                        column: x => x.actorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovies_Movies_movieId",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovies_actorId",
                table: "ActorMovies",
                column: "actorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovies_movieId",
                table: "ActorMovies",
                column: "movieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorMovies");

            migrationBuilder.DropColumn(
                name: "Actors",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MoviesActed",
                table: "Actors");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Actors",
                type: "nvarchar(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
