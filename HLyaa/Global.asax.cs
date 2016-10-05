using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using HLyaa.Models;
using HLyaa.Logger;

namespace HLyaa
{
  public class MvcApplication : System.Web.HttpApplication
  {
    private static NLogLogger logger = new NLogLogger();
    protected void Application_Start()
    {
      
      logger.Info("=============================================================");
      logger.Trace("trace log message");
      logger.Debug("debug log message");
      logger.Info("info log message");
      logger.Warn("warn log message");
      logger.Error("error log message");
      logger.Fatal("fatal log message");

      try
      {
        throw new Exception("A test exception");
      }
      catch (Exception ex)
      {
        logger.Error("An error has occurred", ex);
      }
      Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());

      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    override public void Init() 
    {
      logger.Info("Application Init");
    }

    override public void Dispose()
    {
      logger.Info("Application Dispose");
    }

    protected void Application_Error()
    {
      logger.Info("Application Error");
      logger.Info("----------------------------------");
    }


    protected void Application_End()
    {
      logger.Info("Application End");
      logger.Info("/////////////////////////////////////////////////////////////");
    }
  }
}
