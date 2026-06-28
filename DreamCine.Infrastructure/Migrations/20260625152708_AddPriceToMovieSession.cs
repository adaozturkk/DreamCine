using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamCine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToMovieSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08099c2b-a319-41d7-8aba-a02833b937fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0aaa321-4c92-4bb0-90c5-13604659ff77");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "MovieSessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "083557e1-cbfb-40e6-a96d-1241b6a3f0b3", null, "User", "USER" },
                    { "22debe27-0778-45aa-912b-e0701f49e43c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "083557e1-cbfb-40e6-a96d-1241b6a3f0b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22debe27-0778-45aa-912b-e0701f49e43c");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "MovieSessions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "08099c2b-a319-41d7-8aba-a02833b937fd", null, "Admin", "ADMIN" },
                    { "d0aaa321-4c92-4bb0-90c5-13604659ff77", null, "User", "USER" }
                });
        }
    }
}
