<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<EPiServer.Find.UI.Models.FindBootstrapperViewModel>" MasterPageFile="Find.Master" %>
<%@ Assembly Name="EPiServer.Shell.UI" %>
<%@ Assembly Name="EPiServer.Find.UI" %>
<%@ Import Namespace="EPiServer.Framework.Serialization" %>
<%@ Import Namespace="EPiServer.Framework.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer.Framework.Web.Resources" %>
<%@ Import Namespace="EPiServer.Find.UI" %>
<%@ Import Namespace="EPiServer.ServiceLocation" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Shell.Navigation" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<%@ Import Namespace="EPiServer.Shell.Navigation.Internal" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%: Model.ViewTitle %></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
    <%=Page.ClientResources("navigation", new[] { ClientResourceType.Style })%>
    <link rel="stylesheet" type="text/css" href="<%=HttpUtility.HtmlEncode(Model.ClientSideResourceBaseUrl)%>epi-find/epi.css" />
    <%= Html.RequiredClientResources("BootstrapperHeader") %>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        //<![CDATA[
        var dojoConfig = {
            baseUrl: "<%=HttpUtility.JavaScriptStringEncode(Model.ClientSideResourceBaseUrl)%>",
            locale: "<%=HttpUtility.JavaScriptStringEncode(Model.LocaleString)%>",
            paths: {
                "epi/shell/ui/nls": "<%=HttpUtility.JavaScriptStringEncode(Model.ClientSideResourceBaseUrl)%>epi/nls",
                "epi/cms/nls": "<%=HttpUtility.JavaScriptStringEncode(Model.ClientSideResourceBaseUrl)%>epi/nls"
            },
            packages: ["dojo", "epi-find"],
            find: {
                token: "<%= HttpUtility.JavaScriptStringEncode(Model.Token) %>",
                serviceApiBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.ServiceApiBaseUrl) %>",
                siteServiceApiBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteServiceApiBaseUrl) %>",
                siteModulesBaseUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteModulesBaseUrl) %>",
                siteNoConnectionUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteNoConnectionUrl) %>",
                indexingPluginUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.IndexingPluginUrl) %>",
                siteConfigUrl: "<%= HttpUtility.JavaScriptStringEncode(Model.SiteConfigUrl) %>"
            },
            rootNode: document.getElementById('applicationContainer')
        };
        //]]>
    </script>
   <%=Page.ClientResources("epi.find.bootstrap", new[] { ClientResourceType.Script })%>
    <script type="text/javascript">
        //<![CDATA[
        require(["epi-find-bootstrap", "dojo/parser", "dojo/domReady!"], function (Bootstrap, parser) {
            var bootstrap = new Bootstrap(document.getElementById('applicationContainer'));
            bootstrap.startup();
            parser.parse();
        });
        //]]>
    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.CreatePlatformNavigationMenu()%>
    <div  <%= Html.ApplyFullscreenPlatformNavigation()%> >
        <div id="applicationContainer" class="epi-applicationContainer" style="height: calc(100vh - 40px)"></div>
    </div>
</asp:Content>
