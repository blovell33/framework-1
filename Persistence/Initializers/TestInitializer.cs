using System.Data.Entity;
using Core;
using Core.Framework;

namespace Persistence.Initializers {
	[Application(Environment = ApplicationEnvironment.Test)]
	public class TestInitializer : DropCreateDatabaseAlways<PersistenceContext> {
		protected override void Seed(PersistenceContext context) {
			var setup = new DatabaseSetup();

			setup.Seed(context);
		}
	}
}