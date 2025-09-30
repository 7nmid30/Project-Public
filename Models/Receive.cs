using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ApplePlaceDto
    {
        public string Name { get; set; } = default!;

        // Swift 側は Double なので C# も double
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Url { get; set; }          // Swift の URL? は JSON では文字列になる
        public string? Address { get; set; }

        public double? Rating { get; set; }
        public int? UserRatingCount { get; set; }

        // 価格系は Swift 側が String? なので合わせて string? で受ける
        public string? StartPrice { get; set; }
        public string? EndPrice { get; set; }

        public string? CurrencyCode { get; set; }
        public string? Category { get; set; }
    }
    public class ReviewDto
    {
        public ApplePlaceDto applePlace { get; set; }           // レストラン情報
        public decimal Score { get; set; }       // 0.0～5.0 のスコア
        public int Taste { get; set; }              // 味
        public int CostPerformance { get; set; }    // コスパ
        public int Service { get; set; }            // 接客(モデル側では Hospitality にマッピング)
        public int Atmosphere { get; set; }         //雰囲気
        [MaxLength(500)] 
        public string Comment { get; set; }   // 口コミ（1000字まで）
    }
    public class SearchRequest //Postの受信用クラス。修飾子がPublicなクラスなので同じプロジェクト内からアクセスできる
    {
        public string Keyword { get; set; } = string.Empty;
        public double Latitude { get; set; } // 現在地の緯度
        public double Longitude { get; set; } // 現在地の経度
    }
}

