using System;

namespace Core {
	public class CaptchaException : Exception {
		public CaptchaException() : base("Suspected spambot detected.") {}
	}
}