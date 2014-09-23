using System;

namespace Core {
	public class ServiceException : Exception {
		public ServiceException(ServiceReason reason) : base(GetMessage(reason)) {
			Reason = reason;
		}

		public ServiceReason Reason { get; private set; }

		private static string GetMessage(ServiceReason reason) {
			switch (reason) {
				case ServiceReason.InvalidCredentials:
					return Constants.ServiceMessages.INVALID_CREDENTIALS;

				case ServiceReason.InvalidUserRequest:
					return Constants.ServiceMessages.INVALID_USER_REQUEST;

				case ServiceReason.ActivationError:
					return Constants.ServiceMessages.ACTIVATION_ERROR;

				case ServiceReason.ResetPasswordError:
					return Constants.ServiceMessages.RESET_PASSWORD_ERROR;

				default:
					return Constants.ServiceMessages.INVALID_OPERATION;
			}
		}
	}
}