using System.Web.Mvc;

namespace Core {
	public class SecureConnectionAttribute : ActionFilterAttribute {
		private readonly ApplicationSettings _settings;

		public SecureConnectionAttribute(ApplicationSettings settings) {
			_settings = settings;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			if (!_settings.UseHttps) {
				return;
			}

			var request = filterContext.HttpContext.Request;
			var response = filterContext.HttpContext.Response;

			if (!request.IsSecureConnection && request.Url != null) {
				response.Redirect(request.Url.ToString().Replace("http:", "https:"));
			}
		}
	}
}