using System.Diagnostics;
using HastaneRandevuSistemi.Data; // Veritabanı bağlantısı için eklendi
using HastaneRandevuSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Sıralama ve filtreleme (OrderBy, Take) için gerekli

namespace HastaneRandevuSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Veritabanı bağlamını (Context) tanımlıyoruz
        private readonly ApplicationDbContext _context;

        // Constructor'da (Yapıcı Metot) DbContext'i içeri alıyoruz (Dependency Injection)
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Gelen context'i yerel değişkene atıyoruz
        }

        public IActionResult Index()
        {
            // Veritabanındaki "Duyurular" tablosundan veri çekiyoruz.
            // 1. OrderByDescending: En yeni tarihli duyuru en üstte olsun.
            // 2. Take(5): Sadece son 5 duyuruyu getir.
            // 3. ToList(): Listeye çevir.
            var duyurular = _context.Duyurular
                                    .OrderByDescending(d => d.YayinTarihi)
                                    .Take(5)
                                    .ToList();

            // Duyurular listesini View'e model olarak gönderiyoruz
            return View(duyurular);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}