using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var employeeList = await _userRepository.GetAll().ToListAsync();
                return View(employeeList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> AddUser()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User _user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var existUser = await _userRepository.GetAll().Where(u => u.UserName == _user.UserName).ToListAsync();
                    if (existUser.Any())
                    {
                        TempData["ErrorMessage"] = "UserName must be uniqe !";
                        return RedirectToAction("AddUser", "Admin");

                    }
                    else
                    {
                        var newUser = new User
                        {
                            UserName = _user.UserName,
                            Name = _user.Name,
                            LastName = _user.LastName,
                            Password = _user.Password,
                            Type = _user.Type
                        };
                        await _userRepository.AddAsync(newUser);
                        return RedirectToAction("Index", "Admin");
                    }
                }

                else
                {
                    TempData["ErrorMessage"] = "Add user was unsuccessful !";
                    return RedirectToAction("AddUser", "Admin");
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }




        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try

            {
                User user = await _userRepository.GetAsync(id);

                if (user != null)
                {
                    EditUserDto editUserDto = new EditUserDto()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Type = user.Type
                    };
                    return View(editUserDto);
                }
                else

                    return RedirectToAction("Index", "Admin");
            }

            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDto _user)
        {
            try
            {
                User user = await _userRepository.GetAsync(_user.Id);

                if (user != null)
                {
                    user.Name = _user.Name;
                    user.LastName = _user.LastName;
                    user.UserName = _user.UserName;
                    user.Type = _user.Type;

                    await _userRepository.UpdateAsync(user, user.Id);
                }


                return RedirectToAction("Index", "Admin");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }




        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                User user = await _userRepository.GetAsync(id);

                if (user != null)
                {

                    await _userRepository.DeleteAsync(user);
                }

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }


          
        }

    }
}
