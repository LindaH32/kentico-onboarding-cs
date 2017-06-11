﻿using System.Web;
using System.Web.Http;

namespace TodoList.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(ResponseConfig.Register);
            GlobalConfiguration.Configure(UnityResolverConfig.Register);
        }
    }
}