using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarServices.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elaborations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elaborations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elaborations_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Offer = table.Column<bool>(type: "bit", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    ElaborationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Elaborations_ElaborationId",
                        column: x => x.ElaborationId,
                        principalTable: "Elaborations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elaborations_TableId",
                table: "Elaborations",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ElaborationId",
                table: "Products",
                column: "ElaborationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TableId",
                table: "Products",
                column: "TableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Elaborations");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
