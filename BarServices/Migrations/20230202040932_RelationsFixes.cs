using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarServices.Migrations
{
    /// <inheritdoc />
    public partial class RelationsFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elaborations_Tables_TableId",
                table: "Elaborations");

            migrationBuilder.DropIndex(
                name: "IX_Elaborations_TableId",
                table: "Elaborations");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Elaborations");

            migrationBuilder.AddColumn<int>(
                name: "BarId",
                table: "Tables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KitchenId",
                table: "Tables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tables_BarId",
                table: "Tables",
                column: "BarId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_KitchenId",
                table: "Tables",
                column: "KitchenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Elaborations_BarId",
                table: "Tables",
                column: "BarId",
                principalTable: "Elaborations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Elaborations_KitchenId",
                table: "Tables",
                column: "KitchenId",
                principalTable: "Elaborations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Elaborations_BarId",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Elaborations_KitchenId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_BarId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_KitchenId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "BarId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "KitchenId",
                table: "Tables");

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Elaborations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Elaborations_TableId",
                table: "Elaborations",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elaborations_Tables_TableId",
                table: "Elaborations",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id");
        }
    }
}
