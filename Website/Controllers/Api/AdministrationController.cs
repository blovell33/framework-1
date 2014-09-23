using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core;
using Persistence.Components;
using Persistence.Services;

namespace Website.Controllers.Api {
	[Authorize(Roles = Constants.Roles.ADMINISTRATOR_ROLE)]
	[RoutePrefix("api/administration")]
	[HandleExceptions]
	public class AdministrationController : ApiController {
		private readonly AdministrationService _administrationService;

		public AdministrationController(AdministrationService administrationService) {
			_administrationService = administrationService;
		}

		[Route("getallusers")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetAllUsersAsync(int index = 0, int size = 10, string name = null, string direction = "asc", string filter = null) {
			var parameters = new QueryParameters(index, size, name, direction, filter);
			var model = await _administrationService.GetAllUsersAsync(parameters);

			return Request.OK(model);
		}

		[Route("getnewuser")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetNewUserAsync() {
			var model = await _administrationService.GetNewUserAsync();

			return Request.OK(model);
		}

		[Route("getuser/{username}")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetUserAsync(string username) {
			var model = await _administrationService.GetUserAsync(username);

			return Request.OK(model);
		}

		[Route("createuser")]
		[HttpPost]
		public async Task<HttpResponseMessage> CreateUserAsync(dynamic model) {
			await _administrationService.CreateUserAsync(model);

			return Request.OK("Create successful.");
		}

		[Route("updateuser")]
		[HttpPost]
		public async Task<HttpResponseMessage> UpdateUserAsync(dynamic model) {
			await _administrationService.UpdateUserAsync(model);

			return Request.OK("Update successful.");
		}

		[Route("changepassword")]
		[HttpPost]
		public async Task<HttpResponseMessage> ChangePasswordAsync(dynamic model) {
			await _administrationService.ChangePasswordAsync(model);

			return Request.OK("Password change successful.");
		}

		[Route("deleteuser")]
		[HttpPost]
		public async Task<HttpResponseMessage> DeleteUserAsync(dynamic model) {
			await _administrationService.DeleteUserAsync(model);

			return Request.OK("Delete successful.");
		}

		[Route("getallroles")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetAllRolesAsync(int index = 0, int size = 10, string name = null, string direction = "asc", string filter = null) {
			var parameters = new QueryParameters(index, size, name, direction, filter);
			var model = await _administrationService.GetAllRolesAsync(parameters);

			return Request.OK(model);
		}

		[Route("getrole/{name}")]
		[HttpGet]
		public async Task<HttpResponseMessage> GetRoleAsync(string name) {
			var model = await _administrationService.GetRoleAsync(name);

			return Request.OK(model);
		}
	}
}