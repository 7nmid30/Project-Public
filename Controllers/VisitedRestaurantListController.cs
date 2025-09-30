using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore; //ログイン中のユーザーIDはHTTPリクエストに含まれるClaimsから取得する
using Project.Services.Interfaces;

namespace Project.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VisitedRestaurantListController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public VisitedRestaurantListController(ProjectDBContext context)
        {
            _context = context;
        }

        //private readonly IVisitedRestaurantService _visitedRestaurantService;

        //public GetVisitedRestaurantsController(IVisitedRestaurantService visitedRestaurantService)
        //{
        //    _visitedRestaurantService = visitedRestaurantService;
        //}

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // 認証済みの場合、ClaimsからユーザーIDを取得
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        try
        //        {
        //            IEnumerable<UserVisitedRestaurant> userVisitedRestaurants = await _visitedRestaurantService.GetVisitedRestaurantsAsync(userId);

        //            //return Ok(new { restaurants = userFavoriteRestaurants }); {restaurants: Array(0)}のようにオブジェクトになってしまう
        //            return Ok(new { userVisitedRestaurants });
        //            //return Ok();
        //        }
        //        catch (Exception ex)
        //        {
        //            // ログ出力やエラーハンドリングを適宜実施
        //            return StatusCode(500, new { error = "サーバーエラーが発生しました: " + ex.Message });
        //        }
        //    }
        //    else
        //    {
        //        return StatusCode(401, new { error = "認証に失敗しました。" });
        //    }

        //}
        // POST: /VisitedRestaurantList/add
        // いったことあるレストランを追加するエンドポイント
        [HttpPost("add")]
        public async Task<IActionResult> AddVisited([FromBody] SearchRequest request)
        {
            // リクエストからキーワードを取得（前後の空白は削除）
            string keyword = request.Keyword.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest(new { error = "無効なキーワードです。" });
            }

            // ユーザーが認証されているかチェック
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { error = "認証に失敗しました。" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // EF Core の非同期メソッドを使って対象のレストランを取得する
            Restaurant? restaurant = null;
            const int maxRetryCount = 10;
            int retryCount = 0;
            while (restaurant == null && retryCount < maxRetryCount)
            {
                //restaurantがnullでなくなる、または10回以上繰り返したらここで抜ける
                restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Name == keyword);
                if (restaurant == null)
                {
                    retryCount++;
                    // 非同期処理のため、Task.Delayを使用（Thread.Sleepは使用しない）
                    await Task.Delay(500);
                }
                else
                {
                    break;
                }

            }

            if (restaurant == null)
            {
                return StatusCode(504, new { error = "レストランが見つからなかったため、タイムアウトしました。" });
            }

            // 新規お気に入り登録のデータを作成
            UserVisitedRestaurant userVisitedRestaurant = new UserVisitedRestaurant
            {
                UserId = userId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                CreatedAt = DateTime.UtcNow,
            };

            try
            {
                _context.UserVisitedRestaurants.Add(userVisitedRestaurant);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(AddVisited), new { message = "登録が成功しました", restaurantId = restaurant.Id });
            }
            catch (Exception ex)
            {
                // 必要に応じて例外をログ出力してください
                return StatusCode(500, new { error = "登録処理に失敗しました。", details = ex.Message });
            }
            //try
            //{
            //    // オブジェクト初期化子を使ったインスタンス化
            //    UserVisitedRestaurant userVisitedRestaurant = new UserVisitedRestaurant
            //    {
            //        //.ToString();はnullに例外を返してしまう
            //        UserId = userId,
            //        RestaurantId = RestaurantId,
            //        CreatedAt = DateTime.UtcNow // 現在の日付と時刻を設定
            //    };

            //    // DbContext を使用して、UserVisitedRestaurants モデルをデータベースに追加する
            //    _context.UserVisitedRestaurants.Add(userVisitedRestaurant);
            //    // 変更を保存してデータベースに反映する
            //    _context.SaveChanges();

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}

        }

        // DELETE: /FavoriteRestaurantList/delete
        // お気に入りレストランを削除するエンドポイント
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteVisited([FromBody] SearchRequest request)
        {
            // リクエストからキーワードを取得（前後の空白は削除）
            string keyword = request.Keyword.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest(new { error = "無効なキーワードです。" });
            }

            // ユーザーが認証されているかチェック
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { error = "認証に失敗しました。" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // EF Core の非同期メソッドを使って対象のレストランを取得する
            Restaurant? restaurant = null;
            const int maxRetryCount = 10;
            int retryCount = 0;
            while (restaurant == null && retryCount < maxRetryCount)
            {
                //restaurantがnullでなくなる、または10回以上繰り返したらここで抜ける
                restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Name == keyword);
                if (restaurant == null)
                {
                    retryCount++;
                    // 非同期処理のため、Task.Delayを使用（Thread.Sleepは使用しない）
                    await Task.Delay(500);
                }
                else
                {
                    break;
                }

            }

            if (restaurant == null)
            {
                return StatusCode(504, new { error = "レストランが見つからなかったため、タイムアウトしました。" });
            }

            var userVisitedRestaurant = await _context.UserVisitedRestaurants
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RestaurantId == restaurant.Id);

            if (userVisitedRestaurant == null)
            {
                return NotFound(new { error = "対象のレストランがマイレストランリストに見つかりませんでした。" });
            }

            try
            {
                _context.UserVisitedRestaurants.Remove(userVisitedRestaurant);
                await _context.SaveChangesAsync();
                return Ok(new { message = "削除が成功しました。" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "削除処理に失敗しました。", details = ex.Message });
            }
        }
    }
}
