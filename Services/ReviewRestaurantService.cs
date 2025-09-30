using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class ReviewRestaurantService : IReviewRestaurantService
    {
        private readonly ProjectDBContext _context;

        public ReviewRestaurantService(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserReviewRestaurant>> GetReviewListAsync(string userId)
        {
            try
            {
                return await _context.UserReviewRestaurants
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<UserReviewRestaurant> GetReviewedRestaurantAsync(string userId,int restaurantId)
        {
            try
            {
                return await _context.UserReviewRestaurants
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.RestaurantId == restaurantId);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        //レビューは塗り替え可能だから存在チェック不要？
        public async Task<bool> ExistsReviewAsync(string userId, int restaurantId)
        {
            try
            {
                //AnyAsync は「存在するかどうか」を直接返してくれる
                return await _context.UserReviewRestaurants
                    .AnyAsync(x => x.UserId == userId && x.RestaurantId == restaurantId);
            }
            catch(Exception ex)
            {
                throw;
            }
                  
        }

        public async Task AddReviewAsync(string userId, Restaurant restaurant, ReviewDto review)
        {
            // DB登録処理
            try
            {
                // 新規レビューのデータを作成
                UserReviewRestaurant userReviewRestaurant = new UserReviewRestaurant
                {
                    UserId = userId,
                    RestaurantId = restaurant.Id,
                    RestaurantName = restaurant.Name,
                    TotalScore = review.Score,
                    Taste = review.Taste,
                    CostPerformance = review.CostPerformance,
                    Service = review.Service,
                    Atmosphere = review.Atmosphere,
                    Comment = review.Comment,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.UserReviewRestaurants.Add(userReviewRestaurant);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task DeleteReviewAsync(string userId, Restaurant restaurant)
        {
            try
            {
                // DB削除処理
                var userReviewRestaurant = await _context.UserReviewRestaurants
                   .FirstOrDefaultAsync(x => x.UserId == userId && x.RestaurantId == restaurant.Id);

                if (userReviewRestaurant != null)
                {
                    _context.UserReviewRestaurants.Remove(userReviewRestaurant);
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
