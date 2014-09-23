module Shared {
	"use strict";

	export class Initializer {
		private original: any;

		constructor(private scope: IBaseScope, private labels: any, private http: ng.IHttpService, private route: ng.route.IRouteService, private location: ng.ILocationService, private cache: any) {
		}

		public bootstrap(): void {
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

			var redirect: string = this.location.search().redirect;

			if (redirect && redirect.length !== 0 && redirect.substring(0, 1) === "/") {
				this.scope.redirect = redirect;
			}

			this.scope.add = (key: string, value: any): void => {
				if (typeof (this.cache[key]) === "undefined") {
					this.cache[key] = value;
				}
			};

			this.scope.closeAlert = (index: number = 0): void => {
				this.scope.alerts.splice(index, 1);
			};

			this.scope.load = (url: string): void => {
				this.begin();

				this.http.get(url)
					.success((result: any): void => {
						this.loadComplete(result);
					})
					.error((result: any): void => {
						this.error(result);
					});
			};

			this.scope.post = (url: string, model: any, callback?: Function): void => {
				this.begin();

				this.http.post(url, model)
					.success((result: any): void => {
						if (callback) {
							callback();
						}
						else {
							this.postComplete(result);
						}
					})
					.error((result: any): void => {
						this.error(result);
					});
			};

			this.scope.edit = (): void => {
				this.original = {};

				this.flush();

				this.scope.readonly = false;
			};

			this.scope.view = (): void => {
				this.scope.readonly = true;

				angular.extend(this.scope.model, this.original);
			};

			this.scope.reset = (): void => {
				this.route.reload();
			};
		}

		private begin(): void {
			this.scope.loading = true;
			this.scope.valid = true;

			if (typeof (this.scope.readonly) === "boolean" && !this.scope.readonly) {
				this.flush();
			}
		}

		private end(): void {
			this.scope.loading = false;
		}

		private loadComplete(result: any): void {
			if (result.data) {
				angular.extend(this.scope.model, result.data);

				this.end();

				this.scope.ready = true;
			}
			else {
				this.location.path(this.route.routes[null].redirectTo);
			}
		}

		private postComplete(result: any): void {
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
		}

		private error(result: any): void {
			this.scope.valid = false;

			this.end();
			this.pushAlert(result.message, "danger");
		}

		private pushAlert(msg: string, type: string): void {
			if (!msg) {
				return;
			}

			this.scope.alerts.push({
				msg: msg,
				type: type
			});
		}

		private flush(): void {
			angular.extend(this.original, this.scope.model);
		}
	}
}