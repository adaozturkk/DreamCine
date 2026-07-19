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
    }
}
