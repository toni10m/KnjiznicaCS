using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Knjiznica_AB
{
    class MyLogger
    {
        private ILog Log { get; set; }
        private string className = "<- DEFAULT CLASS ->";
        public class typeOfLogger
        {
            public const string MAIN = "main";
            public const string DB = "database";

        }
        public MyLogger(string aTypeOfLogger)
        {
            initLogger(aTypeOfLogger);
        }
        public MyLogger(string aTypeOfLogger, string aClassName)
        {
            initLogger(aTypeOfLogger);
            className = aClassName;
        }
        private void initLogger(string aTypeOfLogger)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(aTypeOfLogger);
        }
        private string MakeMessage(string message)
        {
            return className + " - " + message;
        }
        public void Debug(string message)
        {
            Log.Debug(MakeMessage(message));
        }
        public void Debug(string message, Exception exception)
        {
            Log.Debug(MakeMessage(message), exception);
        }
        public void Info(string message)
        {
            Log.Info(MakeMessage(message));
        }
        public void Info(string message, Exception exception)
        {
            Log.Info(MakeMessage(message), exception);
        }
        public void Warning(string message)
        {
            Log.Warn(MakeMessage(message));
        }
        public void Warning(string message, Exception exception)
        {
            Log.Warn(MakeMessage(message), exception);

        }
        public void Error(string message)
        {
            Log.Error(MakeMessage(message));
        }
        public void Error(string message, Exception exception)
        {
            Log.Error(MakeMessage(message), exception);

        }
        public void Fatal(string message)
        {
            Log.Fatal(MakeMessage(message));
        }
        public void Fatal(string message, Exception exception)
        {
            Log.Fatal(MakeMessage(message), exception);

        }
    }
}
