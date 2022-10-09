using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data.Repositories;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memo.App.Data.Repository;
using Memo.App.Common;
using Memo.App.Entities.BaseEntity;
using Memo.App.Data;
using Memo.App.Services.Idenitity;

namespace Memo.App.WebFramework.Configuration
{
    public static class AutofacConfigurationsExtensions
    {
        public static void AddAutofacConfiguration(this IHostBuilder host)
        {

            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // Register services directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory.
            host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
        }
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(ApplicationDbContext).Assembly;
            var servicesAssembly = typeof(JwtService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<IScopedDependency>().AsImplementedInterfaces().InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ITransientDependency>().AsImplementedInterfaces().InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ISingletonDependency>().AsImplementedInterfaces().SingleInstance();
        }
    }
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddServices();
        }
    }
}
