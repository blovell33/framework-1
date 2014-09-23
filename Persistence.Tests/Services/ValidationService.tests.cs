using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Moq;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Services;
using Xunit;

namespace Persistence.Tests.Services {
	public class ValidationServiceValidateExistingUserByUsernameAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john"},
				new User {Username = "jill"},
				new User {Username = "jeff"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateExistingUserByUsernameAsync("john"));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john"},
				new User {Username = "jill"},
				new User {Username = "jeff"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateExistingUserByUsernameAsync("jenny"));

			// assert
			Assert.Equal(ValidationReason.UsernameIsUnavailable, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceValidateNewUserByUsernameAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john"},
				new User {Username = "jill"},
				new User {Username = "jeff"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateNewUserByUsernameAsync("jenny"));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john"},
				new User {Username = "jill"},
				new User {Username = "jeff"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateNewUserByUsernameAsync("john"));

			// assert
			Assert.Equal(ValidationReason.UsernameIsUnavailable, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceValidateExistingUserByEmailAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Email = "john@foo.com"},
				new User {Email = "jill@foo.com"},
				new User {Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateExistingUserByEmailAsync("john@foo.com"));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Email = "john@foo.com"},
				new User {Email = "jill@foo.com"},
				new User {Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateExistingUserByEmailAsync("jenny@foo.com"));

