using System.Web.Mvc;
using Moq;
using Xunit;

namespace Core.Tests {
	public class MailSettingsAttributeOnResultExecutingTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var filterContext = new Mock<ResultExecutingContext>();
			var controller = new Mock<ControllerBase>();

			filterContext
				.SetupGet(x => x.Controller)
				.Returns(controller.Object);

			var settings = new MailSettings();
			var attribute = new MailSettingsAttribute(settings);

			// act
			attribute.OnResultExecuting(filterContext.Object);

			// assert
			Assert.NotNull(filterContext.Object.Controller.ViewBag.Mail.Settings);
		}
	}
}