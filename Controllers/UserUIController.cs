using Microsoft.AspNetCore.Mvc;
using Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Xml.Linq;
using Interfaces.Models;


namespace Admin_Panel.Controllers
{
    public class UserUIController : Controller
    {
        private readonly IUserService _userService;

        public static UserDTO LoginedUser;


        public UserUIController(IUserService userService)
        {
            _userService = userService;
        }

        //public async Task<IActionResult> UserExists(string name)
        //{
        //    var exists = await _userService.CheckUserExistsAsync(name);
        //    return Json(new { Exists = exists });
        //}
        public async Task<IActionResult> Login()
        {
            return View();
        }


        public async Task<IActionResult> Success()
        {
            ViewBag.LoginedUser = LoginedUser.Name + " " + LoginedUser.Surname + " (" + LoginedUser.Id + ")";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                // Handle invalid input, you can add a message to ViewBag/ViewData and show it on the view
                ViewBag.Message = "Please provide both login and password.";
                return View();
            }

            // Call the UserServiceClient method to check credentials
            //var isValidCredentials = await _userService.CheckCredentialsAsync(login, password);
            LoginedUser = await _userService.GetLoginedUser(login, password);


            if (LoginedUser != null)
            {
                // Redirect to a success page or do whatever you need to do on successful login
                return RedirectToAction("Success");
            }
            else
            {

                return RedirectToAction("Decline");
                // Handle invalid credentials, show an error message

            }
        }


        public async Task<IActionResult> Decline()
        {
            ViewBag.Message = "Invalid login or password.";
            return View();
        }


        public async Task<IActionResult> Index()
        {
            var exists = await _userService.CheckCredentialsAsync("alex", "alex");
            ViewBag.UserExists = exists;
            return View();
        }

        //public async Task<IActionResult> Index()
        //{
        //    var users = await _userService.GetAsync();
        //    return View(users);
        //}

        //public async Task<IActionResult> UserDetails(int userId)
        //{
        //    var user = await _userService.GetAsync(userId);
        //    return View(user);
        //}
    }
}
