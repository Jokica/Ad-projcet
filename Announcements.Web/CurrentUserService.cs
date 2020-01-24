using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Announcements.Web
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext httpContext;
        private ClaimsPrincipal User => httpContext.User;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        
        public string GetUserId() =>
            User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
    }
}
