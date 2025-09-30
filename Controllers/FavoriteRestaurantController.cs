using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Project.Services.Interfaces; //ログイン中のユーザーIDはHTTPリクエストに含まれるClaimsから取得する

namespace Project.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FavoriteRestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IFavoriteRestaurantService _favoriteRestaurantService;

        public FavoriteRestaurantController(
            IRestaurantService restaurantService,
            IFavoriteRestaurantService favoriteRestaurantService

        )
        {
            _restaurantService = restaurantService;
            _favoriteRestaurantService = favoriteRestaurantService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetFavorite()
        {
            try
            {
                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //このやり方のように配列をそのまま返してもいいが、将来メタ情報（件数、エラーメッセージ、ページング情報など）を付け足しにくい
                //var favorites = await _favoriteRestaurantService.GetFavoriteAsync(userId);               
                //if (favorites == null || !favorites.Any())
                //{
                //    //return NotFound(new { error = "対象ユーザーのお気に入りレストランが見つかりませんでした。" });
                //}
                //return Ok(favorites);

                var userFavoriteRestaurants = await _favoriteRestaurantService.GetFavoriteAsync(userId);
                
                return Ok(new { userFavoriteRestaurants });
            }
            catch(Exception ex)
            {
                // 必要に応じて例外をログ出力
                return StatusCode(500, new { error = "取得処理に失敗しました。", details = ex.Message });
            }
            
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFavorite([FromBody] ApplePlaceDto applePlace)
        {
            try
            {
                // 基本バリデーション（必要に応じてカスタム）
                if (string.IsNullOrWhiteSpace(applePlace.Name))
                    return BadRequest(new { error = "name は必須です。" });

                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //Restaurantテーブルにデータを入れる
                Restaurant restaurant = await _restaurantService.UpSertRestaurantsAsync(applePlace);

                //UserFavoriteRestaurantテーブルでまだお気に入りされてなければデータを入れる
                if (await _favoriteRestaurantService.ExistsFavoriteAsync(userId, restaurant.Id))
                {
                    //409 Conflict =「リソースの現在の状態と競合している」場合に使う。
                    return Conflict(new { error = "このレストランは既にマイレストランリストに登録されています。" });
                }

                await _favoriteRestaurantService.AddFavoriteAsync(userId, restaurant);

                return Ok();
            }
            catch (Exception ex)
            {
                // 必要に応じて例外をログ出力
                return StatusCode(500, new { error = "登録処理に失敗しました。", details = ex.Message });
            }
        }


        // DELETE: /FavoriteRestaurantList/delete
        // お気に入りレストランを削除するエンドポイント
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFavorite([FromBody] ApplePlaceDto applePlace)
        {
            try
            {
                // 基本バリデーション（必要に応じてカスタム）
                if (string.IsNullOrWhiteSpace(applePlace.Name))
                    return BadRequest(new { error = "name は必須です。" });

                // ユーザーが認証されているかチェック
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { error = "認証に失敗しました。" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //削除対象のレストランの情報をまずRestaurantテーブルから取得する
                Restaurant restaurant = await _restaurantService.GetRestaurantByNameAsync(applePlace.Name);

                //ユーザーの気に入りリストから対象のレストランを外す
                await _favoriteRestaurantService.DeleteFavoriteAsync(userId, restaurant);

                return Ok(new { message = "削除が成功しました。" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "削除処理に失敗しました。", details = ex.Message });
            }
        }
    }
}
