using DbLocalizationProvider;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Hosting;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Extensions;
using Foundation.Demo.Extensions;
using Foundation.Find.Cms;
using Foundation.Infrastructure.Services;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Owin;

namespace Foundation.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule), typeof(Cms.Initialize), typeof(EPiServer.ServiceApi.IntegrationInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {

            context.ConfigureFoundationCms();

            context.Services.Configure<ContentApiConfiguration>(config =>
            {
                config.Default()
                    .SetMinimumRoles(string.Empty);
            });

            context.Services.Configure<ContentApiSearchConfiguration>(config =>
            {
                config.Default()
                .SetMaximumSearchResults(200)
                .SetSearchCacheDuration(TimeSpan.FromMinutes(60));
            });

            context.Services.AddSingleton<ICampaignService, CampaignService>();
        }

        public void Initialize(InitializationEngine context)
        {
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            context.InitializeFoundationCms();
            context.InitializeFoundationCommerce();
            context.InitializeFoundationFindCms();
            context.InitializeFoundationDemo();


            var handler = GlobalConfiguration.Configuration.MessageHandlers
                .FirstOrDefault(x => x.GetType() == typeof(PassiveAuthenticationMessageHandler));

            if (handler != null)
            {
                GlobalConfiguration.Configuration.MessageHandlers.Remove(handler);
            }

            var host = context.Locate.Advanced.GetInstance<IHostingEnvironment>();
            if (host == null)
                return;

            var virtualPathMappedProvider = new VirtualPathMappedProvider("InsightUIUpdates", new NameValueCollection());
            virtualPathMappedProvider.PathMappings.Add("/episerver/EPiServer.Insight.UI/Views/Shared/MABootstrapper.aspx", "~/Views/Shared/MABootstrapper.aspx");
            virtualPathMappedProvider.PathMappings.Add("/episerver/EPiServer.Find.UI/Views/Shared/FindBootstrapper.aspx", "~/Views/Shared/FindBootstrapper.aspx");
            host.RegisterVirtualPathProvider(virtualPathMappedProvider);

            ConfigurationContext.Setup(ctx => ctx.Connection = "EpiserverDB");
        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}