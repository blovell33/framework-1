using System;

namespace Core {
	public class ApplicationSettingsException : Exception {
		public ApplicationSettingsException(string key) : base("The value is missing or configured incorrectly.") {
			Key = key;
		}

		public string Key { get; private set; }
	}
}