using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    //public class ProjectDBContext : DbContext
    //{
    //    public ProjectDBContext(DbContextOptions<ProjectDBContext> options) : base(options) { }
    //    //public DbSet<GoogleRestaurant> GoogleRestaurants { get; set; }
    //    public DbSet<Restaurant> Restaurants { get; set; }
    //}
    public class ProjectDBContext : IdentityDbContext
    {
        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }

        // 他のエンティティ（例: Restaurants）があればDbSetとして追加
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<UserFavoriteRestaurant> UserFavoriteRestaurants { get; set; }
        public DbSet<UserVisitedRestaurant> UserVisitedRestaurants { get; set; }
        public DbSet<UserReviewRestaurant> UserReviewRestaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity関連のモデル設定を先に実行
            base.OnModelCreating(modelBuilder);

            // Restaurantsなどカスタムエンティティの設定
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.Rating).HasPrecision(10, 2); // 小数点以下2桁
            });

            // UserFavoriteRestaurant
            // 中間テーブルの設定
            modelBuilder.Entity<UserFavoriteRestaurant>()
                .HasKey(ufr => ufr.Id);　//主キーをIdに設定 //UserFavoriteRestaurant

            modelBuilder.Entity<UserFavoriteRestaurant>()
                .HasOne(ufr => ufr.ApplicationUser) //UserFavoriteRestaurant が1人のユーザー（ApplicationUser）に関連していることを指定=同じユーザーはいない
                .WithMany(au => au.UserFavoriteRestaurant) //1人のユーザー(ApplicationUser)が複数のUserFavoriteRestaurantレコードを持つことを指定
                .HasForeignKey(ufr => ufr.UserId) //UserFavoriteRestaurantのUserId プロパティが外部キーとして ApplicationUser.Id に関連付けられることを指定
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のUserIdが削除されたら削除するようにする

            modelBuilder.Entity<UserFavoriteRestaurant>()
                .HasOne(ufr => ufr.Restaurant) //UserFavoriteRestaurantが1つのレストラン（Restaurant） に関連していることを指定=同じレストランはいない
                .WithMany(r => r.UserFavoriteRestaurant) //1つのレストランが複数の UserFavoriteRestaurant レコードを持つことを指定
                .HasForeignKey(ufr => ufr.RestaurantId) //UserFavoriteRestaurantのRestaurantIdプロパティが外部キーとして Restaurant.Id に関連付けられることを指定
                .HasPrincipalKey(r => r.Id) // Restaurant の Id を参照することを明示（一般的に主キーを参照するから省略可）
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のIdが削除されたら削除するようにする
            
            //UserVisitedRestaurant
            // 中間テーブルの設定
            modelBuilder.Entity<UserVisitedRestaurant>()
                .HasKey(uvr => uvr.Id);　//主キーをIdに設定 //UserVisitedRestaurant

            modelBuilder.Entity<UserVisitedRestaurant>()
                .HasOne(uvr => uvr.ApplicationUser) //UserVisitedRestaurant が1人のユーザー（ApplicationUser）に関連していることを指定=同じユーザーはいない
                .WithMany(au => au.UserVisitedRestaurant) //1人のユーザー(ApplicationUser)が複数のUserVisitedRestaurantレコードを持つことを指定
                .HasForeignKey(uvr => uvr.UserId) //UserVisitedRestaurantのUserId プロパティが外部キーとして ApplicationUser.Id に関連付けられることを指定
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のUserIdが削除されたら削除するようにする

            modelBuilder.Entity<UserVisitedRestaurant>()
                .HasOne(uvr => uvr.Restaurant) //UserVisitedRestaurantが1つのレストラン（Restaurant） に関連していることを指定=同じレストランはいない
                .WithMany(r => r.UserVisitedRestaurant) //1つのレストランが複数の UserVisitedRestaurant レコードを持つことを指定
                .HasForeignKey(uvr => uvr.RestaurantId) //UserVisitedRestaurantのRestaurantIdプロパティが外部キーとして Restaurant.Id に関連付けられることを指定
                .HasPrincipalKey(r => r.Id) // Restaurant の Id を参照することを明示（一般的に主キーを参照するから省略可）
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のIdが削除されたら削除するようにする

            //UserReviewRestaurant
            // 中間テーブルの設定
            modelBuilder.Entity<UserReviewRestaurant>()
                .HasKey(uvr => uvr.Id);　//主キーをIdに設定 //UserReviewRestaurant

            modelBuilder.Entity<UserReviewRestaurant>()
                .HasOne(uvr => uvr.ApplicationUser) //UserReviewRestaurant が1人のユーザー（ApplicationUser）に関連していることを指定=同じユーザーはいない
                .WithMany(au => au.UserReviewRestaurant) //1人のユーザー(ApplicationUser)が複数のUserReviewRestaurantレコードを持つことを指定
                .HasForeignKey(uvr => uvr.UserId) //UserReviewRestaurantのUserId プロパティが外部キーとして ApplicationUser.Id に関連付けられることを指定
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のUserIdが削除されたら削除するようにする

            modelBuilder.Entity<UserReviewRestaurant>()
                .HasOne(uvr => uvr.Restaurant) //UserReviewRestaurantが1つのレストラン（Restaurant） に関連していることを指定=同じレストランはいない
                .WithMany(r => r.UserReviewRestaurant) //1つのレストランが複数の UserReviewRestaurant レコードを持つことを指定
                .HasForeignKey(uvr => uvr.RestaurantId) //UserReviewRestaurantのRestaurantIdプロパティが外部キーとして Restaurant.Id に関連付けられることを指定
                .HasPrincipalKey(r => r.Id) // Restaurant の Id を参照することを明示（一般的に主キーを参照するから省略可）
                .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADEを設定　親テーブルで該当のIdが削除されたら削除するようにする
        }
    }
}
