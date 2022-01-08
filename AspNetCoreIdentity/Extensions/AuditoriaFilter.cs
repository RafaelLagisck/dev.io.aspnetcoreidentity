﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;

namespace AspNetCoreIdentity.Extensions
{
    public class AuditoriaFilter : IActionFilter
    {
        private readonly ILogger<AuditoriaFilter> _logger;
        public AuditoriaFilter(ILogger<AuditoriaFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
      

        public void OnActionExecuting(ActionExecutingContext context)
        {
           if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                var message = context.HttpContext.User.Identity.Name + " Accessou:" +
                              context.HttpContext.Request.GetDisplayUrl();

                _logger.LogInformation(message);
            }
        }
    }
}
