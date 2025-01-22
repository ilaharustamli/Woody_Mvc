using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Woody_Mvc.DAL;
using Woody_Mvc.Models;

namespace Woody_Mvc.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<TeamMember> tm = _context.TeamMembers.Include(t=>t.Position).ToList();
            return View(tm);
        }

       
    }
}
