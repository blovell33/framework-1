﻿using System.Data.Entity;
using Core;
using Core.Framework;

namespace Persistence.Initializers {
	[Application(Environment = ApplicationEnvironment.Development)]
	public class DevelopmentInitializer : DropCreateDatabaseIfModelChanges<PersistenceContext> {
		protected override void Seed(PersistenceContext context) {
			var setup = new DatabaseSetup();

			setup.Seed(context);
		}
	}
}