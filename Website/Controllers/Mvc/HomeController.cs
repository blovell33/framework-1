using System.Web.Mvc;

namespace Website.Controllers.Mvc {
	public class HomeController : Controller {
		public HomeController() {
			ViewBag.Title = "Home";
			ViewBag.Fluid = true;
		}

		public ActionResult Index() {
			return View("DefaultView");
		}
	}
}