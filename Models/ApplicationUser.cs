using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        // 関連プロパティ
        public ICollection<UserFavoriteRestaurant> UserFavoriteRestaurant { get; set; }
        public ICollection<UserVisitedRestaurant> UserVisitedRestaurant { get; set; }
        public ICollection<UserReviewRestaurant> UserReviewRestaurant { get; set; }
    }
}
