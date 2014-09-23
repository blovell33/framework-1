module Administration {
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
					.when("/administration", { controller: "main", templateUrl: "/public/javascripts/administration/templates/index.html" })
					.when("/administration/allusers", { controller: "main", templateUrl: "/public/javascripts/administration/templates/allusers.html" })
					.when("/administration/createuser", { controller: "main", templateUrl: "/public/javascripts/administration/templates/createuser.html" })
					.when("/administration/user/:username", { controller: "main", templateUrl: "/public/javascripts/administration/templates/user.html" })
					.when("/administration/changepassword/:username", { controller: "main", templateUrl: "/public/javascripts/administration/templates/changepassword.html" })
					.when("/administration/deleteuser/:username", { controller: "main", templateUrl: "/public/javascripts/administration/templates/deleteuser.html" })
					.when("/administration/allroles", { controller: "main", templateUrl: "/public/javascripts/administration/templates/allroles.html" })
					.when("/administration/role/:name", { controller: "main", templateUrl: "/public/javascripts/administration/templates/role.html" })
					.otherwise({ redirectTo: "/administration" });

				$locationProvider.html5Mode(true);
			}
		]);
}