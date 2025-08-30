var builder = WebApplication.CreateBuilder(args);

// Thêm cấu hình API Base URL
builder.Configuration.AddJsonFile("appsettings.json");
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7024/api";

// Thêm hỗ trợ Controllers + Views
builder.Services.AddControllersWithViews();

// Thêm hỗ trợ Session (để lưu thông báo hoặc dữ liệu tạm thời)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký HttpClient
builder.Services.AddHttpClient();

// Đăng ký ProductService
builder.Services.AddScoped<TerrariumStore.Web.Services.ProductService>();

// Thêm cấu hình ApiBaseUrl nếu chưa có trong appsettings.json
if (string.IsNullOrEmpty(builder.Configuration["ApiBaseUrl"]))
{
    builder.Configuration["ApiBaseUrl"] = apiBaseUrl;
}

var app = builder.Build();

// Middleware cấu hình
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Sử dụng session
app.UseSession();

app.UseRouting();

// Định nghĩa route mặc định để điều hướng về /Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();