using Microsoft.AspNetCore.Mvc;

namespace CardCollectionApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CollectionsControlles : ControllerBase
	{
		private readonly ILogger<CollectionsControlles> _logger;

		public CollectionsControlles(ILogger<CollectionsControlles> logger)
		{
			_logger = logger;
		}
	}
}
