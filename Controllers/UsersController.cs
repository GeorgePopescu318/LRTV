using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRTV.Models;
using System.Linq;
using System.Threading.Tasks;
using LRTV.ContextModels;
using LRTV.ViewModels;
using LRTV.Logic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LRTV.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersContext context;

        public UsersController(UsersContext context)
        {
            this.context = context;
        }


        [Authorize]
        public IActionResult MyProfile()
        {
            var username = User.Identity.Name;
            var user = context.User.SingleOrDefault(user => user.Username == username);
            if (user == null) return NotFound();
            return View(user);
        }

        

        [HttpGet]
        public IActionResult PromoteUser(int userId)
        {
            UserModel user = context.User.FirstOrDefault(user => user.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Check if the user is already an admin or moderator
            if (user.Role == UserType.Admin || user.Role == UserType.Moderator)
            {
                ModelState.AddModelError(string.Empty, "User is already a moderator or admin.");
                return RedirectToAction("UserProfile", new { userId });
            }

            // Promote the user to moderator
            user.Role = UserType.Moderator;
            context.SaveChanges();
            return RedirectToAction("UserProfile", new { userId });
        }

        [HttpGet]
        public IActionResult DemoteUser(int userId)
        {
            UserModel user = context.User.FirstOrDefault(user => user.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Check if the user is already a member
            if (user.Role == UserType.Member)
            {
                ModelState.AddModelError(string.Empty, "User is already a member.");
                return RedirectToAction("UserProfile", new { userId });
            }

            // Demote the user to member
            user.Role = UserType.Member;
            context.SaveChanges();
            return RedirectToAction("UserProfile", new { userId });
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeUsername()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var username = User.Identity.Name;
            var user = context.User.SingleOrDefault(u => u.Username == username);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(model.NewUsername))
            {
                ModelState.AddModelError(string.Empty, "The new username cannot be empty.");
                return View(model);
            }

            if (context.User.Any(u => u.Username == model.NewUsername))
            {
                ModelState.AddModelError(string.Empty, "The username is already taken.");
                return View(model);
            }

            user.Username = model.NewUsername;
            context.SaveChanges();

            // Log the user out and log them back in with the new username
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        // Add other claims as necessary
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("MyProfile");
        }


        private async Task SignInUser(UserModel user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var username = User.Identity.Name;
            var user = context.User.SingleOrDefault(u => u.Username == username);
            if (user == null) return NotFound();

            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.NewPassword))
            {
                ModelState.AddModelError("NewPassword", "The new password cannot be empty.");
                return View(model);
            }

            user.Password = model.NewPassword;
            user.PasswordConfirm = model.ConfirmPassword;
            context.SaveChanges();

            return RedirectToAction("MyProfile");
        }


        [HttpGet]

        public async Task<IActionResult> DeleteProfile()
        {
            var username = User.Identity.Name;
            var user = context.User.SingleOrDefault(user => user.Username == username);
            if (user == null) return NotFound();

            context.User.Remove(user);
            context.SaveChanges();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var loggedInUsername = User.Identity.Name;
            var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
            if (userRole.ToLower() != "admin")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
            var users = context.User.Where(user => user.Username != loggedInUsername).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult UserProfile(int UserId)
        {
            var user = context.User.SingleOrDefault(user => user.Id == UserId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
            if (userRole.ToLower() != "admin")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
            return View(user);
        }

        [HttpGet]
        
        public IActionResult AddUser()
        {
            var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
            if (userRole.ToLower() != "admin")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
            return View();
        }

        [HttpPost]
        // [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> AddUser(CreateUserViewModel model)
        {



            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(model.Username))
                        ModelState.AddModelError(string.Empty, "The username is empty!");
                    else if (string.IsNullOrWhiteSpace(model.Password))
                        ModelState.AddModelError(string.Empty, "The password is empty!");
                    else if (string.IsNullOrWhiteSpace(model.PasswordConfirm))
                        ModelState.AddModelError(string.Empty, "The password confirmation is empty!");
                    else if (model.Password != model.PasswordConfirm)
                        ModelState.AddModelError(string.Empty, "The password and password confirmation don't match!");
                    else if (context.User.Where(user => user.Username!.ToLower() == model.Username.ToLower()).Count() > 0)
                        ModelState.AddModelError(string.Empty, "The username is already taken!");
                    else
                    {
                        try
                        {
                            var usersVM = new UserModel
                            {
                                Username = model.Username,
                                Password = model.Password,
                                PasswordConfirm = model.PasswordConfirm
                            };

                            context.User.Add(usersVM);
                            context.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, "Error creating account: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
            
        }

        

        [HttpGet]
        
        public IActionResult DeleteUser(int UserId)
        {
            var user = context.User.SingleOrDefault(user => user.Id == UserId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
            if (userRole.ToLower() != "admin")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
            context.User.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
