using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.Globalization;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.ServiceLocation;
using EPiServer.Tracking.Commerce;
using EPiServer.Tracking.Commerce.Data;
using EPiServer.Tracking.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using FluentAssertions;
using Foundation.Cms.Services;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Tests.Fakes;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace Foundation.Commerce.Personalization.Tests
{
    public class CommerceTrackingServiceTests
    {
        [Fact]
        public async Task TrackProduct_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackProduct(_httpContextBase.Object, "test", false);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackProduct_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateProductTrackingData(It.IsAny<string>(), It.IsAny<HttpContextBase>())).Returns(new ProductTrackingData("test", "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackProduct(_httpContextBase.Object, "test", true);
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackSearch_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackSearch(_httpContextBase.Object, null, 10, Enumerable.Empty<string>());
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackSearch_WhenSearchTermIsNull()
        {
            var result = await _subject.TrackSearch(_httpContextBase.Object, null, 10, Enumerable.Empty<string>());
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackSearch_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateSearchTrackingData(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<HttpContextBase>())).Returns(new SearchTrackingData("test", new[] { "test" }, 20, "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackSearch(_httpContextBase.Object, "test", 10, new[] { "test" });
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackOrder_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackOrder(_httpContextBase.Object, new FakePurchaseOrder());
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackOrder_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateOrderTrackingData(It.IsAny<IPurchaseOrder>(), It.IsAny<HttpContextBase>())).Returns(new OrderTrackingData(new List<CartItemData>(), "USD", 100, 10, 110, "po-32", "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackOrder(_httpContextBase.Object, new FakePurchaseOrder());
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackCategory_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackCategory(_httpContextBase.Object, null);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackCategory_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateCategoryTrackingData(It.IsAny<NodeContent>(), It.IsAny<HttpContextBase>())).Returns(new CategoryTrackingData("test", "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackCategory(_httpContextBase.Object, new NodeContent());
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackCart_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackCart(_httpContextBase.Object, new FakeCart());
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackCart_WhenSuccess()
        {
            var result = await _subject.TrackCart(_httpContextBase.Object, new FakeCart());
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackWishlist_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackWishlist(_httpContextBase.Object);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackWishlist_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateWishListTrackingData(It.IsAny<HttpContextBase>())).Returns(new WishListTrackingData(new List<ProductData>(), "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackWishlist(_httpContextBase.Object);
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackCheckout_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackCheckout(_httpContextBase.Object);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackCheckout_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateCheckoutTrackingData(It.IsAny<HttpContextBase>())).Returns(new CheckoutTrackingData(new List<CartItemData>(), "USD", 10, 10, 10, "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackCheckout(_httpContextBase.Object);
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackHome_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackHome(_httpContextBase.Object);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackHome_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateHomeTrackingData(It.IsAny<HttpContextBase>())).Returns(new HomeTrackingData("en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackHome(_httpContextBase.Object);
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackBrand_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackBrand(_httpContextBase.Object, "test");
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackBrand_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateBrandTrackingData(It.IsAny<string>(), It.IsAny<HttpContextBase>())).Returns(new BrandTrackingData("test", "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackBrand(_httpContextBase.Object, "test");
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackAttribute_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackAttribute(_httpContextBase.Object, "test", "test");
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackAttribute_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateAttributeTrackingData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContextBase>())).Returns(new AttributeTrackingData("test", "test", "en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackAttribute(_httpContextBase.Object, "test", "test");
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public async Task TrackDefault_WhenContextModeIsNotDefault()
        {
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Undefined);
            var result = await _subject.TrackDefault(_httpContextBase.Object);
            result.Should().BeNull();
        }

        [Fact]
        public async Task TrackDefault_WhenSuccess()
        {
            _trackingDataFactory.Setup(x => x.CreateOtherTrackingData(It.IsAny<HttpContextBase>())).Returns(new OtherTrackingData("en", new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            }, new CommerceUserData
            {
                Id = "id",
            }));

            var result = await _subject.TrackDefault(_httpContextBase.Object);
            _trackingService.Verify(x => x.Track(It.IsAny<TrackingData<CommerceTrackingData>>(), It.IsAny<HttpContextBase>()), Times.Once);
        }

        [Fact]
        public void GetRecommendedProductTileViewModels_WhenError()
        {
            _contentLoader.Setup(x => x.Get<EntryContentBase>(It.IsAny<ContentReference>(), It.IsAny<CultureInfo>()))
                .Throws(new Exception("test"));
            var result = _subject.GetRecommendedProductTileViewModels(new List<Recommendation>());
            result.Should().BeEquivalentTo(new List<RecommendedProductTileViewModel>());
        }

        [Fact]
        public void GetRecommendedProductTileViewModels_WhenSuccess()
        {
            _languageService.Setup(x => x.GetCurrentLanguage()).Returns(CultureInfo.GetCultureInfo("en"));
            _currentMarket.Setup(x => x.GetCurrentMarket()).Returns(new MarketImpl(MarketId.Default));
            _contentLoader.Setup(x => x.Get<EntryContentBase>(It.IsAny<ContentReference>(), It.IsAny<CultureInfo>()))
                .Returns(new ProductContent());
            _productService.Setup(x => x.GetProductTileViewModel(It.IsAny<EntryContentBase>())).Returns(new ProductTileViewModel());
            
            var result = _subject.GetRecommendedProductTileViewModels(new List<Recommendation>() { new Recommendation(1, new ContentReference(99))});
            result.Should().BeEquivalentTo(new List<RecommendedProductTileViewModel>()
            {
                new RecommendedProductTileViewModel(1, new ProductTileViewModel())
            });
        }

        public CommerceTrackingServiceTests()
        {
            _trackingService = new Mock<ITrackingService>();
            _httpContextBase = new Mock<HttpContextBase>();
            _contentRouteHelper = new Mock<IContentRouteHelper>();
            _contentLoader = new Mock<IContentLoader>();
            _currentMarket = new Mock<ICurrentMarket>();
            _productService = new Mock<IProductService>();

            _contextModeResolver = new Mock<IContextModeResolver>();
            _contextModeResolver.Setup(x => x.CurrentMode).Returns(ContextMode.Default);

            _requestTrackingDataService = new Mock<IRequestTrackingDataService>();
            _requestTrackingDataService.Setup(x => x.GetRequestData(It.IsAny<HttpContextBase>())).Returns(new RequestData
            {
                Url = "test",
                UrlReferrer = "referrer",
                UserAgent = "test",
                UserHostAddress = "test.com"
            });
            _requestTrackingDataService.Setup(x => x.GetUser(It.IsAny<HttpContextBase>())).Returns(new CommerceUserData
            {
                Id = "id",
            });

            _languageResolver = new Mock<LanguageResolver>();
            _languageResolver.Setup(x => x.GetPreferredCulture()).Returns(CultureInfo.GetCultureInfo("en"));

            _languageService = new Mock<LanguageService>(_currentMarket.Object, new Mock<ICookieService>().Object, new Mock<IUpdateCurrentLanguage>().Object);


            _referenceConverter = new Mock<ReferenceConverter>(new Mock<EntryIdentityResolver>(new Mock<ISynchronizedObjectInstanceCache>().Object).Object, 
                new Mock<NodeIdentityResolver>(new Mock<ISynchronizedObjectInstanceCache>().Object).Object);

            _trackingDataFactory = new Mock<TrackingDataFactory>(new Mock<ILineItemCalculator>().Object,
                _contentLoader.Object,
                new Mock<IOrderGroupCalculator>().Object,
                _languageResolver.Object,
                new Mock<IOrderRepository>().Object,
                new Mock<ReferenceConverter>(new Mock<EntryIdentityResolver>(new Mock<ISynchronizedObjectInstanceCache>().Object).Object, 
                    new Mock<NodeIdentityResolver>(new Mock<ISynchronizedObjectInstanceCache>().Object).Object).Object,
                new Mock<IRelationRepository>().Object,
                new Mock<IRecommendationContext>().Object,
                _currentMarket.Object,
                _requestTrackingDataService.Object,
                new Mock<ICartService>().Object);

            _subject = new CommerceTrackingService(new ServiceAccessor<IContentRouteHelper>(()=>_contentRouteHelper.Object),
                _contextModeResolver.Object,
                _trackingDataFactory.Object,
                _trackingService.Object,
                _contentLoader.Object,
                _languageService.Object,
                _httpContextBase.Object,
                _languageResolver.Object,
                new Mock<ILineItemCalculator>().Object,
                _requestTrackingDataService.Object,
                _referenceConverter.Object,
                new Mock<IRelationRepository>().Object,
                _currentMarket.Object, 
                _productService.Object);
        }

        private CommerceTrackingService _subject;
        private Mock<IContextModeResolver> _contextModeResolver;
        private Mock<TrackingDataFactory> _trackingDataFactory;
        private Mock<ITrackingService> _trackingService;
        private Mock<ReferenceConverter> _referenceConverter;
        private Mock<HttpContextBase> _httpContextBase;
        private Mock<IRequestTrackingDataService> _requestTrackingDataService;
        private Mock<LanguageResolver> _languageResolver;
        private Mock<IContentRouteHelper> _contentRouteHelper;
        private Mock<IContentLoader> _contentLoader;
        private Mock<ICurrentMarket> _currentMarket;
        private Mock<LanguageService> _languageService;
        private Mock<IProductService> _productService;
    }
}
