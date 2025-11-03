using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HastaneRandevuSistemi.Models
{
    public class Departman
    {
        // === YENİ EKLENEN BLOK ===
        // Constructor (Yapıcı Metot)
        // Departman nesnesi her oluşturulduğunda Doktorlar listesini 
        // otomatik olarak boş bir liste olarak başlatır.
        // Bu, ModelState validation hatasını çözecektir.
        public Departman()
        {
            Doktorlar = new HashSet<Doktor>();
        }
        // =========================

        [Key]
        public int DepartmanID { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur.")]
        [StringLength(100)]
        public string DepartmanAdi { get; set; }

        // İlişki için:
        public virtual ICollection<Doktor> Doktorlar { get; set; }








    }
}
