module Home {
	"use strict";

	angular.module("app", [
			"ngRoute",
			"ui.bootstrap",
			"app.services",
			"app.directives"
		])
		.config([
			"$routeProvider",
			"$locationProvider",
			($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider): void => {
				$routeProvider
					.when("/system", { controller: "main", templateUrl: "/public/javascripts/system/templates/index.html" })
					.otherwise({ redirectTo: "/system" });

				$locationProvider.html5Mode(true);
			}
		]);
}