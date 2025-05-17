using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Areas.Admin.Repository;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

var builder = WebApplication.CreateBuilder(args);

// K?t n?i cõ s? d? li?u
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDb")));

// C?u h?nh Email Sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// C?u h?nh Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
});

// Thêm d?ch v? HttpClient ð? s? d?ng IHttpClientFactory
builder.Services.AddHttpClient();

// Thêm d?ch v? MVC và session
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseSession();

// Middleware x? l? l?i & HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// C?u h?nh ð?nh tuy?n có Area
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Course}/{action=Index}/{id?}");

// Ð?nh tuy?n custom cho category
app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });

// Ð?nh tuy?n custom cho instructor
app.MapControllerRoute(
    name: "instructor",
    pattern: "/instructor/{Slug?}",
    defaults: new { controller = "Instructor", action = "Index" });

// Tuy?n m?c ð?nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seeding d? li?u m?u
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeeData.SeedingData(context);

app.Run();
