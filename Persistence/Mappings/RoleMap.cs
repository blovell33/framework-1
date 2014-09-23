using System.Data.Entity.ModelConfiguration;
using Persistence.Entities;

namespace Persistence.Mappings {
	public class RoleMap : EntityTypeConfiguration<Role> {
		public RoleMap() {
			Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(45);

			Property(x => x.Caption)
				.IsRequired()
				.HasMaxLength(45);

			Property(x => x.Description)
				.IsRequired()
				.HasMaxLength(150);

			Property(x => x.CreatedDate)
				.IsRequired();

			Property(x => x.ModifiedDate);
		}
	}
}