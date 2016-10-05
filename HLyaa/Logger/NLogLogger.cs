using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NLog;

namespace HLyaa.Logger
{
  public class NLogLogger : ILogger
  {

    private NLog.Logger _logger;

    public NLogLogger()
    {
      _logger = LogManager.GetCurrentClassLogger();
    }

    public void Trace(string message)
    {
      _logger.Trace(message);
    }
    public void Info(string message)
    {
      _logger.Info(message);
    }

    public void Warn(string message)
    {
      _logger.Warn(message);
    }

    public void Debug(string message)
    {
      _logger.Debug(message);
    }

    public void Error(string message)
    {
      _logger.Error(message);
    }

    public void Error(Exception x)
    {
      Error(LogUtility.BuildExceptionMessage(x));
    }

    public void Error(string message, Exception x)
    {
      _logger.Error(x, message);
    }

    public void Fatal(string message)
    {
      _logger.Fatal(message);
    }

    public void Fatal(Exception x)
    {
      Fatal(LogUtility.BuildExceptionMessage(x));
    }
  }
}