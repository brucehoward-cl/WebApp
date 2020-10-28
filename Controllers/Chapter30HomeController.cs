using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApp.Filters; //to use the custom (ChangeArg) filter
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApp.Controllers
{
    //[RequireHttps] //applying at the controller definition applies it to all methods automatically
    //[HttpsOnly]     // the custom filter
    //[ResultDiagnostics]     //the custom result filter
    //[GuidResponse]    //since this is an always-run filter it would replace the global filter, so it's been commented out for that demonstration
    //[GuidResponse]
    //[Message("This is the controller-scoped filter")]  //This custom global result filter allows multiple filters to build a series of messages
    [Message("This is the controller-scoped filter", Order = 10)] //This custom result filter changes the order in which they are executed
    public class Chapter30HomeController : Controller
    {
        #region Implementing security in an action method without using filters
        //public IActionResult Index()
        //{
        //    if (Request.IsHttps)
        //    {
        //        return View("Message", "This is the Index action on the Home controller");
        //    }
        //    else
        //    {
        //        return new StatusCodeResult(StatusCodes.Status403Forbidden);
        //    }
        //}

        //public IActionResult Secure()  //one way to implement security policy; but you must remember to call the method in every secured method!
        //{
        //    if (Request.IsHttps)
        //    {
        //        return View("Message", "This is the Secure action on the Home controller");
        //    }
        //    else
        //    {
        //        return new StatusCodeResult(StatusCodes.Status403Forbidden);
        //    }
        //} 
        #endregion

        //[RequireHttps]
        //[Message("This is the first action-scoped filter")]  //custom  result filter
        //[Message("This is the second action-scoped filter")]  //custom  result filter
        [Message("This is the first action-scoped filter", Order = 1)]   //custom  result filter with order of execution changed
        [Message("This is the second action-scoped filter", Order = -1)]   //custom  result filter with order of execution changed
        public IActionResult Index()
        {
            return View("Message", "This is the Index action on the Home controller");
        }

        //[RequireHttps]
        public IActionResult Secure()
        {
            return View("Message", "This is the Secure action on the Home controller");
        }

        //[ChangeArg]     //custom action filter
        public IActionResult Messages(string message1, string message2 = "None")
        {
            return View("Message", $"{message1}, {message2}");
        }

        //Since Controller base class implements IActionFilter and IAsyncActionFilter interfaces, you can override the methods here
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("message1"))
            {
                context.ActionArguments["message1"] = "New message";
            }
        }

        [RangeException]        //custom exception filter
        public ViewResult GenerateException(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            else if (id > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            else
            {
                return View("Message", $"The value is {id}");
            }
        }
    }
}