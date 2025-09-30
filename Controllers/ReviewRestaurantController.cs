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
using Project.Services;

namespace Project.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewRestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IReviewRestaurantService _reviewRestaurantService;

        public ReviewRestaurantController(
            IRestaurantService restaurantService,
            IReviewRestaurantService reviewRestaurantService

        )
        {
            _restaurantService = restaurantService;
            _reviewRestaurantService = reviewRestaurantService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetReviewList()
        {
            try
            {
                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var userReviewRestaurants = await _reviewRestaurantService.GetReviewListAsync(userId);

                return Ok(new {userReviewRestaurants });
            }
            catch (Exception ex)
            {
                // 必要に応じて例外をログ出力
                return StatusCode(500, new { error = "取得処理に失敗しました。", details = ex.Message });
            }

        }

        [HttpPost("get")]
        public async Task<IActionResult> GetReviewedRestaurant([FromBody] ApplePlaceDto applePlace)
        {
            try
            {
                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //Restaurantテーブルにデータを入れる
                Restaurant restaurant = await _restaurantService.UpSertRestaurantsAsync(applePlace);
                //ユーザーIDとレストランIDでレビュー情報を取得する
                var userReviewedRestaurant = await _reviewRestaurantService.GetReviewedRestaurantAsync(userId, restaurant.Id);

                return Ok(userReviewedRestaurant);
            }
            catch (Exception ex)
            {
                // 必要に応じて例外をログ出力
                return StatusCode(500, new { error = "取得処理に失敗しました。", details = ex.Message });
            }

        }


        // POST: /UserReviewRestaurant/add
        // いったことあるレストランを追加するエンドポイント
        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto review)
        {
            try
            {
                if (string.IsNullOrEmpty(review.applePlace.Name))
                {
                    return BadRequest(new { error = "無効なキーワードです。" });
                }

                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //Restaurantテーブルにデータを入れる
                Restaurant restaurant = await _restaurantService.UpSertRestaurantsAsync(review.applePlace);

                await _reviewRestaurantService.AddReviewAsync(userId, restaurant, review);

                return Ok();
            }
            catch (Exception ex)
            {
                // 必要に応じて例外をログ出力
                return StatusCode(500, new { error = "レビュー登録処理に失敗しました。", details = ex.Message });
            }
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

        // POST: /UserReviewRestaurant/get
        [HttpPost("get")]
        public async Task<IActionResult> Get([FromBody] SearchRequest request)
        {
            string placeName = request.Keyword;

            if (User.Identity.IsAuthenticated)
            {
                // 認証済みの場合、ClaimsからユーザーIDを取得
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    UserReviewRestaurant userReviewRestaurant = await _context.UserReviewRestaurants.FirstOrDefaultAsync(x => x.UserId == userId && x.RestaurantName == placeName);
                    //return Ok(new { restaurants = userFavoriteRestaurants }); {restaurants: Array(0)}のようにオブジェクトになってしまう
                    return Ok(userReviewRestaurant);
                    //return Ok();
                }
                catch (Exception ex)
                {
                    // ログ出力やエラーハンドリングを適宜実施
                    return StatusCode(500, new { error = "サーバーエラーが発生しました: " + ex.Message });
                }
            }
            else
            {
                return StatusCode(401, new { error = "認証に失敗しました。" });
            }

        }

        [HttpGet("getreviewedlist")]
        public async Task<IActionResult> Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                // 認証済みの場合、ClaimsからユーザーIDを取得
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    UserReviewRestaurant userReviewRestaurant = await _context.UserReviewRestaurants.FirstOrDefaultAsync(x => x.UserId == userId);
                    //return Ok(new { restaurants = userFavoriteRestaurants }); {restaurants: Array(0)}のようにオブジェクトになってしまう
                    return Ok(new { userReviewRestaurant });
                    //return Ok();
                }
                catch (Exception ex)
                {
                    // ログ出力やエラーハンドリングを適宜実施
                    return StatusCode(500, new { error = "サーバーエラーが発生しました: " + ex.Message });
                }
            }
            else
            {
                return StatusCode(401, new { error = "認証に失敗しました。" });
            }

        }
    }
}
