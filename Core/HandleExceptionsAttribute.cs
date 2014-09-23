using System.Web.Http.Filters;

namespace Core {
	public class HandleExceptionsAttribute : ExceptionFilterAttribute {
		public bool WithHint { get; set; }

		public override void OnException(HttpActionExecutedContext context) {
			var captchaException = context.Exception as CaptchaException;

			if (captchaException != null) {
				context.Response = context.Request.NotFound();

				return;
			}

			var validationException = context.Exception as ValidationException;

			if (WithHint && validationException != null) {
				context.Response = context.Request.BadRequest(validationException.GetBaseException().Message, new {
					hint = validationException.Message
				});

				return;
			}

			context.Response = context.Request.BadRequest(context.Exception.GetBaseException().Message);
		}
	}
}