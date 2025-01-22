using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Woody_Mvc.DAL;
using Woody_Mvc.Models;
using Woody_Mvc.Helpers.Extensions;
using Woody_Mvc.ViewModels.TeamMember;

namespace Woody_Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class MemberController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MemberController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<TeamMember> teamMembers = await _context.TeamMembers.Include(x=>x.Position).ToListAsync();
            return View(teamMembers);
        }

        public IActionResult Create()
        {
          ViewBag.Positions = _context.Positions.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamMemberVm createTeamMemberVm)
        {
            ViewBag.Positions =await _context.Positions.ToListAsync();

            if (!ModelState.IsValid)
            return View(createTeamMemberVm);

            if(!createTeamMemberVm.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File", "Duzgun sekil formati daxil edin");
                return View(createTeamMemberVm);
            }

            if(createTeamMemberVm.File.Length > 2097152)
            {
                ModelState.AddModelError("File", "Sekil olcusu max 2 mq ola biler!");
                return View(createTeamMemberVm);
            }

            TeamMember team_Member = new TeamMember()
            {
                Name = createTeamMemberVm.Name,
                positionId = createTeamMemberVm.positionId,
                ImgUrl = createTeamMemberVm.File.Upload(_env.WebRootPath, "Upload/TeamMember")

            };
            await _context.TeamMembers.AddAsync(team_Member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            TeamMember? teamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (teamMember == null)
                return NotFound();

             _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Positions =await _context.Positions.ToListAsync();
            if (id == null)
                return BadRequest();
             
            TeamMember? teamMember = await _context.TeamMembers.FirstOrDefaultAsync(x=>x.Id == id);

            if(teamMember == null)
                return NotFound();

            UpdateTeamMemberVm updateTeamMemberVm = new UpdateTeamMemberVm()
            { 
             Id = teamMember.Id,
             Name = teamMember.Name,
             positionId = teamMember.positionId
            };

            return View(updateTeamMemberVm);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTeamMemberVm updateTeamMemberVm)
        {
            ViewBag.Positions =await _context.Positions.ToListAsync();

            if(!ModelState.IsValid)
                return View(updateTeamMemberVm);

            TeamMember? teamMember = await _context.TeamMembers.FirstOrDefaultAsync(x=>x.Id==updateTeamMemberVm.Id);

            if( teamMember == null)
                return NotFound();


            teamMember.Name = updateTeamMemberVm.Name;
                teamMember.positionId = updateTeamMemberVm.positionId;
              

           
            

            if (updateTeamMemberVm.File != null)
            {
                if (!updateTeamMemberVm.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("File", "Duzgun sekil formati daxil edin");
                    return View(updateTeamMemberVm);
                }

                if(updateTeamMemberVm.File.Length > 2097152)
                {
                    ModelState.AddModelError("File", "Sekilin olcusu max 2 mq ola biler");
                    return View(updateTeamMemberVm);
                }

                FileExtension.DeleteFile(_env.WebRootPath, "Upload/TeamMember", teamMember.ImgUrl);
                teamMember.ImgUrl = updateTeamMemberVm.File.Upload(_env.WebRootPath, "Upload/TeamMember");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
}
