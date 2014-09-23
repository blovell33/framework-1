using System;
using System.Collections.Generic;
using Core;
using Persistence.Interfaces;

namespace Persistence.Entities {
	public class Role : IEntity {
		public Role() {
			Users = new HashSet<User>();
		}

		public int Id { get; set; }

		[Filterable]
		public string Name { get; set; }

		[Filterable]
		public string Caption { get; set; }

		[Filterable]
		public string Description { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		public virtual ICollection<User> Users { get; private set; }
	}
}