using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Core.Tests {
	public class LoggerDebugTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Debug.Listeners.Add(listener);

			// act
			Logger.Debug("hello world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Debug: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithObject() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);
			object obj = "hello world";

			Debug.Listeners.Add(listener);

			// act
			Logger.Debug(obj);

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Debug: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithFormat() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Debug.Listeners.Add(listener);

			// act
			Logger.Debug("hello {0}", "world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Debug: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}
	}

	public class LoggerInfoTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Info("hello world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Info: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithObject() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);
			object obj = "hello world";

			Trace.Listeners.Add(listener);

			// act
			Logger.Info(obj);

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Info: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithFormat() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Info("hello {0}", "world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Info: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}
	}

	public class LoggerWarningTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Warning("hello world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Warning: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithObject() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);
			object obj = "hello world";

			Trace.Listeners.Add(listener);

			// act
			Logger.Warning(obj);

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Warning: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithFormat() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Warning("hello {0}", "world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Warning: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}
	}

	public class LoggerErrorTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Error("hello world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Error: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithObject() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);
			object obj = "hello world";

			Trace.Listeners.Add(listener);

			// act
			Logger.Error(obj);

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Error: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithFormat() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Error("hello {0}", "world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Error: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}
	}

	public class LoggerFatalTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Fatal("hello world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Fatal: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithObject() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);
			object obj = "hello world";

			Trace.Listeners.Add(listener);

			// act
			Logger.Fatal(obj);

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Fatal: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}

		[Fact]
		public void TestWithFormat() {
			// arrange
			var stream = new MemoryStream();
			var listener = new TextWriterTraceListener(stream);

			Trace.Listeners.Add(listener);

			// act
			Logger.Fatal("hello {0}", "world");

			// assert
			listener.Flush();
			stream.Position = 0;

			using (var reader = new StreamReader(stream)) {
				Assert.True(Regex.IsMatch(reader.ReadToEnd(), @"^Fatal: \[\d{1,2}-\d{1,2}-\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM)\] hello world\r\n$"));
			}
		}
	}
}