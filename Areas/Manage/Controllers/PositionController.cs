using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Woody_Mvc.DAL;
using Woody_Mvc.Models;
using Woody_Mvc.ViewModels.Position;

namespace Woody_Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PositionController : Controller
    {
        AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var positions =await _context.Positions.Include(x=>x.TeamMembers).ToListAsync();
            return View(positions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePositionVm Vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Position position = new Position()
            { 
              Name = Vm.Name
            };

            _context.Positions.Add(position);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id==null)
                return BadRequest();
            var positions = _context.Positions.FirstOrDefault(x=>x.Id==id);

            if (positions == null)
                return NotFound();
         _context.Positions.Remove(positions);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Update(int? id)
        {
            if(id == null)
                return BadRequest();
            var position = _context.Positions.FirstOrDefault(x=>x.Id == id);
            if (position == null)
                return NotFound();

            UpdatePositionVm updatePositionVm = new UpdatePositionVm()
            { 
              Id = position.Id,
              Name= position.Name
            };

            
            return View(updatePositionVm);
        }

        [HttpPost]
        public IActionResult Update(UpdatePositionVm Vm)
        {
            if (!ModelState.IsValid)
                return NotFound();

            Position position = new Position()
            {
              Id = Vm.Id,
              Name = Vm.Name
            };
            _context.Positions.Update(position);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
