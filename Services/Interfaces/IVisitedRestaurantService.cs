using Project.Models;

namespace Project.Services.Interfaces
{
    public interface IVisitedRestaurantService
    {
        Task<IEnumerable<UserVisitedRestaurant>> GetVisitedRestaurantsAsync(string userId);//IEnumerable<T>は、「列挙可能（enumerable）」なT型オブジェクト
    }
}
