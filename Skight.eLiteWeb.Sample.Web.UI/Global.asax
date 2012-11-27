<%@ Application Language="C#" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Skight.eLiteWeb.Application.Startup" %>
<script  RunAt="server">

    private void Application_Start(object sender, EventArgs e)
    {
       new ApplicationStartup().run();
    }

</script>