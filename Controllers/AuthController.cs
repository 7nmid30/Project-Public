using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    [ApiController] //リクエストの自動検証などの便利な機能を提供する
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;//ASP.NET Core Identityで定義されているジェネリッククラス（IdentityUser型指定）で、ユーザーの作成・更新・削除やパスワード管理など、ユーザー関連の操作を簡単に行うためのメソッドが用意されている
        //依存性注入（Dependency Injection, DI） によって、インスタンスが自動的に生成されている
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;// DIコンテナが「ツールボックス（UserManager）（どんな素材（型）のユーザーを扱うか＝IdentityUser型）」を渡してくれるイメージ
            //UserManager は多くの依存関係を持つため、それらを DI コンテナで管理することでインスタンス生成が簡単になる。
            _configuration = configuration;
        }

        // ユーザー登録エンドポイント
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { message = "User registered successfully!" });
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        // ユーザーログインエンドポイント
        //[AllowAnonymous] これ必要なのかわからん
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) //型を指定するだけでモデルバインディング機能が裏でインスタンス生成してる
        {
            //[FromBody]リクエストボディ（Body）に含まれるJSONデータを、自動的に LoginModel 型のインスタンスに変換する機能を指定
            var user = await _userManager.FindByEmailAsync(model.Email); //_userManager
            var hasPassword = await _userManager.HasPasswordAsync(user);
            var var = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        // JWTトークン生成メソッド
        private string GenerateJwtToken(IdentityUser user)
        {
            //JWTのPayload に含まれるデータで、トークンに格納したいユーザー固有の情報
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));//JWTの署名（Signature）を生成するための秘密鍵
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//秘密鍵をつかって署名資格情報（SigningCredentials）の作成
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],//ペイロード部分
                audience: _configuration["Jwt:Audience"],//ペイロード部分
                claims: claims,//ペイロード部分
                expires: DateTime.UtcNow.AddYears(3),//ペイロード部分(サーバーがどのタイムゾーンにあっても一貫して解釈されるようにutc)
                signingCredentials: creds);//Signature（署名）部分

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // モデルクラス
    public class RegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;    
    }
}
