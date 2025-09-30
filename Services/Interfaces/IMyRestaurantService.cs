using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IMyRestaurantService
    {
        Task<IEnumerable<UserFavoriteRestaurant>> GetMyRestaurantsAsync(string userId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト
    }
}
