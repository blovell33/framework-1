using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Core;
using Newtonsoft.Json.Linq;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Queries;

namespace Persistence.Services {
	public class AccountService {
		private readonly IRepository _repository;
		private readonly ValidationService _validationService;
		private readonly AdministrationService _administrationService;
		private readonly MailService _mailService;

		public AccountService(IRepository repository, ValidationService validationService, AdministrationService administrationService, MailService mailService) {
			_repository = repository;
			_validationService = validationService;
			_administrationService = administrationService;
			_mailService = mailService;
		}

		public virtual async Task RegisterAsync(dynamic model) {
			bool captcha = model.captcha ?? false;

			if (!captcha) {
				throw new CaptchaException();
			}

			model.enabled = false;

			await _administrationService.CreateUserAsync(model);

			var q = await _mailService.CreateUserRequestAsync((string) model.username, UserRequestType.Activation);

			await _mailService.SendRegistrationEmailAsync((string) model.username, q);
		}

		public virtual async Task ActivateAsync(dynamic model) {
			string q = model.q;

			int id;
			string ciphertext;

			try {
				var split = q.Split(new[] {':'});

				id = int.Parse(split[0]);
				ciphertext = split[1];
			}
			catch {
				throw new ServiceException(ServiceReason.InvalidUserRequest);
			}

			var request = await _repository.GetAsync<UserRequest>(id);

			if (request == null || request.RequestType != UserRequestType.Activation) {
				throw new ServiceException(ServiceReason.InvalidUserRequest);
			}

			try {
				var plaintext = CryptoUtilities.Decrypt(ciphertext, request.Key, request.IV);

				dynamic obj = JObject.Parse(plaintext);

				var credentials = new User {
					Username = obj.username,
					Password = obj.password
				};

				if (!CryptoUtilities.ValidatePassword(credentials.Password, request.Password)) {
					throw new ServiceException(ServiceReason.ActivationError);
				}

				var user = await _repository.UserByUsernameAsync(credentials.Username);

				user.Enabled = true;

				await _repository.UpdateAsync(user);
				await _repository.UpdateAsync(request);
			}
			catch {
				throw new ServiceException(ServiceReason.ActivationError);
			}
		}

		public virtual async Task SignInAsync(HttpContextBase httpContext, dynamic model) {
			var user = await _repository.UserByEmailAsync((string) model.email);

			if (user == null || !user.Enabled || !CryptoUtilities.ValidatePassword((string) model.password, user.Password)) {
				throw new ServiceException(ServiceReason.InvalidCredentials);
			}

			bool rememberMe = model.rememberMe ?? false;

			CreateAuthorizationTicket(httpContext, user, rememberMe);
		}

		public virtual async Task RetrievePasswordAsync(dynamic model) {
			bool captcha = model.captcha ?? false;

			if (!captcha) {
				throw new CaptchaException();
			}

			await _validationService.ValidateExistingUserByEmailAsync((string) model.email);

			var user = await _repository.UserByEmailAsync((string) model.email);

			var q = await _mailService.CreateUserRequestAsync(user.Username, UserRequestType.ResetPassword);

			await _mailService.SendResetPasswordEmailAsync(user.Username, q);
		}

		public virtual async Task ResetPasswordAsync(dynamic model) {
			_validationService.Bundles.ValidateNewPassword(model);

			string q = model.q;

			int id;
			string ciphertext;

			try {
				var split = q.Split(new[] {':'});

				id = int.Parse(split[0]);
				ciphertext = split[1];
			}
			catch {
				throw new ServiceException(ServiceReason.InvalidUserRequest);
			}

			var request = await _repository.GetAsync<UserRequest>(id);

			if (request == null || request.RequestType != UserRequestType.ResetPassword) {
				throw new ServiceException(ServiceReason.InvalidUserRequest);
			}

			try {
				var plaintext = CryptoUtilities.Decrypt(ciphertext, request.Key, request.IV);

				dynamic obj = JObject.Parse(plaintext);

				var credentials = new User {
					Username = obj.username,
					Password = obj.password
				};

				if (!CryptoUtilities.ValidatePassword(credentials.Password, request.Password)) {
					throw new ServiceException(ServiceReason.ResetPasswordError);
				}

				var user = await _repository.UserByUsernameAsync(credentials.Username);

				user.Password = CryptoUtilities.CreateHash((string) model.password);

				await _repository.UpdateAsync(user);
				await _repository.UpdateAsync(request);
			}
			catch {
				throw new ServiceException(ServiceReason.ResetPasswordError);
			}
		}

		public virtual async Task ChangePasswordAsync(string username, dynamic model) {
			await _validationService.ValidateOldPasswordAsync(username, (string) model.oldPassword);

			model.username = username;

			await _administrationService.ChangePasswordAsync(model);
		}

		public virtual async Task<object> GetPersonalInformationAsync(string username) {
			var user = await _repository.UserByUsernameAsync(username);

			if (user == null) {
				return null;
			}

			return new {
				firstName = user.FirstName,
				lastName = user.LastName,
				email = user.Email
			};
		}

		public virtual async Task UpdatePersonalInformationAsync(HttpContextBase httpContext, string username, dynamic model) {
			model.username = username;
			model.enabled = true;

			await _administrationService.UpdateUserAsync(model);

			var cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

			if (cookie == null) {
				return;
			}

			var decryptedTicket = FormsAuthentication.Decrypt(cookie.Value);

			if (decryptedTicket == null) {
				return;
			}

			var user = await _repository.UserByUsernameAsync(username);

			CreateAuthorizationTicket(httpContext, user, decryptedTicket.IsPersistent);
		}
		
		private static void CreateAuthorizationTicket(HttpContextBase httpContext, User user, bool rememberMe) {
			var data = JObject.FromObject(new {
				id = user.Id,
				firstName = user.FirstName,
				lastName = user.LastName,
				roles = user.Roles.Select(x => x.Name).ToArray()
			});

			var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.UtcNow, DateTime.UtcNow.Add(FormsAuthentication.Timeout), rememberMe, data.ToString(), FormsAuthentication.FormsCookiePath);
			var encryptedTicket = FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
				Expires = rememberMe ? ticket.Expiration : DateTime.MinValue
			};

			httpContext.Response.Cookies.Add(cookie);
		}
	}
}