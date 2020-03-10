using System.Collections;
using EPiServer.Commerce.Order;

namespace Foundation.Commerce.Tests.Fakes
{
    public class FakeOrderAddress : IOrderAddress
    {
        public FakeOrderAddress()
        {
            Properties = new Hashtable();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string PostalCode { get; set; }

        public string RegionCode { get; set; }

        public string RegionName { get; set; }

        public string DaytimePhoneNumber { get; set; }

        public string EveningPhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Email { get; set; }

        public Hashtable Properties { get; private set; }

        public static FakeOrderAddress CreateOrderAddress(Hashtable properties = null)
        {
            return new FakeOrderAddress
            {
                City = "Springfield",
                CountryCode = "USA",
                CountryName = "",
                DaytimePhoneNumber = "",
                Email = "test@email.com",
                EveningPhoneNumber = "",
                FaxNumber = "",
                FirstName = "John",
                LastName = "Doe",
                Line1 = "4122 Any street",
                Line2 = "",
                Id = "JohnDoe",
                Organization = "",
                PostalCode = "22153",
                RegionCode = "VA",
                RegionName = "VA",
                State = "VA",
                Properties = properties ?? new Hashtable()
            };
        }
        public static FakeOrderAddress CreateOrderAddress(string name, string line1, string city, string postalCode, string region, string countryCode, Hashtable properties = null)
        {
            return new FakeOrderAddress
            {
                City = city,
                CountryCode = countryCode,
                CountryName = "",
                DaytimePhoneNumber = "",
                Email = "test@email.com",
                EveningPhoneNumber = "",
                FaxNumber = "",
                FirstName = "John",
                LastName = "Doe",
                Line1 = line1,
                Line2 = "",
                Id = name,
                Organization = "",
                PostalCode = postalCode,
                RegionCode = region,
                RegionName = region,
                State = region,
                Properties = properties ?? new Hashtable()
            };
        }
    }
}
