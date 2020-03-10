using EPiServer.Commerce.Order;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakePurchaseOrder : FakeOrderGroup, IPurchaseOrder
    {
        public FakePurchaseOrder()
        {
            ReturnForms = new List<IReturnOrderForm>();
        }

        public string OrderNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public ICollection<IReturnOrderForm> ReturnForms { get; private set; }
    }
}
