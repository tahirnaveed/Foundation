using EPiServer.SpecializedProperties;
using FluentAssertions;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Moq;
using Xunit;

namespace Foundation.Commerce.Tests.Customer.Services
{
    public class B2BNavigationServiceTests
    {

        [Fact]
        public void FilterB2BNavigationForCurrentUser_WhenNotB2BUser()
        {
            _contact.UserRole = "None";
            _customerService.Setup(x => x.GetCurrentContact()).Returns(_contact);
            var result = _subject.FilterB2BNavigationForCurrentUser(_linkItems);
            result.Should().HaveCount(0);
        }

        [Fact]
        public void FilterB2BNavigationForCurrentUser_WhenAdmin()
        {
            _contact.UserRole = "Admin";
            _customerService.Setup(x => x.GetCurrentContact()).Returns(_contact);
            var result = _subject.FilterB2BNavigationForCurrentUser(_linkItems);
            result.Should().HaveCount(6);
        }

        [Fact]
        public void FilterB2BNavigationForCurrentUser_WhenApprover()
        {
            _contact.UserRole = "Approver";
            _customerService.Setup(x => x.GetCurrentContact()).Returns(_contact);
            var result = _subject.FilterB2BNavigationForCurrentUser(_linkItems);
            result.Should().HaveCount(4);
        }

        public B2BNavigationServiceTests()
        {
            _customerService = new Mock<ICustomerService>();
            _contact = FoundationContact.New();
            _linkItems = new LinkItemCollection()
            {
                 new LinkItem { Text = "Overview" },
                 new LinkItem { Text = "Users" },
                 new LinkItem { Text = "Orders" },
                 new LinkItem { Text = "Order Pad" },
                 new LinkItem { Text = "Budgeting" },
                 new LinkItem { Text = "B2B Credit Card" }
            };
            _subject = new B2BNavigationService(_customerService.Object);
        }

        private B2BNavigationService _subject;
        private Mock<ICustomerService> _customerService;
        private FoundationContact _contact;
        private LinkItemCollection _linkItems;
    }
}
