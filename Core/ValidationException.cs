using System;

namespace Core {
	public class ValidationException : Exception {
		public ValidationException(string message, ValidationReason reason) : base(message) {
			Reason = reason;
		}

		public ValidationException(ValidationReason reason) : base(GetMessage(reason)) {
			Reason = reason;
		}

		public ValidationReason Reason { get; private set; }

		private static string GetMessage(ValidationReason reason) {
			switch (reason) {
				case ValidationReason.UsernameIsUnavailable:
					return Constants.ValidationMessages.USERNAME_IS_UNAVAILABLE;

				case ValidationReason.EmailIsUnavailable:
					return Constants.ValidationMessages.EMAIL_IS_UNAVAILABLE;

				case ValidationReason.PasswordIsIncorrect:
					return Constants.ValidationMessages.PASSWORD_IS_INCORRECT;

				case ValidationReason.FieldIsRequired:
					return Constants.ValidationMessages.FIELD_IS_REQUIRED;

				case ValidationReason.UsernameIsIncorrectFormat:
					return Constants.ValidationMessages.USERNAME_IS_INCORRECT_FORMAT;

				case ValidationReason.EmailIsIncorrectFormat:
					return Constants.ValidationMessages.EMAIL_IS_INCORRECT_FORMAT;

				case ValidationReason.PasswordIsTooWeak:
					return Constants.ValidationMessages.PASSWORD_IS_TOO_WEAK;

				case ValidationReason.ComparisonDoesNotMatch:
					return Constants.ValidationMessages.COMPARISON_DOES_NOT_MATCH;

				case ValidationReason.InvalidValue:
					return Constants.ValidationMessages.INVALID_VALUE;

				default:
					return Constants.ValidationMessages.INVALID_OPERATION;
			}
		}
	}
}