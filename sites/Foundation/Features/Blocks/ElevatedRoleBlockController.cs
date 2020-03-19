using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Models.Blocks;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.ViewModels;
using Mediachase.Commerce.Customers;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class ElevatedRoleBlockController : BlockController<ElevatedRoleBlock>
    {
        [HttpGet]
        public override ActionResult Index(ElevatedRoleBlock currentContent)
        {
            var viewModel = new ElevatedRoleBlockViewModel(currentContent);
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                if (contact.ElevatedRole == nameof(ElevatedRoles.Reader))
                {
                    viewModel.IsAccess = true;
                }
            }
            return PartialView("~/Features/Blocks/Views/ElevatedRoleBlock.cshtml", viewModel);
        }
    }
}
