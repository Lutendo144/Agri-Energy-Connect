using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_EnergyConnect.Data;
using Agri_EnergyConnect.Models;
using Agri_EnergyConnect.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agri_EnergyConnect.Controllers
{
    public class ProjectCollaborationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectCollaborationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectCollaboration/Index
        public async Task<IActionResult> Index()
        {
            var projects = await _context.ProjectCollaborations
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var projectCommentViewModels = new List<ProjectCommentViewModel>();

            foreach (var project in projects)
            {
                var comments = await _context.ProjectComments
                    .Where(c => c.ProjectId == project.Id)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                projectCommentViewModels.Add(new ProjectCommentViewModel
                {
                    Project = project,
                    Comments = comments,
                    NewComment = new ProjectComment { ProjectId = project.Id }
                });
            }

            var model = new ProjectCommentListViewModel
            {
                ProjectsWithComments = projectCommentViewModels
            };

            return View(model);
        }

        // GET: ProjectCollaboration/Create
        // GET: ProjectCollaboration/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.ProjectCollaborations.FindAsync(id);
            if (project == null) return NotFound();

            ViewBag.ProjectName = project.ProjectName;
            ViewBag.ProjectId = project.Id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinConfirmed(int projectId, string joinerName, string joinerEmail)
        {
            var join = new ProjectJoin
            {
                ProjectId = projectId,
                JoinerName = joinerName,
                JoinerEmail = joinerEmail,
                JoinedAt = DateTime.Now
            };

            _context.ProjectJoins.Add(join);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully joined the project!";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCollaboration model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int projectId, string content, string authorName)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new ProjectComment
                {
                    ProjectId = projectId,
                    Content = content,
                    AuthorName = string.IsNullOrWhiteSpace(authorName) ? "Anonymous" : authorName,
                    CreatedAt = DateTime.Now
                };

                _context.ProjectComments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
