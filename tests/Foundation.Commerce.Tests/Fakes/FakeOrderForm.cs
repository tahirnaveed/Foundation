using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using Mediachase.Commerce;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderForm : IOrderForm
    {
        private readonly int _orderFormId;
        private static int _counter;
        private readonly IList<PromotionInformation> _promotions = new List<PromotionInformation>();
        private readonly ICollection<string> _couponCodes = new List<string>();
        private FakeOrderGroup _parentOrderGroup;

        public FakeOrderForm()
        {
            Shipments = new List<IShipment>();
            _orderFormId = ++_counter;
            Properties = new Hashtable();
            Payments = new List<IPayment>();
        }

        public string Status { get; set; }

        public decimal AuthorizedPaymentTotal { get; set; }

        public decimal CapturedPaymentTotal { get; set; }

        public decimal HandlingTotal { get; set; }
        public string Name { get; set; }

        public ICollection<IShipment> Shipments { get; set; }

        public int OrderFormId
        {
            get { return _orderFormId; }
        }

        public MarketId MarketId { get; set; }

        public IMarket Market
        {
            get { return new MarketImpl(new MarketId("US")); }
        }

        public IList<PromotionInformation> Promotions
        {
            get { return _promotions; }
        }

        public ICollection<string> CouponCodes
        {
            get { return _couponCodes; }
        }

        public ICollection<IPayment> Payments { get; set; }

        public Hashtable Properties { get; protected set; }

        public static FakeOrderForm CreateOrderForm(Hashtable properties = null)
        {
            return new FakeOrderForm
            {
                AuthorizedPaymentTotal = 0,
                CapturedPaymentTotal = 0,
                HandlingTotal = 10,
                Status = "Word",
                MarketId = MarketId.Default,
                Properties = properties ?? new Hashtable()
            };
        }

        public bool PricesIncludeTax => ParentOrderGroup?.PricesIncludeTax ?? false;

        public IOrderGroup ParentOrderGroup
        {
            get => _parentOrderGroup;
            set => _parentOrderGroup = (FakeOrderGroup)value;
        }

        internal void SetParentOrderGroup(FakeOrderGroup orderGroup)
        {
            _parentOrderGroup = orderGroup;
            foreach (FakeShipment shipment in Shipments)
            {
                shipment.SetParentOrderGroup(orderGroup);
            }
        }
    }

    public class FakeReturnOrderForm : FakeOrderForm, IReturnOrderForm
    {
        public int? OriginalOrderFormId { get; set; }

        public int? ExchangeOrderGroupId { get; set; }

        public string ReturnAuthCode { get; set; }

        public string RMANumber { get; set; }

        public string ReturnType { get; set; }

        public string ReturnComment { get; set; }

        ReturnFormStatus IReturnOrderForm.Status { get; set; }

        public static IReturnOrderForm CreateReturnOrderForm()
        {
            var returnForm = new FakeReturnOrderForm
            {
                Name = OrderForm.ReturnName,
                ReturnType = "Refund"
            };

            ((IReturnOrderForm)returnForm).Status = ReturnFormStatus.AwaitingStockReturn;

            return returnForm;
        }

        public static IReturnOrderForm CreateReturnOrderForm(IOrderForm orderForm)
        {
            var returnForm = new FakeReturnOrderForm
            {
                Name = OrderForm.ReturnName,
                ReturnType = "Refund",
                OriginalOrderFormId = orderForm.OrderFormId
            };

            ((IReturnOrderForm)returnForm).Status = ReturnFormStatus.AwaitingStockReturn;

            return returnForm;
        }

        public static FakeReturnOrderForm CreateReturnOrderForm(Hashtable properties = null)
        {
            return new FakeReturnOrderForm
            {
                AuthorizedPaymentTotal = 0,
                CapturedPaymentTotal = 0,
                HandlingTotal = 10,
                Status = "Word",
                MarketId = MarketId.Default,
                Properties = properties ?? new Hashtable()
            };
        }
    }
}