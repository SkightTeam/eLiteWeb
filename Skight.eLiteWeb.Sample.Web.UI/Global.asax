﻿<%@ Application Language="C#" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Skight.eLiteWeb.Application.Startup" %>
<%@ Import Namespace="Skight.eLiteWeb.Infrastructure.Persistent" %>
<%@ Import Namespace="Skight.eLiteWeb.Sample.Presentation.Web" %>
<script  RunAt="server">

    private void Application_Start(object sender, EventArgs e)
    {
     
      new ApplicationStartup().run(Assembly.GetAssembly(typeof(Index)));
      SessionProvider.Instance.InitializeMemoryDBForTest();
      
    }

</script>