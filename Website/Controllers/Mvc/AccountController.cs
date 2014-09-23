using System.Web.Mvc;
using System.Web.Security;
using Core;

namespace Website.Controllers.Mvc {
	[Authorize]
	[RoutePrefix("account")]
	[Route("{action=index}")]
	public class AccountController : Controller {
		private readonly ApplicationSettings _settings;

		public AccountController(ApplicationSettings settings) {
			_settings = settings;

			ViewBag.Title = "Account";
		}

		[Route("{*segments}")]
		public ActionResult Index() {
			ViewBag.Sidebar = true;

			return View("DefaultView");
		}

		[AllowAnonymous]
		[Route("register/{*segments}")]
		[Route("activate/{*segments}")]
		public ActionResult Registration() {
			if (!_settings.CanRegister) {
				return HttpNotFound();
			}

			return View("PublicView");
		}

		[AllowAnonymous]
		[Route("signin/{*segments}")]
		[Route("forgotpassword/{*segments}")]
		[Route("resetpassword/{*segments}")]
		public ActionResult Anonymous() {
			return View("PublicView");
		}

		[Route("signout/{*segments}")]
		public ActionResult SignOut() {
			FormsAuthentication.SignOut();

			return Redirect("/");
		}

		[AllowAnonymous]
		[Route("templates/register")]
		public ActionResult RegisterTemplate() {
			return PartialView("Register");
		}

		[AllowAnonymous]
		[Route("templates/signin")]
		public ActionResult SignInTemplate() {
			return PartialView("SignIn");
		}

		[AllowAnonymous]
		[Route("templates/forgotpassword")]
		public ActionResult ForgotPasswordTemplate() {
			return PartialView("ForgotPassword");
		}
	}
}