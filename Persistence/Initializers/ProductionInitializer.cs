using System.Data.Entity;
using Core;
using Core.Framework;

namespace Persistence.Initializers {
	[Application(Environment = ApplicationEnvironment.Production)]
	public class ProductionInitializer : CreateDatabaseIfNotExists<PersistenceContext> {
		protected override void Seed(PersistenceContext context) {
			var setup = new DatabaseSetup();

			setup.Seed(context);
		}
	}
}