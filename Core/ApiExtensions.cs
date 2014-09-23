using System.Net;
using System.Net.Http;

namespace Core {
	public static class ApiExtensions {
		public static HttpResponseMessage OK(this HttpRequestMessage requestMessage) {
			return requestMessage.OK(null);
		}

		public static HttpResponseMessage OK(this HttpRequestMessage requestMessage, object data) {
			return requestMessage.OK(null, data);
		}

		public static HttpResponseMessage OK(this HttpRequestMessage requestMessage, string message, object data = null) {
			return requestMessage.CreateResponse(HttpStatusCode.OK, new {
				message,
				data
			});
		}

		public static HttpResponseMessage NotFound(this HttpRequestMessage requestMessage) {
			return requestMessage.NotFound(null);
		}

		public static HttpResponseMessage NotFound(this HttpRequestMessage requestMessage, object data) {
			return requestMessage.NotFound(null, data);
		}

		public static HttpResponseMessage NotFound(this HttpRequestMessage requestMessage, string message, object data = null) {
			return requestMessage.CreateResponse(HttpStatusCode.NotFound, new {
				message,
				data
			});
		}

		public static HttpResponseMessage Unauthorized(this HttpRequestMessage requestMessage) {
			return requestMessage.Unauthorized(null);
		}

		public static HttpResponseMessage Unauthorized(this HttpRequestMessage requestMessage, object data) {
			return requestMessage.Unauthorized(null, data);
		}

		public static HttpResponseMessage Unauthorized(this HttpRequestMessage requestMessage, string message, object data = null) {
			return requestMessage.CreateResponse(HttpStatusCode.Unauthorized, new {
				message,
				data
			});
		}

		public static HttpResponseMessage BadRequest(this HttpRequestMessage requestMessage) {
			return requestMessage.BadRequest(null);
		}

		public static HttpResponseMessage BadRequest(this HttpRequestMessage requestMessage, object data) {
			return requestMessage.BadRequest(null, data);
		}

		public static HttpResponseMessage BadRequest(this HttpRequestMessage requestMessage, string message, object data = null) {
			return requestMessage.CreateResponse(HttpStatusCode.BadRequest, new {
				message,
				data
			});
		}

		public static HttpResponseMessage NoContent(this HttpRequestMessage requestMessage) {
			return requestMessage.CreateResponse(HttpStatusCode.NoContent);
		}
	}
}