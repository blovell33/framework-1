using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Core;
using Core.Framework;

namespace Persistence.Initializers {
	public class InitializerFactory {
		private readonly ApplicationSettings _settings;

		public InitializerFactory(ApplicationSettings settings) {
			_settings = settings;
		}

		public IDatabaseInitializer<PersistenceContext> Create() {
			var target = typeof (IDatabaseInitializer<PersistenceContext>);

			var types =
				Assembly
					.GetExecutingAssembly()
					.GetTypes()
					.Where(type => Attribute.IsDefined(type, typeof (ApplicationAttribute)) && target.IsAssignableFrom(type));

			return
				types
					.Select(type => new {
						type,
						attribute =
							type
								.GetCustomAttributes(typeof (ApplicationAttribute), false)
								.Cast<ApplicationAttribute>()
								.FirstOrDefault(x => x.Environment == _settings.Environment)
					})
					.Where(x => x.attribute != null)
					.Select(x => (IDatabaseInitializer<PersistenceContext>) Activator.CreateInstance(x.type))
					.FirstOrDefault();
		}
	}
}