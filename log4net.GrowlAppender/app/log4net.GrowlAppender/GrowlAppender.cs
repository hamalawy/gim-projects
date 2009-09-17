using System;
using log4net.Appender;
using log4net.Core;
using Growl.Connector;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using log4net.GrowlAppender.Properties;

namespace log4net.GrowlAppender {
    public class GrowlAppender : IAppender {
        private Application _application;
        private GrowlConnector _growl;
        readonly IDictionary<Level, Image> _levels = new Dictionary<Level, Image> { 
                {Level.Debug, Icons.Debug}, 
                {Level.Info, Icons.Info}, 
                {Level.Notice, Icons.Info}, 
                {Level.Warn, Icons.Warn}, 
                {Level.Error, Icons.Error}, 
                {Level.Severe, Icons.Fatal}, 
                {Level.Critical, Icons.Fatal}, 
                {Level.Fatal, Icons.Fatal}, 
                {Level.Emergency, Icons.Fatal},  
            };
        public GrowlAppender() {
            Name = "GrowlAppender";
            _growl = new GrowlConnector();
            _application = new Application((Assembly.GetEntryAssembly()??Assembly.GetExecutingAssembly()).GetName().Name);
            
            var notificationTypes = _levels.Select(kv => new NotificationType(kv.Key.Name, kv.Key.DisplayName) { Icon = kv.Value }).ToArray();

            _growl.Register(_application, notificationTypes);
        }
        public string Name { get; set; }

        public void Close() { }

        public void DoAppend(LoggingEvent loggingEvent) {
            var sw = new StringWriter();
            loggingEvent.WriteRenderedMessage(sw);
            _growl.Notify(
                new Notification(_application.Name, loggingEvent.Level.Name, 
                    Guid.NewGuid().ToString(), loggingEvent.Level.DisplayName, sw.ToString()));
        }

    }
}
