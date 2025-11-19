using HastaneRandevuSistemi.Data;
using HastaneRandevuSistemi.Models;
using HastaneRandevuSistemi.Helpers; // Şifreleyici için
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Session için

namespace HastaneRandevuSistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // === KAYIT OL (REGISTER) ===

        // GET: /Account/Register
        public IActionResult Register()
        {

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Hasta hasta)
        {
            ModelState.Remove("Randevular");

            // Şifre ve TC Kimlik boş değilse
            if (ModelState.IsValid)
            {
                // 1. Bu TC veya Email ile daha önce kayıt olunmuş mu kontrol et
                var varMi = _context.Hastalar.Any(h => h.TCKimlikNo == hasta.TCKimlikNo || h.Email == hasta.Email);
                if (varMi)
                {
                    ViewBag.Hata = "Bu TC Kimlik No veya E-posta zaten kayıtlı.";
                    return View(hasta);
                }

                // 2. Şifreyi Hash'le (Güvenlik)
                hasta.Sifre = Sifreleyici.ComputeSha256Hash(hasta.Sifre);

                // 3. Kaydet
                _context.Add(hasta);
                _context.SaveChanges();

                // 4. Giriş sayfasına yönlendir
                TempData["Mesaj"] = "Kaydınız başarıyla oluşturuldu. Lütfen giriş yapın.";
                return RedirectToAction("Login");
            }
            return View(hasta);
        }

        // === GİRİŞ YAP (LOGIN) ===

        // GET: /Account/Login
        public IActionResult Login()
        {

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string TCKimlikNo, string Sifre)
        {
            // Girilen şifreyi de hash'leyip veritabanındaki hash ile karşılaştıracağız
            string hashliSifre = Sifreleyici.ComputeSha256Hash(Sifre);

            var hasta = _context.Hastalar.FirstOrDefault(h =>
                h.TCKimlikNo == TCKimlikNo && h.Sifre == hashliSifre);

            if (hasta != null)
            {
                // Giriş Başarılı -> Session Oluştur
                HttpContext.Session.SetString("HastaKullaniciAdi", hasta.Ad + " " + hasta.Soyad);
                HttpContext.Session.SetInt32("HastaID", hasta.HastaID);

                // Ana sayfaya veya Randevu Paneline yönlendir
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Hata = "TC Kimlik No veya Şifre hatalı.";
                return View();
            }
        }

        // === ÇIKIŞ YAP (LOGOUT) ===
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ===== PROFİL YÖNETİMİ BAŞLANGIÇ =====

        // GET: /Account/Profil
        public async Task<IActionResult> Profil()
        {
            int? hastaId = HttpContext.Session.GetInt32("HastaID");
            if (hastaId == null) return RedirectToAction("Login");

            var hasta = await _context.Hastalar.FindAsync(hastaId);
            return View(hasta);

        }

        // POST: /Account/Profil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(Hasta gelenVeri, string? YeniSifre)
        {
            int? hastaId = HttpContext.Session.GetInt32("HastaID");
            if (hastaId == null) return RedirectToAction("Login");

            // Veritabanındaki asıl kaydı buluyoruz
            var hastaDb = await _context.Hastalar.FindAsync(hastaId);

            if (hastaDb == null) return NotFound();

            // Sadece izin verdiğimiz alanları güncelliyoruz
            hastaDb.Telefon = gelenVeri.Telefon;
            hastaDb.Email = gelenVeri.Email;

            // Eğer yeni şifre kutusu doluysa şifreyi de değiştir
            if (!string.IsNullOrEmpty(YeniSifre))
            {
                hastaDb.Sifre = Sifreleyici.ComputeSha256Hash(YeniSifre);
            }

            _context.Update(hastaDb);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = "Profil bilgileriniz güncellendi.";
            return RedirectToAction(nameof(Profil));
        }
    }
}