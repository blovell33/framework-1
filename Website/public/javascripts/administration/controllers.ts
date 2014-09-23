module Administration {
	"use strict";

	angular.module("app")
		.controller("main", [
			"$scope",
			"$provider",
			($scope: Shared.IAdministrationScope, $provider: Shared.IProviderService): void => {
				$provider.bootstrap($scope, {
					administration: "Administration",
					allusers: "All users",
					createuser: "Create user",
					user: "User detail",
					changepassword: "Change password",
					deleteuser: "Delete user",
					allroles: "All roles",
					role: "Role detail"
				});

				$scope.add("users", new Shared.Parameters());
				$scope.add("roles", new Shared.Parameters());

				$scope.createUser = (): void => {
					$scope.post("/api/administration/createuser", $scope.model);
				};

				$scope.updateUser = (): void => {
					$scope.post("/api/administration/updateuser", $scope.model);
				};

				$scope.changePassword = (): void => {
					$scope.post("/api/administration/changepassword", $scope.model);
				};

				$scope.deleteUser = (): void => {
					$scope.post("/api/administration/deleteuser", $scope.model);
				};
			}
		]);
}