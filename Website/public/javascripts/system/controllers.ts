module System {
	"use strict";

	angular.module("app")
		.controller("main", [
			"$scope",
			"$provider",
			"$location",
			($scope: Shared.IAccountScope, $provider: Shared.IProviderService, $location: ng.ILocationService): void => {
				$provider.bootstrap($scope, {
					system: "System"
				});
			}
		]);
}