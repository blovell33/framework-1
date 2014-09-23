using System.Configuration;
using System.Data.Entity;
using Core;
using Persistence;
using Persistence.Initializers;

namespace Website {
	public static class DatabaseConfig {
		public static void Configure() {
			var settings = new ApplicationSettings(ConfigurationManager.AppSettings);
			var factory = new InitializerFactory(settings);
			var initializer = factory.Create();

			Database.SetInitializer(initializer);

			using (var context = new PersistenceContext()) {
				context.Database.Initialize(false);
			}
		}
	}
}