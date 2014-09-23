using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Core;
using Newtonsoft.Json.Linq;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Services {
	public class MailService {
		private readonly IRepository _repository;

		public MailService(IRepository repository) {
			_repository = repository;
		}

		public virtual async Task<string> CreateUserRequestAsync(string username, UserRequestType requestType) {
			var password = Guid.NewGuid().ToString();
			var credentials = JObject.FromObject(new {
				username,
				password
			});

			using (var algorithm = TripleDES.Create()) {
				var request = new UserRequest {
					Key = algorithm.Key,
					IV = algorithm.IV,
					Username = username,
					Password = CryptoUtilities.CreateHash(password),
					RequestType = requestType
				};

				await _repository.InsertAsync(request);

				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				return HttpUtility.UrlEncode(request.Id + ":" + ciphertext);
			}
		}

		public virtual async Task SendRegistrationEmailAsync(string username, string q) {
			await Task.Run(() => {
				Logger.Info("Sending registration email for {0}...", username);
				Logger.Debug("q=" + q);
			});
		}

		public virtual async Task SendResetPasswordEmailAsync(string username, string q) {
			await Task.Run(() => {
				Logger.Info("Sending reset password email for {0}...", username);
				Logger.Debug("q=" + q);
			});
		}
	}
}