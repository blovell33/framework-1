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
					.when("/", { templateUrl: "/public/javascripts/home/templates/index.html" })
					.otherwise({ redirectTo: "/" });

				$locationProvider.html5Mode(true);
			}
		]);
}