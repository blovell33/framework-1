using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Persistence.Queries {
	public static class RoleQueries {
		public static async Task<Role> RoleByNameAsync(this IRepository repository, string name) {
			var result =
				await
					repository
						.AsQueryable<Role>()
						.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

			return result;
		}

		public static async Task<List<Role>> RolesByNamesAsync(this IRepository repository, IEnumerable<string> names) {
			var result =
				await
					repository
						.AsQueryable<Role>()
						.Where(x => names.Contains(x.Name))
						.ToListAsync();

			return result;
		}
	}
}