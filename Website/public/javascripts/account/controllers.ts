module Account {
	"use strict";

	angular.module("app")
		.controller("main", [
			"$scope",
			"$provider",
			"$location",
			($scope: Shared.IAccountScope, $provider: Shared.IProviderService, $location: ng.ILocationService): void => {
				$provider.bootstrap($scope, {
					account: "Account",
					personalinformation: "Personal information",
					changepassword: "Change password"
				});

				$scope.register = (): void => {
					$scope.post("/api/account/register", $scope.model);
				};

				$scope.activate = (): void => {
					$scope.post("/api/account/activate", $location.search());
				};

				$scope.signIn = (): void => {
					$scope.post("/api/account/signin", $scope.model, (): void => {
						location.href = $scope.redirect ? $scope.redirect : "/";
					});
				};

				$scope.retrievePassword = (): void => {
					$scope.post("/api/account/retrievepassword", $scope.model);
				};

				$scope.resetPassword = (): void => {
					$scope.post("/api/account/resetpassword", angular.extend($scope.model, $location.search()));
				};

				$scope.changePassword = (): void => {
					$scope.post("/api/account/changepassword", $scope.model);
				};

				$scope.updatePersonalInformation = (): void => {
					$scope.post("/api/account/updatepersonalinformation", $scope.model);
				};
			}
		]);
}