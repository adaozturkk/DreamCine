using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamCine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b07d29f-e392-4efb-8c8d-a8a0b5243670");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6bfd4db-9be8-45ef-a23f-8b41dedb146d");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "297de5bf-b618-40c6-b2f3-e36a56d1971e", null, "Admin", "ADMIN" },
                    { "3dfcb2e7-0d2a-476a-873d-694b87f5f238", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ReleaseDate",
                table: "Movies",
                column: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_ReleaseDate",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Title",
                table: "Movies");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "297de5bf-b618-40c6-b2f3-e36a56d1971e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3dfcb2e7-0d2a-476a-873d-694b87f5f238");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b07d29f-e392-4efb-8c8d-a8a0b5243670", null, "Admin", "ADMIN" },
                    { "a6bfd4db-9be8-45ef-a23f-8b41dedb146d", null, "User", "USER" }
                });
        }
    }
}
