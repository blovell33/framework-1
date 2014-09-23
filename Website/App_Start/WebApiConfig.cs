using System.Web.Http;

namespace Website {
	public static class WebApiConfig {
		public static void Register(HttpConfiguration config) {
			config.MapHttpAttributeRoutes();
		}
	}
}