<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<EPiServer.Insight.UI.Models.MABootstrapperViewModel>" MasterPageFile="MA.Master" %>
<%@ Assembly Name="EPiServer.Shell.UI" %>
<%@ Assembly Name="EPiServer.Insight.UI" %>
<%@ Import Namespace="EPiServer.Framework.Serialization" %>
<%@ Import Namespace="EPiServer.Framework.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer.Framework.Web.Resources" %>
<%@ Import Namespace="EPiServer.Insight.UI" %>
<%@ Import Namespace="EPiServer.ServiceLocation" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Shell.Navigation" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer.Shell.Navigation.Internal" %>


<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%: Model.ViewTitle %></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
    <%=Page.ClientResources("navigation", new[] { ClientResourceType.Style })%>
    <link rel="stylesheet" type="text/css" href="<%=HttpUtility.HtmlEncode(Model.ClientSideResourceBaseUrl)%>epi/themes/sleek/document.css" />
    <link rel="stylesheet" type="text/css" href="<%=HttpUtility.HtmlEncode(Model.ClientSideResourceBaseUrl)%>epi/themes/sleek/sleek.css" />
    <link rel="stylesheet" type="text/css" href="<%=HttpUtility.HtmlEncode(Model.ClientSideResourceBaseUrl)%>epi-profiles/epi.css" />
    <%= Html.RequiredClientResources("BootstrapperHeader") %>
    
    <style>
        header {
            z-index: 999 !important;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
     <script type="text/javascript">
            //<![CDATA[
            var dojoConfig = {
                baseUrl: "<%=HttpUtility.JavaScriptStringEncode(new Uri(Model.ClientSideResourceBaseUrl).AbsoluteUri)%>",
            locale: "<%=HttpUtility.JavaScriptStringEncode(Model.LocaleString)%>",
            paths: {
                "epi/shell/ui/nls": "<%=HttpUtility.JavaScriptStringEncode(new Uri(Model.ClientSideResourceBaseUrl).AbsoluteUri)%>epi/nls",
                "epi-cms/nls": "<%=HttpUtility.JavaScriptStringEncode(new Uri(Model.ClientSideResourceBaseUrl).AbsoluteUri)%>epi/nls",
                "dependencies": "<%=HttpUtility.JavaScriptStringEncode(new Uri(Model.ClientSideResourceBaseUrl).AbsoluteUri)%>dependencies"
            },
            packages: ["dojo", "epi-profiles"],
            insight: {
                serviceApiBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.ServiceApiBaseUrl) %>",
                siteServiceApiBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteServiceApiBaseUrl) %>",
                siteModulesBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteModulesBaseUrl) %>",
                siteNoConnectionUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteNoConnectionUrl) %>",
                init: {
                    username: '<%= HttpContext.Current.User.Identity.Name %>'
                }
            },
            rootNode: document.getElementById('applicationContainer')
        };
        //]]>
    </script>
   <%=Page.ClientResources("epi.profiles.bootstrap", new[] { ClientResourceType.Script })%>
    <script type="text/javascript">
        //<![CDATA[
        require(["epi-profiles-bootstrap", "dojo/parser", "dojo/domReady!"], function (Bootstrap, parser) {
            var bootstrap = new Bootstrap(document.getElementById('applicationContainer'));
            bootstrap.startup();
            parser.parse();
        });
        //]]>
    </script>
   
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.CreatePlatformNavigationMenu()%>
    <div style="display: block; height: calc(100vh - 36px); padding-top: 36px;" >
        <div id="applicationContainer" class="epi-applicationContainer" style="height: calc(100vh - 36px)"></div>
    </div>
</asp:Content>
