using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class FavoriteRestaurantService : IFavoriteRestaurantService
    {
        private readonly ProjectDBContext _context;

        public FavoriteRestaurantService(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserFavoriteRestaurant>> GetFavoriteAsync(string userId)
        {
            try
            {
                return await _context.UserFavoriteRestaurants
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> ExistsFavoriteAsync(string userId, int restaurantId)
        {
            try
            {
                //AnyAsync は「存在するかどうか」を直接返してくれる
                return await _context.UserFavoriteRestaurants
                    .AnyAsync(x => x.UserId == userId && x.RestaurantId == restaurantId);
            }
            catch(Exception ex)
            {
                throw;
            }
                  
        }

        public async Task AddFavoriteAsync(string userId, Restaurant restaurant)
        {
            // DB登録処理
            try
            {
                // 新規お気に入り登録のデータを作成
                UserFavoriteRestaurant userFavoriteRestaurant = new UserFavoriteRestaurant
                {
                    UserId = userId,
                    RestaurantId = restaurant.Id,
                    RestaurantName = restaurant.Name,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.UserFavoriteRestaurants.Add(userFavoriteRestaurant);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task DeleteFavoriteAsync(string userId, Restaurant restaurant)
        {
            try
            {
                // DB削除処理
                var userFavoriteRestaurant = await _context.UserFavoriteRestaurants
                   .FirstOrDefaultAsync(x => x.UserId == userId && x.RestaurantId == restaurant.Id);

                if (userFavoriteRestaurant != null)
                {
                    _context.UserFavoriteRestaurants.Remove(userFavoriteRestaurant);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    }
}
