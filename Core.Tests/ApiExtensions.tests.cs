using System.Net;
using System.Net.Http;
using System.Web.Http;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Tests {
	public class ApiExtensionsOKTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.OK();
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Null((JObject) result.data);
		}

		[Fact]
		public void TestWithData() {
			// arrange
			var obj = new {
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.OK(obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}

		[Fact]
		public void TestWithMessageAndData() {
			// arrange
			var obj = new {
				message = "hello world",
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.OK(obj.message, obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
			Assert.Equal(obj.message, (string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}
	}

	public class ApiExtensionsNotFoundTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.NotFound();
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Null((JObject) result.data);
		}

		[Fact]
		public void TestWithData() {
			// arrange
			var obj = new {
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.NotFound(obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}

		[Fact]
		public void TestWithMessageAndData() {
			// arrange
			var obj = new {
				message = "hello world",
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.NotFound(obj.message, obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
			Assert.Equal(obj.message, (string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}
	}

	public class ApiExtensionsUnauthorizedTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.Unauthorized();
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Null((JObject) result.data);
		}

		[Fact]
		public void TestWithData() {
			// arrange
			var obj = new {
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.Unauthorized(obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}

		[Fact]
		public void TestWithMessageAndData() {
			// arrange
			var obj = new {
				message = "hello world",
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.Unauthorized(obj.message, obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
			Assert.Equal(obj.message, (string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}
	}

	public class ApiExtensionsBadRequestTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.BadRequest();
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Null((JObject) result.data);
		}

		[Fact]
		public void TestWithData() {
			// arrange
			var obj = new {
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.BadRequest(obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
			Assert.Null((string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}

		[Fact]
		public void TestWithMessageAndData() {
			// arrange
			var obj = new {
				message = "hello world",
				data = new {
					foo = "foo",
					bar = 1234
				}
			};

			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.BadRequest(obj.message, obj.data);
			dynamic result = responseMessage.Content.ReadAsAsync<JObject>().Result;

			// assert
			Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
			Assert.Equal(obj.message, (string) result.message);
			Assert.Equal(obj.data.foo, (string) result.data.foo);
			Assert.Equal(obj.data.bar, (int) result.data.bar);
		}
	}

	public class ApiExtensionsNoContentTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var requestMessage = Mock.Of<HttpRequestMessage>();
			requestMessage.SetConfiguration(new HttpConfiguration());

			// act
			var responseMessage = requestMessage.NoContent();

			// assert
			Assert.Equal(HttpStatusCode.NoContent, responseMessage.StatusCode);
		}
	}
}