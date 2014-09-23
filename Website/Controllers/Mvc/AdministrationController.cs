using System.Web.Mvc;
using Core;

namespace Website.Controllers.Mvc {
	[Authorize(Roles = Constants.Roles.ADMINISTRATOR_ROLE)]
	[RoutePrefix("administration")]
	[Route("{action=index}")]
	public class AdministrationController : Controller {
		public AdministrationController() {
			ViewBag.Title = "Administration";
		}

		[Route("{*segments}")]
		public ActionResult Index() {
			ViewBag.Sidebar = true;

			return View("DefaultView");
		}
	}
}