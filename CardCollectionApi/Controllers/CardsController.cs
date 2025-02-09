using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardCollectionApi.Controllers
{
	public class CardsController : Controller
	{
		// GET: CardsController
		public ActionResult Index()
		{
			return View();
		}

		// GET: CardsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: CardsController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CardsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CardsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CardsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CardsController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CardsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
