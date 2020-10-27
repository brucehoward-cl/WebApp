using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace WebApp.Filters
{
    #region implemented by inheriting from Attribute Base Class
    //public class ChangeArgAttribute : Attribute, IAsyncActionFilter
    //{
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        if (context.ActionArguments.ContainsKey("message1"))
    //        {
    //            context.ActionArguments["message1"] = "New message";
    //        }
    //        await next();
    //    }
    //} 
    #endregion

    #region implemented by inheriting from ActionFilterAttribute Base Class which implements both IActionFilter and IAsyncActionFilter interfaces
    public class ChangeArgAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("message1"))
            {
                context.ActionArguments["message1"] = "New message";
            }

            await next();
        }
    } 
    #endregion
}