using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using RickSoft.Config;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupConsoleLogging();

            TestConfig cfg = JsonConfig.Load<TestConfig>("test_config");

            Console.WriteLine(cfg.Name);
            Console.WriteLine(cfg.Test);

            cfg.Test = "test2";
            cfg.Save();

            Console.WriteLine(cfg.Name);
            Console.WriteLine(cfg.Test);

            Console.ReadLine();

            cfg.Reload();
        }

        static void SetupConsoleLogging()
        {
            var t = new ColoredConsoleTarget();
            
	        var r = new LoggingRule("*", LogLevel.Trace, t);
	        LogManager.Configuration = new LoggingConfiguration();
	        LogManager.Configuration.AddTarget("console", t);
	        LogManager.Configuration.LoggingRules.Add(r);

	        LogManager.ReconfigExistingLoggers();
        }
    }
}
