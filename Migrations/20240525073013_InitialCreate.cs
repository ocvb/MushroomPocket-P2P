using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MushroomPocket.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnemyCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Hp = table.Column<int>(type: "INTEGER", nullable: false),
                    Skill = table.Column<string>(type: "TEXT", nullable: true),
                    isDead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemyCharacters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MushroomHeroes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Hp = table.Column<double>(type: "REAL", nullable: false),
                    Exp = table.Column<int>(type: "INTEGER", nullable: false),
                    Skill = table.Column<string>(type: "TEXT", nullable: true),
                    damage = table.Column<double>(type: "REAL", nullable: false),
                    isDead = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MushroomHeroes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MushroomHeroes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MushroomHeroes_UserId",
                table: "MushroomHeroes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnemyCharacters");

            migrationBuilder.DropTable(
                name: "MushroomHeroes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
