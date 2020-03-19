using EPiServer.Cms.UI.AspNetIdentity;
using Foundation.Cms.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;

namespace Foundation.CommerceManager
{
#pragma warning disable CA5368 // Set ViewStateUserKey For Classes Derived From Page
    public partial class Logout : Page
#pragma warning restore CA5368 // Set ViewStateUserKey For Classes Derived From Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            Request.GetOwinContext().Get<ApplicationSignInManager<SiteUser>>().SignOut();
        }
    }
}
