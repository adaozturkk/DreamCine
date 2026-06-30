using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamCine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c2cc2e8-ae82-4245-8774-edf5f34d6833", null, "Staff", "STAFF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c2cc2e8-ae82-4245-8774-edf5f34d6833");
        }
    }
}
