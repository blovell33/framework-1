using System.Dynamic;
using System.Web.Mvc;

namespace Core {
	public class MailSettingsAttribute : ActionFilterAttribute {
		private readonly MailSettings _settings;

		public MailSettingsAttribute(MailSettings settings) {
			_settings = settings;
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext) {
			dynamic mail = new ExpandoObject();

			mail.Settings = _settings;

			filterContext.Controller.ViewBag.Mail = mail;
		}
	}
}