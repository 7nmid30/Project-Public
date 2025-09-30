using Microsoft.AspNetCore.Authentication.JwtBearer; //�F�؊֘A
using Microsoft.AspNetCore.Identity; //�F�؊֘A
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; //�F�؊֘A
using Project.Models; //�ǉ�
using Project.Services.Interfaces;
using Project.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DB�ڑ�
var connectionString = builder.Configuration.GetConnectionString("ProjectDBConnectionString");
//builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ProjectDBContext>(options => options.UseNpgsql(connectionString));

//��������`�F�؊֘A
// Identity�T�[�r�X�̒ǉ�
builder.Services.AddIdentity<IdentityUser, IdentityRole>( //IdentityUser: ���[�U�[����\���f�t�H���g�̃N���X,IdentityRole: ���[�U�[�̃��[���i�����j��\���f�t�H���g�̃N���X
    options =>
    {
        options.Password.RequireDigit = false; // ������K�{�ɂ���i�f�t�H���g: true�j
        options.Password.RequireUppercase = false; // �啶����K�{�ɂ��Ȃ�
        options.Password.RequireNonAlphanumeric = false; // �L����K�{�ɂ��Ȃ�
    })
    .AddEntityFrameworkStores<ProjectDBContext>() //ASP.NET Core Identity��ProjectDBContext���g�p����悤�ɐݒ�
    .AddDefaultTokenProviders(); //�p�X���[�h���Z�b�g�⃁�[���m�F�ȂǂŎg�p����g�[�N�������@�\��L����

// JWT�F�؂̐ݒ�
var jwtKey = builder.Configuration["Jwt:Key"]; // appsettings.json���Őݒ肷��
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
//�����܂Ł`�F�؊֘A

//�T�[�r�X�o�^
builder.Services.AddScoped<IFavoriteRestaurantService, FavoriteRestaurantService>();
builder.Services.AddScoped<IVisitedRestaurantService, VisitedRestaurantService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

// Add services to the container.=MVC�T�[�r�X�̒ǉ�
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================================================
// �����ŃX�R�[�v���쐬���ADatabase.Migrate() �����s
// dotnet ef database update�͕s�v�ɂȂ�
// ================================================
using (var scope = app.Services.CreateScope())//�A�v���̋N�����ɂ̓��[�U�[����́uHTTP���N�G�X�g�v�����݂��Ȃ�����AScoped��HTTP���N�G�X�g���Ƃ̃C���X�^���X�𐶐�����
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDBContext>();//ProjectDBContext ���擾���郁�\�b�h
    dbContext.Database.Migrate();//���K�p�̃}�C�O���[�V������DB�ɓK�p���A�X�L�[�}���ŐV��ԂɃA�b�v�f�[�g���鏈��
}

// Configure the HTTP request pipeline.=HTTP���N�G�X�g�p�C�v���C���̍\��
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.=�{�Ԋ�������HSTS�̐ݒ�
    app.UseHsts();
}

//app.UseHttpsRedirection();
//�{�Ԃ� Nginx ���� HTTPS ������������
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

//��������`�F�؊֘A
// �F�؂ƔF�~�h���E�F�A�̒ǉ�
app.UseAuthentication();
app.UseAuthorization();
//�����܂Ł`�F�؊֘A

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
