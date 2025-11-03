using System.ComponentModel.DataAnnotations;



namespace HastaneRandevuSistemi.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(256)] // Hash'lenmiş şifre tutulacak
        public string Sifre { get; set; }
    }
}
