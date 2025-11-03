using Microsoft.EntityFrameworkCore;
using HastaneRandevuSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using HastaneRandevuSistemi.Models;

namespace HastaneRandevuSistemi.Controllers
{
    public class AdminController : Controller
    {
        // Veritabanı bağlantı nesnemiz
        private readonly ApplicationDbContext _context;

        // Constructor (Yapıcı Metot) - DbContext'i projeden isteme (Dependency Injection)
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: /Admin/Index
        public IActionResult Index()
        {
            // Session kontrolü (Eğer giriş yapılmamışsa Login'e yönlendir)
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.ToplamDoktor = _context.Doktorlar.Count();
            ViewBag.ToplamHasta = _context.Hastalar.Count();
            ViewBag.ToplamRandevu = _context.Randevular.Count();
            ViewBag.ToplamDepartman = _context.Departmanlar.Count();

            return View();
        }

        // GET: /Admin/Login
        // Admin Giriş sayfasını gösterir
        public IActionResult Login()
        {
            return View();
        }

        
        // POST: /Admin/Login
        [HttpPost] // Bu metodun form gönderildiğinde (POST) çalışacağını belirtir
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public IActionResult Login(string KullaniciAdi, string Sifre)
        {
            // Views/Admin/Login.cshtml içindeki formda name="KullaniciAdi" ve name="Sifre"
            // olan inputlardaki veriler buraya otomatik olarak gelir.

            
            var admin = _context.Adminler.FirstOrDefault(a =>
                a.KullaniciAdi == KullaniciAdi && a.Sifre == Sifre);

            if (admin != null)
            {
                // 2. Kullanıcı bulunduysa, Session (Oturum) başlat
                HttpContext.Session.SetString("AdminKullaniciAdi", admin.KullaniciAdi);
                HttpContext.Session.SetInt32("AdminID", admin.AdminID);

                // 3. Admin Dashboard'a yönlendir
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // 4. Kullanıcı bulunamadıysa, hata mesajı ver ve Login sayfasını tekrar göster
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View(); // Login.cshtml'i tekrar gösterir
            }
        }
        
   
        // GET: /Admin/Logout
        public IActionResult Logout()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();
            // Login sayfasına yönlendir
            return RedirectToAction("Login", "Admin");
        }
        // ===========================================

        // GET: /Admin/DepartmanYonetimi
        // Mevcut departmanları listeler
        public async Task<IActionResult> DepartmanYonetimi()
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            // Veritabanındaki tüm departmanları "Departmanlar" tablosundan çek
            var departmanlar = await _context.Departmanlar.ToListAsync();

            // Listeyi View'e model olarak gönder
            return View(departmanlar);
        }

        // GET: /Admin/DepartmanEkle
        // Yeni departman ekleme formunu gösterir
        public IActionResult DepartmanEkle()
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            return View(); // Sadece formu göster
        }

        // POST: /Admin/DepartmanEkle
        // Formdan gelen veriyi veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DepartmanEkle(Departman departman)
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            // Model geçerliyse (DepartmanAdi boş değilse vb.)
            if (ModelState.IsValid)
            {
                _context.Add(departman); // Yeni departmanı DbContext'e ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet

                // Başarılı olursa liste sayfasına geri yönlendir
                return RedirectToAction(nameof(DepartmanYonetimi));
            }

            // Model geçerli değilse, formu tekrar göster (hata mesajlarıyla)
            return View(departman);
        }

        // ===========================================

        // DÜZENLEME (GET) 
        // GET: /Admin/DepartmanDuzenle/5 (örn: 5 ID'li departmanı getir)
        public async Task<IActionResult> DepartmanDuzenle(int? id)
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound(); // ID gelmezse 404 hatası ver
            }

            // Veritabanından o ID'ye sahip departmanı bul
            var departman = await _context.Departmanlar.FindAsync(id);

            if (departman == null)
            {
                return NotFound(); // Departman bulunamazsa 404 ver
            }

            // Departmanı bulduysan, formu doldurmak için View'e gönder
            return View(departman);
        }
        


        // DÜZENLEME (POST) 
        // POST: /Admin/DepartmanDuzenle/5
        // Formdan gelen güncel veriyi kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DepartmanDuzenle(int id, [Bind("DepartmanID,DepartmanAdi")] Departman departman)
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            // URL'deki id ile formdan gelen DepartmanID eşleşmiyorsa güvenlik açığı vardır
            if (id != departman.DepartmanID)
            {
                return NotFound();
            }

            // Formdan gelen veri geçerliyse
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departman); // Veriyi güncelle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Veri tabanı eşzamanlılık hatası (nadir)
                    if (!_context.Departmanlar.Any(e => e.DepartmanID == departman.DepartmanID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Başarılıysa listeye geri dön
                return RedirectToAction(nameof(DepartmanYonetimi));
            }

            // Form geçerli değilse, formu hatalarla tekrar göster
            return View(departman);
        }
        // ===========================================

        // SİLME (GET) 
        // GET: /Admin/DepartmanSil/5  
        public async Task<IActionResult> DepartmanSil(int? id)
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            // Departmanı bul, departman adını sormak için
            var departman = await _context.Departmanlar
                .FirstOrDefaultAsync(m => m.DepartmanID == id);

            if (departman == null)
            {
                return NotFound();
            }

            return View(departman); // Onay sayfasına gönder
        }
        

        //SİLME (POST) 
        // POST: /Admin/DepartmanSil/5
        // Asıl silme işlemini yapar
        [HttpPost, ActionName("DepartmanSil")] // Formdan ActionName ile eşleşir
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DepartmanSilOnaylandi(int id)
        {
            // Session kontrolü
            if (HttpContext.Session.GetString("AdminKullaniciAdi") == null)
            {
                return RedirectToAction("Login");
            }

            var departman = await _context.Departmanlar.FindAsync(id);
            if (departman != null)
            {
                _context.Departmanlar.Remove(departman); // Veritabanından kaldır
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }

            return RedirectToAction(nameof(DepartmanYonetimi)); // Listeye geri dön
        }
        // ===========================================
    }
}
