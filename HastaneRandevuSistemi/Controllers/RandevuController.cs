using HastaneRandevuSistemi.Data;
using HastaneRandevuSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // Session için
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için

namespace HastaneRandevuSistemi.Controllers
{
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Randevu/Index (RANDEVULARIM)
        // Hastanın kendi geçmiş/gelecek randevularını listeler
        public async Task<IActionResult> Index()
        {

            int? hastaId = HttpContext.Session.GetInt32("HastaID");
            if (hastaId == null) return RedirectToAction("Login", "Account");

            var randevular = await _context.Randevular
                .Include(r => r.Doktor)
                .ThenInclude(d => d.Departman) // Randevudaki doktorun departmanını da getir
                .Where(r => r.HastaID == hastaId) // Sadece giriş yapan hastanın randevuları
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            return View(randevular);
        }

        // GET: /Randevu/RandevuAl (Adım 1: Formu Göster)
        public async Task<IActionResult> RandevuAl()
        {

            if (HttpContext.Session.GetInt32("HastaID") == null) return RedirectToAction("Login", "Account");

            // Formda kullanılacak Departman listesini ViewBag'e atıyoruz
            ViewBag.Departmanlar = new SelectList(await _context.Departmanlar.ToListAsync(), "DepartmanID", "DepartmanAdi");

            return View();
        }

        // AJAX İÇİN YARDIMCI METOT 1: Departmana göre Doktorları getir
        // Sayfa yenilenmeden (JavaScript ile) çağrılacak
        public JsonResult DoktorlariGetir(int departmanId)
        {
            var doktorlar = _context.Doktorlar
                .Where(d => d.DepartmanID == departmanId)
                .Select(d => new { d.DoktorID, AdSoyad = d.Ad + " " + d.Soyad })
                .ToList();
            return Json(doktorlar);
        }

        // POST: /Randevu/RandevuOlustur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RandevuOlustur(int DoktorID, DateTime RandevuTarihi, string RandevuSaati)
        {
            int? hastaId = HttpContext.Session.GetInt32("HastaID");
            if (hastaId == null) return RedirectToAction("Login", "Account");

            // Randevu saatini TimeSpan'a çevir
            TimeSpan secilenSaat = TimeSpan.Parse(RandevuSaati);
            DateTime tamTarih = RandevuTarihi.Date + secilenSaat;

            // === KONTROL 1: GEÇMİŞ ZAMAN KONTROLÜ ===
            if (tamTarih < DateTime.Now)
            {
                TempData["Hata"] = "Geçmiş bir tarihe randevu alamazsınız.";
                return RedirectToAction("RandevuAl");
            }

            // === KONTROL 2: DOKTOR O GÜN ÇALIŞIYOR MU? ===
            // Seçilen tarihin hangi gün olduğunu bulalım (Pazartesi, Salı vs.)
            DayOfWeek secilenGun = RandevuTarihi.DayOfWeek;

            // Veritabanından doktorun o günkü çalışma saatini çek
            var calismaPlani = await _context.CalismaSaatleri
                .FirstOrDefaultAsync(c => c.DoktorID == DoktorID && c.Gun == secilenGun);

            if (calismaPlani == null)
            {
                // Eğer kayıt gelmezse, doktor o gün çalışmıyor demektir.
                // Gün isimlerini Türkçe göstermek için basit bir çeviri
                string[] gunler = { "Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi" };
                string gunAdi = gunler[(int)secilenGun];

                TempData["Hata"] = $"Seçilen doktor {gunAdi} günleri çalışmamaktadır.";
                return RedirectToAction("RandevuAl");
            }

            // === KONTROL 3: SAAT ARALIĞI KONTROLÜ ===
            // Doktor o gün çalışıyor ama seçilen saat mesai içinde mi?
            if (secilenSaat < calismaPlani.BaslangicSaati || secilenSaat > calismaPlani.BitisSaati)
            {
                TempData["Hata"] = $"Doktorun çalışma saatleri: {calismaPlani.BaslangicSaati:hh\\:mm} - {calismaPlani.BitisSaati:hh\\:mm}";
                return RedirectToAction("RandevuAl");
            }

            // === KONTROL 4: DOLULUK KONTROLÜ ===
            // O gün ve o saatte başka bir randevu var mı?
            bool doluMu = await _context.Randevular.AnyAsync(r =>
                r.DoktorID == DoktorID &&
                r.RandevuTarihi == tamTarih &&
                r.Durum != 3); // İptal edilenler (Durum 3) hariç, aktif randevulara bak

            if (doluMu)
            {
                TempData["Hata"] = "Seçilen saatte doktorun başka bir randevusu mevcut. Lütfen başka bir saat seçiniz.";
                return RedirectToAction("RandevuAl");
            }

            // === HER ŞEY TAMAM, KAYDET ===
            var yeniRandevu = new Randevu
            {
                DoktorID = DoktorID,
                HastaID = hastaId.Value,
                RandevuTarihi = tamTarih,
                Durum = 1 // Onaylandı
            };

            _context.Add(yeniRandevu);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = "Randevunuz başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }
        // GET: /Randevu/RandevuIptal/5
        public async Task<IActionResult> RandevuIptal(int id)
        {
            int? hastaId = HttpContext.Session.GetInt32("HastaID");
            if (hastaId == null) return RedirectToAction("Login", "Account");

            // Randevuyu bul
            var randevu = await _context.Randevular.FindAsync(id);

            if (randevu == null)
            {
                return NotFound();
            }

            // GÜVENLİK KONTROLÜ: 
            // Başkasının randevusunu iptal etmeye çalışıyor olabilir mi?
            // Sadece giriş yapan hasta (Session'daki ID) kendi randevusunu iptal edebilir.
            if (randevu.HastaID != hastaId)
            {
                return Unauthorized(); // Yetkisiz işlem
            }

            // Geçmiş randevular iptal edilemez mantığı (İsteğe bağlı)
            if (randevu.RandevuTarihi < DateTime.Now)
            {
                TempData["Hata"] = "Geçmiş tarihli randevular iptal edilemez.";
                return RedirectToAction("Index");
            }

            // Durumu 3 (İptal) yapıyoruz
            randevu.Durum = 3;

            _context.Update(randevu);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = "Randevunuz başarıyla iptal edildi.";
            return RedirectToAction("Index");
        }
    }
}