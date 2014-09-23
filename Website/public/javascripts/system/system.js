///#source 1 1 /public/javascripts/shared/constants.js
var Shared;
(function (Shared) {
    "use strict";

    (function (ValidationType) {
        ValidationType[ValidationType["ExistingUserByUsername"] = 0] = "ExistingUserByUsername";
        ValidationType[ValidationType["NewUserByUsername"] = 1] = "NewUserByUsername";
        ValidationType[ValidationType["ExistingUserByEmail"] = 2] = "ExistingUserByEmail";
        ValidationType[ValidationType["NewUserByEmail"] = 3] = "NewUserByEmail";
        ValidationType[ValidationType["CurrentUserByEmail"] = 4] = "CurrentUserByEmail";
        ValidationType[ValidationType["OldPassword"] = 5] = "OldPassword";
        ValidationType[ValidationType["IsRequired"] = 6] = "IsRequired";
        ValidationType[ValidationType["Username"] = 7] = "Username";
        ValidationType[ValidationType["Email"] = 8] = "Email";
        ValidationType[ValidationType["Password"] = 9] = "Password";
        ValidationType[ValidationType["Compare"] = 10] = "Compare";
        ValidationType[ValidationType["GreaterThan"] = 11] = "GreaterThan";
        ValidationType[ValidationType["GreaterThanEqualTo"] = 12] = "GreaterThanEqualTo";
        ValidationType[ValidationType["LessThan"] = 13] = "LessThan";
        ValidationType[ValidationType["LessThanEqualTo"] = 14] = "LessThanEqualTo";
    })(Shared.ValidationType || (Shared.ValidationType = {}));
    var ValidationType = Shared.ValidationType;
})(Shared || (Shared = {}));
//# sourceMappingURL=constants.js.map

///#source 1 1 /public/javascripts/shared/initializer.js
var Shared;
(function (Shared) {
    "use strict";

    var Initializer = (function () {
        function Initializer(scope, labels, http, route, location, cache) {
            this.scope = scope;
            this.labels = labels;
            this.http = http;
            this.route = route;
            this.location = location;
            this.cache = cache;
        }
        Initializer.prototype.bootstrap = function () {
            var _this = this;
            this.scope.ready = false;
            this.scope.loading = false;
            this.scope.valid = true;
            this.scope.success = false;
            this.scope.readonly = null;
            this.scope.model = {};
            this.scope.labels = this.labels;
            this.scope.alerts = [];
            this.scope.cache = this.cache;
            this.scope.path = this.location.path();

            var redirect = this.location.search().redirect;

            if (redirect && redirect.length !== 0 && redirect.substring(0, 1) === "/") {
                this.scope.redirect = redirect;
            }

            this.scope.add = function (key, value) {
                if (typeof (_this.cache[key]) === "undefined") {
                    _this.cache[key] = value;
                }
            };

            this.scope.closeAlert = function (index) {
                if (typeof index === "undefined") { index = 0; }
                _this.scope.alerts.splice(index, 1);
            };

            this.scope.load = function (url) {
                _this.begin();

                _this.http.get(url).success(function (result) {
                    _this.loadComplete(result);
                }).error(function (result) {
                    _this.error(result);
                });
            };

            this.scope.post = function (url, model, callback) {
                _this.begin();

                _this.http.post(url, model).success(function (result) {
                    if (callback) {
                        callback();
                    } else {
                        _this.postComplete(result);
                    }
                }).error(function (result) {
                    _this.error(result);
                });
            };

            this.scope.edit = function () {
                _this.original = {};

                _this.flush();

                _this.scope.readonly = false;
            };

            this.scope.view = function () {
                _this.scope.readonly = true;

                angular.extend(_this.scope.model, _this.original);
            };

            this.scope.reset = function () {
                _this.route.reload();
            };
        };

        Initializer.prototype.begin = function () {
            this.scope.loading = true;
            this.scope.valid = true;

            if (typeof (this.scope.readonly) === "boolean" && !this.scope.readonly) {
                this.flush();
            }
        };

        Initializer.prototype.end = function () {
            this.scope.loading = false;
        };

        Initializer.prototype.loadComplete = function (result) {
            if (result.data) {
                angular.extend(this.scope.model, result.data);

                this.end();

                this.scope.ready = true;
            } else {
                this.location.path(this.route.routes[null].redirectTo);
            }
        };

        Initializer.prototype.postComplete = function (result) {
            if (result.data) {
                angular.extend(this.scope.model, result.data);

                if (typeof (this.scope.readonly) === "boolean" && !this.scope.readonly) {
                    this.flush();
                }
            }

            this.scope.success = true;

            if (!this.scope.readonly) {
                this.scope.view();
            }

            this.end();
            this.pushAlert(result.message, "success");
        };

        Initializer.prototype.error = function (result) {
            this.scope.valid = false;

            this.end();
            this.pushAlert(result.message, "danger");
        };

        Initializer.prototype.pushAlert = function (msg, type) {
            if (!msg) {
                return;
            }

            this.scope.alerts.push({
                msg: msg,
                type: type
            });
        };

        Initializer.prototype.flush = function () {
            angular.extend(this.original, this.scope.model);
        };
        return Initializer;
    })();
    Shared.Initializer = Initializer;
})(Shared || (Shared = {}));
//# sourceMappingURL=initializer.js.map

