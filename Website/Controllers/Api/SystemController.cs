using System.Web.Http;
using Core;
using Persistence.Services;

namespace Website.Controllers.Api {
	[Authorize(Roles = Constants.Roles.ADMINISTRATOR_ROLE + "," + Constants.Roles.SYSTEM_MANAGER_ROLE)]
	[RoutePrefix("api/system")]
	[HandleExceptions]
	public class SystemController : ApiController {
		private readonly SystemService _systemService;

		public SystemController(SystemService systemService) {
			_systemService = systemService;
		}
	}
}