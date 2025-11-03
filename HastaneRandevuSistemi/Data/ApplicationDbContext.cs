using HastaneRandevuSistemi.Models; 
using Microsoft.EntityFrameworkCore; 


namespace HastaneRandevuSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor (Yapıcı Metot)
        // Bu metot, Program.cs dosyasından veritabanı ayarlarını (options) alacak
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanında oluşturulacak tablolarımızı DbSet olarak tanımlıyoruz

        public DbSet<Departman> Departmanlar { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<CalismaSaatleri> CalismaSaatleri { get; set; }
    }
}