///#source 1 1 /public/javascripts/shared/parameters.js
var Shared;
(function (Shared) {
    "use strict";

    var Parameters = (function () {
        function Parameters() {
            this.size = 10;
        }
        Parameters.prototype.url = function (path) {
            var url = path;
            var separator = "?";

            if (this.index) {
                url += (separator + "index=" + this.index);
                separator = "&";
            }

            if (this.size) {
                url += (separator + "size=" + this.size);
                separator = "&";
            }

            if (this.name) {
                url += (separator + "name=" + this.name);
                separator = "&";
            }

            if (this.direction) {
                url += (separator + "direction=" + this.direction);
                separator = "&";
            }

            if (this.filter) {
                url += (separator + "filter=" + encodeURIComponent(this.filter));
            }

            return url;
        };
        return Parameters;
    })();
    Shared.Parameters = Parameters;
})(Shared || (Shared = {}));
//# sourceMappingURL=parameters.js.map

///#source 1 1 /public/javascripts/shared/validation.js
var Shared;
(function (Shared) {
    "use strict";

    var Validation = (function () {
        function Validation(scope, controller, http, options, callback) {
            this.scope = scope;
            this.controller = controller;
            this.http = http;
            this.options = options;
            this.callback = callback;
            this.type = Shared.ValidationType[this.options.type];
        }
        Validation.prototype.bootstrap = function () {
            var _this = this;
            this.scope.$watch("readonly", function () {
                _this.validate();
            });

            this.scope.$watch(function () {
                return _this.controller.$modelValue;
            }, function () {
                _this.validate();
            });

            if (this.type === 10 /* Compare */) {
                this.scope.$watch(function () {
                    return _this.scope.model[_this.options.compareTo];
                }, function () {
                    _this.validate();
                });
            }
        };

        Validation.prototype.validate = function () {
            var _this = this;
            if (this.scope.readonly || typeof (this.controller.$modelValue) === "undefined") {
                this.callback();

                return;
            }

            var model = {};

            switch (this.type) {
                case 0 /* ExistingUserByUsername */:
                case 1 /* NewUserByUsername */:
                    model.username = this.controller.$modelValue;

                    break;
                case 2 /* ExistingUserByEmail */:
                case 3 /* NewUserByEmail */:
                    model.email = this.controller.$modelValue;

                    break;
                case 4 /* CurrentUserByEmail */:
                    model.username = this.scope.model.username;
                    model.email = this.controller.$modelValue;

                    break;
                case 5 /* OldPassword */:
                    model.oldPassword = this.controller.$modelValue;

                    break;
                case 6 /* IsRequired */:
                    model.value = this.controller.$modelValue;

                    break;
                case 7 /* Username */:
                    model.username = this.controller.$modelValue;

                    break;
                case 8 /* Email */:
                    model.email = this.controller.$modelValue;

                    break;
                case 9 /* Password */:
                    model.password = this.controller.$modelValue;

                    break;
                case 10 /* Compare */:
                    model.compare = this.controller.$modelValue;
                    model.compareTo = this.scope.model[this.options.compareTo];

                    break;
                case 11 /* GreaterThan */:
                case 12 /* GreaterThanEqualTo */:
                case 13 /* LessThan */:
                case 14 /* LessThanEqualTo */:
                    model.compare = this.controller.$modelValue;

                    if (this.options.compareTo) {
                        model.compareTo = this.scope.model[this.options.compareTo];
                    } else {
                        model.compareTo = this.options.value;
                    }

                    break;
            }

            var url = "/api/validation/" + Shared.ValidationType[this.type].toLowerCase();

            this.http.post(url, model).success(function (result) {
                var hint = result.data ? result.data.hint : null;

                _this.callback(true, hint);
            }).error(function (result) {
                var hint = result.data ? result.data.hint : null;

                _this.callback(false, hint);
            });
        };
        return Validation;
    })();
    Shared.Validation = Validation;
})(Shared || (Shared = {}));
//# sourceMappingURL=validation.js.map

