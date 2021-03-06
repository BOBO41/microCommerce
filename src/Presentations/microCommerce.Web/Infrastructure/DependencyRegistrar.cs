﻿using Autofac;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Localization;
using microCommerce.Logging;
using microCommerce.Module.Core;
using microCommerce.MongoDb;
using microCommerce.Mvc;
using microCommerce.Mvc.Http;
using microCommerce.Mvc.Infrastructure;
using microCommerce.Mvc.Themes;
using microCommerce.Redis;
using microCommerce.Setting;

namespace microCommerce.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(DependencyContext context)
        {
            var builder = context.ContainerBuilder;
            var config = context.AppConfig;

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterInstance(new MongoDbContext(config.NoSqlConnectionString)).As<IMongoDbContext>().SingleInstance();
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerLifetimeScope();

            if (config.CachingEnabled)
            {
                //cache manager
                builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

                //static cache manager
                if (config.UseRedisCaching)
                {
                    builder.RegisterInstance(new RedisConnectionWrapper(config.RedisConnectionString)).As<IRedisConnectionWrapper>().SingleInstance();
                    builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
                }
                else
                    builder.RegisterType<MemoryCacheManager>().As<IStaticCacheManager>().SingleInstance();
            }
            else
                builder.RegisterType<NullCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            if (config.LoggingEnabled)
            {
                if (config.UseNoSqlLogging)
                {
                    builder.RegisterType<MongoDbLogger>().As<ILogger>().InstancePerLifetimeScope();
                }
                else
                {
                    builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
                }
            }
            else
            {
                builder.RegisterType<NullLogger>().As<ILogger>().InstancePerLifetimeScope();
            }

            //module features
            builder.RegisterType<ModuleProvider>().As<IModuleProvider>().InstancePerLifetimeScope();

            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();

            //circuit breaker http client support
            builder.RegisterType<StandardHttpClient>().As<IHttpClient>().InstancePerLifetimeScope();

            builder.RegisterInstance(new StoreSettings { DefaultTheme = "Default" });

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }

        public int Priority
        {
            get { return 1; }
        }
    }
}