using Collectors_Corner_Backend.Interfaces;
using System.Security.Claims;

namespace Collectors_Corner_Backend.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		public string? Username { get; }

		public CurrentUserService(IHttpContextAccessor accessor)
		{
			var user = accessor.HttpContext?.User;
			Username = user?.Identity?.Name;
		}
	}
}
