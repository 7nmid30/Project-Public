using System.ComponentModel.DataAnnotations; //[]のアノテーション使えるようになる
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Restaurants")]
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(80)] // データ注釈を追加
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(30)]
        public string Category { get; set; } = string.Empty;
        public decimal GoogleRating { get; set; }
        public decimal Rating { get; set; }
        public string ReviewCount { get; set; } = string.Empty;
        public string UserRatingTotal { get; set; } = string.Empty;
        public int KensakuSu { get; set; }

        public int StartPrice { get; set; }
        public int EndPrice { get; set; }
        public String CurrencyCode { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        //public string SnsUrl1 { get; set; } = string.Empty;
        //public string SnsUrl2 { get; set; } = string.Empty;
        //public string SnsUrl3 { get; set; } = string.Empty;
        public int Google { get; set; }
        public int Yahoo { get; set; }
        public int Tabelog { get; set; }
        public int Apple { get; set; }
        public DateTime LastUpdated { get; set; }

        // 関連プロパティ
        public ICollection<UserFavoriteRestaurant> UserFavoriteRestaurant { get; set; }　//これたしかジェネリック型で実際のオブジェクトを<>の中において渡す
        public ICollection<UserVisitedRestaurant> UserVisitedRestaurant { get; set; }　//これたしかジェネリック型で実際のオブジェクトを<>の中において渡す
        public ICollection<UserReviewRestaurant> UserReviewRestaurant { get; set; }　//これたしかジェネリック型で実際のオブジェクトを<>の中において渡す
    }
}
