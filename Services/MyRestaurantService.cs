using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class MyRestaurantService : IMyRestaurantService
    {
        private readonly ProjectDBContext _context;

        public MyRestaurantService(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserFavoriteRestaurant>> GetMyRestaurantsAsync(string userId)
        {
            return await _context.UserFavoriteRestaurants
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
