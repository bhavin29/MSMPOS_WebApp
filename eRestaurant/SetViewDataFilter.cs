using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RocketPOS.Framework;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;

namespace RocketPOS
{
    public class SetViewDataFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("returnUrl", out object value))
            {
                // NOTE: this assumes all your controllers derive from Controller.
                // If they don't, you'll need to set the value in OnActionExecuted instead
                // or use an IAsyncActionFilter
                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ReturnUrl"] = value.ToString();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) {

            string name = (string)context.RouteData.Values["Controller"];

              if (LoginInfo.Userid == 0 &&  name != "Login" && name != "Home")
            {
                  context.Result = new RedirectResult("~/Login");
            }
          }
    }
}
