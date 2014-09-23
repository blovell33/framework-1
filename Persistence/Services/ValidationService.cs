using System;
using System.Threading.Tasks;
using Core;
using Persistence.Interfaces;
using Persistence.Queries;

namespace Persistence.Services {
	public class ValidationService {
		private readonly IRepository _repository;
		private readonly ApplicationSettings _settings;

		public ValidationService(IRepository repository, ApplicationSettings settings) {
			_repository = repository;
			_settings = settings;

			Bundles = new ValidationBundles(this);
		}

		public virtual ValidationBundles Bundles { get; private set; }

		public virtual async Task ValidateExistingUserByUsernameAsync(string username) {
			ValidateUsername(username);

			var user = await _repository.UserByUsernameAsync(username);
			var valid = user != null;

			if (!valid) {
				throw new ValidationException(ValidationReason.UsernameIsUnavailable);
			}
		}

		public virtual async Task ValidateNewUserByUsernameAsync(string username) {
			ValidateUsername(username);

			var user = await _repository.UserByUsernameAsync(username);
			var valid = user == null;

			if (!valid) {
				throw new ValidationException(ValidationReason.UsernameIsUnavailable);
			}
		}

		public virtual async Task ValidateExistingUserByEmailAsync(string email) {
			ValidateEmail(email);

			var user = await _repository.UserByEmailAsync(email);
			var valid = user != null;

			if (!valid) {
				throw new ValidationException(ValidationReason.EmailIsUnavailable);
			}
		}

		public virtual async Task ValidateNewUserByEmailAsync(string email) {
			ValidateEmail(email);

			var user = await _repository.UserByEmailAsync(email);
			var valid = user == null;

			if (!valid) {
				throw new ValidationException(ValidationReason.EmailIsUnavailable);
			}
		}

		public virtual async Task ValidateCurrentUserByEmailAsync(string username, string email) {
			ValidateEmail(email);

			var user = await _repository.UserByEmailAsync(email);
			var valid = user == null || user.Username.Equals(username, StringComparison.OrdinalIgnoreCase);

			if (!valid) {
				throw new ValidationException(ValidationReason.EmailIsUnavailable);
			}
		}

		public virtual async Task ValidateOldPasswordAsync(string username, string oldPassword) {
			var user = await _repository.UserByUsernameAsync(username);
			var valid = user != null && CryptoUtilities.ValidatePassword(oldPassword, user.Password);

			if (!valid) {
				throw new ValidationException(ValidationReason.PasswordIsIncorrect);
			}
		}

		public virtual void ValidateIsRequired(string value) {
			var valid = !string.IsNullOrWhiteSpace(value);

			if (!valid) {
				throw new ValidationException(ValidationReason.FieldIsRequired);
			}
		}

		public virtual void ValidateUsername(string username) {
			var valid = RegexUtilities.ValidateUrlPart(username);

			if (!valid) {
				throw new ValidationException(ValidationReason.UsernameIsIncorrectFormat);
			}
		}

		public virtual void ValidateEmail(string email) {
			var valid = RegexUtilities.ValidateEmail(email);

			if (!valid) {
				throw new ValidationException(ValidationReason.EmailIsIncorrectFormat);
			}
		}

		public virtual string ValidatePassword(string password) {
			var passwordScore = _settings.CheckStrength(password);
			var valid = passwordScore != PasswordScore.Blank && passwordScore >= _settings.MinPasswordScore;
			var description = passwordScore.ToString().ToSentenceCase().ToLower();
			var message = string.Format("Password strength: {0}", description);

			if (valid) {
				return message;
			}

			throw new ValidationException(message, ValidationReason.PasswordIsTooWeak);
		}

		public virtual void ValidateComparison(string compare, string compareTo) {
			var valid = compare == compareTo;

			if (!valid) {
				throw new ValidationException(ValidationReason.ComparisonDoesNotMatch);
			}
		}

		public virtual void ValidateGreaterThan(string compare, string compareTo) {
			double result;

			if (!double.TryParse(compareTo, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			ValidateGreaterThan(compare, result);
		}

		public virtual void ValidateGreaterThan(string compare, double compareTo) {
			double result;

			if (!double.TryParse(compare, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			var valid = result > compareTo;

			if (!valid) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}
		}

		public virtual void ValidateGreaterThanEqualTo(string compare, string compareTo) {
			double result;

			if (!double.TryParse(compareTo, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			ValidateGreaterThanEqualTo(compare, result);
		}

		public virtual void ValidateGreaterThanEqualTo(string compare, double compareTo) {
			double result;

			if (!double.TryParse(compare, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			var valid = result >= compareTo;

			if (!valid) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}
		}

		public virtual void ValidateLessThan(string compare, string compareTo) {
			double result;

			if (!double.TryParse(compareTo, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			ValidateLessThan(compare, result);
		}

		public virtual void ValidateLessThan(string compare, double compareTo) {
			double result;

			if (!double.TryParse(compare, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			var valid = result < compareTo;

			if (!valid) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}
		}

		public virtual void ValidateLessThanEqualTo(string compare, string compareTo) {
			double result;

			if (!double.TryParse(compareTo, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			ValidateLessThanEqualTo(compare, result);
		}

		public virtual void ValidateLessThanEqualTo(string compare, double compareTo) {
			double result;

			if (!double.TryParse(compare, out result)) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}

			var valid = result <= compareTo;

			if (!valid) {
				throw new ValidationException(ValidationReason.InvalidValue);
			}
		}
	}
}