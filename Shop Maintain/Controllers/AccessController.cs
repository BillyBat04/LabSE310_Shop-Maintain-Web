using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Shop_Maintain.Models;
using System.Security.Cryptography;

namespace Shop_Maintain.Controllers
{
    public class AccessController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                var existingUser = db.TUsers.FirstOrDefault(x => x.Username == registerUser.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(registerUser);
                }

                // Mã hóa mật khẩu
                var hashedPassword = HashPassword(registerUser.Password);

                // Tạo tài khoản mới
                var newUser = new TUser
                {
                    Username = registerUser.Username,
                    Password = hashedPassword
                };

                db.TUsers.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Login", "Access");
            }
            return View(registerUser);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return Convert.ToBase64String(salt) + "." + hash;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") ==  null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(TUser user)
        {
            if(HttpContext.Session.GetString("UserName")  == null)
            {
                var u = db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            var hashToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return hashToVerify == hash;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
     
}
