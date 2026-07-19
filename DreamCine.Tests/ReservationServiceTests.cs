    using DreamCine.Application.DTOs.Reservation;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Services;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Moq;

namespace DreamCine.Tests
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _reservationMock;
        private readonly Mock<IMovieSessionRepository> _movieSessionMock;
        private readonly Mock<ITicketRepository> _ticketMock;
        private readonly Mock<ISeatRepository> _seatMock;
        private readonly Mock<IJobScheduler> _jobSchedulerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _reservationMock = new Mock<IReservationRepository>();
            _movieSessionMock = new Mock<IMovieSessionRepository>();
            _ticketMock = new Mock<ITicketRepository>();
            _seatMock = new Mock<ISeatRepository>();
            _jobSchedulerMock = new Mock<IJobScheduler>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _service = new ReservationService(
                _reservationMock.Object,
                _movieSessionMock.Object,
                _ticketMock.Object,
                _seatMock.Object,
                _jobSchedulerMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task CreateReservationAsync_ValidRequest_ReturnsSuccessAndCalculatesPrice()
        {
            var session = new MovieSession { Id = 1, Price = 10.00m };
            _movieSessionMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(session);

            var seatIds = new List<int> { 10, 11 };
            var seats = new List<Seat>
            {
                new Seat { Id = 10, SeatNumber = "A1" },
                new Seat { Id = 11, SeatNumber = "A2" }
            };
            _seatMock.Setup(repo => repo.GetSeatsByIdAsync(seatIds))
                .ReturnsAsync(seats);

            _ticketMock.Setup(repo => repo.GetOccupiedSeatIdsAsync(1))
                .ReturnsAsync(new List<int>());

            var dto = new CreateReservationDto
            {
                MovieSessionId = 1,
                SeatIds = seatIds
            };

            var result = await _service.CreateReservationAsync(dto, "test-user-id");

            Assert.True(result.IsSuccess);
            Assert.Equal(20, result.Data!.TotalPrice);
            _unitOfWorkMock.Verify(u =>
                u.CommitTransactionAsync(), Times.Once);
            _jobSchedulerMock.Verify(j => 
                j.ScheduleCancelReservationJob(It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task CreateReservationAsync_SessionDoesNotExist_ReturnsNotFound()
        {
            _movieSessionMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((MovieSession?)null);

            var dto = new CreateReservationDto
            {
                MovieSessionId = 999,
                SeatIds = [1]
            };

            var result = await _service.CreateReservationAsync(dto, "test-user-id");

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Selected movie session does not exist.", result.ErrorMessage);
            _unitOfWorkMock.Verify(u =>
                u.BeginTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateReservationAsync_SeatDoesNotExist_ReturnsBadRequest()
        {
            _movieSessionMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new MovieSession { Id = 1 });

            var seatIds = new List<int> { 10, 11 };
            var seats = new List<Seat>
            {
                new Seat { Id = 10, SeatNumber = "A1" }
            };
            _seatMock.Setup(repo => repo.GetSeatsByIdAsync(seatIds))
                .ReturnsAsync(seats);

            var dto = new CreateReservationDto
            {
                MovieSessionId = 1,
                SeatIds = [10, 11]
            };

            var result = await _service.CreateReservationAsync(dto, "test-user-id");

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("One or more selected seats do not exist.", result.ErrorMessage);
            _unitOfWorkMock.Verify(u =>
                u.BeginTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateReservationAsync_SeatAlreadyReserved_ReturnsBadRequest()
        {
            _movieSessionMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new MovieSession { Id = 1 });

            var seatIds = new List<int> { 10 };
            var seats = new List<Seat>
            {
                new Seat { Id = 10, SeatNumber = "A1" }
            };
            _seatMock.Setup(repo => repo.GetSeatsByIdAsync(seatIds))
                .ReturnsAsync(seats);

            _ticketMock.Setup(repo => repo.GetOccupiedSeatIdsAsync(1))
                .ReturnsAsync(new List<int> { 10 });

            var dto = new CreateReservationDto
            {
                MovieSessionId = 1,
                SeatIds = [10]
            };

            var result = await _service.CreateReservationAsync(dto, "test-user-id");

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("One or more selected seats are already reserved.", result.ErrorMessage);
            _unitOfWorkMock.Verify(u =>
                u.BeginTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task CancelReservationAsync_ValidPendingReservation_ReturnsSuccess()
        {
            var reservation = new Reservation
            {
                Id = 100,
                Status = Core.Enums.ReservationStatus.Pending,
                UserId = "test-user-id",
                Tickets = [new Ticket { Id = 1, Status = Core.Enums.TicketStatus.Pending },
                        new Ticket { Id = 2, Status = Core.Enums.TicketStatus.Pending }]
            };

            _reservationMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(reservation);

            var result = await _service.CancelReservationAsync(100, "test-user-id", false);

            Assert.True(result.IsSuccess);
            Assert.Equal("Reservation cancelled successfully.", result.Data);
            Assert.Equal(Core.Enums.ReservationStatus.Cancelled, reservation.Status);
            Assert.All(reservation.Tickets, t =>
                Assert.Equal(Core.Enums.TicketStatus.Cancelled, t.Status));
            _reservationMock.Verify(r => r.UpdateAsync(100, It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task CancelReservationAsync_ReservationNotPending_ReturnsBadRequest()
        {
            var reservation = new Reservation
            {
                Id = 100,
                Status = Core.Enums.ReservationStatus.Confirmed,
                UserId = "test-user-id",
                Tickets = [new Ticket { Id = 1, Status = Core.Enums.TicketStatus.Paid }]
            };

            _reservationMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(reservation);

            var result = await _service.CancelReservationAsync(100, "test-user-id", false);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Only pending reservations can be cancelled.", result.ErrorMessage);
            _reservationMock.Verify(r => r.UpdateAsync(100, It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ReservationDoesNotExist_ReturnsNotFound()
        {
            _reservationMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Reservation?)null);

            var result = await _service.GetByIdAsync(999, "any-user", false);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Reservation not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsAdmin_ReturnsSuccess()
        {
            var reservation = new Reservation
            {
                Id = 100,
                Status = Core.Enums.ReservationStatus.Pending,
                UserId = "someone-else",
                MovieSession = new MovieSession
                {
                    Movie = new Movie { Title = "Inception" },
                    Screen = new Screen { ScreenNumber = 5 }
                },
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Id = 1,
                        Status = Core.Enums.TicketStatus.Pending,
                        Seat = new Seat { Id = 10, SeatNumber = "A1" }
                    }
                }
            };

            _reservationMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(reservation);

            var result = await _service.GetByIdAsync(100, "any-user", true);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsOwner_ReturnsSuccess()
        {
            var reservation = new Reservation
            {
                Id = 100,
                Status = Core.Enums.ReservationStatus.Pending,
                UserId = "test-owner-id",
                MovieSession = new MovieSession
                {
                    Movie = new Movie { Title = "Inception" },
                    Screen = new Screen { ScreenNumber = 5 }
                },
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Id = 1,
                        Status = Core.Enums.TicketStatus.Pending,
                        Seat = new Seat { Id = 10, SeatNumber = "A1" }
                    }
                }
            };

            _reservationMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(reservation);

            var result = await _service.GetByIdAsync(100, "test-owner-id", false);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByIdAsync_UserIsNotOwnerAndNotAdmin_ReturnsNotFound()
        {
            var reservation = new Reservation
            {
                Id = 100,
                Status = Core.Enums.ReservationStatus.Pending,
                UserId = "original-owner",
                Tickets = [new Ticket { Id = 1, Status = Core.Enums.TicketStatus.Pending }]
            };

            _reservationMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(reservation);

            var result = await _service.GetByIdAsync(100, "hacker-user", false);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Reservation not found.", result.ErrorMessage);
        }
    }
}
