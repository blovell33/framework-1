module Shared {
	"use strict";

	export enum ValidationType {
		ExistingUserByUsername,
		NewUserByUsername,
		ExistingUserByEmail,
		NewUserByEmail,
		CurrentUserByEmail,
		OldPassword,
		IsRequired,
		Username,
		Email,
		Password,
		Compare,
		GreaterThan,
		GreaterThanEqualTo,
		LessThan,
		LessThanEqualTo
	}
}