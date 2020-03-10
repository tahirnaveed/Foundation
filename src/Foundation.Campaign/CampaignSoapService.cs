using Foundation.Campaign.Connected_Services.CouponBlock;
using Foundation.Campaign.Connected_Services.CouponCode;
using Foundation.Campaign.Connected_Services.MailId;
using Foundation.Campaign.Connected_Services.MailingReporting;
using Foundation.Campaign.Connected_Services.MailingService;
using Foundation.Campaign.Connected_Services.RecipientList;
using Foundation.Campaign.Connected_Services.SessionService;

namespace Foundation.Campaign
{
    public class CampaignSoapService : ICampaignSoapService
    {
        public SessionWebserviceClient GetSessionWebserviceClient() => new SessionWebserviceClient();

        public MailingWebserviceClient GetMailingWebserviceClient() => new MailingWebserviceClient();

        public RecipientListWebserviceClient GetRecipientListWebserviceClient() => new RecipientListWebserviceClient();

        public MailIdWebserviceClient GetMailIdClient() => new MailIdWebserviceClient();

        public CouponBlockWebserviceClient GetCouponBlockWebserviceClient() => new CouponBlockWebserviceClient();

        public CouponCodeWebserviceClient GetCouponCodeWebserviceClient() => new CouponCodeWebserviceClient();

        public MailingReportingWebserviceClient GetMailingReportingWebserviceClient() => new MailingReportingWebserviceClient();
    }
}