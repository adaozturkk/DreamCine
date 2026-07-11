using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamCine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieSessionIdToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieSessionId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Tickets SET MovieSessionId = (SELECT MovieSessionId FROM Reservations WHERE Reservations.Id = Tickets.ReservationId)");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_MovieSessionId_SeatId",
                table: "Tickets",
                columns: new[] { "MovieSessionId", "SeatId" },
                unique: true,
                filter: "[Status] IN (1, 2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_MovieSessions_MovieSessionId",
                table: "Tickets",
                column: "MovieSessionId",
                principalTable: "MovieSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_MovieSessions_MovieSessionId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_MovieSessionId_SeatId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "MovieSessionId",
                table: "Tickets");
        }
    }
}
