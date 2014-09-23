namespace Persistence.Services {
	public class ValidationBundles {
		private readonly ValidationService _validationService;

		public ValidationBundles(ValidationService validationService) {
			_validationService = validationService;
		}

		public virtual void ValidateNewPassword(dynamic model) {
			_validationService.ValidatePassword((string) model.password);
			_validationService.ValidateComparison((string) model.password, (string) model.confirmPassword);
		}

		public virtual void ValidateUser(dynamic model) {
			_validationService.ValidateIsRequired((string) model.firstName);
			_validationService.ValidateIsRequired((string) model.lastName);
		}
	}
}