using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatApp.Lib.Azure.Storage;
using ChatApp.Lib.General;
using ChatApp.Lib.Messaging.Persistence;
using ChatApp.Lib.Users.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChatApp.Configuration
{
    public static class DependencyConfiguration
    {
        public static IServiceProvider ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            //Active user repository
            var activeUserRepositoryOptions = new TableStorageClientOptions();
            configuration.GetSection("AzureStorageActiveUserRepositoryOptions").Bind(activeUserRepositoryOptions);

            builder.RegisterType<TableStorageClient>()
                .WithParameter("options", activeUserRepositoryOptions)
                .Named<ITableStorageClient>("activeUsers")
                .As<IInitializable>();

            builder.Register((c, p) => new AzureStorageActiveUserRepository(c.ResolveNamed<ITableStorageClient>("activeUsers")))
                .As<IActiveUserRepository>()
                .As<IInitializable>();

            //Chat message repository
            var chatMessageRepositoryOptions = new TableStorageClientOptions();
            configuration.GetSection("AzureStorageChatMessageRepositoryOptions").Bind(chatMessageRepositoryOptions);

            builder.RegisterType<TableStorageClient>()
                .WithParameter("options", chatMessageRepositoryOptions)
                .Named<ITableStorageClient>("chatMessages")
                .As<IInitializable>();

            builder.Register((c, p) => new AzureStorageChatMessageRepository(c.ResolveNamed<ITableStorageClient>("chatMessages")))
                .As<IChatMessageRepository>();

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
