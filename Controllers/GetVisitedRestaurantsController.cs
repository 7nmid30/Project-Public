using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Claims;
using System;
using Project.Services.Interfaces;

namespace Project.Controllers
{
    [Route("[controller]")]//これが付いてると、コントローラークラスの名前（末尾の "Controller" を除いた部分）に置き換えられる。
    [ApiController] //付いてるクラスがAPIリクエストを処理することを示す。例えば、400エラーや404エラーなど自動的にエラー応答を返す。
    public class GetVisitedRestaurantsController : ControllerBase
    {
        //private readonly ProjectDBContext _context;

        ////コンストラクターで DbContext を注入
        //public GetFavoriteRestaurantsController(ProjectDBContext context)
        //{
        //    _context = context;
        //}

        private readonly IVisitedRestaurantService _visitedRestaurantService;

        public GetVisitedRestaurantsController(IVisitedRestaurantService visitedRestaurantService)
        {
            _visitedRestaurantService = visitedRestaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                // 認証済みの場合、ClaimsからユーザーIDを取得
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    IEnumerable<UserVisitedRestaurant> userVisitedRestaurants = await _visitedRestaurantService.GetVisitedRestaurantsAsync(userId);

                    //return Ok(new { restaurants = userFavoriteRestaurants }); {restaurants: Array(0)}のようにオブジェクトになってしまう
                    return Ok(new { userVisitedRestaurants });
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
