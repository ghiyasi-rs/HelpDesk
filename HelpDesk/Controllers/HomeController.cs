using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repository;
using HelpDesk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace HelpDesk.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserRepository _userRepository;

        public HomeController(IUserRepository userRepository)
        {

            _userRepository = userRepository;

        }

        public async Task<IActionResult> Index()
        {
            int id = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = await _userRepository.GetAsync(id);

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        public async Task<IActionResult> Login(User _user)
        {
            var user = await _userRepository.FindAsync(u => u.Email == _user.Email && u.Password == _user.Password);

            try
            {
                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.Email);
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserType", user.Type.ToString());
                    
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["ErrorMessage"] = "User Name or password in incorrect !";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Login", "Home");
        }


        public async Task<IActionResult> Error()
        {

            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");

            return View();
        }


    }
}