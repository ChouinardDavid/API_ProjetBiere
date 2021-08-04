using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBiere.Entity.Migrations
{
    public partial class AddEntityFramework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bieres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(nullable: true),
                    ABV = table.Column<double>(nullable: false),
                    Style = table.Column<int>(nullable: false),
                    SRM = table.Column<int>(nullable: false),
                    IBU = table.Column<int>(nullable: false),
                    Saisonniere = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bieres", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bieres",
                columns: new[] { "Id", "ABV", "IBU", "Nom", "SRM", "Saisonniere", "Style" },
                values: new object[] { 1, 4.2999999999999998, 53, "IPA 1", 4, 0, 0 });

            migrationBuilder.InsertData(
                table: "Bieres",
                columns: new[] { "Id", "ABV", "IBU", "Nom", "SRM", "Saisonniere", "Style" },
                values: new object[] { 2, 4.2999999999999998, 53, "Blanche 1", 5, 0, 4 });

            migrationBuilder.InsertData(
                table: "Bieres",
                columns: new[] { "Id", "ABV", "IBU", "Nom", "SRM", "Saisonniere", "Style" },
                values: new object[] { 3, 8.0, 70, "IPA 2", 2, 1, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bieres");
        }
    }
}
