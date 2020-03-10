using EPiServer.Commerce.Order;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderGroupBuilder
    {
        private static int counter;
        private readonly Guid _customerId = Guid.NewGuid();
        public FakeOrderGroup OrderGroup { get; private set; }

        public FakeOrderGroupBuilder(FakeOrderGroup orderGroup)
        {
            OrderGroup = orderGroup;
        }

        public FakeOrderGroupBuilder()
        {
            OrderGroup = new FakeOrderGroup
            {
                Currency = new Currency(Currency.USD),
                OrderLink = new OrderReference(++counter, "Default", _customerId, typeof(Cart))
            };
        }

        public FakeOrderGroupBuilder AddOrderForm(IOrderForm form)
        {
            OrderGroup.Forms.Add(form);
            return this;
        }

        public FakeOrderGroupBuilder AddOrderForms(IEnumerable<IOrderForm> forms)
        {
            foreach (var form in forms)
            {
                OrderGroup.Forms.Add(form);
            }
            return this;
        }

        public FakeOrderGroupBuilder SetCurrency(Currency currency)
        {
            OrderGroup.Currency = currency;
            return this;
        }

        public FakeOrderGroupBuilder SetMarket(IMarket market)
        {
            OrderGroup.MarketId = market.MarketId;
            OrderGroup.MarketName = market.MarketName;
            OrderGroup.PricesIncludeTax = market.PricesIncludeTax;
            return this;
        }

        public FakeOrderGroupBuilder AddShipments(IEnumerable<IShipment> shipments)
        {
            AddOrderForm(new FakeOrderFormBuilder().AddShipments(shipments).OrderForm);
            return this;
        }

        public FakeOrderGroupBuilder AddLineItems(params ILineItem[] lineItems)
        {
            var orderFormBuilder = new FakeOrderFormBuilder();
            var shipmentBuilder = new FakeShipmentBuilder();
            shipmentBuilder.AddItems(lineItems);
            orderFormBuilder.AddShipment(shipmentBuilder.Shipment);
            AddOrderForm(orderFormBuilder.OrderForm);
            return this;
        }

        public void Clear()
        {
            OrderGroup.Forms = new List<IOrderForm>();
        }
    }
}
