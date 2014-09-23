using System.Web.Mvc;
using System.Web.Routing;

namespace Website {
	public static class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapMvcAttributeRoutes();

			routes.MapRoute("Home", "{*segments}", new {controller = "Home", action = "Index"});
		}
	}
}