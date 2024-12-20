// Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Shop_Maintain.Models;
using Shop_Maintain.ViewModels;

namespace Shop_Maintain.Controllers
{
    public class UserController : Controller
    {
        private readonly QlbanVaLiContext db;

        public UserController(QlbanVaLiContext context)
        {
            db = context;
        }

        // GET: User
        public IActionResult Index()
        {
            var users = db.TUsers.ToList();
            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new TUser
                {
                    Username = userViewModel.Username,
                    Password = userViewModel.Password,
                    LoaiUser = userViewModel.LoaiUser
                };

                db.TUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(userViewModel);
        }

        // GET: User/Edit/{username}
        public IActionResult Edit(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                Username = user.Username,
                Password = user.Password,
                LoaiUser = user.LoaiUser
            };

            return View(userViewModel);
        }

        // POST: User/Edit
        [HttpPost]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.TUsers.Find(userViewModel.Username);
                if (user == null)
                {
                    return NotFound();
                }

                user.Password = userViewModel.Password;
                user.LoaiUser = userViewModel.LoaiUser;

                db.TUsers.Update(user);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(userViewModel);
        }

        // GET: User/Delete/{username}
        public IActionResult Delete(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string username)
        {
            var user = db.TUsers.Find(username);
            if (user != null)
            {
                db.TUsers.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
