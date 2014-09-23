using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Persistence.Entities;

namespace Persistence.Mappings {
	public class UserMap : EntityTypeConfiguration<User> {
		public UserMap() {
			Property(x => x.FirstName)
				.IsRequired()
				.HasMaxLength(45);

			Property(x => x.LastName)
				.IsRequired()
				.HasMaxLength(45);

			Property(x => x.Username)
				.IsRequired()
				.HasMaxLength(45)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute {
					IsUnique = true
				}));

			Property(x => x.Email)
				.IsRequired()
				.HasMaxLength(254)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute {
					IsUnique = true
				}));

			Property(x => x.Password)
				.HasMaxLength(70)
				.IsRequired();

			Property(x => x.Enabled)
				.IsRequired();

			Property(x => x.CreatedDate)
				.IsRequired();

			Property(x => x.ModifiedDate);

			HasMany(x => x.Roles)
				.WithMany(x => x.Users)
				.Map(map => {
					map.MapLeftKey("UserId");
					map.MapRightKey("RoleId");
				});
		}
	}
}