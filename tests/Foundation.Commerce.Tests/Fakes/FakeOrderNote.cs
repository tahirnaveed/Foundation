using EPiServer.Commerce.Order;
using System;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderNote : IOrderNote
    {
        public DateTime Created { get; set; }

        public Guid CustomerId { get; set; }

        public string Detail { get; set; }

        public int? LineItemId { get; set; }

        public int? OrderNoteId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }
}
