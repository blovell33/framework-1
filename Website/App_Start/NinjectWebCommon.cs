using System;
using System.Configuration;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Core;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Persistence;
using WebActivatorEx;
using Website;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace Website {
	public static class NinjectWebCommon {
		private static readonly Bootstrapper _bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start() {
			DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
			_bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop() {
			_bootstrapper.ShutDown();
		}

		/// <summary>
		/// Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel() {
			var kernel = new StandardKernel();
			try {
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

				RegisterServices(kernel);
				return kernel;
			}
			catch {
				kernel.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel) {
			kernel
				.BindFilter<SecureConnectionAttribute>(FilterScope.Global, 0);

			kernel
				.BindFilter<HandleErrorAttribute>(FilterScope.Global, 1);

			kernel
				.BindFilter<ApplicationSettingsAttribute>(FilterScope.Global, 2);

			kernel
				.BindFilter<MailSettingsAttribute>(FilterScope.Global, 3);

			kernel
				.Bind<ApplicationSettings>()
				.ToMethod(context => new ApplicationSettings(ConfigurationManager.AppSettings))
				.InRequestScope();

			kernel
				.Bind<MailSettings>()
				.ToMethod(context => {
					var configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
					var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

					return
						mailSettings == null
							? null
							: new MailSettings(mailSettings.Smtp);
				})
				.InRequestScope();

			kernel.Load<PersistenceModule>();
		}
	}
}