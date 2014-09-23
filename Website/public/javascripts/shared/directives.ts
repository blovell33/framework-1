module Shared {
	"use strict";

	angular.module("app.directives", [])
		.directive("activate", [
			"$location",
			($location: ng.ILocationService): any => {
				return {
					restrict: "A",
					link: (scope: ng.IScope, element: JQuery, attrs: any): void => {
						scope.$watch((): string => $location.path(), (newValue: any): void => {
							if (attrs.href.match(new RegExp("^" + newValue + "$", "i"))) {
								element.addClass("active");
								$("#sidebar .panel-collapse").first().removeClass("in");
								element.parent().addClass("in");
							}
							else {
								element.removeClass("active");
							}
						});
					}
				};
			}
		])
		.directive("alerts", (): any => {
			return {
				restrict: "E",
				template: "<alert class=\"fixed\" ng-repeat=\"alert in alerts\" type=\"{{alert.type}}\" close=\"closeAlert($index)\" fade>{{alert.msg}}</alert>"
			};
		})
		.directive("fade", (): any => {
			return {
				restrict: "A",
				link: (scope: IBaseScope, element: JQuery): void => {
					element.delay(5000).fadeOut(400, (): void => {
						scope.closeAlert();
					});
				}
			};
		})
		.directive("breadcrumb", (): any => {
			return {
				restrict: "E",
				replace: true,
				scope: true,
				template: "<div><span ng-repeat=\"crumb in breadcrumb\"><span class=\"label text-capitalize\" ng-class=\"{'label-default': !$last, 'label-primary': $last}\">{{crumb}}</span>\n</span></div>",
				link: (scope: IBreadcrumbScope): void => {
					scope.breadcrumb = [];

					var keys: Array<string> = scope.path.split("/").splice(1);

					$.each(keys, (index: number, value: string): void => {
						var label: string = scope.labels[value];

						if (label) {
							scope.breadcrumb.push(label);
						}
					});
				}
			};
		})
		.directive("interactive", (): any => {
			return {
				restrict: "A",
				link: (scope: IBaseScope, element: JQuery): void => {
					scope.$watch("valid", (newValue: any): void => {
						if (newValue) {
							element.removeClass("has-error");
						}
						else {
							element.addClass("has-error");
							element.find("input").first().focus();
						}
					});
				}
			};
		})
		.directive("editable", (): any => {
			return {
				restrict: "A",
				link: (scope: IBaseScope, element: JQuery): void => {
					scope.readonly = true;

					var startWatching: Function = (): void => {
						scope.$watch("readonly", (newValue: any): void => {
							element.find("input").prop("disabled", newValue);
							element.find("select").prop("disabled", newValue);
							element.find("textarea").prop("disabled", newValue);
						});
					};
					var listener: Function = scope.$watch("ready", (newValue: any): void => {
						if (!newValue) {
							return;
						}

						startWatching();
						listener();
					});
				}
			};
		})
		.directive("load", [
			"$routeParams",
			($routeParams: ng.route.IRouteParamsService): any => {
				return {
					restrict: "A",
					link: (scope: IBaseScope, element: JQuery, attrs: any): void => {
						element.hide();

						angular.extend(scope.model, $routeParams);

						var path: string = scope.$eval(attrs.load);

						if (attrs.cacheKey) {
							// loads paged list
							var parameters: Parameters = scope.cache[attrs.cacheKey];

							scope.load(parameters.url(path));
						}
						else {
							// loads model
							scope.load(path);
						}

						var listener: Function = scope.$watch("ready", (newValue: any): void => {
							if (!newValue) {
								return;
							}

							element.show();

							listener();
						});
					}
				};
			}
		])
		.directive("autofocus", (): any => {
			return {
				restrict: "A",
				link: (scope: IBaseScope, element: JQuery): void => {
					element.focus();
				}
			};
		})
		.directive("captcha", (): any => {
			return {
				restrict: "E",
				replace: true,
				template: "<div class=\"checkbox\">" +
					"<label class=\"checkbox\">" +
					"<input type=\"checkbox\" value=\"valid\" required ng-model=\"model.captcha\" /> I am not a spambot" +
					"</label>" +
					"</div>"
			};
		})
		.directive("validate", [
			"$http", $http => {
				return {
					restrict: "A",
					require: "ngModel",
					link: (scope: IBaseScope, element: JQuery, attrs: any, controller: ng.INgModelController): void => {
						var parent: JQuery = element.closest(".form-group");
						parent.addClass("has-feedback");

						var feedback: JQuery = $("<span class=\"glyphicon form-control-feedback\"></span>");
						element.after(feedback);

						var callback: Function = (valid?: boolean, hint?: string): void => {
							controller.$setValidity("change", valid);

							var help: JQuery = parent.find(".help-block");

							if (scope.readonly || typeof (controller.$modelValue) === "undefined") {
								parent.removeClass("has-success");
								parent.removeClass("has-error");

								feedback.removeClass("glyphicon-ok");
								feedback.removeClass("glyphicon-remove");

								help.remove();

								return;
							}

							if (hint) {
								if (help.length === 0) {
									feedback.after("<span class=\"help-block\">" + hint + "</span>");
								}
								else {
									help.text(hint);
								}
							}
							else {
								help.remove();
							}

							if (valid) {
								parent.removeClass("has-error");
								feedback.removeClass("glyphicon-remove");

								parent.addClass("has-success");
								feedback.addClass("glyphicon-ok");
							}
							else {
								parent.removeClass("has-success");
								feedback.removeClass("glyphicon-ok");

								parent.addClass("has-error");
								feedback.addClass("glyphicon-remove");
							}
						};

						var validation: Validation = new Validation(scope, controller, $http, scope.$eval(attrs.validate), callback);

						validation.bootstrap();
					}
				};
			}
		])
		.directive("grid", (): any => {
			return {
				restrict: "E",
				scope: true,
				templateUrl: "/public/javascripts/shared/templates/grid.html",
				link: (scope: IGridScope, element: JQuery, attrs: any): void => {
					scope.parameters = scope.cache[attrs.cacheKey];
					scope.icon = attrs.icon ? attrs.icon : "fa-edit";

					var path: string = scope.$eval(attrs.load);

					scope.resize = (size: number): void => {
						scope.parameters.size = size;

						scope.load(scope.parameters.url(path));
					};

					scope.filter = (): void => {
						scope.load(scope.parameters.url(path));
					};

					scope.sort = (name: string): void => {
						if (scope.parameters.name === name) {
							if (scope.parameters.direction === "asc") {
								scope.parameters.direction = "desc";
							}
							else if (scope.parameters.direction === "desc") {
								delete scope.parameters.name;
								delete scope.parameters.direction;
							}
						}
						else {
							scope.parameters.name = name;
							scope.parameters.direction = "asc";
						}

						scope.load(scope.parameters.url(path));
					};

					scope.page = (): void => {
						scope.parameters.index = scope.model.stats.page - 1;
						scope.load(scope.parameters.url(path));
					};
				}
			};
		});
}