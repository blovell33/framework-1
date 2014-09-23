module Shared {
	"use strict";

	export interface IProviderService {
		bootstrap(scope: IBaseScope, labels: any): void;

		cache: any;
	}

	export interface IBaseScope extends ng.IScope {
		ready: boolean;

		loading: boolean;

		valid: boolean;

		success: boolean;

		readonly: boolean;

		model: any;

		labels: any;

		alerts: {
			msg: string;
			type: string;
		}[];

		cache: any;

		path: string;

		redirect: string;

		add(key: string, value: any): void;

		closeAlert(index?: number): void;

		load(url: string): void;

		post(url: string, model: any, callback?: Function): void;

		edit(): void;

		view(): void;

		reset(): void;
	}

	export interface IBreadcrumbScope extends IBaseScope {
		breadcrumb: Array<string>;
	}

	export interface IGridScope extends IBaseScope {
		parameters: Parameters;

		icon: string;

		resize(size: number): void;

		filter(): void;

		sort(name: string): void;

		page(): void;
	}

	export interface IAccountScope extends Shared.IBaseScope {
		register(): void;

		activate(): void;

		signIn(): void;

		retrievePassword(): void;

		resetPassword(): void;

		changePassword(): void;

		updatePersonalInformation(): void;
	}

	export interface IAdministrationScope extends IBaseScope {
		createUser(): void;

		updateUser(): void;

		changePassword(): void;

		deleteUser(): void;
	}
}