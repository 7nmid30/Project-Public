using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IFavoriteRestaurantService
    {
        Task<IEnumerable<UserFavoriteRestaurant>> GetFavoriteAsync(string userId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト

        Task<bool> ExistsFavoriteAsync(string userId, int restaurantId);
        Task AddFavoriteAsync(string userId,Restaurant restaurant);//

        Task DeleteFavoriteAsync(string userId, Restaurant restaurant);
    }
}
