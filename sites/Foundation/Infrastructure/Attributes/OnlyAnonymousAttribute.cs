using System.Web.Mvc;

namespace Foundation.Infrastructure.Attributes
{
    public class OnlyAnonymousAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }
    }
}