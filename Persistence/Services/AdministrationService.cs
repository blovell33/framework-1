using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Persistence.Components;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Queries;

namespace Persistence.Services {
	public class AdministrationService {
		private readonly IRepository _repository;
		private readonly ValidationService _validationService;

		public AdministrationService(IRepository repository, ValidationService validationService) {
			_repository = repository;
			_validationService = validationService;
		}

		public virtual async Task<object> GetAllUsersAsync(QueryParameters parameters) {
			var builder = _repository.GetPage<User>(parameters);
			var users = await builder.QueryAsync();
			var records = await builder.CountAsync();

			return
				new {
					stats =
						builder
							.Stats(records),
					columns =
						builder
							.Column(x => x.FirstName)
							.Column(x => x.LastName)
							.Column(x => x.Username)
							.Column(x => x.Email)
							.Column(x => x.Enabled, CellAlignment.Center)
							.Column(x => x.CreatedDate, CellAlignment.Right)
							.Column(x => x.ModifiedDate, CellAlignment.Right)
							.ToArray(),
					rows =
						users
							.Select(x => new {
								url = "/administration/user/" + x.Username,
								cells =
									builder
										.Cell(x.FirstName)
										.Cell(x.LastName)
										.Cell(x.Username)
										.Cell(x.Email)
										.Cell(x.Enabled)
										.Cell(x.CreatedDate)
										.Cell(x.ModifiedDate)
										.ToArray()
							})
				};
		}

		public virtual async Task<object> GetNewUserAsync() {
			var roles = await _repository.GetAllAsync<Role>();

			return
				new {
					enabled = true,
					roles =
						roles
							.Select(x => new {
								name = x.Name,
								caption = x.Caption,
								selected = false
							})
				};
		}

		public virtual async Task<object> GetUserAsync(string username) {
			var user = await _repository.UserByUsernameAsync(username);
			var roles = await _repository.GetAllAsync<Role>();

			if (user == null) {
				return null;
			}

			return
				new {
					firstName = user.FirstName,
					lastName = user.LastName,
					username = user.Username,
					email = user.Email,
					enabled = user.Enabled,
					roles =
						roles
							.Select(x => new {
								name = x.Name,
								caption = x.Caption,
								selected = user.Roles.Contains(x)
							})
				};
		}

		public virtual async Task CreateUserAsync(dynamic model) {
			await _validationService.ValidateNewUserByUsernameAsync((string) model.username);
			await _validationService.ValidateNewUserByEmailAsync((string) model.email);

			_validationService.Bundles.ValidateUser(model);
			_validationService.Bundles.ValidateNewPassword(model);

			var user = new User {
				Username = model.username,
				FirstName = model.firstName,
				LastName = model.lastName,
				Email = model.email,
				Password = CryptoUtilities.CreateHash((string) model.password),
				Enabled = model.enabled
			};

			await AddRolesAsync(model, user);

			await _repository.InsertAsync(user);
		}

		public virtual async Task UpdateUserAsync(dynamic model) {
			await _validationService.ValidateCurrentUserByEmailAsync((string) model.username, (string) model.email);

			_validationService.Bundles.ValidateUser(model);

			var user = await _repository.UserByUsernameAsync((string) model.username);

			user.FirstName = model.firstName;
			user.LastName = model.lastName;
			user.Email = model.email;
			user.Enabled = model.enabled;

			await AddRolesAsync(model, user);

			await _repository.UpdateAsync(user);
		}

		public virtual async Task ChangePasswordAsync(dynamic model) {
			_validationService.Bundles.ValidateNewPassword(model);

			var user = await _repository.UserByUsernameAsync((string) model.username);

			user.Password = CryptoUtilities.CreateHash((string) model.password);

			await _repository.UpdateAsync(user);
		}

		public virtual async Task DeleteUserAsync(dynamic model) {
			var user = await _repository.UserByUsernameAsync((string) model.username);

			await _repository.DeleteAsync(user);
		}

		public virtual async Task<object> GetAllRolesAsync(QueryParameters parameters) {
			var builder = _repository.GetPage<Role>(parameters);
			var roles = await builder.QueryAsync();
			var records = await builder.CountAsync();

			return
				new {
					stats =
						builder
							.Stats(records),
					columns =
						builder
							.Column(x => x.Name)
							.Column(x => x.Caption)
							.Column(x => x.Description)
							.ToArray(),
					rows =
						roles
							.Select(x => new {
								url = "/administration/role/" + x.Name,
								cells =
									builder
										.Cell(x.Name)
										.Cell(x.Caption)
										.Cell(x.Description)
										.ToArray()
							})
				};
		}

		public virtual async Task<object> GetRoleAsync(string name) {
			var role = await _repository.RoleByNameAsync(name);

			if (role == null) {
				return null;
			}

			return
				new {
					name = role.Name,
					caption = role.Caption,
					description = role.Description,
					users =
						role
							.Users
							.Select(x => new {
								url = "/administration/user/" + x.Username,
								name = string.Format("{0} {1}", x.FirstName, x.LastName)
							})
							.OrderBy(x => x.name)
				};
		}

		private async Task AddRolesAsync(dynamic model, User user) {
			if (model.roles == null) {
				return;
			}

			user.Roles.Clear();

			var names = new List<string>();

			foreach (var role in model.roles) {
				string name = role.name;
				bool selected = role.selected;

				if (selected) {
					names.Add(name);
				}
			}

			var roles = await _repository.RolesByNamesAsync(names);

			foreach (var role in roles) {
				user.Roles.Add(role);
			}
		}
	}
}