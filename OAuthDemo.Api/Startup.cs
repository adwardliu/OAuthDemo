using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OAuthDemo.Api.Providers;
using OAuthDemo.Core.Infrastructure;
using Owin;

[assembly: OwinStartup(typeof(OAuthDemo.Api.Startup))]
namespace OAuthDemo.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);

            var builder = new ContainerBuilder();

            //webapi 支持依赖注入
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(configuration);
            builder.RegisterWebApiModelBinderProvider();

            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll").Select(Assembly.LoadFrom).ToArray();

            //注册所有实现了 IRepository 和 IApplicationService 接口的类型
            Type serviceType = typeof(IApplicationService);
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => serviceType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            Type repositoryType = typeof(IRepository);
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => repositoryType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //注册OAuth Provider
            builder.RegisterType<OpenAuthorizationServerProvider>();
            builder.RegisterType<OpenRefreshTokenProvider>();

            //注册DataSource
            builder.RegisterType<DefaultDataSource>().As<IDataSource>();
            
            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            IocManager.Container = container;

            ConfigureAuth(app);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(configuration);
            app.UseWebApi(configuration);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/oauth/token"), //获取 access_token 认证服务请求地址
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(2), //access_token 过期时间

                ApplicationCanDisplayErrors = true,

                Provider = IocManager.Container.Resolve<OpenAuthorizationServerProvider>(), //access_token 相关认证服务
                RefreshTokenProvider = IocManager.Container.Resolve<OpenRefreshTokenProvider>(), //refresh_token 认证服务
                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}