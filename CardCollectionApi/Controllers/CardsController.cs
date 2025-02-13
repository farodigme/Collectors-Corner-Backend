using CardCollectionApi.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardCollectionApi.Controllers
{
	public class CardsController : Controller
	{
		private ApplicationContext _context;
		public CardsController(ApplicationContext context)
		{
			_context = context;
		}
	}
}
