using System;
using System.Web;
using System.Web.Mvc;
using Moq;
using Xunit;

namespace Core.Tests {
	public class SecureConnectionAttributeOnActionExecutingTests {
		[Fact]
		public void TestUsingHttp() {
			// arrange
			var filterContext = new Mock<ActionExecutingContext>();
			var httpContext = new Mock<HttpContextBase>();
			var response = new Mock<HttpResponseBase>(MockBehavior.Strict);

			filterContext
				.SetupGet(x => x.HttpContext)
				.Returns(httpContext.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(response.Object);

			var settings = new ApplicationSettings {
				UseHttps = false
			};

			var attribute = new SecureConnectionAttribute(settings);

			// act
			attribute.OnActionExecuting(filterContext.Object);

			// assert
			response.Verify(x => x.Redirect(It.IsAny<string>()), Times.Never);
		}

		[Fact]
		public void TestUsingHttps() {
			// arrange
			var filterContext = new Mock<ActionExecutingContext>();
			var httpContext = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();

			filterContext
				.SetupGet(x => x.HttpContext)
				.Returns(httpContext.Object);

			httpContext
				.SetupGet(x => x.Request)
				.Returns(request.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(response.Object);

			request
				.SetupGet(x => x.IsSecureConnection)
				.Returns(false);

			request
				.SetupGet(x => x.Url)
				.Returns(new Uri("http://localhost/?foo=bar"));

			var settings = new ApplicationSettings {
				UseHttps = true
			};

			var attribute = new SecureConnectionAttribute(settings);

			// act
			attribute.OnActionExecuting(filterContext.Object);

			// assert
			response.Verify(x => x.Redirect(It.Is<string>(s => s == "https://localhost/?foo=bar")), Times.Once);
		}
	}
}