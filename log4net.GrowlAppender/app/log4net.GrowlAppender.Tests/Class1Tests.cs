using System;
using Xunit;
using System.Threading;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
using System.Linq;

namespace log4net.GrowlAppender {
    public class TestAppender : AppenderSkeleton {
        public event Action<LoggingEvent> AppendCalled = delegate { };
        protected override void Append(LoggingEvent loggingEvent) {
            AppendCalled(loggingEvent);
        }
    }
	public class Class1 {
        private TestAppender _appender = new TestAppender();
        public Class1() {
            //log4net.Util.LogLog.InternalDebugging = true;
            //Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            //Logger rootLogger = hierarchy.Root;
            //rootLogger.Level = Level.All;
            //Logger coreLogger = hierarchy.GetLogger("abc") as Logger;
            //coreLogger.Level = Level.All;

            //coreLogger.Parent = rootLogger;
            //PatternLayout patternLayout = new PatternLayout {
            //    ConversionPattern = "%logger - %message %newline"
            //};
            //patternLayout.ActivateOptions();
            //_appender.Layout = patternLayout;
            //_appender.ActivateOptions();
            //rootLogger.AddAppender(_appender);            
        }
        [Fact(Skip="Just making sure things were called properly")]
        public void Test() {
            //log4net.Config.BasicConfigurator.Configure(_appender);
            bool called = false;
            _appender.AppendCalled += e => called = true;
            var log = LogManager.GetLogger("test");
            log.Debug("This is a debugging message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Info("This is an info message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Warn("This is a warning message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Error("This is an error message");
            Assert.True(called);
        }
        [Fact]
        public void Test2() {
            log4net.Config.BasicConfigurator.Configure(new GrowlAppender());
            var log = LogManager.GetLogger("test");
            log.Debug("This is a debugging message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Info("This is an info message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Warn("This is a warning message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Error("This is an error message");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            log.Fatal("This is a fatal message");
        }
    }
}
