using System;
using Core;
using Persistence.Interfaces;

namespace Persistence.Entities {
	public class UserRequest : IEntity {
		public int Id { get; set; }

		public byte[] Key { get; set; }

		public byte[] IV { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public UserRequestType RequestType { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? ModifiedDate { get; set; }
	}
}