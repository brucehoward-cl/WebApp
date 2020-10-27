using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;   //for implementing the asynchronous filter

namespace WebApp.Filters
{
    #region The non-asynchronous implementation
    //public class SimpleCacheAttribute : Attribute, IResourceFilter
    //{
    //    private Dictionary<PathString, IActionResult> CachedResponses = new Dictionary<PathString, IActionResult>();

    //    public void OnResourceExecuting(ResourceExecutingContext context)
    //    {
    //        PathString path = context.HttpContext.Request.Path;

    //        if (CachedResponses.ContainsKey(path))
    //        {
    //            context.Result = CachedResponses[path];
    //            CachedResponses.Remove(path);
    //        }
    //    }

    //    public void OnResourceExecuted(ResourceExecutedContext context)
    //    {
    //        CachedResponses.Add(context.HttpContext.Request.Path, context.Result);
    //    }
    //} 
    #endregion

    #region The asynchronous implementation
    public class SimpleCacheAttribute : Attribute, IAsyncResourceFilter
    {
        private Dictionary<PathString, IActionResult> CachedResponses = new Dictionary<PathString, IActionResult>();

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            PathString path = context.HttpContext.Request.Path;

            if (CachedResponses.ContainsKey(path))
            {
                context.Result = CachedResponses[path];
                CachedResponses.Remove(path);
            }
            else
            {
                ResourceExecutedContext execContext = await next();
                CachedResponses.Add(context.HttpContext.Request.Path, execContext.Result);
            }
        }
    } 
    #endregion
}