using EPiServer.Commerce.Order;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderFormBuilder
    {
        public FakeOrderForm OrderForm { get; }

        public FakeOrderFormBuilder()
        {
            OrderForm = FakeOrderForm.CreateOrderForm();
        }

        public FakeOrderFormBuilder AddShipment(IShipment shipment)
        {
            OrderForm.Shipments.Add(shipment);
            return this;
        }

        public FakeOrderFormBuilder AddShipments(IEnumerable<IShipment> shipments)
        {
            foreach (var shipment in shipments)
            {
                OrderForm.Shipments.Add(shipment);
            }
            return this;
        }
    }

    public class FakeReturnOrderFormBuilder
    {
        public FakeReturnOrderForm OrderForm { get; }

        public FakeReturnOrderFormBuilder()
        {
            OrderForm = FakeReturnOrderForm.CreateReturnOrderForm(new Hashtable());
        }

        public FakeReturnOrderFormBuilder AddShipment(IShipment shipment)
        {
            OrderForm.Shipments.Add(shipment);
            return this;
        }

        public FakeReturnOrderFormBuilder AddShipments(IEnumerable<IShipment> shipments)
        {
            foreach (var shipment in shipments)
            {
                OrderForm.Shipments.Add(shipment);
            }
            return this;
        }
    }
}
