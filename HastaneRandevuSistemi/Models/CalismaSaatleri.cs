using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HastaneRandevuSistemi.Models
{
    public class CalismaSaatleri
    {
        [Key]
        public int CalismaSaatiID { get; set; }

        [Required]
        public int DoktorID { get; set; }

        [ForeignKey("DoktorID")]
        public virtual Doktor Doktor { get; set; }

        [Required]
        public DayOfWeek Gun { get; set; } // DayOfWeek enum'ı (Pazartesi, Salı vb.) kullanmak daha pratiktir

        [Required]
        public TimeSpan BaslangicSaati { get; set; } // Sadece saat (örn: 09:00)

        [Required]
        public TimeSpan BitisSaati { get; set; } // Sadece saat (örn: 17:00)
    }
}
