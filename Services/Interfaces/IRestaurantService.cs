using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IRestaurantService
    {
        //Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string userId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト
        Task<Restaurant> GetRestaurantByNameAsync(string name);
        Task<Restaurant> UpSertRestaurantsAsync(ApplePlaceDto applePlace);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト
    }
}
