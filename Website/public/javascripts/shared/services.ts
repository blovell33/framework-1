module Shared {
	"use strict";

	angular.module("app.services", [])
		.service("$provider", [
			"$http", "$route", "$location", function (http: ng.IHttpService, route: ng.route.IRouteService, location: ng.ILocationService): void {
				this.bootstrap = (scope: IBaseScope, labels: any): void => {
					var initializer: Initializer = new Initializer(scope, labels, http, route, location, this.cache);

					initializer.bootstrap();
				};

				this.cache = {};
			}
		]);
}