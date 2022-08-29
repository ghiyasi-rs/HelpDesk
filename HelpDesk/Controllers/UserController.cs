using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.Session.GetString("UserId"));
                User user = _userRepository.GetAsync(currentUserId).Result;
                ProfileDto profileDto = new ProfileDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    UserName = user.Email,
                    Type = user.Type,
                };
                return View(profileDto);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            ChangePassDto changePassDto = new ChangePassDto()
            {
                Id = id
            };
            return View(changePassDto);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassDto changePassDto)
        {
            try
            {
                User user = await _userRepository.GetAsync(changePassDto.Id);

                if (user != null)
                {
                    if (user.Password == changePassDto.CurrentPass && changePassDto.NewPass == changePassDto.ReNewPass)

                    {
                        user.Password = changePassDto.NewPass;


                        await _userRepository.UpdateAsync(user, user.Id);
                    }
                    else
                    TempData["ErrorMessage"] ="Current Password or New Password is not correct !";

                }

                return RedirectToAction("Profile", "User");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }


        }
    }
}
