module Shared {
	"use strict";

	export class Validation {
		private type: ValidationType;

		constructor(private scope: IBaseScope, private controller: ng.INgModelController, private http: ng.IHttpService, private options: any, private callback: Function) {
			this.type = ValidationType[<string> this.options.type];
		}

		public bootstrap(): void {
			this.scope.$watch("readonly", (): void => {
				this.validate();
			});

			this.scope.$watch((): any => this.controller.$modelValue, (): void => {
				this.validate();
			});

			if (this.type === ValidationType.Compare) {
				this.scope.$watch((): any => this.scope.model[this.options.compareTo], (): void => {
					this.validate();
				});
			}
		}

		public validate(): void {
			if (this.scope.readonly || typeof (this.controller.$modelValue) === "undefined") {
				this.callback();

				return;
			}

			var model: any = {};

			switch (this.type) {
				case ValidationType.ExistingUserByUsername:
				case ValidationType.NewUserByUsername:
					model.username = this.controller.$modelValue;

					break;
				case ValidationType.ExistingUserByEmail:
				case ValidationType.NewUserByEmail:
					model.email = this.controller.$modelValue;

					break;
				case ValidationType.CurrentUserByEmail:
					model.username = this.scope.model.username;
					model.email = this.controller.$modelValue;

					break;
				case ValidationType.OldPassword:
					model.oldPassword = this.controller.$modelValue;

					break;
				case ValidationType.IsRequired:
					model.value = this.controller.$modelValue;

					break;
				case ValidationType.Username:
					model.username = this.controller.$modelValue;

					break;
				case ValidationType.Email:
					model.email = this.controller.$modelValue;

					break;
				case ValidationType.Password:
					model.password = this.controller.$modelValue;

					break;
				case ValidationType.Compare:
					model.compare = this.controller.$modelValue;
					model.compareTo = this.scope.model[this.options.compareTo];

					break;
				case ValidationType.GreaterThan:
				case ValidationType.GreaterThanEqualTo:
				case ValidationType.LessThan:
				case ValidationType.LessThanEqualTo:
					model.compare = this.controller.$modelValue;

					if (this.options.compareTo) {
						model.compareTo = this.scope.model[this.options.compareTo];
					}
					else {
						model.compareTo = this.options.value;
					}

					break;
			}

			var url: string = "/api/validation/" + ValidationType[this.type].toLowerCase();

			this.http.post(url, model)
				.success((result: any): void => {
					var hint: string =
						result.data
							? result.data.hint
							: null;

					this.callback(true, hint);
				})
				.error(result => {
					var hint: string =
						result.data
							? result.data.hint
							: null;

					this.callback(false, hint);
				});
		}
	}
}