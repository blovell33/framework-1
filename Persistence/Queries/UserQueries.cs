using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Queries {
	public static class UserQueries {
		public static async Task<User> UserByUsernameAsync(this IRepository repository, string username) {
			var result =
				await
					repository
						.AsQueryable<User>()
						.FirstOrDefaultAsync(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

			return result;
		}

		public static async Task<User> UserByEmailAsync(this IRepository repository, string email) {
			var result =
				await
					repository
						.AsQueryable<User>()
						.FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

			return result;
		}
	}
}