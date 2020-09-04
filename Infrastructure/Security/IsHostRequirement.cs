using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence;

//  Add to STartup.cs

//   services.AddAuthorization(opt =>
//              {
//                 opt.AddPolicy("IsActivityHost", policy =>
//                 {
//                     policy.Requirements.Add(new IsHostRequirement());
//                 });
//              });
//             services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

namespace Infrastructure.Security
{
    //  This module has the type of check you you use to see if a user has access to delete or change based on who the user is and
    // what record give him access in the database. example  - is owner or admin 
    public class IsHostRequirement : IAuthorizationRequirement
    { }
    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor,
         DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
         IsHostRequirement requirement)
        {
//   if (context.Resource is AuthorizationFilterContext authContext)
            {
                // Get current Logged in User  or user from conextext when called 
                var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?
                .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                // Get the Guid passed in during the call. I wish I knew how to do this when I created error handling code.
                // If I could get to payload and other values from original call I could have created a universal error handler for all Controller and their methods maybe

                var activityId = Guid.Parse
                (_httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key
               == "id").Value.ToString());

                var activity = _context.Activities.FindAsync(activityId).Result;

                var host = activity.UserActivities.FirstOrDefault(x => x.IsHost);


                if (host?.AppUser?.UserName == currentUserName)
                {
                    context.Succeed(requirement);
                }
            }
         //   else { context.Fail(); }


            return Task.CompletedTask;
        }
    }
}