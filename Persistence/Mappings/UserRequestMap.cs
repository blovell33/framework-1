using System.Data.Entity.ModelConfiguration;
using Persistence.Entities;

namespace Persistence.Mappings {
	public class UserRequestMap : EntityTypeConfiguration<UserRequest> {
		public UserRequestMap() {
			Property(x => x.Key)
				.HasMaxLength(24)
				.IsRequired();

			Property(x => x.IV)
				.HasMaxLength(8)
				.IsRequired();

			Property(x => x.Username)
				.IsRequired()
				.HasMaxLength(45);

			Property(x => x.Password)
				.HasMaxLength(70)
				.IsRequired();

			Property(x => x.RequestType)
				.IsRequired();

			Property(x => x.CreatedDate)
				.IsRequired();

			Property(x => x.ModifiedDate);
		}
	}
}