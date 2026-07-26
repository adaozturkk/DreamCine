using DreamCine.Application.Services;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Moq;

namespace DreamCine.Tests
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepoMock;
        private readonly TicketService _service;

        public TicketServiceTests()
        {
            _ticketRepoMock = new Mock<ITicketRepository>();
            _service = new TicketService(_ticketRepoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_TicketDoesNotExist_ReturnsNotFound()
        {
            _ticketRepoMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Ticket?)null);

            var result = await _service.GetByIdAsync(999, "any-user", false);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Ticket not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsAdmin_ReturnsSuccess()
        {
            var ticket = new Ticket
            {
                Id = 1,
                Status = Core.Enums.TicketStatus.Pending,
                Seat = new Seat { Id = 10, SeatNumber = "A1" },
                MovieSession = new MovieSession
                {
                    Movie = new Movie { Title = "Inception" },
                    Screen = new Screen { ScreenNumber = 5 }
                },
                Reservation = new Reservation
                {
                    Id = 100,
                    Status = Core.Enums.ReservationStatus.Pending,
                    UserId = "someone-else",
                    MovieSession = new MovieSession
                    {
                        Movie = new Movie { Title = "Inception" },
                        Screen = new Screen { ScreenNumber = 5 }
                    },
                    Tickets = []
                }
            };
            ticket.Reservation.Tickets.Add(ticket);

            _ticketRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(ticket);

            var result = await _service.GetByIdAsync(1, "admin-user", true);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsOwner_ReturnsSuccess()
        {
            var ticket = new Ticket
            {
                Id = 1,
                Status = Core.Enums.TicketStatus.Pending,
                Seat = new Seat { Id = 10, SeatNumber = "A1" },
                MovieSession = new MovieSession
                {
                    Movie = new Movie { Title = "Inception" },
                    Screen = new Screen { ScreenNumber = 5 }
                },
                Reservation = new Reservation
                {
                    Id = 100,
                    Status = Core.Enums.ReservationStatus.Pending,
                    UserId = "owner-id",
                    MovieSession = new MovieSession
                    {
                        Movie = new Movie { Title = "Inception" },
                        Screen = new Screen { ScreenNumber = 5 }
                    },
                    Tickets = []
                }
            };
            ticket.Reservation.Tickets.Add(ticket);

            _ticketRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(ticket);

            var result = await _service.GetByIdAsync(1, "owner-id", false);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsNotOwnerAndNotAdmin_ReturnsNotFound()
        {
            var ticket = new Ticket
            {
                Id = 1,
                Status = Core.Enums.TicketStatus.Pending,
                Seat = new Seat { Id = 10, SeatNumber = "A1" },
                MovieSession = new MovieSession
                {
                    Movie = new Movie { Title = "Inception" },
                    Screen = new Screen { ScreenNumber = 5 }
                },
                Reservation = new Reservation
                {
                    Id = 100,
                    Status = Core.Enums.ReservationStatus.Pending,
                    UserId = "original-owner",
                    MovieSession = new MovieSession
                    {
                        Movie = new Movie { Title = "Inception" },
                        Screen = new Screen { ScreenNumber = 5 }
                    },
                    Tickets = []
                }
            };
            ticket.Reservation.Tickets.Add(ticket);

            _ticketRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(ticket);

            var result = await _service.GetByIdAsync(1, "hacker-user", false);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Ticket not found.", result.ErrorMessage);
        }
    }
}