			// assert
			Assert.Equal(ValidationReason.EmailIsUnavailable, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceValidateNewUserByEmailAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Email = "john@foo.com"},
				new User {Email = "jill@foo.com"},
				new User {Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateNewUserByEmailAsync("jenny@foo.com"));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Email = "john@foo.com"},
				new User {Email = "jill@foo.com"},
				new User {Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateNewUserByEmailAsync("john@foo.com"));

			// assert
			Assert.Equal(ValidationReason.EmailIsUnavailable, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceValidateCurrentUserByEmailAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john", Email = "john@foo.com"},
				new User {Username = "jill", Email = "jill@foo.com"},
				new User {Username = "jeff", Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateCurrentUserByEmailAsync("john", "john@foo.com"));
			Assert.DoesNotThrow(async () => await validationService.ValidateCurrentUserByEmailAsync("john", "jenny@foo.com"));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Exactly(2));
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = "john", Email = "john@foo.com"},
				new User {Username = "jill", Email = "jill@foo.com"},
				new User {Username = "jeff", Email = "jeff@foo.com"}
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateCurrentUserByEmailAsync("john", "jill@foo.com"));

			// assert
			Assert.Equal(ValidationReason.EmailIsUnavailable, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceValidateOldPasswordAsyncTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var obj = new {
				username = "john",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = obj.username, Password = CryptoUtilities.CreateHash(obj.password)},
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			Assert.DoesNotThrow(async () => await validationService.ValidateOldPasswordAsync(obj.username, obj.password));

			// assert
			repository.Verify(x => x.AsQueryable<User>(), Times.Exactly(1));
		}

		[Fact]
		public async Task TestInvalidCaseAsync() {
			// arrange
			var obj = new {
				username = "john",
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var set = TestUtilities.CreateDbSetMock(new List<User> {
				new User {Username = obj.username, Password = CryptoUtilities.CreateHash(obj.password)},
			});

			repository
				.Setup(x => x.AsQueryable<User>())
				.Returns(set.Object);

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = await TestUtilities.ThrowsAsync<ValidationException>(async () => await validationService.ValidateOldPasswordAsync(obj.username, "something else"));

			// assert
			Assert.Equal(ValidationReason.PasswordIsIncorrect, exception.Reason);
			repository.Verify(x => x.AsQueryable<User>(), Times.Once);
		}
	}

	public class ValidationServiceIsRequiredTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateIsRequired("foo"));
		}

		[Fact]
		public void TestInvalidCase1() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateIsRequired(null));

			// assert
			Assert.Equal(ValidationReason.FieldIsRequired, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase2() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateIsRequired("      "));

			// assert
			Assert.Equal(ValidationReason.FieldIsRequired, exception.Reason);
		}
	}

	public class ValidationServiceValidateUsernameTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateUsername("jenny"));
		}

		[Fact]
		public void TestInvalidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateUsername("jenny jones"));

			// assert
			Assert.Equal(ValidationReason.UsernameIsIncorrectFormat, exception.Reason);
		}
	}

	public class ValidationServiceValidateEmailTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateEmail("jenny@foo.com"));
		}

		[Fact]
		public void TestInvalidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateEmail("jenny@@foo.com"));

			// assert
			Assert.Equal(ValidationReason.EmailIsIncorrectFormat, exception.Reason);
		}
	}

	public class ValidationServiceValidatePasswordTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var obj = new {
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new Mock<ApplicationSettings>();

			settings
				.SetupGet(x => x.MinPasswordScore)
				.Returns(PasswordScore.Medium);

			settings
				.Setup(x => x.CheckStrength(obj.password))
				.Returns(PasswordScore.Medium);

			var validationService = new ValidationService(repository.Object, settings.Object);

			// act
			Assert.DoesNotThrow(() => validationService.ValidatePassword(obj.password));

			// assert
			settings.Verify(x => x.CheckStrength(obj.password), Times.Once);
		}

		[Fact]
		public void TestInvalidCase() {
			// arrange
			var obj = new {
				password = "!!0u812"
			};

			var repository = new Mock<IRepository>();
			var settings = new Mock<ApplicationSettings>();

			settings
				.SetupGet(x => x.MinPasswordScore)
				.Returns(PasswordScore.Medium);

			settings
				.Setup(x => x.CheckStrength(obj.password))
				.Returns(PasswordScore.Weak);

			var validationService = new ValidationService(repository.Object, settings.Object);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidatePassword(obj.password));

			// assert
			Assert.Equal(ValidationReason.PasswordIsTooWeak, exception.Reason);
			settings.Verify(x => x.CheckStrength(obj.password), Times.Once);
		}
	}

	public class ValidationServiceValidateComparisonTests {
		[Fact]
		public void TestValidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateComparison("foo", "foo"));
		}

		[Fact]
		public void TestInvalidCase() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateComparison("foo", "Foo"));

			// assert
			Assert.Equal(ValidationReason.ComparisonDoesNotMatch, exception.Reason);
		}
	}

	public class ValidationServiceValidateGreaterThanTests {
		[Fact]
		public void TestValidCaseWithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateGreaterThan("1", "0"));
		}

		[Fact]
		public void TestInvalidCase1WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThan("0", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase2WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThan("invalid", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase3WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThan("0", "invalid"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestValidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateGreaterThan("1", 0));
		}

		[Fact]
		public void TestInvalidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThan("0", 0));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}
	}

	public class ValidationServiceValidateGreaterThanEqualToTests {
		[Fact]
		public void TestValidCaseWithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateGreaterThanEqualTo("0", "0"));
		}

		[Fact]
		public void TestInvalidCase1WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThanEqualTo("-1", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase2WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThanEqualTo("invalid", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase3WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThanEqualTo("0", "invalid"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestValidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateGreaterThanEqualTo("0", 0));
		}

		[Fact]
		public void TestInvalidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateGreaterThanEqualTo("-1", 0));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}
	}

	public class ValidationServiceValidateLessThanTests {
		[Fact]
		public void TestValidCaseWithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateLessThan("0", "1"));
		}

		[Fact]
		public void TestInvalidCase1WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThan("0", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase2WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThan("invalid", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase3WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThan("0", "invalid"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestValidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateLessThan("0", 1));
		}

		[Fact]
		public void TestInvalidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThan("0", 0));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}
	}

	public class ValidationServiceValidateLessThanEqualToTests {
		[Fact]
		public void TestValidCaseWithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateLessThanEqualTo("0", "0"));
		}

		[Fact]
		public void TestInvalidCase1WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThanEqualTo("0", "-1"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase2WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThanEqualTo("invalid", "0"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestInvalidCase3WithString() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThanEqualTo("0", "invalid"));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}

		[Fact]
		public void TestValidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act/assert
			Assert.DoesNotThrow(() => validationService.ValidateLessThanEqualTo("0", 0));
		}

		[Fact]
		public void TestInvalidCaseWithNumber() {
			// arrange
			var repository = new Mock<IRepository>();
			var settings = new ApplicationSettings();

			var validationService = new ValidationService(repository.Object, settings);

			// act
			var exception = Assert.Throws<ValidationException>(() => validationService.ValidateLessThanEqualTo("0", -1));

			// assert
			Assert.Equal(ValidationReason.InvalidValue, exception.Reason);
		}
	}
}