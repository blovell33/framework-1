using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Core;
using Moq;
using Newtonsoft.Json.Linq;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Services;
using Xunit;

namespace Persistence.Tests.Services {
	public class AccountServiceRegisterAsyncTests {
		[Fact]
		public async Task TestWhenCaptchaIsNullAsync() {
			// arrange
			var repository = Mock.Of<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository, settings);
			var administrationService = new Mock<AdministrationService>(repository, validationService.Object);
			var mailService = new Mock<MailService>(repository);

			dynamic model = new JObject();

			var accountService = new AccountService(repository, validationService.Object, administrationService.Object, mailService.Object);

			// act/assert
			await TestUtilities.ThrowsAsync<CaptchaException>(async () => await accountService.RegisterAsync(model));
		}

		[Fact]
		public async Task TestWhenCaptchaIsFalseAsync() {
			// arrange
			var repository = Mock.Of<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository, settings);
			var administrationService = new Mock<AdministrationService>(repository, validationService.Object);
			var mailService = new Mock<MailService>(repository);

			dynamic model = JObject.FromObject(new {
				captcha = false
			});

			var accountService = new AccountService(repository, validationService.Object, administrationService.Object, mailService.Object);

			// act/assert
			await TestUtilities.ThrowsAsync<CaptchaException>(async () => await accountService.RegisterAsync(model));
		}

		[Fact]
		public async Task TestWhenCaptchaIsTrueAsync() {
			// arrange
			var obj = new {
				firstName = "John",
				lastName = "Smith",
				username = "john.smith",
				email = "john.smith@foo.com",
				password = "!!0u812",
				confirmPassword = "!!0u812",
				captcha = true
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var userSet = TestUtilities.CreateDbSetMock(new List<User>());

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await accountService.RegisterAsync(model);

			// assert
			Assert.False((bool) model.enabled);

			administrationService.Verify(x => x.CreateUserAsync(It.IsAny<object>()), Times.Once);

			mailService.Verify(x => x.CreateUserRequestAsync(obj.username, UserRequestType.Activation), Times.Once);
			mailService.Verify(x => x.SendRegistrationEmailAsync(obj.username, It.IsAny<string>()), Times.Once);
		}
	}

	public class AccountServiceActivateAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				username = "john.smith"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = obj.username
			};

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.Activation
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			repository
				.Setup(x => x.GetAsync<User>(1))
				.ReturnsAsync(user);

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var password = Guid.NewGuid().ToString();
			var credentials = JObject.FromObject(new {
				obj.username,
				password
			});

			string q;

			using (var algorithm = TripleDES.Create()) {
				request.Key = algorithm.Key;
				request.IV = algorithm.IV;
				request.Password = CryptoUtilities.CreateHash(password);

				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await TestUtilities.DoesNotThrowAsync(async () => await accountService.ActivateAsync(model));

			// assert
			Assert.True(user.Enabled);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
			repository.Verify(x => x.UpdateAsync(user), Times.Once());
			repository.Verify(x => x.UpdateAsync(request), Times.Once());
		}

		[Fact]
		public async Task TestWithInvalidQueryAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			dynamic model = JObject.FromObject(new {
				q = "something invalid"
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ActivateAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Never);
		}

		[Fact]
		public async Task TestWithNullUserRequestAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ActivateAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async Task TestWithInvalidUserRequestAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.ResetPassword
			};

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ActivateAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async Task TestWithInvalidPasswordAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = obj.username
			};

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.Activation
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var password = Guid.NewGuid().ToString();
			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				request.Key = algorithm.Key;
				request.IV = algorithm.IV;
				request.Password = CryptoUtilities.CreateHash(password);

				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ActivateAsync(model));

			// assert
			Assert.Equal(ServiceReason.ActivationError, exception.Reason);
			Assert.False(user.Enabled);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
			repository.Verify(x => x.AsQueryable<User>(), Times.Never);
			repository.Verify(x => x.UpdateAsync(user), Times.Never());
			repository.Verify(x => x.UpdateAsync(request), Times.Never());
		}
	}

	public class AccountServiceSignInAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				email = "john.smith@foo.com",
				password = "!!0u812",
				rememberMe = false
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpResponse = new Mock<HttpResponseBase>();
			var cookies = new HttpCookieCollection();

			var user = new User {
				Email = obj.email,
				Password = CryptoUtilities.CreateHash(obj.password),
				Enabled = true
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(httpResponse.Object);

			httpResponse
				.SetupGet(x => x.Cookies)
				.Returns(cookies);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await TestUtilities.DoesNotThrowAsync(async () => await accountService.SignInAsync(httpContext.Object, model));

			// assert
			Assert.Equal(httpResponse.Object.Cookies[".ASPXAUTH"].Expires, DateTime.MinValue);
		}

		[Fact]
		public async Task TestWithDisabledUserAsync() {
			// arrange
			var obj = new {
				email = "john.smith@foo.com",
				password = "!!0u812",
				rememberMe = false
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpResponse = new Mock<HttpResponseBase>();
			var cookies = new HttpCookieCollection();

			var user = new User {
				Email = obj.email,
				Password = CryptoUtilities.CreateHash(obj.password)
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(httpResponse.Object);

			httpResponse
				.SetupGet(x => x.Cookies)
				.Returns(cookies);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.SignInAsync(httpContext.Object, model));

			// assert
			Assert.Equal(exception.Reason, ServiceReason.InvalidCredentials);

			httpResponse.VerifyGet(x => x.Cookies, Times.Never);
		}

		[Fact]
		public async Task TestWithInvalidUserAsync() {
			// arrange
			var obj = new {
				email = "john.smith@foo.com",
				password = "!!0u812",
				rememberMe = false
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpResponse = new Mock<HttpResponseBase>();
			var cookies = new HttpCookieCollection();

			var user = new User {
				Email = "jane.smith@foo.com",
				Password = CryptoUtilities.CreateHash(obj.password)
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(httpResponse.Object);

			httpResponse
				.SetupGet(x => x.Cookies)
				.Returns(cookies);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.SignInAsync(httpContext.Object, model));

			// assert
			Assert.Equal(exception.Reason, ServiceReason.InvalidCredentials);

			httpResponse.VerifyGet(x => x.Cookies, Times.Never);
		}

		[Fact]
		public async Task TestWithInvalidPasswordAsync() {
			// arrange
			var obj = new {
				email = "john.smith@foo.com",
				password = "!!0u812",
				rememberMe = false
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpResponse = new Mock<HttpResponseBase>();
			var cookies = new HttpCookieCollection();

			var user = new User {
				Email = obj.email,
				Password = CryptoUtilities.CreateHash("something else")
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(httpResponse.Object);

			httpResponse
				.SetupGet(x => x.Cookies)
				.Returns(cookies);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.SignInAsync(httpContext.Object, model));

			// assert
			Assert.Equal(exception.Reason, ServiceReason.InvalidCredentials);

			httpResponse.VerifyGet(x => x.Cookies, Times.Never);
		}
	}

	public class AccountServiceRetrievePasswordAsyncTests {
		[Fact]
		public async Task TestWhenCaptchaIsNullAsync() {
			// arrange
			var repository = Mock.Of<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository, settings);
			var administrationService = new Mock<AdministrationService>(repository, validationService.Object);
			var mailService = new Mock<MailService>(repository);

			dynamic model = new JObject();

			var accountService = new AccountService(repository, validationService.Object, administrationService.Object, mailService.Object);

			// act/assert
			await TestUtilities.ThrowsAsync<CaptchaException>(async () => await accountService.RetrievePasswordAsync(model));
		}

		[Fact]
		public async Task TestWhenCaptchaIsFalseAsync() {
			// arrange
			var repository = Mock.Of<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository, settings);
			var administrationService = new Mock<AdministrationService>(repository, validationService.Object);
			var mailService = new Mock<MailService>(repository);

			dynamic model = JObject.FromObject(new {
				captcha = false
			});

			var accountService = new AccountService(repository, validationService.Object, administrationService.Object, mailService.Object);

			// act/assert
			await TestUtilities.ThrowsAsync<CaptchaException>(async () => await accountService.RetrievePasswordAsync(model));
		}

		[Fact]
		public async Task TestWhenCaptchaIsTrueAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				email = "john.smith@foo.com",
				captcha = true
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = obj.username,
				Email = obj.email
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await accountService.RetrievePasswordAsync(model);

			// assert
			validationService.Verify(x => x.ValidateExistingUserByEmailAsync(obj.email), Times.Once);

			mailService.Verify(x => x.CreateUserRequestAsync(obj.username, UserRequestType.ResetPassword), Times.Once);
			mailService.Verify(x => x.SendResetPasswordEmailAsync(obj.username, It.IsAny<string>()), Times.Once);
		}
	}

	public class AccountServiceResetPasswordAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var validationBundles = new Mock<ValidationBundles>(validationService.Object);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			validationService
				.SetupGet(x => x.Bundles)
				.Returns(validationBundles.Object);

			var user = new User {
				Username = obj.username
			};

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.ResetPassword
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			repository
				.Setup(x => x.GetAsync<User>(1))
				.ReturnsAsync(user);

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var password = Guid.NewGuid().ToString();
			var credentials = JObject.FromObject(new {
				obj.username,
				password
			});

			string q;

			using (var algorithm = TripleDES.Create()) {
				request.Key = algorithm.Key;
				request.IV = algorithm.IV;
				request.Password = CryptoUtilities.CreateHash(password);

				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				obj.password,
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await TestUtilities.DoesNotThrowAsync(async () => await accountService.ResetPasswordAsync(model));

			// assert
			Assert.True(CryptoUtilities.ValidatePassword(obj.password, user.Password));

			validationBundles.Verify(x => x.ValidateNewPassword(It.IsAny<object>()), Times.Once);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
			repository.Verify(x => x.UpdateAsync(user), Times.Once());
			repository.Verify(x => x.UpdateAsync(request), Times.Once());
		}

		[Fact]
		public async Task TestWithInvalidQueryAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			validationService
				.SetupGet(x => x.Bundles)
				.Returns(new ValidationBundles(validationService.Object));

			dynamic model = JObject.FromObject(new {
				q = "something invalid"
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ResetPasswordAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Never);
		}

		[Fact]
		public async Task TestWithNullUserRequestAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			validationService
				.SetupGet(x => x.Bundles)
				.Returns(new ValidationBundles(validationService.Object));

			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ResetPasswordAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async Task TestWithInvalidUserRequestAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			validationService
				.SetupGet(x => x.Bundles)
				.Returns(new ValidationBundles(validationService.Object));

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.Activation
			};

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ResetPasswordAsync(model));

			// assert
			Assert.Equal(ServiceReason.InvalidUserRequest, exception.Reason);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async Task TestWithInvalidPasswordAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			validationService
				.SetupGet(x => x.Bundles)
				.Returns(new ValidationBundles(validationService.Object));

			var user = new User {
				Username = obj.username
			};

			var request = new UserRequest {
				Id = 1,
				Username = obj.username,
				RequestType = UserRequestType.ResetPassword
			};

			var userSet = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(userSet.Object);

			repository
				.Setup(x => x.GetAsync<UserRequest>(1))
				.ReturnsAsync(request);

			var password = Guid.NewGuid().ToString();
			var credentials = JObject.FromObject(obj);

			string q;

			using (var algorithm = TripleDES.Create()) {
				request.Key = algorithm.Key;
				request.IV = algorithm.IV;
				request.Password = CryptoUtilities.CreateHash(password);

				var ciphertext = CryptoUtilities.Encrypt(credentials.ToString(), algorithm.Key, algorithm.IV);

				q = 1 + ":" + ciphertext;
			}

			dynamic model = JObject.FromObject(new {
				q
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var exception = await TestUtilities.ThrowsAsync<ServiceException>(async () => await accountService.ResetPasswordAsync(model));

			// assert
			Assert.Equal(ServiceReason.ResetPasswordError, exception.Reason);
			Assert.Null(user.Password);

			repository.Verify(x => x.GetAsync<UserRequest>(It.IsAny<int>()), Times.Once);
			repository.Verify(x => x.AsQueryable<User>(), Times.Never);
			repository.Verify(x => x.UpdateAsync(user), Times.Never());
			repository.Verify(x => x.UpdateAsync(request), Times.Never());
		}
	}

	public class AccountServiceChangePasswordAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				oldPassword = "something old",
				password = "!!0u812",
				confirmPassword = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = obj.username,
				Password = CryptoUtilities.CreateHash(obj.oldPassword)
			};

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			dynamic model = JObject.FromObject(new {
				obj.oldPassword,
				obj.password,
				obj.confirmPassword
			});

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await accountService.ChangePasswordAsync(obj.username, model);

			// assert
			Assert.Equal(obj.username, (string) model.username);

			validationService.Verify(x => x.ValidateOldPasswordAsync(obj.username, obj.oldPassword), Times.Once);

			administrationService.Verify(x => x.ChangePasswordAsync(It.IsAny<object>()), Times.Once);
		}
	}

	public class AccountServiceGetPersonalInformationAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				firstName = "John",
				lastName = "Smith",
				email = "john.smith@foo.com"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = obj.username,
				FirstName = obj.firstName,
				LastName = obj.lastName,
				Email = obj.email
			};

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var result = await accountService.GetPersonalInformationAsync(obj.username);

			// assert
			dynamic model = JObject.FromObject(result);

			Assert.Equal(obj.firstName, (string) model.firstName);
			Assert.Equal(obj.lastName, (string) model.lastName);
			Assert.Equal(obj.email, (string) model.email);
		}

		[Fact]
		public async Task TestWithNullResultAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				firstName = "John",
				lastName = "Smith",
				email = "john.smith@foo.com"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);

			var user = new User {
				Username = "jane.smith",
				FirstName = "Jane",
				LastName = "Smith",
				Email = "jane.smith@foo.com"
			};

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			var result = await accountService.GetPersonalInformationAsync(obj.username);

			// assert
			Assert.Null(result);
		}
	}

	public class AccountServiceUpdatePersonalInformationAsyncTests {
		[Fact]
		public async Task TestDefaultBehaviorAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				firstName = "Johnny",
				lastName = "Lingo",
				email = "johnny.lingo@foo.com"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpRequest = new Mock<HttpRequestBase>();
			var cookies = new HttpCookieCollection();

			httpContext
				.SetupGet(x => x.Request)
				.Returns(httpRequest.Object);

			httpRequest
				.SetupGet(x => x.Cookies)
				.Returns(cookies);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await accountService.UpdatePersonalInformationAsync(httpContext.Object, obj.username, model);

			// assert
			administrationService.Verify(x => x.UpdateUserAsync(It.IsAny<object>()));
		}

		[Fact]
		public async Task TestWithCookieAsync() {
			// arrange
			var obj = new {
				username = "john.smith",
				firstName = "Johnny",
				lastName = "Lingo",
				email = "johnny.lingo@foo.com"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();
			var validationService = new Mock<ValidationService>(repository.Object, settings);
			var administrationService = new Mock<AdministrationService>(repository.Object, validationService.Object);
			var mailService = new Mock<MailService>(repository.Object);
			var httpContext = new Mock<HttpContextBase>();
			var httpRequest = new Mock<HttpRequestBase>();
			var httpResponse = new Mock<HttpResponseBase>();
			var requestCookies = new HttpCookieCollection();
			var responseCookies = new HttpCookieCollection();

			var user = new User {
				Username = obj.username,
				FirstName = obj.firstName,
				LastName = obj.lastName,
				Email = obj.email
			};

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				user
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			httpContext
				.SetupGet(x => x.Request)
				.Returns(httpRequest.Object);

			httpContext
				.SetupGet(x => x.Response)
				.Returns(httpResponse.Object);

			httpRequest
				.SetupGet(x => x.Cookies)
				.Returns(requestCookies);

			httpResponse
				.SetupGet(x => x.Cookies)
				.Returns(responseCookies);

			var data = JObject.FromObject(new {
				id = user.Id,
				firstName = "John",
				lastName = "Smith",
				roles = new string[] {}
			});

			var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.UtcNow, DateTime.UtcNow.Add(FormsAuthentication.Timeout), false, data.ToString(), FormsAuthentication.FormsCookiePath);
			var encryptedTicket = FormsAuthentication.Encrypt(ticket);
			var requestCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
				Expires = DateTime.MinValue
			};

			requestCookies.Add(requestCookie);

			dynamic model = JObject.FromObject(obj);

			var accountService = new AccountService(repository.Object, validationService.Object, administrationService.Object, mailService.Object);

			// act
			await accountService.UpdatePersonalInformationAsync(httpContext.Object, obj.username, model);

			// assert
			var responseCookie = responseCookies[FormsAuthentication.FormsCookieName];
			var decryptedTicket = FormsAuthentication.Decrypt(responseCookie.Value);
			dynamic result = JObject.Parse(decryptedTicket.UserData);

			Assert.Equal(obj.firstName, (string) result.firstName);
			Assert.Equal(obj.lastName, (string) result.lastName);
		}
	}
}