using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace HastaneRandevuSistemi.Models
{
    public class Hasta
    {
        [Key]
        public int HastaID { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50)]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
        public string TCKimlikNo { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email zorunludur.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(256)] // Hash'lenmiş şifre tutulacak
        public string Sifre { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        // Bir hastanın birden fazla randevusu olabilir
        public virtual ICollection<Randevu> Randevular { get; set; }


    }
}
