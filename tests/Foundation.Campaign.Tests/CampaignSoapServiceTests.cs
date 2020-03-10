using FluentAssertions;
using Xunit;

namespace Foundation.Campaign.Tests
{
    public class CampaignSoapServiceTests
    {
        [Fact]
        public void GetSessionWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetSessionWebserviceClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetMailingWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetMailingWebserviceClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetRecipientListWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetRecipientListWebserviceClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetMailIdClient_WhenSUccess()
        {
            var result = _subject.GetMailIdClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetCouponBlockWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetCouponBlockWebserviceClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetCouponCodeWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetCouponCodeWebserviceClient();
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetMailingReportingWebserviceClient_WhenSUccess()
        {
            var result = _subject.GetMailingReportingWebserviceClient();
            result.Should().NotBeNull();
        }

        public CampaignSoapServiceTests()
        {
            _subject = new CampaignSoapService();
        }

        private CampaignSoapService _subject;
    }
}
