using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core {
	public class ApplicationSettings {
		public ApplicationSettings() : this(new NameValueCollection()) {}

		public ApplicationSettings(NameValueCollection collection) {
			var keys = collection.AllKeys;

			if (keys.Contains(Constants.ApplicationKeys.NAME_KEY)) {
				Name = collection[Constants.ApplicationKeys.NAME_KEY];
			}

			if (keys.Contains(Constants.ApplicationKeys.CAN_REGISTER_KEY)) {
				bool canRegister;

				if (!bool.TryParse(collection[Constants.ApplicationKeys.CAN_REGISTER_KEY], out canRegister)) {
					throw new ApplicationSettingsException(Constants.ApplicationKeys.CAN_REGISTER_KEY);
				}

				CanRegister = canRegister;
			}

			if (keys.Contains(Constants.ApplicationKeys.USE_HTTPS_KEY)) {
				bool useHttps;

				if (!bool.TryParse(collection[Constants.ApplicationKeys.USE_HTTPS_KEY], out useHttps)) {
					throw new ApplicationSettingsException(Constants.ApplicationKeys.USE_HTTPS_KEY);
				}

				UseHttps = useHttps;
			}
			else {
				UseHttps = true;
			}

			if (keys.Contains(Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY)) {
				int minPasswordLength;

				if (!int.TryParse(collection[Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY], out minPasswordLength)) {
					throw new ApplicationSettingsException(Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY);
				}

				MinPasswordLength = minPasswordLength;
			}
			else {
				MinPasswordLength = 6;
			}

			if (keys.Contains(Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY)) {
				PasswordScore minPasswordScore;

				if (!Enum.TryParse(collection[Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY], out minPasswordScore)) {
					throw new ApplicationSettingsException(Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY);
				}

				MinPasswordScore = minPasswordScore;
			}
			else {
				MinPasswordScore = PasswordScore.Strong;
			}

			if (keys.Contains(Constants.ApplicationKeys.ENVIRONMENT_KEY)) {
				ApplicationEnvironment environment;

				if (!Enum.TryParse(collection[Constants.ApplicationKeys.ENVIRONMENT_KEY], out environment)) {
					throw new ApplicationSettingsException(Constants.ApplicationKeys.ENVIRONMENT_KEY);
				}

				Environment = environment;
			}
			else {
				Environment = ApplicationEnvironment.Development;
			}
		}

		public virtual string Name { get; set; }

		public virtual bool CanRegister { get; set; }

		public virtual bool UseHttps { get; set; }

		public virtual int MinPasswordLength { get; set; }

		public virtual PasswordScore MinPasswordScore { get; set; }

		public virtual ApplicationEnvironment Environment { get; set; }

		public virtual PasswordScore CheckStrength(string password) {
			if (password == null) {
				return PasswordScore.Blank;
			}

			if (password.Length == 0) {
				return PasswordScore.Blank;
			}

			var score = PasswordScore.VeryWeak;

			if (password.Length < MinPasswordLength) {
				return score;
			}

			if (Regex.IsMatch(password, @"[a-z]")) {
				score++;
			}

			if (Regex.IsMatch(password, @"[A-Z]")) {
				score++;
			}

			if (Regex.IsMatch(password, @"[0-9]")) {
				score++;
			}

			if (Regex.IsMatch(password, @"[^0-9A-Za-z]")) {
				score++;
			}

			return score;
		}
	}
}