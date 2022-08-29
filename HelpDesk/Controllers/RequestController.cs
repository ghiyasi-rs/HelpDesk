using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers
{
    public class RequestController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestRepository _requestRepository;
        public RequestController(IUserRepository userRepository, IRequestRepository requestRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;

        }
        public async Task<IActionResult> Index()
        {

            try
            {
                var userType = HttpContext.Session.GetString("UserType").ToString();

                bool existUser = int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
                if (existUser)
                {
                    List<Request> requestList = new List<Request>();
                    if (userType == UserType.NetworkManager.ToString())

                        requestList = await _requestRepository.GetAll().ToListAsync();
                    else
                        requestList = await _requestRepository.GetAll().Where(p => p.UserId == userId).ToListAsync();

                    return View(requestList);
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


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                RequestDto requestDto = new RequestDto();


                return PartialView("_AddRequest", requestDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(RequestDto _requestDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    bool existUser = int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
                    if (existUser)
                    {
                        Request request = new Request()
                        {
                            Title = _requestDto.Title,
                            Description = _requestDto.Description,
                            UserId = userId

                        };
                        await _requestRepository.AddAsync(request);
                    }


                }

                return RedirectToAction("Index", "Request");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }


        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Request request = await _requestRepository.GetAsync(id);

                if (request != null)
                {

                    await _requestRepository.DeleteAsync(request);
                }

                return RedirectToAction("Index", "Request");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
