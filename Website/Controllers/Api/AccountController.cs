using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Core;
using Persistence.Services;

namespace Website.Controllers.Api {
	[Authorize]
	[RoutePrefix("api/account")]
	[HandleExceptions]
	public class AccountController : ApiController {
		private readonly AccountService _accountService;

		public AccountController(AccountService accountService) {
			_accountService = accountService;
		}

		[AllowAnonymous]
		[Route("register")]
		[HttpPost]
		public async Task<HttpResponseMessage> RegisterAsync(dynamic model) {
			await _accountService.RegisterAsync(model);

			return Request.OK("Registration successful.");
		}

		[AllowAnonymous]
		[Route("activate")]
		[HttpPost]
		public async Task<HttpResponseMessage> ActivateAsync(dynamic model) {
			await _accountService.ActivateAsync(model);

			return Request.OK("Activation successful.");
		}

		[AllowAnonymous]
		[Route("signin")]
		[HttpPost]
		public async Task<HttpResponseMessage> SignInAsync(dynamic model) {
			await _accountService.SignInAsync(new HttpContextWrapper(HttpContext.Current), model);

			return Request.NoContent();
		}

		[AllowAnonymous]
		[Route("retrievepassword")]
		[HttpPost]
		public async Task<HttpResponseMessage> RetrievePasswordAsync(dynamic model) {
			await _accountService.RetrievePasswordAsync(model);

			return Request.OK("Password retrieval successful.");
		}

		[AllowAnonymous]
		[Route("resetpassword")]
		[HttpPost]
		public async Task<HttpResponseMessage> ResetPasswordAsync(dynamic model) {
			await _accountService.ResetPasswordAsync(model);

			return Request.OK("Password reset successful.");
		}

		[Route("changepassword")]
		[HttpPost]
		public async Task<HttpResponseMessage> ChangePasswordAsync(dynamic model) {
			var username = User.Identity.Name;

			await _accountService.ChangePasswordAsync(username, model);

			return Request.OK("Password change successful.");
		}

		[Route("getpersonalinformation")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetPersonalInformationAsync() {
			var username = User.Identity.Name;

			var model = await _accountService.GetPersonalInformationAsync(username);

			return Request.OK(model);
		}

		[Route("updatepersonalinformation")]
		[HttpPost]
		public async Task<HttpResponseMessage> UpdatePersonalInformationAsync(dynamic model) {
			var username = User.Identity.Name;

			await _accountService.UpdatePersonalInformationAsync(new HttpContextWrapper(HttpContext.Current), username, model);

			return Request.OK("Update successful.");
		}
	}
}