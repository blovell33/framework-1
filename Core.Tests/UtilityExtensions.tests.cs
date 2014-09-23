using Xunit;

namespace Core.Tests {
	public class UtilityExtensionsToPascalCaseTests {
		[Fact]
		public void TestCase1() {
			// arrange
			const string s = "Hello world";

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Equal("HelloWorld", result);
		}

		[Fact]
		public void TestCase2() {
			// arrange
			const string s = "hello world";

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Equal("HelloWorld", result);
		}

		[Fact]
		public void TestCase3() {
			// arrange
			const string s = null;

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Null(result);
		}

		[Fact]
		public void TestCase4() {
			// arrange
			const string s = "helloWorld";

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Equal("HelloWorld", result);
		}

		[Fact]
		public void TestCase5() {
			// arrange
			const string s = "HelloWorld";

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Equal("HelloWorld", result);
		}

		[Fact]
		public void TestCase6() {
			// arrange
			const string s = " hello world ";

			// act
			var result = s.ToPascalCase();

			// assert
			Assert.Equal("HelloWorld", result);
		}
	}

	public class UtilityExtensionsToCamelCaseTests {
		[Fact]
		public void TestCase1() {
			// arrange
			const string s = "Hello world";

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Equal("helloWorld", result);
		}

		[Fact]
		public void TestCase2() {
			// arrange
			const string s = "hello world";

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Equal("helloWorld", result);
		}

		[Fact]
		public void TestCase3() {
			// arrange
			const string s = null;

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Null(result);
		}

		[Fact]
		public void TestCase4() {
			// arrange
			const string s = "helloWorld";

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Equal("helloWorld", result);
		}

		[Fact]
		public void TestCase5() {
			// arrange
			const string s = "HelloWorld";

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Equal("helloWorld", result);
		}

		[Fact]
		public void TestCase6() {
			// arrange
			const string s = " hello world ";

			// act
			var result = s.ToCamelCase();

			// assert
			Assert.Equal("helloWorld", result);
		}
	}

	public class UtilityExtensionsToSentenceCaseTests {
		[Fact]
		public void TestCase1() {
			// arrange
			const string s = "Hello world";

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Equal("Hello world", result);
		}

		[Fact]
		public void TestCase2() {
			// arrange
			const string s = "hello world";

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Equal("Hello world", result);
		}

		[Fact]
		public void TestCase3() {
			// arrange
			const string s = null;

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Null(result);
		}

		[Fact]
		public void TestCase4() {
			// arrange
			const string s = "helloWorld";

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Equal("Hello world", result);
		}

		[Fact]
		public void TestCase5() {
			// arrange
			const string s = "HelloWorld";

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Equal("Hello world", result);
		}

		[Fact]
		public void TestCase6() {
			// arrange
			const string s = " hello world ";

			// act
			var result = s.ToSentenceCase();

			// assert
			Assert.Equal("Hello world", result);
		}
	}
}