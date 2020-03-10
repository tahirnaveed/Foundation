using EPiServer.ConnectForCampaign.Core.Configuration;
using EPiServer.ConnectForCampaign.Core.Implementation;
using EPiServer.ConnectForCampaign.Services;
using EPiServer.ConnectForCampaign.Services.Implementation;
using Foundation.Demo.Campaign;
using Moq;

namespace Foundation.Demo.Tests.Campaign
{
    public class ExtendedRecipientListServiceTests
    {
        [Fact]
        public void GetRecommendedProductTileViewModels_WhenSuccess()
        {
            var result = _subject.i(new List<Recommendation>() { new Recommendation(1, new ContentReference(99)) });
            result.Should().BeEquivalentTo(new List<RecommendedProductTileViewModel>()
            {
                new RecommendedProductTileViewModel(1, new ProductTileViewModel())
            });
        }

        public ExtendedRecipientListServiceTests()
        {
            _subject = new ExtendedRecipientListService(new Mock<IServiceClientFactory>().Object,
                new Mock<IAuthenticationService>().Object,
                new Mock<ICacheService>().Object,
                new Mock<ICampaignSettings>().Object);
        }

        private ExtendedRecipientListService _subject;
    }
}
