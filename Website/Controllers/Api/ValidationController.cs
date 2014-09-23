using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core;
using Persistence.Services;

namespace Website.Controllers.Api {
	[RoutePrefix("api/validation")]
	public class ValidationController : ApiController {
		private readonly ValidationService _validationService;

		public ValidationController(ValidationService validationService) {
			_validationService = validationService;
		}

		[Route("existinguserbyusername")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateExistingUserByUsernameAsync(dynamic model) {
			string username = model.username;

			await _validationService.ValidateExistingUserByUsernameAsync(username);

			return Request.NoContent();
		}

		[Route("newuserbyusername")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateNewUserByUsernameAsync(dynamic model) {
			string username = model.username;

			await _validationService.ValidateNewUserByUsernameAsync(username);

			return Request.NoContent();
		}

		[Route("existinguserbyemail")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateExistingUserByEmailAsync(dynamic model) {
			string email = model.email;

			await _validationService.ValidateExistingUserByEmailAsync(email);

			return Request.NoContent();
		}

		[Route("newuserbyemail")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateNewUserByEmailAsync(dynamic model) {
			string email = model.email;

			await _validationService.ValidateNewUserByEmailAsync(email);

			return Request.NoContent();
		}

		[Route("currentuserbyemail")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateCurrentUserByEmailAsync(dynamic model) {
			string username = model.username;
			string email = model.email;

			if (string.IsNullOrWhiteSpace(username)) {
				username = User.Identity.Name;
			}

			await _validationService.ValidateCurrentUserByEmailAsync(username, email);

			return Request.NoContent();
		}

		[Route("oldpassword")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public async Task<HttpResponseMessage> ValidateOldPasswordAsync(dynamic model) {
			var username = User.Identity.Name;
			string oldPassword = model.oldPassword;

			await _validationService.ValidateOldPasswordAsync(username, oldPassword);

			return Request.NoContent();
		}

		[Route("isrequired")]
		[HandleExceptions]
		[HttpPost]
		public HttpResponseMessage ValidateIsRequired(dynamic model) {
			string value = model.value;

			_validationService.ValidateIsRequired(value);

			return Request.NoContent();
		}

		[Route("username")]
		[HandleExceptions]
		[HttpPost]
		public HttpResponseMessage ValidateUsername(dynamic model) {
			string username = model.username;

			_validationService.ValidateUsername(username);

			return Request.NoContent();
		}

		[Route("email")]
		[HandleExceptions]
		[HttpPost]
		public HttpResponseMessage ValidateEmail(dynamic model) {
			string email = model.email;

			_validationService.ValidateEmail(email);

			return Request.NoContent();
		}

		[Route("password")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidatePassword(dynamic model) {
			string password = model.password;

			var hint = _validationService.ValidatePassword(password);

			return Request.OK(new {
				hint
			});
		}

		[Route("compare")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidateComparison(dynamic model) {
			string compare = model.compare;
			string compareTo = model.compareTo;

			_validationService.ValidateComparison(compare, compareTo);

			return Request.NoContent();
		}

		[Route("greaterthan")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidateGreaterThan(dynamic model) {
			string compare = model.compare;
			string compareTo = model.compareTo;

			_validationService.ValidateGreaterThan(compare, compareTo);

			return Request.NoContent();
		}

		[Route("greaterthanequalto")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidateGreaterThanEqualTo(dynamic model) {
			string compare = model.compare;
			string compareTo = model.compareTo;

			_validationService.ValidateGreaterThanEqualTo(compare, compareTo);

			return Request.NoContent();
		}

		[Route("lessthan")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidateLessThan(dynamic model) {
			string compare = model.compare;
			string compareTo = model.compareTo;

			_validationService.ValidateLessThan(compare, compareTo);

			return Request.NoContent();
		}

		[Route("lessthanequalto")]
		[HandleExceptions(WithHint = true)]
		[HttpPost]
		public HttpResponseMessage ValidateLessThanEqualTo(dynamic model) {
			string compare = model.compare;
			string compareTo = model.compareTo;

			_validationService.ValidateLessThanEqualTo(compare, compareTo);

			return Request.NoContent();
		}
	}
}