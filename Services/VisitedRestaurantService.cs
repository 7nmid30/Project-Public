using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class VisitedRestaurantService : IVisitedRestaurantService
    {
        private readonly ProjectDBContext _context;

        public VisitedRestaurantService(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserVisitedRestaurant>> GetVisitedRestaurantsAsync(string userId)
        {
            return await _context.UserVisitedRestaurants
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
