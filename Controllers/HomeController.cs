using System.Diagnostics;
using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatVeMayBay.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.SanBays = _db.LayDanhSachSanBay();
            return View();
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