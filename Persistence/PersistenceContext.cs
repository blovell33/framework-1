using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace Persistence {
	public class PersistenceContext : DbContext {
		protected override void OnModelCreating(DbModelBuilder builder) {
			var types =
				Assembly
					.GetExecutingAssembly()
					.GetTypes()
					.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof (EntityTypeConfiguration<>));

			foreach (var map in types.Select(Activator.CreateInstance)) {
				builder.Configurations.Add((dynamic) map);
			}
		}
	}
}