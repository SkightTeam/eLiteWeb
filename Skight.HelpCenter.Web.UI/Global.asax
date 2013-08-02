<%@ Application Language="C#" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Skight.HelpCenter.Presentation" %>
<%@ Import Namespace="Skight.eLiteWeb.Application.Startup" %>
<script  RunAt="server">

    private void Application_Start(object sender, EventArgs e)
    {
      new ApplicationStartup().run(Assembly.GetAssembly(typeof(Index)));
    }

</script>