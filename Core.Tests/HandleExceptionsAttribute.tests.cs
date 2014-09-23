using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Tests {
	public class HandleExceptionsAttributeOnExceptionTests {
		[Fact]
		public void TestWithCaptchaException() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			var controllerContext = Mock.Of<HttpControllerContext>();
			var actionContext = Mock.Of<HttpActionContext>();

			requestMessage.SetConfiguration(new HttpConfiguration());
			controllerContext.Request = requestMessage;
			actionContext.ControllerContext = controllerContext;

			var actionExecutedContext = new HttpActionExecutedContext(actionContext, null) {
				Exception = new CaptchaException()
			};

			var attribute = new HandleExceptionsAttribute();

			// act
			attribute.OnException(actionExecutedContext);

			// assert
			Assert.Equal(HttpStatusCode.NotFound, actionExecutedContext.Response.StatusCode);
		}

		[Fact]
		public void TestWithValidationException() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			var controllerContext = Mock.Of<HttpControllerContext>();
			var actionContext = Mock.Of<HttpActionContext>();

			requestMessage.SetConfiguration(new HttpConfiguration());
			controllerContext.Request = requestMessage;
			actionContext.ControllerContext = controllerContext;

			var actionExecutedContext = new HttpActionExecutedContext(actionContext, null) {
				Exception = new ValidationException(ValidationReason.FieldIsRequired)
			};

			var attribute = new HandleExceptionsAttribute {
				WithHint = true
			};

			// act
			attribute.OnException(actionExecutedContext);

			// assert
			dynamic result = actionExecutedContext.Response.Content.ReadAsAsync<JObject>().Result;

			Assert.Equal(HttpStatusCode.BadRequest, actionExecutedContext.Response.StatusCode);
			Assert.NotNull((string) result.message);
			Assert.Equal((string) result.message, (string) result.data.hint);
		}

		[Fact]
		public void TestWithAnyException() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			var controllerContext = Mock.Of<HttpControllerContext>();
			var actionContext = Mock.Of<HttpActionContext>();

			requestMessage.SetConfiguration(new HttpConfiguration());
			controllerContext.Request = requestMessage;
			actionContext.ControllerContext = controllerContext;

			var actionExecutedContext = new HttpActionExecutedContext(actionContext, null) {
				Exception = new Exception("Kaboom!!!")
			};

			var attribute = new HandleExceptionsAttribute();

			// act
			attribute.OnException(actionExecutedContext);

			// assert
			dynamic result = actionExecutedContext.Response.Content.ReadAsAsync<JObject>().Result;

			Assert.Equal(HttpStatusCode.BadRequest, actionExecutedContext.Response.StatusCode);
			Assert.NotNull((string) result.message);
			Assert.Null((JObject) result.data);
		}
	}
}