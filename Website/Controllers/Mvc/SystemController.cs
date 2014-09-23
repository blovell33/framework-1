using System.Web.Mvc;
using Core;

namespace Website.Controllers.Mvc {
	[Authorize(Roles = Constants.Roles.ADMINISTRATOR_ROLE + "," + Constants.Roles.SYSTEM_MANAGER_ROLE)]
	[RoutePrefix("system")]
	[Route("{action=index}")]
	public class SystemController : Controller {
		public SystemController() {
			ViewBag.Title = "System";
		}

		[Route("{*segments}")]
		public ActionResult Index() {
			ViewBag.Sidebar = true;

			return View("DefaultView");
		}
	}
}