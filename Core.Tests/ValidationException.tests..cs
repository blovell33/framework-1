using Xunit;

namespace Core.Tests {
	public class ValidationExceptionConstructorTests {
		[Fact]
		public void TestWithMessage() {
			// arrange
			var obj = new {
				message = "Kaboom!!!"
			};

			//act
			var exception = new ValidationException(obj.message, ValidationReason.FieldIsRequired);

			// assert
			Assert.Equal(obj.message, exception.Message);
		}

		[Fact]
		public void TestUsernameIsUnavailable() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.UsernameIsUnavailable);

			// assert
			Assert.Equal(Constants.ValidationMessages.USERNAME_IS_UNAVAILABLE, exception.Message);
			Assert.Equal(ValidationReason.UsernameIsUnavailable, exception.Reason);
		}

		[Fact]
		public void TestEmailIsUnavailable() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.EmailIsUnavailable);

			// assert
			Assert.Equal(Constants.ValidationMessages.EMAIL_IS_UNAVAILABLE, exception.Message);
			Assert.Equal(ValidationReason.EmailIsUnavailable, exception.Reason);
		}

		[Fact]
		public void TestPasswordIsIncorrect() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.PasswordIsIncorrect);

			// assert
			Assert.Equal(Constants.ValidationMessages.PASSWORD_IS_INCORRECT, exception.Message);
			Assert.Equal(ValidationReason.PasswordIsIncorrect, exception.Reason);
		}

		[Fact]
		public void TestFieldIsRequired() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.FieldIsRequired);

			// assert
			Assert.Equal(Constants.ValidationMessages.FIELD_IS_REQUIRED, exception.Message);
			Assert.Equal(ValidationReason.FieldIsRequired, exception.Reason);
		}

		[Fact]
		public void TestUsernameIsIncorrectFormat() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.UsernameIsIncorrectFormat);

			// assert
			Assert.Equal(Constants.ValidationMessages.USERNAME_IS_INCORRECT_FORMAT, exception.Message);
			Assert.Equal(ValidationReason.UsernameIsIncorrectFormat, exception.Reason);
		}

		[Fact]
		public void TestEmailIsIncorrectFormat() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.EmailIsIncorrectFormat);

			// assert
			Assert.Equal(Constants.ValidationMessages.EMAIL_IS_INCORRECT_FORMAT, exception.Message);
			Assert.Equal(ValidationReason.EmailIsIncorrectFormat, exception.Reason);
		}

		[Fact]
		public void TestPasswordIsTooWeak() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.PasswordIsTooWeak);

			// assert
			Assert.Equal(Constants.ValidationMessages.PASSWORD_IS_TOO_WEAK, exception.Message);
			Assert.Equal(ValidationReason.PasswordIsTooWeak, exception.Reason);
		}

		[Fact]
		public void TestComparisonDoesNotMatch() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.ComparisonDoesNotMatch);

			// assert
			Assert.Equal(Constants.ValidationMessages.COMPARISON_DOES_NOT_MATCH, exception.Message);
			Assert.Equal(ValidationReason.ComparisonDoesNotMatch, exception.Reason);
		}

		[Fact]
		public void TestInvalidValue() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.InvalidValue);

			// assert
			Assert.Equal(Constants.ValidationMessages.INVALID_VALUE, exception.Message);
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidOperation() {
			// arrange/act
			var exception = new ValidationException(ValidationReason.InvalidOperation);

			// assert
			Assert.Equal(Constants.ValidationMessages.INVALID_OPERATION, exception.Message);
			Assert.Equal(ValidationReason.InvalidOperation, exception.Reason);
		}
	}
}