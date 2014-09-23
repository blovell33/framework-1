using System.Collections.Specialized;
using Xunit;

namespace Core.Tests {
	public class ApplicationSettingsConstructorTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange/act
			var settings = new ApplicationSettings();

			// assert
			Assert.Null(settings.Name);
			Assert.False(settings.CanRegister);
			Assert.True(settings.UseHttps);
			Assert.Equal(6, settings.MinPasswordLength);
			Assert.Equal(PasswordScore.Strong, settings.MinPasswordScore);
			Assert.Equal(ApplicationEnvironment.Development, settings.Environment);
		}

		[Fact]
		public void TestWithCollection() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.NAME_KEY, "foo"},
				{Constants.ApplicationKeys.CAN_REGISTER_KEY, "true"},
				{Constants.ApplicationKeys.USE_HTTPS_KEY, "false"},
				{Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY, "8"},
				{Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY, "Medium"},
				{Constants.ApplicationKeys.ENVIRONMENT_KEY, "Production"}
			};

			// act
			var settings = new ApplicationSettings(collection);

			// assert
			Assert.Equal(collection[Constants.ApplicationKeys.NAME_KEY], settings.Name);
			Assert.True(settings.CanRegister);
			Assert.False(settings.UseHttps);
			Assert.Equal(8, settings.MinPasswordLength);
			Assert.Equal(PasswordScore.Medium, settings.MinPasswordScore);
			Assert.Equal(ApplicationEnvironment.Production, settings.Environment);
		}

		[Fact]
		public void TestInvalidCanRegister() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.CAN_REGISTER_KEY, "invalid value"}
			};

			// act
			var exception = Assert.Throws<ApplicationSettingsException>(() => new ApplicationSettings(collection));

			// assert
			Assert.Equal(Constants.ApplicationKeys.CAN_REGISTER_KEY, exception.Key);
		}

		[Fact]
		public void TestInvalidUseHttps() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.USE_HTTPS_KEY, "invalid value"}
			};

			// act
			var exception = Assert.Throws<ApplicationSettingsException>(() => new ApplicationSettings(collection));

			// assert
			Assert.Equal(Constants.ApplicationKeys.USE_HTTPS_KEY, exception.Key);
		}

		[Fact]
		public void TestInvalidMinPasswordLength() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY, "invalid value"}
			};

			// act
			var exception = Assert.Throws<ApplicationSettingsException>(() => new ApplicationSettings(collection));

			// assert
			Assert.Equal(Constants.ApplicationKeys.MIN_PASSWORD_LENGTH_KEY, exception.Key);
		}

		[Fact]
		public void TestInvalidMinPasswordScore() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY, "invalid value"}
			};

			// act
			var exception = Assert.Throws<ApplicationSettingsException>(() => new ApplicationSettings(collection));

			// assert
			Assert.Equal(Constants.ApplicationKeys.MIN_PASSWORD_SCORE_KEY, exception.Key);
		}

		[Fact]
		public void TestInvalidEnvironment() {
			// arrange
			var collection = new NameValueCollection {
				{Constants.ApplicationKeys.ENVIRONMENT_KEY, "invalid value"}
			};

			// act
			var exception = Assert.Throws<ApplicationSettingsException>(() => new ApplicationSettings(collection));

			// assert
			Assert.Equal(Constants.ApplicationKeys.ENVIRONMENT_KEY, exception.Key);
		}
	}

	public class ApplicationSettingsCheckStrengthTests {
		[Fact]
		public void TestPasswordIsNull() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength(null);

			// assert
			Assert.Equal(PasswordScore.Blank, score);
		}

		[Fact]
		public void TestPasswordLengthIsZero() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength(string.Empty);

			// assert
			Assert.Equal(PasswordScore.Blank, score);
		}

		[Fact]
		public void TestPasswordLengthIsLessThanMinPasswordLength() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength("foo");

			// assert
			Assert.Equal(PasswordScore.VeryWeak, score);
		}

		[Fact]
		public void TestPasswordHasLowercaseLetters() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength("foobar");

			// assert
			Assert.Equal(PasswordScore.Weak, score);
		}

		[Fact]
		public void TestPasswordHasUppercaseLetters() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength("fooBAR");

			// assert
			Assert.Equal(PasswordScore.Medium, score);
		}

		[Fact]
		public void TestPasswordHasNumbers() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength("f00BAR");

			// assert
			Assert.Equal(PasswordScore.Strong, score);
		}

		[Fact]
		public void TestPasswordHasSpecialCharacters() {
			// arrange
			var settings = new ApplicationSettings();

			// act
			var score = settings.CheckStrength("f00B@R");

			// assert
			Assert.Equal(PasswordScore.VeryStrong, score);
		}
	}
}