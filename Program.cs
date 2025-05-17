using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký controller
builder.Services.AddControllers(); 
builder.Services.AddOpenApi();

var app = builder.Build();

// Tạo thư mục wwwroot/videos nếu chưa tồn tại
var videosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
if (!Directory.Exists(videosPath))
{
    Directory.CreateDirectory(videosPath);
}

// Cho phép truy cập file tĩnh (wwwroot)
app.UseStaticFiles();

// Cho phép truy cập thư mục /videos qua URL
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos")),
    RequestPath = "/videos"
});

// Map controller
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
