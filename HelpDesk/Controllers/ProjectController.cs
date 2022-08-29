using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDutyRepository _dutyRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;
        public ProjectController(IProjectRepository projectRepository, IDutyRepository dutyRepository, IProjectMemberRepository projectMemberRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _dutyRepository = dutyRepository;
            _projectMemberRepository = projectMemberRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var userType = HttpContext.Session.GetString("UserType").ToString();

                bool existUser = int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
                if (existUser)
                {
                    List<Project> projectList = new List<Project>();
                    if (userType == UserType.NetworkManager.ToString())

                        projectList = await _projectRepository.GetAll().ToListAsync();
                    else
                        projectList = await _projectRepository.GetAll().Where(p => p.UserId == userId).ToListAsync();

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

                int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);


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
                            UserId = userId
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


            ProjectDetailDto projectDetailDto = new ProjectDetailDto();
            projectDetailDto.Id = id;
            List<User> projectMember = await _projectMemberRepository.GetAll().Where(p => p.ProjectId == id).Select(p => new User
            {
                Id = p.Id,
                Name = p.User.Name,

            }).ToListAsync();

            projectDetailDto.ProjectMember = projectMember;

            var duties = await _dutyRepository.GetAll().Include(u => u.User).Where(x => x.ProjectId == id).ToListAsync();

            List<DutyDto> projectDuty = await _dutyRepository.GetAll().Where(p => p.ProjectId == id).Select(p => new DutyDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                UserName = p.User.Name


            }).ToListAsync();
            projectDetailDto.Duties = projectDuty;


            return View(projectDetailDto);
        }

        [HttpGet]
        public async Task<IActionResult> AddMember(int projectId)
        {
            if (projectId > 0)
            {
                var existUser = _projectMemberRepository.GetAll().Where(p => p.ProjectId == projectId).Select(x => x.UserId).ToArray();
                ProjectMemberDto projectMemberDto = new ProjectMemberDto();

                projectMemberDto.User = await _userRepository.GetAll().Where(p => !existUser.Contains(p.Id)).ToListAsync();
                projectMemberDto.ProjectId = projectId;

                return PartialView("_AddMember", projectMemberDto);
            }
            else
            {
                return RedirectToAction("Index", "Project");
            }


        }

        [HttpPost]
        public async Task<IActionResult> AddMember(ProjectMemberDto projectMemberDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {

                    ProjectMember projectMember = new ProjectMember();
                    projectMember.ProjectId = projectMemberDto.ProjectId;
                    projectMember.UserId = projectMemberDto.UserId;
                    await _projectMemberRepository.AddAsync(projectMember);

                }

                return RedirectToAction("Detail", "Project", new { id = projectMemberDto.ProjectId });
            }

            catch
            {
                return RedirectToAction("Index", "Project");

            }


        }

        [HttpGet]
        public async Task<IActionResult> AddDuty(int projectId)
        {
            if (projectId > 0)
            {
                DutyDto dutyDto = new DutyDto();
                dutyDto.ProjectId = projectId;
                var existUser = _projectMemberRepository.GetAll().Where(p => p.ProjectId == projectId).Select(x => x.UserId).ToArray();
                dutyDto.Users = await _userRepository.GetAll().Where(p => existUser.Contains(p.Id)).ToListAsync();

                return PartialView("_AddDuty", dutyDto);
            }
            else
            {
                return RedirectToAction("Index", "Project");
            }


        }

        [HttpPost]
        public async Task<IActionResult> AddDuty(DutyDto _dutyDto)
        {
            if (ModelState.IsValid)
            {
                Duty duty = new Duty()
                {
                    Title = _dutyDto.Title,
                    Description = _dutyDto.Description,
                    ProjectId = _dutyDto.ProjectId,
                    UserId = _dutyDto.UserId,


                };


                await _dutyRepository.AddAsync(duty);
                return RedirectToAction("Detail", "Project", new { id = duty.ProjectId });
            }
            else
            {
                return RedirectToAction("Index", "Project");
            }


        }


        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                ProjectMember projectMember = await _projectMemberRepository.GetAsync(id);

                if (projectMember != null)
                {

                    await _projectMemberRepository.DeleteAsync(projectMember);
                }

                return RedirectToAction("Detail", "Project", new { id = projectMember.ProjectId });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> DeleteDuty(int id)
        {
            try
            {
                Duty duty = await _dutyRepository.GetAsync(id);

                if (duty != null)
                {

                    await _dutyRepository.DeleteAsync(duty);
                }

                return RedirectToAction("Detail", "Project", new { id = duty.ProjectId });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }



        }

    }
}
