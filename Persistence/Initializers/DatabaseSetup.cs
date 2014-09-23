using System;
using System.Data.Entity;
using Core;
using Persistence.Entities;

namespace Persistence.Initializers {
	public class DatabaseSetup {
		public void Seed(PersistenceContext context) {
			using (var transaction = context.Database.BeginTransaction()) {
				try {
					var administrator = SeedRole(context, Constants.Roles.ADMINISTRATOR_ROLE, "Administrator", "Super/Root Administrator (Full Privileges)");

					SeedRole(context, Constants.Roles.SYSTEM_MANAGER_ROLE, "System manager", "Provides limited access to system options and functions");

					var bryce = SeedUser(context, "Bryce", "Lovell", "bryce", "brycelovell@gmail.com", "^up&away1");

					bryce.Roles.Add(administrator);

					transaction.Commit();
				}
				catch (Exception e) {
					Logger.Error(e);

					transaction.Rollback();

					throw;
				}
			}
		}

		private static Role SeedRole(DbContext context, string name, string caption, string description) {
			var entity = new Role {
				Name = name,
				Caption = caption,
				Description = description,
				CreatedDate = DateTime.UtcNow
			};

			context.Set<Role>().Add(entity);

			return entity;
		}

		private static User SeedUser(DbContext context, string firstName, string lastName, string username, string email, string password) {
			var entity = new User {
				FirstName = firstName,
				LastName = lastName,
				Username = username,
				Email = email,
				Password = CryptoUtilities.CreateHash(password),
				Enabled = true,
				CreatedDate = DateTime.UtcNow
			};

			context.Set<User>().Add(entity);

			return entity;
		}
	}
}