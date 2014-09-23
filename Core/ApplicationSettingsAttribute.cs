using System.Dynamic;
using System.Web.Mvc;

namespace Core {
	public class ApplicationSettingsAttribute : ActionFilterAttribute {
		private readonly ApplicationSettings _settings;

		public ApplicationSettingsAttribute(ApplicationSettings settings) {
			_settings = settings;
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext) {
			dynamic application = new ExpandoObject();

			application.Settings = _settings;

			filterContext.Controller.ViewBag.Application = application;
		}
	}
}