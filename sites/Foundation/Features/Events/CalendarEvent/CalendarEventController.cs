using EPiServer.Web.Mvc;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        [HttpGet]
        public ActionResult Index(CalendarEventPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}