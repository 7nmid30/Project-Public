using Microsoft.AspNetCore.Authentication.JwtBearer; //認証関連
using Microsoft.AspNetCore.Identity; //認証関連
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; //認証関連
using Project.Models; //追加
using Project.Services.Interfaces;
using Project.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DB接続
var connectionString = builder.Configuration.GetConnectionString("ProjectDBConnectionString");
//builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ProjectDBContext>(options => options.UseNpgsql(connectionString));

//ここから～認証関連
// Identityサービスの追加
builder.Services.AddIdentity<IdentityUser, IdentityRole>( //IdentityUser: ユーザー情報を表すデフォルトのクラス,IdentityRole: ユーザーのロール（役割）を表すデフォルトのクラス
    options =>
    {
        options.Password.RequireDigit = false; // 数字を必須にする（デフォルト: true）
        options.Password.RequireUppercase = false; // 大文字を必須にしない
        options.Password.RequireNonAlphanumeric = false; // 記号を必須にしない
    })
    .AddEntityFrameworkStores<ProjectDBContext>() //ASP.NET Core IdentityにProjectDBContextを使用するように設定
    .AddDefaultTokenProviders(); //パスワードリセットやメール確認などで使用するトークン生成機能を有効化

// JWT認証の設定
var jwtKey = builder.Configuration["Jwt:Key"]; // appsettings.json等で設定する
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
//ここまで～認証関連

//サービス登録
builder.Services.AddScoped<IFavoriteRestaurantService, FavoriteRestaurantService>();
builder.Services.AddScoped<IVisitedRestaurantService, VisitedRestaurantService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

// Add services to the container.=MVCサービスの追加
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================================================
// ここでスコープを作成し、Database.Migrate() を実行
// dotnet ef database updateは不要になる
// ================================================
using (var scope = app.Services.CreateScope())//アプリの起動時にはユーザーからの「HTTPリクエスト」が存在しないから、ScopedでHTTPリクエストごとのインスタンスを生成する
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDBContext>();//ProjectDBContext を取得するメソッド
    dbContext.Database.Migrate();//未適用のマイグレーションをDBに適用し、スキーマを最新状態にアップデートする処理
}

// Configure the HTTP request pipeline.=HTTPリクエストパイプラインの構成
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.=本番環境向けのHSTSの設定
    app.UseHsts();
}

//app.UseHttpsRedirection();
//本番は Nginx 側で HTTPS を処理させる
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

//ここから～認証関連
// 認証と認可ミドルウェアの追加
app.UseAuthentication();
app.UseAuthorization();
//ここまで～認証関連

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
