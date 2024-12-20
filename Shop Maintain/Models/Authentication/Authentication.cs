using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Shop_Maintain.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("UserName");
            System.Diagnostics.Debug.WriteLine($"UserName từ session trong Authentication: {username}");

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller", "Access" },
                        {"Action", "Login" }
                    });
            }
            else
            {
                // Quan trọng: Cho phép request tiếp tục nếu đã đăng nhập
                base.OnActionExecuting(context);
            }
        }
    }
}
