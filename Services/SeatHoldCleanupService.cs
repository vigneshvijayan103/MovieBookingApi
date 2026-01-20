using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Models;

namespace MovieTicketBookingAPI.Services
{
    public class SeatHoldCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

      
        private readonly TimeSpan _holdExpiry = TimeSpan.FromMinutes(2);

        public SeatHoldCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ReleaseExpiredHoldsAsync(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ReleaseExpiredHoldsAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var expiryTime = DateTime.UtcNow - _holdExpiry;

            var expiredSeats = await context.Seats
                .Where(s =>
                    s.Status == Status.Hold &&
                    s.HoldingTime != null &&
                    s.HoldingTime < expiryTime)
                .ToListAsync(stoppingToken);

            if (!expiredSeats.Any())
                return;

            foreach (var seat in expiredSeats)
            {
                seat.Status = Status.Available;
                seat.HoldingTime = null;
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
