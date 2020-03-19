using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Foundation.Cms.Extensions
{
    public static class ContentReferenceExtensions
    {
        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IContentProviderManager> _providerManager =
            new Lazy<IContentProviderManager>(() => ServiceLocator.Current.GetInstance<IContentProviderManager>());

        private static readonly Lazy<IPageCriteriaQueryService> _pageCriteriaQueryService =
            new Lazy<IPageCriteriaQueryService>(() => ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>());

        public static IContent Get<TContent>(this ContentReference contentLink) where TContent : IContent => _contentLoader.Value.Get<TContent>(contentLink);

        public static IEnumerable<T> GetAllRecursively<T>(this ContentReference rootLink) where T : PageData
        {
            foreach (var child in _contentLoader.Value.GetChildren<T>(rootLink))
            {
                yield return child;

                foreach (var descendant in GetAllRecursively<T>(child.ContentLink))
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<PageData> FindPagesByPageType(this ContentReference pageLink, bool recursive, int pageTypeId)
        {
            if (ContentReference.IsNullOrEmpty(pageLink))
            {
                throw new ArgumentNullException(nameof(pageLink), "No page link specified, unable to find pages");
            }

            return recursive
                ? FindPagesByPageTypeRecursively(pageLink, pageTypeId)
                : _contentLoader.Value.GetChildren<PageData>(pageLink);
        }

        private static IEnumerable<PageData> FindPagesByPageTypeRecursively(ContentReference pageLink, int pageTypeId)
        {
            var criteria = new PropertyCriteriaCollection
            {
                new PropertyCriteria
                {
                    Name = "PageTypeID",
                    Type = PropertyDataType.PageType,
                    Condition = CompareCondition.Equal,
                    Value = pageTypeId.ToString(CultureInfo.InvariantCulture)
                }
            };

            if (!_providerManager.Value.ProviderMap.CustomProvidersExist)
            {
                return _pageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
            }

            var contentProvider = _providerManager.Value.ProviderMap.GetProvider(pageLink);
            if (contentProvider.HasCapability(ContentProviderCapabilities.Search))
            {
                criteria.Add(new PropertyCriteria
                {
                    Name = "EPI:MultipleSearch",
                    Value = contentProvider.ProviderKey
                });
            }

            return _pageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
        }
    }
}