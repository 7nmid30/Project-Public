namespace Project.Models
{
    public class UserReviewRestaurant
    {
        public int Id { get; set; }
        public string UserId { get; set; } // 外部キー
        public int RestaurantId { get; set; } // 外部キー
        public string RestaurantName { get; set; }
        public decimal TotalScore { get; set; }
        public int Taste { get; set; }
        public int CostPerformance { get; set; }
        public int Service { get; set; }
        public int Atmosphere { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //日本時間

        // 関連プロパティ（どのテーブルと関わりがあるか）
        public ApplicationUser ApplicationUser { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
