using System;
using System.ComponentModel.DataAnnotations;

namespace HastaneRandevuSistemi.Models
{
    public class Duyuru
    {
        [Key]
        public int DuyuruID { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; } // nvarchar(MAX) olması için StringLength yok

        public DateTime YayinTarihi { get; set; } = DateTime.Now;
    }
}
