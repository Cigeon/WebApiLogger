using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLogger.Loggers
{
    public sealed class CustomLogger
    {
        private static readonly CustomLogger _logger = new CustomLogger();

        private CustomLogger()
        {
        }

        public static CustomLogger GetLogger()
        {
            return _logger;
        }

        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
