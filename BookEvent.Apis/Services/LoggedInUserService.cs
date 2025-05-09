using BookEvent.Core.Application.Abstraction;
using System.Security.Claims;

namespace BookEvent.Apis.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor? _httpcontextAccessor;
        public string? UserId { get; set; }

        public LoggedInUserService(IHttpContextAccessor? contextAccessor)
        {
            _httpcontextAccessor = contextAccessor;


            UserId = _httpcontextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.PrimarySid);
        }

    }
}
