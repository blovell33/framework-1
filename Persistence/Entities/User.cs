using System;
using System.Collections.Generic;
using Core;
using Persistence.Interfaces;

namespace Persistence.Entities {
	public class User : IEntity {
		public User() {
			Roles = new HashSet<Role>();
		}

		public int Id { get; set; }

		[Filterable]
		public string FirstName { get; set; }

		[Filterable]
		public string LastName { get; set; }

		[Filterable]
		public string Username { get; set; }

		[Filterable]
		public string Email { get; set; }

		public string Password { get; set; }

		[Filterable]
		public bool Enabled { get; set; }

		[Filterable]
		public DateTime CreatedDate { get; set; }

		[Filterable]
		public DateTime? ModifiedDate { get; set; }

		public virtual ICollection<Role> Roles { get; private set; }
	}
}