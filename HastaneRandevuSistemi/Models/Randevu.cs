using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace HastaneRandevuSistemi.Models
{
    public class Randevu
    {
        [Key]
        public int RandevuID { get; set; }

        [Required]
        public int HastaID { get; set; }

        [ForeignKey("HastaID")]
        public virtual Hasta Hasta { get; set; }

        [Required]
        public int DoktorID { get; set; }

        [ForeignKey("DoktorID")]
        public virtual Doktor Doktor { get; set; }

        [Required]
        public DateTime RandevuTarihi { get; set; } // Tarih ve saati birlikte tutacağız

        public int Durum { get; set; } // 0=Beklemede, 1=Onaylandı, 2=Tamamlandı, 3=İptal
    }
}
