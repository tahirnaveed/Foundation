using EPiServer.Commerce.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeShipmentBuilder
    {
        public FakeShipment Shipment { get; }

        public FakeShipmentBuilder()
        {
            Shipment = FakeShipment.CreateShipment();
            Shipment.ShippingAddress = FakeOrderAddress.CreateOrderAddress();
        }

        public FakeShipmentBuilder SetShippingMethod(Guid shippingMethodId)
        {
            Shipment.ShippingMethodId = shippingMethodId;
            return this;
        }

        public FakeShipmentBuilder SetShippingAddress(IOrderAddress shippingAddress = null)
        {
            Shipment.ShippingAddress = shippingAddress ?? FakeOrderAddress.CreateOrderAddress();
            return this;
        }

        public FakeShipmentBuilder AddItems(IEnumerable<ILineItem> items)
        {
            Shipment.LineItems = Shipment.LineItems.Concat(items).ToList();
            return this;
        }

        public FakeShipmentBuilder AddItem(ILineItem item)
        {
            Shipment.LineItems = Shipment.LineItems.Concat(new[] { item }).ToList();
            return this;
        }
    }
}
