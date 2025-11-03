using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace HastaneRandevuSistemi.Models
{
    public class Doktor
    {
        
        // Constructor (Yapıcı Metot)
        public Doktor()
        {
            Randevular = new HashSet<Randevu>();
            CalismaSaatleri = new HashSet<CalismaSaatleri>();
        }
       

        [Key]
        public int DoktorID { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50)]
        public string Soyad { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        // Departman ile İlişki (Foreign Key)
        [Required(ErrorMessage = "Departman seçimi zorunludur.")]
        public int DepartmanID { get; set; }

        [ForeignKey("DepartmanID")]
        public virtual Departman Departman { get; set; } // Navigation property

        // Bir doktorun birden fazla randevusu olabilir
        public virtual ICollection<Randevu> Randevular { get; set; }

        // Bir doktorun çalışma saatleri olabilir
        public virtual ICollection<CalismaSaatleri> CalismaSaatleri { get; set; }



    }
}
