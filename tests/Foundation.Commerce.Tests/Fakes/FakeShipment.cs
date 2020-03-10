using Mediachase.Commerce.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.Order.Internal;
using System.Linq;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeShipment : IShipment, IShipmentDiscountAmount, IShipmentCalculatedAmount
    {
        private Guid _shippingMethodId;
        private IOrderAddress _shippingAddress;
        private decimal _shipmentDiscount;
        private static int _counter;
        private FakeOrderGroup _parentOrderGroup;

        public FakeShipment()
        {
            LineItems = new List<ILineItem>();
            ShipmentId = ++_counter;
            Properties = new Hashtable();
        }

        public int ShipmentId { get; set; }

        public Guid ShippingMethodId
        {
            get
            {
                return _shippingMethodId;
            }
            set
            {
                _shippingMethodId = value;
                ResetUpToDateFlags();
            }
        }

        public string ShippingMethodName { get; set; }

        public IOrderAddress ShippingAddress
        {
            get
            {
                return _shippingAddress;
            }
            set
            {
                _shippingAddress = value;
                ResetUpToDateFlags(true);
            }
        }

        public string ShipmentTrackingNumber { get; set; }

        public OrderShipmentStatus OrderShipmentStatus { get; set; }

        public int? PickListId { get; set; }

        public string WarehouseCode { get; set; }

        public ICollection<ILineItem> LineItems { get; set; }

        public decimal ShipmentDiscount
        {
            get
            {
                return _shipmentDiscount;
            }
            set
            {
                _shipmentDiscount = value;
                ResetUpToDateFlags();
            }
        }

        public Hashtable Properties { get; private set; }

        decimal IShipmentCalculatedAmount.ShippingCost { get; set; }

        decimal IShipmentCalculatedAmount.ShippingTax { get; set; }

        bool IShipmentCalculatedAmount.IsShippingCostUpToDate { get; set; }

        bool IShipmentCalculatedAmount.IsShippingTaxUpToDate { get; set; }

        public FakeOrderForm Parent { get; set; }

        public IOrderGroup ParentOrderGroup
        {
            get => _parentOrderGroup;
            set => _parentOrderGroup = (FakeOrderGroup)value;
        }

        public static FakeShipment CreateShipment(int id, IOrderAddress orderAddress, decimal discount, IList<ILineItem> items, string shippingMethodIdString = null, Hashtable properties = null)
        {
            return new FakeShipment
            {
                ShipmentId = id,
                ShippingAddress = orderAddress,
                ShippingMethodId = new Guid(shippingMethodIdString ?? "7eedee57-c8f4-4d19-a58c-284e72094527"),
                ShipmentDiscount = discount,
                WarehouseCode = "default",
                LineItems = items,
                Properties = properties ?? new Hashtable()

            };
        }

        public static FakeShipment CreateShipment(int id, Guid shippingMethodId, IOrderAddress shippingAddress, IList<ILineItem> items = null, Hashtable properties = null)
        {
            return new FakeShipment
            {
                ShipmentId = id,
                ShippingAddress = shippingAddress,
                ShippingMethodId = shippingMethodId,
                ShipmentDiscount = 0,
                WarehouseCode = "default",
                LineItems = items ?? new List<ILineItem>(),
                Properties = properties ?? new Hashtable()
            };
        }

        public static FakeShipment CreateShipment()
        {
            return new FakeShipment
            {
                ShipmentId = 1,
                ShipmentDiscount = 0,
                WarehouseCode = "default",
                OrderShipmentStatus = OrderShipmentStatus.AwaitingInventory
            };
        }

        internal void SetParentOrderGroup(FakeOrderGroup orderGroup)
        {
            _parentOrderGroup = orderGroup;
            foreach (FakeLineItem item in LineItems)
            {
                item.SetParentOrderGroup(orderGroup);
            }
        }

        private void ResetUpToDateFlags(bool includedLineItemSalesTax = false)
        {
            ((IShipmentCalculatedAmount)this).IsShippingCostUpToDate = false;
            ((IShipmentCalculatedAmount)this).IsShippingTaxUpToDate = false;

            var orderGroupCalculatedAmount = ParentOrderGroup as IOrderGroupCalculatedAmount;
            if (orderGroupCalculatedAmount != null)
            {
                orderGroupCalculatedAmount.IsTaxTotalUpToDate = false;
            }

            if (includedLineItemSalesTax)
            {
                foreach (var lineItem in LineItems.OfType<ILineItemCalculatedAmount>())
                {
                    lineItem.IsSalesTaxUpToDate = false;
                }
            }
        }
    }
}
