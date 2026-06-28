using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamCine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_StatusId",
                table: "Movies",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Statuses_StatusId",
                table: "Movies",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Statuses_StatusId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_StatusId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Movies");
        }
    }
}
