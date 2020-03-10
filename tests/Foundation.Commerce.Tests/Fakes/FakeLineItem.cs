using EPiServer.Commerce.Order;
using EPiServer.Commerce.Order.Internal;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Orders;
using System.Collections;
using System.Linq;

namespace Foundation.Commerce.Tests.Fakes
{
#pragma warning disable 618
    public class FakeLineItem : ILineItem, ILineItemCalculatedAmount, ILineItemDiscountAmount, ILineItemInventory
#pragma warning restore 618
    {
        private static int _counter;
        private decimal _quantity;
        private decimal _placedPrice;
        private FakeOrderGroup _parentOrderGroup;


        public FakeLineItem()
        {
            LineItemId = ++_counter;
            Properties = new Hashtable();
        }

        /// <summary>
        /// Gets or sets the parent fake order form.
        /// </summary>
        /// <value>The parent.</value>
        public FakeOrderForm Parent { get; set; }

        public int LineItemId { get; set; }

        public string Code { get; set; }

        public decimal Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    ResetUpToDateFlags();
                }
            }
        }

        public decimal ReturnQuantity { get; set; }
        public InventoryTrackingStatus InventoryTrackingStatus { get; set; }
        public bool IsInventoryAllocated { get; set; }

        public decimal LineItemDiscountAmount { get; set; }

        public decimal PlacedPrice
        {
            get
            {
                return _placedPrice;
            }
            set
            {
                if (_placedPrice != value)
                {
                    _placedPrice = value;
                    ResetUpToDateFlags();
                }
            }
        }

        public decimal OrderLevelDiscountAmount { get; set; }

        public string DisplayName { get; set; }

        public bool IsGift { get; set; }

        public bool AllowBackordersAndPreorders { get; set; }
        public decimal InStockQuantity { get; set; }
        public decimal BackorderQuantity { get; set; }
        public decimal PreorderQuantity { get; set; }
        public int InventoryStatus { get; set; }
        public decimal MaxQuantity { get; set; }
        public decimal MinQuantity { get; set; }

        public static FakeLineItem CreateLineItem(string code, decimal price, decimal quantity, decimal lineItemDiscount = 0, bool isGift = false)
        {
            return CreateLineItem(++_counter, code, price, quantity, lineItemDiscount, isGift: isGift);
        }

        public static FakeLineItem CreateLineItem(int id, string code, decimal price, decimal quantity, decimal lineItemDiscount = 0, decimal orderLevelDiscount = 0,
            bool isGift = false, Hashtable properties = null, decimal returnQuantity = 0, string displayName = null)
        {

            var fakeLineItem = new FakeLineItem
            {
                Code = code,
                LineItemDiscountAmount = lineItemDiscount,
                OrderLevelDiscountAmount = orderLevelDiscount,
                LineItemId = id,
                PlacedPrice = price,
                Quantity = quantity,
                IsGift = isGift,
                Properties = properties ?? new Hashtable(),
                ReturnQuantity = returnQuantity,
                DisplayName = displayName
            };

            fakeLineItem.SetEntryDiscountValue(fakeLineItem.LineItemDiscountAmount);
            fakeLineItem.SetOrderDiscountValue(fakeLineItem.OrderLevelDiscountAmount);

            return fakeLineItem;
        }

        public Hashtable Properties { get; protected set; }

        decimal ILineItemDiscountAmount.EntryAmount
        {
            get
            {
                return LineItemDiscountAmount;
            }
            set
            {
                if (LineItemDiscountAmount != value)
                {
                    LineItemDiscountAmount = value;
                    ResetUpToDateFlags();
                }
            }
        }

        /// <summary>
        /// Gets or sets the order level discount amount.
        /// </summary>
        /// <value>The order level discount amount.</value>
        decimal ILineItemDiscountAmount.OrderAmount
        {
            get
            {
                return OrderLevelDiscountAmount;
            }
            set
            {
                if (OrderLevelDiscountAmount != value)
                {
                    OrderLevelDiscountAmount = value;
                    // Changing the line item order amount does not affect calculated shipping tax of parent shipment.
                    ResetUpToDateFlags(false);
                }
            }
        }

        public int? TaxCategoryId { get; set; }

        internal void SetParentOrderGroup(FakeOrderGroup orderGroup)
        {
            _parentOrderGroup = orderGroup;
        }

        bool ILineItemCalculatedAmount.IsSalesTaxUpToDate { get; set; }

        decimal ILineItemCalculatedAmount.SalesTax { get; set; }

        private void ResetUpToDateFlags(bool includedShipmentShippingTax = true)
        {
            ((ILineItemCalculatedAmount)this).IsSalesTaxUpToDate = false;

            var orderGroupCalculatedAmount = ParentOrderGroup as IOrderGroupCalculatedAmount;
            var shipmentCalculatedPrices = this.Parent?.Shipments.FirstOrDefault(s => s.LineItems.Any(l => l.LineItemId == this.LineItemId)) as IShipmentCalculatedAmount;

            if (orderGroupCalculatedAmount != null)
            {
                orderGroupCalculatedAmount.IsTaxTotalUpToDate = false;
            }

            if (shipmentCalculatedPrices != null && includedShipmentShippingTax)
            {
                shipmentCalculatedPrices.IsShippingTaxUpToDate = false;
            }
        }

        public bool PricesIncludeTax { get; set; }
        public string LanguageCode { get; set; }

        public IOrderGroup ParentOrderGroup
        {
            get => _parentOrderGroup;
            set => _parentOrderGroup = (FakeOrderGroup)value;
        }
    }

    public class FakeReturnLineItem : FakeLineItem, IReturnLineItem
    {
        public int? OriginalLineItemId { get; set; }

        public string ReturnReason { get; set; }

        public static FakeReturnLineItem CreateReturnLineItem(int id, string code, decimal price, decimal quantity, decimal lineItemDiscount = 0, decimal orderLevelDiscount = 0,
            bool isGift = false, Hashtable properties = null, decimal returnQuantity = 0, string displayName = null)
        {

            var fakeLineItem = new FakeReturnLineItem()
            {
                Code = code,
                LineItemDiscountAmount = lineItemDiscount,
                OrderLevelDiscountAmount = orderLevelDiscount,
                LineItemId = id,
                PlacedPrice = price,
                Quantity = quantity,
                IsGift = isGift,
                Properties = properties ?? new Hashtable(),
                ReturnQuantity = returnQuantity,
                DisplayName = displayName
            };

            fakeLineItem.SetEntryDiscountValue(fakeLineItem.LineItemDiscountAmount);
            fakeLineItem.SetOrderDiscountValue(fakeLineItem.OrderLevelDiscountAmount);

            return fakeLineItem;
        }
    }
}