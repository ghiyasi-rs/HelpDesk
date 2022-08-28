using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HelpDesk.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDutyRepository _dutyRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        public ProjectController(IProjectRepository projectRepository,IDutyRepository dutyRepository, IProjectMemberRepository projectMemberRepository)
        {
            _projectRepository = projectRepository;
            _dutyRepository = dutyRepository;
            _projectMemberRepository = projectMemberRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {

                bool existUser = int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
                if (existUser)
                {
                    var projectList = await _projectRepository.GetAll().Where(p => p.UserId == userId).ToListAsync();
                    return View(projectList);
                }
                else
                {
                    var projectList = "";
                    return View(projectList);
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProjectDto _projectDto)
        {

            try
            {
                int userId;
                bool existUser = int.TryParse(HttpContext.Session.GetString("UserId"), out userId);


                if (ModelState.IsValid)
                {
                    var existProject = await _projectRepository.GetAll().Where(p => p.Name == _projectDto.Name).ToListAsync();
                    if (existProject.Any())
                    {
                        TempData["ErrorMessage"] = "Project Nmae must be uniqe !";
                        return RedirectToAction("Add", "Project");

                    }
                    else
                    {
                        var newProject = new Project
                        {
                            Name = _projectDto.Name,
                            UserId= userId
                        };
                        await _projectRepository.AddAsync(newProject);
                        return RedirectToAction("Index", "Project");
                    }
                }

                else
                {
                    TempData["ErrorMessage"] = "Add Project was unsuccessful !";
                    return RedirectToAction("Add", "Project");
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }




        }

        public async Task<IActionResult> Detail(int id)
        {
           
            var duty = await _dutyRepository.GetAll().Where(x => x.ProjectId == id).ToListAsync();
            ProjectDetailDto projectDetailDto = new ProjectDetailDto();
            List<User> projectMember = await _projectMemberRepository.GetAll().Where(p => p.ProjectId == id).Select(p => new User
            {
                Id = p.Id,
                Name = p.User.Name,             
            }).ToListAsync();

            projectDetailDto.ProjectMember = projectMember;

            return View(projectDetailDto);
        }
    }
}
