using System.Security.Principal;
using Moq;
using Xunit;

namespace Core.Tests {
	public class CustomPrincipalConstructorTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var identity = new Mock<IIdentity>();

			var obj = new {
				id = 1,
				firstName = "John",
				lastName = "Smith",
				roles = new[] {
					Constants.Roles.ADMINISTRATOR_ROLE
				}
			};

			// act
			var principal = new CustomPrincipal(identity.Object, obj.roles, obj.id, obj.firstName, obj.lastName);

			// assert
			Assert.Equal(obj.id, principal.Id);
			Assert.Equal(obj.firstName, principal.FirstName);
			Assert.Equal(obj.lastName, principal.LastName);
			Assert.True(principal.IsInRole(Constants.Roles.ADMINISTRATOR_ROLE));
		}
	}
}