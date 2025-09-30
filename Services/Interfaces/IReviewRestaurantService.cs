using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IReviewRestaurantService
    {
        Task<IEnumerable<UserReviewRestaurant>> GetReviewListAsync(string userId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト
        Task<UserReviewRestaurant> GetReviewedRestaurantAsync(string userId, int restaurantId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト

        Task<bool> ExistsReviewAsync(string userId, int restaurantId);
        Task AddReviewAsync(string userId, Restaurant restaurant, ReviewDto review);//

        Task DeleteReviewAsync(string userId, Restaurant restaurant);
    }
}
