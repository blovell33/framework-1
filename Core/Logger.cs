using System;
using System.Diagnostics;

namespace Core {
	public static class Logger {
		public static void Debug(string message) {
			System.Diagnostics.Debug.WriteLine(FormatMessage(message), "Debug");
		}

		public static void Debug(object obj) {
			Debug(obj.ToString());
		}

		public static void Debug(string message, params object[] arg) {
			Debug(string.Format(message, arg));
		}

		public static void Info(string message) {
			Trace.WriteLine(FormatMessage(message), "Info");
		}

		public static void Info(object obj) {
			Info(obj.ToString());
		}

		public static void Info(string message, params object[] arg) {
			Info(string.Format(message, arg));
		}

		public static void Warning(string message) {
			Trace.WriteLine(FormatMessage(message), "Warning");
		}

		public static void Warning(object obj) {
			Warning(obj.ToString());
		}

		public static void Warning(string message, params object[] arg) {
			Warning(string.Format(message, arg));
		}

		public static void Error(string message) {
			Trace.WriteLine(FormatMessage(message), "Error");
		}

		public static void Error(object obj) {
			Error(obj.ToString());
		}

		public static void Error(string message, params object[] arg) {
			Error(string.Format(message, arg));
		}

		public static void Fatal(string message) {
			Trace.WriteLine(FormatMessage(message), "Fatal");
		}

		public static void Fatal(object obj) {
			Fatal(obj.ToString());
		}

		public static void Fatal(string message, params object[] arg) {
			Fatal(string.Format(message, arg));
		}

		private static string FormatMessage(string message) {
			return string.Format("[{0:M-d-yyyy h:mm:ss tt}] {1}", DateTime.Now, message);
		}
	}
}