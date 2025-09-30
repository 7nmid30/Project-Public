using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services.Interfaces;

namespace Project.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ProjectDBContext _context;

        public RestaurantService(ProjectDBContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string userId)
        //{
        //    return await _context.Restaurants
        //        .Where(r => r.UserId == userId)
        //        .ToListAsync();
        //}

        public async Task<Restaurant> GetRestaurantByNameAsync(string name)
        {
            return await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<Restaurant> UpSertRestaurantsAsync(ApplePlaceDto applePlace)
        {
            Restaurant? existingRestaurant = null;//クラス自体（Restaurant）は参照型なのでnullを許容しますが、コンパイラはより安全なコードを促進するために、警告でるから型に?をつけてnull許容にする
            try
            {
                existingRestaurant = _context.Restaurants.FirstOrDefault(x => x.Name == applePlace.Name);
            }
            catch (Exception ex)
            {
                existingRestaurant = null;
            }
            try
            {
                //Restaurantテーブルへの登録
                if (existingRestaurant == null)
                {
                    // オブジェクト初期化子を使ったインスタンス化
                    Restaurant restaurant = new Restaurant
                    {
                        //.ToString();はnullに例外を返してしまう
                        Name = applePlace.Name,
                        Latitude = applePlace.Latitude,
                        Longitude = applePlace.Longitude,
                        PhoneNumber = applePlace.PhoneNumber,
                        Url = applePlace.Url,
                        Address = applePlace.Address,
                        Category = applePlace.Category,
                        KensakuSu = 1,
                        Apple = 1,
                        LastUpdated = DateTime.UtcNow // 現在の日付と時刻を設定
                    };

                    // DbContext を使用して、Restaurant モデルをデータベースに追加する
                    _context.Restaurants.Add(restaurant);

                    await _context.SaveChangesAsync();

                    return restaurant; // 登録したレコードの restaurantオブジェクト を返す
                }
                else
                {
                    existingRestaurant.Latitude = applePlace.Latitude;
                    existingRestaurant.Longitude = applePlace.Longitude;
                    existingRestaurant.PhoneNumber = applePlace.PhoneNumber;
                    existingRestaurant.Url = applePlace.Url;
                    existingRestaurant.Address = applePlace.Address;
                    existingRestaurant.Category = applePlace.Category;
                    existingRestaurant.KensakuSu++;
                    existingRestaurant.Apple = 1;
                    existingRestaurant.LastUpdated = DateTime.UtcNow; // 現在の日付と時刻を設定

                    _context.Restaurants.Update(existingRestaurant);

                    // 変更を保存してデータベースに反映する
                    await _context.SaveChangesAsync();

                    return existingRestaurant; // 更新したレコードの restaurantオブジェクト を返す
                }
                
            }
            catch (Exception ex)
            {
                // エラー時は -1 とか例外を投げるか選択
                Console.WriteLine(ex.Message);
                throw; // そのまま上位へ投げる方が一般的
            }
        }
    }
}
