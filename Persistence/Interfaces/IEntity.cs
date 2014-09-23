using System;

namespace Persistence.Interfaces {
	public interface IEntity {
		int Id { get; set; }

		DateTime CreatedDate { get; set; }

		DateTime? ModifiedDate { get; set; }
	}
}