///#source 1 1 /public/javascripts/shared/services.js
var Shared;
(function (Shared) {
    "use strict";

    angular.module("app.services", []).service("$provider", [
        "$http", "$route", "$location", function (http, route, location) {
            var _this = this;
            this.bootstrap = function (scope, labels) {
                var initializer = new Shared.Initializer(scope, labels, http, route, location, _this.cache);

                initializer.bootstrap();
            };

            this.cache = {};
        }
    ]);
})(Shared || (Shared = {}));
//# sourceMappingURL=services.js.map

///#source 1 1 /public/javascripts/shared/directives.js
var Shared;
(function (Shared) {
    "use strict";

    angular.module("app.directives", []).directive("activate", [
        "$location",
        function ($location) {
            return {
                restrict: "A",
                link: function (scope, element, attrs) {
                    scope.$watch(function () {
                        return $location.path();
                    }, function (newValue) {
                        if (attrs.href.match(new RegExp("^" + newValue + "$", "i"))) {
                            element.addClass("active");
                            $("#sidebar .panel-collapse").first().removeClass("in");
                            element.parent().addClass("in");
                        } else {
                            element.removeClass("active");
                        }
                    });
                }
            };
        }
    ]).directive("alerts", function () {
        return {
            restrict: "E",
            template: "<alert class=\"fixed\" ng-repeat=\"alert in alerts\" type=\"{{alert.type}}\" close=\"closeAlert($index)\" fade>{{alert.msg}}</alert>"
        };
    }).directive("fade", function () {
        return {
            restrict: "A",
            link: function (scope, element) {
                element.delay(5000).fadeOut(400, function () {
                    scope.closeAlert();
                });
            }
        };
    }).directive("breadcrumb", function () {
        return {
            restrict: "E",
            replace: true,
            scope: true,
            template: "<div><span ng-repeat=\"crumb in breadcrumb\"><span class=\"label text-capitalize\" ng-class=\"{'label-default': !$last, 'label-primary': $last}\">{{crumb}}</span>\n</span></div>",
            link: function (scope) {
                scope.breadcrumb = [];

                var keys = scope.path.split("/").splice(1);

                $.each(keys, function (index, value) {
                    var label = scope.labels[value];

                    if (label) {
                        scope.breadcrumb.push(label);
                    }
                });
            }
        };
    }).directive("interactive", function () {
        return {
            restrict: "A",
            link: function (scope, element) {
                scope.$watch("valid", function (newValue) {
                    if (newValue) {
                        element.removeClass("has-error");
                    } else {
                        element.addClass("has-error");
                        element.find("input").first().focus();
                    }
                });
            }
        };
    }).directive("editable", function () {
        return {
            restrict: "A",
            link: function (scope, element) {
                scope.readonly = true;

                var startWatching = function () {
                    scope.$watch("readonly", function (newValue) {
                        element.find("input").prop("disabled", newValue);
                        element.find("select").prop("disabled", newValue);
                        element.find("textarea").prop("disabled", newValue);
                    });
                };
                var listener = scope.$watch("ready", function (newValue) {
                    if (!newValue) {
                        return;
                    }

                    startWatching();
                    listener();
                });
            }
        };
    }).directive("load", [
        "$routeParams",
        function ($routeParams) {
            return {
                restrict: "A",
                link: function (scope, element, attrs) {
                    element.hide();

                    angular.extend(scope.model, $routeParams);

                    var path = scope.$eval(attrs.load);

                    if (attrs.cacheKey) {
                        // loads paged list
                        var parameters = scope.cache[attrs.cacheKey];

                        scope.load(parameters.url(path));
                    } else {
                        // loads model
                        scope.load(path);
                    }

                    var listener = scope.$watch("ready", function (newValue) {
                        if (!newValue) {
                            return;
                        }

                        element.show();

                        listener();
                    });
                }
            };
        }
    ]).directive("autofocus", function () {
        return {
            restrict: "A",
            link: function (scope, element) {
                element.focus();
            }
        };
    }).directive("captcha", function () {
        return {
            restrict: "E",
            replace: true,
            template: "<div class=\"checkbox\">" + "<label class=\"checkbox\">" + "<input type=\"checkbox\" value=\"valid\" required ng-model=\"model.captcha\" /> I am not a spambot" + "</label>" + "</div>"
        };
    }).directive("validate", [
        "$http", function ($http) {
            return {
                restrict: "A",
                require: "ngModel",
                link: function (scope, element, attrs, controller) {
                    var parent = element.closest(".form-group");
                    parent.addClass("has-feedback");

                    var feedback = $("<span class=\"glyphicon form-control-feedback\"></span>");
                    element.after(feedback);

                    var callback = function (valid, hint) {
                        controller.$setValidity("change", valid);

                        var help = parent.find(".help-block");

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
                            } else {
                                help.text(hint);
                            }
                        } else {
                            help.remove();
                        }

                        if (valid) {
                            parent.removeClass("has-error");
                            feedback.removeClass("glyphicon-remove");

                            parent.addClass("has-success");
                            feedback.addClass("glyphicon-ok");
                        } else {
                            parent.removeClass("has-success");
                            feedback.removeClass("glyphicon-ok");

                            parent.addClass("has-error");
                            feedback.addClass("glyphicon-remove");
                        }
                    };

                    var validation = new Shared.Validation(scope, controller, $http, scope.$eval(attrs.validate), callback);

                    validation.bootstrap();
                }
            };
        }
    ]).directive("grid", function () {
        return {
            restrict: "E",
            scope: true,
            templateUrl: "/public/javascripts/shared/templates/grid.html",
            link: function (scope, element, attrs) {
                scope.parameters = scope.cache[attrs.cacheKey];
                scope.icon = attrs.icon ? attrs.icon : "fa-edit";

                var path = scope.$eval(attrs.load);

                scope.resize = function (size) {
                    scope.parameters.size = size;

                    scope.load(scope.parameters.url(path));
                };

                scope.filter = function () {
                    scope.load(scope.parameters.url(path));
                };

                scope.sort = function (name) {
                    if (scope.parameters.name === name) {
                        if (scope.parameters.direction === "asc") {
                            scope.parameters.direction = "desc";
                        } else if (scope.parameters.direction === "desc") {
                            delete scope.parameters.name;
                            delete scope.parameters.direction;
                        }
                    } else {
                        scope.parameters.name = name;
                        scope.parameters.direction = "asc";
                    }

                    scope.load(scope.parameters.url(path));
                };

                scope.page = function () {
                    scope.parameters.index = scope.model.stats.page - 1;
                    scope.load(scope.parameters.url(path));
                };
            }
        };
    });
})(Shared || (Shared = {}));
//# sourceMappingURL=directives.js.map

///#source 1 1 /public/javascripts/system/app.js
var Home;
(function (Home) {
    "use strict";

    angular.module("app", [
        "ngRoute",
        "ui.bootstrap",
        "app.services",
        "app.directives"
    ]).config([
        "$routeProvider",
        "$locationProvider",
        function ($routeProvider, $locationProvider) {
            $routeProvider.when("/system", { controller: "main", templateUrl: "/public/javascripts/system/templates/index.html" }).otherwise({ redirectTo: "/system" });

            $locationProvider.html5Mode(true);
        }
    ]);
})(Home || (Home = {}));
//# sourceMappingURL=app.js.map

///#source 1 1 /public/javascripts/system/controllers.js
var System;
(function (System) {
    "use strict";

    angular.module("app").controller("main", [
        "$scope",
        "$provider",
        "$location",
        function ($scope, $provider, $location) {
            $provider.bootstrap($scope, {
                system: "System"
            });
        }
    ]);
})(System || (System = {}));
//# sourceMappingURL=controllers.js.map

