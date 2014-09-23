using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Core;
using Newtonsoft.Json.Linq;

namespace Website {
	public class Global : HttpApplication {
		private void Application_Start(object sender, EventArgs e) {
			// Code that runs on application startup
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			DatabaseConfig.Configure();
		}

		private void Application_AuthenticateRequest(Object sender, EventArgs e) {
			if (HttpContext.Current.User == null) {
				return;
			}

			var cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

			if (cookie == null) {
				return;
			}

			var decryptedTicket = FormsAuthentication.Decrypt(cookie.Value);

			if (decryptedTicket == null) {
				return;
			}

			dynamic data = JObject.Parse(decryptedTicket.UserData);

			int id = data.id;
			string firstName = data.firstName;
			string lastName = data.lastName;
			string[] roles = data.roles.ToObject<string[]>();

			var principal = new CustomPrincipal(HttpContext.Current.User.Identity, roles, id, firstName, lastName);

			HttpContext.Current.User = principal;
		}
	}
}