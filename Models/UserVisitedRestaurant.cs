namespace Project.Models
{
    public class UserVisitedRestaurant
    {
        public int Id { get; set; }
        public string UserId { get; set; } // 外部キー
        public int RestaurantId { get; set; } // 外部キー
        public string RestaurantName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //日本時間

        // 関連プロパティ（どのテーブルと関わりがあるか）
        public ApplicationUser ApplicationUser { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
