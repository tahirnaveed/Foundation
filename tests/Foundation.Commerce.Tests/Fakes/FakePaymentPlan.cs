using Mediachase.Commerce.Orders;
using System;
using EPiServer.Commerce.Order;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakePaymentPlan : FakeOrderGroup, IPaymentPlan
    {
        public PaymentPlanCycle CycleMode { get; set; }
        public int CycleLength { get; set; }
        public int MaxCyclesCount { get; set; }
        public int CompletedCyclesCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastTransactionDate { get; set; }
        public bool IsActive { get; set; }
    }
}
