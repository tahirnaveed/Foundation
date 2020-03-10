using EPiServer.Commerce.Order;
using EPiServer.Commerce.Order.Internal;
using Mediachase.Commerce;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderGroup : ICart, IOrderGroupCalculatedAmount
    {
        private Guid _customerId = Guid.NewGuid();
        private static int _counter;
        private MarketId _marketId;
        private Currency _currency;

        public FakeOrderGroup()
        {
            Forms = new List<IOrderForm>();
            var market = new MarketImpl(MarketId.Default);
            MarketId = market.MarketId;
            MarketName = market.MarketName;
            PricesIncludeTax = market.PricesIncludeTax;

            OrderLink = new OrderReference(++_counter, "Default", _customerId, typeof(Cart));
            Properties = new Hashtable();
            Notes = new List<IOrderNote>();
            OrderStatus = OrderStatus.InProgress;
            Currency = Currency.USD;
        }

        public OrderReference OrderLink { get; set; }

        public ICollection<IOrderForm> Forms { get; set; }

        [Obsolete("This property is no longer used. Use IMarketService to get the market from MarketId instead. Will remain at least until May 2019.")]
        public IMarket Market { get; set; }

        public MarketId MarketId
        {
            get { return _marketId; }
            set
            {
                _marketId = value;
                ResetUpToDateFlags();
            }
        }

        public string MarketName { get; set; }

        public bool PricesIncludeTax { get; set; }

        public ICollection<IOrderNote> Notes { get; }

        public string Name { get; set; }

        public Guid? Organization { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Hashtable Properties { get; private set; }

        decimal IOrderGroupCalculatedAmount.TaxTotal { get; set; }

        bool IOrderGroupCalculatedAmount.IsTaxTotalUpToDate { get; set; }

        public static FakeOrderGroup CreateOrderGroup(ICollection<FakeOrderForm> forms, Hashtable properties = null)
        {
            var market = new MarketImpl(MarketId.Default);
            return new FakeOrderGroup
            {
                MarketId = market.MarketId,
                MarketName = market.MarketName,
                PricesIncludeTax = market.PricesIncludeTax,
                Forms = forms.Cast<IOrderForm>().ToList(),
                Properties = properties ?? new Hashtable()
            };
        }

        public Currency Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                ResetUpToDateFlags();
            }
        }

        public Guid CustomerId
        {
            get
            {
                return _customerId;
            }
            set
            {
                _customerId = value;
            }
        }

        public DateTime Created => DateTime.MaxValue;

        public DateTime? Modified => null;
        private void ResetUpToDateFlags()
        {
            ((IOrderGroupCalculatedAmount)this).IsTaxTotalUpToDate = false;

            if (Forms == null)
            {
                return;
            }

            foreach (var shipmentCalculatedPrice in Forms.SelectMany(x => x.Shipments.OfType<IShipmentCalculatedAmount>()))
            {
                shipmentCalculatedPrice.ResetUpToDateFlags();
            }
        }
    }
}
