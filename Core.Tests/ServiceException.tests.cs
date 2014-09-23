using Xunit;

namespace Core.Tests {
	public class ServiceExceptionConstructorTests {
		[Fact]
		public void TestInvalidCredentials() {
			// arrange/act
			var exception = new ServiceException(ServiceReason.InvalidCredentials);

			// assert
			Assert.Equal(Constants.ServiceMessages.INVALID_CREDENTIALS, exception.Message);
			Assert.Equal(ServiceReason.InvalidCredentials, exception.Reason);
		}

		[Fact]
		public void TestInvalidUserRequest() {
			// arrange/act
			var exception = new ServiceException(ServiceReason.InvalidUserRequest);

			// assert
			Assert.Equal(Constants.ServiceMessages.INVALID_USER_REQUEST, exception.Message);
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);
		}

		[Fact]
		public void TestActivationError() {
			// arrange/act
			var exception = new ServiceException(ServiceReason.ActivationError);

			// assert
			Assert.Equal(Constants.ServiceMessages.ACTIVATION_ERROR, exception.Message);
			Assert.Equal(ServiceReason.ActivationError, exception.Reason);
		}

		[Fact]
		public void TestResetPasswordError() {
			// arrange/act
			var exception = new ServiceException(ServiceReason.ResetPasswordError);

			// assert
			Assert.Equal(Constants.ServiceMessages.RESET_PASSWORD_ERROR, exception.Message);
			Assert.Equal(ServiceReason.ResetPasswordError, exception.Reason);
		}

		[Fact]
		public void TestInvalidOperation() {
			// arrange/act
			var exception = new ServiceException(ServiceReason.InvalidOperation);

			// assert
			Assert.Equal(Constants.ServiceMessages.INVALID_OPERATION, exception.Message);
			Assert.Equal(ServiceReason.InvalidOperation, exception.Reason);
		}
	}
}