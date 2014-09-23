module Account {
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
					.when("/account", { controller: "main", templateUrl: "public/javascripts/account/templates/index.html" })
					.when("/account/register", { controller: "main", templateUrl: "/account/templates/register" })
					.when("/account/activate", { controller: "main", templateUrl: "/public/javascripts/account/templates/activate.html" })
					.when("/account/signin", { controller: "main", templateUrl: "/account/templates/signin" })
					.when("/account/forgotpassword", { controller: "main", templateUrl: "/account/templates/forgotpassword" })
					.when("/account/resetpassword", { controller: "main", templateUrl: "/public/javascripts/account/templates/resetpassword.html" })
					.when("/account/changepassword", { controller: "main", templateUrl: "/public/javascripts/account/templates/changepassword.html" })
					.when("/account/personalinformation", { controller: "main", templateUrl: "/public/javascripts/account/templates/personalinformation.html" })
					.otherwise({ redirectTo: "/account/signin" });

				$locationProvider.html5Mode(true);
			}
		]);
}