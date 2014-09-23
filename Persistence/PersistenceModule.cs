using System;
using System.Data.Entity;
using System.Web;
using Core;
using Ninject.Modules;
using Ninject.Web.Common;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence {
	public class PersistenceModule : NinjectModule {
		public override void Load() {
			Bind<AccountService>()
				.ToSelf()
				.InRequestScope();

			Bind<AdministrationService>()
				.ToSelf()
				.InRequestScope();

			Bind<MailService>()
				.ToSelf()
				.InRequestScope();

			Bind<SystemService>()
				.ToSelf()
				.InRequestScope();

			Bind<ValidationService>()
				.ToSelf()
				.InRequestScope();

			Bind<IRepository>()
				.To<Repository>()
				.InRequestScope();

			Bind<PersistenceContext>()
				.ToSelf()
				.InRequestScope()
				.OnActivation((context, persistenceContext) => {
					Logger.Debug("Activating transaction...");

					var scope = context.GetScope() as HttpContext;

					if (scope != null) {
						scope.Items.Add("__transaction__", persistenceContext.Database.BeginTransaction());
					}
				})
				.OnDeactivation((context, persistenceContext) => {
					Logger.Debug("Deactivating transaction...");

					var scope = context.GetScope() as HttpContext;

					if (scope == null) {
						return;
					}

					var transaction = (DbContextTransaction) scope.Items["__transaction__"];

					try {
						transaction.Commit();
					}
					catch (Exception e) {
						Logger.Error(e);

						transaction.Rollback();

						throw;
					}
					finally {
						transaction.Dispose();
					}
				});
		}
	}
}