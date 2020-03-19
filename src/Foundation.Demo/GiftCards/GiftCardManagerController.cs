using EPiServer.Shell.Navigation;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Mediachase.Commerce.Customers;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Demo.GiftCards
{
    public class GiftCardManagerController : Controller
    {
        private readonly IGiftCardService _giftCardService;

        public GiftCardManagerController(IGiftCardService giftCardService) => _giftCardService = giftCardService;

        [MenuItem("/global/foundation/giftcards", TextResourceKey = "/Shared/GiftCards", SortIndex = 300)]
        [HttpGet]
        public ActionResult Index() => View();

        [HttpGet]
        public ContentResult GetAllGiftCards()
        {
            var data = _giftCardService.GetAllGiftCards();
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AddGiftCard(GiftCard giftCard) => _giftCardService.CreateGiftCard(giftCard);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string UpdateGiftCard(GiftCard giftCard) => _giftCardService.UpdateGiftCard(giftCard);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteGiftCard(string giftCardId) => _giftCardService.DeleteGiftCard(giftCardId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult GetAllContacts()
        {
            var data = CustomerContext.Current.GetContacts(0, 1000).Select(c => new
            {
                ContactId = c.PrimaryKeyId.ToString(),
                ContactName = c.FullName
            });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }
    }
}