using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Knight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Knights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NickName = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Attributes_Strength = table.Column<int>(type: "integer", nullable: false),
                    Attributes_Dexterity = table.Column<int>(type: "integer", nullable: false),
                    Attributes_Constitution = table.Column<int>(type: "integer", nullable: false),
                    Attributes_Intelligence = table.Column<int>(type: "integer", nullable: false),
                    Attributes_Wisdom = table.Column<int>(type: "integer", nullable: false),
                    Attributes_Charisma = table.Column<int>(type: "integer", nullable: false),
                    KeyAttribute = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnightWeapons",
                columns: table => new
                {
                    KnightId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mod = table.Column<int>(type: "integer", nullable: false),
                    Attr = table.Column<string>(type: "text", nullable: false),
                    Equipped = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnightWeapons", x => new { x.KnightId, x.Id });
                    table.ForeignKey(
                        name: "FK_KnightWeapons_Knights_KnightId",
                        column: x => x.KnightId,
                        principalTable: "Knights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnightWeapons");

            migrationBuilder.DropTable(
                name: "Knights");
        }
    }
}
