using System.Security.Principal;

namespace Core {
	public class CustomPrincipal : GenericPrincipal {
		public CustomPrincipal(IIdentity identity, string[] roles, int id, string firstName, string lastName) : base(identity, roles) {
			Id = id;
			FirstName = firstName;
			LastName = lastName;
		}

		public int Id { get; private set; }

		public string FirstName { get; private set; }

		public string LastName { get; private set; }
	}
}