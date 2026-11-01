using DatVeMayBay.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Bật đăng nhập, lưu tài khoản trong database của mình
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Chưa đăng nhập thì chuyển tới trang này; không đủ quyền thì chuyển tới trang kia
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/TaiKhoan/DangNhap";
                options.AccessDeniedPath = "/TaiKhoan/TuChoi";
            });

            var app = builder.Build();

            await TaoTaiKhoanAdmin(app);

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();   // xác định "bạn là ai" — phải đứng trước UseAuthorization
            app.UseAuthorization();    // kiểm tra "bạn được làm gì"

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=QuanLyChuyenBay}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }

        // Tạo vai trò Admin và tài khoản admin mặc định nếu chưa có
        private static async Task TaoTaiKhoanAdmin(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            const string email = "admin@datve.vn";
            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin@123");
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}