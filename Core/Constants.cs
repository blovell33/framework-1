namespace Core {
	public static class Constants {
		public static class Roles {
			public const string ADMINISTRATOR_ROLE = "administrator";
			public const string SYSTEM_MANAGER_ROLE = "system-manager";
		}

		public static class ApplicationKeys {
			public const string NAME_KEY = "application:name";
			public const string CAN_REGISTER_KEY = "application:can-register";
			public const string USE_HTTPS_KEY = "application:use-https";
			public const string MIN_PASSWORD_LENGTH_KEY = "application:min-password-length";
			public const string MIN_PASSWORD_SCORE_KEY = "application:min-password-score";
			public const string ENVIRONMENT_KEY = "application:environment";
		}

		public static class ValidationMessages {
			public const string USERNAME_IS_UNAVAILABLE = "The username is not available.";
			public const string EMAIL_IS_UNAVAILABLE = "The email address is not available.";
			public const string PASSWORD_IS_INCORRECT = "The password is incorrect.";
			public const string FIELD_IS_REQUIRED = "The field is required.";
			public const string USERNAME_IS_INCORRECT_FORMAT = "The username is invalid.";
			public const string EMAIL_IS_INCORRECT_FORMAT = "The email address is invalid.";
			public const string PASSWORD_IS_TOO_WEAK = "The password is invalid.";
			public const string COMPARISON_DOES_NOT_MATCH = "The comparison does not match.";
			public const string INVALID_VALUE = "The value is invalid.";
			public const string INVALID_OPERATION = "The operation is invalid.";
		}

		public static class ServiceMessages {
			public const string INVALID_CREDENTIALS = "Oops! The email address or password is invalid. Please try again.";
			public const string INVALID_USER_REQUEST = "Oops! The user request is invalid. Please contact the site administrator for assistance.";
			public const string ACTIVATION_ERROR = "Oops! We were unable to activate your account. Please contact the site administrator for assistance.";
			public const string RESET_PASSWORD_ERROR = "Oops! We were unable to reset your password. Please contact the site administrator for assistance.";
			public const string INVALID_OPERATION = "The operation is invalid.";
		}

		// The database is sized to these constants.
		// Do not change them.

		public static class Cryptography {
			public const int SALT_BYTE_SIZE = 24;
			public const int HASH_ITERATIONS = 1000;
			public const int HASH_BYTE_SIZE = 24;
		}
	}

	public enum ApplicationEnvironment {
		Development,
		Production,
		Test
	}

	public enum PasswordScore {
		Blank = 0,
		VeryWeak = 1,
		Weak = 2,
		Medium = 3,
		Strong = 4,
		VeryStrong = 5
	}

	public enum ValidationReason {
		FieldIsRequired,
		UsernameIsIncorrectFormat,
		UsernameIsUnavailable,
		EmailIsIncorrectFormat,
		EmailIsUnavailable,
		PasswordIsIncorrect,
		PasswordIsTooWeak,
		ComparisonDoesNotMatch,
		InvalidValue,
		InvalidOperation
	}

	public enum ServiceReason {
		InvalidCredentials,
		InvalidUserRequest,
		ActivationError,
		ResetPasswordError,
		InvalidOperation
	}

	public enum UserRequestType {
		Activation,
		ResetPassword
	}

	public enum CellAlignment {
		Left = 0,
		Center = 1,
		Right = 2
	}
}