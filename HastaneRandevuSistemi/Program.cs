using HastaneRandevuSistemi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. appsettings.json dosyasından "DefaultConnection" adlı bağlantı dizesini al.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext'i servislere "AddDbContext" ile ekle.
//    Proje içinde ApplicationDbContext istendiğinde, SQL Server'a 
//    yukarıdaki connectionString ile bağlanmasını söyle.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

// ===
builder.Services.AddDistributedMemoryCache(); // Session'ları hafızada tutmak için
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum 30dk sonra sona ersin
    options.Cookie.HttpOnly = true; // Güvenlik için
    options.Cookie.IsEssential = true; // GDPR uyumluluğu için
});
// =====

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
