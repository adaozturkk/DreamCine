using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamCine.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixMovieSessionPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "083557e1-cbfb-40e6-a96d-1241b6a3f0b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22debe27-0778-45aa-912b-e0701f49e43c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0aa1f064-95bd-41f8-9b61-95570f73ef92", null, "User", "USER" },
                    { "bb6829d7-a5c6-4216-acf6-e8658a8b7c24", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0aa1f064-95bd-41f8-9b61-95570f73ef92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb6829d7-a5c6-4216-acf6-e8658a8b7c24");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "083557e1-cbfb-40e6-a96d-1241b6a3f0b3", null, "User", "USER" },
                    { "22debe27-0778-45aa-912b-e0701f49e43c", null, "Admin", "ADMIN" }
                });
        }
    }
}
