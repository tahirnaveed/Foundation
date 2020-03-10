using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeTaxValue : ITaxValue
    {
        public double Percentage { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public TaxType TaxType { get; set; }
    }
}
