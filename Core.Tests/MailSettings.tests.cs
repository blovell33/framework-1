using System.Net.Configuration;
using Xunit;

namespace Core.Tests {
	public class MailSettingsConstructorTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange/act
			var settings = new MailSettings();

			// assert
			Assert.Null(settings.From);
			Assert.Null(settings.Host);
			Assert.Equal(25, settings.Port);
			Assert.Null(settings.Username);
			Assert.Null(settings.Password);
		}

		[Fact]
		public void TestWithSection() {
			// arrange
			var smtp = new SmtpSection {
				From = "noreply@foo.com"
			};

			smtp.Network.Host = "smtp.foo.com";
			smtp.Network.Port = 25;
			smtp.Network.UserName = "foo";
			smtp.Network.Password = "bar";

			// act
			var settings = new MailSettings(smtp);

			// assert
			Assert.Equal(smtp.From, settings.From);
			Assert.Equal(smtp.Network.Host, settings.Host);
			Assert.Equal(smtp.Network.Port, settings.Port);
			Assert.Equal(smtp.Network.UserName, settings.Username);
			Assert.Equal(smtp.Network.Password, settings.Password);
		}
	}
}